namespace Crypto.Bot.Domain.Entity
{
    public class PriceAlert : IEntity
    {
        public string _Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public PriceAlertType Type { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
    }
}
