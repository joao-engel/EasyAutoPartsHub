using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly IProdutoServices _produtoServices;

        public ProdutoController(IProdutoServices produtoServices)
        {
            _produtoServices = produtoServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProdutoRQModel model)
        {
            try
            {
                List<ProdutoModel> produtos = await _produtoServices.Listar(model);
                return PartialView("_Tabela", produtos);
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
                ProdutoModel model = new()
                {
                    Ativo = true,
                };

                if (id.HasValue)
                {
                    model = await _produtoServices.Obter(id.Value);
                }

                return View("Cadastro", model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(ProdutoModel model)
        {
            try
            {
                await _produtoServices.Salvar(model);
                return Ok("Produto cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
