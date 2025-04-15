using System.Text.Json;
using System.Text.Json.Serialization;
using EnChat.Context;
using EnChat.Models;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnChat.Controllers;

[Route("api/[controller]/[action]")]
public class Content : Controller
{
    public EnChatDbContext Context => new EnChatDbContext();

  
    [HttpGet]
    [RequestTimeout(10000)]
    public ActionResult<object> LongPolling()
    {
        
    }

    [HttpPost]
    public ActionResult<object> Send([FromBody] HttpBodyContexts.SendContext send)
    {
        
    }

    private static void Sync(EnChatDbContext context)
    {
       
    }
}