using EntityFrameworkVerificationApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.Models
{
    public class CheckoutModel
    {
        public CreditCard CreditCardDetails { get; set; }
        public Address BillingAddress { get; set; }
        public bool SaveNewCreditCardDetails { get; set; }
    }
}
