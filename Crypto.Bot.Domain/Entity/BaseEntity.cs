using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto.Bot.Domain.Entity
{
    public interface IEntity
    {
        string _Id { get; set; }
        long ChatId { get; set; }
        string Name { get; set; }
    }
}
