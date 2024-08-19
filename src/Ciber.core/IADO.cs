namespace Ciber.core;

public  interface IADO
{
    void AltaMaquina(Maquina maquina);
    void AltaCuenta(Cuenta cuenta);
    List<Cuenta> cuentas(); 
}
