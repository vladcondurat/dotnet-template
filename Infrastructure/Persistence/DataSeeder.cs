using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        
        try
        {
            logger.LogInformation("Starting database seeding...");
            
            // First seed roles
            await SeedRolesAsync(scope.ServiceProvider, logger);
            
            // Then seed admin user (which depends on roles)
            await SeedAdminUserAsync(scope.ServiceProvider, logger);
            
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database seeding.");
            throw; // Re-throw to ensure the error is visible
        }
    }
    
    private static async Task SeedRolesAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        logger.LogInformation("Starting role seeding...");
        
        // Define all roles the application needs
        string[] roleNames = { "Admin", "User", "Premium" };
        
        foreach (var roleName in roleNames)
        {
            // Check if role already exists
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            
            if (!roleExists)
            {
                logger.LogInformation($"Creating role: {roleName}");
                
                // Create the role
                var role = new IdentityRole<Guid>(roleName);
                var result = await roleManager.CreateAsync(role);
                
                if (result.Succeeded)
                {
                    logger.LogInformation($"Role '{roleName}' created successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError($"Failed to create role '{roleName}': {errors}");
                    throw new Exception($"Role creation failed: {errors}");
                }
            }
            else
            {
                logger.LogInformation($"Role '{roleName}' already exists.");
            }
        }
        
        logger.LogInformation("Role seeding completed.");
    }
    
    private static async Task SeedAdminUserAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        
        logger.LogInformation("Starting admin user seeding...");
        
        // Admin user details
        const string adminEmail = "admin@learnbuddy.com";
        const string adminUsername = "admin";
        const string adminPassword = "Admin@123!";
        
        // Check if admin user already exists
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            logger.LogInformation("Admin user does not exist. Creating...");
            
            // Create the admin user
            adminUser = new User
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                logger.LogInformation("Admin user created successfully.");
                
                // Add admin to the Admin role
                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                
                if (roleResult.Succeeded)
                {
                    logger.LogInformation("Admin user added to Admin role successfully.");
                }
                else
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    logger.LogError($"Failed to add Admin role to admin user: {errors}");
                    throw new Exception($"Adding admin to role failed: {errors}");
                }
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError($"Failed to create admin user: {errors}");
                throw new Exception($"Admin user creation failed: {errors}");
            }
        }
        else
        {
            logger.LogInformation("Admin user already exists.");
            
            // Ensure admin is in Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                
                if (roleResult.Succeeded)
                {
                    logger.LogInformation("Added existing admin user to Admin role.");
                }
                else
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    logger.LogError($"Failed to add existing admin user to Admin role: {errors}");
                }
            }
        }
        
        logger.LogInformation("Admin user seeding completed.");
    }
} 