using Facade.Builders;
using Facade.Data.MockData;
using Facade.Facades;
using Facade.Services;

namespace Facade
{
    // Flight Booking System
    // FlightBookingFacade is a coordinator
    // 1- The coordinator doesn't perform the actual work
    // means: The FlightBookingFacade delegates flight search to
    // FlightSearchService and payment processing to PaymentGateway.
    // 2- Manages Workflow The FlightBookingFacade ensures that flights are
    // searched before calculating the fare and that seats are reserved before processing payment.
    // 3- Handles Errors: If the flight search returns no results,
    // the FlightBookingFacade stops the process and returns an appropriate error message.
    // 4- Simplifies Client Interaction:The client only interacts with the
    // FlightBookingFacade to book a flight, without needing to know about FlightSearchService,
    // PaymentGateway, etc.
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize subsystems
            var search = new FlightSearchService();
            var pricing = new DynamicPricingEngine();
            var inventory = new SeatInventoryManager();
            var payment = new PaymentGateway();
            var notifications = new NotificationService();

            // Create facade
            var facade = new FlightBookingFacade(search, pricing, inventory, payment, notifications);

            // Build FlightRequest using Builder Pattern
            var request = new FlightRequestBuilder()
                .SetOrigin("New York (JFK)")
                .SetDestination("Los Angeles (LAX)")
                .SetDepartureDate(DateTime.Now.AddDays(7))
                .SetPassengerCount(2)
                .SetFlightNumber("AA123")
                .SetPayment(MockPaymentData.GetPayments().First())
                .SetUser(MockUserData.GetUsers().First())
                .Build();

            // Book flight
            var result = facade.BookFlight(request);

            // Output result
            Console.WriteLine(result.IsSuccess
                ? $"Booking successful! ID: {result.BookingId}"
                : $"Booking failed: {result.Message}");
        }
    }
}
