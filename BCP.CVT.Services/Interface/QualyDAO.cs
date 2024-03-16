using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class QualyDAO : ServiceProvider
    {
        public abstract List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, bool withAsignadas, bool withDistinct, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<QualyDto> GetQualysVulnerabilidadesPorEquipo(string qualyId, string equipo, string tipoVulnerabilidad, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel1(string qid, string equipo, string producto);
        public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel2(string qid, string equipo, string producto, bool withEquipo);
        //public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel3(string qid, string equipo, string producto, bool withEquipo, string severidad);
        public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel3(string qid, string equipo, string producto, bool withEquipo, bool withProductoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel4(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia);
        public abstract List<QualyConsolidadoDto> GetQualysConsolidadoNivel5(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia, string tipoVulnerabilidad, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        //public abstract List<QualyConsolidadoDto> GetQualysConsolidadoExportar(string qid, string equipo, string producto);
        public abstract List<QualyConsolidadoCsvDto> GetQualysConsolidadoExportar(/*string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, int? unidadOrganizativaId, int? squadId, string equipoStr, string estadosVulnerabilidad, bool? tieneEquipoAsignado, bool? tieneProductoAsignado, bool? tieneTecnologiaAsignado, */int pageNumber, int pageSize, out int totalRows);
        public abstract QualyDto GetQualyById(int qualyId);
        public abstract bool SaveQualy(QualyDto registro);
        public abstract QualysConsolidadoKPIDto GetQualysConsolidadoKPI(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, int? unidadOrganizativaId, int? squadId, string equipoStr, string estadosVulnerabilidad, bool? tieneEquipoAsignado, bool? tieneProductoAsignado, bool? tieneTecnologiaAsignado);
        public abstract List<CustomAutocomplete> GetQualysVulnStatusList();
        public abstract bool VolcarDatosReporteQualysConsolidado();
        public abstract bool ActualizarEquipoIdEnQualysVulnServidores();
        public abstract bool TransferenciaReporteGeneralSemanaAQualysVulnServidores();
        public abstract bool TransferenciaReporteGeneralSemanaAQualy();
        public abstract QualysConsolidadoKPIDto GetAlertaQualysConsolidadoKPI();
        public abstract bool VolcarDatosKPI();
        public abstract bool VolcarDatosEquiposNoRegistrados();
        public abstract bool VolcarDatosTecnologiasInstaladasNoRegistradas();
        public abstract List<VulnerabilidadEquipoDto> GetVulnerabilidadesAplicacion(int tecnologia, int qid, string subdominios, string dominios, string gestionado, string squads, string estado, string codigoAPT, int pageNumber, int pageSize, int perfil, string matricula, out int totalRows);
    }
}
