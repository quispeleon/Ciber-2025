using Ciber.core;
using Ciber.Dapper;


string connectionString = "Server=localhost;Database=Ciber;User=5to_agbd;Password=Trigg3rs!;";

// las instancias
IDAO cuentaRepo = new CuentaRepository(connectionString);
// IMaquinaRepository maquinaRepo = new MaquinaRepository(connectionString);
// ITipoRepository tipoRepo = new TipoRepository(connectionString);
// IAlquilerRepository alquilerRepo = new AlquilerRepository(connectionString);
// IHistorialdeAlquilerRepository historialRepo = new HistorialdeAlquilerRepository(connectionString);

// agregar una nueva cuenta
var nuevaCuenta = new Cuenta
{
    Nombre = "carlosss",
    Pass = "sofkla",
    Dni = 1748423,
    HoraRegistrada = DateTime.Now.TimeOfDay
};
cuentaRepo.AgregarCuenta(nuevaCuenta);

var cuentas = cuentaRepo.ObtenerTodasLasCuentas();
foreach (var cuenta in cuentas)
{
    Console.WriteLine($"{cuenta.Nombre} - {cuenta.Dni}");
}

var cuentasid = cuentaRepo.ObtenerCuentaPorId(2);

Console.WriteLine($"{cuentasid.Nombre}- {cuentasid.Dni}");
