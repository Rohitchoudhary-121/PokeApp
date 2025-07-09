using System.ComponentModel.DataAnnotations;

namespace PokemonWebApp.Application.Models.Account
{
    public class RegisterUserModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
