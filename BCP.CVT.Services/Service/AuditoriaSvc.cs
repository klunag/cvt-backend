using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Auditoria;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class AuditoriaSvc : AuditoriaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<AuditoriaDataDTO> GetHistoricoModificacion(string Accion, string Entidad, DateTime? FechaActualizacion, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AuditoriaDataDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_HistoricoModificaciones_Listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AuditoriaDataDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Entidad = reader.IsDBNull(reader.GetOrdinal("Entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Entidad")),
                                EntidadId = reader.IsDBNull(reader.GetOrdinal("EntidadId")) ? string.Empty : reader.GetString(reader.GetOrdinal("EntidadId")),
                                EntidadClave = reader.IsDBNull(reader.GetOrdinal("EntidadClave")) ? string.Empty : reader.GetString(reader.GetOrdinal("EntidadClave")),
                                CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                FechaOrden = reader.IsDBNull(reader.GetOrdinal("FechaOrden")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaOrden"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].TotalFilas;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetHistoricoModificacion()"
                    , new object[] { null });
            }
        }

        public override List<VisitaSiteDTO> GetVisitaSite(string Matricula, string Nombre, DateTime? FechaDesde, DateTime? FechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.VisitaSite
                                     where u.Activo &&
                                     (u.Matricula.ToUpper().Contains(Matricula.ToUpper())
                                     || string.IsNullOrEmpty(Matricula))
                                     && (u.Nombre.ToUpper().Contains(Nombre.ToUpper())
                                     || string.IsNullOrEmpty(Nombre))
                                     && (FechaDesde == null
                                     || DbFunctions.TruncateTime(u.FechaIngreso) >= DbFunctions.TruncateTime(FechaDesde).Value)
                                     && (FechaHasta == null
                                     || DbFunctions.TruncateTime(u.FechaIngreso) <= DbFunctions.TruncateTime(FechaHasta).Value)
                                     select new VisitaSiteDTO()
                                     {
                                         VisitaSiteId = u.VisitaSiteId,
                                         Nombre = u.Nombre,
                                         Matricula = u.Matricula,
                                         Activo = u.Activo,
                                         FechaIngreso = u.FechaIngreso,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         UrlSite = u.UrlSite
                                     }).OrderBy(sortName + " " + sortOrder).ToList();
                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override List<DTO.Auditoria.AuditoriaDTO> GetAuditoriaLista(string matricula, string entidad, string campo, string accion, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<DTO.Auditoria.AuditoriaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_AuditoriaData_Listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Matricula", matricula));
                        comando.Parameters.Add(new SqlParameter("@Entidad", entidad));
                        comando.Parameters.Add(new SqlParameter("@Campo", campo));
                        comando.Parameters.Add(new SqlParameter("@Accion", accion));
                        comando.Parameters.Add(new SqlParameter("@FechaDesde", fechaDesde));
                        comando.Parameters.Add(new SqlParameter("@FechaHasta", fechaHasta));


                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DTO.Auditoria.AuditoriaDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Accion = reader.IsDBNull(reader.GetOrdinal("Accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Accion")),
                                Entidad = reader.IsDBNull(reader.GetOrdinal("Entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Entidad")),
                                IdAsociado = reader.IsDBNull(reader.GetOrdinal("IdAsociado")) ? string.Empty : reader.GetString(reader.GetOrdinal("IdAsociado")),
                                Campo = reader.IsDBNull(reader.GetOrdinal("Campo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Campo")),
                                ValorNuevo = reader.IsDBNull(reader.GetOrdinal("ValorNuevo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ValorNuevo")),
                                ValorAnterior = reader.IsDBNull(reader.GetOrdinal("ValorAnterior")) ? string.Empty : reader.GetString(reader.GetOrdinal("ValorAnterior")),

                                CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].TotalFilas;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAdmin
                    , "Error en el metodo: GetAuditoriaLista()"
                    , new object[] { null });
            }
        }

        public override bool RegistrarAuditoriaAPI(AuditoriaAPIDTO objRegistro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    var entidad = new AuditoriaAPI()
                    {
                        AuditoriaAPIId = objRegistro.AuditoriaAPIId,
                        APINombre = objRegistro.APINombre,
                        APIMetodo = objRegistro.APIMetodo,
                        APIUsuario = objRegistro.APIUsuario,
                        APIParametros = objRegistro.APIParametros,
                        FechaCreacion = DateTime.Now,
                        CreadoPor = objRegistro.CreadoPor,
                    };
                    ctx.AuditoriaAPI.Add(entidad);
                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return false;
            }
        }

        public override List<AuditoriaAPIDTO> GetAuditoriaAPILista(string APIUsuario, string APINombre, string APIMetodo,  DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AuditoriaAPIDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_AuditoriaAPI_Listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@APIUsuario", APIUsuario));
                        comando.Parameters.Add(new SqlParameter("@APINombre", APINombre));
                        comando.Parameters.Add(new SqlParameter("@APIMetodo", APIMetodo));
                        comando.Parameters.Add(new SqlParameter("@FechaDesde", fechaDesde));
                        comando.Parameters.Add(new SqlParameter("@FechaHasta", fechaHasta));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AuditoriaAPIDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                APINombre = reader.IsDBNull(reader.GetOrdinal("APINombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("APINombre")),
                                APIMetodo = reader.IsDBNull(reader.GetOrdinal("APIMetodo")) ? string.Empty : reader.GetString(reader.GetOrdinal("APIMetodo")),
                                APIUsuario = reader.IsDBNull(reader.GetOrdinal("APIUsuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("APIUsuario")),
                                APIParametros = reader.IsDBNull(reader.GetOrdinal("APIParametros")) ? string.Empty : reader.GetString(reader.GetOrdinal("APIParametros")),
                                AuditoriaAPIId = reader.IsDBNull(reader.GetOrdinal("AuditoriaAPIId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AuditoriaAPIId")),
                                
                                //CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                        if (lista.Count > 0)
                            totalRows = lista[0].TotalFilas;
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAdmin
                    , "Error en el metodo: GetAuditoriaAPILista()"
                    , new object[] { null });
            }
        }



    }
}
