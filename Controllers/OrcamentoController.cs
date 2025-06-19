using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class OrcamentoController : Controller
    {
        private readonly IOrcamentoServices _seOrcamento;

        public OrcamentoController(IOrcamentoServices seOrcamento)
        {
            _seOrcamento = seOrcamento;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrcamentoCabecalhoRQModel model)
        {
            try
            {
                List<OrcamentoCabecalhoModel> lst = await _seOrcamento.ListarOrcamentos(model);

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
                OrcamentoCadastroModel ret = new();

                if (id.HasValue)
                {
                    ret = await _seOrcamento.ObterOrcamentoCadastro(id.Value);
                }

                return View(ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(OrcamentoCadastroModel model)
        {
            try
            {
                if (!model.ClienteID.HasValue)
                {
                    throw new Exception("Informe o cliente!");
                }
                if (model.Produtos == null || model.Produtos.Count == 0)
                {
                    throw new Exception("Informe ao menos um produto!");
                }

                await _seOrcamento.Salvar(model);

                return Ok("Orçamento cadastrado!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
