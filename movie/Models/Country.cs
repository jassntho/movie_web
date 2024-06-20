using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? CountryAlias { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
