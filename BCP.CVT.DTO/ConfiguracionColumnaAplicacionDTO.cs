using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ConfiguracionColumnaAplicacionDTO : BaseDTO
    {
        public string NombreBD { get; set; }
        public string NombreExcel { get; set; }
        public int TablaProcedenciaId { get; set; }
        public bool? FlagEdicion { get; set; }
        public bool? FlagVerExportar { get; set; }
        public int OrdenColumna { get; set; }
        public string NombreBDEntidadRelacion { get; set; }
        public string Valor { get; set; }
        public bool? FlagCampoNuevo { get; set; }
        public bool? FlagModificable { get; set; }
        public bool? FlagObligatorio { get; set; }
        public bool? FlagMostrarCampo { get; set; }
        public InfoCampoPortafolioDTO InfoCampoPortafolio { get; set; }

        public string TablaProcedenciaIdStr => Utilitarios.GetEnumDescription2((ETablaProcedenciaAplicacion)(TablaProcedenciaId));
        public string FlagEdicionStr => FlagEdicion.HasValue ? FlagEdicion.Value ? "Si" : "No" : "-";
        public string FlagVerExportarStr => FlagVerExportar.HasValue ? FlagVerExportar.Value ? "Si" : "No" : "-";
        public string OrdenColumnaStr => OrdenColumna == -1 ? "Sin orden" : OrdenColumna.ToString();

        public int? ActivoAplica { get; set; }
        public int?ModoLlenado { get; set; }
        public int? NivelConfiabilidad { get; set; }
        public int? TipoRegistro { get; set; }
        public string RolRegistra { get; set; }
        public string RolAprueba { get; set; }
        public string DescripcionCampo { get; set; }
        public string RolResponsableActualizacion { get; set; }

        public string ActivoAplicaToString
        {
            get
            {
                ActivoAplica? str = (ActivoAplica?)ActivoAplica;
                return str.HasValue ? Utilitarios.GetEnumDescription2(str) : null;
            }
        }

        public string ModoLlenadoToString
        {
            get
            {
                ModoLlenado? str = (ModoLlenado?)ModoLlenado;
                return str.HasValue ? Utilitarios.GetEnumDescription2(str) : null;
            }
        }

        public string TipoRegistroToString
        {
            get
            {
                TippRegistroDato? str = (TippRegistroDato?)TipoRegistro;
                return str.HasValue ? Utilitarios.GetEnumDescription2(str) : null;
            }
        }

        public string NivelConfiabilidadToString
        {
            get
            {
                NivelConfiabilidad? str = (NivelConfiabilidad?)NivelConfiabilidad;
                return str.HasValue ? Utilitarios.GetEnumDescription2(str) : null;
            }
        }
    }

    public class ConfiguracionColumnaAplicacionOrdenDTO
    {
        public int Id { get; set; }
        public int OrdenColumna { get; set; }
    }
}
