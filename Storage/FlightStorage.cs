using FlightPlanner.Models;
using FlightPlanner.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private readonly FlightPlannerDbContext _context;

        public FlightStorage(FlightPlannerDbContext context)
        {
            _context = context;
        }
        private static int _id = 0;

        public Airport SearchAirports(string phrase)
        {
            phrase = phrase.Trim().ToLower(); 
            var matchingAirport = _context.Airports
                .FirstOrDefault(airport =>
                    airport.AirportCode.ToLower().Contains(phrase) ||
                    airport.Country.ToLower().Contains(phrase) ||
                    airport.City.ToLower().Contains(phrase));

            if (matchingAirport != null)
            {
                return matchingAirport;
            }

            else
            {
                return null;
            }
        }
                      
        public void AddFlight(Flight flight)
        {
            if (_context.Flights.Any(f=> f.From.AirportCode == flight.From.AirportCode && 
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

            _context.Flights.Add(flight);
            _context.SaveChanges();
        }

       public Flight FindFlightById(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);

            return flight;
        }

        public Flight GetFlight(int id)
        {
            var flight = _context.Flights
              .Include(f => f.From)
              .Include(f => f.To)
              .SingleOrDefault(f => f.Id == id);
            return flight;
        }

        public List<Flight> SearchFlights(SearchFlightRequest request)
        {
            var flights = _context.Flights.Where(f=> 
                f.From.AirportCode == request.From && 
                f.To.AirportCode == request.To &&
                f.departureTime.Contains(request.DepartureDate)).ToList();

            return flights;
        }

        public void DeleteFlight(int id)
        {
            var flightToDelete = _context.Flights.FirstOrDefault(f => f.Id == id);

            if (flightToDelete == null)
            {
                throw new FlightNotFoundException(); 
            }
            else
            {
                _context.Flights.Remove(flightToDelete);
                _context.SaveChanges();
            }
        }

        public void Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
        }
    }
}
