using EnChat.Context;
using EnChat.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EnChat.Controllers;

[Route("api/[controller]/[action]")]
public class User : Controller
{
    // GET
    [HttpPost]
    public IActionResult Register([FromBody] HttpBodyContexts.RegisterLoginContext context)
    {
        try
        {
            using var contextDb = new EnChatDbContext();
            if (contextDb.Users.Any(u => u.Username == context.Username))
            {
                return BadRequest("Username is already taken");
            }
            contextDb.Users.Add(new Models.User
            {
                Uuid = Guid.NewGuid().ToString(), Contacts = "[]", Username = context.Username,
                Password = context.Password.ToSha256HexHashString(), Pending = "[]"
            });
            contextDb.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }

        return new OkResult(); 
    }

    [HttpPost]
    public IActionResult Login([FromBody] HttpBodyContexts.RegisterLoginContext context)
    {
        using var contextDb = new EnChatDbContext();
        if (contextDb.Users.Count(i =>
                i.Username == context.Username && i.Password == context.Password.ToSha256HexHashString()) == 1)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    public ActionResult<object> OnPulse()
    {
        return null!;
    }
}