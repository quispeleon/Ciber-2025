using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IDAO _dao;

        public ReportesController(IDAO dao)
        {
            _dao = dao;
        }

        public async Task<IActionResult> Index()
        {
            var reporte = new ReporteViewModel
            {
                IngresosHoy = await _dao.ObtenerIngresosPorFechaAsync(DateTime.Today),
                IngresosSemana = await _dao.ObtenerIngresosPorRangoFechasAsync(
                    DateTime.Today.AddDays(-7), DateTime.Today),
                TotalClientes = (await _dao.ObtenerTodasLasCuentasAsync()).Count(),
                MaquinasDisponibles = await _dao.ObtenerCantidadMaquinasDisponiblesAsync()
            };
            return View(reporte);
        }

        public async Task<IActionResult> TopClientes()
        {
            var topClientes = await _dao.ObtenerTopClientesAsync(10);
            return View(topClientes);
        }

        public async Task<IActionResult> MaquinasRentables()
        {
            var maquinas = await _dao.ObtenerMaquinasMasRentablesAsync();
            return View(maquinas);
        }

        public async Task<IActionResult> Transacciones()
        {
            var transacciones = await _dao.ObtenerTodasLasTransaccionesAsync();
            return View(transacciones);
        }

        [HttpPost]
        public async Task<IActionResult> ReportePorFecha(DateTime fecha)
        {
            var ingresos = await _dao.ObtenerIngresosPorFechaAsync(fecha);
            var historial = await _dao.ObtenerHistorialPorFechaAsync(fecha);
            
            ViewBag.Fecha = fecha;
            ViewBag.Ingresos = ingresos;
            
            return View("ReporteDetallado", historial);
        }
    }

    public class ReporteViewModel
    {
        public decimal IngresosHoy { get; set; }
        public decimal IngresosSemana { get; set; }
        public int TotalClientes { get; set; }
        public int MaquinasDisponibles { get; set; }
    }
}