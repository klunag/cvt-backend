using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace BCP.CVT.WebApi.Auth
{
    public class TokenGenerator
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GenerateTokenJwt(UsuarioDTO user)
        {
            try
            {
                // appsetting for Token JWT
                HelperLog.usuario = user.UsuarioBCP_Dto;
                var secretKey = Constantes.JwtSecretKey;
                var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                // create a claimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.UsuarioBCP_Dto.Matricula),
                    new Claim(ClaimTypes.Email, user.CorreoElectronico),
                    new Claim(ClaimTypes.Role, user.UsuarioBCP_Dto.Perfil),
                    new Claim("RoleId", user.UsuarioBCP_Dto.PerfilId.ToString()),
                    new Claim(ClaimTypes.Name, user.UsuarioBCP_Dto.Name),
                    new Claim("FlagAdminPortafolio", user.UsuarioBCP_Dto.FlagAdminPortafolio.ToString()),
                    new Claim("VerDetalle", user.UsuarioBCP_Dto.VerDetalle.ToString()),
                    new Claim("FlagAprobador", user.UsuarioBCP_Dto.FlagAprobador.ToString()),
                    new Claim("PerfilesPAP", user.UsuarioBCP_Dto.PerfilesPAP.ToString()),
                    new Claim("FlagPortafolio", user.FlagPortafolio.ToString()),
                    new Claim("FlagAdmin", user.FlagAdmin.ToString()),
                });

                // create token to the user
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                //log.DebugFormat("Generando token");
                HelperLog.info("Generando token");
                var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    subject: claimsIdentity,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);
                //log.DebugFormat("Token generado");
                HelperLog.info("Token generado");
                var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
                HelperLog.infoFormat("Token {0}", jwtTokenString);
                //log.DebugFormat("Token {0}", jwtTokenString);

                return jwtTokenString;
            }
            catch (Exception e)
            {
                HelperLog.Error(e.Message);
                return e.Message;
            }

        }

        public static RespuestaTokenDTO GenerateTokenJwtExterno(UserExternoDTO userExterno)
        {
            RespuestaTokenDTO rspt = new RespuestaTokenDTO();

            if (IsAuthorizedUser(userExterno.app, userExterno.key))
            {
                // appsetting for Token JWT
                var secretKey = Constantes.JwtSecretKey;
                var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                // create a claimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userExterno.app) });

                // create token to the user
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    subject: claimsIdentity,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);

                var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
                rspt.success = true;
                rspt.message = "Token generado correctamente.";
                rspt.token = jwtTokenString;
                return rspt;
            }
            else
            {
                rspt.success = true;
                rspt.message = "Error al validar los datos enviados.";
                return rspt;
            }

        }

        public static bool IsAuthorizedUser(string codAplicacion, string apikey)
        {
            var credenciales = ServiceManager<AuthDAO>.Provider.ObtenerApiKey(codAplicacion);
            if (credenciales != null)
                return apikey == credenciales.ApiKey;
            else
                return false;
        }

        public static UsuarioCurrent GetCurrentUser()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UsuarioCurrent
                {
                    CorreoElectronico = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Matricula = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Perfil = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                    PerfilId = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == "RoleId")?.Value),
                    Nombres = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    FlagAdminPortafolio = Convert.ToBoolean(userClaims.FirstOrDefault(x => x.Type == "FlagAdminPortafolio")?.Value),
                    VerDetalle = Convert.ToBoolean(userClaims.FirstOrDefault(x => x.Type == "VerDetalle")?.Value),
                    FlagAprobador = Convert.ToBoolean(userClaims.FirstOrDefault(x => x.Type == "FlagAprobador")?.Value),
                    PerfilesPAP = userClaims.FirstOrDefault(x => x.Type == "PerfilesPAP")?.Value,
                    FlagPortafolio = Convert.ToBoolean(userClaims.FirstOrDefault(x => x.Type == "FlagPortafolio")?.Value),
                    FlagAdmin = Convert.ToBoolean(userClaims.FirstOrDefault(x => x.Type == "FlagAdmin")?.Value),
                };
            }
            return null;
        }

    }
}