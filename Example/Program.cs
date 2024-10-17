using Ciber.core;
using Ciber.Dapper;
using Microsoft.Extensions.Configuration;


Console.WriteLine(AppContext.BaseDirectory);


var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

                        var cadena = configuration.GetConnectionString("CiberDb");

                        var cadena1 = configuration.GetConnectionString("Ciber");

var CadenaCiberDbadena = configuration.GetConnectionString("CiberDb");

if (string.IsNullOrEmpty(cadena))
{
    throw new InvalidOperationException("La cadena de conexión 'CiberDb' no está configurada.");
}

IDAO ado = new CuentaRepository(cadena);

IEnumerable<Cuenta>? cuentas = ado.ObtenerTodasLasCuentas();

Console.WriteLine("Lista de cuentas:");
foreach (var cuenta in cuentas)
{
    Console.WriteLine($"ID: {cuenta.Ncuenta}, Nombre: {cuenta.Nombre}, DNI: {cuenta.Dni}, Hora Registrada: {cuenta.HoraRegistrada}");
}

var cadena2 = configuration.GetConnectionString("Ciber");

if (string.IsNullOrEmpty(cadena2))
{
    throw new InvalidOperationException("La cadena de conexión 'Ciber' no está configurada.");
}

IDAO otroado = new CuentaRepository(cadena2);

IEnumerable<Maquina> maquinas = otroado.ObtenerTodasLasMaquinas();

Console.WriteLine("Lista de maquinas: ");

foreach (var n in maquinas){
    Console.WriteLine($"{n.Nmaquina}, {n.Estado}, {n.Caracteristicas} ");

}