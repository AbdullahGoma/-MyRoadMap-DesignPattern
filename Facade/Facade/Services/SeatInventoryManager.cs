using Facade.Data.Models;

namespace Facade.Services
{
    public class SeatInventoryManager
    {
        public bool ReserveSeats(FlightRequest request, List<Flight> flights)
        {
            var flight = flights.FirstOrDefault(f => f.FlightNumber == request.FlightNumber);
            if (flight == null)
            {
                Console.WriteLine("Flight not found.");
                return false;
            }

            if (flight.AvailableSeats < request.PassengerCount)
            {
                Console.WriteLine("Not enough seats available.");
                return false;
            }

            flight.AvailableSeats -= request.PassengerCount;
            Console.WriteLine($"Reserved {request.PassengerCount} seats on flight {flight.FlightNumber}. Remaining seats: {flight.AvailableSeats}");
            return true;
        }
    }
}
