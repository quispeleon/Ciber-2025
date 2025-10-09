using Microsoft.AspNetCore.Mvc;
using Ciber.MVC.Models;
using Ciber.core; // Para mapear a las clases core si es necesario

namespace Ciber.MVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IDAO _iDAO;

        public ClienteController(IDAO iDAO)
        {
            _iDAO = iDAO;
        }

        public IActionResult Index()
        {
            // Obtenemos las cuentas desde core y mapeamos a Models
            var clientesCore = _iDAO.ObtenerTodasLasCuentas(); // IEnumerable<Cuenta>
            var clientes = clientesCore.Select(c => new Cliente
            {
                Ncuenta = c.Ncuenta,
                NombreCompleto = c.Nombre,
                Contrasena = c.Pass,
                Dni = c.Dni
            });

            return View(clientes);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            // Convertimos a core
            var cuentaCore = new Cuenta
            {
                Ncuenta = cliente.Ncuenta,
                Nombre = cliente.NombreCompleto,
                Pass = cliente.Contrasena,
                Dni = cliente.Dni
            };

            _iDAO.AgregarCuenta(cuentaCore);

            return RedirectToAction(nameof(Index));
        }
    }
}
