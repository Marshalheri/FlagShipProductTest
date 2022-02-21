using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.Models
{
    public class BaseModel
    {
        public long Id { get; set; }
        public DateTime DateCreated => DateTime.Now;
    }
}
