
using System;
using System.Web;
using Xunit;

namespace Crypto.Bot.Domain.Command
{
    public class PriceAlertBotCommandTests
    {
        [Fact]
        public async void ParseArgumentsTest()
        {
            var url = new Uri("http://pa?c=add&n=berry data&p=10");
           var r = HttpUtility.ParseQueryString("n=berry data&p=10");
           var v = r.Get("p");
           var nv = r.Get("n");
        }

    }
}
