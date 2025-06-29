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
                var ret = await _relatorioServices.FaturamentoProduto(dataIni, dataFim);
                return PartialView("_TabelaFaturamentoProduto", ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult FaturamentoCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FaturamentoCliente(DateTime dataIni, DateTime dataFim)
        {
            try
            {
                var ret = await _relatorioServices.FaturamentoCliente(dataIni, dataFim);
                return PartialView("_TabelaFaturamentoCliente", ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult OrcamentoStatus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrcamentoStatus(DateTime dataIni, DateTime dataFim)
        {
            try
            {
                var ret = await _relatorioServices.OrcamentoStatus(dataIni, dataFim);
                return PartialView("_TabelaOrcamentoStatus", ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
