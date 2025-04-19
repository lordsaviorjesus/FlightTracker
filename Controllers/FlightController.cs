using Microsoft.AspNetCore.Mvc;

namespace flight_tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : Controller
    {
        [HttpGet]
        public IActionResult GetFlightData()
        {
            var flightDataFetcher = new FlightData();
            OpenSkyResponse data = flightDataFetcher.getFlightData();
            List<FlightRecord> flightRecords = flightDataFetcher.convertFlightRecords(data);

            flightDataFetcher.debugToConsole(flightRecords); // test debug to console
            //DEBUG works !!

            //todo: get into DB

            return Ok(data);
        }
    }
}
