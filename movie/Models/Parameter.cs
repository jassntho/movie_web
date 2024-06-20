using System;
using System.Collections.Generic;

namespace Movie_Web.Models;

public partial class Parameter
{
    public int ParameterId { get; set; }

    public string? ParameterName { get; set; }

    public int? Value { get; set; }
}
