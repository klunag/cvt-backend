using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Custom
{
    public class ApplicationDetail
    {
        public int id { get; set; }
        public int? managed { get; set; }
        public string applicationName { get; set; }

        public List<int?> TipoPCI { get; set; }
        public int? implementationType { get; set; }
        public int? deploymentType { get; set; }
        public int? deploymentTypeOriginal { get; set; }
        public string parentAPTCode { get; set; }
        public string parentAPTCodeName { get; set; }

        public string registerBy { get; set; }
        public string parentAPT
        {
            get
            {
                return string.Format("{0} - {1}", parentAPTCode, parentAPTCodeName);
            }
        }
        public int? mainOffice { get; set; }
        public bool? hasInterfaceId { get; set; }
        public string interfaceId { get; set; }
        public int? unit { get; set; }
        public string unitDetail { get; set; }
        public string teamName { get; set; }
        public string userEntity { get; set; }
        public string developmentType { get; set; }
        public string developmentProvider { get; set; }
        public int? infrastructure { get; set; }
        public string replacementApplication { get; set; }
        public string replacementApplicationName { get; set; }
        public string replacementAPT
        {
            get
            {
                return string.Format("{0} - {1}", replacementApplication, replacementApplicationName);
            }
        }

        public string applicationId { get; set; }
        public DateTime? registerDate { get; set; }
        public DateTime? dateFirstRelease { get; set; }
        public string description { get; set; }
        public int? status { get; set; }
        public int? authenticationMethod { get; set; }
        public int? authorizationMethod { get; set; }
        public int? architecId { get; set; }
        public int? architectSolutionId { get; set; }
        public bool? isActiveArchitec { get; set; }
        public int? teamId { get; set; }

        public int? BIANdomain { get; set; }
        public int? BIANarea { get; set; }
        public int? tobe { get; set; }
        public int? assetType { get; set; }
        public string tierPreProduction { get; set; }
        public string tierProduction { get; set; }

        public int? technologyCategory { get; set; }
        public int? technicalClassification { get; set; }
        public int? technicalSubclassification { get; set; }
        public int? functionalLayer { get; set; }
        public string operatingHours { get; set; }
        public bool? FlagComponenteCross { get; set; }
        public string registroUmbral { get; set; }
        public string expertId { get; set; }
        public string expertName { get; set; }
        public string expertEmail { get; set; }

        public string authorizedName { get; set; }
        public string authorizedId { get; set; }
        public string authorizedEmail { get; set; }

        public string architectName { get; set; }

        public string deploymentTypeName { get; set; }

        public string implementationTypeName { get; set; }
        public string managedName { get; set; }

        public string userEntityName { get; set; }

        public string developmentTypeName { get; set; }

        public string infrastructureName { get; set; }

        public string authenticationMethodName { get; set; }
        public string authorizationMethodName { get; set; }

        public string statusName { get; set; }
        public string unitName { get; set; }

        public bool? isFormalApplication { get; set; }

        public DateTime? regularizationDate { get; set; }

        public string regularizationDateDetail
        {
            get
            {
                return regularizationDate.HasValue ? regularizationDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public int? groupTicketRemedy { get; set; }

        public string webDomain { get; set; }

        public int AreaId { get; set; }

        public int DivisionId { get; set; }

        public int GerenciaId { get; set; }

        public string gerenciaName { get; set; }

        public string unidadName { get; set; }

        public string areaName { get; set; }

        public string divisionName { get; set; }

        public string areaBIANName { get; set; }

        public string tipoActivoName { get; set; }

        public string dominioBIANName { get; set; }

        public string jefaturaATIName { get; set; }

        public string TOBEName { get; set; }

        public string categoriaTecnologicaName { get; set; }

        public string clasificacionTecnicaName { get; set; }

        public string subClasificacionTecnicaName { get; set; }
        public string functionalLayerName { get; set; }
        

        public string usuarioAutorizadorName { get; set; }

        public string TIERProduccionName { get; set; }

        public string TIERPreProduccionName { get; set; }

        public string brokerName { get; set; }

        public string tribeLeadName { get; set; }

        public string technicalTribeLeadName { get; set; }

        public string jefeEquipoName { get; set; }

        public string liderUsuarioName { get; set; }

        public string grupoTicketRemedyName { get; set; }

        public string urlCertificadosDigitalesName { get; set; }

        public string criticidadBIANName { get; set; }

        public string clasificacionActivoName { get; set; }

        public string nuevaCriticidadName { get; set; }

        public string ProductoServicioRepresentativoName { get; set; }

        public string MenorRTOName { get; set; }

        public string MayorGradoInterrupcionName { get; set; }

        public string fechaPaseProduccionName {
            get
            {
                return dateFirstRelease.HasValue ? dateFirstRelease.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public int? applicationCriticalityBIA { get; set; }

        public int? classification { get; set; }

        public int? finalCriticality { get; set; }

        public string starProduct { get; set; }

        public int? technicalSubClassification { get; set; }

        public int AreaBianId { get; set; }

        public bool hasArchitectEvalApproved { get; set; }

        public bool hasDevSecOpsApproved { get; set; }

        public bool hasGobUserITApproved { get; set; }

        public bool hasOwnerApproved { get; set; }
        public bool hasArchitectSolutionApproved { get; set; }

        public bool hasDateReleased {
            get
            {
                return dateFirstRelease.HasValue;
            }
        }

        public bool hasWeb { get; set; }

        public bool? isApproved { get; set; }

        public string approvedBy { get; set; }

        public DateTime? dateApproved { get; set; }
        public string fechaAprobaciónRegistroCompleto
        {
            get
            {
                return dateApproved.HasValue ? dateApproved.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public bool isImported { get; set; }
        public bool isActive { get; set; }

        public string desactivatedBy { get; set; }

        public DateTime? dateDesactivated { get; set; }

        public bool isObserved { get; set; }

        public string commentsObserved { get; set; }

        public string summaryStandard { get; set; }

        public decimal? complianceLevel { get; set; }

        public string complianceLevelName { get; set; }



        //Campos Update Masivo
        public int? FilaExcel { get; set; }
        public bool? FlagRegistroValido { get; set; }

        public bool? FlagRegistroDistinto { get; set; }
        public bool ValidarExcel { get; set; }

        public int shorterApplicationResponseTime { get; set; }

        public int highestDegreeInterruption { get; set; }

        public int registrationSituation { get; set; }

        public string NombreArchivoDesestimacion { get; set; }
        public string NombreArchivoSeguridad { get; set; }

        public string Experto { get; set; }
        public string ArquitectoNegocio { get; set; }

        public string UsuarioAutorizador { get; set; }

        public string GerenciaCentral { get; set; }

        public string DivisionDetail { get; set; }

        public string AreaDetail { get; set; }

        public string BrokerSistemas { get; set; }

        public string TribeLead { get; set; }


        public string approvedByEmail { get; set; }
        public string TribeTechnicalLead { get; set; }

        public string JefeEquipo { get; set; }

        public string LiderUsuarioDetail { get; set; }

        public string SituacionRegistro { get; set; }

        public string FlagPirata { get; set; }

        public string FechaFlagPirata { get; set; }

        public string SolicitanteDetail { get; set; }

        public int IdAplicacionProduccion { get; set; }

        public int IdTIpoActivoIDTTactico { get; set; }

        public string TextoAplicacionProduccion { get; set; }

        public string ResponsableUnidad { get; set; }

        public string ResponsableUnidadMatricula { get; set; }

        public string ResponsableUnidadEmail { get; set; }

        public string ResponsableUnidadAntiguo { get; set; }

        public string ResponsableUnidadMatriculaAntiguo { get; set; }

        public string ResponsableUnidadEmailAntiguo { get; set; }

        public bool? isReactivated { get; set; }

        public string tipoPCI { get; set; }
    }
}
