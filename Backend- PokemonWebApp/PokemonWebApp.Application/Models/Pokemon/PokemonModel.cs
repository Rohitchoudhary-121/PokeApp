namespace PokemonWebApp.Application.Models.Pokemon
{
    public class PokemonModel
    {
        public string Name { get; set; }
        public string SpriteUrl { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<string> Abilities { get; set; }
        public List<string> Types { get; set; }
    }
}
