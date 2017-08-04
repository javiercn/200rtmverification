using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntityFrameworkVerificationApp.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkVerificationApp.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        public static readonly byte[] MoviePoster = Convert.FromBase64String("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");

        public DetailsModel(CatalogContext catalog, ReviewsContext reviews, ShowsContext showsContext)
        {
            Catalog = catalog;
            Reviews = reviews;
            ShowsContext = showsContext;
        }

        public async Task Init()
        {
            Comments = await Reviews.Reviews.Where(r => r.TopicId == Id).ToListAsync();
            Comments.Where(c => c.ReplyTo == null).ToList();

            Movie = await Catalog.Movies.Where(m => m.Id == Id)
                .Include(m => m.Details)
                .Include(m => m.Cast)
                .ThenInclude(c => c.Artist)
                .ThenInclude(a => a.PersonalInformation)
                .ThenInclude(p => p.BirthPlace)
                .SingleOrDefaultAsync();

            Artists = Movie.Cast.Select(c => c.Artist).ToList();

            PlayingOn = await ShowsContext.Shows.Where(s => s.MovieId == Id).CountAsync();
        }

        public CatalogContext Catalog { get; }
        public ReviewsContext Reviews { get; }
        public ShowsContext ShowsContext { get; }

        [FromRoute]
        public int Id { get; set; }

        public Movie Movie { get; private set; }
        public List<Artist> Artists { get; private set; }
        public int PlayingOn { get; private set; }
        public List<Review> Comments { get; private set; }

        public async Task OnGet()
        {
            await Init();
        }

        [Authorize]
        public async Task<IActionResult> OnPostDelete([FromForm] int commentId)
        {
            await Init();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var review = await Reviews.Reviews.FindAsync(commentId);
            if (!review.Details.AuthorId.Equals(User.FindFirst(ClaimTypes.NameIdentifier)?.Value))
            {
                return Forbid();
            }

            review.Details.Body = null;
            await Reviews.SaveChangesAsync();

            return RedirectToPage();
        }

        [Authorize]
        public async Task<IActionResult> OnPostComment([FromQuery]int? replyTo, [FromForm]string comment)
        {
            await Init();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Page();
            }

            var newReview = new Review()
            {
                Details = new ReviewDetails
                {
                    Author = User.Identity.Name,
                    AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Body = comment,
                    PublishDate = DateTimeOffset.UtcNow
                },
                ReplyToId = replyTo,
                TopicId = Id
            };

            Reviews.Reviews.Add(newReview);
            await Reviews.SaveChangesAsync();

            return RedirectToPage();
        }

        public IActionResult OnGetPoster() =>
             File(MoviePoster, "img/jpeg");
    }
}