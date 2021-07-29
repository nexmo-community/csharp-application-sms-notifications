using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationsUsingVonage.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NotificationsUsingVonage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NotificationManager _notificationManager;
        public HomeController(ILogger<HomeController> logger, 
            NotificationManager notificationManager)
        {
            _logger = logger;
            _notificationManager = notificationManager;
        }

        public async Task<IActionResult> Index()
        {
            await _notificationManager.SendNotification();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
