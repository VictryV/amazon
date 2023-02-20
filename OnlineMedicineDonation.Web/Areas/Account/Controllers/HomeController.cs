using Microsoft.AspNetCore.Mvc;

namespace OnlineMedicineDonation.Web.Areas.Account.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
