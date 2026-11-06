using casaDoBolo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace casaDoBolo.Data
{
    /// <summary>
    /// Roda na inicialização: aplica as migrations, cria os perfis (roles)
    /// e o primeiro administrador (dados vindos do appsettings, seção "AdminInicial").
    /// </summary>
    public static class DbSeeder
    {
        public const string RoleAdmin = "Administrador(a)";
        public const string RoleFuncionario = "Funcionário(a)";

        public static async Task SeedAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

            // 1) Cria/atualiza o banco com as migrations
            var context = services.GetRequiredService<IESContext>();
            await context.Database.MigrateAsync();

            // 2) Cria os perfis, se ainda não existirem
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in new[] { RoleAdmin, RoleFuncionario })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 3) Cria o primeiro administrador (só se ainda não existir nenhum)
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            if ((await userManager.GetUsersInRoleAsync(RoleAdmin)).Any())
            {
                return;
            }

            var config = services.GetRequiredService<IConfiguration>();
            var email = config["AdminInicial:Email"];
            var senha = config["AdminInicial:Senha"];
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                logger.LogWarning("Seção AdminInicial não configurada: nenhum administrador foi criado.");
                return;
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Firstname = config["AdminInicial:Nome"] ?? "Administrador",
                    Lastname = config["AdminInicial:Sobrenome"] ?? "(a)"
                };

                var created = await userManager.CreateAsync(user, senha);
                if (!created.Succeeded)
                {
                    logger.LogError("Não foi possível criar o administrador inicial: {Erros}",
                        string.Join("; ", created.Errors.Select(e => e.Description)));
                    return;
                }
            }

            await userManager.AddToRoleAsync(user, RoleAdmin);
            logger.LogInformation("Administrador inicial pronto: {Email}", email);
        }
    }
}
