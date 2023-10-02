using FlightPlanner.Storage;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Exceptions;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public ApiController(FlightStorage storage) 
        {
            _storage = storage;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            var airports = new List<Airport>();
        
            var matchingAirport = _storage.SearchAirports(search);

            airports.Add(matchingAirport);

            return Ok(airports);
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = _storage.FindFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            var flights = _storage.SearchFlights(request);

            var pageResult = new PageResult<Flight>
            {
                Page = 0,
                TotalItems = flights.Count,
                Items = flights
            };

            try
            {
                if (request.From.Trim().ToUpper() == request.To.Trim().ToUpper())
                {
                    throw new SameAirportException();
                }
                if (string.IsNullOrEmpty(request.From) ||
                string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.DepartureDate))
                {
                    throw new WrongValuesException();
                }
            }
            catch (SameAirportException)
            {
                return BadRequest();
            }
            catch (WrongValuesException)
            {
                return BadRequest();
            }

            return Ok(pageResult);
        }
    }
}
