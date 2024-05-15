
using ChatApi.Context;
using ChatApi.Hubs;
using DefaultCorsPolicyNugetPackage;
using Microsoft.EntityFrameworkCore;

namespace ChatApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDefaultCors();
            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
          
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chat-hub");
            app.Run();
        }
    }
}
