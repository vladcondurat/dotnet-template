using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;
using Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var defaultConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(defaultConnection, b => b.MigrationsAssembly("Infrastructure")));
            
            // Configure Identity
            services.AddIdentity<User, IdentityRole<Guid>>(options => 
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                
                // User settings
                options.User.RequireUniqueEmail = true;
                
                // Email confirmation required
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddUserManager<UserManager<User>>();
            
            // Add repositories
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>(); 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IFlashcardRepository, FlashcardRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            
            // Configure JWT settings
            var jwtKey = Environment.GetEnvironmentVariable("JwtSettings__Key")
                ?? throw new InvalidOperationException("JWT Key is not configured.");
            var jwtIssuer = Environment.GetEnvironmentVariable("JwtSettings__Issuer")
                ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var jwtAudience = Environment.GetEnvironmentVariable("JwtSettings__Audience")
                ?? throw new InvalidOperationException("JWT Audience is not configured.");
            var jwtExpiration = int.Parse(Environment.GetEnvironmentVariable("JwtSettings__ExpirationInMinutes") ?? "1440");
            var jwtRefreshExpiration = int.Parse(Environment.GetEnvironmentVariable("JwtSettings__RefreshTokenExpirationInDays") ?? "7");
            
            services.Configure<JwtSettings>(options =>
            {
                options.Key = jwtKey;
                options.Issuer = jwtIssuer;
                options.Audience = jwtAudience;
                options.ExpirationInMinutes = jwtExpiration;
                options.RefreshTokenExpirationInDays = jwtRefreshExpiration;
            });
            
            // Add services
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IGeminiService, GeminiService>();
            
            // Configure Email settings
            var sendGridApiKey = Environment.GetEnvironmentVariable("EmailSettings__SendGridApiKey")
                ?? throw new InvalidOperationException("SendGridApiKey is not configured.");
            var fromEmail = Environment.GetEnvironmentVariable("EmailSettings__FromEmail")
                ?? throw new InvalidOperationException("FromEmail is not configured.");
            var fromName = Environment.GetEnvironmentVariable("EmailSettings__FromName")
                ?? throw new InvalidOperationException("FromName is not configured.");
            
            services.Configure<EmailSettings>(options =>
            {
                options.SendGridApiKey = sendGridApiKey;
                options.FromEmail = fromEmail;
                options.FromName = fromName;
            });
            
            // Register the appropriate email service based on environment
            if (environment.IsDevelopment())
            {
                services.AddScoped<IEmailService, EmailService>();
                // services.AddScoped<IEmailService, DevelopmentEmailService>();
            }
            else
            {
                services.AddScoped<IEmailService, EmailService>();
            }
            
            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            
            // Add health checks
            services.AddHealthChecks().AddCheck<SendGridHealthCheckService>("sendgrid");
            
            return services;
        }
    }
}