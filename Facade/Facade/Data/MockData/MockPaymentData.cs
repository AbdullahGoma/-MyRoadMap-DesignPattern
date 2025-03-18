using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Data.MockData
{
    public static class MockPaymentData
    {
        public static List<PaymentDetails> GetPayments()
        {
            return new List<PaymentDetails>
        {
            new PaymentDetails
            {
                Method = "CreditCard",
                CardNumber = "4111-1111-1111-1111",
                ExpiryDate = "12/25",
                CVV = "123",
                Amount = 0.00m // Will be set dynamically
            },
            new PaymentDetails
            {
                Method = "PayPal",
                CardNumber = null,
                ExpiryDate = null,
                CVV = null,
                Amount = 0.00m
            }
        };
        }
    }
}
