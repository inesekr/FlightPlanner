using FlightPlanner.Models;
using FlightPlanner.Exceptions;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flightStorage = new List<Flight>();
        private static int _id = 0;

       public Airport SearchAirports(string phrase)
        {
            phrase = phrase.Trim();
            var matchingAirportTo = _flightStorage
                .Select(flight => flight.To)
                .FirstOrDefault(airport =>
             airport.AirportCode.Contains(phrase, StringComparison.OrdinalIgnoreCase) ||
             airport.Country.Contains(phrase, StringComparison.OrdinalIgnoreCase) ||
             airport.City.Contains(phrase, StringComparison.OrdinalIgnoreCase));

            var matchingAirportFrom = _flightStorage
                .Select(flight => flight.From)
                .FirstOrDefault(airport =>
                    airport.AirportCode.Contains(phrase, StringComparison.OrdinalIgnoreCase) ||
                    airport.Country.Contains(phrase, StringComparison.OrdinalIgnoreCase) ||
                    airport.City.Contains(phrase, StringComparison.OrdinalIgnoreCase));

            if (matchingAirportTo != null)
            {
                return matchingAirportTo;
            }
            else if (matchingAirportFrom != null)
            {
                return matchingAirportFrom;
            }
            else
            {
                return null;
            }
        }
                      
        public void AddFlight(Flight flight)
        {
            if (_flightStorage.Any(f=> f.From.AirportCode == flight.From.AirportCode && 
                f.To.AirportCode == flight.To.AirportCode &&
                f.Carrier == flight.Carrier &&
                f.departureTime == flight.departureTime &&
                f.arrivalTime == flight.arrivalTime))
            {
                throw new DuplicateFlightException();
            }  

            else if (string.IsNullOrEmpty(flight.From.AirportCode) ||
                string.IsNullOrEmpty(flight.To.AirportCode) || 
                string.IsNullOrEmpty(flight.From.Country) || 
                string.IsNullOrEmpty(flight.To.Country) || 
                string.IsNullOrEmpty(flight.From.City) || 
                string.IsNullOrEmpty(flight.To.City) || 
                string.IsNullOrEmpty(flight.Carrier) || 
                string.IsNullOrEmpty(flight.departureTime) || 
                string.IsNullOrEmpty(flight.arrivalTime))
            {
                throw new WrongValuesException();
            }

            else if(flight.From.AirportCode.ToUpper().Trim() == flight.To.AirportCode.ToUpper().Trim())
            {
                throw new SameAirportException();
            }

            else if(DateTime.Parse(flight.departureTime) >= DateTime.Parse(flight.arrivalTime))
            { 
                throw new WrongValuesException(); 
            }

            flight.Id = _id++;
            _flightStorage.Add(flight);
        }

        public Flight FindFlightById(int id)
        {
            var flight = _flightStorage.FirstOrDefault(f => f.Id == id);
            //if (flight==null)
            //{
            //    throw new FlightNotFoundException();
            //}
            return flight;
        }

        public List<Flight> SearchFlights(SearchFlightRequest request)
        {
            //if (string.IsNullOrEmpty(request.From) || 
            //    string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.DepartureDate))
            //{
            //    throw new WrongValuesException();
            //}
            //if (request.From.Trim().ToUpper() == request.To.Trim().ToUpper())
            //{
            //    throw new SameAirportException();
            //}
            var flights = _flightStorage.Where(f=> 
                f.From.AirportCode == request.From && 
                f.To.AirportCode == request.To &&
                f.departureTime.Contains(request.DepartureDate)).ToList();

            return flights;
        }

        public void DeleteFlight(int id)
        {
            var flightToDelete = _flightStorage.FirstOrDefault(f => f.Id == id);

            if (flightToDelete == null)
            {
                throw new FlightNotFoundException(); 
            }
            else
            {
                _flightStorage.Remove(flightToDelete);
            }
        }

        public void Clear()
        {
            _flightStorage.Clear();
        }
    }
}
