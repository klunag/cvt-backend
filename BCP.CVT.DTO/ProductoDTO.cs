using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class ProductoDTO : BaseDTO
    {
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ProductoStr { get; set; }
        public int DominioId { get; set; }

        public int RolId { get; set; }
        public string DominioStr { get; set; }
        public int SubDominioId { get; set; }
        public string SubDominioStr { get; set; }

        public int CantidadRoles { get; set; }
        public int TipoProductoId { get; set; }
        public string TipoProductoStr { get; set; }
        public int EstadoObsolescenciaId { get; set; }
        public string EstadoObsolescenciaStr
        {
            get
            {
                EEstadoObsolescenciaProducto? item = Utilitarios.EnumToList<EEstadoObsolescenciaProducto>().Count(x => (int)x == EstadoObsolescenciaId) > 0 ? (EEstadoObsolescenciaProducto)EstadoObsolescenciaId : (EEstadoObsolescenciaProducto?)null;
                return !item.HasValue ? null : Utilitarios.GetEnumDescription2(item.Value);
            }
        }
        public string TribuCoeId { get; set; }
        public string TribuCoeDisplayName { get; set; }
        public string SquadId { get; set; }
        public string SquadDisplayName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerMatricula { get; set; }
        public int? GrupoTicketRemedyId { get; set; }
        public string GrupoTicketRemedyStr { get; set; }
        public string GrupoTicketRemedyNombre { get; set; }
        public bool EsAplicacion { get; set; }
        public int? AplicacionId { get; set; }
        public string Codigo { get; set; }
        public int? TipoCicloVidaId { get; set; }
        public string TipoCicloVidaStr { get; set; }
        public int? EsquemaLicenciamientoSuscripcionId { get; set; }
        public string EsquemaLicenciamientoSuscripcionStr
        {
            get
            {
                if (!EsquemaLicenciamientoSuscripcionId.HasValue) return null;
                return Utilitarios.GetEnumDescription2((EEsquemaLicenciamientoSuscripcion)EsquemaLicenciamientoSuscripcionId.Value);
            }
        }

        public string EquipoAdmContacto { get; set; }
        public string EquipoAprovisionamiento { get; set; }

        public int CantidadTecnologiasRegistradas { get; set; }

        public int[] ListaArquetipoEliminados { get; set; }
        public List<ProductoArquetipoDTO> ListaArquetipo { get; set; }

        public AplicacionDTO Aplicacion { get; set; }
        public List<TecnologiaDTO> ListaTecnologia { get; set; }
        public int CantidadFunciones { get; set; }
        public string TribuCoe { get; set; }
        public string GrupoRed { get; set; }
        public string Chapter { get; set; }
        public string Funcion { get; set; }
        public string Rol { get; set; }
        public string DescripcionRol { get; set; }
        public int FuncionId { get; set; }
        public string EstadoFuncion { get; set; }
        public int? TipoCuenta { get; set; }
        public string TipoCuentaStr
        {
            get
            {
                var resultado = (TipoCuenta == null) ? string.Empty : Utilitarios.GetEnumDescription2((TipoCuenta)TipoCuenta);
                return resultado;
            }
        }

        public List<RolesProductoAmbienteDTO> Ambiente { get; set; }

        public int RolesProductoId { get; set; }

        #region Flujo de Aprobación de Tecnologías
        public string TribuResponsableMatricula { get; set; }
        public string TribuResponsableNombre { get; set; }
        public string LiderUsuarioAutorizador { get; set; }
        #endregion
        public List<ProductoManagerRolesDTO> ListExpertos { get; set; }
    }
}
