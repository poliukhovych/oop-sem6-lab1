namespace TelephoneSwitch.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Auth0Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public required string Role { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
    }
}
