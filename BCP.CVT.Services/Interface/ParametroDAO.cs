using BCP.CVT.DTO;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ParametroDAO : ServiceProvider
    {
        public abstract List<LogsDTO> GetLogsCD(PaginaReporteLogs pag, out int totalRows);
        public abstract List<LogsDTO> GetLogsJOBS(PaginaReporteLogs pag, out int totalRows);
        public abstract List<ProcesosLogsDTO> GetProcesosLogs(PaginaReporteLogs pag, out int totalRows);
        public abstract List<CustomAutocomplete> GetTipoProceso(int estado);
        public abstract ParametroDTO GetPrametroById(int id);
        public abstract ParametroDTO ObtenerParametro(string Codigo);
        public abstract ParametroDTO ObtenerParametroApp(string Codigo);
        public abstract void ActualizarParametro(ParametroDTO parametro);
        public abstract void ActualizarParametroApp(ParametroDTO parametro);
        public abstract void ActualizarParametroByCodigo(ParametroDTO parametro, GestionCMDB_ProdEntities _ctx);
        public abstract void ActualizarParametroAppByCodigo(ParametroDTO parametro, GestionCMDB_ProdEntities _ctx);
        public abstract List<ParametroDTO> GetParametros(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract void ActualizarParametro(string parametro, string valor);
        public abstract void ActualizarParametroDatosByCodigo(ParametroDTO parametro);
      
    }
}
