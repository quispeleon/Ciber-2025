DROP DATABASE IF EXISTS 5to_Ciber;
CREATE DATABASE 5to_Ciber;
USE 5to_Ciber;


-- Tabla Cuenta
CREATE TABLE Cuenta (
    Ncuenta INT AUTO_INCREMENT,
    nombre VARCHAR(45),
	pass CHAR(64),
    dni INT,
    horaRegistrada TIME,
    PRIMARY KEY (Ncuenta)
);

-- Tabla Maquina
CREATE TABLE Maquina (
    Nmaquina INT AUTO_INCREMENT,
    estado BOOL ,
    caracteristicas VARCHAR(45),
    PRIMARY KEY (Nmaquina)
);

-- Tabla Tipo
CREATE TABLE Tipo (
    idTipo INT AUTO_INCREMENT,
    tipo VARCHAR(45),
    PRIMARY KEY (idTipo)
);

-- Tabla ALquiler
CREATE TABLE Alquiler (
    idAlquiler INT AUTO_INCREMENT,
    Ncuenta INT,
	Nmaquina INT,
    tipo INT,
    cantidadTiempo TIME, -- si elije la opcion 2
    pagado BOOL NULL , -- Columna para estado de pago
    PRIMARY KEY (idAlquiler),
    CONSTRAINT FK_Reservacion_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta),
    CONSTRAINT FK_Reservacion_Tipo FOREIGN KEY (tipo) REFERENCES Tipo (idTipo),
	constraint FK_Reservacion_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina)
);

-- Tabla HistorialdeAlquiler
CREATE TABLE HistorialdeAlquiler (
    idHistorial INT AUTO_INCREMENT,
    Ncuenta INT,
    Nmaquina INT,
    fechaInicio DATETIME,
    fechaFin DATETIME,
	TotalPagar decimal,
    PRIMARY KEY (idHistorial),
    CONSTRAINT FK_HistorialdeAlquiler_Maquina FOREIGN KEY (Nmaquina) REFERENCES Maquina (Nmaquina),
	constraint FK_HistorialdeAlquiler_Cuenta FOREIGN KEY (Ncuenta) REFERENCES Cuenta (Ncuenta)
);




-- Los datos para el tipo si es libre o una hora ya definida IMPORTANTEEEEEE
INSERT INTO Tipo (tipo) VALUES ('Libre');
INSERT INTO Tipo (tipo) VALUES ('Hora Definida');