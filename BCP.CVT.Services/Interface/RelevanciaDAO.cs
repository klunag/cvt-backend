using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class RelevanciaDAO : ServiceProvider
    {
        public abstract List<RelevanciaDTO> GetAllRelevancia();
        public abstract int AddOrEditRelevancia(RelevanciaDTO objRegistro);
        public abstract RelevanciaDTO GetRelevanciaById(int id);
        public abstract bool EditRelevanciaFlagActivo(int id, string Usuario);
        public abstract List<RelevanciaDTO> GetRelevancia(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
