using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Services
{
    public class DynamicPricingEngine
    {
        public decimal CalculateFare(List<Flight> flights, int passengerCount)
        {
            if (!flights.Any())
                throw new InvalidOperationException("No flights available for pricing.");

            var flight = flights.First();
            decimal baseFare = flight.BaseFare;

            // Simulate dynamic pricing based on demand
            decimal demandMultiplier = 1.0m;
            if (flight.AvailableSeats < 50)
                demandMultiplier = 1.5m; // High demand
            else if (flight.AvailableSeats < 100)
                demandMultiplier = 1.2m; // Moderate demand

            decimal totalFare = baseFare * demandMultiplier * passengerCount;
            Console.WriteLine($"Calculated fare: ${totalFare} for {passengerCount} passengers.");
            return totalFare;
        }
    }
}
