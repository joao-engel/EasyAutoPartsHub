using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    [Authorize]
    public class GrupoController : Controller
    {
        private readonly IGrupoServices _grupoServices;

        public GrupoController(IGrupoServices grupoServices)
        {
            _grupoServices = grupoServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(GrupoProdutoModel model)
        {
            try
            {
                var result = await _grupoServices.Listar(model);
                return PartialView("_Tabela", result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
        {
            try
            {
                GrupoProdutoModel model = new();

                if (id.HasValue)
                {
                    model = await _grupoServices.Obter(id.Value);
                }

                return View("Cadastro", model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(GrupoProdutoModel model)
        {
            try
            {
                await _grupoServices.Salvar(model);
                return Ok("Grupo cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
