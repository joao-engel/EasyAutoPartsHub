using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;


namespace EasyAutoPartsHub.Views.Shared.Components.DropGruposProdutos
{
    public class DropGruposProdutos : ViewComponent
    {
        private readonly IGrupoServices _se;

        public DropGruposProdutos(IGrupoServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id = null, List<GrupoProdutoModel> lst = null)
        {
            try
            {
                if (lst == null)
                {
                    GrupoProdutoModel model = new();
                    lst = await _se.Listar(model);
                }

                ViewBag.Id = id;
                return View("Default", lst.OrderBy(x => x.Descricao).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
