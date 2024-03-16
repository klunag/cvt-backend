using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class GuardicoreApiDTO 
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nameLimpio { get; set; }
        public string nameSource { get; set; }
        public string nameDestination { get; set; }
        public int equipoID { get; set; }
        public GuestAgentDetails guest_agent_details { get; set; }
        public string parseLabel { get; set; }
        public string[] ip_addresses { get; set; }
        public string ipaddressesString { get; set; }
        public string status { get; set; }
        public List<Label> labels { get; set; }
        public string labelsString { get; set; }
        public string last_guest_agent_details_update { get; set; }
        public int estado { get; set; }
        public int destination_port { get; set; }
        public double slot_start_time { get; set; }
        public string source_ip { get; set; }
        public string destination_ip { get; set; }
        public string ip_protocol { get; set; }
        public string idLabel { get; set; }
        public double first_seen { get; set; }
        public double last_seen { get; set; } 

        public string parseDesPro { get; set; }
        public GuestSources source { get; set; }
        public GuestSources destination { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string nombreLabel {
            get
            {
                return key + ": " + value;
            }
        }
        public class GuestSources
        {
            public GuestVMs vm { get; set; }
            public class GuestVMs
            {
                public string id { get; set; }
                public string name { get; set; }
            }
        }
        public class GuestAgentDetails
        {
            public OSDetails os_details { get; set; }
            public string agent_version { get; set; }

            public class OSDetails
            {
                public string os_display_name { get; set; }
                public string os_version_name { get; set; }
                public string num_of_processors { get; set; }
                public string install_date { get; set; }

            }


        }
        public class Label
        {
            public string name { get; set; }
        }
        public class GuardicoreCsvLabel
        {
            public List<string> replicated_labels { get; set; }
        }
    }

    public class GuardicoreTipoDTO : BaseDTO
    {
        public int total_count { get; set; }
        public string value { get; set; }
        public string text { get; set; }
    }

    public class GuardicoreCsvSourceFullDataDto
    {
        public string name { get; set; }
    }

    public class GuardicoreParametrosApiDto
    {
        public string fromTime { get; set; } 
        public string toTime { get; set; }
        public string tipo { get; set; }
        public string accion { get; set; }
        public string appOrigen { get; set; }
        public string appDestino { get; set; }
        public string ambOrigen { get; set; }
        public string ambDestino { get; set; }
        public string puertos { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
    }
}