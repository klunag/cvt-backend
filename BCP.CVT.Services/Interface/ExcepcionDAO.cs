using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ExcepcionDAO : ServiceProvider
    {
        public abstract int AddOrEdit(ExcepcionDTO objRegistro);
        public abstract List<ExcepcionDTO> GetList(string filtro, string username, int pageNumber, int pageSize, string sortName, string sortOrder, string codigoAPT, int? tecnologiaId, int? equipoId, int? tipoExcepcionId, out int totalRows);
        public abstract ExcepcionDTO GetById(int id);
        public abstract bool DeleteById(int id);
        public abstract List<ExcepcionDTO> GetExcepcionXTecnologiaId(int tipoExcepcionId, int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
    }
}
