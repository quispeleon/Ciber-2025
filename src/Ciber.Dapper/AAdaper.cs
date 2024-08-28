using Dapper;
using MySqlConnector;

// using MySql.Data.MySqlClient;
using Ciber.core;
namespace Ciber.Dapper;
public class CuentaRepository : IDAO 
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

    public void AgregarMaquina(Maquina maquina)
    {
        
        var sql = "INSERT INTO Maquina(Nmaquina, estado ,caracteristcas) VALUES (@Nmaquina,@Estado,@Caracteristicas)";

        _dbConnection.Execute(sql, maquina);
    }

    public Maquina ObtenerMaquinaPorId(int nmaquina)
    {
        var sql = "SELECT * FROM Maquina WHERE Nmaquina = @nmaquina ";
        return _dbConnection.QueryFirstOrDefault<Maquina>(sql, new {
            Nmaquina = nmaquina
        });
    }

    public void ActualizarMaquina(Maquina maquina)
    {
        var sql = "UPDATE Maquina SET Nmaquina = @Nmaquina , estado = @estado , caracteristicas = @Caracteristicas";
        _dbConnection.Execute(sql,maquina);
    }

    public void EliminarMaquina(int nmaquina)
    {
        var sql = "DELETE FROM Maquina WHERE Nmaquina = @Nmaquna ";
        _dbConnection.Execute(sql,new {Ncuenta = nmaquina});
    }

    public void AgregarTipo(Tipo tipo)
    {
        throw new NotImplementedException();
    }

    public Tipo ObtenerTipoPorId(int idTipo)
    {
        throw new NotImplementedException();
    }

    public void ActualizarTipo(Tipo tipo)
    {
        throw new NotImplementedException();
    }

    public void EliminarTipo(int idTipo)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Tipo> ObtenerTodosLosTipos()
    {
        throw new NotImplementedException();
    }

    public void AgregarAlquiler(Alquiler alquiler)
    {
        throw new NotImplementedException();
    }

    public Alquiler ObtenerAlquilerPorId(int idAlquiler)
    {
        throw new NotImplementedException();
    }

    public void ActualizarAlquiler(Alquiler alquiler)
    {
        throw new NotImplementedException();
    }

    public void EliminarAlquiler(int idAlquiler)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Alquiler> ObtenerTodosLosAlquileres()
    {
        throw new NotImplementedException();
    }

    public void AgregarHistorial(HistorialdeAlquiler historial)
    {
        throw new NotImplementedException();
    }

    public HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial)
    {
        throw new NotImplementedException();
    }

    public void ActualizarHistorial(HistorialdeAlquiler historial)
    {
        throw new NotImplementedException();
    }

    public void EliminarHistorial(int idHistorial)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial()
    {
        throw new NotImplementedException();
    }
}


