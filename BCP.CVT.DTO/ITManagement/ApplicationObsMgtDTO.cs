using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class ApplicationObsMgtDTO
    {
        public string applicationId { get; set; }
        public string startDate { get; set; }
        public string currentObsolescence { get; set; }
        public string firstPeriodObsolescence { get; set; }
        public string secondPeriodObsolescence { get; set; }

        public ApplicationObsMgtDTO()
        {

        }

        public ApplicationObsMgtDTO(string _appId,
            string _startDate,
            string _currentObs,
            string _firstObs,
            string _secondObs)
        {
            applicationId = _appId;
            startDate = _startDate;
            currentObsolescence = _currentObs;
            firstPeriodObsolescence = _firstObs;
            secondPeriodObsolescence = _secondObs;
        }
    }

    public class ApplicationStateDTO
    {
        public string CodigoAPT { get; set; }
        public string Estado { get; set; }
    }
}
