namespace CraigBot.Core.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public ulong UserId { get; set; }

        public string Username { get; set; }

        public decimal Balance { get; set; }
    }
}