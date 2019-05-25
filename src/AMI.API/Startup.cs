using System.Net;
using AMI.API.Configuration;
using AMI.API.Filters;
using AMI.API.Handlers;
using AMI.API.Serializers;
using AMI.Core.Configuration;
using AMI.Core.Entities.ViewModels;
using AMI.Core.Serializers;
using AMI.Core.Strategies;
using AMI.Core.Uploaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PNL.Web.Extensions.ApplicationBuilderExtensions;

namespace AMI.API
{
    /// <summary>
    /// The startup of the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var defaultSerializer = new ExtendedJsonSerializer();

            services.AddScoped<IResumableUploader, ResumableUploader>()
                .AddSingleton<IFileSystemStrategy, FileSystemStrategy>()
                .AddSingleton<IApiConfiguration, ApiConfiguration>()
                .AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>()
                .AddTransient<IDefaultJsonSerializer, ExtendedJsonSerializer>()
                .AddTransient<IExtendedJsonSerializer, ExtendedJsonSerializer>()
                .AddTransient<ICustomExceptionHandler, CustomExceptionHandler>();

            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.BadRequest));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.Unauthorized));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.Forbidden));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.NotFound));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.Conflict));
                    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), (int)HttpStatusCode.InternalServerError));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    defaultSerializer.SetJsonSerializerSettings(options.SerializerSettings);
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
