using FlagshipProductTest.Client.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace FlagshipProductTest.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("BasicPolicy", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers();
            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddDependencies(Configuration);
            services.AddSwaggerGenHandler();
            services.AddHttpClientHandler();
            services.AddJwtAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
            }

            if (Configuration.GetValue<bool>("SystemSettings:UseSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(s =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(s.RoutePrefix) ? "." : "..";
                    s.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Flagship Product Test APIs");
                });
            }

            app.UseRouting();
            app.UseCors("BasicPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
