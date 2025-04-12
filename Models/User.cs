using System;
using System.Collections.Generic;

namespace EnChat.Models;

/// <summary>
/// User info
/// </summary>
public partial class User
{
    public string Uuid { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Contacts { get; set; } = null!;

    public string Pending { get; set; } = null!;

    public byte[] Profile { get; set; } = null!;
}
