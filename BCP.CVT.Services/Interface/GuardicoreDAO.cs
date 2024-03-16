using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class GuardicoreDAO : ServiceProvider
    {
        public abstract List<GuardicoreApiDTO> GetGuardicoreConCvtEquipos(List<GuardicoreApiDTO> datos);
        public abstract List<ConnectionDto> GetGuardicoreConnectionsConCvtEquipos(List<ConnectionDto> datos);
        public abstract List<GuardicoreConsolidadoDto> GetGuardicoreConsolidado();
        public abstract List<GuardicoreConsolidadoDto> GetGuardicoreConsolidadoDetalle(int idestado, string so);
        public abstract List<GuardicoreConsolidadoDto> GetGuardicoreGrupoSO(int estado);
        public abstract List<GuardicoreConsolidadoDto> GetGuardicoreExportar();
        public abstract List<GuardicoreApiDTO> GetGuardicoreLabels();
        public abstract List<GuardicoreApiDTO> GetGuardicoreLabelsAmbiente();
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreComboEstado();
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1(string estado, string apps, string gest);
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2(string estado, string apps, string gest);
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1Nivel2(string estado, string apps, string gest);
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2Nivel2(string apps);
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab1Exportar(string estado, string apps, string gest);
        public abstract List<GuardicoreConsolidado2DTO> GetGuardicoreConsolidado2tab2Exportar(string estado, string apps, string gest);
        public abstract List<GuardicoreFase2DTO> GetGuardicoreFase2Tab2Listado();
        public abstract List<GuardicoreEtiquetado> GetEtiquetado(string etiqueta, string clave);
        public abstract GuardicoreEtiquetado GetEtiquetadoId(int id);
        public abstract int ActualizarRegistro(int id, string clave, string etiqueta, string comentario, string matricula);
        public abstract void EliminarRegistro(int id);
        public abstract int SetEtiquetado(string clave, string etiquetado, string comentario, string matricula);
        public abstract List<GuardicoreServidorRelacionDTO> GetServidorRelacion(string etiqueta,string comodin,int prioridad, int tipo);
        public abstract GuardicoreServidorRelacionDTO GetServidorRelacionId(int id);
        public abstract int ActualizarServidorRelacion(int id, string etiqueta, string comodin, int prioridad, string comentario, int tipo, string matricula);
        public abstract void EliminarServidorRelacion(int id);
        public abstract int SetServidorRelacion(string etiqueta, string comodin, int prioridad, string comentario, int tipo, string matricula);
        public abstract List<CustomAutocomplete> GetAplicacionMatricula(string matricula, string filtro);
    }
}
