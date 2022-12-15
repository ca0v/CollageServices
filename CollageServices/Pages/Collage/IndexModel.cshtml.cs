using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel: PageModel
{
    private readonly ImageRipper.PhotoContext _context;

    public IList<ImageRipper.Collage> Collages { get; set; }

    public IndexModel(ImageRipper.PhotoContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Collages = _context.Collages.ToList();
    }

}