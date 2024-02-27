using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Ports;

namespace GestionCasos.Infrastructure.Adapters
{
    [Repository]
    public class SolicitudRepository : ISolicitudRepository
    {
        readonly IRepository<Solicitud> _solicitudRepository;
        private readonly ILogMessageService _resourceManager;
        public SolicitudRepository(IRepository<Solicitud> solicitudRepository, ILogMessageService resourceManager)
        {
            _solicitudRepository = solicitudRepository
            ?? throw new ArgumentNullException(nameof(solicitudRepository));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public async Task<Solicitud> GuardarSolicitudAsync(Solicitud solicitud)
        {
            try
            {

                return await _solicitudRepository.AddAsync(solicitud);
             
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorPersistencia} {ex.Message}");
            }
        }
        public async Task<Solicitud> ActualizarSolicitudAsync(Solicitud solicitud)
        {
            try
            {
                return await _solicitudRepository.UpdateAsync(solicitud);
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorPersistencia} {ex.Message}");
            }
        }
    }
}
