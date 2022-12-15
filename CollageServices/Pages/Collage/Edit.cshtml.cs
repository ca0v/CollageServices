using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditModel : PageModel
{
    [BindProperty]
    public ImageRipper.Collage? Collage { get; set; }

    private readonly ImageRipper.PhotoContext _context;

    public EditModel(ImageRipper.PhotoContext context)
    {
        _context = context;
    }

    public void OnGet(string id)
    {
        Collage = _context.Collages?.Find(id);
    }

}