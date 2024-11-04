using Dapper;
using MySqlConnector;

// using MySql.Data.MySqlClient;
using Ciber.core;
using System.Data;
namespace Ciber.Dapper
{
public class ADOD: IDAO
{
        private readonly MySqlConnection _dbConnection;

        public ADOD(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }
                                        
       public void AgregarCuenta(Cuenta cuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("uNcuenta", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("unnombre", cuenta.Nombre);
            parameters.Add("unPas", cuenta.Pass);  // Suponiendo que el pass viene ya en su formato
            parameters.Add("dni", cuenta.Dni);
            parameters.Add("hora", cuenta.HoraRegistrada);

            _dbConnection.Execute("Cuentas", parameters, commandType: CommandType.StoredProcedure);
            
            // Obtener el valor del id generado por el procedimiento almacenado
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


       public void AgregarMaquina(Maquina maquina)
        {
            var parameters = new DynamicParameters();
            parameters.Add("uNmaquina", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("unestado", maquina.Estado);
            parameters.Add("UnaCaracteristicas", maquina.Caracteristicas);

            _dbConnection.Execute("Maquinas", parameters, commandType: CommandType.StoredProcedure);

            maquina.Nmaquina = parameters.Get<int>("uNmaquina");
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

        public IEnumerable<Maquina> ObtenerTodasLasMaquinas(){
            var sql = "SELECT * FROM Maquina";
            return _dbConnection.Query<Maquina>(sql);
        }




        public void AgregarTipo(Tipo tipo)
        {
            var sql = "INSERT INTO Tipo (IdTipo, TipoDescripcion) VALUES (@IdTipo, @TipoDescripcion)";
            _dbConnection.Execute(sql, tipo);
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

            // Añade el parametro de salida para el ID
            parameters.Add("nIdAlquiler", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

            // Asigna el ID obtenido al alquiler
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
        
    }   

}
 