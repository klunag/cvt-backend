using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class RoadMapDAO : ServiceProvider
    {
        public abstract List<RoadMapDTO> GetAllRoadMap();
        public abstract int AddOrEdit(RoadMapDTO objRegistro);
        public abstract RoadMapDTO GetRoadMapById(int Id);
        public abstract List<RoadMapDTO> GetRoadMap(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
