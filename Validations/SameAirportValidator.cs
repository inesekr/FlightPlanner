﻿using FlightPlanner.Core.Models;
using FlightPlanner.Core.Interfaces;

namespace FlightPlanner.Validations;

public class SameAirportValidator : IValidate
{
    public bool IsValid(Flight flight)
    {
        return flight?.To?.AirportCode?.Trim()?.ToLower() !=
                flight?.From?.AirportCode?.Trim()?.ToLower();
    }
}
