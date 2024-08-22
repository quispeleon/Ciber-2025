namespace Ciber.core;

public  interface IADO
{
    void AltaMaquina(Maquina maquina);
    List<Maquina> ObtenerMaquinas();
    void AltaCuenta(Cuenta cuenta);
    List<Cuenta> cuentas(); 
}
