using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ciber.core;
using Ciber.Dapper;
 

namespace Ciber.MVC.Controllers
{
    [Authorize] // 🔐 nadie entra sin login
    public class CuentaController : Controller
    {
        private readonly IDAO _dao;

        public CuentaController(IDAO dao)
        {
            _dao = dao;
        }

        // ===============================
        // LISTAR CUENTAS (GENERAL + FINANZAS)
        // ===============================
        [Authorize(Roles = "ADMIN_GENERAL,ADMIN_FINANZAS")]
        public async Task<IActionResult> Index()
        {
            var cuentas = await _dao.ObtenerTodasLasCuentasAsync();
            return View(cuentas);
        }

        // ===============================
        // DETALLE CUENTA (GENERAL + FINANZAS)
        // ===============================
        [Authorize(Roles = "ADMIN_GENERAL,ADMIN_FINANZAS")]
        public async Task<IActionResult> Details(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
                return NotFound();

            ViewBag.Historial = await _dao.ObtenerHistorialPorCuentaAsync(id);
            ViewBag.Transacciones = await _dao.ObtenerTransaccionesPorCuentaAsync(id);

            return View(cuenta);
        }

        // ===============================
        // CREAR CUENTA (ADMIN GENERAL)
        // ===============================
        [Authorize(Roles = "ADMIN_GENERAL")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN_GENERAL")]
        public async Task<IActionResult> Create(Cuenta cuenta)
        {
            if (!ModelState.IsValid)
                return View(cuenta);

            try
            {
                cuenta.HoraRegistrada = DateTime.Now.TimeOfDay;
                await _dao.AgregarCuentaAsync(cuenta);

                TempData["SuccessMessage"] = "Cuenta creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(cuenta);
            }
        }

        // ===============================
        // EDITAR CUENTA (ADMIN GENERAL)
        // ===============================
        [Authorize(Roles = "ADMIN_GENERAL")]
        public async Task<IActionResult> Edit(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
                return NotFound();

            return View(cuenta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN_GENERAL")]
        public async Task<IActionResult> Edit(int id, Cuenta cuenta)
        {
            if (id != cuenta.Ncuenta)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cuenta);

            try
            {
                await _dao.ActualizarCuentaAsync(cuenta);
                TempData["SuccessMessage"] = "Cuenta actualizada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(cuenta);
            }
        }

        // ===============================
        // RECARGAR SALDO (ADMIN FINANZAS)
        // ===============================
        [Authorize(Roles = "ADMIN_FINANZAS")]
        public async Task<IActionResult> Recargar(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
                return NotFound();

            return View(cuenta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN_FINANZAS")]
        public async Task<IActionResult> Recargar(int id, decimal monto)
        {
            try
            {
                await _dao.RecargarSaldoAsync(id, monto);
                TempData["SuccessMessage"] = $"Saldo recargado: ${monto:N2}";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // ===============================
        // ELIMINAR CUENTA (ADMIN GENERAL)
        // ===============================
        [Authorize(Roles = "ADMIN_GENERAL")]
        public async Task<IActionResult> Delete(int id)
        {
            var cuenta = await _dao.ObtenerCuentaPorIdAsync(id);
            if (cuenta == null)
                return NotFound();

            return View(cuenta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN_GENERAL")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _dao.EliminarCuentaAsync(id);
                TempData["SuccessMessage"] = "Cuenta eliminada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
