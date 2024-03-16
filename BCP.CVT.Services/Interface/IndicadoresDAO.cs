using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class IndicadoresDAO : ServiceProvider
    {

        public abstract IndicadoresGerencialEquipoData GetReporteIndicadoresGerencialEquipos(FiltrosIndicadoresGerencialEquipo filtros,bool flagTotales);

        public abstract IndicadoresGerencialTecnologiaData GetReporteIndicadoresGerencialTecnologias(FiltrosIndicadoresGerencialTecnologia filtros, bool flagTotales);

        public abstract IndicadoresGerencialAplicacionData GetReporteIndicadoresGerencialAplicaciones(FiltrosIndicadoresGerencialAplicacion filtros, bool flagTotales);

    }
}
