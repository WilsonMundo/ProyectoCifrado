using Cifrado.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Cifrado.Components.Cifrados
{
    public class EncriptaTexto
    {
        [Required(ErrorMessage = "El texto a cifrar es obligatorio.")]
        [MinLength(5, ErrorMessage = "El texto debe tener al menos 5 caracteres.")]
        public string TextToEncrypt { get; set; } = null!;             
    }
    public partial class CifradoPage
    {
        private string? GeneratedKey { get; set; } = null;
        private string? SuccessMessage { get; set; } = null;
        private string? ErrorMessage { get; set; } = null;
        private bool ShowAlert { get; set; } = false;
        [SupplyParameterFromForm]
        private EncriptaTexto encriptaTexto { get; set; } = new();
        private void ShowSuccessAlert(string key)
        {
            GeneratedKey = key;
            ShowAlert = true;
        }
        private void HideAlert()
        {
            ShowAlert = false;
            GeneratedKey = string.Empty;
            StateHasChanged();
        }

        private async Task EncryptText()
        {
            try
            {
                ShowAlert = false;
                ErrorMessage = "";


                if (string.IsNullOrEmpty(encriptaTexto.TextToEncrypt))
                {
                    ErrorMessage = "El texto para cifrar no puede estar vacío.";
                    return;
                }


                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var userEmail = NormalizeEmail(user.Identity?.Name);

                if (string.IsNullOrEmpty(userEmail))
                {
                    SuccessMessage = "Error: No se pudo obtener el correo del usuario autenticado.";
                    return;
                }


                var (encryptedText, key) = _encriptado.Encrypt(encriptaTexto.TextToEncrypt);


                var encriptado = new Encriptado
                {
                    NormalizedEmail = userEmail,
                    TextEncript = encryptedText,
                    KeyEncript = key,
                    btFecha = DateTime.Now
                };


                _context.Encriptados.Add(encriptado);
                await _context.SaveChangesAsync();


                GeneratedKey = key;
                SuccessMessage = "Texto cifrado correctamente. Guarda la clave en un lugar seguro.";

                encriptaTexto = new EncriptaTexto();

                ShowSuccessAlert(key);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

       
        private string NormalizeEmail(string? email)
        {
            return string.IsNullOrEmpty(email)
                ? "SIN_CORREO"
                : email.Trim().ToUpperInvariant();
        }
    }
}
