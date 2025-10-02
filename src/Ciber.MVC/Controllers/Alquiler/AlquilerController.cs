using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ciber.MVC.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IDAO _iDAO;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ILogger<AlquilerController> logger, IDAO iDAO)
        {
            _iDAO = iDAO;
            _logger = logger;
        }

        // GET: Mostrar todos los alquileres
        public async Task<IActionResult> Index()
        {
            IEnumerable<Alquiler> alquileres = await _iDAO.ObtenerTodosLosAlquileresAsync();
            return View(alquileres);
        }

        // GET: Mostrar formulario para crear alquiler
        public IActionResult Crear()
        {
            // Le indicamos el nombre exacto de la vista
            return View("AltaAlquiler");
        }

        // POST: Crear nuevo alquiler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Alquiler alquiler, bool tipoAlquiler)
        {
            if (!ModelState.IsValid)
                return View("AltaAlquiler", alquiler); // Vista con modelo para mostrar errores

            await _iDAO.AgregarAlquilerAsync(alquiler, tipoAlquiler);
            return RedirectToAction(nameof(Index));
        }

        // GET: Confirmar eliminación
        public async Task<IActionResult> Eliminar(int idAlquiler)
        {
            var alquiler = await _iDAO.ObtenerAlquilerPorIdAsync(idAlquiler);
            if (alquiler == null) return NotFound();

            return View("EliminarAlquiler", alquiler);
        }

        // POST: Eliminar confirmado
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int idAlquiler)
        {
            await _iDAO.EliminarAlquilerAsync(idAlquiler);
            return RedirectToAction(nameof(Index));
        }
    }
}
