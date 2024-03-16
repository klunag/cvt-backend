using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class TecnologiaNoRegistradaQualysDto
    {
        public int TecnologiaId { get; set; }
        public string DominioStr { get; set; }
        public string SubDominioStr { get; set; }
        public string TecnologiaStr { get; set; }
        public int Origen { get; set; }
        public string OrigenStr { get { return Origen == 0 ? null : Utilitarios.GetEnumDescription2((OrigenTecnologiaInstaladaNoRegistrada)Origen); } }
        public int Motivo { get; set; }
        public string MotivoStr { get { return Origen == 0 ? null : Utilitarios.GetEnumDescription2((MotivoTecnologiaInstaladaNoRegistrada)Motivo); } }
        public DateTime FechaEscaneo { get; set; }
        public string FechaEscaneoToString { get { return FechaEscaneo.ToString("dd/MM/yyyy hh:mm:ss tt"); } }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroToString { get { return FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss tt"); } }
        public bool? FlagAprobado { get; set; }
        public string FlagAprobadoStr { get { return !FlagAprobado.HasValue ? "Pendiente" : FlagAprobado.Value ? "Aprobado" : "Rechazado"; } }
        public bool? FlagAprobadoEquipo { get; set; }
        public string FlagAprobadoEquipoStr { get { return !FlagAprobadoEquipo.HasValue ? null : FlagAprobadoEquipo.Value ? "Atendido" : "Pendiente"; } }
        public int Id { get; set; }
        public int? QualyId { get; set; }
        public string QualyIds { get; set; }
        public int? ProductoId { get; set; }
        public string ProductoStr { get; set; }
        public string TecnologiaIds { get; set; }
        public string TecnologiaStrs { get; set; }
        public int EquipoId { get; set; }
        public string EquipoStr { get; set; }
        public string NetBIOS { get; set; }
        public string Title { get; set; }
        public string Vuln_Status { get; set; }
        public int VulnStatusId { get; set; }
        public string VulnStatusIdStr { get { return VulnStatusId == 0 ? null : Utilitarios.GetEnumDescription2((EstadoVulnerabilidades)VulnStatusId); } }
        public string creadoPor { get; set; }
        public int CantidadTecnologias { get; set; }
        public string Solucion { get; set; }
        public int NivelSeveridad { get; set; }
        public string Categoria { get; set; }
    }
}