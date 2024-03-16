using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Custom
{
	public class ApplicationHistoricoDetail
	{
		public int AppId { get; set; }
        public string applicationId { get; set; }
		public string applicationName { get; set; }
		public string description { get; set; }
	 	public string interfaceId { get; set; }
		public string parentAPTCode{ get; set; }
	    public string gerencia { get; set; }
		public string division { get; set; }
		public string area { get; set; }
		public string unity { get; set; }
		public string managed{ get; set; }
		public string implementationType { get; set; }
		public string deploymentType { get; set; }
		public string status{ get; set; }
		public string teamName { get; set; }
		public string userEntity{ get; set; }
		public string developmentType { get; set; }
		public string developmentProvider { get; set; }
		public string replacementApplication { get; set; }
		public string infrastructure { get; set; }
		public string authenticationMethod { get; set; }
		public string authorizationMethod { get; set; }
		public string groupTicketRemedy{ get; set; }
		public string webDomain{ get; set; }
		public decimal? complianceLevel { get; set; }
		public string summaryStandard{ get; set; }
		public string assetType{ get; set; }
		public string BIANArea { get; set; }
		public string BIANdomain { get; set; }
		public string mainOffice{ get; set; }
		public string tobe{ get; set; }
		public string technologyCategory { get; set; }
		public string technicalSubclassification { get; set; }
		public string technicalClassification { get; set; }
		public string tierProduction { get; set; }
		public string tierPreProduction { get; set; }
		public string applicationCriticalityBIA { get; set; }
		public string classification { get; set; }
		public string finalCriticality { get; set; }
		public string starProduct { get; set; }
		public string shorterApplicationResponseTime { get; set; }
		public string highestDegreeInterruption{ get; set; }
		public DateTime? dateFirstRelease { get; set; }
public int? tribeTechnicalLeadId { get; set; }
	public string tribeTechnicalLeadNombre{ get; set; }
	public string tribeTechnicalLeadEmail{ get; set; }
		public int? jefeDeEquipoId { get; set; }
public string jefeDeEquipoNombre{ get; set; }
public string jefeDeEquipoEmail{ get; set; }
		public int? brokerId{ get; set; }
public string brokerNombre{ get; set; }
public string brokerEmail { get; set; }
public int? ownerId{ get; set; }
public string ownerNombre { get; set; }
public string ownerEmail { get; set; }
public int? gestorId{ get; set; }
public string gestorNombre{ get; set; }
public string gestorEmail{ get; set; }
		public int? expertoId{ get; set; }
public string expertoNombre{ get; set; }
public string expertoEmail{ get; set; }
		public int? tribeLeadId{ get; set; }
public string tribeLeadNombre{ get; set; }
public string tribeLeadEmail{ get; set; }
		public int? solicitanteId{ get; set; }
public string solicitanteNombre{ get; set; }
public string solicitanteEmail{ get; set; }
		public int? arquitectoEvaluadorId{ get; set; }
public string arquitectoEvaluadorNombre{ get; set; }
public string arquitectoEvaluadorEmail{ get; set; }



		//Campos Update Masivo
		public int? FilaExcel { get; set; }
		public bool? FlagRegistroValido { get; set; }
		public bool ValidarExcel { get; set; }



		//IDS
		public string gerenciaId { get; set; }
		public string divisionId { get; set; }
		public string areaId { get; set; }
		public string unityId { get; set; }
		public string managedId { get; set; }
		public string implementationTypeId { get; set; }
		public string deploymentTypeId { get; set; }
		public string statusId { get; set; }
		public string developmentTypeId { get; set; }
		public string infrastructureId { get; set; }
		public string authenticationMethodId { get; set; }
		public string authorizationMethodId { get; set; }
		public string groupTicketRemedyId { get; set; }

		public string assetTypeId { get; set; }
		public string BIANAreaId { get; set; }
		public string BIANdomainId { get; set; }
		public string mainOfficeId { get; set; }
		public string tobeId { get; set; }
		public string technologyCategoryId { get; set; }
		public string technicalSubclassificationId { get; set; }
		public string technicalClassificationId { get; set; }
		public string applicationCriticalityBIAId { get; set; }
		public string classificationId { get; set; }
		public string finalCriticalityId { get; set; }

		public string userEntityId { get; set; }


	}
}
