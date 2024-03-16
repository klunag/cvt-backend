using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.ITManagement
{
    public class ItResourceMgtDTO
    {
        public string itResourceId { get; set; }
        public string type { get; set; }
        public string discovery { get; set; }
        public string status { get; set; }
        public string enviroment { get; set; }
        public string domain { get; set; }
        public string operatingSystem { get; set; }
        public string registerDate { get; set; }
        public string ipAddress { get; set; }
    }
}
