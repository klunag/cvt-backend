using BCP.CVT.DTO;
using BCP.CVT.DTO.Appliance;
using BCP.CVT.Services.ModelDB;

namespace BCP.CVT.Services.Interface.Appliance
{
    public abstract partial class ApplianceEquipoDAO : ServiceProvider
    {
        public abstract EquipoSolicitud GetEquipoSolicitud(int id);
        public abstract Equipo GetEquipo(int equipoId);
        public abstract EquipoSoftwareBase GetEquipoSoftwareBase(int equipoId);
        public abstract Relacion GetRealcionExistente(RelacionApplianceDTO relacionApplianceDTO);
        public abstract EquipoSolicitudDTO GetEquipoSolicitudXId(int equipoId);
        public abstract TipoNotificacion GetTipoNotificacion(string notification_type);
    }
}
