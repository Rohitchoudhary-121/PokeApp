using Moq;
using PokemonWebApp.Application.Interfaces.Pokemon;
using PokemonWebApp.Application.Models.Pokemon;
using System.Net;

public class PokemonControllerTests
{
    private readonly Mock<IPokemonService> _mockService;
    private readonly PokemonController _controller;

    public PokemonControllerTests()
    {
        _mockService = new Mock<IPokemonService>();
        _controller = new PokemonController(_mockService.Object);
    }

    [Fact]
    public async Task GetPokemon_ReturnsPokemon_WhenServiceReturnsPokemon()
    {
        // Arrange
        var pokemon = new PokemonModel
        {
            Name = "pikachu",
            SpriteUrl = "http://sprite.url/pikachu.png",
            Height = 4,
            Weight = 60,
            Abilities = new List<string> { "static" },
            Types = new List<string> { "electric" }
        };
        _mockService.Setup(s => s.GetPokemonByNameOrIdAsync("pikachu")).ReturnsAsync(pokemon);

        // Act
        var result = await _controller.GetPokemon("pikachu");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(pokemon, result.Result);
        Assert.Equal(HttpStatusCode.OK, result.ResponseStatusCode);
    }

    [Fact]
    public async Task GetPokemon_ReturnsError_WhenServiceThrowsException()
    {
        // Arrange
        _mockService.Setup(s => s.GetPokemonByNameOrIdAsync("unknown")).ThrowsAsync(new Exception("Not found"));

        // Act
        var result = await _controller.GetPokemon("unknown");

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Result);
        Assert.Equal(HttpStatusCode.BadRequest, result.ResponseStatusCode);
        Assert.Contains("Not found", result.Errors);
    }

    [Fact]
    public async Task GetPokemonListWithSprites_ReturnsList_WhenServiceReturnsList()
    {
        // Arrange
        var list = new List<PokemonListModel>
        {
            new PokemonListModel { Id = 1, Name = "bulbasaur", SpriteUrl = "http://sprite.url/bulbasaur.png" }
        };
        _mockService.Setup(s => s.GetPokemonListWithSpritesAsync(1, 0)).ReturnsAsync(list);

        // Act
        var result = await _controller.GetPokemonListWithSprites(1, 0);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(list, result.Result);
        Assert.Equal(HttpStatusCode.OK, result.ResponseStatusCode);
    }

    [Fact]
    public async Task GetPokemonListWithSprites_ReturnsError_WhenServiceThrowsException()
    {
        // Arrange
        _mockService.Setup(s => s.GetPokemonListWithSpritesAsync(1, 0)).ThrowsAsync(new Exception("API error"));

        // Act
        var result = await _controller.GetPokemonListWithSprites(1, 0);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Result);
        Assert.Equal(HttpStatusCode.BadRequest, result.ResponseStatusCode);
        Assert.Contains("API error", result.Errors);
    }
}