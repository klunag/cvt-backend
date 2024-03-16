using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class EmployeeSigaMgtDTO
    {
        public string samAccountName { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public bool status
        {
            get
            {
                return !dateUnsubscribe.HasValue;
            }
        }
        public string organization { get; set; }
        public string jobTitleCode { get; set; }
        public string jobTitle { get; set; }
        public string unitOrganizationCode { get; set; }
        public string unitOrganization { get; set; }
        public string serviceCode { get; set; }
        public string service { get; set; }
        public string areaCode { get; set; }
        public string area { get; set; }
        public string divisionCode { get; set; }
        public string division { get; set; }
        public string samAccountNameManager { get; set; }
        public string nameManager { get; set; }
        public DateTime dateRegistration { get; set; }
        public DateTime dateLastModification { get; set; }
        public DateTime? dateUnsubscribe { get; set; }
    }
}
