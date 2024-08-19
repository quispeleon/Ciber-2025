namespace Ciber.core;

public class Maquina
{
    public int id {get;set;}
    public bool estado {get;set;}
    public string Caracteristicas {get;set;}
    public List<Alquiler> alquileres {get;set;}
}
