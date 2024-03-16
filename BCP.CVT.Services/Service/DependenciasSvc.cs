using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class DependenciasSvc : DependenciasDAO
    {

        public override List<DependenciasDTO> GetListDependencias(PaginacionDependencias pag, out int totalRows)
        {
            try
            {
                var lista = new List<DependenciasDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependencies_Vista]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codigoAPT", pag.codigoAPT);
                        //comando.Parameters.AddWithValue("@tipoConexion", pag.TipoConexionId);
                        comando.Parameters.AddWithValue("@tipoConsultaId", pag.TipoConsultaId);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", pag.TipoRelacionamientoId);
                        comando.Parameters.AddWithValue("@tipoEtiquetaId", pag.TipoEtiquetaId);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@sortOrder", pag.sortOrder);
                        comando.Parameters.AddWithValue("@sortName", pag.sortName);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasDTO()
                            {
                                //CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                //CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                AplicacionDuenaRelacion = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacion")),
                                AplicacionDuenaRelacionNombre = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacionNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacionNombre")),
                                AplicacionGeneraImpacto = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpacto")),
                                AplicacionGeneraImpactoNombre = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpactoNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpactoNombre")),
                                //TipoConexion = reader.IsDBNull(reader.GetOrdinal("Conexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Conexion")),
                                TipoRelacion = reader.IsDBNull(reader.GetOrdinal("TipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacion")),
                                EtiquetaAplicacion = reader.IsDBNull(reader.GetOrdinal("EtiquetaAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EtiquetaAplicacion"))
                            };
                            lista.Add(objeto);

                            totalRows = totalRows == 0 ? reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")) : totalRows;
                        }
                        reader.Close();
                    }

                    cnx.Close();

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

        public override List<DependenciasComponentesDTO> GetListDependenciasComponentes(PaginacionDependenciasComponentes pag, out int totalRows)
        {
            try
            {
                var lista = new List<DependenciasComponentesDTO>();
                var registros = new List<DependenciasComponentesDTO>();
                var resultado = new List<DependenciasComponentesDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependenciesComponentes]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codigoAPT", pag.codigoAPT);
                        comando.Parameters.AddWithValue("@equipoId", pag.equipoId);
                        comando.Parameters.AddWithValue("@tipo", pag.tipoIds);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", pag.TipoRelacionamientoId);
                        //comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        //comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        //comando.Parameters.AddWithValue("@sortOrder", pag.sortOrder);
                        //comando.Parameters.AddWithValue("@sortName", pag.sortName);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasComponentesDTO()
                            {
                                //TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                TipoRelacionamiento = reader.IsDBNull(reader.GetOrdinal("TipoRelacionamiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacionamiento")),
                                TipoComponente = reader.IsDBNull(reader.GetOrdinal("TipoComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoComponente")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Relevancia = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    if (lista.Count > 0)
                    {
                        totalRows = lista.Count();
                        registros = lista.OrderBy(pag.sortName + " " + pag.sortOrder).ToList();
                        resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();
                    }

                    return resultado;

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
        public override List<DependenciasDTO> GetListDependenciasDetalle(PaginacionDependencias pag, out int totalRows)
        {
            try
            {
                var lista = new List<DependenciasDTO>();
                var registros = new List<DependenciasDTO>();
                var resultado = new List<DependenciasDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependenciesDetalle_Vista]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@relacionId", pag.RelacionId);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", pag.TipoRelacionamientoId);
                        comando.Parameters.AddWithValue("@pageNumber", pag.pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@sortOrder", pag.sortOrder);
                        comando.Parameters.AddWithValue("@sortName", pag.sortName);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasDTO()
                            {
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                AplicacionOrigen = reader.IsDBNull(reader.GetOrdinal("AplicacionOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionOrigen")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("EquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoOrigen")),
                                IPEquipoOrigen = reader.IsDBNull(reader.GetOrdinal("IPEquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPEquipoOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                AplicacionDestino = reader.IsDBNull(reader.GetOrdinal("AplicacionDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDestino")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),
                                IPEquipoDestino = reader.IsDBNull(reader.GetOrdinal("IPEquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPEquipoDestino")),
                                //TipoConexion = reader.IsDBNull(reader.GetOrdinal("Conexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Conexion")),
                                //TipoRelacion = reader.IsDBNull(reader.GetOrdinal("TipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacion")),
                                Puerto = reader.IsDBNull(reader.GetOrdinal("EquipoPuerto")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoPuerto")),
                                Protocolo = reader.IsDBNull(reader.GetOrdinal("EquipoProtocolo")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoProtocolo")),
                                ProcesoOrigen = reader.IsDBNull(reader.GetOrdinal("ProcesoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoOrigen")),
                                ProcesoDestino = reader.IsDBNull(reader.GetOrdinal("ProcesoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoDestino")),
                                //AplicacionDuenaRelacion = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacion")),
                                //AplicacionGeneraImpacto = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpacto")),
                            };
                            lista.Add(objeto);

                            totalRows = totalRows == 0 ? reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")) : totalRows;
                        }
                        reader.Close();
                    }

                    cnx.Close();

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

        public override List<DependenciasComponentesDTO> GetListExportDependenciasComponentes(string codigoAPT, int equipoId, string tipoCompId, int tipoRelacionamientoId)
        {
            try
            {
                var lista = new List<DependenciasComponentesDTO>();
                var registros = new List<DependenciasComponentesDTO>();
                var resultado = new List<DependenciasComponentesDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependenciesComponentes]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codigoAPT", codigoAPT);
                        comando.Parameters.AddWithValue("@equipoId", equipoId);
                        comando.Parameters.AddWithValue("@tipo", tipoCompId);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", tipoRelacionamientoId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasComponentesDTO()
                            {
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                TipoRelacionamiento = reader.IsDBNull(reader.GetOrdinal("TipoRelacionamiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacionamiento")),
                                TipoComponente = reader.IsDBNull(reader.GetOrdinal("TipoComponente")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoComponente")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                Relevancia = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                TipoTecnologia = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListExportDependenciasComponentes(string codigoAPT, int equipoId, string tipoCompId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListExportDependenciasComponentes(string codigoAPT, int equipoId, string tipoCompId)"
                    , new object[] { null });
            }
        }

        public override List<DependenciasDTO> GetExportDependencias(string codigoAPT, string tipoConexionId, int tipoRelacionamientoId, int tipoEtiquetaId, int tipoConsultaId)
        {
            try
            {
                var lista = new List<DependenciasDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependencies]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codigoAPT", codigoAPT);
                        //comando.Parameters.AddWithValue("@tipoConexion", tipoConexionId);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", tipoRelacionamientoId);
                        comando.Parameters.AddWithValue("@tipoEtiquetaId", tipoEtiquetaId);
                        comando.Parameters.AddWithValue("@tipoConsultaId", tipoConsultaId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasDTO()
                            {
                                AplicacionDuenaRelacion = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacion")),
                                AplicacionDuenaRelacionNombre = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacionNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacionNombre")),
                                AplicacionGeneraImpacto = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpacto")),
                                AplicacionGeneraImpactoNombre = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpactoNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpactoNombre")),
                                //TipoConexion = reader.IsDBNull(reader.GetOrdinal("Conexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Conexion")),
                                TipoRelacion = reader.IsDBNull(reader.GetOrdinal("TipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacion")),
                                EtiquetaAplicacion = reader.IsDBNull(reader.GetOrdinal("EtiquetaAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EtiquetaAplicacion"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetExportDependenciasDetalle(PaginacionDependencias pag)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetExportDependenciasDetalle(PaginacionDependencias pag)"
                    , new object[] { null });
            }
        }

        public override List<DependenciasDTO> GetExportDependenciasDetalle(string codigoAPT, string tipoConexionId, int tipoRelacionamientoId, int tipoEtiquetaId, int tipoConsultaId)
        {
            try
            {
                var lista = new List<DependenciasDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependenciesDetalle]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codigoAPT", codigoAPT);
                        //comando.Parameters.AddWithValue("@tipoConexion", tipoConexionId);
                        comando.Parameters.AddWithValue("@tipoRelacionamientoId", tipoRelacionamientoId);
                        comando.Parameters.AddWithValue("@tipoConsultaId", tipoConsultaId);
                        comando.Parameters.AddWithValue("@tipoEtiquetaId", tipoEtiquetaId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasDTO()
                            {
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                AplicacionOrigen = reader.IsDBNull(reader.GetOrdinal("AplicacionOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionOrigen")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("EquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoOrigen")),
                                IPEquipoOrigen = reader.IsDBNull(reader.GetOrdinal("IPEquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPEquipoOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                AplicacionDestino = reader.IsDBNull(reader.GetOrdinal("AplicacionDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDestino")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoDestino")),
                                IPEquipoDestino = reader.IsDBNull(reader.GetOrdinal("IPEquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPEquipoDestino")),
                                //TipoConexion = reader.IsDBNull(reader.GetOrdinal("Conexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Conexion")),
                                TipoRelacion = reader.IsDBNull(reader.GetOrdinal("TipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacion")),
                                Puerto = reader.IsDBNull(reader.GetOrdinal("EquipoPuerto")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoPuerto")),
                                Protocolo = reader.IsDBNull(reader.GetOrdinal("EquipoProtocolo")) ? string.Empty : reader.GetString(reader.GetOrdinal("EquipoProtocolo")),
                                ProcesoOrigen = reader.IsDBNull(reader.GetOrdinal("ProcesoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoOrigen")),
                                ProcesoDestino = reader.IsDBNull(reader.GetOrdinal("ProcesoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoDestino")),
                                AplicacionDuenaRelacion = reader.IsDBNull(reader.GetOrdinal("AplicacionDuenaRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionDuenaRelacion")),
                                AplicacionGeneraImpacto = reader.IsDBNull(reader.GetOrdinal("AplicacionGeneraImpacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionGeneraImpacto")),
                                PrimeraFechaEscaneo = reader.IsDBNull(reader.GetOrdinal("PrimeraFechaEscaneo")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PrimeraFechaEscaneo")),
                                UltimaFechaEscaneo = reader.IsDBNull(reader.GetOrdinal("UltimaFechaEscaneo")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("UltimaFechaEscaneo")),
                                CantidadConexiones = reader.IsDBNull(reader.GetOrdinal("CantidadConexiones")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadConexiones")),
                                CantidadEscaneo = reader.IsDBNull(reader.GetOrdinal("CantidadEscaneo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadEscaneo"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetExportDependenciasDetalle(PaginacionDependencias pag)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetExportDependenciasDetalle(PaginacionDependencias pag)"
                    , new object[] { null });
            }
        }


        public override CombosDependenciasDTO GetCombosData()
        {
            try
            {
                var listaTipoRelacionamiento = new List<ItemLista>();
                var listaTipoConexion = new List<ItemLista2>();
                var listaEtiquetaAplicacion = new List<ItemLista>();

                var combosData = new CombosDependenciasDTO();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoRelacionamiento]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItemLista()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("TipoRelacionamientoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionamientoId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("NombreRelacionamiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreRelacionamiento"))
                            };
                            listaTipoRelacionamiento.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoConexion]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItemLista2()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("NombreConexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreConexion")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("NombreConexion")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreConexion"))
                            };
                            listaTipoConexion.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoEtiqueta]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItemLista()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("TipoEtiquetaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEtiquetaId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("NombreEtiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEtiqueta"))
                            };
                            listaEtiquetaAplicacion.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    combosData.ListaTipoConexion = listaTipoConexion;
                    combosData.ListaTipoRelacionamiento = listaTipoRelacionamiento;
                    combosData.ListaEtiquetaAplicacion = listaEtiquetaAplicacion;

                    return combosData;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: CombosDependenciasDTO GetCombosData()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: CombosDependenciasDTO GetCombosData()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipoByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidadComponente = (from u in ctx.Equipo
                                                 where u.FlagActivo
                                                 && (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                                 orderby u.Nombre
                                                 select new CustomAutocomplete()
                                                 {
                                                     Id = u.EquipoId.ToString(),
                                                     Descripcion = u.Nombre,
                                                     value = u.Nombre,
                                                     TipoId = "1"
                                                 }).ToList();

                        var entidadTecnologia = (from u in ctx.Tecnologia
                                                 where u.Activo
                                                 && (string.IsNullOrEmpty(filtro) || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper()))
                                                 orderby u.ClaveTecnologia
                                                 select new CustomAutocomplete()
                                                 {
                                                     Id = u.TecnologiaId.ToString(),
                                                     Descripcion = u.ClaveTecnologia,
                                                     value = u.ClaveTecnologia,
                                                     TipoId = "2"
                                                 }).ToList();

                        var lista = entidadComponente.Union(entidadTecnologia).ToList();

                        return lista;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<DependenciasDTO> GetListAccesoApps(string matricula)
        {
            try
            {
                var lista = new List<DependenciasDTO>();


                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetListDependencies_AccesoApps]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        comando.Parameters.AddWithValue("@matricula", matricula);


                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DependenciasDTO()
                            {
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListAccesoApps(string matricula)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DependenciasDTO> GetListAccesoApps(string matricula)"
                    , new object[] { null });
            }
        }

        #region Vista Grafica Dependencias
        public override List<DataComponente> GetDataComponente(int TipoComponente, int ComponenteId)
        {
            try
            {
                var lista = new List<DataComponente>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VGD_DataComponente]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@TipoComponente", TipoComponente);
                        comando.Parameters.AddWithValue("@ComponenteId", ComponenteId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DataComponente()
                            {
                                ComponenteId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                CodigoAptOwner = reader.IsDBNull(reader.GetOrdinal("CodigoAptOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOwner")),
                                NombreAptOwner = reader.IsDBNull(reader.GetOrdinal("NombreAptOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOwner")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("SistOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistOperativo")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DataEquipo> GetDataEquipo(int EquipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<DataEquipo> GetDataEquipo(int EquipoId)"
                    , new object[] { null });
            }
        }

        public override List<ApsRelacionComponente> GetAppsRelacionComponente(int TipoComponente, int EquipoId, int TipoRelacionId)
        {
            try
            {
                var lista = new List<ApsRelacionComponente>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VGD_AppsRelacionComponente]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@TipoComponente", TipoComponente);
                        comando.Parameters.AddWithValue("@ComponenteId", EquipoId);
                        comando.Parameters.AddWithValue("@TipoRelacionId", TipoRelacionId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApsRelacionComponente()
                            {
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                NombreApt = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoRelacionId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                TipoRelacionDesc = reader.IsDBNull(reader.GetOrdinal("DescTipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRelacion"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ApsRelacionComponente> GetApsRelacionComponente(int EquipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ApsRelacionComponente> GetApsRelacionComponente(int EquipoId)"
                    , new object[] { null });
            }
        }

        public override List<AppsDependenciaImpacto> GetAppsDependenciaImpacto(string codigoAPT, string TipoBusqueda, int TipoRelacionId, int TipoEtiquetaId)
        {
            try
            {
                var lista = new List<AppsDependenciaImpacto>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    var storedProcedure = "";

                    storedProcedure = (TipoBusqueda == "D" ? "[cvt].[USP_VGD_AppsDependencia]" : "[cvt].[USP_VGD_AppsImpacto]");

                    using (var comando = new SqlCommand(storedProcedure, cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", codigoAPT);
                        comando.Parameters.AddWithValue("@TipoRelacionId", TipoRelacionId);
                        comando.Parameters.AddWithValue("@TipoEtiquetaId", TipoEtiquetaId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AppsDependenciaImpacto()
                            {
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                NombreAptOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                NombreAptDestino = reader.IsDBNull(reader.GetOrdinal("NombreAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptDestino")),
                                TipoRelacionId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                TipoRelacionDesc = reader.IsDBNull(reader.GetOrdinal("TipoRelacionDesc")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelacionDesc"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<AppsDependenciaImpacto> GetAppsDependenciaImpacto(string codigoAPT, string TipoBusqueda)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<AppsDependenciaImpacto> GetAppsDependenciaImpacto(string codigoAPT, string TipoBusqueda)"
                    , new object[] { null });
            }
        }

        public override List<RelacionAppToApp> GetRelacionApps(int RelacionId)
        {
            try
            {
                var lista = new List<RelacionAppToApp>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_VGD_RelacionApps]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionId", RelacionId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionAppToApp()
                            {
                                //RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionId")),
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                NombreAptOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOrigen")),
                                EquipoOrigen = reader.IsDBNull(reader.GetOrdinal("EquipoOrigen")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoOrigen")),
                                NomEquipoOrigen = reader.IsDBNull(reader.GetOrdinal("NomEquipoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NomEquipoOrigen")),
                                TipoEquipoIdOrigen = reader.IsDBNull(reader.GetOrdinal("TipoEquipoIdOrigen")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoIdOrigen")),
                                EqOrigen_SO = reader.IsDBNull(reader.GetOrdinal("EqOrigen_SO")) ? string.Empty : reader.GetString(reader.GetOrdinal("EqOrigen_SO")),
                                TipoRelAppEqOrigen_Id = reader.IsDBNull(reader.GetOrdinal("TipoRelAppEqOrigen_Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelAppEqOrigen_Id")),
                                TipoRelAppEqOrigen_Desc = reader.IsDBNull(reader.GetOrdinal("TipoRelAppEqOrigen_Desc")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelAppEqOrigen_Desc")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                NombreAptDestino = reader.IsDBNull(reader.GetOrdinal("NombreAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptDestino")),
                                EquipoDestino = reader.IsDBNull(reader.GetOrdinal("EquipoDestino")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoDestino")),
                                NomEquipoDestino = reader.IsDBNull(reader.GetOrdinal("NomEquipoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NomEquipoDestino")),
                                TipoEquipoIdDestino = reader.IsDBNull(reader.GetOrdinal("TipoEquipoIdDestino")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoIdDestino")),
                                EqDestino_SO = reader.IsDBNull(reader.GetOrdinal("EqDestino_SO")) ? string.Empty : reader.GetString(reader.GetOrdinal("EqDestino_SO")),
                                TipoRelAppEqDestino_Id = reader.IsDBNull(reader.GetOrdinal("TipoRelAppEqDestino_Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelAppEqDestino_Id")),
                                TipoRelAppEqDestino_Desc = reader.IsDBNull(reader.GetOrdinal("TipoRelAppEqDestino_Desc")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoRelAppEqDestino_Desc")),
                                TipoRelId_App = reader.IsDBNull(reader.GetOrdinal("TipoRelId_App")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelId_App")),
                                DescTipoRel_App = reader.IsDBNull(reader.GetOrdinal("DescTipoRel_App")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRel_App")),
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionApps> GetRelacionApps(int RelacionId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionApps> GetRelacionApps(int RelacionId)"
                    , new object[] { null });
            }
        }
        #endregion

        #region Diagrama de Infraestructura
        public override List<ServidoresRelacionadosDTO> GetListServers(Paginacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<ServidoresRelacionadosDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_BuscarServidores]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ServidoresRelacionadosDTO()
                            {
                                //CodAppOrigen = reader.IsDBNull(reader.GetOrdinal("CodAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodAppOrigen")),
                                //NombreAppOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAppOrigen")),
                                Relacionado = reader.IsDBNull(reader.GetOrdinal("Relacionado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Relacionado")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                NombreEquipo = reader.IsDBNull(reader.GetOrdinal("NombreEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),
                                SistemaOperativoEquipo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativoEquipo")),
                                EquipoId_Relacion = reader.IsDBNull(reader.GetOrdinal("EquipoId_Relacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId_Relacion")),
                                //NombreEquipoRelacionado = reader.IsDBNull(reader.GetOrdinal("NombreEquipoRelacionado")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipoRelacionado")),
                                //SistemaOperativoEquipoRelacionado = reader.IsDBNull(reader.GetOrdinal("SistemaOperativoEquipoRelacionado")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativoEquipoRelacionado")),
                                //CodAppRelacionada = reader.IsDBNull(reader.GetOrdinal("CodAppRelacionada")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodAppRelacionada")),
                                //NombreAppRelacionada = reader.IsDBNull(reader.GetOrdinal("NombreAppRelacionada")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAppRelacionada"))
                            };
                            lista.Add(objeto);
                        }
                        totalRows = lista.Count();
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ServidoresRelacionadosDTO> GetListServers(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ServidoresRelacionadosDTO> GetListServers(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ApisRelacionadasDTO> GetListApis(Paginacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<ApisRelacionadasDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_BuscarApis]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApisRelacionadasDTO()
                            {
                                //CodAppOrigen = reader.IsDBNull(reader.GetOrdinal("CodAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodAppOrigen")),
                                //NombreAppOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAppOrigen")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                NombreApi = reader.IsDBNull(reader.GetOrdinal("NombreApi")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreApi")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                NombreOwner = reader.IsDBNull(reader.GetOrdinal("NombreOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreOwner")),
                            };
                            lista.Add(objeto);
                        }
                        totalRows = lista.Count();
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ApisRelacionadasDTO> GetListApis(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ApisRelacionadasDTO> GetListApis(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ServiciosNubeRelacionadosDTO> GetListServiciosNube(Paginacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<ServiciosNubeRelacionadosDTO>();
                totalRows = 0;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_BuscarServiciosNube]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);
                        comando.Parameters.AddWithValue("@AmbienteId", pag.ambienteId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ServiciosNubeRelacionadosDTO()
                            {
                                //CodAppOrigen = reader.IsDBNull(reader.GetOrdinal("CodAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodAppOrigen")),
                                //NombreAppOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAppOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAppOrigen")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Suscripcion = reader.IsDBNull(reader.GetOrdinal("Suscripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Suscripcion")),
                                GrupoRecursos = reader.IsDBNull(reader.GetOrdinal("GrupoRecursos")) ? string.Empty : reader.GetString(reader.GetOrdinal("GrupoRecursos")),
                            };
                            lista.Add(objeto);
                        }
                        totalRows = lista.Count();
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ServiciosNubeRelacionadosDTO> GetListServiciosNube(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<ServiciosNubeRelacionadosDTO> GetListServiciosNube(Paginacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<RelacionReglasGeneralesDTO> GetReglasGenerales()
        {
            try
            {
                var lista = new List<RelacionReglasGeneralesDTO>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_VerReglasGenerales]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionReglasGeneralesDTO()
                            {
                                RelacionReglasGeneralesId = reader.IsDBNull(reader.GetOrdinal("RelacionReglasGeneralesId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionReglasGeneralesId")),
                                AplicaEn = reader.IsDBNull(reader.GetOrdinal("AplicaEn")) ? 0 : reader.GetInt32(reader.GetOrdinal("AplicaEn")),
                                Origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Origen")),
                                Destino = reader.IsDBNull(reader.GetOrdinal("Destino")) ? string.Empty : reader.GetString(reader.GetOrdinal("Destino"))
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasGeneralesDTO> GetReglasGenerales()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasGeneralesDTO> GetReglasGenerales()"
                    , new object[] { null });
            }
        }
        public override List<RelacionReglasPorAppTablaDTO> GetReglasPorApp(Paginacion pag)
        {
            try
            {
                var lista = new List<RelacionReglasPorAppTablaDTO>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_VerReglasPorApp]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionReglasPorAppTablaDTO()
                            {
                                RelacionReglasPorAppId = reader.IsDBNull(reader.GetOrdinal("RelacionReglasPorAppId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RelacionReglasPorAppId")),
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecPrincipal = reader.IsDBNull(reader.GetOrdinal("TecPrincipal")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecPrincipal")),
                                EquipoId_Relacion = reader.IsDBNull(reader.GetOrdinal("EquipoId_Relacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId_Relacion")),
                                Nombre_Relacion = reader.IsDBNull(reader.GetOrdinal("Nombre_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre_Relacion")),
                                TecPrincipal_Relacion = reader.IsDBNull(reader.GetOrdinal("TecPrincipal_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecPrincipal_Relacion")),
                                Relacionado = reader.IsDBNull(reader.GetOrdinal("Relacionado")) ? false : reader.GetBoolean(reader.GetOrdinal("Relacionado")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacionStr = reader.IsDBNull(reader.GetOrdinal("FechaCreacionStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaCreacionStr")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado")),
                                ListaIpEquipo = reader.IsDBNull(reader.GetOrdinal("ListaIpEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ListaIpEquipo")),
                                ListaIpEquipo_Relacion = reader.IsDBNull(reader.GetOrdinal("ListaIpEquipo_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("ListaIpEquipo_Relacion")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoId_Relacion = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId_Relacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId_Relacion")),
                                ClaveTecnologia_Componente = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia_Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia_Componente")),
                                ClaveTecnologia_ConectaCon = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia_ConectaCon")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia_ConectaCon")),
                                NombreAplicacion_Componente = reader.IsDBNull(reader.GetOrdinal("NombreAplicacion_Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAplicacion_Componente")),
                                NombreAplicacion_ConectaCon = reader.IsDBNull(reader.GetOrdinal("NombreAplicacion_ConectaCon")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAplicacion_ConectaCon"))
                            };
                            if (objeto.TipoEquipoId == 11)
                            {
                                objeto.Nombre = objeto.ClaveTecnologia_Componente;
                            }
                            if (objeto.TipoEquipoId_Relacion == 11)
                            {
                                objeto.Nombre_Relacion = objeto.ClaveTecnologia_ConectaCon;
                            }
                            if (objeto.TipoEquipoId == 12)
                            {
                                objeto.Nombre = objeto.NombreAplicacion_Componente;
                            }
                            if (objeto.TipoEquipoId_Relacion == 12)
                            {
                                objeto.Nombre_Relacion = objeto.NombreAplicacion_ConectaCon;
                            }
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasPorAppDTO> GetReglasPorApp(Paginacion pag)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasPorAppDTO> GetReglasPorApp(Paginacion pag)"
                    , new object[] { null });
            }
        }
        public override List<RelacionReglasPorAppDiagramaDTO> ListarReglasPorApp(Paginacion pag)
        {
            try
            {
                var lista = new List<RelacionReglasPorAppDiagramaDTO>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_ListarReglasPorApp]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionReglasPorAppDiagramaDTO()
                            {
                                CodigoApt = reader.IsDBNull(reader.GetOrdinal("CodigoApt")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoApt")),
                                Funcion = reader.IsDBNull(reader.GetOrdinal("Funcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Funcion")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TecPrincipal = reader.IsDBNull(reader.GetOrdinal("TecPrincipal")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecPrincipal")),
                                EquipoId_Relacion = reader.IsDBNull(reader.GetOrdinal("EquipoId_Relacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId_Relacion")),
                                Nombre_Relacion = reader.IsDBNull(reader.GetOrdinal("Nombre_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre_Relacion")),
                                TecPrincipal_Relacion = reader.IsDBNull(reader.GetOrdinal("TecPrincipal_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecPrincipal_Relacion")),
                                Relacionado = reader.IsDBNull(reader.GetOrdinal("Relacionado")) ? false : reader.GetBoolean(reader.GetOrdinal("Relacionado")),
                                ListaIpEquipo = reader.IsDBNull(reader.GetOrdinal("ListaIpEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ListaIpEquipo")),
                                ListaIpEquipo_Relacion = reader.IsDBNull(reader.GetOrdinal("ListaIpEquipo_Relacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("ListaIpEquipo_Relacion")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoId_Relacion = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId_Relacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId_Relacion")),
                                FlagIncluir_Componente = reader.IsDBNull(reader.GetOrdinal("FlagIncluir_Componente")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagIncluir_Componente")),
                                FlagIncluir_ConectaCon = reader.IsDBNull(reader.GetOrdinal("FlagIncluir_ConectaCon")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagIncluir_ConectaCon")),
                                ClaveTecnologia_Componente = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia_Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia_Componente")),
                                ClaveTecnologia_ConectaCon = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia_ConectaCon")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia_ConectaCon")),
                                NombreAplicacion_Componente = reader.IsDBNull(reader.GetOrdinal("NombreAplicacion_Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAplicacion_Componente")),
                                NombreAplicacion_ConectaCon = reader.IsDBNull(reader.GetOrdinal("NombreAplicacion_ConectaCon")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAplicacion_ConectaCon"))
                            };
                            if (objeto.TipoEquipoId == 11)
                            {
                                objeto.Nombre = objeto.ClaveTecnologia_Componente;
                            }
                            if (objeto.TipoEquipoId_Relacion == 11)
                            {
                                objeto.Nombre_Relacion = objeto.ClaveTecnologia_ConectaCon;
                            }
                            if (objeto.TipoEquipoId == 12)
                            {
                                objeto.Nombre = objeto.NombreAplicacion_Componente;
                            }
                            if (objeto.TipoEquipoId_Relacion == 12)
                            {
                                objeto.Nombre_Relacion = objeto.NombreAplicacion_ConectaCon;
                            }
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
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasPorAppDTO> ListarReglasPorApp(Paginacion pag)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<RelacionReglasPorAppDTO> ListarReglasPorApp(Paginacion pag)"
                    , new object[] { null });
            }
        }
        public override string ActualizarEstadoServidores(Paginacion pag)
        {
            var resultado = "";
            try
            {
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_EstadoServidores]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", pag.codigoAPT);
                        //comando.Parameters.Add(new SqlParameter { ParameterName = "@Resultado", Value = "", Direction = System.Data.ParameterDirection.InputOutput, SqlDbType = System.Data.SqlDbType.VarChar });

                        comando.ExecuteNonQuery();
                        //resultado = comando.Parameters["@Resultado"].Value.ToString();
                    }
                    cnx.Close();
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ActualizarEstadoServidores(Paginacion pag)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: string ActualizarEstadoServidores(Paginacion pag)"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocompleteComponentes> GetComponenteByFiltro_RelacionamientoComponentes(string filtro, string codigoApt)
        {
            try
            {
                var lista = new List<CustomAutocompleteComponentes>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_diagramainfraestructura_buscarcomponentes]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CodigoAPT", codigoApt);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocompleteComponentes()
                            {
                                ComponenteId = reader.IsDBNull(reader.GetOrdinal("ComponenteId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ComponenteId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return lista.Where(e => string.IsNullOrEmpty(filtro) || e.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList();
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<CustomAutocompleteComponentes> GetComponenteByFiltro_RelacionamientoComponentes(string filtro, string codigoApt)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<CustomAutocompleteComponentes> GetComponenteByFiltro_RelacionamientoComponentes(string filtro, string codigoApt)"
                    , new object[] { null });
            }
        }
        #endregion

    }
}
