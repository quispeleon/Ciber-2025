using Microsoft.AspNetCore.Mvc;
using Ciber.core;
namespace Ciber.MVC.Controllers;

public class CuentaController : Controller
{
    private readonly IDAO _iDAO;

    private readonly ILogger<CuentaController> _logger;

    public CuentaController(ILogger<CuentaController> logger, IDAO iDAO)
    {
        _iDAO = iDAO;
        _logger = logger;
    }
    public async Task<IActionResult> Registrarte()
    {
        return View(); // Renderiza la vista 'Registrarte.cshtml'
    }
    [HttpPost]
    public async Task<IActionResult> Registrarte(Cuenta cuenta)
    {
        if (cuenta == null)
        {
            return BadRequest("La cuenta es nula.");
        }

        // Asigna la hora actual a la propiedad HoraRegistrada
        cuenta.HoraRegistrada = DateTime.Now.TimeOfDay; // Asignar la hora del sistema al momento del registro

        // Llama al método de la interfaz para agregar la cuenta
        await _iDAO.AgregarCuentaAsync(cuenta);

        // Redirige al Index para mostrar todas las cuentas
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Index()
    {
        var cuentas = await _iDAO.ObtenerTodasLasCuentasAsync();
        return View(cuentas);
    }
    
    // Acción para mostrar la confirmación de la eliminación (GET)
    public async Task<IActionResult> Eliminar(int ncuenta)
    {
        // Busca la cuenta por ID (o DNI si lo prefieres)
        var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
        
        if (cuenta == null)
        {
            return NotFound();  // Si no se encuentra la cuenta, devolver error 404
        }
        
        return View(cuenta);  // Devuelve la vista de confirmación con la cuenta
    }

    // Acción para eliminar la cuenta (POST)
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int ncuenta)
    {
    // Busca la cuenta a eliminar
    var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
    
    if (cuenta == null)
    {
        return NotFound();  // Si no se encuentra la cuenta, devolver error 404
    }
    
    // Llama al método de eliminación en el DAO
    await _iDAO.EliminarCuentaAsync(ncuenta);
    
    // Redirige al índice para mostrar todas las cuentas restantes
    return RedirectToAction("Index");
}
}