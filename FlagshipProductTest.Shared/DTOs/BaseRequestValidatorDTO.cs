using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs
{
    public class BaseRequestValidatorDTO
    {
        public virtual bool IsValid(out string problemSource)
        {
            problemSource = string.Empty;
            return true;
        }
    }
}
