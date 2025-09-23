namespace Ciber.core
{
    public interface IDAO
    {
        // ===========================
        // MÉTODOS SINCRÓNICOS
        // ===========================

        // Cuentas
        void AgregarCuenta(Cuenta cuenta);
        Cuenta ObtenerCuentaPorId(int ncuenta);
        void ActualizarCuenta(Cuenta cuenta);
        void EliminarCuenta(int ncuenta);
        IEnumerable<Cuenta> ObtenerTodasLasCuentas();

        // Maquinas
        void AgregarMaquina(Maquina maquina);
        Maquina ObtenerMaquinaPorId(int nmaquina);
        void ActualizarMaquina(Maquina maquina);
        void EliminarMaquina(int nmaquina);
        IEnumerable<Maquina> ObtenerTodasLasMaquinas();

        // Tipo
        void AgregarTipo(Tipo tipo);

        // Alquiler
        void AgregarAlquiler(Alquiler alquiler, bool tipoAlquiler);
        Alquiler ObtenerAlquilerPorId(int idAlquiler);
        void EliminarAlquiler(int idAlquiler);
        IEnumerable<Alquiler> ObtenerTodosLosAlquileres();

        // Historial
        void AgregarHistorial(HistorialdeAlquiler historial);
        HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial);
        IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial();

        // ===========================
        // MÉTODOS ASÍNCRONICOS
        // ===========================

        // Cuentas
        Task AgregarCuentaAsync(Cuenta cuenta);
        Task<Cuenta> ObtenerCuentaPorIdAsync(int ncuenta);
        Task ActualizarCuentaAsync(Cuenta cuenta);
        Task EliminarCuentaAsync(int ncuenta);
        Task<IEnumerable<Cuenta>> ObtenerTodasLasCuentasAsync();

        // Maquinas
        Task AgregarMaquinaAsync(Maquina maquina);
        Task<Maquina> ObtenerMaquinaPorIdAsync(int nmaquina);
        Task<IEnumerable<Maquina>> ObtenerMaquinaDisponiblesAsync();
        Task<IEnumerable<Maquina>> ObtenerMaquinaNoDisponiblesesAsync();
        Task ActualizarMaquinaAsync(Maquina maquina);
        Task EliminarMaquinaAsync(int nmaquina);
        Task<IEnumerable<Maquina>> ObtenerTodasLasMaquinasAsync();

        // Tipo
        Task AgregarTipoAsync(Tipo tipo);

        // Alquiler
        Task AgregarAlquilerAsync(Alquiler alquiler, bool tipoAlquiler);
        Task<Alquiler> ObtenerAlquilerPorIdAsync(int idAlquiler);
        Task EliminarAlquilerAsync(int idAlquiler);
        Task<IEnumerable<Alquiler>> ObtenerTodosLosAlquileresAsync();

        // Historial
        Task AgregarHistorialAsync(HistorialdeAlquiler historial);
        Task<HistorialdeAlquiler> ObtenerHistorialPorIdAsync(int idHistorial);
        Task<IEnumerable<HistorialdeAlquiler>> ObtenerTodoElHistorialAsync();
    }
}
