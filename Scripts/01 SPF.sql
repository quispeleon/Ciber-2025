
-- PROCEDURES


DELIMITER $$
DROP PROCEDURE IF EXISTS Maquinas$$
CREATE PROCEDURE Maquinas(
 OUT uNmaquina INT ,
IN unestado VARCHAR(45),
IN UnaCaracteristicas VARCHAR(45)
)
BEGIN


	INSERT INTO Maquina( estado , caracteristicas)
	VALUES(unestado, UnaCaracteristicas);
	SET uNmaquina = last_insert_id(); 	


END  $$


DROP PROCEDURE IF EXISTS Cuentas $$
CREATE PROCEDURE Cuentas(OUT uNcuenta INT,IN unnombre VARCHAR(45), IN unPas CHAR(64), IN dni INT , IN hora TIME)
BEGIN

	INSERT INTO Cuenta(nombre,pass,dni,horaRegistrada)
  	  VALUES(unnombre,sha2(unPas,256),dni,hora);
	set uNcuenta = last_insert_id();
	 
END $$


DROP PROCEDURE IF EXISTS alquilarMaquina2 $$
CREATE PROCEDURE alquilarMaquina2( IN unNcuenta INT ,
  							  IN unNmaquina INT,
                       		 IN tcantidad TIME,
                       		 IN pagadood bool,
                      		 OUT nIdAlquiler INT)
                      		 
BEGIN
 	INSERT INTO Alquiler(Ncuenta, Nmaquina, tipo, cantidadTiempo, pagado)
    VALUES (unNcuenta, unNmaquina, 2, tcantidad, pagadood);

    SET nIdAlquiler = LAST_INSERT_ID();
END $$

DROP PROCEDURE IF EXISTS alquilarMaquina1 $$
CREATE PROCEDURE alquilarMaquina1(IN unNcuenta INT ,
  							  IN unNmaquina INT)
                      		 
                      		 
BEGIN
	INSERT INTO Alquiler(Ncuenta,Nmaquina,tipo,cantidadTiempo,pagado)
  	  values (unNcuenta,unNmaquina,1,0,false);

END $$

DROP PROCEDURE IF EXISTS salirMaquina $$
CREATE PROCEDURE  salirMaquina(IN unmaqui INT)
BEGIN
	update maquina set estado = true where Nmaquina = unmaqui;
END $$

DROP function IF EXISTS cantMaquinasLibres $$
create function cantMaquinasLibres(unestado tinyint)
returns int reads sql data
BEGIN
	Declare cantMaquinasLibres INT;
	Select count(*) into cantMaquinasLibres
	from Maquina
	where estado = unestado;
	RETURN cantMaquinasLibres;
end $$
