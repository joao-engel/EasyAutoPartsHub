using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteServices _clienteServices;
        public ClienteController(IClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClienteModel model)
        {
            try
            {
                Task.Delay(5000).Wait(); // Simula um atraso para a requisição
                var lst = await _clienteServices.Listar(model);
                return PartialView("_Tabela", lst);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cadastro(int? id)
        {
            try
            {
                ClienteModel model = new();

                if (id.HasValue)
                {
                    model = await _clienteServices.Obter(id.Value);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(ClienteModel model)
        {
            try
            {
                await _clienteServices.Salvar(model);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
