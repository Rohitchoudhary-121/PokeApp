using PokemonWebApp.Application.Interfaces.Pokemon;
using PokemonWebApp.Application.Models.Pokemon;
using System.Net.Http.Json;

public class PokemonService : IPokemonService
{
    private readonly HttpClient _httpClient;

    public PokemonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PokemonModel> GetPokemonByNameOrIdAsync(string nameOrId)
    {
        // Call external API
        var response = await _httpClient.GetFromJsonAsync<PokeApiResponseModel>($"pokemon/{nameOrId.ToLower()}");

        if (response == null)
            throw new Exception("Pokémon not found");

        // Map to your DTO
        return new PokemonModel
        {
            Name = response.name,
            SpriteUrl = response.sprites.front_default,
            Height = response.height,
            Weight = response.weight,
            Abilities = response.abilities.Select(a => a.ability.name).ToList(),
            Types = response.types.Select(t => t.type.name).ToList()
        };
    }

    public async Task<List<PokemonListModel>> GetPokemonListWithSpritesAsync(int limit = 10, int offset = 0)
    {
        var response = await _httpClient.GetFromJsonAsync<PokeApiListResponse>($"pokemon?limit={limit}&offset={offset}");

        if (response == null || response.results == null)
            return new List<PokemonListModel>();

        var list = new List<PokemonListModel>();

        foreach (var item in response.results)
        {
            try
            {
                var parts = item.url.TrimEnd('/').Split('/');
                int id = int.Parse(parts[^1]);

                // Fetch details to get sprite
                var details = await _httpClient.GetFromJsonAsync<PokeApiResponseModel>($"pokemon/{id}");

                list.Add(new PokemonListModel
                {
                    Id = id,
                    Name = item.name,
                    SpriteUrl = details?.sprites?.front_default
                });
            }
            catch
            {
                // skip this Pokémon if error happens
            }
        }

        return list;
    }

}
