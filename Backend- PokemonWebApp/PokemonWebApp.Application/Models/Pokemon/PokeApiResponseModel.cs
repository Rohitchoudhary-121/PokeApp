namespace PokemonWebApp.Application.Models.Pokemon
{
    public class PokeApiResponseModel
    {
        public string name { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public Sprites sprites { get; set; }
        public List<AbilityWrapper> abilities { get; set; }
        public List<TypeWrapper> types { get; set; }
    }

    public class Sprites
    {
        public string front_default { get; set; }
    }

    public class AbilityWrapper
    {
        public NamedApiResource ability { get; set; }
    }

    public class TypeWrapper
    {
        public NamedApiResource type { get; set; }
    }

    public class NamedApiResource
    {
        public string name { get; set; }
    }
}
