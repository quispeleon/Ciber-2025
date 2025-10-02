using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ciber.MVC.Controllers
{
    public class EstadoMaquinaController : Controller
    {
        private readonly IDAO _iDAO;
        private readonly ILogger<EstadoMaquinaController> _logger;

        public EstadoMaquinaController(ILogger<EstadoMaquinaController> logger, IDAO iDAO)
        {
            _iDAO = iDAO;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int nmaquina)
        {
            var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(nmaquina);
            if (maquina == null)
                return NotFound();

            maquina.Estado = !maquina.Estado; // Cambia disponible <-> no disponible
            await _iDAO.ActualizarMaquinaAsync(maquina);

            return RedirectToAction("Index", "Maquina"); // Volver al listado de máquinas
        }
    }
}
