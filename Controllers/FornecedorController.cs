using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly IFornecedorServices _se;

        public FornecedorController(IFornecedorServices se)
        {
            _se = se;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(FornecedorModel model)
        {
            try
            {
                var lst = await _se.Listar(model);
                return PartialView("_Tabela", lst);
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
                FornecedorModel model = new();

                if (id.HasValue)
                {
                    model = await _se.Obter(id.Value);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(FornecedorModel model)
        {
            try
            {
                await _se.Salvar(model);
                return Ok("Fornecedor cadastado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
