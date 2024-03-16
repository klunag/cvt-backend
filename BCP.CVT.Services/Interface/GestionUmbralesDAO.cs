using BCP.CVT.DTO;
using BCP.CVT.DTO.Graficos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class GestionUmbralesDAO : ServiceProvider
    {
        public abstract List<CustomAutocomplete> GetTiposByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetTipoValorByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDriverByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDriverUMByFiltro(string filtro);
        public abstract List<GestionUmbralesDTO> ObtenerUmbrales(int EquipoId, string codigoAPT, int umbralId, int FlagActivo, string Matricula, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<GestionUmbralesDetalleDTO> ObtenerUmbralesDetalle(int umbralId, out int totalRows);
        public abstract List<GestionUmbralesDTO> ExportarObtenerUmbrales(int EquipoId, string CodigoAPT);
        public abstract List<GestionUmbralesDetalleDTO> ExportarObtenerUmbralesDetalle(int EquipoId, string CodigoAPT);
        public abstract List<GestionUmbralesDTO> ReporteObtenerUmbrales(int tenologiaId, string matricula);
        public abstract List<GestionUmbralesDetalleDTO> ReporteObtenerUmbralesDetalle(int tenologiaId, string matricula);
        public abstract int addOrEditUmbral(GestionUmbralesDTO objeto);
        public abstract int addOrEditUmbralArchivo(GestionUmbralesDTO objeto);
        public abstract int editUmbralComponenteCross(GestionUmbralesDTO objeto);


        //Nuevo
        public abstract List<CustomAutocomplete> GetAppsApisByFiltro(string filtro);
        public abstract List<AplicacionApisDTO> GetListAplicacionesApisCross(string codigoAppApi, string Matricula, int PerfilId, string RolPerfil, string tipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int editUmbralComponenteCross2(GestionComponenteCrossDTO objeto);

        public abstract int addOrEditPeak(GestionPeakDTO objeto);

        public abstract List<GestionPeakDTO> ObtenerPeak(string codigoAPT, int equipoId, bool activo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<GestionPeakDTO> ExportarObtenerUmbralesPeak(int EquipoId, string CodigoAPT);
    }
}
