using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EntityFrameworkVerificationApp.Pages.Artists.Profile
{
    // This is just here cause we need to clean up the model.
    public class ImageModel : PageModel
    {
        public IActionResult OnGet(int id)
        {
            return RedirectToPage("/Artists/Profile/Index", "image", new { id }, "");
        }
    }
}