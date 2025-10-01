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

    public async Task<IActionResult> Eliminar(int ncuenta)
    {
        var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
        if (cuenta == null)
        {
            return NotFound();
        }
        return View(cuenta);
    }

    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int ncuenta)
    {
        var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
        if (cuenta == null)
        {
            return NotFound();
        }

        await _iDAO.EliminarCuentaAsync(ncuenta);
        return RedirectToAction("Index");
    }


}