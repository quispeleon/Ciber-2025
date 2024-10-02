

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
}
