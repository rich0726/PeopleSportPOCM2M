using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OData.UriParser;
using Microsoft.AspNetCore.Mvc;

namespace PeopleSportsSandbox
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            services.AddOData();
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection(builder =>
                {
                    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new StringAsEnumResolver() { EnableCaseInsensitive = true });
                });
                routeBuilder.Select().OrderBy().Filter().MaxTop(null).Count().Expand();
            });


            Microsoft.OData.Edm.IEdmModel GetEdmModel()
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<Person>("People");
                odataBuilder.EnumType<Sport>();
                var returnModel = odataBuilder.GetEdmModel();
                return returnModel;
            }

          

        }

    }
}
