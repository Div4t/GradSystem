using Microsoft.AspNetCore.Identity;

namespace GradSystem.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Crear rol Admin si no existe
        const string rolAdmin = "Admin";
        if (!await roleManager.RoleExistsAsync(rolAdmin))
        {
            await roleManager.CreateAsync(new IdentityRole(rolAdmin));
        }

        // Crear usuario admin por defecto si no existe
        const string emailAdmin    = "admin@grad.com";
        const string passwordAdmin = "Admin1234!";

        var adminExistente = await userManager.FindByEmailAsync(emailAdmin);
        if (adminExistente is null)
        {
            var nuevoAdmin = new IdentityUser
            {
                UserName       = emailAdmin,
                Email          = emailAdmin,
                EmailConfirmed = true
            };

            var resultado = await userManager.CreateAsync(nuevoAdmin, passwordAdmin);
            if (resultado.Succeeded)
            {
                await userManager.AddToRoleAsync(nuevoAdmin, rolAdmin);
            }
        }
    }
}
