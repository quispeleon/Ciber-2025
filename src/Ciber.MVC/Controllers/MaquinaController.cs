using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ciber.core;
namespace Ciber.MVC.Controllers
{
    public class MaquinaController : Controller
    {


        private readonly IDAO _iDAO;

        private readonly ILogger<MaquinaController> _logger;

        public MaquinaController(ILogger<MaquinaController> logger,IDAO iDAO)
        {
            _iDAO = iDAO;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var maquinas = await _iDAO.ObtenerTodasLasMaquinasAsync();
            return View(maquinas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }


}