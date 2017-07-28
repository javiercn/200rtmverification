using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EntityFrameworkVerificationApp.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkVerificationApp.Pages.Sessions
{
    public class IndexModel : PageModel
    {
        public IndexModel(ShowsContext shows, BillingContext billing)
        {
            Shows = shows;
            Billing = billing;
        }

        public ShowsContext Shows { get; }
        public BillingContext Billing { get; }

        public Show CurrentShow { get; set; }
        public Session CurrentSession { get; set; }

        public async Task OnGetAsync([FromRoute] int id)
        {
            var sessionAndShow = await Shows.Shows
                .Include(s => s.Theater)
                .ThenInclude(s => s.Location)
                .Include(s => s.Sessions)
                .ThenInclude(s => s.Seats)
                .Where(s => s.Sessions.Any(ss => ss.Id == id))
                .Select(s => new
                {
                    Show = s,
                    Session = s.Sessions.Single(ss => ss.Id == id)
                })
                .SingleOrDefaultAsync();

            CurrentSession = sessionAndShow.Session;
            CurrentShow = sessionAndShow.Show;
        }
    }
}