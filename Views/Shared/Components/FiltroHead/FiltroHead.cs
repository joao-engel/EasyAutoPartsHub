using EasyAutoPartsHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Views.Shared.Components.FiltroHead
{
    public class FiltroHead : ViewComponent
    {
        public IViewComponentResult Invoke(FiltrosModel model)
        {
            return View("Default", model);
        }
    }
}
