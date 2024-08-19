
namespace Ciber.core;

public class Cuenta
{
    public int id {get;set;}
    public string Nombre {get;set}
    public string pass {get;set;}
    public int dni {get;set;}
    public Datetime Horaregistrada {get;set}
    public List<HistorialAlquiler> historial {get;set;}

}
