using FlagshipProductTest.Shared;
using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;

namespace FlagshipProductTest.Client.Filters
{
    public class ValidateRequestBodyFilter<T> : IAsyncActionFilter where T : BaseRequestValidatorDTO
    {
        public T RequestDataType { get; set; }
        private const string REQUEST = "request";
        private readonly IMessageProvider _messageProvider;
        private readonly ILogger<ValidateRequestBodyFilter<T>> _logger;
        private readonly SystemSettings _settings;

        public ValidateRequestBodyFilter(IMessageProvider messageProvider, ILogger<ValidateRequestBodyFilter<T>> logger,
                IOptions<SystemSettings> settings)
        {
            _messageProvider = messageProvider;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue(REQUEST, out var output))
            {
                context.Result = new ObjectResult(
                    new ErrorResponse
                    {
                        ErrorCode = ResponseCodes.INVALID_INPUT_PARAMETER,
                        Description = _messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER)
                    })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            var request = output as T;
            _logger.LogInformation($"{typeof(T).Name} SANITIZED REQUEST ==> {Util.SerializeAsJson(request.Clone().Sanitize(_settings.PropertiesToSanitize))}");
            if (!request.IsValid(out string problemSource))
            {
                context.Result = new ObjectResult(
                    new ErrorResponse
                    {
                        ErrorCode = ResponseCodes.INVALID_INPUT_PARAMETER,
                        Description = $"{_messageProvider.GetMessage(ResponseCodes.INVALID_INPUT_PARAMETER)} - {problemSource}"
                    })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            await next();
        }
    }
}
