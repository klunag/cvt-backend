using BCP.CVT.DTO;
using BCP.PAPP.Common.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class InfoAplicacionDAO : ServiceProvider
    {
        public abstract List<InfoAplicacionDTO> PostInfoApliListado(string username, string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber, int pageSize, out int totalRows);
        public abstract List<InfoAplicacionDTO> GetConsultaValidadaHistorico(string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU, int pageNumber, int pageSize, out int totalRows);
        public abstract List<CustomAutocompleteApplication> GetInfoApliComboSoportado();
        public abstract List<CustomAutocompleteApplication> GetInfoApliComboJefe();
        public abstract List<CustomAutocompleteApplication> GetInfoApliComboLider();
        public abstract List<InfoAplicacionDTO> GetHistoricoAplicacion(string username, string codigoAPT, int pageNumber, int pageSize, out int totalRows);
        public abstract string GetRolAplicacion(string username);
        public abstract List<DetalleInforAplicacionNivel0> GetDetalleTecnicoNivel0(string codigoAPT);
        public abstract List<DetalleInforAplicacionNivel1> GetDetalleTecnicoNivel1(string username, string codigoAPT, int subdominio, int pageNumber, int pageSize, out int totalRows);
        public abstract int GetValidarInfoApp(string username, string codigoAPT);
        public abstract List<InfoAplicacionDTO> GetInfoAppExportar(string username, string soportado, string codigoAPT, string dominio, string subdominio, string comboJE, string comboLU);
    }
}
