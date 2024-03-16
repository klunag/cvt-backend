using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Custom
{
    public class ApplicationFlowList
    {
        public int Id { get; set; }


        
        public int AppId { get; set; }
        public int FlowAppId { get; set; }
        public int? typeRegister { get; set; }
        public int? typeFlow { get; set; }
        public bool? isApproved { get; set; }
        public bool? isApprovedApplication { get; set; }
        public string comments { get; set; }
        public DateTime? dateCreation { get; set; }
        public string dateCreationStr => dateCreation.HasValue ? dateCreation.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
        public DateTime? dateApproved { get; set; }
        public bool? isCompleted { get; set; }
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public int? status { get; set; }
        public string name { get; set; }
        public string nameDetail { get; set; }
        public string registeredBy { get; set; }
        public string managedBy { get; set; }

        public string estadoEliminacion { get; set; }

        public string estadoModificacion { get; set; }
        public bool? isActive { get; set; }
        public string assignedTo { get; set; }
        public string assignedToDetail { get; set; }
        public string mail { get; set; }

        public int? IdSolicitud { get; set; }

        public DateTime? dateRejected { get; set; }
        public DateTime? dateTransfer { get; set; }
        public string dateApprovedStr => dateApproved.HasValue ? dateApproved.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateRejectedStr => dateRejected.HasValue ? dateRejected.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateTransferStr => dateTransfer.HasValue ? dateTransfer.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;


        public string fechaAtencionStr { 
            get {
                if (dateApprovedStr != null) { return dateApprovedStr; }
                else if (dateRejectedStr != null) { return dateRejectedStr; }
                else if (dateTransferStr != null) { return dateTransferStr; }
                else return null;
            } 
        }

        public string statusDetail
        {
            get
            {
                return status.HasValue ? Utilitarios.GetEnumDescription2((ApplicationState)status.Value) : string.Empty;
            }
        }

        public string typeFlowDetail
        {
            get
            {
                return typeFlow.HasValue ? Utilitarios.GetEnumDescription2((Flow)typeFlow.Value) : string.Empty;
            }
        }

        public string typeRegisterDetail
        {
            get
            {
                return typeRegister.HasValue ? Utilitarios.GetEnumDescription2((ApplicationManagerRole)typeRegister.Value) : string.Empty;
            }
        }

        public string typeRegisterDetail2
        {
            get; set;
        }
        public bool? isObserved
        {
            get; set;
        }

        public string isApprovedDetail
        {
            get
            {
                if (isCompleted.HasValue)
                {
                    if (isCompleted.Value)
                    {
                        if (isObserved.HasValue)
                        {
                            if (isObserved.Value)
                            {
                                return "Observado";
                            }
                            return "Observado";
                        }
                        else if (isApproved.HasValue)
                        {
                            var esAppActiva = isActive.HasValue ? isActive.Value : false;
                        
                           if (typeFlow == (int)Flow.Registro || typeFlow == (int)Flow.Eliminacion || typeFlow == (int)Flow.Modificacion)
                            {
                                return isApproved.Value ? "Aprobado" : esAppActiva ? "Observado" : "Rechazado";
                            }
                            else
                            {
                                if (typeRegister == (int)ApplicationManagerRole.DevSecOps ||
                                    typeRegister == (int)ApplicationManagerRole.JefeDeEquipo ||
                                    typeRegister == (int)ApplicationManagerRole.TTL ||
                                    typeRegister == (int)ApplicationManagerRole.GobiernoUserIT ||
                                    typeRegister == (int)ApplicationManagerRole.AdministradorPortafolio ||
                                    typeRegister == (int)ApplicationManagerRole.Owner ||
                                    typeRegister == null)
                                    return isApproved.Value ? "Aprobado" : "Rechazado";
                                else
                                    return isApproved.Value ? "Asignado" : "Rechazado";
                            }
                        }
                       else if (!isActive.Value)
                        {
                            return "Desestimada";
                        }
                        else 
                            return "Transferido";
                    }
                    else
                        return "Pendiente de atención";
                }
                else
                    return "Pendiente de atención";
            }
        }
        public int indicadorexcel => isCompleted.HasValue ? isCompleted.Value ? 1 : 0 : 0;

        public string NombreUsuarioCreacion { get; set; }

        public int EstadoSolicitudId { get; set; }

        public string EstadoSolicitud
        {
            get
            {
                return EstadoSolicitudId != 0 ? Utilitarios.GetEnumDescription2((EstadoSolicitud)EstadoSolicitudId) : string.Empty;
            }
        }

        public string ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedByName { get; set; }
        public string TransferedBy { get; set; }
        public string TransferedByName { get; set; }

        public string ObservedBy { get; set; }
        public string ObservedByName { get; set; }
    }
}
