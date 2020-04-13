using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;

using PluralsightCourseAPI.DbContexts;
using PluralsightCourseAPI.Services;

namespace PluralsightCourseAPI
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
            services.AddControllers(config =>
            {
                config.ReturnHttpNotAcceptable = true;
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                // se ejecuta cuando el modelo es inválido
                setupAction.InvalidModelStateResponseFactory = actionContext =>
                {
                    // obtiene factory para crear ProblemDetails
                    ProblemDetailsFactory factory = actionContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();

                    // crea nuevo objeto con los detalles del problema
                    ValidationProblemDetails problemDetails = factory
                        .CreateValidationProblemDetails(
                            actionContext.HttpContext,
                            actionContext.ModelState);

                    // personaliza objeto
                    problemDetails.Detail = "See the errors field for details.";
                    problemDetails.Instance = actionContext.HttpContext.Request.Path;

                    // si existen errores y todos los argumentos de entrada
                    // fueron parseados correctamente, se trata de un erro de validación
                    if (actionContext.ModelState.ErrorCount > 0 &&
                        (actionContext as ActionExecutingContext)?.ActionArguments.Count ==
                        actionContext.ActionDescriptor.Parameters.Count)
                    {
                        // url donde obtener más detalles del problema
                        problemDetails.Type = "https://evopayments.com/tag/mexico/api/modelValidationProblems";
                        // de acuerdo al estándar, hay que regresasa 422 en caso de error por validación
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        // título genérico para el error por validaciones
                        problemDetails.Title = "One or more validation errors ocurred.";
                        // regresa un objeto de tipo application/problem+json de acuerdo al estándar
                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    // en caso de que no se pudo leer o encontrar un parámetro en 
                    // el request, regresamos un error de tipo 400 - BadRequest
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more errors on input occurred.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
                options.UseSqlite("Data Source=blogging.db"));
            // options.UseSqlServer(Configuration.GetConnectionString("CourseLibraryDB"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
