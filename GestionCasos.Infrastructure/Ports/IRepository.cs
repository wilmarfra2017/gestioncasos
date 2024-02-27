using GestionCasos.Domain.Entities;
using MongoDB.Driver;

namespace GestionCasos.Infrastructure.Ports;

public interface IRepository<T> where T : DomainEntity
{
    Task<T> GetOneAsync(string id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(string objectId);
    Task<T> GetOneByFilterAsync(FilterDefinition<T> filter);
    Task<IEnumerable<T>> GetManyByFilterAsync(FilterDefinition<T> filter);
    Task<IEnumerable<T>> GetManyByFilterPaginatedAsync(FilterDefinition<T> filter, int paginate, int size);
    Task<double> GetTotalRecordsByFilterAsync(FilterDefinition<T> filter);

}
