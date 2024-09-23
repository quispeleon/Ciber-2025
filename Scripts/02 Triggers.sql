
DROP  TRIGGER IF EXISTS after_insert_alquiler $$
CREATE TRIGGER after_insert_alquiler AFTER INSERT ON Alquiler
FOR EACH ROW
BEGIN

	DECLARE end_time DATETIME;
	DECLARE total_time TIME;
	
    DECLARE total_cost DECIMAL(10,2);

    -- Calcular el tiempo final basado en la cantidad de tiempo especificada
    SET end_time = NOW() + INTERVAL TIME_TO_SEC(NEW.cantidadTiempo) SECOND;
    
    -- Calcular el costo total (8.33 pesos por minuto)
    SET total_cost = TIME_TO_SEC(NEW.cantidadTiempo) / 60 * 8.33;
    
    update maquina 
    set estado = false
    where Nmaquina = new.Nmaquina;
   
    if new.tipo = 2 then
     if new.tipo = 2 and new.pagado = true  then
			INSERT INTO historialdealquiler(Ncuenta, Nmaquina, fechaInicio,fechaFin, TotalPagar)
				VALUES (NEW.Ncuenta,NEW.Nmaquina,NOW(),end_time,total_cost);
		else
			signal sqlstate '45000'
			set MESSAGE_TEXT = 'Primero debes pagar para alquilar';
		end if ;
    end if;
    IF NEW.tipo = 1 THEN
   	 INSERT INTO HistorialdeAlquiler (Ncuenta, Nmaquina, fechaInicio, TotalPagar)
   	 VALUES (NEW.Ncuenta, NEW.Nmaquina, NOW(),0);
     
    END IF;
    
    
    
    
    
END; $$
DROP TRIGGER IF EXISTS before_insert_alquiler $$
CREATE TRIGGER before_insert_alquiler BEFORE INSERT ON Alquiler
FOR EACH ROW
BEGIN
	declare validar bool;
    
    
    select estado into validar 
    from maquina
    where Nmaquina = new.Nmaquina;
    
    if exists(select Ncuenta 
		from alquiler 
        where Ncuenta = NEW.Ncuenta) then
        signal sqlstate '45000'
        set MESSAGE_TEXT = 'El usuario este ya esta usando otra maquina';
        
	end if;
    
    if validar = FALSE then
		signal sqlstate '45000'
        set MESSAGE_TEXT = 'La maquina esta ocupado';
    end if;
    
    
    if new.tipo = 1 then
   	 if new.cantidadTiempo > 0 then
   	 
   		 signal sqlstate '45000'
   		 set MESSAGE_TEXT = 'SI tienes un costo debes poner la opcion Hora definida tipo 2';
       	 
   	 end if;
    	if new.pagado = true then
   		 signal sqlstate '45000'
        	set MESSAGE_TEXT = 'Como pagaste si es libre animal';
   	 end if ;
	end if ;
    
    
    
    
    
END $$



DROP TRIGGER IF EXISTS afterupadte_maquina $$
CREATE TRIGGER afterupadte_maquina  after update ON Maquina
for each row
begin
  DECLARE start_time DATETIME;
    DECLARE end_time DATETIME;
    DECLARE total_time INT;
    DECLARE total_cost DECIMAL(10,2);

    IF NEW.estado = TRUE THEN
        SELECT fechaInicio INTO start_time 
        FROM HistorialdeAlquiler 
        WHERE Nmaquina = NEW.Nmaquina AND fechaFin IS NULL
        ORDER BY fechaInicio DESC 
        LIMIT 1;

        SET end_time = NOW();
        SET total_time = TIMESTAMPDIFF(SECOND, start_time, end_time);
        SET total_cost = total_time / 60 * 8.33;
        UPDATE HistorialdeAlquiler 
        SET fechaFin = end_time,  TotalPagar = total_cost
        WHERE Nmaquina = NEW.Nmaquina AND fechaFin IS NULL
        ORDER BY fechaInicio DESC 
        LIMIT 1;
    END IF;
end $$