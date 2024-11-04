CREATE USER IF NOT EXISTS 'Administrador'@'%' IDENTIFIED BY 'Admin!123';
CREATE USER IF NOT EXISTS 'Tesorero'@'localhost' IDENTIFIED BY 'Gerente!123';


GRANT ALL ON 5to_Ciber.* to 'Administrador'@'%';
GRANT SELECT ,DELETE ,UPDATE ON 5to_Ciber.Cuenta to 'Tesorero'@'localhost';
GRANT SELECT, DELETE, UPDATE ON 5to_Ciber.ALquiler to 'Tesorero'@'localhost';
GRANT SELECT, DELETE, UPDATE ON 5to_Ciber.Maquina to 'Tesorero'@'localhost';

CREATE USER IF NOT EXISTS 'administrador'@'%' IDENTIFIED BY 'Admin!123';
CREATE USER IF NOT EXISTS 'tesorero'@'localhost' IDENTIFIED BY 'Gerente!123';



FLUSH PRIVILEGES;