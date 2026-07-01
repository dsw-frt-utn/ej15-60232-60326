using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=Dsw2026Ej16;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;";

            // Add services to the container.
            builder.Services.AddDbContext<Dsw2026Ej15DbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }); // registro el dbContext con DI con opcion de usar el motor de SqlServer

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(); // Swagger en vez de openApi
            
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>(); // Registro en Singleton

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMW>();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
