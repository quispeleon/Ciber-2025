-- Script para crear la base de datos y tablas del Sistema Ciber
-- Ejecutar este script en MySQL para resolver el error de tabla faltante

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

-- Tabla Maquina
CREATE TABLE Maquina (
    Nmaquina INT AUTO_INCREMENT,
    estado BOOL,
    caracteristicas VARCHAR(45),
    PRIMARY KEY (Nmaquina)
);

-- Tabla Tipo
CREATE TABLE Tipo (
    idTipo INT AUTO_INCREMENT,
    tipo VARCHAR(45),
    PRIMARY KEY (idTipo)
);

-- Tabla Alquiler
CREATE TABLE Alquiler (
    idAlquiler INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    tipo INT,
    cantidadTiempo TIME, -- si elije la opcion 2
    pagado BOOL NULL, -- Columna para estado de pago
    PRIMARY KEY (idAlquiler),
    CONSTRAINT FK_Reservacion_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta),
    CONSTRAINT FK_Reservacion_Tipo FOREIGN KEY (tipo) REFERENCES Tipo (idTipo),
    CONSTRAINT FK_Reservacion_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina)
);

-- Tabla HistorialdeAlquiler
CREATE TABLE HistorialdeAlquiler (
    idHistorial INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    fechaInicio DATETIME,
    fechaFin DATETIME,
    TotalPagar DECIMAL(10,2),
    PRIMARY KEY (idHistorial),
    CONSTRAINT FK_HistorialdeAlquiler_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina),
    CONSTRAINT FK_HistorialdeAlquiler_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta)
);

-- Insertar datos iniciales para Tipo
INSERT INTO Tipo (tipo) VALUES ('Libre');
INSERT INTO Tipo (tipo) VALUES ('Hora Definida');

-- Insertar algunos datos de ejemplo para probar
INSERT INTO Cuenta (nombre, pass, dni, horaRegistrada) VALUES 
('Juan Pérez', 'password123', 12345678, '08:00:00'),
('María García', 'password456', 87654321, '09:30:00'),
('Carlos López', 'password789', 11223344, '10:15:00');

INSERT INTO Maquina (estado, caracteristicas) VALUES 
(TRUE, 'Intel i7, 16GB RAM, GTX 1660'),
(TRUE, 'AMD Ryzen 5, 8GB RAM, RTX 3060'),
(FALSE, 'Intel i5, 8GB RAM, GTX 1050'),
(TRUE, 'AMD Ryzen 7, 32GB RAM, RTX 4070');

-- Verificar que las tablas se crearon correctamente
SHOW TABLES;

-- Mostrar estructura de la tabla HistorialdeAlquiler
DESCRIBE HistorialdeAlquiler;
