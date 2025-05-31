using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EasyAutoPartsHub.Views.Shared.Components.ModalClientes
{
    public class ModalClientes : ViewComponent
    {
        private readonly IClienteServices _se;

        public ModalClientes(IClienteServices se)
        {
            _se = se;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool? todos)
        {
            try
            {
                List<ClienteModel> lista = await _se.Listar(new ClienteModel { });

                ViewBag.Todos = todos;
                return View("Default", lista);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
