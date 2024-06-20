using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoriesAlias { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
