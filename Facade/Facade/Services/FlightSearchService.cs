using Facade.Data.MockData;
using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Services
{
    public class FlightSearchService
    {
        public List<Flight> FindFlights(FlightRequest request)
        {
            Console.WriteLine($"Searching flights from {request.Origin} to {request.Destination}...");
            var flights = MockFlightData.GetFlights()
                .Where(f => f.Origin == request.Origin && f.Destination == request.Destination)
                .ToList();

            if (flights.Any())
            {
                Console.WriteLine($"{flights.Count} flights found.");
                return flights;
            }

            Console.WriteLine("No flights found.");
            return new List<Flight>();
        }
    }
}
