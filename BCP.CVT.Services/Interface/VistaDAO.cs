using BCP.CVT.DTO.Grilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class VistaDAO : ServiceProvider
    {
        public abstract List<TecnologiaG> GetVistaTecnologia(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
