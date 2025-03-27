using LibraryMVC.Domain.Entities.Interfaces;
using LibraryMVC.Infrastructure.Extensions;
using System.Collections;
using System.Reflection;

namespace LibraryMVC.Infrastructure.Persistence;

public class FileRepository<TEntity> : IDisposable 
    where TEntity : class, IEntity, new()
{
    private readonly string _filePath;
    private List<TEntity> _cache;
    private readonly char _propertiesSeparator = '|';
    private readonly char _collectionItemsSeparator = '#';
    private bool _disposed = false;
    private bool _isDirty;

    public FileRepository(string filePath)
    {
        this._filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        this._cache = new();
        this._isDirty = false;

        if (!File.Exists(filePath))
            File.Create(filePath).Dispose();

        LoadCacheFromFile();
    }
    
    public void Create(TEntity entity)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        this._cache.Add(entity);
        this._isDirty = true;
    }

    public List<TEntity> Get(Predicate<TEntity> filter = null, string sortBy = null, bool ascending = true, int page = 1, int pageSize = 10)
    {
        var data = this._cache.AsEnumerable();

        if (filter is not null)
            data = data.Where(new Func<TEntity, bool>(filter));

        if (!string.IsNullOrEmpty(sortBy))
        {
            var property = typeof(TEntity).GetProperty(sortBy);

            if (property is not null)
                data = ascending ? data.OrderBy(x => property.GetValue(x)) : 
                                   data.OrderByDescending(x => property.GetValue(x));
        }

        return data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public void Update(Predicate<TEntity> filter, TEntity updatedEntity)
    {
        int index = this._cache.FindIndex(filter);

        if (index >= 0)
        {
            updatedEntity.Id = this._cache[index].Id;
            this._cache[index] = updatedEntity;
            this._isDirty = true;
        }
    }

    public void Delete(Predicate<TEntity> filter)
    {
        this._cache = this._cache.Where(x => !filter(x)).ToList();
        this._isDirty = true;
    }
    

    private void LoadCacheFromFile()
    {
        this._cache.Clear();
        var lines = File.ReadAllLines(this._filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            this._cache.Add(Deserialize(line));
        }    
    }

    private TEntity Deserialize(string line)
    {
        var parts = line.Split(this._propertiesSeparator);
        var entity = new TEntity();
        var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (int i = 0; i < Math.Min(parts.Length, properties.Length); i++)
        {
            var property = properties[i];
            var part = parts[i];

            if (IsCollectionType(property.PropertyType))
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(itemType);
                var list = Activator.CreateInstance(listType);

                if (!string.IsNullOrEmpty(part))
                {
                    var items = part.Split(_collectionItemsSeparator);

                    foreach (var item in items)
                    {
                        var convertedItem = item.ChangeTypeExtended(itemType);
                        list.GetType().GetMethod("Add").Invoke(list, new[] { convertedItem });
                    }
                }
                property.SetValue(entity, list);
            }
            else
            {
                var value = string.IsNullOrEmpty(part) ? null : part.ChangeTypeExtended(property.PropertyType);
                property.SetValue(entity, value);
            }
        }

        return entity;
    }

    private string Serialize(TEntity entity)
    {
        var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var values = properties.Select(p =>
        {
            var value = p.GetValue(entity);

            if (IsCollectionType(p.PropertyType) && value != null)
            {
                var enumerable = value as IEnumerable;
                return string.Join(_collectionItemsSeparator, enumerable.Cast<object>().Select(x => x?.ToString() ?? ""));
            }
            return value?.ToString() ?? string.Empty;
        });

        return string.Join(this._propertiesSeparator, values);
    }

    private void SaveToFile()
    {
        if (this._isDirty && !this._disposed)
        {
            var lines = this._cache.Select(Serialize);
            File.WriteAllLines(this._filePath, lines);
            this._isDirty = false;
        }
    }

    private static bool IsCollectionType(Type type) 
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
                SaveToFile();

            this._disposed = true;
        }
    }

    ~FileRepository()
    {
        Dispose(false);
    }
}