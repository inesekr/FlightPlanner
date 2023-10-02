﻿using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        [StringLength(100)]
        public string Carrier { get; set; }
        [StringLength(100)]
        public string departureTime { get; set; }
        [StringLength(100)]
        public string arrivalTime { get; set; }

    }
}
