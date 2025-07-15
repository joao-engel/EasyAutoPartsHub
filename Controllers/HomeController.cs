using EasyAutoPartsHub.Models;
using EasyAutoPartsHub.Models.ViewModels;
using EasyAutoPartsHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EasyAutoPartsHub.Controllers
{
    [Authorize]
    public class HomeController(IDashboardServices dashboardServices) : Controller
    {
        private readonly IDashboardServices _dashboardServices = dashboardServices;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int ano = 2025; // DateTime.Now.Year;
                int mes = 6; // DateTime.Now.Month;
                DashboardViewModel model = await _dashboardServices.Dashboard(ano, mes);
                return View(model);
            }
            catch (Exception)
            {
                DashboardViewModel model = new();
                return View(model);
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
