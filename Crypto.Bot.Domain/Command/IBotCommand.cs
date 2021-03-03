using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Crypto.Bot.Domain.Abstraction
{
    public interface IBotCommand
    {
        string Name { get; }
        Task Execute(Message message);
    }
}
