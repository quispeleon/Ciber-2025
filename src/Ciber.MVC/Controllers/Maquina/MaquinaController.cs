using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ciber.MVC.Controllers
{
    public class MaquinaController : Controller
    {
        private readonly IDAO _iDAO;
        private readonly ILogger<MaquinaController> _logger;

        public MaquinaController(ILogger<MaquinaController> logger, IDAO iDAO)
        {
            _iDAO = iDAO;
            _logger = logger;
        }

        // GET: Listar máquinas
        public async Task<IActionResult> Index()
        {
            var maquinas = await _iDAO.ObtenerTodasLasMaquinasAsync();
            return View(maquinas);
        }

        // ✅ GET: Mostrar formulario para registrar máquina (esto evita el NullReference)
        public IActionResult RegistrarMaquina()
        {
            var nuevaMaquina = new Maquina(); // Instancia vacía evita el error
            return View(nuevaMaquina);
        }

        // ✅ POST: Registrar nueva máquina
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarMaquina(Maquina maquina)
        {
            if (!ModelState.IsValid)
            {
                return View(maquina); // Devuelve con errores si los hay
            }

            await _iDAO.AgregarMaquinaAsync(maquina);
            return RedirectToAction(nameof(Index));
        }

        // ✅ GET: Confirmar eliminación
        public async Task<IActionResult> Eliminar(int nmaquina)
        {
            var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
            if (maquina == null)
            {
                return NotFound();
            }
            return View(maquina);
        }

// POST: Eliminar confirmado
[HttpPost, ActionName("Eliminar")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EliminarConfirmado(int idAlquiler)
{
    // 1️⃣ Obtener el alquiler para saber qué máquina estaba ocupando
    var alquiler = await _iDAO.ObtenerAlquilerPorIdAsync(idAlquiler);
    if (alquiler == null) return NotFound();

    // 2️⃣ Eliminar el alquiler
    await _iDAO.EliminarAlquilerAsync(idAlquiler);

    // 3️⃣ Liberar la máquina
    var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(alquiler.Nmaquina);
    if (maquina != null)
    {
        maquina.Estado = true; // Disponible
        await _iDAO.ActualizarMaquinaAsync(maquina);
    }

    return RedirectToAction(nameof(Index));
}

/// MUCHAAAAAAAAAAAAAAAA ATENCION
// GET: Mostrar formulario editar
public async Task<IActionResult> Edit(int nmaquina)
{
    var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
    if (maquina == null) return NotFound();
    return View(maquina); // Vista espera Ciber.core.Maquina
}

// POST: Guardar cambios de edición
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(Ciber.core.Maquina maquina)
{
    if (!ModelState.IsValid)
    {
        return View(maquina);
    }

    // opcional: cargar la máquina actual para evitar sobrescribir datos no editables
    var current = await _iDAO.ObtenerMaquinaPorIdAsync(maquina.Nmaquina);
    if (current == null) return NotFound();

    // aplicar cambios permitidos
    current.Caracteristicas = maquina.Caracteristicas;
    current.Estado = maquina.Estado; // true = disponible, false = ocupada/deshabilitada
    await _iDAO.ActualizarMaquinaAsync(current);

    return RedirectToAction("Index", "Maquina");
}

// POST: Deshabilitar (poner Estado = false) — acción pensada para botones rápidos
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Disable(int nmaquina)
{
    var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
    if (maquina == null) return NotFound();

    maquina.Estado = false;
    await _iDAO.ActualizarMaquinaAsync(maquina);

    return RedirectToAction("Index", "Maquina");
}

// POST: Habilitar (poner Estado = true)
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Enable(int nmaquina)
{
    var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
    if (maquina == null) return NotFound();

    maquina.Estado = true;
    await _iDAO.ActualizarMaquinaAsync(maquina);

    return RedirectToAction("Index", "Maquina");
}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
