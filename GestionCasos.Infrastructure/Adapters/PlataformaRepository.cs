using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Ports;
using MongoDB.Driver;

namespace GestionCasos.Infrastructure.Adapters
{
    [Repository]
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly ILogMessageService _resourceManager;
        readonly IRepository<Plataforma> _PlataformaRepository;
        public PlataformaRepository(IRepository<Plataforma> PlataformaRepository, ILogMessageService resourceManager)
        {
            _PlataformaRepository = PlataformaRepository
            ?? throw new ArgumentNullException(nameof(PlataformaRepository));
            _resourceManager = resourceManager;
        }

        public async Task<Plataforma> BuscarPorKeyAsync(string key)
        {
            try
            {
                var filter = Builders<Plataforma>.Filter.Eq(model => model.Key, key);
                return await _PlataformaRepository.GetOneByFilterAsync(filter);
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorPersistencia} {ex.Message}");
            }
        }

    }
}
