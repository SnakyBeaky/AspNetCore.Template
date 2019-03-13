using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCore.Template.API
{
    public class Startup
    {
        #region Static Methods

        /// <summary>
        /// Entry point of the API.
        /// </summary>
        private static async Task Main(string[] args)
        {
            try
            {
                await WebHost.CreateDefaultBuilder(args)
                    .UseSerilog(ConfigureSerilog)
                    .UseStartup<Startup>()
                    .Build()
                    .RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString());
            }
        }

        /// <summary>
        /// Configures Serilog to read the MinimumLevel overrides and other settins grom the appsettings.json.
        /// </summary>
        private static void ConfigureSerilog(WebHostBuilderContext context, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    path: Path.Combine(AppContext.BaseDirectory, "log.txt"),
                    shared: true,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null);
        }

        #endregion

        #region ASP.NET Core Bootstrapping

        /// <summary>
        /// Adds services to the container.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            AddSwagger(services);

            RegisterServices(services);
        }
        
        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime.
        /// </remarks>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            UseSwagger(app);
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Adds and configures swagger documentation generation.
        /// </summary>
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Namespace}.xml"));

                options.SwaggerDoc("v1", new Info() { Title = typeof(Startup).Namespace, Version = "v1" });
            });
        }

        /// <summary>
        /// Configures the SwaggerUI and the endpoint to get the data from.
        /// </summary>
        private void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{typeof(Startup).Namespace} v1");
            });
        }

        /// <summary>
        /// Registers the custom dependencies of the application.
        /// </summary>
        private void RegisterServices(IServiceCollection services)
        {
            // Register all dependencies here
        }

        #endregion
    }
}
