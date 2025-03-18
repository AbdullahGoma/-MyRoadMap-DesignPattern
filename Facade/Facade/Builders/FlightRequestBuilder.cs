using Facade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade.Builders
{
    public class FlightRequestBuilder
    {
        private readonly FlightRequest _request = new FlightRequest();

        public FlightRequestBuilder SetOrigin(string origin)
        {
            _request.Origin = origin;
            return this;
        }

        public FlightRequestBuilder SetDestination(string destination)
        {
            _request.Destination = destination;
            return this;
        }

        public FlightRequestBuilder SetDepartureDate(DateTime departureDate)
        {
            _request.DepartureDate = departureDate;
            return this;
        }

        public FlightRequestBuilder SetPassengerCount(int passengerCount)
        {
            _request.PassengerCount = passengerCount;
            return this;
        }

        public FlightRequestBuilder SetFlightNumber(string flightNumber)
        {
            _request.FlightNumber = flightNumber;
            return this;
        }

        public FlightRequestBuilder SetPayment(PaymentDetails payment)
        {
            _request.Payment = payment;
            return this;
        }

        public FlightRequestBuilder SetUser(User user)
        {
            _request.User = user;
            return this;
        }

        public FlightRequest Build()
        {
            // Validate required fields
            if (string.IsNullOrEmpty(_request.Origin))
                throw new InvalidOperationException("Origin is required.");
            if (string.IsNullOrEmpty(_request.Destination))
                throw new InvalidOperationException("Destination is required.");
            if (_request.PassengerCount <= 0)
                throw new InvalidOperationException("Passenger count must be greater than 0.");

            return _request;
        }
    }
}
