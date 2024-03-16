using BCP.CVT.DTO;
using BCP.CVT.DTO.ITManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface.ITManagement
{
    public abstract class SigaMgtDAO : ServiceProvider
    {
        public abstract EmployeeSigaMgtDTO GetEmployeeSigaMgtDTOByMatricula(string matricula);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeProductoPorFiltro(string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadProductoPorFiltro(string codigoUnidad, string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeTecnologiaPorFiltro(string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadTecnologiaPorFiltro(string codigoUnidad, string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeTecnologiaKPIPorFiltro(string filtro);
        public abstract List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadTecnologiaKPIPorFiltro(string codigoUnidad, string filtro);
    }
}
