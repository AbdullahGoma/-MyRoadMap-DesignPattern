using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Data.Models
{
    public class FlightRequest
    {
        // Flight details
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public string FlightNumber { get; set; }

        // Passenger details
        public int PassengerCount { get; set; }

        // Payment details
        public PaymentDetails Payment { get; set; }

        // User details
        public User User { get; set; }
    }
}
