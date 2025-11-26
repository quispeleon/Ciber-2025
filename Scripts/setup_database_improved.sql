-- Script mejorado para crear la base de datos del Sistema Ciber
-- Con lógica de precios y cálculos reales

DROP DATABASE IF EXISTS 5to_Ciber;
CREATE DATABASE 5to_Ciber CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE 5to_Ciber;

-- Tabla Cuenta
CREATE TABLE Cuenta (
    Ncuenta INT AUTO_INCREMENT,
    nombre VARCHAR(45) CHARACTER SET utf8mb4, 
    pass CHAR(64),
    dni INT,
    horaRegistrada TIME,
    PRIMARY KEY (Ncuenta)
);

-- Tabla Maquina (MEJORADA con precios)
CREATE TABLE Maquina (
    Nmaquina INT AUTO_INCREMENT,
    estado BOOL,
    caracteristicas VARCHAR(100),
    precioPorHora DECIMAL(10,2) DEFAULT 5.00,
    tipoMaquina VARCHAR(20) DEFAULT 'Estándar',
    PRIMARY KEY (Nmaquina)
);

-- Tabla Tipo
CREATE TABLE Tipo (
    idTipo INT AUTO_INCREMENT,
    tipo VARCHAR(45),
    PRIMARY KEY (idTipo)
);

-- Tabla Alquiler (MEJORADA con fechas y precios)
CREATE TABLE Alquiler (
    idAlquiler INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    tipo INT,
    cantidadTiempo TIME,
    pagado BOOL NULL,
    fechaInicio DATETIME DEFAULT CURRENT_TIMESTAMP,
    fechaFin DATETIME NULL,
    precioPorHora DECIMAL(10,2),
    totalAPagar DECIMAL(10,2) NULL,
    montoPagado DECIMAL(10,2) NULL,
    PRIMARY KEY (idAlquiler),
    CONSTRAINT FK_Reservacion_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta),
    CONSTRAINT FK_Reservacion_Tipo FOREIGN KEY (tipo) REFERENCES Tipo (idTipo),
    CONSTRAINT FK_Reservacion_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina)
);

-- Tabla HistorialdeAlquiler (MEJORADA con montos pagados)
CREATE TABLE HistorialdeAlquiler (
    idHistorial INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    fechaInicio DATETIME,
    fechaFin DATETIME,
    totalPagar DECIMAL(10,2),
    montoPagado DECIMAL(10,2),
    precioPorHora DECIMAL(10,2),
    PRIMARY KEY (idHistorial),
    CONSTRAINT FK_HistorialdeAlquiler_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina),
    CONSTRAINT FK_HistorialdeAlquiler_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta)
);

-- Insertar datos iniciales para Tipo
INSERT INTO Tipo (tipo) VALUES ('Libre'), ('Hora Definida');

-- Insertar cuentas de ejemplo
INSERT INTO Cuenta (nombre, pass, dni, horaRegistrada) VALUES 
('Juan Pérez', 'password123', 12345678, '08:00:00'),
('María García', 'password456', 87654321, '09:30:00'),
('Carlos López', 'password789', 11223344, '10:15:00'),
('Ana Martínez', 'password111', 55667788, '11:00:00');

-- Insertar máquinas con diferentes precios
INSERT INTO Maquina (estado, caracteristicas, precioPorHora, tipoMaquina) VALUES 
(TRUE, 'Intel i7, 16GB RAM, GTX 1660', 4.50, 'Estándar'),
(TRUE, 'AMD Ryzen 5, 8GB RAM, RTX 3060', 6.00, 'Gaming'),
(FALSE, 'Intel i5, 8GB RAM, GTX 1050', 3.50, 'Básica'),
(TRUE, 'AMD Ryzen 7, 32GB RAM, RTX 4070', 8.00, 'Premium'),
(TRUE, 'Intel i9, 64GB RAM, RTX 4080', 10.00, 'Elite');

-- Insertar algunos alquileres de ejemplo
INSERT INTO Alquiler (Ncuenta, Nmaquina, tipo, cantidadTiempo, pagado, fechaInicio, precioPorHora, totalAPagar, montoPagado) VALUES 
(1, 1, 1, NULL, TRUE, '2024-01-15 10:00:00', 4.50, 9.00, 9.00),
(2, 2, 2, '02:00:00', TRUE, '2024-01-15 14:30:00', 6.00, 12.00, 12.00),
(3, 4, 1, NULL, FALSE, '2024-01-15 16:00:00', 8.00, NULL, NULL);

-- Insertar historial de ejemplo
INSERT INTO HistorialdeAlquiler (Ncuenta, Nmaquina, fechaInicio, fechaFin, totalPagar, montoPagado, precioPorHora) VALUES 
(1, 1, '2024-01-14 09:00:00', '2024-01-14 11:30:00', 11.25, 11.25, 4.50),
(2, 2, '2024-01-14 15:00:00', '2024-01-14 17:00:00', 12.00, 12.00, 6.00),
(3, 3, '2024-01-14 19:00:00', '2024-01-14 21:15:00', 7.00, 5.00, 3.50);

-- Verificar que las tablas se crearon correctamente
SHOW TABLES;

-- Mostrar estructura de las tablas principales
DESCRIBE Maquina;
DESCRIBE Alquiler;
DESCRIBE HistorialdeAlquiler;
