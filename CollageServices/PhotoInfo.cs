namespace ImageRipper;

public class PhotoInfo
{
    public string? id { get; set; }
    public string? filename { get; set; }
    public string? created { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class CollageState
{
    public string id { get; set; }
    public string title { get; set; }
    public CollageCellState[] data { get; set; }
    public string? note { get; set; }
}

public class CollageCellState
{
    public string id { get; set; }
    public string target { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string transform { get; set; }
    public string clipPath { get; set; }
    public CollageCellBackground? background { get; set; }
}

public class CollageCellBackground
{
    public string? fill { get; set; }
    public string? stroke { get; set; }
}

