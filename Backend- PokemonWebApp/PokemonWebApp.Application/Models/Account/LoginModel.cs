using System.ComponentModel.DataAnnotations;

namespace PokemonWebApp.Application.Models.Account
{
    public class LoginModel
    {
        [Required]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
