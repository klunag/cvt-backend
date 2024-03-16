using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Descubrimiento
{
    public class AddmSOServidorDto
    {
        public string Server { get; set; }
        public string Os { get; set; }
        public string ServicePack { get; set; }
        public string OsBuild { get; set; }
        public string OsEdition { get; set; }
        public string OsVersion { get; set; }
        public string OsUpdateLevel { get; set; }
        public string ExtendedEosDate { get; set; }
        public string EosDate { get; set; }
        public string RetirementDate { get; set; }
        public string LastUpdateSuccess { get; set; }
        public string LastUpdateFailure { get; set; }
        public string Equivalencia
        {
            get
            {
                var result = string.Empty;
                var bloque1 = string.Empty;
                var bloque2 = string.Empty;
                var bloque3 = string.Empty;
                var bloque4 = string.Empty;
                var bloque5 = string.Empty;
                var bloque6 = string.Empty;

                if (string.IsNullOrWhiteSpace(Os)){
                    bloque1 = string.Empty;
                }
                else{
                    bloque1 = string.Concat(Os, " - ") ;
                }

                if (string.IsNullOrWhiteSpace(ServicePack)){
                    bloque2 = string.Empty;
                }
                else{
                    bloque2 = string.Concat(ServicePack, " - ");
                }

                if (string.IsNullOrWhiteSpace(OsBuild)){
                    bloque3 = string.Empty;
                }
                else{
                    bloque3 = string.Concat(OsBuild, " - ");
                }

                if (string.IsNullOrWhiteSpace(OsEdition)){
                    bloque4 = string.Empty;
                }
                else{
                    bloque4 = string.Concat(OsEdition, " - ");
                }

                if (string.IsNullOrWhiteSpace(OsVersion)){
                    bloque5 = string.Empty;
                }
                else{
                    bloque5 = string.Concat(OsVersion, " - ");
                }

                if (string.IsNullOrWhiteSpace(OsUpdateLevel)){
                    bloque6 = string.Empty;
                }
                else{
                    bloque6 = string.Concat(OsUpdateLevel, " - ");
                }

                result = string.Concat(bloque1, bloque2, bloque3, bloque4, bloque5, bloque6);
                return result; 
            }
        }
    }
}
