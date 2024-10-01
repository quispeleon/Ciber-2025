using Ciber.core;
using Ciber.Dapper;

const string cadena = "Server=localhost;Database=5to_Ciber;Uid=5to_agbd;pwd=Trigg3rs!;Allow User Variables=True";
IDAO ado = new CuentaRepository(cadena);

            // Fetch all accounts
IEnumerable<Cuenta> cuentas = ado.ObtenerTodasLasCuentas();

 // Display the accounts
Console.WriteLine("Lista de cuentas:");
foreach (var cuenta in cuentas)
 {
    Console.WriteLine($"ID: {cuenta.Ncuenta}, Nombre: {cuenta.Nombre}, DNI: {cuenta.Dni}, Hora Registrada: {cuenta.HoraRegistrada}");
}

