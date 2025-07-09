using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApp.Application.Interfaces.Pokemon;
using PokemonWebApp.Application.Models.Common;
using PokemonWebApp.Application.Models.Pokemon;
using System.Net;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    /// <summary>
    /// Get pokemon by id or name.
    /// </summary>
    /// <returns>Pokemon Model Result</returns>
    /// <param name="nameOrId">Name or id of pokemon</param>
    [HttpGet("{nameOrId}")]
    public async Task<GenericBaseResult<PokemonModel>> GetPokemon(string nameOrId)
    {
        try
        {
            if(string.IsNullOrEmpty(nameOrId))
                throw new ArgumentNullException("Name or id is required.");

            var pokemon = await _pokemonService.GetPokemonByNameOrIdAsync(nameOrId);
            return new GenericBaseResult<PokemonModel>(pokemon);
        }
        catch (Exception ex)
        {
            return new GenericBaseResult<PokemonModel>(null)
            {
                ResponseStatusCode = HttpStatusCode.BadRequest,
                Errors = new List<string> { ex.Message },
                Message = ex.Message
            };
        }
    }

    /// <summary>
    /// Get pokemon list.
    /// </summary>
    /// <returns>Pokemon List Result</returns>
    [Authorize]
    [HttpGet("list-with-images")]
    public async Task<GenericBaseResult<List<PokemonListModel>>> GetPokemonListWithSprites([FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        try
        {
            var response = await _pokemonService.GetPokemonListWithSpritesAsync(limit, offset);
            return new GenericBaseResult<List<PokemonListModel>>(response);
        }
        catch (Exception ex)
        {
            return new GenericBaseResult<List<PokemonListModel>> (null)
            {
                ResponseStatusCode = HttpStatusCode.BadRequest,
                Errors = new List<string> { ex.Message },
                Message = ex.Message
            };
        }
    }

}
