using System;
using System.Collections.Generic;

namespace EnChat.Models;

public partial class Pending
{
    public DateTime Date { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Uuid { get; set; } = null!;
}
