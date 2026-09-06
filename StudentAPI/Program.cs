using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Data;
using StudentAPI.Repositories;
using StudentAPI.Services;
using System.Text;
using StudentAPI.Middleware;

namespace StudentAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ EF Core
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // ✅ Repositories
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

            // ✅ Services
            builder.Services.AddScoped<IStudentService, StudentService>();

            // ✅ Lifetime Services
            builder.Services.AddSingleton<ISingletonService, SingletonService>();
            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();

            // ✅ JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["JwtSettings:SecretKey"]!))
                };
            });


            // ✅ CORS — নতুন যোগ করলাম
            builder.Services.AddCors(options =>
            {
                // Policy 1 — শুধু React App কে allow
                options.AddPolicy("ReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                // Policy 2 — সবাইকে allow (Development এ)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });




            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>(); // Custom Exception Handling Middleware
            app.UseHttpsRedirection();
            app.UseCors("AllowAll"); // CORS Policy apply করলাম
            app.UseAuthentication(); 
            app.UseAuthorization();  
            app.MapControllers();
            app.Run();
        }
    }
}