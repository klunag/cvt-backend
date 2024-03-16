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
    public class RelacionamientoFuncionSvc : RelacionamientoFuncionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        public override List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List(PaginacionRelacionamientoFuncion pag, out int totalFilas)
        {
            try
            {
                var lista = new List<RelacionamientoFuncionDTO>();
                totalFilas = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_RelacionamientoFuncion_List]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionReglasGeneralesId", pag.RelacionReglasGeneralesId);
                        //comando.Parameters.AddWithValue("@AplicaEn", null);
                        comando.Parameters.AddWithValue("@Origen", pag.Origen);
                        comando.Parameters.AddWithValue("@Destino", pag.Destino);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionamientoFuncionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                RelacionReglasGeneralesId = reader.IsDBNull(reader.GetOrdinal("RelacionReglasGeneralesId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionReglasGeneralesId")),
                                AplicaEn = reader.IsDBNull(reader.GetOrdinal("AplicaEn")) ? 0 : reader.GetInt32(reader.GetOrdinal("AplicaEn")),
                                AplicaEnStr = reader.IsDBNull(reader.GetOrdinal("AplicaEnStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicaEnStr")),
                                Origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Origen")),
                                Destino = reader.IsDBNull(reader.GetOrdinal("Destino")) ? string.Empty : reader.GetString(reader.GetOrdinal("Destino")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
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
                    , "Error en el metodo: List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List(PaginacionRelacionamientoFuncion pag, out int totalFilas)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List(PaginacionRelacionamientoFuncion pag, out int totalFilas)"
                    , new object[] { null });
            }
        }

        public override RelacionamientoFuncionDTO RelacionamientoFuncion_Id(int id)
        {
            try
            {
                var objeto = new RelacionamientoFuncionDTO();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_RelacionamientoFuncion_List]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionReglasGeneralesId", id);
                        comando.Parameters.AddWithValue("@Origen", "");
                        comando.Parameters.AddWithValue("@Destino", "");
                        comando.Parameters.AddWithValue("@pageNumber", 0);
                        comando.Parameters.AddWithValue("@pageSize", 0);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto.RelacionReglasGeneralesId = reader.IsDBNull(reader.GetOrdinal("RelacionReglasGeneralesId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionReglasGeneralesId"));
                            objeto.AplicaEn = reader.IsDBNull(reader.GetOrdinal("AplicaEn")) ? 0 : reader.GetInt32(reader.GetOrdinal("AplicaEn"));
                            objeto.AplicaEnStr = reader.IsDBNull(reader.GetOrdinal("AplicaEnStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicaEnStr"));
                            objeto.Origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Origen"));
                            objeto.Destino = reader.IsDBNull(reader.GetOrdinal("Destino")) ? string.Empty : reader.GetString(reader.GetOrdinal("Destino"));
                            objeto.FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo"));
                            objeto.CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor"));
                            objeto.FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion"));
                            objeto.ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor"));
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
                    , "Error en el metodo: RelacionamientoFuncionDTO RelacionamientoFuncion_Id(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: RelacionamientoFuncionDTO RelacionamientoFuncion_Id(int id)"
                    , new object[] { null });
            }
        }

        public override string RelacionamientoFuncion_Insert(RelacionamientoFuncionDTO relacionamientoFuncion)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_RelacionamientoFuncion_Insert]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@AplicaEn", relacionamientoFuncion.AplicaEn);
                        comando.Parameters.AddWithValue("@Origen", relacionamientoFuncion.Origen);
                        comando.Parameters.AddWithValue("@Destino", relacionamientoFuncion.Destino);
                        comando.Parameters.AddWithValue("@FlagActivo", relacionamientoFuncion.FlagActivo);
                        comando.Parameters.AddWithValue("@CreadoPor", relacionamientoFuncion.CreadoPor);
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
                    , "Error en el metodo: string RelacionamientoFuncion_Insert(RelacionamientoFuncionDTO relacionamientoFuncion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string RelacionamientoFuncion_Insert(RelacionamientoFuncionDTO relacionamientoFuncion)"
                    , new object[] { null });
            }
        }
        public override string RelacionamientoFuncion_Update(RelacionamientoFuncionDTO relacionamientoFuncion)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_RelacionamientoFuncion_Update]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionReglasGeneralesId", relacionamientoFuncion.RelacionReglasGeneralesId);
                        comando.Parameters.AddWithValue("@AplicaEn", relacionamientoFuncion.AplicaEn);
                        comando.Parameters.AddWithValue("@Origen", relacionamientoFuncion.Origen);
                        comando.Parameters.AddWithValue("@Destino", relacionamientoFuncion.Destino);
                        comando.Parameters.AddWithValue("@FlagActivo", relacionamientoFuncion.FlagActivo);
                        comando.Parameters.AddWithValue("@ModificadoPor", relacionamientoFuncion.ModificadoPor);
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
                    , "Error en el metodo: string RelacionamientoFuncion_Update(RelacionamientoFuncionDTO relacionamientoFuncion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string RelacionamientoFuncion_Update(RelacionamientoFuncionDTO relacionamientoFuncion)"
                    , new object[] { null });
            }
        }

        public override List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List_Combo()
        {
            try
            {
                var lista = new List<RelacionamientoFuncionDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Puerto_Combo]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionamientoFuncionDTO()
                            {
                                RelacionReglasGeneralesId = reader.IsDBNull(reader.GetOrdinal("RelacionReglasGeneralesId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionReglasGeneralesId")),
                                Origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Origen"))
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
                    , "Error en el metodo: List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List_Combo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionamientoFuncionDTO> RelacionamientoFuncion_List_Combo()"
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
