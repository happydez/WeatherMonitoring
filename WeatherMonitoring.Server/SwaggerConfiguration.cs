using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Asp.Versioning.ApiExplorer;
using System.Reflection;

namespace WeatherMonitoring.Server
{
    public static class SwaggerConfiguration
    {
        private static string AppTitle = "WeatherMonitoring API";

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services
                .AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((options, provider) =>
                {
                    foreach (var avd in provider.ApiVersionDescriptions)
                        options.SwaggerDoc(avd.GroupName, new OpenApiInfo
                        {
                            Version = avd.GroupName,
                            Title = $"{AppTitle}"
                        });
                });

            services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();

                options.UseInlineDefinitionsForEnums();

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                options.DescribeAllParametersInCamelCase();

                options.CustomSchemaIds(x => x.FullName);

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "api.xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.UseOneOfForPolymorphism();
                options.EnableAnnotations(true, true);

                options.UseAllOfForInheritance();
                options.UseOneOfForPolymorphism();

                options.SelectSubTypesUsing(baseType =>
                    typeof(Program).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
                );

                options.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            services.AddSwaggerGenNewtonsoftSupport();
        }


        public static void UseSwagger(this WebApplication app)
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger(options => { options.RouteTemplate = "docs/{documentname}/api.yaml"; });

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddJsonFile("appsettings.Production.json")
                .Build();

            var publicUrl = configuration.GetSection("Main:PublicUrl").Value;

            app.UseSwaggerUI(
                options =>
                {
                    options.RoutePrefix = "docs";
                    provider.ApiVersionDescriptions.ToList().ForEach(
                        description =>
                            options.SwaggerEndpoint(
                                publicUrl + $"/docs/{description.GroupName}/api.yaml",
                                description.GroupName.ToUpperInvariant())
                    );

                    options.DocExpansion(DocExpansion.List);
                    options.DefaultModelsExpandDepth(-1);
                    options.OAuthAppName(AppTitle);
                }
            );
        }
    }
}
