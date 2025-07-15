using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyAutoPartsHub.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        public LoginController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string senha)
        {
            UsuarioModel usuarioModel = await _usuarioServices.ObterParaLogin(usuario);

            if (usuario == null || !SenhaHelper.VerificarSenha(senha, usuarioModel.Senha, usuarioModel.Salt))
            {
                ModelState.AddModelError("", "Email ou senha inválidos");
                return View();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, usuarioModel.Nome),
                new(ClaimTypes.NameIdentifier, usuarioModel.ID.ToString()),
                new(ClaimTypes.Email, usuarioModel.Email),
            };

            var identidade = new ClaimsIdentity(claims, "EasyAutoPartsHub");
            var principal = new ClaimsPrincipal(identidade);

            await HttpContext.SignInAsync("EasyAutoPartsHub", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
