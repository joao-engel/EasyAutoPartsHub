using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EasyAutoPartsHub.Views.Shared.Components.DropAtivoInativo
{
    public class DropAtivoInativo : ViewComponent
    {
        public IViewComponentResult Invoke(bool? ativo)
        {
            try
            {
                List<SelectListItem> lst = [];
                lst.Add(new SelectListItem { Value = "true", Text = "Ativo", Selected = true == ativo });
                lst.Add(new SelectListItem { Value = "false", Text = "Inativo", Selected = false == ativo });
                return View("Default", lst);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
