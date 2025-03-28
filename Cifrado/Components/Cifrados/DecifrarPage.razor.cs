using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cifrado.Components.Cifrados
{
    public class ModelDecifra
    {
        [Required(ErrorMessage = "la llave  es obligatoria.")]
        public string KeyToDecrypt { get; set; } = null!;
    }
    public partial class DecifrarPage
    {
        [SupplyParameterFromForm]
       private ModelDecifra decifra { get; set; } = new ModelDecifra();
        private string? DecryptedText { get; set; }
        private string? ErrorMessage { get; set; }
        private string? SuccessMessage { get; set; }
        private bool ShowDecryptedText { get; set; } = false;


        private async Task DecryptText()
        {
            ErrorMessage = null;
            SuccessMessage = null;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            var userEmail = NormalizeEmail(user.Identity?.Name);

            if (string.IsNullOrEmpty(decifra.KeyToDecrypt))
            {
                ErrorMessage = "Por favor, ingresa una clave para descifrar.";
                return;
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                ErrorMessage = "Error: No se pudo obtener el correo del usuario autenticado.";
                return;
            }


            var encriptado = await _context.Encriptados
                .Where(e => e.NormalizedEmail == userEmail && e.KeyEncript == decifra.KeyToDecrypt)
                .FirstOrDefaultAsync();

            if (encriptado == null)
            {
                ErrorMessage = "No se encontró ningún texto encriptado para esta clave.";
                ShowDecryptedText = false;
                return;
            }

            try
            {

                DecryptedText = _encriptado.Decrypt(encriptado.TextEncript, encriptado.KeyEncript);
                SuccessMessage = "Texto desencriptado exitosamente.";
                ShowDecryptedText = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al desencriptar: {ex.Message}";
                ShowDecryptedText = false;
            }
        }


        private string NormalizeEmail(string? email)
        {
            return string.IsNullOrEmpty(email)
                ? "SIN_CORREO"
                : email.Trim().ToUpperInvariant();
        }


        private void HideAlert()
        {
            ErrorMessage = null;
            SuccessMessage = null;
            ShowDecryptedText = false;
        }
    }
}
