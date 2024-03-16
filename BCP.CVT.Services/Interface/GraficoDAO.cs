using BCP.CVT.DTO.Graficos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class GraficoDAO : ServiceProvider
    {
        public abstract List<ReportePrincipalDTO> GetDataGraficoRenovacionTecnologica(int anio, int mes, int dia);
    }
}
