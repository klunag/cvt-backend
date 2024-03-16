using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class TecnologiaNoRegistradaDAO: ServiceProvider
    {
        public abstract List<TecnologiaNoRegistradaDTO> GetTecNoReg(string filtro, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaNoRegistradaDTO> GetTecNoRegSP(string filtro, string equivalencia, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaNoRegistradaVistaDTO> GetTecNoRegVista(string tecnologia, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoNoRegistradoDto> GetEquipoNoRegSP(string nombre, int motivo, int origen, int? flagAprobado, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaNoRegistradaVistaDTO> GetTecNoRegSP_Detalle(string tecnologia, int tipoEquipoId);
        public abstract bool AsociarTecNoReg(int tecId, List<int> listTecNoReg, string UsuarioCreacion, string UsuarioModificacion);
        public abstract bool AsociarTecnologiasNoRegistradas(int tecId, List<ObjTecnologiaNoRegistrada> listTecNoReg, string UsuarioCreacion, string UsuarioModificacion);
        public abstract List<int> GetSubdominiosSugeridos(string nombre);
        public abstract List<TecnologiaDTO> GetTecSugeridas(string filtro, List<int> idSubSugeridos, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaNoRegistradaDTO> GetTecnologiaNoRegistradaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetTecnologiasUnicas();
        public abstract int GetDominioIdBySubdominioId(int SubdominioId);

        //Validaciones Carga Masiva
        public abstract int ExisteFamiliaByNombre(string nombre);
        public abstract int ExisteTipoByNombre(string nombre);
        public abstract int ExisteDominioByNombre(string nombre);
        public abstract int ExisteSubdominioByNombre(string nombre);
        public abstract bool ExisteClaveByNombre(string nombre);
        public abstract bool ExisteDominioBySubdominio(int iddominio, int idsubdominio);

        //public abstract List<EquipoNoRegistradoDto> GetEquipoNoRegSP(string nombre, int motivo, int origen, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        //public abstract bool UpdateFlagAprobadoEquipoNoRegSP(int equipoNoRegistradoId, bool flagAprobado);
        //public abstract List<TecnologiaNoRegistradaQualysDto> GetEquipoTecnologiaNoRegQualysSP(string equipoStr, string tecnologiaStr, int? flagAprobadoEquipo, int? flagAprobado, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        //public abstract List<TecnologiaNoRegistradaQualysDto> GetTecnologiaNoRegQualysSP(string tecnologiaStr, int? flagAprobadoEquipo, int? flagAprobado, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, int? equipoId = null, string equipoStr = null);
        //public abstract bool UpdateFlagAprobadoTecnologiaNoRegQualysSP(int equipoId, string qualyIds, int tecnologiaId, bool flagAprobado);
        public abstract List<TecnologiaNoRegistradaQualysDto> GetTecnologiaNoRegQualysXEquipoSP(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int GetCantidadVulnerabilidadesXEquipoSP(int equipoId);
        //public abstract bool AddTecnologiaToEquipo(TecnologiaNoRegistradaQualysDto registro);
        //public abstract bool AddEquipoFromEquipoNoRegistrado(EquipoNoRegistradoDto registro);
    }
}