using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class DependenciasAppsDAO: ServiceProvider
    {
        public abstract List<DependenciasAppsDTO> ListaDependenciasApps(PaginacionDependenciasApps pag, out int totalRows);
        public abstract string RegistrarDependenciasApps(string codigoAPT, bool activo, bool flagProceso);
        public abstract string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso);
        public abstract List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows);
        public abstract EtiquetasDTO ObtenerEtiquetas(PaginacionEtiquetas pag);
        public abstract string ProcesarEtiquetas(int? etiquetaId, string descripcion,  bool activo, bool flagDefault);
        public abstract List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows);
        public abstract TiposRelacionDTO ObtenerTiposRelacion(PaginacionTiposRelacion pag);
        public abstract string ProcesarTipoRelacion(int? tipoRelacionId, string descripcion, bool activo, bool porDefecto);



    }
}
