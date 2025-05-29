using Microsoft.AspNetCore.Mvc;

namespace EasyAutoPartsHub.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MenuController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Cadastros()
        {
            return View();
        }
    }
}
