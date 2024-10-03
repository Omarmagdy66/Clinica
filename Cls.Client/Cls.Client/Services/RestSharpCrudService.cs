using Api;
using RestSharp;
using System.Net.Http;

public class RestSharpCrudService : ICrudService
{
    private readonly RestClient _client;
    private readonly string baseAddress;
     
        public RestSharpCrudService(IConfiguration configuration)
        {
            baseAddress = configuration.GetSection("api")["baseurl"];
            _client = new RestClient(baseAddress);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string url)
    {
        var request = new RestRequest(url, Method.Get);
        var response = await _client.ExecuteAsync<List<T>>(request);
        return response.IsSuccessful ? response.Data : new List<T>();
    }

    public async Task<T> GetByIdAsync<T>(string url, int id)
    {
        var request = new RestRequest($"{url}/{id}", Method.Get);
        var response = await _client.ExecuteAsync<T>(request);
        return response.IsSuccessful ? response.Data : default;
    }

    public async Task<bool> CreateAsync<T>(string url, T entity)
    {
        var request = new RestRequest(url, Method.Post);
        request.AddBody(entity); // Updated for newer RestSharp
        var response = await _client.ExecuteAsync(request);
        return response.IsSuccessful;
    }

    public async Task<bool> UpdateAsync<T>(string url, int id, T entity)
    {
        var request = new RestRequest($"{url}/{id}", Method.Put);
        request.AddBody(entity); // Updated for newer RestSharp
        var response = await _client.ExecuteAsync(request);
        return response.IsSuccessful;
    }

    public async Task<bool> DeleteAsync<T>(string url, int id)
    {
        var request = new RestRequest($"{url}/{id}", Method.Delete);
        var response = await _client.ExecuteAsync(request);
        return response.IsSuccessful;
    }
}
