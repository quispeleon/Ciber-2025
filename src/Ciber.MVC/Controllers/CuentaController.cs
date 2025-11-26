    using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class CuentaController : Controller
    {
        private readonly IDAO _dao;

        public CuentaController(IDAO dao)
        {
            _dao = dao;
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            var cuentas = await _dao.ObtenerTodasLasCuentasAsync();
            return View(cuentas);
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            
            // Obtener historial de la cuenta
            var historial = await _dao.ObtenerHistorialPorCuentaAsync(id);
            var transacciones = await _dao.ObtenerTransaccionesPorCuentaAsync(id);
            
            ViewBag.Historial = historial;
            ViewBag.Transacciones = transacciones;
            
            return View(cuenta);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cuenta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Asegurar que la hora de registro sea la actual
                    cuenta.HoraRegistrada = DateTime.Now.TimeOfDay;
                    await _dao.AgregarCuentaAsync(cuenta);
                    TempData["SuccessMessage"] = $"Cuenta creada exitosamente. ID: {cuenta.Ncuenta}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear la cuenta: {ex.Message}";
                }
            }
            return View(cuenta);
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cuenta cuenta)
        {
            if (id != cuenta.Ncuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dao.ActualizarCuentaAsync(cuenta);
                    TempData["SuccessMessage"] = "Cuenta actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar la cuenta: {ex.Message}";
                }
            }
            return View(cuenta);
        }

        // GET: Cuenta/Recargar/5
        public async Task<IActionResult> Recargar(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Recargar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recargar(int id, decimal monto)
        {
            try
            {
                await _dao.RecargarSaldoAsync(id, monto);
                TempData["SuccessMessage"] = $"Saldo recargado exitosamente: ${monto:N2}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al recargar saldo: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _dao.EliminarCuentaAsync(id);
                TempData["SuccessMessage"] = "Cuenta eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar la cuenta: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}