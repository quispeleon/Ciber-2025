namespace Ciber.Test;

public class Class1
{
 string connectionString = "Server=localhost;Database=Ciber;User=root;Password=tu_contraseña;";

        // ]las instancias
        ICuentaRepository cuentaRepo = new CuentaRepository(connectionString);
        IMaquinaRepository maquinaRepo = new MaquinaRepository(connectionString);
        ITipoRepository tipoRepo = new TipoRepository(connectionString);
        IAlquilerRepository alquilerRepo = new AlquilerRepository(connectionString);
        IHistorialdeAlquilerRepository historialRepo = new HistorialdeAlquilerRepository(connectionString);

        // agregar una nueva cuenta
        var nuevaCuenta = new Cuenta
        {
            Nombre = "Juan Perez",
            Pass = "1234",
            Dni = 12345678,
            HoraRegistrada = DateTime.Now.TimeOfDay
        };
        cuentaRepo.AgregarCuenta(nuevaCuenta);

        //  todas las cuentas
        var cuentas = cuentaRepo.ObtenerTodasLasCuentas();
        foreach (var cuenta in cuentas)
        {
            Console.WriteLine($"{cuenta.Nombre} - {cuenta.Dni}");
        }

}
