using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyAutoPartsHub.Views.Shared.Components.DropTipoPessoa
{
    public class DropTipoPessoa : ViewComponent
    {
        public IViewComponentResult Invoke(string tipo)
        {
            try
            {
                List<SelectListItem> lst = [];
                lst.Add(new SelectListItem { Value = "PJ", Text = "PJ", Selected = "PJ" == tipo });
                lst.Add(new SelectListItem { Value = "PF", Text = "PF", Selected = "PF" == tipo });
                return View("Default", lst);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
