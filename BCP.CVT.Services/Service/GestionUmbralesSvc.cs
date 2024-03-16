using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Graficos;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.Services.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    class GestionUmbralesSvc : GestionUmbralesDAO
    {
        public override List<CustomAutocomplete> GetTiposByFiltro(string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Tipos_Combo_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<int>("tipoid").ToString(),
                                Descripcion = reader.GetData<string>("TipoDescripcion"),
                                value = reader.GetData<string>("TipoDescripcion"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTiposByFiltro()"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetTipoValorByFiltro(string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_TipoValor_Combo_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<int>("TipoValorId").ToString(),
                                Descripcion = reader.GetData<string>("TipoValorDescripcion"),
                                value = reader.GetData<string>("TipoValorDescripcion"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTipoValorByFiltro()"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetDriverByFiltro(string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Driver_Combo_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<int>("DriverId").ToString(),
                                Descripcion = reader.GetData<string>("DriverNombre"),
                                value = reader.GetData<string>("DriverNombre"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDriverByFiltro()"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetDriverUMByFiltro(string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_DriverUM_Combo_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<int>("UnidadMedidaId").ToString(),
                                Descripcion = reader.GetData<string>("UnidadMedidaNombre"),
                                value = reader.GetData<string>("UnidadMedidaNombre"),
                                TipoId = reader.GetData<int>("DriverId").ToString(),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetDriverUMByFiltro()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDTO> ObtenerUmbrales(int EquipoId, string codigoAPT, int umbralId, int FlagActivo, string Matricula, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_LISTAR_GUMBRALES_COMPONENTE_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        //comando.Parameters.Add(new SqlParameter("@pTecnologiaId", tenologiaId));
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", ((object)EquipoId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", ((object)codigoAPT) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pUmbralId", umbralId));
                        comando.Parameters.Add(new SqlParameter("@pFlagActivo", FlagActivo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDTO()
                            {
                                UmbralId = reader.GetData<int>("UmbralId"),
                                TipoUmbralId = reader.GetData<int>("TipoUmbralId"),
                                TipoDescripcion = reader.GetData<string>("TipoDescripcion"),
                                TipoValorId = reader.GetData<int>("TipoValorId"),
                                TipoValorDescripcion = reader.GetData<string>("TipoValorDescripcion"),
                                ValorUmbral = reader.GetData<string>("ValorUmbral"),
                                RefEvidencia = reader.GetData<string>("RefEvidencia"),
                                ArchivoEvidencia = reader.GetData<byte[]>("ArchivoEvidencia"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                                indicador = reader.GetData<int>("indicador"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                                UsuarioCreacion = reader.GetData<string>("Usuario"),
                                FechaCreacion = reader.GetData<DateTime>("FechaCreacion"),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                    {
                        totalRows = lista[0].TotalRows;

                        lista.ForEach(x =>
                        {
                            x.isOwnerRecord = x.UsuarioCreacion.Equals(Matricula) ? 1 : 0;
                            if (x.ValorUmbral != "")
                            {
                                long number = 0;
                                bool isNumber = long.TryParse(x.ValorUmbral, out number);
                                if (isNumber)
                                {
                                    number = number * 2;
                                    x.ValorAspiracional = number.ToString();
                                }
                            }
                        });
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDTO> ExportarObtenerUmbrales(int EquipoId, string CodigoAPT)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_EXPORTAR_GUMBRALES_COMPONENTE_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", ((object)EquipoId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", ((object)CodigoAPT) ?? DBNull.Value));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDTO()
                            {
                                UmbralId = reader.GetData<int>("UmbralId"),
                                UmbralClaveTecnologica = reader.GetData<string>("ClaveTecnologia"),
                                TipoDescripcion = reader.GetData<string>("TipoDescripcion"),
                                TipoValorDescripcion = reader.GetData<string>("TipoValorDescripcion"),
                                ValorUmbral = reader.GetData<string>("ValorUmbral"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                                indicador = reader.GetData<int>("indicador"),
                                UsuarioCreacion = reader.GetData<string>("Usuario"),
                                FechaCreacion = reader.GetData<DateTime>("FechaCreacion"),
                                FechaModificacion = reader.GetData<DateTime>("FechaModificacion"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ExportarObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDetalleDTO> ExportarObtenerUmbralesDetalle(int EquipoId, string CodigoAPT)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDetalleDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_EXPORTAR_GUMBRALES_DETALLE_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", EquipoId));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", ((object)CodigoAPT) ?? DBNull.Value));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDetalleDTO()
                            {
                                UmbralDetalleId = reader.GetData<int>("UmbralDetalleId"),
                                UmbralId = reader.GetData<int>("UmbralId"),
                                UmbralClaveTecnologica = reader.GetData<string>("ClaveTecnologia"),
                                UmbralTipoDescripcion = reader.GetData<string>("TipoDescripcion"),
                                UmbralTipoValorDescripcion = reader.GetData<string>("TipoValorDescripcion"),
                                UmbralValorUmbral = reader.GetData<string>("ValorUmbral"),
                                DriverNombre = reader.GetData<string>("DriverNombre"),
                                UnidadMedidaNombre = reader.GetData<string>("UnidadMedidaNombre"),
                                Valor = reader.GetData<string>("Valor"),
                                UsuarioCreacion = reader.GetData<string>("Usuario"),
                                FechaCreacion = reader.GetData<DateTime>("FechaCreacion"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }




                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDetalleDTO> ObtenerUmbralesDetalle(int umbralId, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDetalleDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_LISTAR_GUMBRALES_DETALLE]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pUmbralId", umbralId));


                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        // UmbralDetalleId DriverId    DriverNombre UnidadMedidaId  UnidadMedidaNombre flagactivo  Valor
                        ///   1            2         Disco          1              GB                  1         4500
                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDetalleDTO()
                            {
                                UmbralDetalleId = reader.GetData<int>("UmbralDetalleId"),
                                DriverId = reader.GetData<int>("DriverId"),
                                DriverNombre = reader.GetData<string>("DriverNombre"),
                                UnidadMedidaId = reader.GetData<int>("UnidadMedidaId"),
                                UnidadMedidaNombre = reader.GetData<string>("UnidadMedidaNombre"),
                                Valor = reader.GetData<string>("Valor"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }


                    if (lista.Count > 0)
                        totalRows = lista[0].TotalRows;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDTO> ReporteObtenerUmbrales(int tenologiaId, string matricula)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_EXPORTAR_GUMBRALES_COMPONENTE]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pTecnologiaId", tenologiaId));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDTO()
                            {
                                UmbralId = reader.GetData<int>("UmbralId"),
                                TipoUmbralId = reader.GetData<int>("TipoUmbralId"),
                                TipoDescripcion = reader.GetData<string>("TipoDescripcion"),
                                TipoValorId = reader.GetData<int>("TipoValorId"),
                                TipoValorDescripcion = reader.GetData<string>("TipoValorDescripcion"),
                                ValorUmbral = reader.GetData<string>("ValorUmbral"),
                                RefEvidencia = reader.GetData<string>("RefEvidencia"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                                indicador = reader.GetData<int>("indicador"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                                UsuarioCreacion = reader.GetData<string>("Usuario"),
                                FechaCreacion = reader.GetData<DateTime>("FechaCreacion"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }



                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override List<GestionUmbralesDetalleDTO> ReporteObtenerUmbralesDetalle(int tenologiaId, string matricula)
        {
            try
            {

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionUmbralesDetalleDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_LISTAR_GUMBRALES_DETALLE]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pUmbralId", tenologiaId));


                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        // UmbralDetalleId DriverId    DriverNombre UnidadMedidaId  UnidadMedidaNombre flagactivo  Valor
                        ///   1            2         Disco          1              GB                  1         4500
                        while (reader.Read())
                        {
                            var objeto = new GestionUmbralesDetalleDTO()
                            {
                                UmbralDetalleId = reader.GetData<int>("UmbralDetalleId"),
                                DriverId = reader.GetData<int>("DriverId"),
                                DriverNombre = reader.GetData<string>("DriverNombre"),
                                UnidadMedidaId = reader.GetData<int>("UnidadMedidaId"),
                                UnidadMedidaNombre = reader.GetData<string>("UnidadMedidaNombre"),
                                Valor = reader.GetData<string>("Valor"),
                                FlagActivo = reader.GetData<bool>("flagactivo"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }



                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ObtenerUmbrales()"
                    , new object[] { null });
            }
        }
        public override int addOrEditUmbral(GestionUmbralesDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Procesar_Umbral_Componente_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pUmbralId", objeto.UmbralId));
                        comando.Parameters.Add(new SqlParameter("@pTipoUmbralId", objeto.TipoUmbralId));
                        comando.Parameters.Add(new SqlParameter("@pTipoValorId", objeto.TipoValorId));
                        comando.Parameters.Add(new SqlParameter("@pTecnologiaId", ((object)objeto.UmbralTecnologiaId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", ((object)objeto.UmbralEquipoId) ?? DBNull.Value));
                        comando.Parameters.Add(new SqlParameter("@pValorUmbral", objeto.ValorUmbral));
                        comando.Parameters.Add(new SqlParameter("@pRefEvidencia", objeto.RefEvidencia));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", objeto.UmbralAppId.Equals("") ? DBNull.Value : (object)objeto.UmbralAppId));
                        comando.Parameters.Add(new SqlParameter("@pUsuario", objeto.UsuarioModificacion));
                        comando.Parameters.Add(new SqlParameter("@pDetalle", objeto.UmbralDetalle));

                        resultado = (int)comando.ExecuteScalar();
                    }
                    cnx.Close();
                }
                if (objeto.UmbralAppId != "")
                {
                    int AppId;
                    using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                    {
                        cnx.Open();
                        using (var comando = new SqlCommand("[cvt].[USP_ObtenerAppIDByApplicationId]", cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@applicationId", objeto.UmbralAppId));
                            AppId = (int)comando.ExecuteScalar();
                        }
                        cnx.Close();
                    }
                    ServiceManager<ApplicationDAO>.Provider.ValidateRegister(AppId);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error al registrar/editar addOrEditUmbral."
                    , new object[] { null });
            }
        }
        public override int addOrEditUmbralArchivo(GestionUmbralesDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Procesar_Umbral_Componente_Archivo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pUmbralId", objeto.UmbralId));
                        comando.Parameters.Add(new SqlParameter("@pArchivoEvidencia", objeto.ArchivoEvidencia));


                        resultado = comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error al registrar/editar addOrEditUmbralArchivo."
                    , new object[] { null });
            }
        }
        public override int editUmbralComponenteCross(GestionUmbralesDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Procesar_Umbral_Componente_Cross]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pTecnologiaId", objeto.UmbralTecnologiaId));
                        comando.Parameters.Add(new SqlParameter("@pMatricula", objeto.UsuarioModificacion));
                        comando.Parameters.Add(new SqlParameter("@pEstado", objeto.Activo));

                        resultado = comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error al registrar/editar editUmbralComponenteCross."
                    , new object[] { null });
            }
        }

        //Nuevo
        public override List<CustomAutocomplete> GetAppsApisByFiltro(string filtro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Buscar_App_Apis_Autocomplete_GU]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pfiltro", filtro));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.GetData<string>("Id").ToString(),
                                Descripcion = reader.GetData<string>("nombre"),
                                value = reader.GetData<int>("Tipo").ToString(),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO, "Error en el metodo: GetTiposByFiltro()", new object[] { null });
            }
        }


        public override List<AplicacionApisDTO> GetListAplicacionesApisCross(string codigoAppApi, string Matricula, int PerfilId, string RolPerfil, string tipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                var lista = new List<AplicacionApisDTO>();
                bool isQa = false;
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GetList_APP_APIS_GestionUmbrales]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoAppApi", codigoAppApi));
                        comando.Parameters.Add(new SqlParameter("@Matricula", Matricula));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionApisDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("id")) ? string.Empty : reader.GetString(reader.GetOrdinal("id")),
                                nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                                gestionadoPor = reader.IsDBNull(reader.GetOrdinal("gestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("gestionadoPor")),
                                squad = reader.IsDBNull(reader.GetOrdinal("squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("squad")),
                                ownerLiderUsuario = reader.IsDBNull(reader.GetOrdinal("ownerLiderUsuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("ownerLiderUsuario")),
                                expertoLidertecnico = reader.IsDBNull(reader.GetOrdinal("expertoLidertecnico")) ? string.Empty : reader.GetString(reader.GetOrdinal("expertoLidertecnico")),
                                aplicacionOwner = reader.IsDBNull(reader.GetOrdinal("aplicacionOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("aplicacionOwner")),
                                PuedeGestionar = reader.IsDBNull(reader.GetOrdinal("PuedeGestionar")) ? 0 : reader.GetInt32(reader.GetOrdinal("PuedeGestionar")),
                                FlagComponenteCross = reader.GetData<bool>("FlagComponenteCross"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();


                    if (lista.Count > 0)
                        totalRows = lista[0].TotalRows;


                    if (PerfilId == (int)EPerfilBCP.Administrador)
                    {
                        lista.ForEach(x =>
                        {
                            x.PuedeGestionar = 1;
                            x.gestorQa = 1;
                        });
                    }
                    else
                    {
                        var rolQA = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_UMBRALES_ROL_QA").Valor.Trim();
                        var listaQAs = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_UMBRALES_LISTA_QA").Valor.Trim();

                        if (RolPerfil.Contains(rolQA))
                            isQa = true;

                        if (listaQAs.Contains(Matricula))
                            isQa = true;
                    }

                    if (isQa)
                        lista.ForEach(x => { x.gestorQa = 1; });


                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListDependencias(PaginacionDependencias pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListDependencias(PaginacionDependencias pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override int editUmbralComponenteCross2(GestionComponenteCrossDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Procesar_Umbral_Componente_Cross_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pcodigoAppApi", objeto.codigoAppApi));
                        comando.Parameters.Add(new SqlParameter("@pMatricula", objeto.matricula));
                        comando.Parameters.Add(new SqlParameter("@pTipo", objeto.tipoCross));
                        comando.Parameters.Add(new SqlParameter("@pEstado", objeto.Activo));

                        resultado = comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO, "Error al registrar/editar editUmbralComponenteCross2.", new object[] { null });
            }
        }


        public override int addOrEditPeak(GestionPeakDTO objeto)
        {
            var resultado = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Procesar_Umbral_Peak]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pPeakId", objeto.PeakId));
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", objeto.EquipoId == 0 ? DBNull.Value : (object)objeto.EquipoId));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", objeto.CodigoAPT.Equals("") ? DBNull.Value : (object)objeto.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@pTipoValorId", objeto.TipoValorId));
                        comando.Parameters.Add(new SqlParameter("@pValor", objeto.ValorPeak));
                        comando.Parameters.Add(new SqlParameter("@pUsuario", objeto.UsuarioModificacion));
                        comando.Parameters.Add(new SqlParameter("@pDetalle", objeto.DetallePeak));
                        resultado = (int)comando.ExecuteScalar();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO, "Error al registrar/editar addOrEditUmbral.", new object[] { null });
            }
        }

        public override List<GestionPeakDTO> ObtenerPeak(string codigoAPT, int equipoId, bool activo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionPeakDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_LISTAR_GUMBRALES_PEAK]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@pEquipoID", equipoId));
                        comando.Parameters.Add(new SqlParameter("@pEstado", activo ? 1 : 0));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new GestionPeakDTO()
                            {
                                PeakId = reader.GetData<int>("UmbralPeakId"),
                                Fecha = reader.GetData<DateTime>("fecha").ToString(),
                                TipoValorDesc = reader.GetData<string>("descripcion"),
                                ValorPeak = reader.GetData<string>("valor"),
                                DetallePeak = reader.GetData<string>("detalle"),
                                TotalRows = reader.GetData<int>("TotalRows"),
                                FlagActivo = reader.GetData<bool>("FlagActivo"),
                                FlagActivoDesc = reader.GetData<string>("estado")
                                //UsuarioCreacion = reader.GetData<string>("Usuario"),
                                //FechaCreacion = reader.GetData<DateTime>("FechaCreacion"),
                                //FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                    {
                        totalRows = lista[0].TotalRows;
                        lista.ForEach(x =>
                        {
                            if (x.ValorPeak != "")
                            {

                                long number = 0;
                                bool isNumber = long.TryParse(x.ValorPeak, out number);
                                if (isNumber)
                                {
                                    number = number * 2;
                                    x.ValorAspiracional = number.ToString();
                                }


                            }
                        });

                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO, "Error en el metodo: GestionPeakDTO()", new object[] { null });
            }
        }


        public override List<GestionPeakDTO> ExportarObtenerUmbralesPeak(int EquipoId, string CodigoAPT)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<GestionPeakDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_EXPORTAR_GUMBRALES_PEAK]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pEquipoId", EquipoId));
                        comando.Parameters.Add(new SqlParameter("@pCodigoAPT", ((object)CodigoAPT) ?? DBNull.Value));

                        var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new GestionPeakDTO()
                            {
                                PeakId = reader.GetData<int>("UmbralPeakId"),
                                Fecha = reader.GetData<DateTime>("fecha").ToString(),
                                TipoValorDesc = reader.GetData<string>("descripcion"),
                                ValorPeak = reader.GetData<string>("valor"),
                                DetallePeak = reader.GetData<string>("detalle"),
                                FlagActivo = reader.GetData<bool>("FlagActivo"),
                                FlagActivoDesc = reader.GetData<string>("estado"),
                                NombreCross = reader.GetData<string>("Nombre")
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                    {
                        lista.ForEach(x =>
                        {
                            if (x.ValorPeak != "")
                            {
                                int valor = Convert.ToInt32(x.ValorPeak) * 2;
                                x.ValorAspiracional = valor.ToString();
                            }
                        });
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO, "Error en el metodo: GestionPeakDTO()", new object[] { null });
            }
        }
    }
}
