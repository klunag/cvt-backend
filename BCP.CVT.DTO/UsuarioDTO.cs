using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class UsuarioDTO
    {
            public string UsuarioBCP { get; set; }
            public UsuarioBCP_DTO UsuarioBCP_Dto { get; set; }
            public List<MenuMantenimientoDTO> Paginas { get; set; }
            public string UserName { get; set; }
            public string Proyeccion1 { get; set; }
            public string Proyeccion2 { get; set; }
            public string FechaActualizacion { get; set; }
            public string SiteTitle { get; set; }
            public bool FlagPortafolio { get; set; }
            public bool FlagAdmin { get; set; }
            public string CorreoElectronico { get; set; }
            public string Token { get; set; }
    }
    public class UsuarioBCP_DTO
    {
        public string SamAccountName { get; set; }
        public string Name { get; set; }
        public string Matricula { get; set; }
        public string Perfil { get; set; }
        public string PerfilesPAP { get; set; }
        public int PerfilId { get; set; }
        public bool FlagAprobador { get; set; }
        public bool VerDetalle { get; set; }
        public int Bandeja { get; set; }
        public bool FlagAdminPortafolio
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Perfil))
                    return false;
                else
                {
                    return Perfil.Contains("E195_PortafolioAplicaciones");
                }
            }
        }
    }
    public class MenuMantenimientoDTO
    {
        public string GrupoMenu { get; set; }
        public string SubgrupoMenu { get; set; }
        public string Menu { get; set; }
        public string LinkMenu { get; set; }
        public int? OrdenMenu { get; set; }
        public string Icono { get; set; }
    }

    public class UserExternoDTO
    {
        public string app { get; set; }
        public string key { get; set; }
    }

    public class RespuestaTokenDTO
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }

    public class UsuarioCurrent
    {
        public string CorreoElectronico { get; set; }
        public string Matricula { get; set; }
        public int PerfilId { get; set; }
        public string Perfil { get; set; }
        public string Nombres { get; set; }
        public string PerfilesPAP { get; set; }
        public bool FlagAprobador { get; set; }
        public bool VerDetalle { get; set; }
        public bool FlagAdminPortafolio { get; set; }
        public bool FlagPortafolio { get; set; }
        public bool FlagAdmin { get; set; }
    }

    public class UsuarioDTO_Storage
    {
        public string UsuarioBCP { get; set; }
        public UsuarioBCP_DTO_Storage UsuarioBCP_Dto { get; set; }
        public List<MenuMantenimientoDTO> Paginas { get; set; }
        public string UserName { get; set; }
        public string Proyeccion1 { get; set; }
        public string Proyeccion2 { get; set; }
        public string FechaActualizacion { get; set; }
        public string SiteTitle { get; set; }
        public string CorreoElectronico { get; set; }
        public string Token { get; set; }
    }
    public class UsuarioBCP_DTO_Storage
    {
        public string SamAccountName { get; set; }
        public string Name { get; set; }
        public string Matricula { get; set; }
        public int Bandeja { get; set; }
    }
}
