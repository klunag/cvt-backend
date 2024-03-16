using BCP.CVT.DTO;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class NotificacionDAO : ServiceProvider
    {
        public abstract int AddOrEditNotificacion(NotificacionDTO objRegistro);
        public abstract int AddOrEditNotificacionApp(NotificacionAplicacionDTO objRegistro);
        public abstract bool ActualizarNotificacionEstado(int idNotificacion, bool estado, string usuario);        
        public abstract List<NotificacionDTO> ListarNotificacionesPendientesEnvio();
        public abstract List<NotificacionDTO> ListarNotificaciones(bool? flagEstado = null);
        public abstract List<TipoNotificacionDto> ListarNotificacionesFrecuentes();
        public abstract List<TipoNotificacionDto> ListarNotificacionesDiarias();
        public abstract List<TipoNotificacionDto> ListarNotificacionesAppDiarias();

        public abstract List<string> ListarRolOnDemandAdmins();
        public abstract List<TipoNotificacionDto> ListarTipoNotificaciones(bool? flagEstado = null);
        public abstract int AddOrEditTipoNotificacion(TipoNotificacionDto objRegistro);
        public abstract TipoNotificacionDto ObtenerTipoNotificacion(int idTipoNotificacion);
        public abstract List<CustomAutocomplete> ListarPortafolioResponsable();
        public abstract List<AplicacionResponsableDto> ListarNotificacionesResponsablesAplicaciones(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<AplicacionDTO> ListarNotificacionesResponsablesAplicacionesDetalle(int idPortafolioResponsable, string matricula, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract NotificacionResponsableAplicacionDto ObtenerNotificacionResponsableAplicacion(int idPortafolioResponsable, bool flagTTLJdE);
        public abstract int AddOrEditNotificacionResponsableAplicacion(NotificacionResponsableAplicacionDto objRegistro);
        public abstract void ActualizarFechaUltimoEnvio(int tipoNotificacion);

        public abstract List<AplicacionPortafolioResponsablesDTO> ListarNotificacionesResponsablesTTLJdE(int idPortafolioResponsable, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<NotificacionResponsableAplicacionDto> ListarNotificacionesResponsableAplicacion();
        public abstract bool ActualizarNotificacionResponsableAplicacionEstado(int idNotificacion, bool estado, string usuario);
        public abstract List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionExpertos();
        public abstract List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionUsuariosLideres(TipoNotificacionDto tipo);
        public abstract List<NotificacionResponsableAplicacionDto> ListarNotificacionesAplicacionUsuariosLideres();

        public abstract List<TipoNotificacionDto> ListarTipoNotificacionesApp(bool? flagEstado = null);
        public abstract List<TipoNotificacionDto> GetTipoNotificacionesApp(Paginacion pag, out int totalRows);
        public abstract int AddOrEditTipoNotificacionApp(TipoNotificacionDto objRegistro);
        public abstract TipoNotificacionDto ObtenerTipoNotificacionApp(int idTipoNotificacion);
        public abstract List<int> ObtenerIDsTipoNotificacionApp_NoAplicationFlow(string valor);
        public abstract int RegistrarNotificacionApp(NotificacionAplicacionDTO objRegistro, GestionCMDB_ProdEntities _ctx);

        public abstract List<NotificacionAplicacionDTO> ListarNotificacionesAppPendientesEnvio();
        public abstract bool ActualizarNotificacionAppEstado(int idNotificacion, bool estado, string usuario);
        public abstract TipoNotificacionDto ObtenerTipoNotificacionPorNombre(string nombre);
    }
}
