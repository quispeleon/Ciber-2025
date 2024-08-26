using Dapper;
using MySqlConnector;

// using MySql.Data.MySqlClient;
using Ciber.core;
namespace Ciber.Dapper;
public class CuentaRepository : ICuentaRepository
{
    private readonly MySqlConnection _dbConnection;

    public CuentaRepository(string connectionString)
    {
        _dbConnection = new MySqlConnection(connectionString);
    }

    public void AgregarCuenta(Cuenta cuenta)
    {                                 
        var sql = "INSERT INTO Cuenta (nombre, pass, dni, horaRegistrada) VALUES (@Nombre, @Pass, @Dni, @HoraRegistrada)";
        _dbConnection.Execute(sql, cuenta);
    }

    public Cuenta ObtenerCuentaPorId(int ncuenta)
    {
        var sql = "SELECT * FROM Cuenta WHERE Ncuenta = @Ncuenta";
        return _dbConnection.QueryFirstOrDefault<Cuenta>(sql, new { Ncuenta = ncuenta });
    }

    public void ActualizarCuenta(Cuenta cuenta)
    {
        var sql = "UPDATE Cuenta SET nombre = @Nombre, pass = @Pass, dni = @Dni, horaRegistrada = @HoraRegistrada WHERE Ncuenta = @Ncuenta";
        _dbConnection.Execute(sql, cuenta);
    }

    public void EliminarCuenta(int ncuenta)
    {
        var sql = "DELETE FROM Cuenta WHERE Ncuenta = @Ncuenta";
        _dbConnection.Execute(sql, new { Ncuenta = ncuenta });
    }

    public IEnumerable<Cuenta> ObtenerTodasLasCuentas()
    {
        var sql = "SELECT * FROM Cuenta";
        return _dbConnection.Query<Cuenta>(sql);
    }
}