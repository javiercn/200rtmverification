using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.Pages.Shows
{
    public class IndexModel : PageModel
    {
        public IndexModel(CatalogContext catalog, ShowsContext shows)
        {
            ShowsContext = shows;
            CatalogContext = catalog;
        }

        public ShowsContext ShowsContext { get; }
        public CatalogContext CatalogContext { get; }
        public IEnumerable<Show> Shows { get; set; }
        public Movie Movie { get; set; }
        public IList<Artist> Cast { get; set; }

        public async Task OnGet([FromRoute]int movieId)
        {
            Shows = await ShowsContext.Shows
                .Where(s => s.MovieId == movieId)
                .Include(s => s.Sessions)
                .Include(s => s.Theater)
                .ThenInclude(s => s.Location)
                .ToListAsync();

            Movie = await CatalogContext.Movies
                .Include(m => m.Details)
                .Include(m => m.Cast)
                .ThenInclude(c => c.Artist)
                .ThenInclude(a => a.PersonalInformation)
                .SingleOrDefaultAsync(m => m.Id == movieId);

            Cast = Movie.Cast.Select(a => a.Artist).ToList();
        }
    }
}