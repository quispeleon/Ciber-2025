using Dapper;
using MySqlConnector;
using Ciber.core;
using System.Data;

namespace Ciber.Dapper
{
    public class ADOD : IDAO
    {
        private readonly IDbConnection _dbConnection;

        public ADOD(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // ========== CUENTA ==========
        public void AgregarCuenta(Cuenta cuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unnombre", cuenta.Nombre);
            parameters.Add("unPas", cuenta.Pass);
            parameters.Add("unDni", cuenta.Dni);
            parameters.Add("saldoInicial", cuenta.Saldo);
            parameters.Add("uNcuenta", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute("RegistrarCuenta", parameters, commandType: CommandType.StoredProcedure);
            cuenta.Ncuenta = parameters.Get<int>("uNcuenta");
        }

        public Cuenta ObtenerCuentaPorId(int ncuenta)
        {
            var sql = "SELECT * FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return _dbConnection.QueryFirstOrDefault<Cuenta>(sql, new { Ncuenta = ncuenta });
        }

        public void ActualizarCuenta(Cuenta cuenta)
        {
            var sql = @"UPDATE Cuenta SET nombre = @Nombre, pass = @Pass, dni = @Dni, 
                       horaRegistrada = @HoraRegistrada, saldo = @Saldo, activa = @Activa 
                       WHERE Ncuenta = @Ncuenta";
            _dbConnection.Execute(sql, cuenta);
        }

        public void EliminarCuenta(int ncuenta)
        {
            var sql = "UPDATE Cuenta SET activa = false WHERE Ncuenta = @Ncuenta";
            _dbConnection.Execute(sql, new { Ncuenta = ncuenta });
        }

        public IEnumerable<Cuenta> ObtenerTodasLasCuentas()
        {
            var sql = "SELECT * FROM Cuenta WHERE activa = true";
            return _dbConnection.Query<Cuenta>(sql);
        }

        public Cuenta ObtenerCuentaPorDni(string dni)
        {
            var sql = "SELECT * FROM Cuenta WHERE dni = @Dni AND activa = true";
            return _dbConnection.QueryFirstOrDefault<Cuenta>(sql, new { Dni = dni });
        }

        public IEnumerable<Cuenta> ObtenerCuentasActivas()
        {
            var sql = "SELECT * FROM Cuenta WHERE activa = true";
            return _dbConnection.Query<Cuenta>(sql);
        }

        public void RecargarSaldo(int ncuenta, decimal monto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("monto", monto);
            _dbConnection.Execute("RecargarSaldo", parameters, commandType: CommandType.StoredProcedure);
        }

        public decimal ObtenerSaldoCuenta(int ncuenta)
        {
            var sql = "SELECT saldo FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return _dbConnection.QueryFirstOrDefault<decimal>(sql, new { Ncuenta = ncuenta });
        }

        public bool ValidarCredenciales(string dni, string password)
        {
            var sql = "SELECT COUNT(*) FROM Cuenta WHERE dni = @Dni AND pass = SHA2(@Password, 256) AND activa = true";
            var count = _dbConnection.QueryFirstOrDefault<int>(sql, new { Dni = dni, Password = password });
            return count > 0;
        }

        // ========== MÁQUINA ==========
        public void AgregarMaquina(Maquina maquina)
        {
            var sql = @"INSERT INTO Maquina (estado, caracteristicas, precioPorHora, tipoMaquina, 
                       ultimoMantenimiento, proximoMantenimiento) 
                       VALUES (@Estado, @Caracteristicas, @PrecioPorHora, @TipoMaquina, 
                       @UltimoMantenimiento, @ProximoMantenimiento);
                       SELECT LAST_INSERT_ID();";
            var id = _dbConnection.QuerySingle<int>(sql, maquina);
            maquina.Nmaquina = id;
        }

        public Maquina ObtenerMaquinaPorId(int nmaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE Nmaquina = @Nmaquina";
            return _dbConnection.QueryFirstOrDefault<Maquina>(sql, new { Nmaquina = nmaquina });
        }

        public void ActualizarMaquina(Maquina maquina)
        {
            var sql = @"UPDATE Maquina SET estado = @Estado, caracteristicas = @Caracteristicas, 
                       precioPorHora = @PrecioPorHora, tipoMaquina = @TipoMaquina,
                       ultimoMantenimiento = @UltimoMantenimiento, proximoMantenimiento = @ProximoMantenimiento
                       WHERE Nmaquina = @Nmaquina";
            _dbConnection.Execute(sql, maquina);
        }

        public void EliminarMaquina(int nmaquina)
        {
            var sql = "DELETE FROM Maquina WHERE Nmaquina = @Nmaquina";
            _dbConnection.Execute(sql, new { Nmaquina = nmaquina });
        }

        public IEnumerable<Maquina> ObtenerTodasLasMaquinas()
        {
            var sql = "SELECT * FROM Maquina";
            return _dbConnection.Query<Maquina>(sql);
        }

        public IEnumerable<Maquina> ObtenerMaquinasDisponibles()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Disponible'";
            return _dbConnection.Query<Maquina>(sql);
        }

        public IEnumerable<Maquina> ObtenerMaquinasOcupadas()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Ocupada'";
            return _dbConnection.Query<Maquina>(sql);
        }

        public IEnumerable<Maquina> ObtenerMaquinasEnMantenimiento()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Mantenimiento'";
            return _dbConnection.Query<Maquina>(sql);
        }

        public void CambiarEstadoMaquina(int nmaquina, string nuevoEstado)
        {
            var sql = "UPDATE Maquina SET estado = @Estado WHERE Nmaquina = @Nmaquina";
            _dbConnection.Execute(sql, new { Estado = nuevoEstado, Nmaquina = nmaquina });
        }

        public int ObtenerCantidadMaquinasDisponibles()
        {
            var sql = "SELECT COUNT(*) FROM Maquina WHERE estado = 'Disponible'";
            return _dbConnection.QueryFirstOrDefault<int>(sql);
        }

        // ========== TIPO ==========
        public void AgregarTipo(Tipo tipo)
        {
            var sql = @"INSERT INTO Tipo (tipo, descripcion, requierePagoPrevio) 
                       VALUES (@TipoDescripcion, @Descripcion, @RequierePagoPrevio);
                       SELECT LAST_INSERT_ID();";
            var id = _dbConnection.QuerySingle<int>(sql, tipo);
            tipo.IdTipo = id;
        }

        public Tipo ObtenerTipoPorId(int idTipo)
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = @IdTipo";
            return _dbConnection.QueryFirstOrDefault<Tipo>(sql, new { IdTipo = idTipo });
        }

        public void ActualizarTipo(Tipo tipo)
        {
            var sql = @"UPDATE Tipo SET tipo = @TipoDescripcion, descripcion = @Descripcion, 
                       requierePagoPrevio = @RequierePagoPrevio WHERE idTipo = @IdTipo";
            _dbConnection.Execute(sql, tipo);
        }

        public void EliminarTipo(int idTipo)
        {
            var sql = "DELETE FROM Tipo WHERE idTipo = @IdTipo";
            _dbConnection.Execute(sql, new { IdTipo = idTipo });
        }

        public IEnumerable<Tipo> ObtenerTodosLosTipos()
        {
            var sql = "SELECT * FROM Tipo";
            return _dbConnection.Query<Tipo>(sql);
        }

        public Tipo ObtenerTipoLibre()
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = 1";
            return _dbConnection.QueryFirstOrDefault<Tipo>(sql);
        }

        public Tipo ObtenerTipoHoraDefinida()
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = 2";
            return _dbConnection.QueryFirstOrDefault<Tipo>(sql);
        }

        // ========== ALQUILER ==========
        public void AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", alquiler.Ncuenta);
            parameters.Add("unNmaquina", alquiler.Nmaquina);
            parameters.Add("unTipo", tipoAlquiler ? 2 : 1);
            parameters.Add("tiempoContratado", alquiler.TiempoContratado?.ToString(@"hh\:mm\:ss"));
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            alquiler.IdAlquiler = parameters.Get<int>("idAlquilerCreado");
        }

        public Alquiler ObtenerAlquilerPorId(int idAlquiler)
        {
            var sql = "SELECT * FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return _dbConnection.QueryFirstOrDefault<Alquiler>(sql, new { IdAlquiler = idAlquiler });
        }

        public void ActualizarAlquiler(Alquiler alquiler)
        {
            var sql = @"UPDATE Alquiler SET Ncuenta = @Ncuenta, Nmaquina = @Nmaquina, tipo = @Tipo,
                       fechaInicio = @FechaInicio, fechaFin = @FechaFin, tiempoContratado = @TiempoContratado,
                       tiempoUsado = @TiempoUsado, precioPorHora = @PrecioPorHora, totalAPagar = @TotalAPagar,
                       montoPagado = @MontoPagado, estado = @Estado, sesionActiva = @SesionActiva
                       WHERE idAlquiler = @IdAlquiler";
            _dbConnection.Execute(sql, alquiler);
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

        public IEnumerable<Alquiler> ObtenerAlquileresActivos()
        {
            var sql = "SELECT * FROM Alquiler WHERE sesionActiva = true";
            return _dbConnection.Query<Alquiler>(sql);
        }

        public IEnumerable<Alquiler> ObtenerAlquileresPorCuenta(int ncuenta)
        {
            var sql = "SELECT * FROM Alquiler WHERE Ncuenta = @Ncuenta";
            return _dbConnection.Query<Alquiler>(sql, new { Ncuenta = ncuenta });
        }

        public IEnumerable<Alquiler> ObtenerAlquileresPorMaquina(int nmaquina)
        {
            var sql = "SELECT * FROM Alquiler WHERE Nmaquina = @Nmaquina";
            return _dbConnection.Query<Alquiler>(sql, new { Nmaquina = nmaquina });
        }

        public Alquiler ObtenerAlquilerActivoPorCuenta(int ncuenta)
        {
            var sql = "SELECT * FROM Alquiler WHERE Ncuenta = @Ncuenta AND sesionActiva = true";
            return _dbConnection.QueryFirstOrDefault<Alquiler>(sql, new { Ncuenta = ncuenta });
        }

        public Alquiler ObtenerAlquilerActivoPorMaquina(int nmaquina)
        {
            var sql = "SELECT * FROM Alquiler WHERE Nmaquina = @Nmaquina AND sesionActiva = true";
            return _dbConnection.QueryFirstOrDefault<Alquiler>(sql, new { Nmaquina = nmaquina });
        }

        public void FinalizarAlquiler(int idAlquiler, decimal montoPagado)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdAlquiler", idAlquiler);
            _dbConnection.Execute("FinalizarAlquiler", parameters, commandType: CommandType.StoredProcedure);
        }

        public bool TieneAlquilerActivo(int ncuenta)
        {
            var sql = "SELECT COUNT(*) FROM Alquiler WHERE Ncuenta = @Ncuenta AND sesionActiva = true";
            return _dbConnection.QueryFirstOrDefault<int>(sql, new { Ncuenta = ncuenta }) > 0;
        }

        public TimeSpan ObtenerTiempoUsoActual(int idAlquiler)
        {
            var sql = "SELECT TIMEDIFF(NOW(), fechaInicio) FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return _dbConnection.QueryFirstOrDefault<TimeSpan>(sql, new { IdAlquiler = idAlquiler });
        }

        public decimal ObtenerCostoActualAlquiler(int idAlquiler)
        {
            var alquiler = ObtenerAlquilerPorId(idAlquiler);
            if (alquiler == null) return 0;

            var tiempoUso = ObtenerTiempoUsoActual(idAlquiler);
            return (decimal)tiempoUso.TotalHours * alquiler.PrecioPorHora;
        }

        // ========== HISTORIAL ==========
        public void AgregarHistorial(HistorialdeAlquiler historial)
        {
            var sql = @"INSERT INTO HistorialdeAlquiler (idAlquiler, Ncuenta, Nmaquina, fechaInicio, fechaFin,
                       tiempoContratado, tiempoUsado, precioPorHora, TotalPagar, montoPagado, estadoFinal, observaciones)
                       VALUES (@IdAlquiler, @Ncuenta, @Nmaquina, @FechaInicio, @FechaFin, @TiempoContratado,
                       @TiempoUsado, @PrecioPorHora, @TotalPagar, @MontoPagado, @EstadoFinal, @Observaciones);
                       SELECT LAST_INSERT_ID();";
            var id = _dbConnection.QuerySingle<int>(sql, historial);
            historial.IdHistorial = id;
        }

        public HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE idHistorial = @IdHistorial";
            return _dbConnection.QueryFirstOrDefault<HistorialdeAlquiler>(sql, new { IdHistorial = idHistorial });
        }

        public void ActualizarHistorial(HistorialdeAlquiler historial)
        {
            var sql = @"UPDATE HistorialdeAlquiler SET idAlquiler = @IdAlquiler, Ncuenta = @Ncuenta, Nmaquina = @Nmaquina,
                       fechaInicio = @FechaInicio, fechaFin = @FechaFin, tiempoContratado = @TiempoContratado,
                       tiempoUsado = @TiempoUsado, precioPorHora = @PrecioPorHora, TotalPagar = @TotalPagar,
                       montoPagado = @MontoPagado, estadoFinal = @EstadoFinal, observaciones = @Observaciones
                       WHERE idHistorial = @IdHistorial";
            _dbConnection.Execute(sql, historial);
        }

        public void EliminarHistorial(int idHistorial)
        {
            var sql = "DELETE FROM HistorialdeAlquiler WHERE idHistorial = @IdHistorial";
            _dbConnection.Execute(sql, new { IdHistorial = idHistorial });
        }

        public IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial()
        {
            var sql = "SELECT * FROM HistorialdeAlquiler ORDER BY fechaFin DESC";
            return _dbConnection.Query<HistorialdeAlquiler>(sql);
        }

        public IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorCuenta(int ncuenta)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE Ncuenta = @Ncuenta ORDER BY fechaFin DESC";
            return _dbConnection.Query<HistorialdeAlquiler>(sql, new { Ncuenta = ncuenta });
        }

        public IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorMaquina(int nmaquina)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE Nmaquina = @Nmaquina ORDER BY fechaFin DESC";
            return _dbConnection.Query<HistorialdeAlquiler>(sql, new { Nmaquina = nmaquina });
        }

        public IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorFecha(DateTime fecha)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE DATE(fechaFin) = @Fecha ORDER BY fechaFin DESC";
            return _dbConnection.Query<HistorialdeAlquiler>(sql, new { Fecha = fecha.Date });
        }

        public IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorRangoFechas(DateTime inicio, DateTime fin)
        {
            var sql = @"SELECT * FROM HistorialdeAlquiler 
                       WHERE fechaFin BETWEEN @Inicio AND @Fin 
                       ORDER BY fechaFin DESC";
            return _dbConnection.Query<HistorialdeAlquiler>(sql, new { Inicio = inicio, Fin = fin });
        }

        public decimal ObtenerIngresosPorFecha(DateTime fecha)
        {
            var sql = "SELECT COALESCE(SUM(TotalPagar), 0) FROM HistorialdeAlquiler WHERE DATE(fechaFin) = @Fecha";
            return _dbConnection.QueryFirstOrDefault<decimal>(sql, new { Fecha = fecha.Date });
        }

        public decimal ObtenerIngresosPorRangoFechas(DateTime inicio, DateTime fin)
        {
            var sql = "SELECT COALESCE(SUM(TotalPagar), 0) FROM HistorialdeAlquiler WHERE fechaFin BETWEEN @Inicio AND @Fin";
            return _dbConnection.QueryFirstOrDefault<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }

        // ========== TRANSACCIONES ==========
        public void AgregarTransaccion(Transaccion transaccion)
        {
            var sql = @"INSERT INTO Transacciones (Ncuenta, tipo, monto, idAlquiler, descripcion)
                       VALUES (@Ncuenta, @Tipo, @Monto, @IdAlquiler, @Descripcion);
                       SELECT LAST_INSERT_ID();";
            var id = _dbConnection.QuerySingle<int>(sql, transaccion);
            transaccion.IdTransaccion = id;
        }

        public Transaccion ObtenerTransaccionPorId(int idTransaccion)
        {
            var sql = "SELECT * FROM Transacciones WHERE idTransaccion = @IdTransaccion";
            return _dbConnection.QueryFirstOrDefault<Transaccion>(sql, new { IdTransaccion = idTransaccion });
        }

        public void ActualizarTransaccion(Transaccion transaccion)
        {
            var sql = @"UPDATE Transacciones SET Ncuenta = @Ncuenta, tipo = @Tipo, monto = @Monto,
                       idAlquiler = @IdAlquiler, descripcion = @Descripcion
                       WHERE idTransaccion = @IdTransaccion";
            _dbConnection.Execute(sql, transaccion);
        }

        public void EliminarTransaccion(int idTransaccion)
        {
            var sql = "DELETE FROM Transacciones WHERE idTransaccion = @IdTransaccion";
            _dbConnection.Execute(sql, new { IdTransaccion = idTransaccion });
        }

        public IEnumerable<Transaccion> ObtenerTodasLasTransacciones()
        {
            var sql = "SELECT * FROM Transacciones ORDER BY fecha DESC";
            return _dbConnection.Query<Transaccion>(sql);
        }

        public IEnumerable<Transaccion> ObtenerTransaccionesPorCuenta(int ncuenta)
        {
            var sql = "SELECT * FROM Transacciones WHERE Ncuenta = @Ncuenta ORDER BY fecha DESC";
            return _dbConnection.Query<Transaccion>(sql, new { Ncuenta = ncuenta });
        }

        public IEnumerable<Transaccion> ObtenerTransaccionesPorTipo(string tipo)
        {
            var sql = "SELECT * FROM Transacciones WHERE tipo = @Tipo ORDER BY fecha DESC";
            return _dbConnection.Query<Transaccion>(sql, new { Tipo = tipo });
        }

        public IEnumerable<Transaccion> ObtenerTransaccionesPorAlquiler(int idAlquiler)
        {
            var sql = "SELECT * FROM Transacciones WHERE idAlquiler = @IdAlquiler ORDER BY fecha DESC";
            return _dbConnection.Query<Transaccion>(sql, new { IdAlquiler = idAlquiler });
        }

        public IEnumerable<Transaccion> ObtenerTransaccionesPorFecha(DateTime fecha)
        {
            var sql = "SELECT * FROM Transacciones WHERE DATE(fecha) = @Fecha ORDER BY fecha DESC";
            return _dbConnection.Query<Transaccion>(sql, new { Fecha = fecha.Date });
        }

        public decimal ObtenerTotalRecargasPorCuenta(int ncuenta)
        {
            var sql = "SELECT COALESCE(SUM(monto), 0) FROM Transacciones WHERE Ncuenta = @Ncuenta AND tipo = 'Recarga'";
            return _dbConnection.QueryFirstOrDefault<decimal>(sql, new { Ncuenta = ncuenta });
        }

        public decimal ObtenerTotalConsumosPorCuenta(int ncuenta)
        {
            var sql = "SELECT COALESCE(SUM(monto), 0) FROM Transacciones WHERE Ncuenta = @Ncuenta AND tipo = 'Consumo'";
            return _dbConnection.QueryFirstOrDefault<decimal>(sql, new { Ncuenta = ncuenta });
        }

        // ========== MÉTODOS DE NEGOCIO ESPECÍFICOS ==========
        public int IniciarAlquilerLibre(int ncuenta, int nmaquina)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("unNmaquina", nmaquina);
            parameters.Add("unTipo", 1);
            parameters.Add("tiempoContratado", null);
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("idAlquilerCreado");
        }

        public int IniciarAlquilerTiempoDefinido(int ncuenta, int nmaquina, TimeSpan tiempo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("unNmaquina", nmaquina);
            parameters.Add("unTipo", 2);
            parameters.Add("tiempoContratado", tiempo.ToString(@"hh\:mm\:ss"));
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            _dbConnection.Execute("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("idAlquilerCreado");
        }

        public void FinalizarAlquilerCompleto(int idAlquiler)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdAlquiler", idAlquiler);
            _dbConnection.Execute("FinalizarAlquiler", parameters, commandType: CommandType.StoredProcedure);
        }

        public bool VerificarSaldoSuficiente(int ncuenta, TimeSpan tiempoSolicitado, int nmaquina)
        {
            var maquina = ObtenerMaquinaPorId(nmaquina);
            var saldo = ObtenerSaldoCuenta(ncuenta);
            var costo = (decimal)tiempoSolicitado.TotalHours * maquina.PrecioPorHora;
            return saldo >= costo;
        }

        public decimal CalcularCostoTiempo(TimeSpan tiempo, decimal precioPorHora)
        {
            return (decimal)tiempo.TotalHours * precioPorHora;
        }

        public IEnumerable<Maquina> ObtenerMaquinasPorTipo(string tipoMaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE tipoMaquina = @TipoMaquina";
            return _dbConnection.Query<Maquina>(sql, new { TipoMaquina = tipoMaquina });
        }

        public Dictionary<string, decimal> ObtenerEstadisticasDiarias(DateTime fecha)
        {
            var sql = @"
                SELECT 
                    COUNT(*) as TotalAlquileres,
                    COALESCE(SUM(TotalPagar), 0) as IngresosTotales,
                    AVG(TIME_TO_SEC(tiempoUsado)/3600) as PromedioHoras
                FROM HistorialdeAlquiler 
                WHERE DATE(fechaFin) = @Fecha";

            var result = _dbConnection.QueryFirstOrDefault(sql, new { Fecha = fecha.Date });

            return new Dictionary<string, decimal>
            {
                { "TotalAlquileres", result?.TotalAlquileres ?? 0 },
                { "IngresosTotales", result?.IngresosTotales ?? 0 },
                { "PromedioHoras", result?.PromedioHoras ?? 0 }
            };
        }

        public IEnumerable<dynamic> ObtenerTopClientes(int cantidad)
        {
            var sql = @"
                SELECT c.nombre, c.dni, COUNT(*) as TotalAlquileres, SUM(h.TotalPagar) as TotalGastado
                FROM HistorialdeAlquiler h
                JOIN Cuenta c ON h.Ncuenta = c.Ncuenta
                GROUP BY c.Ncuenta, c.nombre, c.dni
                ORDER BY TotalGastado DESC
                LIMIT @Cantidad";

            return _dbConnection.Query(sql, new { Cantidad = cantidad });
        }

        public IEnumerable<dynamic> ObtenerMaquinasMasRentables()
        {
            var sql = @"
                SELECT m.Nmaquina, m.caracteristicas, m.tipoMaquina,
                       COUNT(*) as VecesAlquilada, 
                       COALESCE(SUM(h.TotalPagar), 0) as IngresosGenerados
                FROM Maquina m
                LEFT JOIN HistorialdeAlquiler h ON m.Nmaquina = h.Nmaquina
                GROUP BY m.Nmaquina, m.caracteristicas, m.tipoMaquina
                ORDER BY IngresosGenerados DESC";

            return _dbConnection.Query(sql);
        }

        // ========== MÉTODOS ASÍNCRONOS ==========
        // Implementaré solo algunos métodos asíncronos clave para mostrar el patrón

        public async Task AgregarCuentaAsync(Cuenta cuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unnombre", cuenta.Nombre);
            parameters.Add("unPas", cuenta.Pass);
            parameters.Add("unDni", cuenta.Dni);
            parameters.Add("saldoInicial", cuenta.Saldo);
            parameters.Add("uNcuenta", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("RegistrarCuenta", parameters, commandType: CommandType.StoredProcedure);
            cuenta.Ncuenta = parameters.Get<int>("uNcuenta");
        }

        public async Task<Cuenta> ObtenerCuentaPorIdAsync(int ncuenta)
        {
            var sql = "SELECT * FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return await _dbConnection.QueryFirstOrDefaultAsync<Cuenta>(sql, new { Ncuenta = ncuenta });
        }

        public async Task ActualizarCuentaAsync(Cuenta cuenta)
        {
            var sql = @"UPDATE Cuenta SET nombre = @Nombre, pass = @Pass, dni = @Dni, 
                       horaRegistrada = @HoraRegistrada, saldo = @Saldo, activa = @Activa 
                       WHERE Ncuenta = @Ncuenta";
            await _dbConnection.ExecuteAsync(sql, cuenta);
        }

        public async Task EliminarCuentaAsync(int ncuenta)
        {
            var sql = "UPDATE Cuenta SET activa = false WHERE Ncuenta = @Ncuenta";
            await _dbConnection.ExecuteAsync(sql, new { Ncuenta = ncuenta });
        }

        public async Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentasAsync()
        {
            var sql = "SELECT * FROM Cuenta WHERE activa = true";
            return await _dbConnection.QueryAsync<Cuenta>(sql);
        }

        // Continuaría con el resto de métodos asíncronos siguiendo el mismo patrón...
        // Por brevedad, solo muestro algunos ejemplos

        public async Task<IEnumerable<Maquina>> ObtenerMaquinasDisponiblesAsync()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Disponible'";
            return await _dbConnection.QueryAsync<Maquina>(sql);
        }

        public async Task<IEnumerable<Alquiler>> ObtenerAlquileresActivosAsync()
        {
            var sql = "SELECT * FROM Alquiler WHERE sesionActiva = true";
            return await _dbConnection.QueryAsync<Alquiler>(sql);
        }

        public async Task FinalizarAlquilerAsync(int idAlquiler, decimal montoPagado)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdAlquiler", idAlquiler);
            await _dbConnection.ExecuteAsync("FinalizarAlquiler", parameters, commandType: CommandType.StoredProcedure);
        }

        // Implementar el resto de métodos asíncronos siguiendo el mismo patrón...
        // Por espacio, no los incluyo todos, pero el patrón es el mismo
        // ========== MÉTODOS ASÍNCRONOS COMPLETOS ==========

        public async Task<Cuenta> ObtenerCuentaPorDniAsync(string dni)
        {
            var sql = "SELECT * FROM Cuenta WHERE dni = @Dni AND activa = true";
            return await _dbConnection.QueryFirstOrDefaultAsync<Cuenta>(sql, new { Dni = dni });
        }

        public async Task<IEnumerable<Cuenta>> ObtenerCuentasActivasAsync()
        {
            var sql = "SELECT * FROM Cuenta WHERE activa = true";
            return await _dbConnection.QueryAsync<Cuenta>(sql);
        }

        public async Task RecargarSaldoAsync(int ncuenta, decimal monto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("monto", monto);
            await _dbConnection.ExecuteAsync("RecargarSaldo", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<decimal> ObtenerSaldoCuentaAsync(int ncuenta)
        {
            var sql = "SELECT saldo FROM Cuenta WHERE Ncuenta = @Ncuenta";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<bool> ValidarCredencialesAsync(string dni, string password)
        {
            var sql = "SELECT COUNT(*) FROM Cuenta WHERE dni = @Dni AND pass = SHA2(@Password, 256) AND activa = true";
            var count = await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { Dni = dni, Password = password });
            return count > 0;
        }

        public async Task AgregarMaquinaAsync(Maquina maquina)
        {
            var sql = @"INSERT INTO Maquina (estado, caracteristicas, precioPorHora, tipoMaquina, 
                ultimoMantenimiento, proximoMantenimiento) 
                VALUES (@Estado, @Caracteristicas, @PrecioPorHora, @TipoMaquina, 
                @UltimoMantenimiento, @ProximoMantenimiento);
                SELECT LAST_INSERT_ID();";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, maquina);
            maquina.Nmaquina = id;
        }

        public async Task<Maquina> ObtenerMaquinaPorIdAsync(int nmaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE Nmaquina = @Nmaquina";
            return await _dbConnection.QueryFirstOrDefaultAsync<Maquina>(sql, new { Nmaquina = nmaquina });
        }

        public async Task ActualizarMaquinaAsync(Maquina maquina)
        {
            var sql = @"UPDATE Maquina SET estado = @Estado, caracteristicas = @Caracteristicas, 
                precioPorHora = @PrecioPorHora, tipoMaquina = @TipoMaquina,
                ultimoMantenimiento = @UltimoMantenimiento, proximoMantenimiento = @ProximoMantenimiento
                WHERE Nmaquina = @Nmaquina";
            await _dbConnection.ExecuteAsync(sql, maquina);
        }

        public async Task EliminarMaquinaAsync(int nmaquina)
        {
            var sql = "DELETE FROM Maquina WHERE Nmaquina = @Nmaquina";
            await _dbConnection.ExecuteAsync(sql, new { Nmaquina = nmaquina });
        }

        public async Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinasAsync()
        {
            var sql = "SELECT * FROM Maquina";
            return await _dbConnection.QueryAsync<Maquina>(sql);
        }

        public async Task<IEnumerable<Maquina>> ObtenerMaquinasOcupadasAsync()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Ocupada'";
            return await _dbConnection.QueryAsync<Maquina>(sql);
        }

        public async Task<IEnumerable<Maquina>> ObtenerMaquinasEnMantenimientoAsync()
        {
            var sql = "SELECT * FROM Maquina WHERE estado = 'Mantenimiento'";
            return await _dbConnection.QueryAsync<Maquina>(sql);
        }

        public async Task CambiarEstadoMaquinaAsync(int nmaquina, string nuevoEstado)
        {
            var sql = "UPDATE Maquina SET estado = @Estado WHERE Nmaquina = @Nmaquina";
            await _dbConnection.ExecuteAsync(sql, new { Estado = nuevoEstado, Nmaquina = nmaquina });
        }

        public async Task<int> ObtenerCantidadMaquinasDisponiblesAsync()
        {
            var sql = "SELECT COUNT(*) FROM Maquina WHERE estado = 'Disponible'";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(sql);
        }

        public async Task AgregarTipoAsync(Tipo tipo)
        {
            var sql = @"INSERT INTO Tipo (tipo, descripcion, requierePagoPrevio) 
                VALUES (@TipoDescripcion, @Descripcion, @RequierePagoPrevio);
                SELECT LAST_INSERT_ID();";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, tipo);
            tipo.IdTipo = id;
        }

        public async Task<Tipo> ObtenerTipoPorIdAsync(int idTipo)
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = @IdTipo";
            return await _dbConnection.QueryFirstOrDefaultAsync<Tipo>(sql, new { IdTipo = idTipo });
        }

        public async Task ActualizarTipoAsync(Tipo tipo)
        {
            var sql = @"UPDATE Tipo SET tipo = @TipoDescripcion, descripcion = @Descripcion, 
                requierePagoPrevio = @RequierePagoPrevio WHERE idTipo = @IdTipo";
            await _dbConnection.ExecuteAsync(sql, tipo);
        }

        public async Task EliminarTipoAsync(int idTipo)
        {
            var sql = "DELETE FROM Tipo WHERE idTipo = @IdTipo";
            await _dbConnection.ExecuteAsync(sql, new { IdTipo = idTipo });
        }

        public async Task<IEnumerable<Tipo>> ObtenerTodosLosTiposAsync()
        {
            var sql = "SELECT * FROM Tipo";
            return await _dbConnection.QueryAsync<Tipo>(sql);
        }

        public async Task<Tipo> ObtenerTipoLibreAsync()
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = 1";
            return await _dbConnection.QueryFirstOrDefaultAsync<Tipo>(sql);
        }

        public async Task<Tipo> ObtenerTipoHoraDefinidaAsync()
        {
            var sql = "SELECT * FROM Tipo WHERE idTipo = 2";
            return await _dbConnection.QueryFirstOrDefaultAsync<Tipo>(sql);
        }

        public async Task AgregarAlquilerAsync(Alquiler alquiler, bool tipoAlquiler)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", alquiler.Ncuenta);
            parameters.Add("unNmaquina", alquiler.Nmaquina);
            parameters.Add("unTipo", tipoAlquiler ? 2 : 1);
            parameters.Add("tiempoContratado", alquiler.TiempoContratado?.ToString(@"hh\:mm\:ss"));
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            alquiler.IdAlquiler = parameters.Get<int>("idAlquilerCreado");
        }

        public async Task<Alquiler> ObtenerAlquilerPorIdAsync(int idAlquiler)
        {
            var sql = "SELECT * FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return await _dbConnection.QueryFirstOrDefaultAsync<Alquiler>(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task ActualizarAlquilerAsync(Alquiler alquiler)
        {
            var sql = @"UPDATE Alquiler SET Ncuenta = @Ncuenta, Nmaquina = @Nmaquina, tipo = @Tipo,
                fechaInicio = @FechaInicio, fechaFin = @FechaFin, tiempoContratado = @TiempoContratado,
                tiempoUsado = @TiempoUsado, precioPorHora = @PrecioPorHora, totalAPagar = @TotalAPagar,
                montoPagado = @MontoPagado, estado = @Estado, sesionActiva = @SesionActiva
                WHERE idAlquiler = @IdAlquiler";
            await _dbConnection.ExecuteAsync(sql, alquiler);
        }

        public async Task EliminarAlquilerAsync(int idAlquiler)
        {
            var sql = "DELETE FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            await _dbConnection.ExecuteAsync(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileresAsync()
        {
            var sql = "SELECT * FROM Alquiler";
            return await _dbConnection.QueryAsync<Alquiler>(sql);
        }

        public async Task<IEnumerable<Alquiler>> ObtenerAlquileresPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT * FROM Alquiler WHERE Ncuenta = @Ncuenta";
            return await _dbConnection.QueryAsync<Alquiler>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<IEnumerable<Alquiler>> ObtenerAlquileresPorMaquinaAsync(int nmaquina)
        {
            var sql = "SELECT * FROM Alquiler WHERE Nmaquina = @Nmaquina";
            return await _dbConnection.QueryAsync<Alquiler>(sql, new { Nmaquina = nmaquina });
        }

        public async Task<Alquiler> ObtenerAlquilerActivoPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT * FROM Alquiler WHERE Ncuenta = @Ncuenta AND sesionActiva = true";
            return await _dbConnection.QueryFirstOrDefaultAsync<Alquiler>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<Alquiler> ObtenerAlquilerActivoPorMaquinaAsync(int nmaquina)
        {
            var sql = "SELECT * FROM Alquiler WHERE Nmaquina = @Nmaquina AND sesionActiva = true";
            return await _dbConnection.QueryFirstOrDefaultAsync<Alquiler>(sql, new { Nmaquina = nmaquina });
        }

        public async Task<bool> TieneAlquilerActivoAsync(int ncuenta)
        {
            var sql = "SELECT COUNT(*) FROM Alquiler WHERE Ncuenta = @Ncuenta AND sesionActiva = true";
            var count = await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, new { Ncuenta = ncuenta });
            return count > 0;
        }

        public async Task<TimeSpan> ObtenerTiempoUsoActualAsync(int idAlquiler)
        {
            var sql = "SELECT TIMEDIFF(NOW(), fechaInicio) FROM Alquiler WHERE idAlquiler = @IdAlquiler";
            return await _dbConnection.QueryFirstOrDefaultAsync<TimeSpan>(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task<decimal> ObtenerCostoActualAlquilerAsync(int idAlquiler)
        {
            var alquiler = await ObtenerAlquilerPorIdAsync(idAlquiler);
            if (alquiler == null) return 0;

            var tiempoUso = await ObtenerTiempoUsoActualAsync(idAlquiler);
            return (decimal)tiempoUso.TotalHours * alquiler.PrecioPorHora;
        }

        public async Task AgregarHistorialAsync(HistorialdeAlquiler historial)
        {
            var sql = @"INSERT INTO HistorialdeAlquiler (idAlquiler, Ncuenta, Nmaquina, fechaInicio, fechaFin,
                tiempoContratado, tiempoUsado, precioPorHora, TotalPagar, montoPagado, estadoFinal, observaciones)
                VALUES (@IdAlquiler, @Ncuenta, @Nmaquina, @FechaInicio, @FechaFin, @TiempoContratado,
                @TiempoUsado, @PrecioPorHora, @TotalPagar, @MontoPagado, @EstadoFinal, @Observaciones);
                SELECT LAST_INSERT_ID();";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, historial);
            historial.IdHistorial = id;
        }

        public async Task<HistorialdeAlquiler> ObtenerHistorialPorIdAsync(int idHistorial)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE idHistorial = @IdHistorial";
            return await _dbConnection.QueryFirstOrDefaultAsync<HistorialdeAlquiler>(sql, new { IdHistorial = idHistorial });
        }

        public async Task ActualizarHistorialAsync(HistorialdeAlquiler historial)
        {
            var sql = @"UPDATE HistorialdeAlquiler SET idAlquiler = @IdAlquiler, Ncuenta = @Ncuenta, Nmaquina = @Nmaquina,
                fechaInicio = @FechaInicio, fechaFin = @FechaFin, tiempoContratado = @TiempoContratado,
                tiempoUsado = @TiempoUsado, precioPorHora = @PrecioPorHora, TotalPagar = @TotalPagar,
                montoPagado = @MontoPagado, estadoFinal = @EstadoFinal, observaciones = @Observaciones
                WHERE idHistorial = @IdHistorial";
            await _dbConnection.ExecuteAsync(sql, historial);
        }

        public async Task EliminarHistorialAsync(int idHistorial)
        {
            var sql = "DELETE FROM HistorialdeAlquiler WHERE idHistorial = @IdHistorial";
            await _dbConnection.ExecuteAsync(sql, new { IdHistorial = idHistorial });
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorialAsync()
        {
            var sql = "SELECT * FROM HistorialdeAlquiler ORDER BY fechaFin DESC";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql);
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE Ncuenta = @Ncuenta ORDER BY fechaFin DESC";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorMaquinaAsync(int nmaquina)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE Nmaquina = @Nmaquina ORDER BY fechaFin DESC";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql, new { Nmaquina = nmaquina });
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorFechaAsync(DateTime fecha)
        {
            var sql = "SELECT * FROM HistorialdeAlquiler WHERE DATE(fechaFin) = @Fecha ORDER BY fechaFin DESC";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql, new { Fecha = fecha.Date });
        }

        public async Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            var sql = @"SELECT * FROM HistorialdeAlquiler 
                WHERE fechaFin BETWEEN @Inicio AND @Fin 
                ORDER BY fechaFin DESC";
            return await _dbConnection.QueryAsync<HistorialdeAlquiler>(sql, new { Inicio = inicio, Fin = fin });
        }

        public async Task<decimal> ObtenerIngresosPorFechaAsync(DateTime fecha)
        {
            var sql = "SELECT COALESCE(SUM(TotalPagar), 0) FROM HistorialdeAlquiler WHERE DATE(fechaFin) = @Fecha";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { Fecha = fecha.Date });
        }

        public async Task<decimal> ObtenerIngresosPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            var sql = "SELECT COALESCE(SUM(TotalPagar), 0) FROM HistorialdeAlquiler WHERE fechaFin BETWEEN @Inicio AND @Fin";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }

        public async Task AgregarTransaccionAsync(Transaccion transaccion)
        {
            var sql = @"INSERT INTO Transacciones (Ncuenta, tipo, monto, idAlquiler, descripcion)
                VALUES (@Ncuenta, @Tipo, @Monto, @IdAlquiler, @Descripcion);
                SELECT LAST_INSERT_ID();";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, transaccion);
            transaccion.IdTransaccion = id;
        }

        public async Task<Transaccion> ObtenerTransaccionPorIdAsync(int idTransaccion)
        {
            var sql = "SELECT * FROM Transacciones WHERE idTransaccion = @IdTransaccion";
            return await _dbConnection.QueryFirstOrDefaultAsync<Transaccion>(sql, new { IdTransaccion = idTransaccion });
        }

        public async Task ActualizarTransaccionAsync(Transaccion transaccion)
        {
            var sql = @"UPDATE Transacciones SET Ncuenta = @Ncuenta, tipo = @Tipo, monto = @Monto,
                idAlquiler = @IdAlquiler, descripcion = @Descripcion
                WHERE idTransaccion = @IdTransaccion";
            await _dbConnection.ExecuteAsync(sql, transaccion);
        }

        public async Task EliminarTransaccionAsync(int idTransaccion)
        {
            var sql = "DELETE FROM Transacciones WHERE idTransaccion = @IdTransaccion";
            await _dbConnection.ExecuteAsync(sql, new { IdTransaccion = idTransaccion });
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTodasLasTransaccionesAsync()
        {
            var sql = "SELECT * FROM Transacciones ORDER BY fecha DESC";
            return await _dbConnection.QueryAsync<Transaccion>(sql);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT * FROM Transacciones WHERE Ncuenta = @Ncuenta ORDER BY fecha DESC";
            return await _dbConnection.QueryAsync<Transaccion>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorTipoAsync(string tipo)
        {
            var sql = "SELECT * FROM Transacciones WHERE tipo = @Tipo ORDER BY fecha DESC";
            return await _dbConnection.QueryAsync<Transaccion>(sql, new { Tipo = tipo });
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorAlquilerAsync(int idAlquiler)
        {
            var sql = "SELECT * FROM Transacciones WHERE idAlquiler = @IdAlquiler ORDER BY fecha DESC";
            return await _dbConnection.QueryAsync<Transaccion>(sql, new { IdAlquiler = idAlquiler });
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorFechaAsync(DateTime fecha)
        {
            var sql = "SELECT * FROM Transacciones WHERE DATE(fecha) = @Fecha ORDER BY fecha DESC";
            return await _dbConnection.QueryAsync<Transaccion>(sql, new { Fecha = fecha.Date });
        }

        public async Task<decimal> ObtenerTotalRecargasPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT COALESCE(SUM(monto), 0) FROM Transacciones WHERE Ncuenta = @Ncuenta AND tipo = 'Recarga'";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<decimal> ObtenerTotalConsumosPorCuentaAsync(int ncuenta)
        {
            var sql = "SELECT COALESCE(SUM(monto), 0) FROM Transacciones WHERE Ncuenta = @Ncuenta AND tipo = 'Consumo'";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { Ncuenta = ncuenta });
        }

        public async Task<int> IniciarAlquilerLibreAsync(int ncuenta, int nmaquina)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("unNmaquina", nmaquina);
            parameters.Add("unTipo", 1);
            parameters.Add("tiempoContratado", null);
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("idAlquilerCreado");
        }

        public async Task<int> IniciarAlquilerTiempoDefinidoAsync(int ncuenta, int nmaquina, TimeSpan tiempo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unNcuenta", ncuenta);
            parameters.Add("unNmaquina", nmaquina);
            parameters.Add("unTipo", 2);
            parameters.Add("tiempoContratado", tiempo.ToString(@"hh\:mm\:ss"));
            parameters.Add("idAlquilerCreado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync("IniciarAlquiler", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("idAlquilerCreado");
        }

        public async Task FinalizarAlquilerCompletoAsync(int idAlquiler)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdAlquiler", idAlquiler);
            await _dbConnection.ExecuteAsync("FinalizarAlquiler", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> VerificarSaldoSuficienteAsync(int ncuenta, TimeSpan tiempoSolicitado, int nmaquina)
        {
            var maquina = await ObtenerMaquinaPorIdAsync(nmaquina);
            var saldo = await ObtenerSaldoCuentaAsync(ncuenta);
            var costo = (decimal)tiempoSolicitado.TotalHours * maquina.PrecioPorHora;
            return saldo >= costo;
        }

        public async Task<decimal> CalcularCostoTiempoAsync(TimeSpan tiempo, decimal precioPorHora)
        {
            return await Task.FromResult((decimal)tiempo.TotalHours * precioPorHora);
        }

        public async Task<IEnumerable<Maquina>> ObtenerMaquinasPorTipoAsync(string tipoMaquina)
        {
            var sql = "SELECT * FROM Maquina WHERE tipoMaquina = @TipoMaquina";
            return await _dbConnection.QueryAsync<Maquina>(sql, new { TipoMaquina = tipoMaquina });
        }

        public async Task<Dictionary<string, decimal>> ObtenerEstadisticasDiariasAsync(DateTime fecha)
        {
            var sql = @"
        SELECT 
            COUNT(*) as TotalAlquileres,
            COALESCE(SUM(TotalPagar), 0) as IngresosTotales,
            AVG(TIME_TO_SEC(tiempoUsado)/3600) as PromedioHoras
        FROM HistorialdeAlquiler 
        WHERE DATE(fechaFin) = @Fecha";

            var result = await _dbConnection.QueryFirstOrDefaultAsync(sql, new { Fecha = fecha.Date });

            return new Dictionary<string, decimal>
    {
        { "TotalAlquileres", result?.TotalAlquileres ?? 0 },
        { "IngresosTotales", result?.IngresosTotales ?? 0 },
        { "PromedioHoras", result?.PromedioHoras ?? 0 }
    };
        }

        public async Task<IEnumerable<dynamic>> ObtenerTopClientesAsync(int cantidad)
        {
            var sql = @"
        SELECT c.nombre, c.dni, COUNT(*) as TotalAlquileres, SUM(h.TotalPagar) as TotalGastado
        FROM HistorialdeAlquiler h
        JOIN Cuenta c ON h.Ncuenta = c.Ncuenta
        GROUP BY c.Ncuenta, c.nombre, c.dni
        ORDER BY TotalGastado DESC
        LIMIT @Cantidad";

            return await _dbConnection.QueryAsync(sql, new { Cantidad = cantidad });
        }

        public async Task<IEnumerable<dynamic>> ObtenerMaquinasMasRentablesAsync()
        {
            var sql = @"
        SELECT m.Nmaquina, m.caracteristicas, m.tipoMaquina,
               COUNT(*) as VecesAlquilada, 
               COALESCE(SUM(h.TotalPagar), 0) as IngresosGenerados
        FROM Maquina m
        LEFT JOIN HistorialdeAlquiler h ON m.Nmaquina = h.Nmaquina
        GROUP BY m.Nmaquina, m.caracteristicas, m.tipoMaquina
        ORDER BY IngresosGenerados DESC";

            return await _dbConnection.QueryAsync(sql);
        }

        // ========== VALIDACIÓN DE SALDO Y AUTO-FINALIZACIÓN ==========
        public async Task<bool> VerificarYFinalizarSiExcedeSaldoAsync(int idAlquiler)
        {
            var alquiler = await ObtenerAlquilerPorIdAsync(idAlquiler);
            if (alquiler == null || !alquiler.SesionActiva)
                return false;

            var saldo = await ObtenerSaldoCuentaAsync(alquiler.Ncuenta);
            var costoActual = await ObtenerCostoActualAlquilerAsync(idAlquiler);

            // Si el costo actual excede el saldo, finalizar automáticamente
            if (costoActual > saldo)
            {
                await FinalizarAlquilerCompletoAsync(idAlquiler);
                return true; // Fue finalizado por exceso de saldo
            }

            return false; // No fue finalizado
        }

        public async Task<IEnumerable<Alquiler>> ObtenerAlquileresQueExcedenSaldoAsync()
        {
            var alquileresActivos = await ObtenerAlquileresActivosAsync();
            var alquileresExcedidos = new List<Alquiler>();

            foreach (var alquiler in alquileresActivos)
            {
                var saldo = await ObtenerSaldoCuentaAsync(alquiler.Ncuenta);
                var costoActual = await ObtenerCostoActualAlquilerAsync(alquiler.IdAlquiler);

                if (costoActual > saldo)
                {
                    alquileresExcedidos.Add(alquiler);
                }
            }

            return alquileresExcedidos;
        }

        public async Task FinalizarAlquileresExcedidosAsync()
        {
            var alquileresExcedidos = await ObtenerAlquileresQueExcedenSaldoAsync();
            foreach (var alquiler in alquileresExcedidos)
            {
                await FinalizarAlquilerCompletoAsync(alquiler.IdAlquiler);
            }
        }
    }

}