using System.Text.Json.Serialization;
namespace Ciber.core;

public class Cuenta
{
    public int Ncuenta { get; set; } 
    public string Nombre { get; set; }  
    [JsonIgnore]
    public string Pass { get; set; }
    public int Dni { get; set; }
    public TimeSpan HoraRegistrada { get; set; }
}
