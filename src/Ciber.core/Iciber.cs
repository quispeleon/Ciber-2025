namespace Ciber.core;

public interface IDAO
{
    // ========== CUENTA ==========
    void AgregarCuenta(Cuenta cuenta);
    Cuenta ObtenerCuentaPorId(int ncuenta);
    void ActualizarCuenta(Cuenta cuenta);
    void EliminarCuenta(int ncuenta);
    IEnumerable<Cuenta> ObtenerTodasLasCuentas();
    Cuenta ObtenerCuentaPorDni(string dni);
    IEnumerable<Cuenta> ObtenerCuentasActivas();
    void RecargarSaldo(int ncuenta, decimal monto);
    decimal ObtenerSaldoCuenta(int ncuenta);
    bool ValidarCredenciales(string dni, string password);

    // ========== MÁQUINA ==========
    void AgregarMaquina(Maquina maquina);
    Maquina ObtenerMaquinaPorId(int nmaquina);
    void ActualizarMaquina(Maquina maquina);
    void EliminarMaquina(int nmaquina);
    IEnumerable<Maquina> ObtenerTodasLasMaquinas();
    IEnumerable<Maquina> ObtenerMaquinasDisponibles();
    IEnumerable<Maquina> ObtenerMaquinasOcupadas();
    IEnumerable<Maquina> ObtenerMaquinasEnMantenimiento();
    void CambiarEstadoMaquina(int nmaquina, string nuevoEstado);
    int ObtenerCantidadMaquinasDisponibles();

    // ========== TIPO ==========
    void AgregarTipo(Tipo tipo);
    Tipo ObtenerTipoPorId(int idTipo);
    void ActualizarTipo(Tipo tipo);
    void EliminarTipo(int idTipo);
    IEnumerable<Tipo> ObtenerTodosLosTipos();
    Tipo ObtenerTipoLibre();
    Tipo ObtenerTipoHoraDefinida();

    // ========== ALQUILER ==========
    void AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler);
    Alquiler ObtenerAlquilerPorId(int idAlquiler);
    void ActualizarAlquiler(Alquiler alquiler);
    void EliminarAlquiler(int idAlquiler);
    IEnumerable<Alquiler> ObtenerTodosLosAlquileres();
    IEnumerable<Alquiler> ObtenerAlquileresActivos();
    IEnumerable<Alquiler> ObtenerAlquileresPorCuenta(int ncuenta);
    IEnumerable<Alquiler> ObtenerAlquileresPorMaquina(int nmaquina);
    Alquiler ObtenerAlquilerActivoPorCuenta(int ncuenta);
    Alquiler ObtenerAlquilerActivoPorMaquina(int nmaquina);
    void FinalizarAlquiler(int idAlquiler, decimal montoPagado);
    bool TieneAlquilerActivo(int ncuenta);
    TimeSpan ObtenerTiempoUsoActual(int idAlquiler);
    decimal ObtenerCostoActualAlquiler(int idAlquiler);

    // ========== HISTORIAL ==========
    void AgregarHistorial(HistorialdeAlquiler historial);
    HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial);
    void ActualizarHistorial(HistorialdeAlquiler historial);
    void EliminarHistorial(int idHistorial);
    IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial();
    IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorCuenta(int ncuenta);
    IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorMaquina(int nmaquina);
    IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorFecha(DateTime fecha);
    IEnumerable<HistorialdeAlquiler> ObtenerHistorialPorRangoFechas(DateTime inicio, DateTime fin);
    decimal ObtenerIngresosPorFecha(DateTime fecha);
    decimal ObtenerIngresosPorRangoFechas(DateTime inicio, DateTime fin);

    // ========== TRANSACCIONES ==========
    void AgregarTransaccion(Transaccion transaccion);
    Transaccion ObtenerTransaccionPorId(int idTransaccion);
    void ActualizarTransaccion(Transaccion transaccion);
    void EliminarTransaccion(int idTransaccion);
    IEnumerable<Transaccion> ObtenerTodasLasTransacciones();
    IEnumerable<Transaccion> ObtenerTransaccionesPorCuenta(int ncuenta);
    IEnumerable<Transaccion> ObtenerTransaccionesPorTipo(string tipo);
    IEnumerable<Transaccion> ObtenerTransaccionesPorAlquiler(int idAlquiler);
    IEnumerable<Transaccion> ObtenerTransaccionesPorFecha(DateTime fecha);
    decimal ObtenerTotalRecargasPorCuenta(int ncuenta);
    decimal ObtenerTotalConsumosPorCuenta(int ncuenta);

    // ========== MÉTODOS DE NEGOCIO ESPECÍFICOS ==========
    int IniciarAlquilerLibre(int ncuenta, int nmaquina);
    int IniciarAlquilerTiempoDefinido(int ncuenta, int nmaquina, TimeSpan tiempo);
    void FinalizarAlquilerCompleto(int idAlquiler);
    bool VerificarSaldoSuficiente(int ncuenta, TimeSpan tiempoSolicitado, int nmaquina);
    decimal CalcularCostoTiempo(TimeSpan tiempo, decimal precioPorHora);
    IEnumerable<Maquina> ObtenerMaquinasPorTipo(string tipoMaquina);
    Dictionary<string, decimal> ObtenerEstadisticasDiarias(DateTime fecha);
    IEnumerable<dynamic> ObtenerTopClientes(int cantidad);
    IEnumerable<dynamic> ObtenerMaquinasMasRentables();

    // ========== MÉTODOS ASÍNCRONOS ==========
    
    // CUENTA
    Task AgregarCuentaAsync(Cuenta cuenta);
    Task<Cuenta> ObtenerCuentaPorIdAsync(int ncuenta);
    Task ActualizarCuentaAsync(Cuenta cuenta);
    Task EliminarCuentaAsync(int ncuenta);
    Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentasAsync();
    Task<Cuenta> ObtenerCuentaPorDniAsync(string dni);
    Task<IEnumerable<Cuenta>> ObtenerCuentasActivasAsync();
    Task RecargarSaldoAsync(int ncuenta, decimal monto);
    Task<decimal> ObtenerSaldoCuentaAsync(int ncuenta);
    Task<bool> ValidarCredencialesAsync(string dni, string password);

    // MÁQUINA
    Task AgregarMaquinaAsync(Maquina maquina);
    Task<Maquina> ObtenerMaquinaPorIdAsync(int nmaquina);
    Task ActualizarMaquinaAsync(Maquina maquina);
    Task EliminarMaquinaAsync(int nmaquina);
    Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinasAsync();
    Task<IEnumerable<Maquina>> ObtenerMaquinasDisponiblesAsync();
    Task<IEnumerable<Maquina>> ObtenerMaquinasOcupadasAsync();
    Task<IEnumerable<Maquina>> ObtenerMaquinasEnMantenimientoAsync();
    Task CambiarEstadoMaquinaAsync(int nmaquina, string nuevoEstado);
    Task<int> ObtenerCantidadMaquinasDisponiblesAsync();

    // TIPO
    Task AgregarTipoAsync(Tipo tipo);
    Task<Tipo> ObtenerTipoPorIdAsync(int idTipo);
    Task ActualizarTipoAsync(Tipo tipo);
    Task EliminarTipoAsync(int idTipo);
    Task<IEnumerable<Tipo>> ObtenerTodosLosTiposAsync();
    Task<Tipo> ObtenerTipoLibreAsync();
    Task<Tipo> ObtenerTipoHoraDefinidaAsync();

    // ALQUILER
    Task AgregarAlquilerAsync(Alquiler alquiler, bool tipoAlquiler);
    Task<Alquiler> ObtenerAlquilerPorIdAsync(int idAlquiler);
    Task ActualizarAlquilerAsync(Alquiler alquiler);
    Task EliminarAlquilerAsync(int idAlquiler);
    Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileresAsync();
    Task<IEnumerable<Alquiler>> ObtenerAlquileresActivosAsync();
    Task<IEnumerable<Alquiler>> ObtenerAlquileresPorCuentaAsync(int ncuenta);
    Task<IEnumerable<Alquiler>> ObtenerAlquileresPorMaquinaAsync(int nmaquina);
    Task<Alquiler> ObtenerAlquilerActivoPorCuentaAsync(int ncuenta);
    Task<Alquiler> ObtenerAlquilerActivoPorMaquinaAsync(int nmaquina);
    Task FinalizarAlquilerAsync(int idAlquiler, decimal montoPagado);
    Task<bool> TieneAlquilerActivoAsync(int ncuenta);
    Task<TimeSpan> ObtenerTiempoUsoActualAsync(int idAlquiler);
    Task<decimal> ObtenerCostoActualAlquilerAsync(int idAlquiler);

    // HISTORIAL
    Task AgregarHistorialAsync(HistorialdeAlquiler historial);
    Task<HistorialdeAlquiler> ObtenerHistorialPorIdAsync(int idHistorial);
    Task ActualizarHistorialAsync(HistorialdeAlquiler historial);
    Task EliminarHistorialAsync(int idHistorial);
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorialAsync();
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorCuentaAsync(int ncuenta);
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorMaquinaAsync(int nmaquina);
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorFechaAsync(DateTime fecha);
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerHistorialPorRangoFechasAsync(DateTime inicio, DateTime fin);
    Task<decimal> ObtenerIngresosPorFechaAsync(DateTime fecha);
    Task<decimal> ObtenerIngresosPorRangoFechasAsync(DateTime inicio, DateTime fin);

    // TRANSACCIONES
    Task AgregarTransaccionAsync(Transaccion transaccion);
    Task<Transaccion> ObtenerTransaccionPorIdAsync(int idTransaccion);
    Task ActualizarTransaccionAsync(Transaccion transaccion);
    Task EliminarTransaccionAsync(int idTransaccion);
    Task<IEnumerable<Transaccion>> ObtenerTodasLasTransaccionesAsync();
    Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorCuentaAsync(int ncuenta);
    Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorTipoAsync(string tipo);
    Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorAlquilerAsync(int idAlquiler);
    Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorFechaAsync(DateTime fecha);
    Task<decimal> ObtenerTotalRecargasPorCuentaAsync(int ncuenta);
    Task<decimal> ObtenerTotalConsumosPorCuentaAsync(int ncuenta);

    // MÉTODOS DE NEGOCIO ASÍNCRONOS
    Task<int> IniciarAlquilerLibreAsync(int ncuenta, int nmaquina);
    Task<int> IniciarAlquilerTiempoDefinidoAsync(int ncuenta, int nmaquina, TimeSpan tiempo);
    Task FinalizarAlquilerCompletoAsync(int idAlquiler);
    Task<bool> VerificarSaldoSuficienteAsync(int ncuenta, TimeSpan tiempoSolicitado, int nmaquina);
    Task<decimal> CalcularCostoTiempoAsync(TimeSpan tiempo, decimal precioPorHora);
    Task<IEnumerable<Maquina>> ObtenerMaquinasPorTipoAsync(string tipoMaquina);
    Task<Dictionary<string, decimal>> ObtenerEstadisticasDiariasAsync(DateTime fecha);
    Task<IEnumerable<dynamic>> ObtenerTopClientesAsync(int cantidad);
    Task<IEnumerable<dynamic>> ObtenerMaquinasMasRentablesAsync();

    // VALIDACIÓN DE SALDO Y AUTO-FINALIZACIÓN
    Task<bool> VerificarYFinalizarSiExcedeSaldoAsync(int idAlquiler);
    Task<IEnumerable<Alquiler>> ObtenerAlquileresQueExcedenSaldoAsync();
    Task FinalizarAlquileresExcedidosAsync();
}