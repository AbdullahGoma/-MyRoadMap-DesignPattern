using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Services
{
    public class NotificationService
    {
        public void SendConfirmation(User user)
        {
            // Simulate sending confirmation email
            Console.WriteLine($"Sending booking confirmation to {user.Email}");
        }
    }
}
