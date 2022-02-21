using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace FlagshipProductTest.Client.Dependencies
{
    public static class HttpClientInstaller
    {
        public static void AddHttpClientHandler(this IServiceCollection services)
        {
            services.AddHttpClient("HttpMessageHandler").ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    UseDefaultCredentials = true,
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, policy) =>
                    {
                        return true;
                    }
                };
            });
        }
    }
}
