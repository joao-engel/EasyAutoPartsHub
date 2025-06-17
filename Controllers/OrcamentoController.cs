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
    }
}
