using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs
{
    public class PayloadResponse<T> : BasicResponse
    {
        private T _payload;

        public PayloadResponse() : base(false)
        {

        }
        public PayloadResponse(bool isSuccessful) : base(isSuccessful)
        {

        }

        public T GetPayload()
        {
            return _payload;
        }

        public void SetPayload(T payload)
        {
            _payload = payload;
        }
    }
}
