using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;


namespace EasyAutoPartsHub.Views.Shared.Components.DropGruposProdutos
{
    public class DropPedidoStatus : ViewComponent
    {
        private readonly IPedidoServices _se;

        public DropPedidoStatus(IPedidoServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id = null, List<PedidoStatusModel> lst = null)
        {
            try
            {
                lst ??= await _se.ListarStatus();

                ViewBag.Id = id;
                return View("Default", lst.OrderBy(x => x.Ordem).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
