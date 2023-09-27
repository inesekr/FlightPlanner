namespace FlightPlanner.Exceptions
{
    public class DuplicateFlightException : Exception
    {
        public DuplicateFlightException() : base("Flight with equal data already exists")
        {

        }
    }
}
