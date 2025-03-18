using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Data.Models
{
    public class PaymentDetails
    {
        public string Method { get; set; } // e.g., "CreditCard", "PayPal"
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
    }
}
