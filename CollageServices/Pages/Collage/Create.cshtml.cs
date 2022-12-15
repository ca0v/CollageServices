using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CollagePageModel: PageModel
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
        Collage = new ImageRipper.Collage() {
            Title = "Title",
            Note = "Note",
            Data = "Data"
        };
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _context.Collages.AddAsync(Collage);

        return RedirectToPage("./Index");
    }
}