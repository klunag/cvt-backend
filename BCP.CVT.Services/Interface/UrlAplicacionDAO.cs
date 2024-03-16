using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class UrlAplicacionDAO : ServiceProvider
    {
        public abstract List<UrlAplicacionDTO> GetListado(PaginacionUrlApp pag, out int totalRows);
        public abstract List<UrlAplicacionDTO> GetListadoAll(PaginacionUrlApp pag, out int totalRows);
        public abstract List<CustomAutocompleteUrlFuente> GetUrlFuenteByFiltro(int? id, string nombre);
        public abstract List<UrlAplicacionEquipoDTO> GetUrlAplicacionEquipo(int id);
        public abstract bool CambiarEstado(int id, string usuario, string comentario);
        public abstract List<AplicacionCandidataDTO> GetListAppCandidatasByUrl(int id);
        public abstract bool UpdateAplicacionByUrl(int id, string usuario, string codigoAPT);
        public abstract List<UrlAplicacionDTO> GetAlertaFuncional_UrlHuerfana_Detalle(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
