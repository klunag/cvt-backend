using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross.Models
{
    public class AKSModel
    {
        public string id { get; set; }
        public string name { get; set; } //ResouceName
        public string type { get; set; } //Type
        public PropertiesAKSModel properties { get; set; }

        public string aksVersion => properties == null ? string.Empty : properties.kubernetesVersion;
    }

    public class PropertiesAKSModel
    {
        public string provisioningState { get; set; }
        public string kubernetesVersion { get; set; }
    }
}
