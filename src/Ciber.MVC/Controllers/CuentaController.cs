using Microsoft.AspNetCore.Mvc;   // Para Controller, IActionResult, atributos como HttpPost, ValidateAntiForgeryToken, ActionName
using Ciber.core;                 // Para IDAO, Cuenta y demás modelos y interfaces de tu proyecto
using Microsoft.Extensions.Logging;  // Para ILogger si lo usas
using System.Threading.Tasks;         // Para usar Task y async/await
 
public class CuentaController : Controller
{
    private readonly IDAO _iDAO;
    private readonly ILogger<CuentaController> _logger;

    public CuentaController(ILogger<CuentaController> logger, IDAO iDAO)
    {
        _iDAO = iDAO;
        _logger = logger;
    }

    // GET: Mostrar formulario para registrarse
    public IActionResult Registrarte()
    {
        return View();
    }

    // POST: Crear nueva cuenta
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrarte(Cuenta cuenta)
    {
        if (!ModelState.IsValid)
        {
            return View(cuenta); // Devuelve la vista con los errores
        }

        // Asignar la hora actual
        cuenta.HoraRegistrada = DateTime.Now.TimeOfDay;

        await _iDAO.AgregarCuentaAsync(cuenta);

        return RedirectToAction(nameof(Index));
    }

    // GET: Mostrar lista de cuentas
    public async Task<IActionResult> Index()
    {
        var cuentas = await _iDAO.ObtenerTodasLasCuentasAsync();
        return View(cuentas);
    }

    // GET: Confirmar eliminación
    public async Task<IActionResult> Eliminar(int ncuenta)
    {
        var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
        if (cuenta == null)
        {
            return NotFound();
        }
        return View(cuenta);
    }

    // POST: Eliminar cuenta confirmada
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
        return RedirectToAction(nameof(Index));
    }
}
