using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Data.MockData
{
    public static class MockFlightData
    {
        public static List<Flight> GetFlights()
        {
            return new List<Flight>
        {
            new Flight
            {
                FlightNumber = "AA123",
                Airline = "American Airlines",
                Origin = "New York (JFK)",
                Destination = "Los Angeles (LAX)",
                DepartureTime = DateTime.Now.AddDays(7).AddHours(6),
                ArrivalTime = DateTime.Now.AddDays(7).AddHours(9),
                AvailableSeats = 120,
                BaseFare = 250.00m
            },
            new Flight
            {
                FlightNumber = "DL456",
                Airline = "Delta Airlines",
                Origin = "Atlanta (ATL)",
                Destination = "Chicago (ORD)",
                DepartureTime = DateTime.Now.AddDays(7).AddHours(8),
                ArrivalTime = DateTime.Now.AddDays(7).AddHours(10),
                AvailableSeats = 90,
                BaseFare = 180.00m
            },
            new Flight
            {
                FlightNumber = "UA789",
                Airline = "United Airlines",
                Origin = "San Francisco (SFO)",
                Destination = "Seattle (SEA)",
                DepartureTime = DateTime.Now.AddDays(7).AddHours(12),
                ArrivalTime = DateTime.Now.AddDays(7).AddHours(14),
                AvailableSeats = 49,
                BaseFare = 150.00m
            }
        };
        }
    }
}
