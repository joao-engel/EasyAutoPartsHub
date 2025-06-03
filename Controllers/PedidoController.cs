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

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(PedidoCadastroModel model)
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

                await _pedidoServices.Salvar(model);

                return Ok("Pedido cadastrado!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
