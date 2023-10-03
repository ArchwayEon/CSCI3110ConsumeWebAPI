using CSCI3110ConsumeWebAPI.Models;
using System.Text.Json;

namespace CSCI3110ConsumeWebAPI.Services;

public class WebAPIPetRepository : IPetRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WebAPIPetRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ICollection<Pet>> ReadAllAsync()
    {
        var client = _httpClientFactory.CreateClient("PetAPI");
        List<Pet>? pets = null;
        // HTTP GET
        var response = await client.GetAsync("pet/all");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            pets = JsonSerializer.Deserialize<List<Pet>>(json, serializeOptions);
        }
        pets ??= new List<Pet>(); // If pets is null, create an empty list
        return pets;
    }

    public async Task<Pet?> ReadAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("PetAPI");
        Pet? pet = null;
        // HTTP GET
        var response = await client.GetAsync($"pet/one/{id}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            pet = JsonSerializer.Deserialize<Pet>(json, serializeOptions);
        }
        return pet;
    }

    public async Task<Pet?> CreateAsync(Pet pet)
    {
        var client = _httpClientFactory.CreateClient("PetAPI");
        var petData = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["Id"] = $"{pet.Id}",
            ["Name"] = $"{pet.Name}",
            ["Weight"] = $"{pet.Weight}"
        });
        var result = await client.PostAsync("pet/create", petData);
        if (result.IsSuccessStatusCode)
        {
            return pet;
        }
        return null;
    }

    public async Task UpdateAsync(int oldId, Pet pet)
    {
        var client = _httpClientFactory.CreateClient("PetAPI");
        var petData = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["Id"] = $"{pet.Id}",
            ["Name"] = $"{pet.Name}",
            ["Weight"] = $"{pet.Weight}"
        });
        var result = await client.PutAsync("pet/update", petData);

        if (!result.IsSuccessStatusCode)
        {
            // May want to do something here
        }
    }

    public async Task DeleteAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("PetAPI");
        var result = await client.DeleteAsync($"pet/delete/{id}");
        if (!result.IsSuccessStatusCode)
        {
            // May want to do something here
        }
    }


}
