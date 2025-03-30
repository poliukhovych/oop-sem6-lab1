namespace TelephoneSwitch.Server.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
    }
}
