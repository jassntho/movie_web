using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Actor
{
    public int ActorId { get; set; }

    public string? ActorName { get; set; }

    public string? ActorAlias { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
