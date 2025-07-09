using Moq;
using Moq.Protected;
using PokemonWebApp.Application.Models.Pokemon;
using System.Net;
using System.Net.Http.Json;

public class PokemonServiceTests
{
    // Helper to create a mocked HttpClient
    private HttpClient CreateMockHttpClient(HttpResponseMessage responseMessage, Func<HttpRequestMessage, bool> requestMatcher = null)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => requestMatcher == null || requestMatcher(req)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();
        return new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
    }

    [Fact]
    public async Task GetPokemonByNameOrIdAsync_ReturnsMappedPokemonModel_WhenApiReturnsValidJson()
    {
        // Arrange: mock a valid API response
        var pokeApiResponse = new PokeApiResponseModel
        {
            name = "pikachu",
            height = 4,
            weight = 60,
            sprites = new Sprites { front_default = "http://sprite.url/pikachu.png" },
            abilities = new List<AbilityWrapper> { new AbilityWrapper { ability = new NamedApiResource { name = "static" } } },
            types = new List<TypeWrapper> { new TypeWrapper { type = new NamedApiResource { name = "electric" } } }
        };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(pokeApiResponse)
        };
        var httpClient = CreateMockHttpClient(responseMessage);
        var service = new PokemonService(httpClient);

        // Act
        var result = await service.GetPokemonByNameOrIdAsync("pikachu");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("pikachu", result.Name);
        Assert.Equal("http://sprite.url/pikachu.png", result.SpriteUrl);
        Assert.Equal(4, result.Height);
        Assert.Equal(60, result.Weight);
        Assert.Contains("static", result.Abilities);
        Assert.Contains("electric", result.Types);
    }

    [Fact]
    public async Task GetPokemonByNameOrIdAsync_ThrowsException_WhenApiReturnsNull()
    {
        // Arrange: mock a null API response
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null")
        };
        var httpClient = CreateMockHttpClient(responseMessage);
        var service = new PokemonService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetPokemonByNameOrIdAsync("unknown"));
    }

    [Fact]
    public async Task GetPokemonListWithSpritesAsync_ReturnsList_WhenApiReturnsValidListAndDetails()
    {
        // Arrange: mock the list and details responses
        var pokeApiListResponse = new PokeApiListResponse
        {
            count = 1,
            next = null,
            previous = null,
            results = new List<NamedApiResources> { new NamedApiResources { name = "bulbasaur", url = "https://pokeapi.co/api/v2/pokemon/1/" } }
        };
        var pokeApiDetailResponse = new PokeApiResponseModel
        {
            name = "bulbasaur",
            sprites = new Sprites { front_default = "http://sprite.url/bulbasaur.png" }
        };
        // Setup handler to return list for first call, details for second call
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(pokeApiListResponse) })
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(pokeApiDetailResponse) });
        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
        var service = new PokemonService(httpClient);

        // Act
        var result = await service.GetPokemonListWithSpritesAsync(1, 0);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("bulbasaur", result[0].Name);
        Assert.Equal("http://sprite.url/bulbasaur.png", result[0].SpriteUrl);
    }

    [Fact]
    public async Task GetPokemonListWithSpritesAsync_ReturnsEmptyList_WhenApiReturnsNull()
    {
        // Arrange: mock a null API response for the list
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null")
        };
        var httpClient = CreateMockHttpClient(responseMessage, req => req.RequestUri.ToString().Contains("pokemon?"));
        var service = new PokemonService(httpClient);

        // Act
        var result = await service.GetPokemonListWithSpritesAsync(1, 0);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}