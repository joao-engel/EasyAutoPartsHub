using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Models.ViewModels;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoServices _pedidoServices;

        public PedidoController(IPedidoServices pedidoServices)
        {
            _pedidoServices = pedidoServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PedidoCabecalhoRQModel model)
        {
            try
            {
                var lst = await _pedidoServices.ListarPedidos(model);
                return PartialView("_Tabela", lst);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VisualizarPedido(int id)
        {
            try
            {
                PedidoViewModel ret = await _pedidoServices.VisualizarPedido(id);
                return PartialView("_ModalVisualizaPedido", ret);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
