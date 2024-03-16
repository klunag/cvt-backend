using BCP.CVT.DTO;
using BCP.CVT.DTO.Auditoria;
using System;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class AuditoriaDAO : ServiceProvider
    {
        public abstract List<AuditoriaDataDTO> GetHistoricoModificacion(string Accion, string Entidad, DateTime? FechaActualizacion, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<VisitaSiteDTO> GetVisitaSite(string Matricula, string Nombre, DateTime? FechaDesde, DateTime? FechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<DTO.Auditoria.AuditoriaDTO> GetAuditoriaLista(string matricula, string entidad, string campo, string accion, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract bool RegistrarAuditoriaAPI(AuditoriaAPIDTO objRegistro);
        public abstract List<AuditoriaAPIDTO> GetAuditoriaAPILista(string APIUsuario, string APINombre, string APIMetodo, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
