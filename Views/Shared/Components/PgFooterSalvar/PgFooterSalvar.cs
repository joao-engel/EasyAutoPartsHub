using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Views.Shared.Components.PgFooterSalvar
{
    public class PgFooterSalvar : ViewComponent
    {
        public IViewComponentResult Invoke(bool submit, string cancelarController, string cancelarAction, string chave, string param)
        {
            try
            {
                ViewBag.cancelarController = cancelarController;
                ViewBag.cancelarAction = cancelarAction;
                ViewBag.submit = submit;
                ViewBag.chave = chave;
                ViewBag.param = param;
                return View("Default");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
