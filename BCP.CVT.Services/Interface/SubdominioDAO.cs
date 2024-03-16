using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class SubdominioDAO : ServiceProvider
    {
        public abstract List<SubdominioDTO> GetSubdom(int dominioId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditSubdom(SubdominioDTO objeto);
        public abstract SubdominioDTO GetSubdomById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<TecnologiaDTO> GetTecBySubdom(int subdomId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder,out int totalRows);
        public abstract List<TablaAsociada> ValidarRegistrosAsociados(int id);
        public abstract List<SubdominioEquivalenciaDTO> GetSubdominiosEqBySubdom(int id);
        public abstract List<SubdominioEquivalenciaDTO> GetSubdominiosEquivalentes(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<SubdominioDTO> GetSubdominio(); //Autocomplete
        public abstract List<SubdominioDTO> GetAllSubdominio();
        public abstract List<ComboSubdominioDTO> GetSubdominiosMultiSelect(); //Multiselect
        public abstract List<CustomAutocomplete> GetAllSubdominioByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetSubdominioByDominioId(int id);
        public abstract List<CustomAutocompleteSubdominio> GetSubdominiosEquivalentesByFiltro(string filtro, int id); 
        public abstract bool AsociarSubdominiosEquivalentes(int id, string equivalencia, string usuario);
        public abstract string GetMatriculaSubDominio(int id);
        public abstract bool CambiarFlagEquivalencia(int id, string usuario);
    }
}
