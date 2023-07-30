using Microsoft.AspNetCore.Mvc;

namespace MarketData.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() =>
            View();
    }
}