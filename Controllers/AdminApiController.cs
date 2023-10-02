using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Exceptions;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly FlightStorage _storage;
        private static readonly object _locker = new();

        public AdminApiController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _storage.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(Flight flight)
        {
            lock (_locker)
            {
                try
                {
                    _storage.AddFlight(flight);
                }
                catch (SameAirportException)
                {
                    return BadRequest();
                }
                catch (WrongValuesException)
                {
                    return BadRequest();
                }
                catch (DuplicateFlightException)
                {
                    return Conflict();
                }

                return Created("", flight);
            }
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (_locker)
            {
                try
                {
                    _storage.DeleteFlight(id);
                }
                catch (FlightNotFoundException)
                {
                    return Ok();
                }

                return Ok();
            }
        }
    }
}
