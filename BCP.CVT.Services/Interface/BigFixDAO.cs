using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class BigFixDAO : ServiceProvider
    {
        public abstract int AddPeticion(BigFixDTO objeto);
        public abstract int UpdatePeticion(int id, bool estado, string idBigFix);
        public abstract List<BigFixDTO> GetLog(int page, int rows, out int nroTotal);
        public abstract BigFixDTO GetBigFixById(string id);

        public abstract string ConsultarEstadoBigFix(string id);
        public abstract string SolicitarArchivosBigFix(BigFixDTO objeto);
        public abstract List<BigFixDTO> ListarBixFixPorFlagProcesado(bool flagProcesado);

        public abstract bool ExisteID(string Id);

        //CMDB
        public abstract List<CmdbDiscoBigFix> GetDiscos(string servidor);
    }
}
