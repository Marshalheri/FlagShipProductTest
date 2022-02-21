using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared
{
    public interface IMessageProvider
    {
        string GetMessage(string code);
    }
}
