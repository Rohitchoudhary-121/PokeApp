namespace PokemonWebApp.Application.Models.Account
{
    public class AuthResponseModel
    {
        public string AccessToken { get; set; }
        public long Expires { get; set; }
    }
}
