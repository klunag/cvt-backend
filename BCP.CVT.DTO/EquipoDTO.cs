using BCP.CVT.Cross;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoDTO : BaseDTO
    {
        public EquipoDTO()
        {
            ClientSecrets = new List<EquipoClientSecretDTO>();
        }

        public int EquipoId { get; set; }
        public int ProcedenciaId { get; set; }
        public int TablaProcedenciaId { get; set; }
        public int? TipoEquipoId { get; set; }
        public int? AmbienteId { get; set; }
        public bool? FlagTemporal { get; set; }
        public bool? FlagServidorServicio { get; set; }
        public int? EquipoSolicitudId { get; set; }

        public EquipoTecnologiaDTO EquipoTecnologia { get; set; }
        public EquipoSoftwareBaseDTO EquipoSoftwareBase { get; set; }

        //Indicadores
        public int EstadoActual { get; set; }
        public int EstadoIndicador1 { get; set; }
        public int EstadoIndicador2 { get; set; }

        public bool? FlagCambioDominioRed { get; set; }

        public DateTime? FechaCambioEstado { get; set; }
        public string UsuarioCambioEstado { get; set; }

        public DateTime? FechaCambioDominioRed { get; set; }
        public string UsuarioCambioDominioRed { get; set; }

        public string FechaCambioEstadoStr => FechaCambioEstado.HasValue ? FechaCambioEstado.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaCambioDominioRedStr => FechaCambioDominioRed.HasValue ? FechaCambioDominioRed.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string Ubicacion { get; set; }
        public int DescubrimientoId { get; set; }
        public string Suscripcion { get; set; }
        public string GrupoRecursos { get; set; }

        public string SuscripcionStr => !string.IsNullOrEmpty(Suscripcion) ? Suscripcion : string.Empty;
        public string GrupoRecursosStr => !string.IsNullOrEmpty(GrupoRecursos) ? GrupoRecursos : string.Empty;

        public string Nombre { get; set; }
        public string TipoEquipo { get; set; }

        public string NombreEquipo { get; set; }
        public string Ticket { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime? FechaCargarArchivoBaja { get; set; }
        public string FechaCargarArchivoBajaStr => FechaCargarArchivoBaja.HasValue ? FechaCargarArchivoBaja.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaAprobadoStr => FechaAprobado.HasValue ? FechaAprobado.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaCreacionBajaStr => FechaCreacionBaja.HasValue ? FechaCreacionBaja.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaRechazadoBajaStr => FechaRechazado.HasValue ? FechaRechazado.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string Observacion { get; set; }
        public string EstadoServidorStr { get; set; }
        public string AplicacionesRelacionadasStr { get; set; }
        public string EstadoProcesoBajaStr { get; set; }
        public string AprobadoPor { get; set; }
        public DateTime? FechaAprobado { get; set; }
        public string RechazadoPor { get; set; }
        public DateTime? FechaRechazado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaCreacionBaja { get; set; }
        public string Descripcion { get; set; }
        //public string TemporalToString => FlagTemporal.HasValue ? (FlagTemporal.Value ? Utilitarios.GetEnumDescription2((EDescubrimiento)DescubrimientoId) : "Descubrimiento automático - Remedy") : "Descubrimiento automático - Remedy";
        public string TemporalToString
        {
            get
            {
                var strDescubrimiento = string.Empty;
                if (FlagTemporal.HasValue)
                {
                    if (FlagTemporal.Value)
                    {
                        strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Manual);
                    }
                    else
                    {
                        strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Automaticamente);
                    }
                }
                else
                {
                    strDescubrimiento = Utilitarios.GetEnumDescription2(EDescubrimiento.Automaticamente);
                }

                return strDescubrimiento;
            }
        }

        public string TemporalDescripcion
        {
            get
            {
                return FlagTemporal.HasValue ? (FlagTemporal.Value ? "Carga manual" : "Descubrimiento automático") : "Descubrimiento automático";
            }
        }

        public DateTime? FechaUltimoEscaneoCorrecto { get; set; }
        public DateTime? FechaUltimoEscaneoError { get; set; }

        public string FechaUltimoEscaneoCorrectoStr /*=> FechaUltimoEscaneoCorrecto.HasValue ? FechaUltimoEscaneoCorrecto.Value.ToString("dd/MM/yyyy") : "-";*/
        {
            get; set;
        }

        public string FechaUltimoEscaneoErrorStr /*=> FechaUltimoEscaneoError.HasValue ? FechaUltimoEscaneoError.Value.ToString("dd/MM/yyyy") : "-";*/
        {
            get; set;
        }

        public string Ambiente { get; set; }
        public string SistemaOperativo { get; set; }
        public int AplicacionesRelacionadas { get; set; }
        public int TecnologiasInstaladas { get; set; }

        public bool? FlagExcluirCalculo { get; set; }
        public string FlagExcluirCalculoStr => FlagExcluirCalculo.HasValue ? (FlagExcluirCalculo.Value ? "Si" : "No") : "-";

        public string IP { get; set; }
        public string Dominio { get; set; }
        public int CaracteristicaEquipo { get; set; }
        public string CaracteristicaEquipoToString
        {
            get
            {
                if (CaracteristicaEquipo == 0)
                    return string.Empty;
                else
                    return Utilitarios.GetEnumDescription2((ECaracteristicaEquipo)CaracteristicaEquipo);
            }
        }
        public AmbienteDTO AmbienteDTO { get; set; }
        public long RelacionId { get; set; }
        public bool FlagObsoleto { get; set; }
        public EquipoConfiguracionDTO EquipoConfiguracionDTO { get; set; }
        public DominioServidorDTO DominioServidorDTO { get; set; }

        public string Model { get; set; }
        public string Subsidiaria { get; set; }
        public int? DominioServidorId { get; set; }

        public int TipoExclusionId { get; set; }
        public int TecnologiaId { get; set; }
        public string MotivoExclusion { get; set; }
        public int EstadoRelacionId { get; set; }
        public string EstadoRelacionToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EEstadoRelacion)EstadoRelacionId);
            }
        }
        public string MemoriaRam { get; set; }
        public List<EquipoProcesadoresDto> Procesadores { get; set; }
        public List<EquipoEspacioDiscoDto> Discos { get; set; }


        public string ClaveTecnologia { get; set; }
        public int TotalFilas { get; set; }

        public StorageEquipoDto Storage { get; set; }
        public string Modelo { get; set; }
        public string CodigoEquipo { get; set; }
        public string NumeroSerie { get; set; }
        public int CantidadVulnerabilidades { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteToString
        {
            get
            {
                if (FechaFinSoporte.HasValue)
                    return FechaFinSoporte.Value.ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
        }

        public string Owner { get; set; }

        public string OwnerContacto { get; set; }

        public string AssetId { get; set; }
        public string TagNumber { get; set; }
        public string SystemRole { get; set; }
        public string PartNumber { get; set; }
        public string Urgency { get; set; }
        public string SystemEnvironment { get; set; }
        public string VersionNumber { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string EnvironmentSpecification { get; set; }
        public string ConfigurationOptions { get; set; }
        public string Expansion { get; set; }
        public string ExpansionInterface { get; set; }
        public string IsVirtual { get; set; }
        public string Domain { get; set; }
        public string CreateDate { get; set; }
        public string DisposalDate { get; set; }
        public string ContractId { get; set; }
        public string ContractDesc { get; set; }
        public string CustomerID { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractExpDate { get; set; }
        public string AssignedSupportCompany { get; set; }
        public string AssignedSupportOrganization { get; set; }
        public string NotificationGroup { get; set; }
        public string PrimaryCapability { get; set; }
        public int PerfilId { get; set; }

        public int ControlAprovisionamiento { get; set; }
        public string ControlAprovisionamientoDesc
        {
            get
            {
                if (ControlAprovisionamiento == 0)
                    return string.Empty;
                else
                    return Utilitarios.GetEnumDescription2((EControlAprovisionamientoEquipo)ControlAprovisionamiento);
            }
        }

        public string DominioServidor { get; set; }

        public List<EquipoClientSecretDTO> ClientSecrets { get; set; }
        public string EquipoFisico { get; set; }
    }
    #region EquipoConfiguración
    public class EquipoConfiguracionDTO
    {
        public string Componente { get; set; }
        public string DetalleComponente { get; set; }
    }
    #endregion

    #region Procesadores
    public class EquipoProcesadoresDto
    {
        public string Fabricante { get; set; }
        public string Descripcion { get; set; }
    }
    #endregion

    public class EquipoEspacioDiscoDto
    {
        public string DescripcionCorta { get; set; }
        public string Nombre { get; set; }
        public string FileSystemType { get; set; }
        public string FileSystemSizeGB { get; set; }
        public string AvailableSpaceGB { get; set; }
        public string FileSystemSize { get; set; }
        public string AvailableSpace { get; set; }
    }


    public class EquipoClientSecretDTO
    {
        public string NombreCloud { get; set; }
        public string Suscripcion { get; set; }
        public string TipoRecurso { get; set; }
        public string AKSsecret { get; set; }
        public string Recurso { get; set; }
        public string TipoClientSecret { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFinSoporte { get; set; }
        public string FechaCreacionStr { get { return FechaCreacion.ToString("dd/MM/yyyy"); } }
        public string FechaFinSoporteStr { get { return FechaFinSoporte.ToString("dd/MM/yyyy"); } }
        //public int TotalFilas { get; set; }
        public string IdentificadorClientSecret { get; set; }
        public string NamespaceClientSecret { get; set; }
        public string ResourceGroupClientSecret { get; set; }
        public string ThumbprintClientSecret { get; set; }
        public string IssuerClientSecret { get; set; }
    }

    public class EquipoServidorVirtualDTO : EquipoServidorVirtualMasDTO
    { 
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string SistemaOperativo { get; set; }
        public string Ambiente { get; set; }
        public string EquipoFisico { get; set; }
        public int CantidadRelaciones { get; set; }
        //public int TotalFilas { get; set; }
    }
    public class EquipoServidorVirtualMasDTO
    {
        public string CodigoApt { get; set; }
        public string Aplicacion { get; set; }
        public string Owner { get; set; }
        public string OwnerEmail { get; set; }
        public string Experto { get; set; }
        public string ExpertoEmail { get; set; }
        public int TotalFilas { get; set; }
    }


}
