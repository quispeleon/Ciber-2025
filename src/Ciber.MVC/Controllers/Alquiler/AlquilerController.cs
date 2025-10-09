using Microsoft.AspNetCore.Mvc;
using Ciber.core;
using Ciber.MVC.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Ciber.MVC.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly IDAO _iDAO;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ILogger<AlquilerController> logger, IDAO iDAO)
        {
            _iDAO = iDAO;
            _logger = logger;
        }

        // GET: Mostrar todos los alquileres
        public async Task<IActionResult> Index()
        {
            var alquileresCore = await _iDAO.ObtenerTodosLosAlquileresAsync();

            // Convertir a modelos de vista
            var alquileresView = alquileresCore.Select(a => new AlquilerViewModel
            {
                IdAlquiler = a.IdAlquiler,
                Ncuenta = a.Ncuenta,
                Nmaquina = a.Nmaquina,
                Tipo = a.Tipo,
                CantidadTiempo = a.CantidadTiempo,
                Pagado = a.Pagado
            }).ToList();

            return View(alquileresView);
        }

        // GET: Mostrar formulario para crear alquiler
        public IActionResult Crear()
        {
            return View("AltaAlquiler");
        }

        // POST: Crear nuevo alquiler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(AlquilerViewModel alquilerView, bool tipoAlquiler)
        {
            if (!ModelState.IsValid)
                return View("AltaAlquiler", alquilerView);

            // Convertir de ViewModel a Core
            var alquilerCore = new Alquiler
            {
                IdAlquiler = alquilerView.IdAlquiler,
                Ncuenta = alquilerView.Ncuenta,
                Nmaquina = alquilerView.Nmaquina,
                Tipo = alquilerView.Tipo,
                CantidadTiempo = alquilerView.CantidadTiempo,
                Pagado = alquilerView.Pagado
            };

            await _iDAO.AgregarAlquilerAsync(alquilerCore, tipoAlquiler);
            return RedirectToAction(nameof(Index));
        }

        // GET: Confirmar eliminación
        public async Task<IActionResult> Eliminar(int idAlquiler)
        {
            var alquiler = await _iDAO.ObtenerAlquilerPorIdAsync(idAlquiler);
            if (alquiler == null) return NotFound();

            var viewModel = new AlquilerViewModel
            {
                IdAlquiler = alquiler.IdAlquiler,
                Ncuenta = alquiler.Ncuenta,
                Nmaquina = alquiler.Nmaquina,
                Tipo = alquiler.Tipo,
                CantidadTiempo = alquiler.CantidadTiempo,
                Pagado = alquiler.Pagado
            };

            return View("EliminarAlquiler", viewModel);
        }
        
        // GET: Mostrar detalle de un alquiler
public async Task<IActionResult> Detalle(int idAlquiler)
{
    var alquiler = await _iDAO.ObtenerAlquilerPorIdAsync(idAlquiler);
    if (alquiler == null)
        return NotFound();

    // Convertir a ViewModel si estás usando ViewModel
    var viewModel = new AlquilerViewModel
    {
        IdAlquiler = alquiler.IdAlquiler,
        Ncuenta = alquiler.Ncuenta,
        Nmaquina = alquiler.Nmaquina,
        Tipo = alquiler.Tipo,
        CantidadTiempo = alquiler.CantidadTiempo,
        Pagado = alquiler.Pagado
    };

    return View(viewModel); // Vista Detalle.cshtml
}


// POST: Eliminar confirmado
        [HttpPost, ActionName("Eliminar")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EliminarConfirmado(int idAlquiler)
{
    // Primero obtengo el alquiler
    var alquiler = await _iDAO.ObtenerAlquilerPorIdAsync(idAlquiler);
    if (alquiler != null)
    {
        // Obtengo la máquina asociada
        var maquina = await _iDAO.ObtenerMaquinaPorIdAsync(alquiler.Nmaquina);
        if (maquina != null)
        {
            // La libero
            maquina.Estado = true; 
            await _iDAO.ActualizarMaquinaAsync(maquina);
        }

        // Ahora elimino el alquiler
        await _iDAO.EliminarAlquilerAsync(idAlquiler);
    }

    return RedirectToAction(nameof(Index));
}

    }
}
