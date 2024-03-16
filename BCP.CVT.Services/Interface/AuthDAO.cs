using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class AuthDAO : ServiceProvider
    {
        public abstract AuthAplicacionDTO ObtenerApiKey(string codAplicacion);

        public abstract List<AuthAplicacionDTO> GetAuthAplicacion(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract void RegistrarAuthApiKey(AuthAplicacionDTO objRegistro);

        public abstract void ActualizarAuthApiKey(AuthAplicacionDTO objRegistro);

        public abstract bool ExisteAuthKeyByCodigoAPT(string codAplicacion);
    }
}
