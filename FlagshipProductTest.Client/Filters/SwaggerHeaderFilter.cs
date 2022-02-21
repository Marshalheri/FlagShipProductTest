using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlagshipProductTest.Client.Filters
{
    public class SwaggerHeaderFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                operation = new OpenApiOperation()
                {
                    Parameters = new List<OpenApiParameter>()
                };
            }

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
        }
    }
}
