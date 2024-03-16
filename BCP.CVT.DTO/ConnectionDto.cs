using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ConnectionDto : BaseDTO
    {
        public string source_node_type { get; set; }
        public string destination_node_type { get; set; }
        public string source_process { get; set; }
        public string destination_process { get; set; }
        public string destination_port { get; set; }

        public string id { get; set; }

        public string connection_type { get; set; }
        public string source_ip { get; set; }

        public int EstadoDestination { get; set; }
        public int EstadoSource { get; set; }

        public string destination_ip { get; set; }
        public string ip_protocol { get; set; }
        public Source source { get; set; }
        public string source_desc
        {

            get
            {
                return this.source != null && this.source.vm != null ? this.source.vm.name : "";
            }
        }
        public string validateSource { get; set; }

        public Source destination { get; set; }
        public string destination_desc
        {

            get
            {
                return this.destination != null && this.destination.vm != null ? this.destination.vm.name : "";
            }
        }
        public string validateDestination { get; set; }

        public class Source
        {
            public VM vm { get; set; }

            public class VM
            {
                public string name { get; set; }
            }

        }
        public string policy_ruleset { get; set; }
        public double slot_start_time { get; set; }
        public string labels { get; set; }
        public string labelsOrigen { get; set; }
        public string labelDestino { get; set; }
        public string hostname { get; set; }
    }
}
