using System;
using System.Collections.Generic;

namespace ImageRipper.Test;

public partial class Photo
{
    public string Id { get; set; } = null!;

    public string? Filename { get; set; }

    public string? Created { get; set; }

    public long? Width { get; set; }

    public long? Height { get; set; }
}
