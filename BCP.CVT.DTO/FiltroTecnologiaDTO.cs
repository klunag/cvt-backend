using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class FiltroTecnologiaDTO
    {
        public string nombre { get; set;}
        public List<int> dominioIds {get; set;}
        public List<int> subdominioIds {get; set;}
        public int? productoId {get; set;}
        public string codigo {get; set;}
        public string tribuCoe {get; set;}
        public string squad {get; set;}
        public List<int> tipoTecIds {get; set;}
        public List<int> estObsIds {get; set;}
        public string sortName {get; set;}
        public string sortOrder {get; set;}
        public string aplica { get; set; }
        public string dueno { get; set; }
        public List<int> resolucionCambio { get; set; }
    }

    public class FiltroProductoDTO
    {
        public string nombre {get;set;}
        public int? dominioId {get;set;}
        public int? subdominioId {get;set;}
        public bool activo {get;set;}
        public string sortName {get;set;}
        public string sortOrder {get;set;}
    }

    public class FiltroTecnologiaEstandarDTO
    {
        public string nombre { get; set; }
        public string tipoTecnologiaIds {get;set;}
        public string estadoTecnologiaIds {get;set;}
        public bool getAll {get;set;}
        public string subdominioIds {get;set;}
        public string dominiosIds {get;set;}
        public string aplicaIds {get;set;}
        public string compatibilidadSOIds {get;set;}
        public string compatibilidadCloudIds {get;set;}
        public string filtro { get; set; }
    }

    public class FiltroTecnologiaObsolescenciaDTO
    {
        public string dominioIds {get; set;}
        public string subDominioIds {get; set;}
        public string productoStr {get; set;}
        public string tecnologiaStr {get; set;}
        public string ownerStr {get; set;}
        public string squadId {get; set;}
        public int nivel {get; set;}
        public string ownerParentIds {get; set;}
        public string tipoEquipoIds {get; set;}
        public string fechaFiltro { get; set; }
    }
}
