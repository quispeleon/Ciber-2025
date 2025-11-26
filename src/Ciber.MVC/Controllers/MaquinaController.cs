using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class MaquinaController : Controller
    {
        private readonly IDAO _dao;

        public MaquinaController(IDAO dao)
        {
            _dao = dao;
        }

        public async Task<IActionResult> Index()
        {
            var maquinas = await _dao.ObtenerTodasLasMaquinasAsync();
            return View(maquinas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var maquina = await _dao.ObtenerMaquinaPorIdAsync(id);
            if (maquina == null)
            {
                return NotFound();
            }
            
            var historial = await _dao.ObtenerHistorialPorMaquinaAsync(id);
            ViewBag.Historial = historial;
            
            return View(maquina);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maquina maquina)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dao.AgregarMaquinaAsync(maquina);
                    TempData["SuccessMessage"] = "Máquina creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al crear la máquina: {ex.Message}";
                }
            }
            return View(maquina);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var maquina = await _dao.ObtenerMaquinaPorIdAsync(id);
            if (maquina == null)
            {
                return NotFound();
            }
            return View(maquina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maquina maquina)
        {
            if (id != maquina.Nmaquina)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dao.ActualizarMaquinaAsync(maquina);
                    TempData["SuccessMessage"] = "Máquina actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar la máquina: {ex.Message}";
                }
            }
            return View(maquina);
        }

        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            try
            {
                await _dao.CambiarEstadoMaquinaAsync(id, estado);
                TempData["SuccessMessage"] = $"Estado cambiado a: {estado}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cambiar estado: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}