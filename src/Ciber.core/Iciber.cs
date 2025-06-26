namespace Ciber.core
{
    public interface IDAO
    {
        // Cuenta
        Task AgregarCuenta(Cuenta cuenta);
        Task<Cuenta> ObtenerCuentaPorId(int ncuenta);
        Task ActualizarCuenta(Cuenta cuenta);
        Task EliminarCuenta(int ncuenta);
        Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentas();

        // Maquina
        Task AgregarMaquina(Maquina maquina);
        Task<Maquina> ObtenerMaquinaPorId(int nmaquina);
        Task ActualizarMaquina(Maquina maquina);
        Task EliminarMaquina(int nmaquina);
        Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinas();

        // Tipo
        Task AgregarTipo(Tipo tipo);

        // Alquiler
        Task AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler);
        Task<Alquiler> ObtenerAlquilerPorId(int idAlquiler);
        Task EliminarAlquiler(int idAlquiler);
        Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileres();

        // Historial
        Task AgregarHistorial(HistorialdeAlquiler historial);
        Task<HistorialdeAlquiler> ObtenerHistorialPorId(int idHistorial);
        Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorial();
    }
}