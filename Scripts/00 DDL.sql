-- Eliminar y recrear la base de datos con mejoras
DROP DATABASE IF EXISTS 5to_Ciber;
CREATE DATABASE 5to_Ciber CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE 5to_Ciber;

-- Tabla Cuenta (MEJORADA)
CREATE TABLE Cuenta (
    Ncuenta INT AUTO_INCREMENT,
    nombre VARCHAR(45) CHARACTER SET utf8mb4,
    pass CHAR(64),
    dni VARCHAR(15),
    horaRegistrada TIME,
    saldo DECIMAL(10,2) DEFAULT 0.00,
    activa BOOL DEFAULT TRUE,
    PRIMARY KEY (Ncuenta),
    UNIQUE KEY uk_dni (dni)
);

-- Tabla Maquina (MEJORADA)
CREATE TABLE Maquina (
    Nmaquina INT AUTO_INCREMENT,
    estado ENUM('Disponible', 'Ocupada', 'Mantenimiento', 'Reservada') DEFAULT 'Disponible',
    caracteristicas VARCHAR(100),
    precioPorHora DECIMAL(10,2) DEFAULT 5.00,
    tipoMaquina VARCHAR(20) DEFAULT 'Estándar',
    ultimoMantenimiento DATE,
    proximoMantenimiento DATE,
    PRIMARY KEY (Nmaquina)
);

-- Tabla Tipo (MEJORADA)
CREATE TABLE Tipo (
    idTipo INT AUTO_INCREMENT,
    tipo VARCHAR(45),
    descripcion TEXT,
    requierePagoPrevio BOOL DEFAULT FALSE,
    PRIMARY KEY (idTipo)
);

-- Tabla Alquiler (MEJORADA)
CREATE TABLE Alquiler (
    idAlquiler INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    tipo INT,
    fechaInicio DATETIME DEFAULT CURRENT_TIMESTAMP,
    fechaFin DATETIME NULL,
    tiempoContratado TIME NULL,
    tiempoUsado TIME NULL,
    precioPorHora DECIMAL(10,2),
    totalAPagar DECIMAL(10,2) DEFAULT 0.00,
    montoPagado DECIMAL(10,2) DEFAULT 0.00,
    estado ENUM('Activo', 'Finalizado', 'Cancelado', 'Pendiente Pago') DEFAULT 'Activo',
    sesionActiva BOOL DEFAULT TRUE,
    PRIMARY KEY (idAlquiler),
    CONSTRAINT FK_Alquiler_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta),
    CONSTRAINT FK_Alquiler_Tipo FOREIGN KEY (tipo) REFERENCES Tipo (idTipo),
    CONSTRAINT FK_Alquiler_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina),
    INDEX idx_sesion_activa (sesionActiva),
    INDEX idx_maquina_estado (Nmaquina, estado)
);

-- Tabla HistorialdeAlquiler (MEJORADA)
CREATE TABLE HistorialdeAlquiler (
    idHistorial INT AUTO_INCREMENT,
    idAlquiler INT,
    Ncuenta INT,
    Nmaquina INT,
    fechaInicio DATETIME,
    fechaFin DATETIME,
    tiempoContratado TIME,
    tiempoUsado TIME,
    precioPorHora DECIMAL(10,2),
    TotalPagar DECIMAL(10,2),
    montoPagado DECIMAL(10,2),
    estadoFinal VARCHAR(50),
    observaciones TEXT,
    PRIMARY KEY (idHistorial),
    CONSTRAINT FK_Historial_Alquiler FOREIGN KEY (idAlquiler) REFERENCES Alquiler (idAlquiler),
    CONSTRAINT FK_Historial_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina),
    CONSTRAINT FK_Historial_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta)
);

-- Tabla de Transacciones (NUEVA)
CREATE TABLE Transacciones (
    idTransaccion INT AUTO_INCREMENT,
    Ncuenta INT,
    tipo ENUM('Recarga', 'Consumo', 'Devolucion'),
    monto DECIMAL(10,2),
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    idAlquiler INT NULL,
    descripcion TEXT,
    PRIMARY KEY (idTransaccion),
    CONSTRAINT FK_Transacciones_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta),
    CONSTRAINT FK_Transacciones_Alquiler FOREIGN KEY (idAlquiler) REFERENCES Alquiler (idAlquiler)
);

-- Insertar tipos mejorados
INSERT INTO Tipo (tipo, descripcion, requierePagoPrevio) VALUES 
('Libre', 'Uso libre hasta agotar saldo', FALSE),
('Hora Definida', 'Tiempo específico contratado', TRUE),
('Reserva', 'Reserva anticipada', TRUE);

-- PROCEDURES MEJORADOS
DELIMITER $$

DROP PROCEDURE IF EXISTS RegistrarCuenta$$
CREATE PROCEDURE RegistrarCuenta(
    OUT uNcuenta INT,
    IN unnombre VARCHAR(45),
    IN unPas VARCHAR(128),
    IN unDni VARCHAR(15),
    IN saldoInicial DECIMAL(10,2)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    IF EXISTS (SELECT 1 FROM Cuenta WHERE dni = unDni) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El DNI ya está registrado';
    END IF;
    
    INSERT INTO Cuenta(nombre, pass, dni, horaRegistrada, saldo) 
    VALUES (unnombre, SHA2(unPas, 256), unDni, CURTIME(), saldoInicial);
    
    SET uNcuenta = LAST_INSERT_ID();
    
    IF saldoInicial > 0 THEN
        INSERT INTO Transacciones(Ncuenta, tipo, monto, descripcion)
        VALUES (uNcuenta, 'Recarga', saldoInicial, 'Saldo inicial al registrar cuenta');
    END IF;
    
    COMMIT;
END $$

DROP PROCEDURE IF EXISTS RecargarSaldo$$
CREATE PROCEDURE RecargarSaldo(
    IN unNcuenta INT,
    IN monto DECIMAL(10,2)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    IF NOT EXISTS (SELECT 1 FROM Cuenta WHERE Ncuenta = unNcuenta AND activa = TRUE) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Cuenta no existe o está inactiva';
    END IF;
    
    IF monto <= 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El monto debe ser mayor a 0';
    END IF;
    
    UPDATE Cuenta SET saldo = saldo + monto WHERE Ncuenta = unNcuenta;
    
    INSERT INTO Transacciones(Ncuenta, tipo, monto, descripcion)
    VALUES (unNcuenta, 'Recarga', monto, CONCAT('Recarga de saldo: $', monto));
    
    COMMIT;
END $$

DROP PROCEDURE IF EXISTS IniciarAlquiler$$
CREATE PROCEDURE IniciarAlquiler(
    IN unNcuenta INT,
    IN unNmaquina INT,
    IN unTipo INT,
    IN tiempoContratado TIME,
    OUT idAlquilerCreado INT
)
BEGIN
    DECLARE v_saldo DECIMAL(10,2);
    DECLARE v_precio_hora DECIMAL(10,2);
    DECLARE v_costo_estimado DECIMAL(10,2);
    DECLARE v_requiere_pago_previo BOOL;
    DECLARE v_estado_maquina VARCHAR(20);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    IF EXISTS (SELECT 1 FROM Alquiler WHERE Ncuenta = unNcuenta AND sesionActiva = TRUE) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El usuario ya tiene una sesión activa';
    END IF;
    
    SELECT estado, precioPorHora INTO v_estado_maquina, v_precio_hora 
    FROM Maquina WHERE Nmaquina = unNmaquina;
    
    IF v_estado_maquina != 'Disponible' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La máquina no está disponible';
    END IF;
    
    SELECT requierePagoPrevio INTO v_requiere_pago_previo 
    FROM Tipo WHERE idTipo = unTipo;
    
    SELECT saldo INTO v_saldo FROM Cuenta WHERE Ncuenta = unNcuenta;
    
    IF unTipo = 2 AND tiempoContratado IS NOT NULL THEN
        SET v_costo_estimado = (TIME_TO_SEC(tiempoContratado) / 3600) * v_precio_hora;
        
        IF v_requiere_pago_previo AND v_saldo < v_costo_estimado THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Saldo insuficiente para el tiempo contratado';
        END IF;
    END IF;
    
    IF unTipo = 1 AND v_saldo <= 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Saldo insuficiente para uso libre';
    END IF;
    
    INSERT INTO Alquiler (Ncuenta, Nmaquina, tipo, tiempoContratado, precioPorHora, estado, sesionActiva)
    VALUES (unNcuenta, unNmaquina, unTipo, tiempoContratado, v_precio_hora, 'Activo', TRUE);
    
    SET idAlquilerCreado = LAST_INSERT_ID();
    
    UPDATE Maquina SET estado = 'Ocupada' WHERE Nmaquina = unNmaquina;
    
    COMMIT;
END $$

DROP PROCEDURE IF EXISTS FinalizarAlquiler$$
CREATE PROCEDURE FinalizarAlquiler(
    IN unIdAlquiler INT
)
BEGIN
    DECLARE v_ncuenta INT;
    DECLARE v_nmaquina INT;
    DECLARE v_fecha_inicio DATETIME;
    DECLARE v_precio_hora DECIMAL(10,2);
    DECLARE v_tiempo_contratado TIME;
    DECLARE v_tipo INT;
    DECLARE v_costo_total DECIMAL(10,2);
    DECLARE v_tiempo_usado TIME;
    DECLARE v_saldo_actual DECIMAL(10,2);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    SELECT Ncuenta, Nmaquina, fechaInicio, precioPorHora, tiempoContratado, tipo
    INTO v_ncuenta, v_nmaquina, v_fecha_inicio, v_precio_hora, v_tiempo_contratado, v_tipo
    FROM Alquiler 
    WHERE idAlquiler = unIdAlquiler AND sesionActiva = TRUE;
    
    IF v_ncuenta IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Alquiler no encontrado o ya finalizado';
    END IF;
    
    SET v_tiempo_usado = TIMEDIFF(NOW(), v_fecha_inicio);
    SET v_costo_total = (TIME_TO_SEC(v_tiempo_usado) / 3600) * v_precio_hora;
    
    SELECT saldo INTO v_saldo_actual FROM Cuenta WHERE Ncuenta = v_ncuenta;
    
    IF v_tipo = 1 AND v_saldo_actual < v_costo_total THEN
        SET v_costo_total = v_saldo_actual;
        UPDATE Cuenta SET saldo = 0 WHERE Ncuenta = v_ncuenta;
    ELSE
        UPDATE Cuenta SET saldo = saldo - v_costo_total WHERE Ncuenta = v_ncuenta;
    END IF;
    
    UPDATE Alquiler 
    SET fechaFin = NOW(),
        tiempoUsado = v_tiempo_usado,
        totalAPagar = v_costo_total,
        montoPagado = v_costo_total,
        estado = 'Finalizado',
        sesionActiva = FALSE
    WHERE idAlquiler = unIdAlquiler;
    
    UPDATE Maquina SET estado = 'Disponible' WHERE Nmaquina = v_nmaquina;
    
    INSERT INTO HistorialdeAlquiler (
        idAlquiler, Ncuenta, Nmaquina, fechaInicio, fechaFin, 
        tiempoContratado, tiempoUsado, precioPorHora, TotalPagar, 
        montoPagado, estadoFinal
    )
    SELECT idAlquiler, Ncuenta, Nmaquina, fechaInicio, fechaFin,
           tiempoContratado, tiempoUsado, precioPorHora, totalAPagar,
           montoPagado, estado
    FROM Alquiler 
    WHERE idAlquiler = unIdAlquiler;
    
    INSERT INTO Transacciones (Ncuenta, tipo, monto, idAlquiler, descripcion)
    VALUES (v_ncuenta, 'Consumo', v_costo_total, unIdAlquiler, 
            CONCAT('Uso de máquina: ', v_tiempo_usado));
    
    COMMIT;
END $$

DROP FUNCTION IF EXISTS cantMaquinasLibres$$
CREATE FUNCTION cantMaquinasLibres() RETURNS INT READS SQL DATA
BEGIN
    DECLARE cant INT;
    SELECT COUNT(*) INTO cant FROM Maquina WHERE estado = 'Disponible';
    RETURN cant;
END $$

DROP FUNCTION IF EXISTS CalcularCosto$$
CREATE FUNCTION CalcularCosto(
    p_precio_hora DECIMAL(10,2),
    p_segundos_usados INT
) RETURNS DECIMAL(10,2) DETERMINISTIC
BEGIN
    RETURN (p_segundos_usados / 3600) * p_precio_hora;
END $$

DROP TRIGGER IF EXISTS before_insert_alquiler$$
CREATE TRIGGER before_insert_alquiler 
BEFORE INSERT ON Alquiler 
FOR EACH ROW
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Maquina WHERE Nmaquina = NEW.Nmaquina AND estado = 'Disponible') THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La máquina no está disponible para alquiler';
    END IF;
    
    IF EXISTS (SELECT 1 FROM Alquiler WHERE Ncuenta = NEW.Ncuenta AND sesionActiva = TRUE) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El usuario ya tiene una sesión activa';
    END IF;
    
    IF NEW.tipo = 1 THEN
        IF NEW.tiempoContratado IS NOT NULL THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Tipo Libre no requiere tiempo contratado';
        END IF;
    ELSEIF NEW.tipo = 2 THEN
        IF NEW.tiempoContratado IS NULL THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Tipo Hora Definida requiere tiempo contratado';
        END IF;
    END IF;
END $$



DELIMITER ;

-- DATOS DE EJEMPLO
INSERT INTO Maquina (estado, caracteristicas, precioPorHora, tipoMaquina) VALUES
('Disponible', 'Intel i7, 16GB RAM, RTX 3060', 8.00, 'Gaming'),
('Disponible', 'Intel i5, 8GB RAM, GTX 1650', 6.00, 'Estándar'),
('Disponible', 'Intel i3, 4GB RAM, Integrado', 4.00, 'Básico'),
('Mantenimiento', 'Intel i9, 32GB RAM, RTX 4080', 12.00, 'Premium');

CALL RegistrarCuenta(@cuenta1, 'Juan Pérez', 'password123', '12345678', 50.00);
CALL RegistrarCuenta(@cuenta2, 'María García', 'securepass', '87654321', 25.00);