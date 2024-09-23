using Dapper;
using MySqlConnector;

// using MySql.Data.MySqlClient;
using Ciber.core;
namespace Ciber.Dapper
{
public class CuentaRepository : IDAO
{
        private readonly MySqlConnection _dbConnection;

        public CuentaRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }
                                        
        public void AgregarCuenta(Cuenta cuenta)
        {
            var sql = "INSERT INTO Cuenta (nombre, pass, dni, horaRegistrada) VALUES (@Nombre, sha2(@Pass, 256), @Dni, @HoraRegistrada); SELECT LAST_INSERT_ID();";

          
            var newId = _dbConnection.ExecuteScalar<int>(sql, cuenta);

            cuenta.Ncuenta = newId;
        }


        public Cuenta ObtenerCuentaPorId(int ncuenta)
        {
            var sql = "SELECT * FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return _dbConnection.QueryFirstOrDefault<Cuenta>(sql, new { Ncuenta = ncuenta });
        }   

        public void ActualizarCuenta(Cuenta cuenta)
        {
            var sql = "UPDATE Cuenta SET nombre = @Nombre, pass = sha2(@Pass, 256), dni = @Dni, horaRegistrada = @HoraRegistrada WHERE Ncuenta = @Ncuenta";
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
            var sql = "INSERT INTO Maquina (Nmaquina, estado, caracteristicas) VALUES (@Nmaquina, @Estado, @Caracteristicas); SELECT LAST_INSERT_ID();";
            var newId = _dbConnection.ExecuteScalar<int>(sql, maquina);
            maquina.Nmaquina = newId;

        }


        public Maquina ObtenerMaquinaPorId(int nmaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE Nmaquina = @Nmaquina";
            return _dbConnection.QueryFirstOrDefault<Maquina>(sql, new { Nmaquina = nmaquina });
        }

        public void ActualizarMaquina(Maquina maquina)
        {
            var sql = "UPDATE Maquina SET estado = @Estado, caracteristicas = @Caracteristicas WHERE Nmaquina = @Nmaquina";
            _dbConnection.Execute(sql, maquina);
        }

        public void EliminarMaquina(int nmaquina)
        {
            var sql = "DELETE FROM Maquina WHERE Nmaquina = @Nmaquina";
            _dbConnection.Execute(sql, new { Nmaquina = nmaquina });
        }
        public void AgregarTipo(Tipo tipo)
        {
            var sql = "INSERT INTO Tipo (IdTipo, TipoDescripcion) VALUES (@IdTipo, @TipoDescripcion)";
            _dbConnection.Execute(sql, tipo);
        }

        public void AgregarAlquiler(Alquiler alquiler)
        {
            var sql = "INSERT INTO Alquiler (Ncuenta, Nmaquina, Tipo, CantidadTiempo, Pagado) VALUES (@Ncuenta, @Nmaquina, @Tipo, @CantidadTiempo, @Pagado);SELECT LAST_INSERT_ID();";
            var newId = _dbConnection.ExecuteScalar<int>(sql, alquiler);
            alquiler.IdAlquiler = newId;
        }
      
        public Alquiler ObtenerAlquilerPorId(int idAlquiler)
        {
            var sql = "SELECT * FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return _dbConnection.QueryFirstOrDefault<Alquiler>(sql, new { IdAlquiler = idAlquiler });
        }

        public void EliminarAlquiler(int idAlquiler)
        {
            var sql = "DELETE FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            _dbConnection.Execute(sql, new { IdAlquiler = idAlquiler });
        }

        public IEnumerable<Alquiler> ObtenerTodosLosAlquileres()
        {
            var sql = "SELECT * FROM Alquiler";
            return _dbConnection.Query<Alquiler>(sql);
        }

        public void AgregarHistorial(HistorialdeAlquiler historial)
        {
            var sql = "INSERT INTO HistorialAlquiler (Ncuenta, Nmaquina, FechaInicio, FechaFin, TotalPagara) VALUES (@Ncuenta, @Nmaquina, @FechaInicio, @FechaFin, @TotalPagara)";
            _dbConnection.Execute(sql, historial);
        }

        public HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial)
        {
            var sql = "SELECT * FROM HistorialAlquiler WHERE idHistorial = @IdHistorial";
            return _dbConnection.QueryFirstOrDefault<HistorialdeAlquiler>(sql, new { IdHistorial = idHistorial });
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
}
