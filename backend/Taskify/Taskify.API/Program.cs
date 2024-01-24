
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Taskify.Core.DbModels;
using Taskify.DAL;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Repositories;

namespace Taskify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Repositories
            builder.Services.AddScoped<IDataRepository<User>, UserRepository>();

            // Add services to the container.

            // Add Identity services
            //builder.Services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<DataContext>()
            //    .AddDefaultTokenProviders();

            builder.Services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<DataContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                option.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            // DB
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Taskify.DAL"));
            });

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapIdentityApi<User>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // Cors
            app.UseCors("AllowAllHeaders");

            app.Run();
        }
    }
}
