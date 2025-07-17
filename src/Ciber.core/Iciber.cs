namespace Ciber.core;

public interface IDAO
{
    void AgregarCuenta(Cuenta cuenta);
    Cuenta ObtenerCuentaPorId(int ncuenta);
    void ActualizarCuenta(Cuenta cuenta);
    void EliminarCuenta(int ncuenta);
    IEnumerable<Cuenta> ObtenerTodasLasCuentas();


    // Tabla maquina 
    void AgregarMaquina(Maquina maquina);
    Maquina ObtenerMaquinaPorId(int nmaquina);
    void ActualizarMaquina(Maquina maquina);
    void EliminarMaquina(int nmaquina);
    IEnumerable<Maquina> ObtenerTodasLasMaquinas();

    // tabla tipo 

    void AgregarTipo(Tipo tipo);
    // Alquiler 
    void AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler);
    Alquiler ObtenerAlquilerPorId(int idAlquiler);
   
    void EliminarAlquiler(int idAlquiler);
    IEnumerable<Alquiler> ObtenerTodosLosAlquileres();

// historial 
    void AgregarHistorial(HistorialdeAlquiler historial);
    HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial);
    IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial();
    // Alquilar maquina
 
    // Métodos asíncronos
    Task AgregarCuentaAsync(Cuenta cuenta);
    Task<Cuenta> ObtenerCuentaPorIdAsync(int ncuenta);
    Task ActualizarCuentaAsync(Cuenta cuenta);
    Task EliminarCuentaAsync(int ncuenta);
    Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentasAsync();

    Task AgregarMaquinaAsync(Maquina maquina);
    Task<Maquina> ObtenerMaquinaPorIdAsync(int nmaquina);
    Task ActualizarMaquinaAsync(Maquina maquina);
    Task EliminarMaquinaAsync(int nmaquina);
    Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinasAsync();

    Task AgregarTipoAsync(Tipo tipo);
    Task AgregarAlquilerAsync(Alquiler alquiler, bool tipoAlquiler);
    Task<Alquiler> ObtenerAlquilerPorIdAsync(int idAlquiler);
    Task EliminarAlquilerAsync(int idAlquiler);
    Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileresAsync();

    Task AgregarHistorialAsync(HistorialdeAlquiler historial);
    Task<HistorialdeAlquiler> ObtenerHistorialPorIdAsync(int idHistorial);
    Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorialAsync();

}

