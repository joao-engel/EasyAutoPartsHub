using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioServices.Listar();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                return Problem("Erro ao carregar lista de usuários<br>" + ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
        {
            try
            {
                UsuarioModel model = new();


                if (id.HasValue)
                {
                    model = await _usuarioServices.ObterPorId(id.Value);
                }

                return View("Cadastro", model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioModel model)
        {
            try
            {
                model.DataCadastro = DateTime.Now;

                await _usuarioServices.Salvar(model);
                return Ok("Usuário cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
