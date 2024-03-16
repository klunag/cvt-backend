using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class ApplianceDAO : ServiceProvider
    {
        public abstract List<EquipoSolicitudDTO> GetSolicitudes(string nombre
            , int estado
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string perfil
            , string usuario
            , out int totalRows);
        public abstract int AddOrEditSolicitud(EquipoSolicitudDTO objRegistro);
        public abstract EquipoSolicitudDTO GetSolicitudById(int id);
        public abstract EquipoSolicitudDTO GetArchivoById(int id);
        public abstract ValidacionSolicitudDTO GetValidacion(int id);
        public abstract void CambiarEstadoSolicitud(int id, int estado, string usuario, string nombres, string comentarios, string matricula_aprobador, string codigoAPT);
        public abstract void ActualizarArchivo(int id, byte[] archivo, string nombre);
        public abstract void ActualizarTipoEquipo(int equipoId, string usuario, int solicitud);
        public abstract void DesestimarSolicitud(int equipoId, string usuario, string nombres);
        public abstract void ActualizarArchivoSoftwareBase(int id, byte[] archivo, string nombre);
        public abstract void ActualizarArchivoSoftwareBase2(int id, byte[] archivo, string nombre);
        public abstract void RevertirSolicitud(int equipoId, string usuario, string nombres);
        public abstract List<EquipoSolicitudDTO> GetSolicitudPendientesXAprobarCvt(string nombre
          , int estado
          , int pageNumber
          , int pageSize
          , string sortName
          , string sortOrder
          , string perfil
          , string usuario
          , out int totalRows);
    }
}
