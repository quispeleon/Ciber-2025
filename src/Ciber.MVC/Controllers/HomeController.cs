using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.Dapper;

namespace Ciber.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDAO _dao;

        public HomeController(IDAO dao)
        {
            _dao = dao;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                MaquinasDisponibles = await _dao.ObtenerCantidadMaquinasDisponiblesAsync(),
                AlquileresActivos = (await _dao.ObtenerAlquileresActivosAsync()).Count(),
                IngresosHoy = await _dao.ObtenerIngresosPorFechaAsync(DateTime.Today),
                TopMaquinas = await _dao.ObtenerMaquinasMasRentablesAsync()
            };
            return View(dashboard);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string dni, string password)
        {
            if (await _dao.ValidarCredencialesAsync(dni, password))
            {
                // Aquí podrías implementar autenticación
                TempData["SuccessMessage"] = "Login exitoso";
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Credenciales inválidas";
            return View();
        }
    }

    public class DashboardViewModel
    {
        public int MaquinasDisponibles { get; set; }
        public int AlquileresActivos { get; set; }
        public decimal IngresosHoy { get; set; }
        public dynamic? TopMaquinas { get; set; }
    }
}