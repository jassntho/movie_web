using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string? MovieName { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public int? Time { get; set; }

    public int? PublishedYear { get; set; }

    public string? MovieLink { get; set; }

    public int? TypeId { get; set; }

    public int? Episode { get; set; }

    public int? Viewed { get; set; }

    public bool Status { get; set; }

    public int? CountryId { get; set; }

    public string? Director { get; set; }

    public string? MovieLinkContingency { get; set; }

    public string? MovieLinkTrailer { get; set; }

    public string? ImageSlider { get; set; }

    public string? Alias { get; set; }

    public int? EpisodeLimit { get; set; }

    public string? DirectorAlias { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();

    public virtual Type? Type { get; set; }

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
