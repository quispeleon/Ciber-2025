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

        // ✅ POST: Confirmar eliminación
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int nmaquina)
        {
            var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
            if (maquina == null)
            {
                return NotFound();
            }

            await _iDAO.EliminarMaquinaAsync(nmaquina);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
