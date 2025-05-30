using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;


namespace EasyAutoPartsHub.Views.Shared.Components.DropFornecedores
{
    public class DropFornecedores : ViewComponent
    {
        private readonly IFornecedorServices _se;

        public DropFornecedores(IFornecedorServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id = null, List<FornecedorModel> lst = null)
        {
            try
            {
                if (lst == null)
                {
                    FornecedorModel model = new();
                    lst = await _se.Listar(model);
                }

                ViewBag.Id = id;
                return View("Default", lst.OrderBy(x => x.NomeFantasia).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
