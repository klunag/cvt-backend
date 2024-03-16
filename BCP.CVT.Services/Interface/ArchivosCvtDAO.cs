using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ArchivosCvtDAO : ServiceProvider
    {
        public abstract int AddOrEdit(ArchivosCvtDTO objRegistro);
        public abstract int AddOrEdit2(ArchivosCvtDTO objRegistro);
        public abstract int AddOrEdit3(ArchivosCvtDTO objRegistro);

        public abstract int AddOrEditTempFile(ArchivosCvtDTO objRegistro);
        public abstract ArchivosCvtDTO GetById(int id);

        public abstract ApplicationFileDTO GetById2(int appId);

        public abstract ApplicationFileDTO GetById3(int appId);

        public abstract ApplicationFileDTO GetById4(int Id);

        public abstract ApplicationFileDTO GetByIdGST(int Id);

        public abstract ApplicationFileDTO GetByIdTicket(int Id);

        public abstract ApplicationFileDTO GetByIdRatificacion(int Id);
        public abstract ArchivosCvtDTO GetByEntidadIdByProcedenciaId(string entidadId, int procedenciaId);
        public abstract List<ArchivosCvtDTO> GetArchivos(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract void DeleteFile(int id, string usuario);
    }
}
