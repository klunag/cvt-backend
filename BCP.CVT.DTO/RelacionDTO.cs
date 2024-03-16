using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RelacionDTO : BaseDTO
    {
        public long RelacionId { get; set; }
        public string CodigoAPT { get; set; }
        public int TipoId { get; set; }

        public int ApplicationId { get; set; }
        public int? AmbienteId { get; set; }
        public int? EquipoId { get; set; }
        public int EstadoId { get; set; }
        public string Observacion { get; set; }
        public int TecnologiaId { get; set; }
        public string Tecnologia { get; set; }

        public string ListaPCI { get; set; }

        public string AplicacionStr { get; set; }

        public string EquipoTecnologiaStr { get; set; }


        public List<int> TecnologiaIdsEliminar { get; set; }

        public List<RelacionDetalleDTO> ListRelacionDetalle { get; set; }

        public string TipoStr
        {
            get
            {
                String resultado = "";
                if (TipoEquipoId== TipoEquipoIdCD)
                {
                    resultado = "Certificado Digital (CA)";
                }
                else
                {
                    resultado = TipoId == 0 ? null : Utilitarios.GetEnumDescription2((ETipoRelacion)(TipoId));
                }
                return resultado;
            }
        }

        public string AmbienteStr { get; set; }

        public string EstadoStr
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)(this.EstadoId));
            }
        }

        public int RelevanciaId { get; set; }

        public string RelevanciaStr
        {
            get
            {
                return RelevanciaId == 0 ? "" : Utilitarios.GetEnumDescription2((ERelevancia)(RelevanciaId));
            }
        }

        public string EquipoStr { get; set; }
        public string Componente { get; set; }
        public string ComponenteBD { get; set; }
        public int TotalFilas { get; set; }
        public string Aprobar { get; set; }
        //public string CodigoAPTVinculo { get; set; }
        //public string DetalleVinculo { get; set; }

        public bool? FlagRelacionAplicacion { get; set; }
        public string Suscripcion { get; set; }
        public string Funcion { get; set; }
        public int TipoEquipoId { get; set; }
        public bool FlagRemoveEquipo { get; set; }
        public int TipoEquipoIdCD { get; set; }
        public string DominioStr { get; set; }
        public string SubDominioStr { get; set; }
        public string TipoTecnologiaStr { get; set; }
        public string RelevanciaStrBD { get; set; }
        public bool FlagActivo { get; set; }
        public string FlagActivoToString
        {
            get
            {
                return FlagActivo ? "Activo" : "Inactivo";
            }
        }

        public DateTime? FechaInicioCuarentena { get; set; }
        public string FechaInicioCuarentenaToString
        {
            get
            {
                return FechaInicioCuarentena.HasValue ? FechaInicioCuarentena.Value.ToString("dd/MM/yyyy mm:dd:ss tt") : string.Empty;
            }
        }
        public DateTime? FechaFinCuarentena { get; set; }
        public string FechaFinCuarentenaToString
        {
            get
            {
                return FechaFinCuarentena.HasValue ? FechaFinCuarentena.Value.ToString("dd/MM/yyyy mm:dd:ss tt") : string.Empty;
            }
        }

        public DateTime? FechaRegistroCuarentena { get; set; }
        public string FechaRegistroCuarentenaFormat => FechaRegistroCuarentena.HasValue? FechaRegistroCuarentena.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaFinCuarentenaFormat => FechaFinCuarentena.HasValue ? FechaFinCuarentena.Value.ToString("dd/MM/yyyy") : string.Empty;
        public int TipoEquipoIdAPIS { get; set; }

        // Relaciones App-App
        public string CodigoAptOrigen { get; set; }
        public string NombreAptOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public string NombreAptDestino { get; set; }
        public string DescripcionTipoRelacion { get; set; }
        public string AplicacionRelacion { get; set; }
        public string MotivoEliminacionRelacionApp { get; set; }

        // Relacion App-Componente
        public int? TipoRelacionComponente { get; set; }
        public string DescTipoRelacionComp { get; set; }

        public int? EstadoIdTecnologia { get; set; }
        public string EstadoIdTecnologiaStr => EstadoIdTecnologia.HasValue ? (EstadoIdTecnologia.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoIdTecnologia)) : "-";
        public List<string> InstanciasBD { get; set; }

        public string NombreRecursoAzure { get; set; }
        public string TipoRecursoAzure { get; set; }
        public string GrupoRecursoAzure { get; set; }
        public string OrigenDestino { get; set; }

    }

}
