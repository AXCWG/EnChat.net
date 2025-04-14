using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using EnChat.Context;
using EnChat.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                Password = context.Password.ToSha256HexHashString(), Pending = "[]", Profile = []
            });
            contextDb.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }

        return Login(new HttpBodyContexts.RegisterLoginContext()
            { Username = context.Username, Password = context.Password });
    }

    [HttpPost]
    public IActionResult Login([FromBody] HttpBodyContexts.RegisterLoginContext context)
    {
        using var contextDb = new EnChatDbContext();
        var userInfo = contextDb.Users.Select(i => new
        {
            i.Username, i.Password, i.Uuid
        }).Where(i =>
            i.Username == context.Username && i.Password == context.Password.ToSha256HexHashString());
        if (userInfo.Count() == 1)
        {
            HttpContext.Session.SetString("uuid", userInfo.First().Uuid);
            HttpContext.Session.SetString("username", userInfo.First().Username);

            return new OkObjectResult(new
            {
                userInfo.First().Username,
                userInfo.First().Uuid
            });
        }

        return BadRequest();
    }

    [HttpGet]
    public ActionResult<object> UserApi()
    {
        using var contextDb = new EnChatDbContext();
        var userInfo = contextDb.Users.Select(i => new
        {
            i.Username, i.Uuid,
            Contacts = JsonSerializer.Deserialize<List<UserDeserializationContexts.Contacts>>(i.Contacts,
                JsonSerializerOptions.Default)
        }).FirstOrDefault(i => i.Uuid == HttpContext.Session.GetString("uuid"));
        if (userInfo == null)
        {
            return BadRequest();
        }

        return new OkObjectResult(userInfo);
    }

    [HttpGet]
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    [HttpPost]
    public ActionResult<object> Profile([FromBody] HttpBodyContexts.ProfileUpdateContext context)
    {
        try
        {
            string? uuid = HttpContext.Session.GetString("uuid");
            if (uuid == null)
            {
                return BadRequest();
            }

            using var contextDb = new EnChatDbContext();
            contextDb.Users.Where(i => i.Uuid == uuid)
                .ExecuteUpdate(setter => setter.SetProperty(i => i.Profile, context.Profile));
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<object> ProfileGet(string uuidDestination, [Required] bool img)
    {
        try
        {
            var uuidSession = HttpContext.Session.GetString("uuid");
            if (uuidSession == null)
            {
                return BadRequest();
            }

            using var contextDb = new EnChatDbContext();
            var userObj = contextDb.Users.FirstOrDefault(i => i.Uuid == uuidDestination);

            var contactObj = JsonSerializer.Deserialize<List<UserDeserializationContexts.Contacts>>(
                userObj?.Contacts!,
                JsonSerializerOptions.Default);
            var userSub = contextDb.Users.FirstOrDefault(i => i.Uuid == uuidSession);
            var contactSub = JsonSerializer.Deserialize<List<UserDeserializationContexts.Contacts>>(
                userSub?.Contacts!,
                JsonSerializerOptions.Default);
            if (contactObj == null || userObj == null)
            {
                return BadRequest();
            }

            if (contactSub == null || userSub == null)
            {
                return BadRequest();
            }

            if (contactObj.Count(i => i.Uuid == uuidSession) == 1 &&
                contactSub.Count(i => i.Uuid == uuidDestination) == 1)
            {
                if (img)
                {
                    return File(userObj.Profile, "image/png");
                }

                return
                    userObj.Username;
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}