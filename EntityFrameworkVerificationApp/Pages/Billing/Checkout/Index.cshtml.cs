using EntityFrameworkVerificationApp.Data;
using EntityFrameworkVerificationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public CheckoutModel Checkout { get; set; }

        public Item CurrentItem { get; private set; }

        [FromQuery]
        public int MovieId { get; set; }

        [FromQuery]
        public int ShowId { get; set; }

        [FromQuery]
        public int SessionId { get; set; }

        [FromRoute]
        public int Code { get; set; }

        private async Task InitializePage()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var customers = await BillingContext.Customers.ToListAsync();
            CurrentCustomer = await BillingContext.Customers
                .Include(c => c.Invoices)
                .SingleOrDefaultAsync(c => c.Id == userId);

            CurrentItem = await BillingContext.Items.SingleAsync(s => s.Code == Code);
        }

        public Task OnGetAsync()
        {
            TempData.Keep();
            return InitializePage();
        }

        public async Task<IActionResult> OnPostBuy()
        {
            await InitializePage();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProcessCheckout();

            try
            {
                await BillingContext.SaveChangesAsync();
            }
            catch
            {
                ModelState.AddModelError("", "The selected seat is not longer available.");
                return Page();
            }

            return RedirectToPage("/Billing/Checkout/Confirmed");
        }

        private void ProcessCheckout()
        {
            if (CurrentCustomer == null)
            {
                CurrentCustomer = new Customer
                {
                    Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    CreditCard = Checkout.CreditCardDetails,
                    Invoices = new List<Invoice>()
                    {
                        CreateInvoice()
                    }
                };

                BillingContext.Add(CurrentCustomer);
            }
            else
            {
                if (Checkout.SaveNewCreditCardDetails)
                {
                    CurrentCustomer.CreditCard = Checkout.CreditCardDetails;
                }
                CurrentCustomer.Invoices.Add(CreateInvoice());
            }
            CurrentItem.Status = ItemStatus.Sold;
        }

        private Invoice CreateInvoice()
        {
            return new Invoice
            {
                BillingAddress = Checkout.BillingAddress,
                Details = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Ammount = 1,
                        Item = CurrentItem
                    }
                }
            };
        }
    }
}