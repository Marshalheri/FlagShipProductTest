using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs.Document
{
    public class DocumentDTO : BaseRequestValidatorDTO
    {
        public string RawData { get; set; }
        public string Extension { get; set; }

        public override bool IsValid(out string sourceModel)
        {
            sourceModel = string.Empty;
            if (string.IsNullOrEmpty(RawData))
            {
                sourceModel = "Data";
                return false;
            }
            if (string.IsNullOrEmpty(Extension))
            {
                sourceModel = "Extension";
                return false;
            }
            return true;
        }
    }
}
