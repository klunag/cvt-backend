using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class UsuarioDAO : ServiceProvider
    {
        public abstract int AddOrEditUsuario(BaseUsuarioDTO objeto);

        public abstract long AddOrEditVisitaSite(VisitaSiteDTO objeto);

        public abstract AplicacionResponsableDto GetResponsable(string matricula, int perfil, string codigoApt);

        public abstract void ActivarResponsable(int responsableId, string correo);

        public abstract void AgregarResponsable(string matricula, string colaborador, string codigoApt, int perfil, string correo);

        public abstract void AddRegistroContexto(string matricula, string grupo, string correo);

        public abstract void AddRegistroDB(string matricula, string grupo, string correo);

        //public abstract int DevolverPerfil(string matricula);

        public abstract string DevolverMatriculaByCorreo(string matricula);

        public abstract List<AplicacionResponsableDto> GetResponsables();

        public abstract BaseUsuarioDTO GetUsuarioByMail(string correoElectronico);

        public abstract void UpdateBaseUsuarios(string correoElectronico, string matricula, string colaborador);
        
        public abstract int DevolverBandejaRegistro(string matricula);
    }
}
