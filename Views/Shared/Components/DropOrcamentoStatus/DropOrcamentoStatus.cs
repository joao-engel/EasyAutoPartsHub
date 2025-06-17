using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;


namespace EasyAutoPartsHub.Views.Shared.Components.DropOrcamentoStatus
{
    public class DropOrcamentoStatus : ViewComponent
    {
        private readonly IOrcamentoServices _se;

        public DropOrcamentoStatus(IOrcamentoServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id = null, List<StatusModel> lst = null)
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
