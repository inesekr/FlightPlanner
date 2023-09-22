using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Storage;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiController : ControllerBase
    {
        private readonly FlightStorage _storage;
        public CleanupApiController()
        {
            _storage = new FlightStorage();
        }
        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            _storage.Clear();
            return Ok();
        }
    }
}
