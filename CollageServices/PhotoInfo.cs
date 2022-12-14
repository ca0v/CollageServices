namespace ImageRipper;

public class CollageData
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Note { get; set; }
    public CollageCellState[]? Data { get; set; }
}
public class CollageCellState
{
    public string? id { get; set; }
    public string? target { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string? transform { get; set; }
    public string? clipPath { get; set; }
    public CollageCellBackground? background { get; set; }
}

public class CollageCellBackground
{
    public string? fill { get; set; }

    public string? stroke { get; set; }
}

