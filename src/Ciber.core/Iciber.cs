namespace Ciber.core
{
    public interface IDAO
    {
        //Sincronico
        
        // Cuenta
        void AgregarCuenta(Cuenta cuenta);
        Cuenta ObtenerCuentaPorId(int ncuenta);
        void ActualizarCuenta(Cuenta cuenta);
        void EliminarCuenta(int ncuenta);
        IEnumerable<Cuenta> ObtenerTodasLasCuentas();

        // Maquina
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

        //Async

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