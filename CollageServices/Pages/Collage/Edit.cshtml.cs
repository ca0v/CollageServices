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

    public IActionResult OnPost()
    {
        if (Collage is null)
        {
            return NotFound();
        }

        var original = _context.Collages?.Find(Collage.Id);
        if (original is null)
        {
            return NotFound();
        }

        original.Title = Collage.Title;
        original.Note = Collage.Note;

        _context.Collages?.Update(original);
        _context.SaveChanges();

        return RedirectToPage("/Collage/Edit", new { id = Collage.Id });
    }

}