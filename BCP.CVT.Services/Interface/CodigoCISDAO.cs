using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class CodigoCISDAO : ServiceProvider
    {
        public abstract List<CodigoCISDTO> GetCodigoCIS(string username, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int AddOrEditCodigoCIS(CodigoCISDTO objeto);
        public abstract CodigoCISDTO GetCodigoCISById(int id);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<CodigoCISDTO> GetCodigoCISByUsuario(string nombre, string usuario, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract bool ValidarCodigo(string codigo);
        public abstract List<CodigoCISDTO> GetServidoresCIS(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        //Metodos propios
        public abstract List<ServidorCISDTO> GetServidoresRelacionados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<ServidorCISDTO> GetServidoresNoRegistrados(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);


    }
}
