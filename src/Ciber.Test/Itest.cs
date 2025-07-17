using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Ciber.Test;
public class TestAdo
{
    protected readonly IDbConnection Conexion;
    public TestAdo()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
            .Build();
        string cadena = config.GetConnectionString("MySQL")!;
        Conexion = new MySqlConnection(cadena);
    }        
}