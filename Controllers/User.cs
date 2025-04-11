using Microsoft.AspNetCore.Mvc;

namespace EnChat.Controllers;

[Route("api/[controller]/[action]")]
public class User : Controller
{
    // GET
    [HttpPost]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult<object> Login()
    {
        return null!; 
    }

    [HttpPost]
    public ActionResult<object> OnPulse()
    {
        return null!;
    }
}