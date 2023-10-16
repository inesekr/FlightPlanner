using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlightPlanner.Models;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using AutoMapper;
using FlightPlanner.Core.Interfaces;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IValidate> _validators;
        
        private static readonly object _locker = new();
       
        public AdminApiController(
            IFlightService flightService,
            IMapper mapper, 
            IEnumerable<IValidate> validators)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validators = validators;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightRequest>(flight));
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(FlightRequest request)
        {
            lock (_locker)
            {
                var flight = _mapper.Map<Flight>(request);

                if (!_validators.All(v => v.IsValid(flight)))
                {
                    return BadRequest();
                }

                if (_flightService.Exists(flight))
                {
                    return Conflict();
                }

                _flightService.Create(flight);

                request = _mapper.Map<FlightRequest>(flight);

                return Created("", request);
            }   
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (_locker)
            {
                var flightToDelete = _flightService.GetFullFlightById(id);
                if (flightToDelete == null)
                {
                    return Ok();
                }
                else
                {
                    _flightService.Delete(flightToDelete);

                    return Ok();
                }
            }
        }
    }
}
