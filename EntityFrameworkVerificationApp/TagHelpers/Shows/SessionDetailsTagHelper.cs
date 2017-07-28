using EntityFrameworkVerificationApp.Data;
using EntityFrameworkVerificationApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.TagHelpers.Shows
{
    public class SessionDetailsTagHelper : TagHelper
    {
        private const string ShowIdAttributeName = "show-id";
        private const string SessionIdAttributeName = "session-id";
        private const string SeatCodeAttributeName = "seat-code";

        public SessionDetailsTagHelper(ShowsContext showsContext, IHtmlHelper htmlHelper)
        {
            ShowsContext = showsContext;
            Helper = htmlHelper;
        }

        [HtmlAttributeName(ShowIdAttributeName)]
        public int ShowId { get; set; }

        [HtmlAttributeName(SessionIdAttributeName)]
        public int SessionId { get; set; }

        [HtmlAttributeName(SeatCodeAttributeName)]
        public int SeatCode { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeNotBound]
        public IHtmlHelper Helper { get; set; }

        [HtmlAttributeNotBound]
        public ShowsContext ShowsContext { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var details = await ShowsContext.Shows
                .Include(s => s.Theater)
                .ThenInclude(t => t.Location)
                .Include(s => s.Sessions)
                .ThenInclude(s => s.Seats)
                .Where(s => s.Id == SessionId && s.Sessions.Any(ss => ss.Id == SessionId))
                .Select(s => new SessionDetails
                {
                    Show = s,
                    Session = s.Sessions.Single(ss => ss.Id == SessionId),
                    Seat = s.Sessions.Single(ss => ss.Id == SessionId).Seats.Single(sss => sss.Code == SeatCode)
                })
                .SingleOrDefaultAsync();

            ((IViewContextAware)Helper).Contextualize(ViewContext);

            var result = await Helper.PartialAsync("_sessionDetails", details);

            output.Content.SetHtmlContent(result);
        }
    }
}
