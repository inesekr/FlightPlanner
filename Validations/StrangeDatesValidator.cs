using FlightPlanner.Core.Models;
using FlightPlanner.Core.Interfaces;

namespace FlightPlanner.Validations;

public class StrangeDatesValidator : IValidate
{
    public bool IsValid(Flight flight)
    {
        if (DateTime.TryParse(flight?.ArrivalTime, out var arrivalTime) &&
            DateTime.TryParse(flight?.DepartureTime, out var departureTime))
        {
            return arrivalTime > departureTime;
        }

        return false;
    }
}
