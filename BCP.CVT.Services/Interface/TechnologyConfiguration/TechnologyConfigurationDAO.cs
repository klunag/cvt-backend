using BCP.CVT.DTO;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.ModelDB;
using System.Collections.Generic;
using System.Linq;

namespace BCP.CVT.Services.Interface.TechnologyConfiguration
{
    public abstract partial class TechnologyConfigurationDAO : ServiceProvider
    {
        public abstract List<FlujoSolicitudTecnologiaDTO> GetTechonologyConfigurationFields();
        public abstract List<SolicitudFlujoDetalleDTO> GetRequestFlowDetail(int idApplication);
        public abstract string GetNewDomainValue(int id);
        public abstract string GetNewSubDomainValue(int id);
        public abstract string GetNewValueType(int? id);
        public abstract string GetNewValueTypeLifeCycle(int? id);
        public abstract Aplicacion GetNewValueApplication(int id);
        public abstract string GetTechnologyReason(int id);
        public abstract string GetTechnologyRequest(int id);
        public abstract Tecnologia GetTechnology(int id);
        public abstract Producto GetProducto(int id);
        public abstract int GetIdTechnologyConfiguration(int id);
        public abstract string GetTechnologyEquivalence(int id, int idTecnologiaEquivalencia);
        public abstract string GetApplicationTechnology(int id);
        public abstract TecnologiaDTO GetAllTechonologyXId(int id);
        public abstract IEnumerable<TecnologiaG> GetAllTechnologyDesactivated(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, string dueno, string equipo, List<int> tipoTecIds, List<int> estObsIds, string sortName, string sortOrder, int? flagActivo);
        public abstract List<MasterDetail> GetMasterDetail(string code);
        public abstract List<MasterDetail> EstadoResolucionCambioBajaOwner();
        public abstract MasterDetail GetMasterDetailXId(string code, string keycampo);
        public abstract MasterDetail GetValidListData(string key, string value);
        public abstract MasterDetail GetTechnologyDescription(string id);
    }
}
