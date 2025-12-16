using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Ciber.MVC.Models;
using Ciber.core;
using System.Security.Claims;
using BCrypt.Net;

namespace Ciber.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IDAO _dao;

        public AuthController(IDAO dao)
        {
            _dao = dao;
        }

        // ========= LOGIN =========

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new UsuarioLoginDto());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioLoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _dao.ObtenerUsuarioSistemaPorUsernameAsync(model.Username);

            if (usuario == null || !usuario.Activo)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            if (!VerificarPassword(model.Password, usuario.PasswordHash))
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("Id", usuario.Id.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            return RedirectToAction("Index", "Home");
        }

        // ========= LOGOUT =========

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }

        // ========= PASSWORD =========

        private bool VerificarPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
