using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Exceptions;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Services;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly IFlightService _flightService;
        public ApiController(IAirportService airportService, IFlightService flightService)
        {
            _airportService = airportService;
            _flightService = flightService;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            var airports = new List<AirportResult>();
            var matchingAirport = _airportService.SearchAirport(search);

            if (matchingAirport != null)
            {
                airports.Add(matchingAirport);
                return Ok(airports);
            }
              
            else
            {
                return NotFound("No matching airports found.");
            }
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            FlightResult flight = _flightService.FindFlightById(id);

            if(flight != null)
            {
                return Ok(flight);
            }

            return NotFound();
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights(FlightSearchRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.From) ||
                    string.IsNullOrEmpty(request.To) ||
                    string.IsNullOrEmpty(request.DepartureDate))
                {
                    throw new WrongValuesException();
                }

                if (request.From.Trim().ToUpper() == request.To.Trim().ToUpper())
                {
                    throw new SameAirportException();
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

            var flights = _flightService.SearchFlightsByRequest(request);

            var pageResult = new RequestPageResult
            {
                Page = 0, 
                TotalItems = flights.Count,
                Items = flights
            };

            return Ok(pageResult);           
        }
    }
}
