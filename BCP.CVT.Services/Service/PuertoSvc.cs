using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class PuertoSvc : PuertoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        public override List<PuertoDTO> Puerto_List(Paginacion pag, out int totalFilas)
        {
            try
            {
                var lista = new List<PuertoDTO>();
                totalFilas = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_List]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@PuertoId", pag.id);
                        comando.Parameters.AddWithValue("@Nombre", pag.nombre);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new PuertoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? reader.GetDateTime(reader.GetOrdinal("FechaCreacion")) : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    if (lista.Count > 0)
                        totalFilas = lista[0].TotalFilas;

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<PuertoDTO> Puerto_List(Paginacion pag, out int totalFilas)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<PuertoDTO> Puerto_List(Paginacion pag, out int totalFilas)"
                    , new object[] { null });
            }
        }

        public override PuertoDTO Puerto_Id(int id)
        {
            try
            {
                var objeto = new PuertoDTO();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_List]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@PuertoId", id);
                        comando.Parameters.AddWithValue("@Nombre", "");
                        comando.Parameters.AddWithValue("@pageNumber", 0);
                        comando.Parameters.AddWithValue("@pageSize", 0);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto.PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId"));
                            objeto.Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"));
                            objeto.FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo"));
                            objeto.UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion"));
                            objeto.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion"));
                            objeto.UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion"));
                            objeto.FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"));

                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return objeto;

                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: PuertoDTO Puerto_Id(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: PuertoDTO Puerto_Id(int id)"
                    , new object[] { null });
            }
        }

        public override string Puerto_Insert(PuertoDTO puerto)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_Insert]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Nombre", puerto.Nombre);
                        comando.Parameters.AddWithValue("@FlagActivo", puerto.FlagActivo);
                        comando.Parameters.AddWithValue("@UsuarioCreacion", "auto");
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
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Puerto_Insert(entidad)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Puerto_Insert(entidad)"
                    , new object[] { null });
            }
        }
        public override string Puerto_Update(PuertoDTO puerto)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_Update]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@PuertoId", puerto.PuertoId);
                        comando.Parameters.AddWithValue("@Nombre", puerto.Nombre);
                        comando.Parameters.AddWithValue("@FlagActivo", puerto.FlagActivo);
                        comando.Parameters.AddWithValue("@ModificadoPor", "auto");
                        comando.Parameters.Add(new SqlParameter { ParameterName = "@Resultado", Value ="", Direction = System.Data.ParameterDirection.InputOutput, SqlDbType = System.Data.SqlDbType.VarChar });

                        comando.ExecuteNonQuery();
                        resultado = comando.Parameters["@Resultado"].Value.ToString();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Puerto_Update(entidad)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string Puerto_Update(entidad)"
                    , new object[] { null });
            }
        }

        public override List<PuertoDTO> Puerto_List_Combo()
        {
            try
            {
                var lista = new List<PuertoDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_Combo]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new PuertoDTO()
                            {
                                PuertoId = reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetByFiltro(string filtro)
        {
            try
            {
                var lista = new List<CustomAutocomplete>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_Filtro]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Filtro", filtro);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = (reader.IsDBNull(reader.GetOrdinal("PuertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuertoId"))).ToString(),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                value = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<CustomAutocomplete> GetByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
    }
}
