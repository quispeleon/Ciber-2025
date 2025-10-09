using Microsoft.AspNetCore.Mvc;
using Ciber.MVC.Models;
using Ciber.core; // Para acceder a Maquina
using System.Linq;

namespace Ciber.MVC.Controllers
{
    public class EquipoController : Controller
    {
        private readonly IDAO _iDAO;

        public EquipoController(IDAO iDAO)
        {
            _iDAO = iDAO;
        }

        // GET: Lista de equipos
        public IActionResult Index()
        {
            var equiposCore = _iDAO.ObtenerTodasLasMaquinas(); // IEnumerable<Maquina>
            var equipos = equiposCore.Select(e => new Equipo
            {
                Nmaquina = e.Nmaquina,
                Nombre = e.Caracteristicas, // Mapear a Nombre para la vista
                TipoEquipo = e.Caracteristicas, // O si tenés un campo específico
                Estado = e.Estado
            });

            return View(equipos);
        }

        // GET: Crear equipo
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Crear nuevo equipo
        [HttpPost]
        public IActionResult Crear(Equipo equipo)
        {
            if (!ModelState.IsValid)
                return View(equipo);

            // Convertimos a core
            var maquinaCore = new Maquina
            {
                Nmaquina = equipo.Nmaquina,
                Caracteristicas = equipo.Nombre, // o TipoEquipo según tu lógica
                Estado = equipo.Estado
            };

            _iDAO.AgregarMaquina(maquinaCore);

            return RedirectToAction(nameof(Index));
        }

        // GET: Eliminar equipo
        public IActionResult Eliminar(int id)
        {
            var equipoCore = _iDAO.ObtenerMaquinaPorId(id);
            if (equipoCore == null) return NotFound();

            var equipo = new Equipo
            {
                Nmaquina = equipoCore.Nmaquina,
                Nombre = equipoCore.Caracteristicas,
                TipoEquipo = equipoCore.Caracteristicas,
                Estado = equipoCore.Estado
            };

            return View(equipo);
        }

        // POST: Confirmar eliminación
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _iDAO.EliminarMaquina(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
