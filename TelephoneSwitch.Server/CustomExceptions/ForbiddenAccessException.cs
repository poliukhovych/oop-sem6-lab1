namespace TelephoneSwitch.Server.CustomExceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Access denied") : base(message) { }
    }
}
