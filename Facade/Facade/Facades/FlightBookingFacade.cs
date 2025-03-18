using Facade.Data.Models;
using Facade.Services;
using Facade.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Facades
{
    public class FlightBookingFacade
    {
        private readonly FlightSearchService _search;
        private readonly DynamicPricingEngine _pricing;
        private readonly SeatInventoryManager _inventory;
        private readonly PaymentGateway _payment;
        private readonly NotificationService _notifications;

        public FlightBookingFacade(
            FlightSearchService search,
            DynamicPricingEngine pricing,
            SeatInventoryManager inventory,
            PaymentGateway payment,
            NotificationService notifications)
        {
            _search = search;
            _pricing = pricing;
            _inventory = inventory;
            _payment = payment;
            _notifications = notifications;
        }

        public BookingResult BookFlight(FlightRequest request)
        {
            Console.WriteLine("Starting flight booking process...");

            // Step 1: Search for flights
            var flights = _search.FindFlights(request);
            if (flights.Count == 0)
                return new BookingResult { IsSuccess = false, Message = Constants.NoFlightsFound };

            // Step 2: Calculate fare
            var fare = _pricing.CalculateFare(flights, request.PassengerCount);
            request.Payment.Amount = fare;

            // Step 3: Reserve seats
            if (!_inventory.ReserveSeats(request, flights))
                return new BookingResult { IsSuccess = false, Message = Constants.SeatReservationFailed };

            // Step 4: Process payment
            var paymentResult = _payment.Process(request.Payment);
            if (!paymentResult.Success)
                return new BookingResult { IsSuccess = false, Message = Constants.PaymentFailed };

            // Step 5: Send confirmation
            _notifications.SendConfirmation(request.User);

            Console.WriteLine("Booking completed successfully!");
            return new BookingResult
            {
                IsSuccess = true,
                BookingId = paymentResult.TransactionId,
                Message = "Booking confirmed"
            };
        }
    }
}
