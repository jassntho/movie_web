using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Rate
{
    public int MovieId { get; set; }

    public int AccountId { get; set; }

    public int? Rate1 { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
