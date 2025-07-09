using PokemonWebApp.Application.Models.Pokemon;

namespace PokemonWebApp.Application.Interfaces.Pokemon
{
    public interface IPokemonService
    {
        Task<PokemonModel> GetPokemonByNameOrIdAsync(string nameOrId);
        Task<List<PokemonListModel>> GetPokemonListWithSpritesAsync(int limit = 10, int offset = 0);

    }
}
