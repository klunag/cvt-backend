using BCP.CVT.DTO;
using System.Collections.Generic;
using BCP.PAPP.Common.Custom;
using BCP.PAPP.Common.Dto;

namespace BCP.CVT.Services.Interface
{
    public abstract class SolicitudRolDAO : ServiceProvider
    {
        public abstract List<SolicitudRolDTO> GetFuncionSolicitudes(PaginacionSolicitud pag, out int totalRows);
        public abstract DataResultAplicacion AprobarSolicitudOwner(int id, string matricula, string nombre, string comentario);
        public abstract DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario, int idProducto);
        public abstract DataResultAplicacion AprobarSolicitudSeguridad(int id, string matricula, string nombre, string comentario);
        public abstract DataResultAplicacion ObservarSolicitudSeguridad(int id, string matricula, string nombre, string comentario, int idProducto);
        public abstract List<SolRolHistorialDTO> GetHistorialSolicitud(PaginacionSolicitud pag, out int totalRows);
        public abstract SolicitudRolDTO GetProductoObservadoById(int id);
        public abstract List<RolesProductoDTO> GetProductoRolesDetalle(PaginacionSolicitud pag, out int totalRows);
        public abstract List<FuncionDTO> GetDetalleFuncionesProductosRoles(PaginacionSolicitud pag, out int totalRows);
        public abstract List<SolicitudRolCorreosDTO> ObtenerCorreoSolicitud(int idSolicitud, int idTipo);
        public abstract List<SolicitudRolResponsablesDTO> GetResponsablesPorSolicitud(PaginacionSolicitud pag, out int totalRows);
        public abstract bool SolicitudAprobadoResponsable(int idCabecera, int idTipoSolicitud, int idTipoResponsable);
        public abstract SolicitudRolDTO GetSolicitudPorId(int id);
        public abstract List<FuncionDTO> GetDetalleSolicitudRoles(PaginacionSolicitud pag, out int totalRows);
        public abstract List<RolesProductoAmbienteDTO> GetAmbienteRolProductoByIdSolicitud(int id);

        public abstract List<RolesProductoDTO> GetProductoRolesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows);
        public abstract List<FuncionDTO> GetProductoFuncionesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows);
    }
}
