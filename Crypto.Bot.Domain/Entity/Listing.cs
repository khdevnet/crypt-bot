namespace Crypto.Bot.Domain.Entity
{
    public class Listing : IEntity
    {
        public string _Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public bool Sent { get; set; }
    }
}
