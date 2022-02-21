using FlagshipProductTest.Core;
using FlagshipProductTest.Core.Fakes;
using FlagshipProductTest.Core.Implementations;
using FlagshipProductTest.Shared;
using FlagshipProductTest.Shared.DAOs;
using FlagshipProductTest.Shared.DAOs.Implementations;
using FlagshipProductTest.Shared.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace FlagshipProductTest.Client.Dependencies
{
    public static class DependencyInstaller
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Services 
            services.AddScoped<IUserService, UserService>();

            if (configuration.GetValue<bool>("SystemSettings:UseMocks"))
            {
                //Fakes
                services.AddSingleton<IMessagePackProvider, FakeMessagePackProvider>();
            }
            else
            {
                //Concretes
                services.AddScoped<IMessagePackProvider, MessagePackProvider>();
            }

            //DAOs
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IDocumentDAO, DocumentDAO>();
            services.AddScoped<IUserContributionDAO, UserContributionDAO>();
            services.AddScoped<IAdminUserDAO, AdminUserDAO>();

            //Utilities
            services.AddScoped<ICryptoProvider, CryptoProvider>();
            services.AddScoped<IMessageProvider, MessageProvider>();
            services.AddScoped<IValidators, Validators>();


            services.AddScoped<IDbConnection>((s) => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
            services.Configure<SystemSettings>(opt => configuration.GetSection("SystemSettings").Bind(opt));
            services.Configure<MessagePackSettings>(opt => configuration.GetSection("MessagePackSettings").Bind(opt));
        }
    }
}
