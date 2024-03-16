using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Data;

namespace BCP.CVT.Services.Service
{
    public class InstanciasBDSvc : InstanciasBDDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<EquipoInstanciaBdDTO> GetInstanciasBd(PaginacionEquipoInstanciaBD pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoInstanciaBdDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_EquipoInstanciasBD_List]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Nombre", pag.Nombre == null ? "" : pag.Nombre));
                        comando.Parameters.Add(new SqlParameter("@EquipoId", pag.EquipoId < 0 ? 0 : pag.EquipoId));
                        comando.Parameters.Add(new SqlParameter("@CodigoApt", pag.CodigoApt == null ? "" : pag.CodigoApt));
                        comando.Parameters.Add(new SqlParameter("@Procedencia", pag.Procedencia < 0 ? 0 : pag.Procedencia));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoInstanciaBdDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                NombreEquipo = reader.IsDBNull(reader.GetOrdinal("NombreEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo")),
                                InstanciaBD = reader.IsDBNull(reader.GetOrdinal("InstanciaBD")) ? string.Empty : reader.GetString(reader.GetOrdinal("InstanciaBD")),
                                Inventario = reader.IsDBNull(reader.GetOrdinal("Inventario")) ? 0 : reader.GetInt32(reader.GetOrdinal("Inventario")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                NombreApt = reader.IsDBNull(reader.GetOrdinal("NombreApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreApt")),
                                Usado = reader.IsDBNull(reader.GetOrdinal("Usado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usado")),

                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    totalRows = lista.Count();
                    var resultado = lista.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<EquipoInstanciaBdDTO> GetInstanciasBd(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoInstanciaBdDTO> GetInstanciasBd(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override void AddEditInstanciaBd(int EquipoId, string instanciaBd)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_EquipoInstanciasBD_Insert]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@EquipoId", EquipoId);
                        comando.Parameters.AddWithValue("@InstanciaBD", instanciaBd);
                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: AddEditInstanciaBd(int EquipoId, int TecnologiaId, string instanciaBd)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: AddEditInstanciaBd(int EquipoId, int TecnologiaId, string instanciaBd)"
                    , new object[] { null });
            }
        }

        public override string ValidarCambiarEstado(int Id)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_EquipoInstanciasBD_ValidarCambioEstado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Id", Id);
                        comando.Parameters.Add(new SqlParameter { ParameterName = "@Resultado", Value = "", Direction = System.Data.ParameterDirection.InputOutput, SqlDbType = System.Data.SqlDbType.VarChar });
                        comando.ExecuteNonQuery();
                        resultado = comando.Parameters["@Resultado"].Value.ToString();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ValidarCambiarEstado(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ValidarCambiarEstado(int Id)"
                    , new object[] { null });
            }
        }
        public override void CambiarEstado(EquipoInstanciaBdDTO obj)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_EquipoInstanciasBD_CambiarEstado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Id", obj.Id);
                        comando.Parameters.AddWithValue("@UsuarioModificacion", obj.UsuarioModificacion);
                        comando.ExecuteNonQuery();

                    }
                    cnx.Close();
                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
        }
    }
}
