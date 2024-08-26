

namespace Ciber.core;

public interface ICuentaRepository
{
    void AgregarCuenta(Cuenta cuenta);
    Cuenta ObtenerCuentaPorId(int ncuenta);
    void ActualizarCuenta(Cuenta cuenta);
    void EliminarCuenta(int ncuenta);
    IEnumerable<Cuenta> ObtenerTodasLasCuentas();
}

public interface IMaquinaRepository
{
    void AgregarMaquina(Maquina maquina);
    Maquina ObtenerMaquinaPorId(int nmaquina);
    void ActualizarMaquina(Maquina maquina);
    void EliminarMaquina(int nmaquina);
    IEnumerable<Maquina> ObtenerTodasLasMaquinas();
}

public interface ITipoRepository
{
    void AgregarTipo(Tipo tipo);
    Tipo ObtenerTipoPorId(int idTipo);
    void ActualizarTipo(Tipo tipo);
    void EliminarTipo(int idTipo);
    IEnumerable<Tipo> ObtenerTodosLosTipos();
}

public interface IAlquilerRepository
{
    void AgregarAlquiler(Alquiler alquiler);
    Alquiler ObtenerAlquilerPorId(int idAlquiler);
    void ActualizarAlquiler(Alquiler alquiler);
    void EliminarAlquiler(int idAlquiler);
    IEnumerable<Alquiler> ObtenerTodosLosAlquileres();
}

public interface IHistorialdeAlquilerRepository
{
    void AgregarHistorial(HistorialdeAlquiler historial);
    HistorialdeAlquiler ObtenerHistorialPorId(int idHistorial);
    void ActualizarHistorial(HistorialdeAlquiler historial);
    void EliminarHistorial(int idHistorial);
    IEnumerable<HistorialdeAlquiler> ObtenerTodoElHistorial();
}
