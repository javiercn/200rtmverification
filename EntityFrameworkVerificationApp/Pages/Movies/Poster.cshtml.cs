using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EntityFrameworkVerificationApp.Pages.Movies
{
    public class PosterModel : PageModel
    {
        public IActionResult OnGet([FromRoute]int id)
        {
            return RedirectToPage("/Movies/Details", "poster", new { id });
        }
    }
}