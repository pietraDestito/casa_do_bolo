// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using casaDoBolo.Data;
using casaDoBolo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace casaDoBolo.Areas.Identity.Pages.Account
{
    // Só o(a) administrador(a) pode criar novas contas.
    [Authorize(Roles = DbSeeder.RoleAdmin)]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Informe o nome.")]
            [Display(Name = "Primeiro Nome")]
            public string Firstname { get; set; }

            [Required(ErrorMessage = "Informe o sobrenome.")]
            [Display(Name = "Sobrenome")]
            public string Lastname { get; set; }

            [Required(ErrorMessage = "Informe o e-mail.")]
            [EmailAddress(ErrorMessage = "E-mail inválido.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Informe a senha.")]
            [StringLength(100, ErrorMessage = "A senha deve ter de {2} a {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar senha")]
            [Compare("Password", ErrorMessage = "As senhas não conferem.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Selecione a função.")]
            public string Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }

        public void OnGet()
        {
            CarregarFuncoes();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CarregarFuncoes();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(Input.Role))
            {
                ModelState.AddModelError("Input.Role", "Função inválida.");
                return Page();
            }

            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            user.Firstname = Input.Firstname;
            user.Lastname = Input.Lastname;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, Input.Role);
            if (!roleResult.Succeeded)
            {
                // não deixa uma conta "sem função" para trás
                await _userManager.DeleteAsync(user);
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            _logger.LogInformation("Conta {Email} criada com a função {Role}.", Input.Email, Input.Role);
            StatusMessage = $"Usuário {Input.Email} cadastrado como {Input.Role}.";
            return RedirectToPage();
        }

        private void CarregarFuncoes()
        {
            Input ??= new InputModel();
            Input.RoleList = _roleManager.Roles
                .Select(r => r.Name)
                .OrderBy(n => n)
                .ToList()
                .Select(n => new SelectListItem { Text = n, Value = n });
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
