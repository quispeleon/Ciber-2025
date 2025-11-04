using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
            return View(cuenta);
        }

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

    // GET: Cuenta/Editar/12345
    public async Task<IActionResult> Editar(int ncuenta)
    {
        var cuenta = await _iDAO.ObtenerCuentaPorIdAsync(ncuenta);
        if (cuenta == null)
        {
            return NotFound();
        }
        return View(cuenta);
    }

    // POST: Cuenta/Editar/12345
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int ncuenta, Cuenta cuenta)
    {
        if (ncuenta != cuenta.Ncuenta)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _iDAO.ActualizarCuentaAsync(cuenta);
            return RedirectToAction(nameof(Index));
        }

        return View(cuenta);
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

