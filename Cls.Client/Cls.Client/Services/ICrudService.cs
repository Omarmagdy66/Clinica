namespace Api;

public interface ICrudService
{
    Task<IEnumerable<T>> GetAllAsync<T>(string url);
    Task<T> GetByIdAsync<T>(string url, int id);
    Task<bool> CreateAsync<T>(string url, T entity);
    Task<bool> UpdateAsync<T>(string url, int id, T entity);
    Task<bool> DeleteAsync<T>(string url, int id);
}