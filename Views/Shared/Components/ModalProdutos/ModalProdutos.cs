using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Views.Shared.Components.ModalClientes
{
    public class ModalProdutos : ViewComponent
    {
        private readonly IProdutoServices _se;
        private readonly IGrupoServices _seGrupo;

        public ModalProdutos(IProdutoServices se, IGrupoServices seGrupo)
        {
            _se = se;
            _seGrupo = seGrupo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                List<ProdutoModel> lista = await _se.Listar(new ProdutoRQModel { Ativo = true });                
                List<GrupoProdutoModel> lstGrupos = await _seGrupo.Listar(new GrupoProdutoModel {  });

                ViewBag.Grupos = lstGrupos;
                return View("Default", lista);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
