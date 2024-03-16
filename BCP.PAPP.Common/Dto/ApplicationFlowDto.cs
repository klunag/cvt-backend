using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Dto
{
    public class ApplicationFlowDto
    {
        public int FlowAppId { get; set; }
        public int AppId { get; set; }
        public int? typeRegister { get; set; }
        public int? typeFlow { get; set; }
        public string ownerId { get; set; }
        public string ownerEmail { get; set; }
        public bool? isApproved { get; set; }
        public string comments { get; set; }
        public DateTime? dateCreation { get; set; }
        public string createdBy { get; set; }
        public DateTime? dateApproved { get; set; }
        public string approvedBy { get; set; }
        public DateTime? dateRejected { get; set; }
        public string rejectedBy { get; set; }
        public DateTime? dateTransfer { get; set; }
        public string transferedBy { get; set; }
        public bool? isCompleted { get; set; }
        public bool? isNotified { get; set; }
        public int? registrationSituation { get; set; }

        public List<ApplicationFlowDataDto> data { get; set; }

        public string applicationId { get; set; }

        public string UsuarioCreacion { get; set; }

        public string dateApprovedStr => dateApproved.HasValue ? dateApproved.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateRejectedStr => dateRejected.HasValue ? dateRejected.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateTransferStr => dateTransfer.HasValue ? dateTransfer.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string approvedByName { get; set; }

        public string transferedByName { get; set; }

        public string rejectedByName { get; set; }

        public bool? isActiveOwner { get; set; }
        
        public int? SolicitudAplicacionId { get; set; }

        public int TotalFlujosSolicitudes { get; set; }

        public bool? isObserved { get; set; }
    }
}
