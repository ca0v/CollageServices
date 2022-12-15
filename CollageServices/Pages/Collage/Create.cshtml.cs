using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CollagePageModel : PageModel
{
    private readonly ImageRipper.PhotoContext _context;

    [BindProperty]
    public ImageRipper.Collage? Collage { get; set; }

    public CollagePageModel(ImageRipper.PhotoContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        Collage = new ImageRipper.Collage()
        {
            Id = "PRACTICE_MODE",
            Title = "Title",
            Note = "Note",
            Data = "Data"
        };
        return Page();
    }


    public IActionResult OnPost()
    {
        if (Collage == null)
        {
            return Page();
        }

        Collage.Id = System.Guid.NewGuid().ToString();
        _context.Collages.Add(Collage);
        _context.SaveChanges();

        return RedirectToPage("./Index");
    }
}