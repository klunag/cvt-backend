using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class AlertaDAO : ServiceProvider
    {
        public abstract List<AlertaDTO> GetAlertasXTipo(int idTipoAlerta, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<AlertaDetalleDTO> GetAlertasDetalle(int idAlerta, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<AlertaDetalleDTO> GetAlertasNoCriticas(DateTime fechaConsulta);

        public abstract List<AlertaDetalleDTO> GetAlertasTecnicasExportar(DateTime fechaConsulta, string sortName, string sortOrder);

        public abstract AlertaDTO GetAlertaFuncional_TecnologiasEstadoPendiente();
        public abstract AlertaDTO GetAlertaFuncional_TecnologiasSinEquivalencias();
        public abstract AlertaDTO GetAlertaFuncional_EquiposSinSistemaOperativo(DateTime fechaConsulta);
        public abstract AlertaDTO GetAlertaFuncional_EquiposSinTecnologias(DateTime fechaConsulta);
        public abstract AlertaDTO GetAlertaFuncional_EquiposSinRelaciones(string TipoEquipoIdList, int TipoAlertaId);
        public abstract AlertaDTO GetAlertaFuncional_TecnologiasSinFechaSoporte();
        public abstract AlertaDTO GetAlertaFuncional_UrlHuerfana();
        public abstract AlertaDTO GetAlertaFuncional_EquipoNoRegistrado();
        public abstract int AddorEditAlertaProgramacion(AlertaProgramacionDTO objRegistro);
        public abstract AlertaProgramacionDTO GetAlertaProgramacion(int id);
        public abstract List<AlertaProgramacionDTO> GetAllAlertaProgramacionAEnviar();

        public abstract void AddorEditAlertaDetalle(AlertaDetalleDTO objRegistro);
        public abstract AlertaDetalleDTO GetAlertaDetalle(int id);
        public abstract bool CambiarEstadoAlertaDetalle(int id, string usuario);
        public abstract List<AlertaDetalleDTO> GetAlertasDetalleAEnviar(int AlertaId);
        public abstract bool ActualizarAlertaFechaUltimaEjecucion(int id, string usuario);
        public abstract bool ActualizarAlertaProgramacionFechaUltimoEnvio(int id, string usuario);

        public abstract IndicadoresDto GetIndicadores();

        #region MENSAJES
        public abstract int AddMensaje(MensajeDTO objeto);
        public abstract List<MensajeDTO> GetMensaje(string matricula, string nombre, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract MensajeDTO GetMensajeById(int Id, string usuario);
        #endregion

        public abstract List<IndicadorResponsableDto> GetResponsablesIndicadores(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<AplicacionResponsableDto> GetResponsableDetalle();
        public abstract List<IndicadorResponsableDetalleDto> GetResponsableDetalle(int responsableId, int pageNumber, int pageSize, out int totalRows);

        public abstract List<NotificacionDTO> GetNotificaciones(string para, string asunto, string mesesTrimestre, int? anio, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract DataRetornoAplicacion GetAplicacionesSinInformacion(string matricula);
        public abstract NotificacionExpertosDTO GetNotificacionExpertos(string correo);
        public abstract List<NotificacionAplicacionDTO> GetNotificacionesPortafolio(string para, string asunto, string mesesTrimestre, int? anio, DateTime? fechaRegistro, int tipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
