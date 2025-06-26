using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IRelatorioServices _relatorioServices;

        public RelatorioController(IRelatorioServices relatorioServices)
        {
            _relatorioServices = relatorioServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FaturamentoProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FaturamentoProduto(DateTime dataIni, DateTime dataFim)
        {
            try
            {
                var ret = await _relatorioServices.FaturamentoPeriodo(dataIni, dataFim);
                return PartialView("_TabelaFaturamentoProduto", ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
