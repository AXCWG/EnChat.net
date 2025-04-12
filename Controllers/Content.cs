using System.Text.Json;
using System.Text.Json.Serialization;
using EnChat.Context;
using EnChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnChat.Controllers;

[Route("api/[controller]/[action]")]
public class Content : Controller
{
    [HttpGet]
    public ActionResult<object> LongPolling()
    {
        var context = new EnChatDbContext();
        var userInfo = context.Users.FirstOrDefault(i => i.Uuid == HttpContext.Session.GetString("uuid"));
        if (userInfo == null)
        {
            return BadRequest();
        }

        var pends = JsonSerializer.Deserialize<List<UserDeserializationContexts.Pending>>(userInfo.Pending,
            JsonSerializerOptions.Default);
        Console.WriteLine(userInfo.Pending);

        while (pends?.Count == 0)
        {
            context = new EnChatDbContext();
            Task.Delay(500).Wait();
            userInfo = context.Users.FirstOrDefault(i => i.Uuid == HttpContext.Session.GetString("uuid"));
            if (userInfo == null)
            {
                return BadRequest();
            }

            pends = JsonSerializer.Deserialize<List<UserDeserializationContexts.Pending>>(userInfo.Pending,
                JsonSerializerOptions.Default);
        }

        context = new EnChatDbContext();
        context.Users.Where(i => i.Uuid == HttpContext.Session.GetString("uuid"))
            .ExecuteUpdate(setter => setter.SetProperty(i => i.Pending, "[]"));
        return pends!;
    }

    [HttpPost]
    public ActionResult<object> Send([FromBody] HttpBodyContexts.SendContext send)
    {
        try
        {
            var context = new EnChatDbContext();
            var toUserInfo = context.Users.FirstOrDefault(i => i.Uuid == send.To);
            if (toUserInfo == null)
            {
                return BadRequest();
            }

            var fromUserInfo = context.Users.FirstOrDefault(i => i.Uuid == HttpContext.Session.GetString("uuid"));
            if (fromUserInfo == null)
            {
                return BadRequest();
            }

            var contactsTo =
                JsonSerializer.Deserialize<List<UserDeserializationContexts.Contacts>>(toUserInfo.Contacts);
            if (contactsTo?.Count(i => i.Uuid == fromUserInfo.Uuid) == 0)
            {
                return BadRequest();
            }

            context = new EnChatDbContext();
            var original = JsonSerializer.Deserialize<List<UserDeserializationContexts.Pending>>(toUserInfo.Pending);
            if (original == null)
            {
                throw new NullReferenceException();
            }
            original.Add(new UserDeserializationContexts.Pending()
            {
                From = HttpContext.Session.GetString("uuid")!, Content = send.Content, Date = DateTimeOffset.UtcNow,
            });
            context.Users.Where(i => i.Uuid == toUserInfo.Uuid).ExecuteUpdate(update =>
                update.SetProperty(i => i.Pending, JsonSerializer.Serialize(original, JsonSerializerOptions.Default)));
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BadRequestObjectResult(e.Message);
        }
        
    }
}