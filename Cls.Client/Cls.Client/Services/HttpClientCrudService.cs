 
namespace Api;
public class HttpClientCrudService : ICrudService
{
    private readonly HttpClient _httpClient;
    private readonly string baseAddress;
    public HttpClientCrudService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        baseAddress = configuration.GetSection("api")["baseurl"];
        _httpClient.BaseAddress = new Uri(baseAddress);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<IEnumerable<T>>();
    }

    public async Task<T> GetByIdAsync<T>(string url, int id)
    {
        var response = await _httpClient.GetAsync($"{url}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<T>();
    }

    public async Task<bool> CreateAsync<T>(string url, T entity)
    {
        var response = await _httpClient.PostAsJsonAsync(url, entity);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync<T>(string url, int id, T entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"{url}/{id}", entity);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync<T>(string url, int id)
    {
        var response = await _httpClient.DeleteAsync($"{url}/{id}");
        return response.IsSuccessStatusCode;
    }
}
