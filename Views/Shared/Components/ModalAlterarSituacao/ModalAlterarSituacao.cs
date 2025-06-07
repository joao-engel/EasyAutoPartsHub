using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Views.Shared.Components.ModalAlterarSituacao
{
    public class ModalAlterarSituacao : ViewComponent
    {
        private readonly IPedidoServices _se;

        public ModalAlterarSituacao(IPedidoServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            try
            {
                PedidoCabecalhoModel pedido = await _se.ObterPedido(id);

                string proxStatus = pedido.StatusID switch
                {
                    1 => "Faturado", // Pendente
                    2 => "Entregue", // Faturado
                    _ => "Desconhecido"
                };
                ViewBag.ProximoStatus = proxStatus;

                return View("Default", pedido);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
