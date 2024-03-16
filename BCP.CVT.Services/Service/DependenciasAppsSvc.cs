using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class DependenciasAppsSvc : DependenciasAppsDAO
    {
         

        public override List<DependenciasAppsDTO> ListaDependenciasApps(PaginacionDependenciasApps pag, out int totalRows)
        {
            try
            {
                var lista = new List<DependenciasAppsDTO>();
                totalRows = 0;

                string paraCodigoAPT = "";
                if (pag.codigoAPT != null)
                {
                    paraCodigoAPT = pag.codigoAPT.ToString();
                }

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_USP_DependenciaApp_Mantenimiento_Listado]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@pCodigoAPT", paraCodigoAPT);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasAppsDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalRows")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalRows")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                NombreAPT = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                FechaUltProceso = reader.IsDBNull(reader.GetOrdinal("FechaUltProceso")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaUltProceso")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo")),
                                FlagProceso = reader.IsDBNull(reader.GetOrdinal("FlagProceso")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagProceso")),
                                ConfiguracionId = reader.IsDBNull(reader.GetOrdinal("ConfiguracionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ConfiguracionId"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasAppsDTO> ListaDependenciasApps(PaginacionDependenciasApps pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasAppsDTO> ListaDependenciasApps(PaginacionDependenciasApps pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override string RegistrarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_USP_DependenciaApp_Mantenimiento_Agregar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@pCodigoAPT", codigoAPT);
                        comando.Parameters.AddWithValue("@pActivo", activo);
                        comando.Parameters.AddWithValue("@pFlagProceso", flagProceso);
                        resultado = comando.ExecuteScalar().ToString();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string RegistrarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string RegistrarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
        }

        public override string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_USP_DependenciaApp_Mantenimiento_Procesar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@pCodigoAPT", codigoAPT);
                        comando.Parameters.AddWithValue("@pActivo", activo);
                        comando.Parameters.AddWithValue("@pFlagProceso", flagProceso);
                        resultado = comando.ExecuteScalar().ToString();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarDependenciasApps(string codigoAPT, bool activo, bool flagProceso)"
                    , new object[] { null });
            }
        }

        public override List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows)
        {
            try
            {
                var lista = new List<EtiquetasDTO>();
                totalRows = 0;

                string paraCodigoAPT = "";
                if (pag.codigoAPT != null)
                {
                    paraCodigoAPT = pag.codigoAPT.ToString();
                }

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListEtiqueta]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@EtiquetaId", pag.EtiquetaId);
                        comando.Parameters.AddWithValue("@Descripcion", pag.Descripcion);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new EtiquetasDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFIlas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFIlas")),
                                EtiquetaId = reader.IsDBNull(reader.GetOrdinal("EtiquetaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo")),
                                FlagDefault = reader.IsDBNull(reader.GetOrdinal("FlagDefault")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagDefault")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override EtiquetasDTO ObtenerEtiquetas(PaginacionEtiquetas pag)
        {
            try
            {
                var objeto = new EtiquetasDTO();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListEtiqueta]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@EtiquetaId", pag.EtiquetaId);
                        comando.Parameters.AddWithValue("@Descripcion", pag.Descripcion);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            objeto.EtiquetaId = reader.IsDBNull(reader.GetOrdinal("EtiquetaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EtiquetaId"));
                            objeto.Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion"));
                            objeto.Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo"));
                            objeto.FlagDefault = reader.IsDBNull(reader.GetOrdinal("FlagDefault")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagDefault"));
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetasDTO> ListaEtiquetas(PaginacionEtiquetas pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override string ProcesarEtiquetas(int? etiquetaId, string descripcion, bool activo, bool flagDefault)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_MantEtiqueta]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@EtiquetaId", etiquetaId);
                        comando.Parameters.AddWithValue("@Descripcion", descripcion);
                        comando.Parameters.AddWithValue("@Activo", activo);
                        comando.Parameters.AddWithValue("@FlagDefault", flagDefault);
                        comando.Parameters.AddWithValue("@Usuario", "auto");
                        comando.ExecuteNonQuery();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarEtiquetas(int? etiquetaId, string descripcion, bool activo, bool flagDefault)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarEtiquetas(int? etiquetaId, string descripcion, bool activo, bool flagDefault)"
                    , new object[] { null });
            }
        }
        public override List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<TiposRelacionDTO>();
                totalRows = 0;

                string paraCodigoAPT = "";
                if (pag.codigoAPT != null)
                {
                    paraCodigoAPT = pag.codigoAPT.ToString();
                }

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoRelacion]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@TipoRelacionId", pag.TipoRelacionId);
                        comando.Parameters.AddWithValue("@Descripcion", pag.Descripcion);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new TiposRelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFIlas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFIlas")),
                                TipoRelacionId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo")),
                                PorDefecto = reader.IsDBNull(reader.GetOrdinal("PorDefecto")) ? false : reader.GetBoolean(reader.GetOrdinal("PorDefecto")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                ParaAplicaciones = reader.IsDBNull(reader.GetOrdinal("ParaAplicaciones")) ? false : reader.GetBoolean(reader.GetOrdinal("ParaAplicaciones")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override TiposRelacionDTO ObtenerTiposRelacion(PaginacionTiposRelacion pag)
        {
            try
            {
                var objeto = new TiposRelacionDTO();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoRelacion]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@TipoRelacionId", pag.TipoRelacionId);
                        comando.Parameters.AddWithValue("@Descripcion", pag.Descripcion);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            objeto.TipoRelacionId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId"));
                            objeto.Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion"));
                            objeto.Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo"));
                            objeto.PorDefecto = reader.IsDBNull(reader.GetOrdinal("PorDefecto")) ? false : reader.GetBoolean(reader.GetOrdinal("PorDefecto"));
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<TiposRelacionDTO> ListaTiposRelacion(PaginacionTiposRelacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override string ProcesarTipoRelacion(int? tipoRelacionId, string descripcion, bool activo, bool porDefecto)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_MantTipoRelacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@TipoRelacionId", tipoRelacionId);
                        comando.Parameters.AddWithValue("@Descripcion", descripcion);
                        comando.Parameters.AddWithValue("@Activo", activo);
                        comando.Parameters.AddWithValue("@PorDefecto", porDefecto);
                        comando.Parameters.AddWithValue("@Usuario", "auto");
                        comando.ExecuteNonQuery();

                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarTipoRelacion(int? tipoRelacionId, string descripcion, bool activo, bool porDefecto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ProcesarTipoRelacion(int? tipoRelacionId, string descripcion, bool activo, bool porDefecto)"
                    , new object[] { null });
            }
        }
    }
}
