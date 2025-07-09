namespace PokemonWebApp.Application.Models.Pokemon
{
    public class PokeApiListResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<NamedApiResources> results { get; set; }
    }

    public class NamedApiResources
    {
        public string name { get; set; }
        public string url { get; set; }
    }

}
