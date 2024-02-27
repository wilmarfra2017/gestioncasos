using GestionCasos.Domain.Entities;
using GestionCasos.Infrastructure.Config;
using GestionCasos.Infrastructure.Ports;
using MongoDB.Driver;

namespace GestionCasos.Infrastructure.Adapters;

public class GenericRepository<T> : IRepository<T> where T : DomainEntity
{
    private readonly IMongoCollection<T> _mongoCollection;

    public GenericRepository(IMongoClient client, ClienteMongoConfig config) : this(client, config, null)
    {
    }

    public GenericRepository(IMongoClient client, ClienteMongoConfig config, string? queryFileName)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client), "La instancia de mongo cliente no puede ser nula.");
        }

        if (config == null)
        {
            throw new ArgumentNullException(nameof(config), "La configuración para mongodb no puede ser nula.");
        }

        if (string.IsNullOrEmpty(config.NombreBaseDatos))
        {
            throw new ArgumentException("El nombre de la base de datos no puede ser nulo o vacío.", nameof(config));
        }
        var database = client.GetDatabase(config.NombreBaseDatos);
        var collection = database.GetCollection<T>(typeof(T).Name);
        _mongoCollection = collection;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _mongoCollection.InsertOneAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(string objectId)
    {
        var filter = Builders<T>.Filter.Eq<string>(model => model.Id, objectId);
        await _mongoCollection.DeleteOneAsync(filter);
    }

    public async Task<T> GetOneAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(model => model.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();

    }
    public async Task<T> GetOneByFilterAsync(FilterDefinition<T> filter)
    {
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<T>> GetManyByFilterAsync(FilterDefinition<T> filter)
    {
        return await _mongoCollection.Find(filter).ToListAsync();
    }
    public async Task<IEnumerable<T>> GetManyByFilterPaginatedAsync(FilterDefinition<T> filter, int paginate, int size)
    {
        return await _mongoCollection.Find(filter).Skip(paginate).Limit(size).ToListAsync();
    }
    public async Task<double> GetTotalRecordsByFilterAsync(FilterDefinition<T> filter)
    {
        return await _mongoCollection.Find(filter).CountDocumentsAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        GenericRepository<T>.CheckEntityForUpdate(entity);

        var filter = Builders<T>.Filter.Eq(model => model.Id, entity.Id);
        await _mongoCollection.ReplaceOneAsync(filter, entity);
        return entity;
    }

    private static void CheckEntityForUpdate(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula");
        }
        if (string.IsNullOrEmpty(entity.Id))
        {
            throw new ArgumentException("El Id de la entidad no puede ser nula o vacía", nameof(entity));
        }
    }
}
