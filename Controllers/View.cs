using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace EnChat.Controllers;

[Route("/[action]")]
public class View : Controller
{
    // GET
    [HttpGet]
    [Route("/")]
    public async Task<ContentResult> Index()
    {
        return Content(await Helper.ReadEmbeddedAssets("index.html"), "text/html");
    }
}