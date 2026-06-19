using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //reemplazo OpenApi por Swagger
            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            //registro como singleton, como se pide en el f
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMW>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi(); reemplazo por swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
