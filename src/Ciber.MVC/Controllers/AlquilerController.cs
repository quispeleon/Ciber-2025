using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IDAO _dao;

        public AlquilerController(IDAO dao)
        {
            _dao = dao;
        }

        public async Task<IActionResult> Index()
        {
            var alquileres = await _dao.ObtenerTodosLosAlquileresAsync();
            return View(alquileres);
        }

        public async Task<IActionResult> Activos()
        {
            // Finalizar automáticamente los alquileres que exceden el saldo
            await _dao.FinalizarAlquileresExcedidosAsync();
            
            var alquileresActivos = await _dao.ObtenerAlquileresActivosAsync();
            return View(alquileresActivos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var alquiler = await _dao.ObtenerAlquilerPorIdAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }
            return View(alquiler);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Cuentas = await _dao.ObtenerCuentasActivasAsync();
            ViewBag.MaquinasDisponibles = await _dao.ObtenerMaquinasDisponiblesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlquilerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var alquiler = new Alquiler
                    {
                        Ncuenta = model.Ncuenta,
                        Nmaquina = model.Nmaquina,
                        Tipo = model.TipoAlquiler ? 2 : 1,
                        TiempoContratado = model.TipoAlquiler ? model.TiempoContratado : null
                    };

                    await _dao.AgregarAlquilerAsync(alquiler, model.TipoAlquiler);
                    TempData["SuccessMessage"] = "Alquiler iniciado exitosamente.";
                    return RedirectToAction(nameof(Activos));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear el alquiler: {ex.Message}";
                }
            }
            
            ViewBag.Cuentas = await _dao.ObtenerCuentasActivasAsync();
            ViewBag.MaquinasDisponibles = await _dao.ObtenerMaquinasDisponiblesAsync();
            return View(model);
        }

        public async Task<IActionResult> Finalizar(int id)
        {
            var alquiler = await _dao.ObtenerAlquilerPorIdAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }
            
            var tiempoTranscurrido = await _dao.ObtenerTiempoUsoActualAsync(id);
            var costoActual = await _dao.ObtenerCostoActualAlquilerAsync(id);
            
            ViewBag.TiempoTranscurrido = tiempoTranscurrido;
            ViewBag.CostoActual = costoActual;
            
            return View(alquiler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id, decimal montoPagado)
        {
            try
            {
                await _dao.FinalizarAlquilerCompletoAsync(id);
                TempData["SuccessMessage"] = $"Alquiler finalizado exitosamente. Monto: ${montoPagado:N2}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al finalizar el alquiler: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> QuickStart(int cuentaId, int maquinaId, int tipo)
        {
            try
            {
                int idAlquiler;
                if (tipo == 1) // Libre
                {
                    idAlquiler = await _dao.IniciarAlquilerLibreAsync(cuentaId, maquinaId);
                }
                else // Tiempo definido
                {
                    idAlquiler = await _dao.IniciarAlquilerTiempoDefinidoAsync(cuentaId, maquinaId, TimeSpan.FromHours(1));
                }
                
                TempData["SuccessMessage"] = $"Alquiler iniciado. ID: {idAlquiler}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            
            return RedirectToAction("Activos");
        }
    }

    public class AlquilerViewModel
    {
        public int Ncuenta { get; set; }
        public int Nmaquina { get; set; }
        public bool TipoAlquiler { get; set; } // false = Libre, true = Tiempo Definido
        public TimeSpan? TiempoContratado { get; set; }
    }
}