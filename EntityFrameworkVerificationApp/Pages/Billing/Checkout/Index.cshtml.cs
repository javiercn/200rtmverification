using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.Pages.Billing.Checkout
{
    public class IndexModel : PageModel
    {
        public IndexModel(BillingContext billingContext)
        {
            BillingContext = billingContext;
        }

        public BillingContext BillingContext { get; }

        public Customer CurrentCustomer { get; private set; }

        [BindProperty]
        public Address BillingAddress { get; set; }

        [BindProperty]
        public CreditCard CardDetails { get; set; }

        public Item CurrentItem { get; private set; }

        [TempData]
        public int Movie { get; set; }

        [TempData]
        public int Show { get; set; }

        [TempData]
        public int Session { get; set; }

        public int Code { get; private set; }

        public async Task OnGetAsync([FromRoute] int code)
        {
            Code = code;
            TempData.Keep();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            CurrentCustomer = await BillingContext.Customers
                .Include(c => c.Invoices)
                .SingleOrDefaultAsync(c => c.Id == userId);

            CurrentItem = await BillingContext.Items.SingleAsync(s => s.Code == code);
        }

        public async Task OnPostBuy()
        {

        }
    }
}