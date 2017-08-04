using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.TagHelpers.Shows
{
    public class SessionsForMovieTagHelper : TagHelper
    {
        public SessionsForMovieTagHelper(ShowsContext showsContext)
        {
            ShowsContext = showsContext;
        }

        public ShowsContext ShowsContext { get; }

        public int MovieId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var playingOn = await ShowsContext.Shows.Where(s => s.MovieId == MovieId).CountAsync();
            output.TagName = null;
            switch (playingOn)
            {
                case 0:
                    output.Content.SetContent("Not playing right now");
                    break;
                case 1:
                    output.Content.SetContent("Playing on 1 theater");
                    break;
                default:
                    output.Content.SetContent($"Playing on {playingOn} theatres");
                    break;
            }
        }
    }
}
