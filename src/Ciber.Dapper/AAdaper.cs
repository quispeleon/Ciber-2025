using Dapper;
using MySqlConnector;

// using MySql.Data.MySqlClient;
using Ciber.core;
using System.Data;
namespace Ciber.Dapper
{
public class ADOD: IDAO
{
        private readonly IDbConnection _dbConnection;

        public ADOD(IDbConnection connectionString)
        {
            _dbConnection = connectionString;
        }
        
        --Sincronico--
        public void AgregarCuenta(Cuenta cuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("uNcuenta", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("unnombre", cuenta.Nombre);
            parameters.Add("unPas", cuenta.Pass);
            parameters.Add("dni", cuenta.Dni);
            parameters.Add("hora", cuenta.HoraRegistrada);

            _dbConnection.Execute("Cuentas", parameters, commandType: CommandType.StoredProcedure);
            cuenta.Ncuenta = parameters.Get<int>("uNcuenta");
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
        public void AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler)
        {
            var procedureName = tipoAlquiler ? "alquilarMaquina2" : "alquilarMaquina1";
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", alquiler.Ncuenta);
            parameters.Add("unNmaquina", alquiler.Nmaquina);

            if (tipoAlquiler)
            {
                parameters.Add("tcantidad", alquiler.CantidadTiempo);
                parameters.Add("pagadood", alquiler.Pagado);
            }

            parameters.Add("nIdAlquiler", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

            alquiler.IdAlquiler = parameters.Get<int>("nIdAlquiler");
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
        public IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial()
        {
            var sql = "SELECT * FROM HistorialAlquiler";
            return _dbConnection.Query<HistorialdeAlquiler>(sql);
        }                
        --Async--       

        public async Task AgregarCuenta(Cuenta cuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("uNcuenta", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("unnombre", cuenta.Nombre);
            parameters.Add("unPas", cuenta.Pass);
            parameters.Add("dni", cuenta.Dni);
            parameters.Add("hora", cuenta.HoraRegistrada);

            await _dbConnection.ExecuteAsync("Cuentas", parameters, commandType: CommandType.StoredProcedure);
            cuenta.Ncuenta = parameters.Get<int>("uNcuenta");
        }

        public async Task<Cuenta> ObtenerCuentaPorId(int ncuenta)
        {
            var sql = "SELECT * FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return await _dbConnection.QueryFirstOrDefaultAsync<Cuenta>(sql, new { Ncuenta = ncuenta });
        }

        public async Task ActualizarCuenta(Cuenta cuenta)
        {
            var sql = "UPDATE Cuenta SET nombre = @Nombre, pass = sha2(@Pass, 256), dni = @Dni, horaRegistrada = @HoraRegistrada WHERE Ncuenta = @Ncuenta";
            await _dbConnection.ExecuteAsync(sql, cuenta);
        }

        public async Task EliminarCuenta(int ncuenta)
        {
            var sql = "DELETE FROM Cuenta WHERE Ncuenta = @Ncuenta";
            await _dbConnection.ExecuteAsync(sql, new { Ncuenta = ncuenta });
        }

        public async Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentas()
        {
            var sql = "SELECT * FROM Cuenta";
            return await _dbConnection.QueryAsync<Cuenta>(sql);
        }

        public async Task AgregarMaquina(Maquina maquina)
        {
            var parameters = new DynamicParameters();
            parameters.Add("uNmaquina", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("unestado", maquina.Estado);
            parameters.Add("UnaCaracteristicas", maquina.Caracteristicas);

            await _dbConnection.ExecuteAsync("Maquinas", parameters, commandType: CommandType.StoredProcedure);
            maquina.Nmaquina = parameters.Get<int>("uNmaquina");
        }

        public async Task<Maquina> ObtenerMaquinaPorId(int nmaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE Nmaquina = @Nmaquina";
            return await _dbConnection.QueryFirstOrDefaultAsync<Maquina>(sql, new { Nmaquina = nmaquina });
        }

        public async Task ActualizarMaquina(Maquina maquina)
        {
            var sql = "UPDATE Maquina SET estado = @Estado, caracteristicas = @Caracteristicas WHERE Nmaquina = @Nmaquina";
            await _dbConnection.ExecuteAsync(sql, maquina);
        }

        public async Task EliminarMaquina(int nmaquina)
        {
            var sql = "DELETE FROM Maquina WHERE Nmaquina = @Nmaquina";
            await _dbConnection.ExecuteAsync(sql, new { Nmaquina = nmaquina });
        }

        public async Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinas()
        {
            var sql = "SELECT * FROM Maquina";
            return await _dbConnection.QueryAsync<Maquina>(sql);
        }

        public async Task AgregarTipo(Tipo tipo)
        {
            var sql = "INSERT INTO Tipo (IdTipo, TipoDescripcion) VALUES (@IdTipo, @TipoDescripcion)";
            await _dbConnection.ExecuteAsync(sql, tipo);
        }

        public async Task AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler)
        {
            var procedureName = tipoAlquiler ? "alquilarMaquina2" : "alquilarMaquina1";
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", alquiler.Ncuenta);
            parameters.Add("unNmaquina", alquiler.Nmaquina);

            if (tipoAlquiler)
            {
                parameters.Add("tcantidad", alquiler.CantidadTiempo);
                parameters.Add("pagadood", alquiler.Pagado);
            }

            parameters.Add("nIdAlquiler", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

            alquiler.IdAlquiler = parameters.Get<int>("nIdAlquiler");
        }

        public async Task<Alquiler> ObtenerAlquilerPorId(int idAlquiler)
        {
            var sql = "SELECT * FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return await _dbConnection.QueryFirstOrDefaultAsync<Alquiler>(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task EliminarAlquiler(int idAlquiler)
        {
            var sql = "DELETE FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            await _dbConnection.ExecuteAsync(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileres()
        {
            var sql = "SELECT * FROM Alquiler";
            return await _dbConnection.QueryAsync<Alquiler>(sql);
        }

        public async Task AgregarHistorial(HistorialdeAlquiler historial)
        {
            var sql = "INSERT INTO HistorialAlquiler (Ncuenta, Nmaquina, FechaInicio, FechaFin, TotalPagara) VALUES (@Ncuenta, @Nmaquina, @FechaInicio, @FechaFin, @TotalPagara)";
            await _dbConnection.ExecuteAsync(sql, historial);
        }

        public async Task<HistorialdeAlquiler> ObtenerHistorialPorId(int idHistorial)
        {
            var sql = "SELECT * FROM HistorialAlquiler WHERE idHistorial = @IdHistorial";
            return await _dbConnection.QueryFirstOrDefaultAsync<HistorialdeAlquiler>(sql, new { IdHistorial = idHistorial });
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorial()
        {
            var sql = "SELECT * FROM HistorialAlquiler";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql);
        }
    }
}