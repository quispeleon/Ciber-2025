using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class HistorialController : Controller
    {
        private readonly IDAO _dao;

        public HistorialController(IDAO dao)
        {
            _dao = dao;
        }

        // GET: Historial
        public async Task<IActionResult> Index()
        {
            var historial = await _dao.ObtenerTodoElHistorialAsync();
            return View(historial);
        }

        // GET: Historial/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var historial = await _dao.ObtenerHistorialPorIdAsync(id);
            if (historial == null)
            {
                return NotFound();
            }
            return View(historial);
        }

        // GET: Historial/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var historial = await _dao.ObtenerHistorialPorIdAsync(id);
            if (historial == null)
            {
                return NotFound();
            }
            return View(historial);
        }

        // POST: Historial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _dao.EliminarHistorialAsync(id);
                TempData["SuccessMessage"] = "Registro de historial eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el registro: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Historial/Reporte
        public async Task<IActionResult> Reporte()
        {
            var reporte = new ReporteHistorialViewModel
            {
                TotalRegistros = (await _dao.ObtenerTodoElHistorialAsync()).Count(),
                IngresosTotales = await _dao.ObtenerIngresosPorRangoFechasAsync(
                    DateTime.Today.AddMonths(-1), DateTime.Today),
                PromedioTiempoUso = await CalcularPromedioTiempoUsoAsync()
            };
            return View(reporte);
        }

        // POST: Historial/Buscar
        [HttpPost]
        public async Task<IActionResult> Buscar(string tipo, string valor, DateTime? fechaInicio, DateTime? fechaFin)
        {
            IEnumerable<HistorialdeAlquiler> resultados;

            switch (tipo)
            {
                case "cuenta":
                    if (int.TryParse(valor, out int cuentaId))
                    {
                        resultados = await _dao.ObtenerHistorialPorCuentaAsync(cuentaId);
                    }
                    else
                    {
                        resultados = new List<HistorialdeAlquiler>();
                    }
                    break;
                case "maquina":
                    if (int.TryParse(valor, out int maquinaId))
                    {
                        resultados = await _dao.ObtenerHistorialPorMaquinaAsync(maquinaId);
                    }
                    else
                    {
                        resultados = new List<HistorialdeAlquiler>();
                    }
                    break;
                case "fecha":
                    if (fechaInicio.HasValue && fechaFin.HasValue)
                    {
                        resultados = await _dao.ObtenerHistorialPorRangoFechasAsync(fechaInicio.Value, fechaFin.Value);
                    }
                    else
                    {
                        resultados = new List<HistorialdeAlquiler>();
                    }
                    break;
                default:
                    resultados = await _dao.ObtenerTodoElHistorialAsync();
                    break;
            }

            ViewBag.TipoBusqueda = tipo;
            ViewBag.ValorBusqueda = valor;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            return View("Index", resultados);
        }
        private async Task<TimeSpan> CalcularPromedioTiempoUsoAsync()
        {
            var historial = await _dao.ObtenerTodoElHistorialAsync();
            if (!historial.Any()) return TimeSpan.Zero;

            var itemsConTiempo = historial.Where(h => h.TiempoUsado.HasValue).ToList();
            if (!itemsConTiempo.Any()) return TimeSpan.Zero;
            
            var totalSegundos = itemsConTiempo.Average(h => h.TiempoUsado!.Value.TotalSeconds);

            return TimeSpan.FromSeconds(totalSegundos);
        }
    }

    public class ReporteHistorialViewModel
    {
        public int TotalRegistros { get; set; }
        public decimal IngresosTotales { get; set; }
        public TimeSpan PromedioTiempoUso { get; set; }
    }
}