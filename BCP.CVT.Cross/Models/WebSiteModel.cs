using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross.Models
{
    public class PropertiesWS
    {
        public string[] hostNames { get; set; }
        public string hostNamesList => hostNames != null && hostNames.Count() > 0 ? string.Join("|", hostNames) : string.Empty;
    }

    public class WebSiteModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string kind { get; set; }
        public PropertiesWS properties { get; set; }

        public string hostnames => properties == null ? string.Empty : properties.hostNamesList;
    }
}
