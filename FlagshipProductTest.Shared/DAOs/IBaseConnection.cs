using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs
{
    public interface IBaseConnection
    {
        IUnitOfWork Begin();
        void Join(IUnitOfWork token);
    }
}
