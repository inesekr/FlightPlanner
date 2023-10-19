namespace FlightPlanner.Exceptions
{
    public class WrongValuesException : Exception
    {
        public WrongValuesException() : base("Empty data or wrong dates")
        {

        }
    }
}
