using Microsoft.AspNetCore.Identity;
using pepeizqs_deals_web.Data;

namespace pepeizqs_deals_blazor_web.Componentes.Account
{
    internal sealed class UsuarioAcceso(UserManager<Usuario> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<Usuario> GetRequiredUserAsync(HttpContext contexto)
        {
            Usuario? usuario = await userManager.GetUserAsync(contexto.User);

            if (usuario is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(contexto.User)}'.", contexto);
            }

            return usuario;
        }
    }
}
