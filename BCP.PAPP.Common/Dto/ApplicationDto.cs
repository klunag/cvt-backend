using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT;

namespace BCP.PAPP.Common.Dto
{
	public class ApplicationDto
	{
		public int AppId { get; set; }
		public string applicationId { get; set; }
		public string motivoActualizacion { get; set; }
		public string applicationName { get; set; }
		public string description { get; set; }
		public byte[] archivoMotivo { get; set; }
		public List<int?> TipoPCI { get; set; }
		public string archivoMotivoNombre { get; set; }
		public int? implementationType { get; set; }
		public int? managed { get; set; }
		public int? deploymentType { get; set; }
		public string parentAPTCode { get; set; }
		public int? status { get; set; }
		public string interfaceId { get; set; }
		public DateTime? registerDate { get; set; }
		
		public int? unit { get; set; }
		public string teamName { get; set; }
		public string userEntity { get; set; }
		public string developmentType { get; set; }
		public string developmentProvider { get; set; }
		public int? infrastructure { get; set; }
		public string replacementApplication { get; set; }
		public int? assetType { get; set; }
		public int? BIANdomain { get; set; }
		public int? BIANarea { get; set; }
		public int? mainOffice { get; set; }
		public int? technologyCategory { get; set; }
		public int? functionalLayer { get; set; }
		public string operatingHours { get; set; }
		public bool? FlagComponenteCross { get; set; }
		public int? technicalClassification { get; set; }
		public int? technicalSubclassification { get; set; }
		public int? area { get; set; }
		public int? groupTicketRemedy { get; set; }
		public string webDomain { get; set; }
		public DateTime? dateFirstRelease { get; set; }
		public string starProduct { get; set; }
		public string shorterApplicationResponseTime { get; set; }
		public string highestDegreeInterruption { get; set; }
		public int? applicationCriticalityBIA { get; set; }
		public int? classification { get; set; }
		public int? finalCriticality { get; set; }
		public int? tobe { get; set; }
		public string tierPreProduction { get; set; }
		public string tierProduction { get; set; }
		public int? registrationSituation { get; set; }
		public bool? isFormalApplication { get; set; }
		public DateTime? regularizationDate { get; set; }
		public int? authorizationMethod { get; set; }
		public int? authenticationMethod { get; set; }
		public DateTime? lastModifiedBy { get; set; }
		public string lastDateModified { get; set; }
		public bool? hasInterfaceId { get; set; }
		public int? teamId { get; set; }

		public int architectId { get; set; }
		public int architectTechnology { get; set; }
		
		public int architectSolutionId { get; set; }

		public string registerBy { get; set; }
		public string registerByEmail { get; set; }
		public string registerByName { get; set; }		

		public string expertId { get; set; }
		public string expertName { get; set; }
		public string expertEmail { get; set; }

		public string authorizedName { get; set; }
		public string authorizedId { get; set; }
		public string authorizedEmail { get; set; }
		//
		public int FlowAppId { get; set; }
		public bool isApproved { get; set; }
		public string comments { get; set; }
		public int actionManager { get; set; }

		public decimal? complianceLevel { get; set; }

		public string summaryStandard { get; set; }

		public List<OwnerDto> Owners { get; set; }

		public List<ExpertoDto> Expertos { get; set; }

		public string Matricula { get; set; }

		public string NombreUsuarioModificacion { get; set; }

		public string NombreUsuarioAprobacion { get; set; }

		public string unitName { get; set; }

		public string prevUnitName { get; set; }

		public int RegistroOModificacion { get; set; }

		public List<ApplicationManagerCatalogDto> NuevosRolesList { get; set; }

		public List<ApplicationManagerCatalogDto> NuevosExpertosList { get; set; }

		public List<ApplicationManagerCatalogDto> NuevosAutorizadoresList { get; set; }

		public string EmailSolicitante { get; set; }

		public bool? aplicacionRevertida { get; set; }

		public int tipoAIO { get; set; }

		public string nombreArchivo { get; set; }

		public bool Eliminacion { get; set; }

		public bool Desestimacion { get; set; }

		public string emailCustodio { get; set; }
	}
}
