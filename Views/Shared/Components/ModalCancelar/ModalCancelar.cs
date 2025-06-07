using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Views.Shared.Components.ModalCancelar
{
    public class ModalCancelar : ViewComponent
    {
        private readonly IPedidoServices _se;

        public ModalCancelar(IPedidoServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            try
            {
                PedidoCabecalhoModel pedido = await _se.ObterPedido(id);

                return View("Default", pedido);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
