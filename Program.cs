using EmployeeAPI.Data;

namespace EmployeeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSingleton<DbHelper>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("OpenAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

           
            var app = builder.Build();
            app.UseCors("OpenAll");
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
