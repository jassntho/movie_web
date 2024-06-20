using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Type
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public string? TypeAlias { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
