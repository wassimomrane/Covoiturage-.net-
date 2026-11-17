using Covoiturage.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Covoiturage.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. Seeding des Villes initiales si la table est vide
        if (!await context.Villes.AnyAsync())
        {
            var villes = new List<Ville>
            {
                new() { Nom = "Tunis", CodePostal = "1000" },
                new() { Nom = "Sousse", CodePostal = "4000" },
                new() { Nom = "Sfax", CodePostal = "3000" },
                new() { Nom = "Bizerte", CodePostal = "7000" },
                new() { Nom = "Gabès", CodePostal = "6000" },
                new() { Nom = "Monastir", CodePostal = "5000" },
                new() { Nom = "Nabeul", CodePostal = "8000" }
            };
            await context.Villes.AddRangeAsync(villes);
            await context.SaveChangesAsync();
        }

        // 2. Seeding des Rôles
        string[] roleNames = { "Admin", "Conducteur", "Passager" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 3. Création de votre compte Administrateur principal
        var adminEmail = "wassim.omrane02@gmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nom = "Omrane",
                Prenom = "Wassim",
                PhoneNumber = "21600000000",
                EmailConfirmed = true, // Activé immédiatement sans besoin de code OTP
                DateInscription = DateTime.UtcNow
            };

            var createAdmin = await userManager.CreateAsync(newAdmin, "Admin@2026!");
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRolesAsync(newAdmin, new[] { "Admin", "Conducteur", "Passager" });
            }
        }
    }
}