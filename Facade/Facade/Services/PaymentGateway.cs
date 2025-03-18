using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Services
{
    public class PaymentGateway
    {
        public PaymentResult Process(PaymentDetails payment)
        {
            // Simulate payment processing
            Console.WriteLine($"Processing payment of {payment.Amount} via {payment.Method}");
            return new PaymentResult { Success = true, TransactionId = Guid.NewGuid().ToString() };
        }
    }
}
