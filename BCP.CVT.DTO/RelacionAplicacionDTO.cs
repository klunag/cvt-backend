using BCP.CVT.Cross;

namespace BCP.CVT.DTO
{
    public class RelacionAplicacionDTO : BaseDTO
    {
        public int OrigenId { get; set; }
        public string CodigoAptOrigen { get; set; }
        public string NombreAptOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public string NombreAptDestino { get; set; }
        public int? TipoRelacionId { get; set; }
        public string DescripcionTipoRelacion { get; set; }
        public int? EstadoId { get; set; }
        public string DescripcionEstadoRelacion 
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)EstadoId);
            }
        }
        public string AplicacionRelacion { get; set; }
        public string Aprobar { get; set; }
        public int TotalFilas { get; set; }
    }
}
