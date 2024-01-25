using Blazored.LocalStorage;

namespace DevBuddy.Services;

public class SimpleStorage<TEntity>
    where TEntity : class
{
    private readonly ILocalStorageService _localStorage;
    private readonly Type _entityType;

    public SimpleStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _entityType = typeof(TEntity);
    }

    public async ValueTask<TEntity?> GetAsync()
    {        
        try
        {
            return await _localStorage.GetItemAsync<TEntity>(_entityType.FullName);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async ValueTask SetAsync(TEntity? entity)
    {
        await _localStorage.SetItemAsync(_entityType.FullName, entity);
    }
    
}
public class SimpleStorage<TEntity, TKey>
    where TEntity : class
{
    private readonly ILocalStorageService _localStorage;
    private readonly Type _keyType;

    public SimpleStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _keyType = typeof(TKey);
    }

    public async ValueTask<TEntity?> GetAsync()
    {        
        try
        {
            return await _localStorage.GetItemAsync<TEntity>(_keyType.FullName);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async ValueTask SetAsync(TEntity? entity)
    {
        await _localStorage.SetItemAsync(_keyType.FullName, entity);
    }
    
}
