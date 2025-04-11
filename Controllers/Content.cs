using Microsoft.AspNetCore.Mvc;

namespace EnChat.Controllers;

public class Content : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}