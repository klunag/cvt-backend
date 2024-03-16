using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Log;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BCP.CVT.Services.Service
{
    public class QualySvc : QualyDAO
    {
        

        public override List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, bool withAsignadas, bool withDistinct, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                List<QualyDto> registros = new List<QualyDto>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GetQualys]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qualyId", ((object)qualyId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@titulo", ((object)titulo) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@nivelSeveridad", ((object)nivelSeveridad) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@withAsignaciones", withAsignadas);
                        comando.Parameters.AddWithValue("@withDistinct", withDistinct);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyDto();
                                    item.Id = dr.GetData<int>("QualyId");
                                    item.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    item.NivelSeveridad = dr.GetData<int>("NivelSeveridad");
                                    item.Titulo = dr.GetData<string>("Titulo");
                                    item.Categoria = dr.GetData<string>("Categoria");
                                    item.FechaPublicacion = dr.GetData<DateTime?>("FechaPublicacion");
                                    item.ListaSoftware = dr.GetData<string>("ListaSoftware");
                                    item.Diagnostico = dr.GetData<string>("Diagnostico");
                                    item.Codigo = dr.GetData<string>("Codigo");
                                    item.Producto = item.Codigo == null ? null : new DTO.ProductoDTO { Nombre = dr.GetData<string>("ProductoStr") };
                                    item.Solucion = dr.GetData<string>("Solucion");
                                    item.UsuarioCreacion = dr.GetData<string>("CreadoPor");
                                    item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                    item.UsuarioModificacion = dr.GetData<string>("ModificadoPor");
                                    item.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");

                                    registros.Add(item);
                                    totalRows = dr.GetData<int>("TotalRows");
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<QualyDto> GetQualysVulnerabilidadesPorEquipo(string qualyId, string equipo, string tipoVulnerabilidad, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                List<QualyDto> registros = new List<QualyDto>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYS_VULNERABILIDADES_POR_EQUIPO_REPORTE]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        comando.Parameters.AddWithValue("@qualyId", ((object)qualyId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@equipo", ((object)equipo) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tipoVulnerabilidad", ((object)tipoVulnerabilidad) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@titulo", ((object)titulo) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@nivelSeveridad", ((object)nivelSeveridad) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@withDistinct", withDistinct);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyDto();
                                    item.Id = dr.GetData<int>("QualyId");
                                    item.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    item.NivelSeveridad = dr.GetData<int>("NivelSeveridad");
                                    item.Titulo = dr.GetData<string>("Titulo");
                                    item.Categoria = dr.GetData<string>("Categoria");
                                    //item.FechaPublicacion = dr.GetData<DateTime?>("FechaPublicacion");
                                    //item.ListaSoftware = dr.GetData<string>("ListaSoftware");
                                    //item.Diagnostico = dr.GetData<string>("Diagnostico");
                                    //item.ProductoId = dr.GetData<int?>("ProductoId");
                                    //item.Producto = item.ProductoId == null ? null : new DTO.ProductoDTO { Nombre = dr.GetData<string>("ProductoStr") };
                                    //item.Solucion = dr.GetData<string>("Solucion");
                                    //item.ReferenciaVendedor = dr.GetData<string>("ReferenciaVendedor");
                                    //item.Amenaza = dr.GetData<string>("Amenaza");
                                    //item.Impacto = dr.GetData<string>("Impacto");
                                    //item.Explotabilidad = dr.GetData<string>("Explotabilidad");
                                    //item.MalwareAsociado = dr.GetData<string>("MalwareAsociado");
                                    //item.PCIVuln = dr.GetData<string>("PCIVuln");
                                    item.TecnologiaStr = dr.GetData<string>("TecnologiaStr");
                                    //item.TotalTecnologias = dr.GetData<int>("TotalTecnologias");
                                    //item.UsuarioCreacion = dr.GetData<string>("CreadoPor");
                                    //item.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                    //item.UsuarioModificacion = dr.GetData<string>("ModificadoPor");
                                    //item.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");
                                    item.EquipoNombre = dr.GetData<string>("EquipoNombre");
                                    item.TipoEquipoNombre = dr.GetData<string>("TipoEquipoNombre");

                                    registros.Add(item);
                                    totalRows = dr.GetData<int>("TotalRows");
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(string qualyId, string titulo, string nivelSeveridad, string productoStr, string tecnologiaStr, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel1(string qid, string equipo, string producto)
        {
            try
            {
                List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_NIVEL1]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        comando.Parameters.AddWithValue("@qid", qid);
                        comando.Parameters.AddWithValue("@equipo", equipo);
                        comando.Parameters.AddWithValue("@producto", producto);

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoDto();
                                    item.Leyenda = dr.GetData<string>("Leyenda");
                                    item.FlagEquipoAsignado = dr.GetData<bool>("FlagEquipoAsignado");
                                    item.Cantidad = dr.GetData<int>("Cantidad");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel2(string qid, string equipo, string producto, bool withEquipo)
        {
            try
            {
                List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_NIVEL2]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qid", qid);
                        comando.Parameters.AddWithValue("@equipo", equipo);
                        comando.Parameters.AddWithValue("@producto", producto);
                        comando.Parameters.AddWithValue("@withEquipo", withEquipo);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoDto();
                                    item.Leyenda = dr.GetData<string>("Leyenda");
                                    item.FlagEquipoAsignado = dr.GetData<bool>("FlagEquipoAsignado");
                                    //item.Severidad = dr.GetData<string>("Severidad");
                                    item.FlagProductoTecnologiaAsignado = dr.GetData<bool>("FlagProductoTecnologiaAsignado");
                                    item.Cantidad = dr.GetData<int>("Cantidad");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        //public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel3(string qid, string equipo, string producto, bool withEquipo, string severidad)
        public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel3(string qid, string equipo, string producto, bool withEquipo, bool withProductoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_NIVEL3]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qid", qid);
                        comando.Parameters.AddWithValue("@equipo", equipo);
                        comando.Parameters.AddWithValue("@producto", producto);
                        comando.Parameters.AddWithValue("@withEquipo", withEquipo);
                        comando.Parameters.AddWithValue("@withProductoTecnologia", withProductoTecnologia);
                        //comando.Parameters.AddWithValue("@severity", (object)severidad ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoDto();
                                    item.Fila = dr.GetData<int>("Fila");
                                    item.QID = dr.GetData<int>("QID");
                                    item.IP = dr.GetData<string>("IP");
                                    item.NetBIOS = dr.GetData<string>("NetBIOS");
                                    item.Titulo = dr.GetData<string>("Titulo");
                                    item.Categoria = dr.GetData<string>("Categoria");
                                    item.Diagnostico = dr.GetData<string>("Diagnostico");
                                    item.Solucion = dr.GetData<string>("Solucion");
                                    item.EquipoStr = dr.GetData<string>("EquipoStr");
                                    item.ProductoStr = dr.GetData<string>("ProductoStr");
                                    item.TecnologiaStr = dr.GetData<string>("TecnologiaStr");
                                    item.FlagEquipoAsignado = dr.GetData<bool>("FlagEquipoAsignado");
                                    //item.Severidad = dr.GetData<string>("Severidad");
                                    item.FlagProductoTecnologiaAsignado = dr.GetData<bool>("FlagProductoTecnologiaAsignado");
                                    //item.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    //item.Cantidad = dr.GetData<int>("Cantidad");
                                    totalRows = dr.GetData<int>("TotalRows");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel4(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia)
        {
            try
            {
                List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_NIVEL4]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qid", qid);
                        comando.Parameters.AddWithValue("@equipo", equipo);
                        comando.Parameters.AddWithValue("@producto", producto);
                        comando.Parameters.AddWithValue("@withEquipo", withEquipo);
                        comando.Parameters.AddWithValue("@severity", (object)severidad ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@withProductoTecnologia", withProductoTecnologia);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoDto();
                                    item.Leyenda = dr.GetData<string>("Leyenda");
                                    item.FlagEquipoAsignado = dr.GetData<bool>("FlagEquipoAsignado");
                                    item.Severidad = dr.GetData<string>("Severidad");
                                    item.FlagProductoTecnologiaAsignado = dr.GetData<bool>("FlagProductoTecnologiaAsignado");
                                    item.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    item.Cantidad = dr.GetData<int>("Cantidad");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<QualyConsolidadoDto> GetQualysConsolidadoNivel5(string qid, string equipo, string producto, bool withEquipo, string severidad, bool withProductoTecnologia, string tipoVulnerabilidad, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_NIVEL5]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qid", qid);
                        comando.Parameters.AddWithValue("@equipo", equipo);
                        comando.Parameters.AddWithValue("@producto", producto);
                        comando.Parameters.AddWithValue("@withEquipo", withEquipo);
                        comando.Parameters.AddWithValue("@severity", (object)severidad ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@withProductoTecnologia", withProductoTecnologia);
                        comando.Parameters.AddWithValue("@tipoVulnerabilidad", (object)tipoVulnerabilidad ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageNumber", pageNumber);
                        comando.Parameters.AddWithValue("@pageSize", pageSize);
                        comando.Parameters.AddWithValue("@sortName", sortName);
                        comando.Parameters.AddWithValue("@sortOrder", sortOrder);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoDto();
                                    item.IP = dr.GetData<string>("IP");
                                    item.NetBIOS = dr.GetData<string>("NetBIOS");
                                    item.Titulo = dr.GetData<string>("Titulo");
                                    item.Categoria = dr.GetData<string>("Categoria");
                                    item.Diagnostico = dr.GetData<string>("Diagnostico");
                                    item.Solucion = dr.GetData<string>("Solucion");
                                    item.ProductoStr = dr.GetData<string>("ProductoStr");
                                    item.TecnologiaStr = dr.GetData<string>("TecnologiaStr");
                                    item.FlagEquipoAsignado = dr.GetData<bool>("FlagEquipoAsignado");
                                    item.Severidad = dr.GetData<string>("Severidad");
                                    item.FlagProductoTecnologiaAsignado = dr.GetData<bool>("FlagProductoTecnologiaAsignado");
                                    item.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    //item.Cantidad = dr.GetData<int>("Cantidad");
                                    totalRows = dr.GetData<int>("TotalRows");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        //public override List<QualyConsolidadoDto> GetQualysConsolidadoExportar(string qid, string equipo, string producto)
        //{
        //    try
        //    {
        //        List<QualyConsolidadoDto> registros = new List<QualyConsolidadoDto>();
        //        using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
        //        {
        //            cnx.Open();
        //            using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_EXPORTAR]", cnx))
        //            {
        //                comando.CommandType = System.Data.CommandType.StoredProcedure;
        //                comando.Parameters.AddWithValue("@qid", qid);
        //                comando.Parameters.AddWithValue("@equipo", equipo);
        //                comando.Parameters.AddWithValue("@producto", producto);
        //                comando.CommandTimeout = 0;

        //                using (var dr = comando.ExecuteReader())
        //                {

        //                    if (dr.HasRows)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            var item = new QualyConsolidadoDto();
        //                            item.Fila = dr.GetData<int>("Fila");
        //                            item.QID = dr.GetData<int>("QID");
        //                            item.IP = dr.GetData<string>("IP");
        //                            item.DNS = dr.GetData<string>("DNS");
        //                            item.NetBIOS = dr.GetData<string>("NetBIOS");
        //                            item.SO = dr.GetData<string>("OS");
        //                            //item.Titulo = dr.GetData<string>("Titulo");
        //                            //item.Categoria = dr.GetData<string>("Categoria");
        //                            //item.Diagnostico = dr.GetData<string>("Diagnostico");
        //                            //item.Solucion = dr.GetData<string>("Solucion");
        //                            item.EquipoAsignadoStr = dr.GetData<string>("EquipoAsignadoStr");
        //                            item.ProductoTecnologiaAsignadoStr = dr.GetData<string>("ProductoTecnologiaAsignadoStr");
        //                            item.EquipoStr = dr.GetData<string>("EquipoStr");
        //                            item.ProductoStr = dr.GetData<string>("ProductoStr");
        //                            item.DominioStr = dr.GetData<string>("DominioStr");
        //                            item.SubDominioStr = dr.GetData<string>("SubDominioStr");
        //                            item.TecnologiaStr = dr.GetData<string>("TecnologiaStr");
        //                            item.EstadoIdTecnologia = dr.GetData<int>("EstadoId");
        //                            item.TipoTecnologiaStr = dr.GetData<string>("TipoTecnologiaStr");
        //                            //totalRows = dr.GetData<int>("TotalRows");

        //                            registros.Add(item);
        //                        }

        //                    }
        //                }

        //            }
        //            cnx.Close();

        //            return registros;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        HelperLog.Error(ex.Message);
        //        throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
        //            , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
        //            , new object[] { null });
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperLog.Error(ex.Message);
        //        throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
        //            , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
        //            , new object[] { null });
        //    }
        //}

        public override List<QualyConsolidadoCsvDto> GetQualysConsolidadoExportar(/*string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, int? unidadOrganizativaId, int? squadId, string equipoStr, string estadosVulnerabilidad, bool? tieneEquipoAsignado, bool? tieneProductoAsignado, bool? tieneTecnologiaAsignado, */int pageNumber, int pageSize, out int totalRows)
        {
            try
            {
                List<QualyConsolidadoCsvDto> registros = new List<QualyConsolidadoCsvDto>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_EXPORTAR]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        //comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@unidadOrganizativaId", ((object)unidadOrganizativaId) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@squadId", ((object)squadId) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@equipoStr", ((object)equipoStr) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@estadosVulnerabilidad", ((object)estadosVulnerabilidad) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@tieneEquipoAsignado", ((object)tieneEquipoAsignado) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@tieneProductoAsignado", ((object)tieneProductoAsignado) ?? DBNull.Value);
                        //comando.Parameters.AddWithValue("@tieneTecnologiaAsignado", ((object)tieneTecnologiaAsignado) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageNumber", ((object)pageNumber) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@pageSize", ((object)pageSize) ?? DBNull.Value);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new QualyConsolidadoCsvDto();
                                    //item.Fila = dr.GetData<int>("Fila");
                                    item.QID = dr.GetData<int>("QID");
                                    item.IP = dr.GetData<string>("IP");
                                    item.DNS = dr.GetData<string>("DNS");
                                    item.NetBIOS = dr.GetData<string>("NetBIOS");
                                    item.SO = dr.GetData<string>("OS");
                                    //item.Titulo = dr.GetData<string>("Titulo");
                                    //item.Categoria = dr.GetData<string>("Categoria");
                                    //item.Diagnostico = dr.GetData<string>("Diagnostico");
                                    //item.Solucion = dr.GetData<string>("Solucion");
                                    //item.EquipoAsignadoStr = dr.GetData<string>("EquipoAsignadoStr");
                                    //item.ProductoTecnologiaAsignadoStr = dr.GetData<string>("ProductoTecnologiaAsignadoStr");
                                    item.EquipoStr = dr.GetData<string>("EquipoStr");
                                    item.ProductoStr = dr.GetData<string>("ProductoStr");
                                    item.DominioStr = dr.GetData<string>("DominioStr");
                                    item.SubDominioStr = dr.GetData<string>("SubDominioStr");
                                    item.TribuCoeStr = dr.GetData<string>("TribuCoeDisplayName");
                                    item.SquadStr = dr.GetData<string>("SquadDisplayName");
                                    item.TecnologiaStr = dr.GetData<string>("TecnologiaStr");
                                    //item.EstadoIdTecnologia = dr.GetData<int>("EstadoId");
                                    //item.TipoTecnologiaStr = dr.GetData<string>("TipoTecnologiaStr");
                                    //totalRows = dr.GetData<int>("TotalRows");
                                    item.TrackingMethod = dr.GetData<string>("TrackingMethod");
                                    item.IPStatus = dr.GetData<string>("IPStatus");
                                    item.Title = dr.GetData<string>("Title");
                                    item.VulnStatus = dr.GetData<string>("VulnStatus");
                                    item.Type = dr.GetData<string>("Type");
                                    item.Severity = dr.GetData<string>("Severity");
                                    item.Port = dr.GetData<string>("Port");
                                    item.Protocol = dr.GetData<string>("Protocol");
                                    item.FQDN = dr.GetData<string>("FQDN");
                                    item.SSL = dr.GetData<string>("SSL");
                                    item.FirstDetected = dr.GetData<string>("FirstDetected");
                                    item.LastDetected = dr.GetData<string>("LastDetected");
                                    item.TimesDetected = dr.GetData<string>("TimesDetected");
                                    item.DateLastFixed = dr.GetData<string>("DateLastFixed");
                                    item.FirstReopened = dr.GetData<string>("FirstReopened");
                                    item.LastReopened = dr.GetData<string>("LastReopened");
                                    item.TimesReopened = dr.GetData<string>("TimesReopened");
                                    item.CVEId = dr.GetData<string>("CVEId");
                                    item.VendorReference = dr.GetData<string>("VendorReference");
                                    item.BugtraqId = dr.GetData<string>("BugtraqId");
                                    item.CVSS = dr.GetData<string>("CVSS");
                                    item.CVSSBase = dr.GetData<string>("CVSSBase");
                                    item.CVSSTemporal = dr.GetData<string>("CVSSTemporal");
                                    item.CVSSEnvironment = dr.GetData<string>("CVSSEnvironment");
                                    item.CVSS3 = dr.GetData<string>("CVSS3");
                                    item.CVSS3Base = dr.GetData<string>("CVSS3Base");
                                    item.CVSS3Temporal = dr.GetData<string>("CVSS3Temporal");
                                    item.Threat = dr.GetData<string>("Threat");
                                    item.Impact = dr.GetData<string>("Impact");
                                    item.Solution = dr.GetData<string>("Solution");
                                    item.Exploitability = dr.GetData<string>("Exploitability");
                                    item.AssociatedMalware = dr.GetData<string>("AssociatedMalware");
                                    item.Results = dr.GetData<string>("Results");
                                    item.PCIVuln = dr.GetData<string>("PCIVuln");
                                    item.TicketState = dr.GetData<string>("TicketState");
                                    item.Instance = dr.GetData<string>("Instance");
                                    item.Category = dr.GetData<string>("Category");
                                    item.AssociatedTags = dr.GetData<string>("AssociatedTags");
                                    item.TieneEquipoStr = dr.GetData<bool>("TieneEquipo") ? "Sí" : "No";
                                    item.TieneTecnologiaStr = dr.GetData<bool>("TieneTecnologia") ? "Sí" : "No";
                                    item.TieneProductoStr = dr.GetData<bool>("TieneProducto") ? "Sí" : "No";

                                    totalRows = dr.GetData<int>("TotalRows");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , $"Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1() {ex.Message}"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , $"Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1() {ex.Message}"
                    , new object[] { null });
            }
        }

        public override QualyDto GetQualyById(int qualyId)
        {
            try
            {
                QualyDto registro = null;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GetQualyById]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@qualyId", qualyId);

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                registro = new QualyDto();
                                if (dr.Read())
                                {
                                    registro.Id = dr.GetData<int>("QualyId");
                                    registro.TipoVulnerabilidad = dr.GetData<string>("TipoVulnerabilidad");
                                    registro.NivelSeveridad = dr.GetData<int>("NivelSeveridad");
                                    registro.Titulo = dr.GetData<string>("Titulo");
                                    registro.Categoria = dr.GetData<string>("Categoria");
                                    registro.FechaPublicacion = dr.GetData<DateTime?>("FechaPublicacion");
                                    registro.ListaSoftware = dr.GetData<string>("ListaSoftware");
                                    registro.Diagnostico = dr.GetData<string>("Diagnostico");
                                    registro.ProductoId = dr.GetData<int?>("ProductoId");
                                    registro.Producto = registro.ProductoId == null ? null : new DTO.ProductoDTO { Nombre = dr.GetData<string>("ProductoStr") };
                                    registro.Solucion = dr.GetData<string>("Solucion");
                                    registro.ReferenciaVendedor = dr.GetData<string>("ReferenciaVendedor");
                                    registro.Amenaza = dr.GetData<string>("Amenaza");
                                    registro.Impacto = dr.GetData<string>("Impacto");
                                    registro.Explotabilidad = dr.GetData<string>("Explotabilidad");
                                    registro.MalwareAsociado = dr.GetData<string>("MalwareAsociado");
                                    registro.PCIVuln = dr.GetData<string>("PCIVuln");
                                    registro.UsuarioCreacion = dr.GetData<string>("CreadoPor");
                                    registro.FechaCreacion = dr.GetData<DateTime>("FechaCreacion");
                                    registro.UsuarioModificacion = dr.GetData<string>("ModificadoPor");
                                    registro.FechaModificacion = dr.GetData<DateTime?>("FechaModificacion");
                                }

                            }
                        }
                    }

                    cnx.Close();

                    return registro;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyDto> GetQualys(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool SaveQualy(QualyDto registro)
        {
            try
            {
                bool seGuardo = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (var comando = new SqlCommand("[cvt].[USP_SaveQualy]", cnx))
                        {
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter { ParameterName = "@qualyId", Value = registro.Id == -1 ? DBNull.Value : (object)registro.Id, Direction = System.Data.ParameterDirection.InputOutput, SqlDbType = System.Data.SqlDbType.Int });
                            comando.Parameters.AddWithValue("@tipoVulnerabilidad", registro.TipoVulnerabilidad);
                            comando.Parameters.AddWithValue("@nivelSeveridad", registro.NivelSeveridad);
                            comando.Parameters.AddWithValue("@titulo", registro.Titulo);
                            comando.Parameters.AddWithValue("@categoria", registro.Categoria);
                            comando.Parameters.AddWithValue("@fechaPublicacion", (object)registro.FechaPublicacion ?? DBNull.Value);
                            comando.Parameters.AddWithValue("@listaSoftware", registro.ListaSoftware);
                            comando.Parameters.AddWithValue("@diagnostico", registro.Diagnostico);
                            comando.Parameters.AddWithValue("@productoId", registro.ProductoId.HasValue ? registro.ProductoId.Value : 0);
                            comando.Parameters.AddWithValue("@solucion", registro.Solucion);                            
                            comando.Parameters.AddWithValue("@usuario", registro.UsuarioCreacion);

                            int filasAfectadas = comando.ExecuteNonQuery();
                            seGuardo = filasAfectadas > 0;
                            if (seGuardo) registro.Id = (int)comando.Parameters["@qualyId"].Value;
                        }                        

                        if (seGuardo) scope.Complete();

                        cnx.Close();

                        return seGuardo;
                    }
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override QualysConsolidadoKPIDto GetQualysConsolidadoKPI(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, int? unidadOrganizativaId, int? squadId, string equipoStr, string estadosVulnerabilidad, bool? tieneEquipoAsignado, bool? tieneProductoAsignado, bool? tieneTecnologiaAsignado)
        {
            try
            {
                QualysConsolidadoKPIDto registro = null;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_qualys_consolidado_kpi]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@dominioIds", ((object)dominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@subDominioIds", ((object)subDominioIds) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@productoStr", ((object)productoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tecnologiaStr", ((object)tecnologiaStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@unidadOrganizativaId", ((object)unidadOrganizativaId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@squadId", ((object)squadId) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@equipoStr", ((object)equipoStr) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@estadosVulnerabilidad", ((object)estadosVulnerabilidad) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tieneEquipoAsignado", ((object)tieneEquipoAsignado) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tieneProductoAsignado", ((object)tieneProductoAsignado) ?? DBNull.Value);
                        comando.Parameters.AddWithValue("@tieneTecnologiaAsignado", ((object)tieneTecnologiaAsignado) ?? DBNull.Value);
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    registro = new QualysConsolidadoKPIDto();
                                    registro.CantidadEquiposCVTEncontrados = dr.GetData<int>("CantidadEquiposCVTEncontrados");
                                    registro.CantidadEquiposCVTNoEncontrados = dr.GetData<int>("CantidadEquiposCVTNoEncontrados");
                                    registro.CantidadProductoAsignado = dr.GetData<int>("CantidadProductoAsignado");
                                    registro.CantidadProductoNoAsignado = dr.GetData<int>("CantidadProductoNoAsignado");
                                    registro.CantidadTecnologiaAsignada = dr.GetData<int>("CantidadTecnologiaAsignada");
                                    registro.CantidadTecnologiaNoAsignada = dr.GetData<int>("CantidadTecnologiaNoAsignada");
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registro;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetQualysVulnStatusList()
        {
            try
            {
                List<CustomAutocomplete> registros = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_estadovulnerabilidad_listar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new CustomAutocomplete();
                                    item.Id = dr.GetData<string>("Vuln_Status");
                                    item.Descripcion = dr.GetData<string>("Vuln_Status");

                                    registros.Add(item);
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override bool VolcarDatosReporteQualysConsolidado()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_QUALYVULNERABILIDADES_REPORTECONSOLIDADO_PROCESAR]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override bool ActualizarEquipoIdEnQualysVulnServidores()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[UPDATE_EQUIPOID_FROM_QUALYS_VULNSERVIDORES]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override bool TransferenciaReporteGeneralSemanaAQualysVulnServidores()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_transfer_reporte_general_semanal_to_qualys_vulnservidores]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override bool TransferenciaReporteGeneralSemanaAQualy()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_transfer_reporte_general_semanal_to_qualys]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override QualysConsolidadoKPIDto GetAlertaQualysConsolidadoKPI()
        {
            try
            {
                QualysConsolidadoKPIDto registro = null;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_alerta_qualys_equiposnoencontrados_tecnologiasinstaladasnoregistradas]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;

                        using (var dr = comando.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    registro = new QualysConsolidadoKPIDto();
                                    registro.CantidadEquiposExistentes = dr.GetData<int>("CantidadEquiposExistentes");
                                    registro.CantidadEquiposCVTEncontrados = dr.GetData<int>("CantidadEquiposEncontrados");
                                    registro.CantidadEquiposCVTNoEncontrados = dr.GetData<int>("CantidadEquiposNoEncontrados");
                                    registro.CantidadTecnologiasInstaladasExistentes = dr.GetData<int>("CantidadTecnologiasInstaladasExistentes");
                                    registro.CantidadTecnologiasInstaladasRegistradas = dr.GetData<int>("CantidadTecnologiasInstaladasRegistradas");
                                    registro.CantidadTecnologiasInstaladasNoRegistradas = dr.GetData<int>("CantidadTecnologiasInstaladasNoRegistradas");
                                }

                            }
                        }

                    }
                    cnx.Close();

                    return registro;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<QualyConsolidadoDto> GetQualysConsolidadoNivel1()"
                    , new object[] { null });
            }
        }

        public override bool VolcarDatosKPI()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_qualys_consolidado_kpi_procesar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override bool VolcarDatosEquiposNoRegistrados()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_equiponoregistro_procesar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override bool VolcarDatosTecnologiasInstaladasNoRegistradas()
        {
            try
            {
                bool seVolco = false;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_tecnologianoregistradoqualys_procesar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 0;
                        int filasAfectadas = comando.ExecuteNonQuery();
                        seVolco = filasAfectadas > 0;
                    }

                    cnx.Close();

                    return seVolco;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool GetQualys(QualyDto registro)"
                    , new object[] { null });
            }
        }

        public override List<VulnerabilidadEquipoDto> GetVulnerabilidadesAplicacion(int tecnologia, int qid, string subdominios, string dominios, string gestionado, string squads, string estado, string codigoAPT, int pageNumber, int pageSize, int perfil, string matricula, out int totalRows)
        {
            try
            {
                List<VulnerabilidadEquipoDto> registros = new List<VulnerabilidadEquipoDto>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    if (perfil == (int)EPerfilBCP.Administrador
                           || perfil == (int)EPerfilBCP.Seguridad
                           || perfil == (int)EPerfilBCP.Auditoria
                           || perfil == (int)EPerfilBCP.GestorCVT_CatalogoTecnologias)
                    {
                        using (var comando = new SqlCommand("[cvt].[USP_VulnerabilidadesPorAplicacion]", cnx))
                        {
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@tecnologia", ((object)tecnologia) ?? 0);
                            comando.Parameters.AddWithValue("@qid", ((object)qid) ?? 0);
                            comando.Parameters.AddWithValue("@subdominios", ((object)subdominios) ?? string.Empty);
                            comando.Parameters.AddWithValue("@dominios", ((object)dominios) ?? string.Empty);
                            comando.Parameters.AddWithValue("@estado", ((object)estado) ?? string.Empty);
                            comando.Parameters.AddWithValue("@codigoAPT", ((object)codigoAPT) ?? string.Empty);
                            comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionado) ?? string.Empty);
                            comando.Parameters.AddWithValue("@squads", ((object)squads) ?? string.Empty);
                            comando.Parameters.AddWithValue("@PageSize", ((object)pageSize) ?? 1);
                            comando.Parameters.AddWithValue("@PageNumber", ((object)pageNumber) ?? 30);
                            comando.CommandTimeout = 0;

                            using (var dr = comando.ExecuteReader())
                            {

                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        var item = new VulnerabilidadEquipoDto();

                                        item.QID = dr.GetData<int>("QID");
                                        item.Aplicacion = dr.GetData<string>("Aplicacion");
                                        item.CodigoAPT = dr.GetData<string>("CodigoAPT");
                                        item.Date_Last_Fixed = dr.GetData<string>("Date_Last_Fixed");
                                        item.Dominio = dr.GetData<string>("Dominio");
                                        item.EquipoId = dr.GetData<int>("EquipoId");
                                        item.status = dr.GetData<int>("status");
                                        item.First_Detected = dr.GetData<string>("First_Detected");
                                        item.Last_Detected = dr.GetData<string>("Last_Detected");
                                        item.NivelSeveridad = dr.GetData<int>("NivelSeveridad");
                                        item.Nombre = dr.GetData<string>("Nombre");
                                        item.Port = dr.GetData<string>("Port");
                                        item.Producto = dr.GetData<string>("Producto");
                                        item.Protocol = dr.GetData<string>("Protocol");
                                        item.Solucion = dr.GetData<string>("Solucion");
                                        item.Subdominio = dr.GetData<string>("Subdominio");
                                        item.Times_Detected = dr.GetData<int>("Times_Detected");
                                        item.Times_Reopened = dr.GetData<int>("Times_Reopened");
                                        item.Titulo = dr.GetData<string>("Titulo");
                                        item.VulnStatusId = dr.GetData<int>("VulnStatusId");
                                        item.Codigo = dr.GetData<string>("Codigo");
                                        item.GestionadoPor = dr.GetData<string>("GestionadoPor");
                                        item.TeamSquad = dr.GetData<string>("TeamSquad");
                                        item.Categoria = dr.GetData<string>("Categoria");

                                        totalRows = dr.GetData<int>("totalFilas");

                                        registros.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var comando = new SqlCommand("[cvt].[USP_VulnerabilidadesPorAplicacion_Consultor]", cnx))
                        {
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@tecnologia", ((object)tecnologia) ?? 0);
                            comando.Parameters.AddWithValue("@qid", ((object)qid) ?? 0);
                            comando.Parameters.AddWithValue("@subdominios", ((object)subdominios) ?? string.Empty);
                            comando.Parameters.AddWithValue("@dominios", ((object)dominios) ?? string.Empty);
                            comando.Parameters.AddWithValue("@estado", ((object)estado) ?? string.Empty);
                            comando.Parameters.AddWithValue("@codigoAPT", ((object)codigoAPT) ?? string.Empty);
                            comando.Parameters.AddWithValue("@gestionadoPor", ((object)gestionado) ?? string.Empty);
                            comando.Parameters.AddWithValue("@squads", ((object)squads) ?? string.Empty);
                            comando.Parameters.AddWithValue("@matricula", ((object)matricula) ?? string.Empty);
                            comando.Parameters.AddWithValue("@PageSize", ((object)pageSize) ?? 1);
                            comando.Parameters.AddWithValue("@PageNumber", ((object)pageNumber) ?? 30);
                            comando.CommandTimeout = 0;

                            using (var dr = comando.ExecuteReader())
                            {

                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        var item = new VulnerabilidadEquipoDto();

                                        item.QID = dr.GetData<int>("QID");
                                        item.Aplicacion = dr.GetData<string>("Aplicacion");
                                        item.CodigoAPT = dr.GetData<string>("CodigoAPT");
                                        item.Date_Last_Fixed = dr.GetData<string>("Date_Last_Fixed");
                                        item.Dominio = dr.GetData<string>("Dominio");
                                        item.EquipoId = dr.GetData<int>("EquipoId");
                                        item.status = dr.GetData<int>("status");
                                        item.First_Detected = dr.GetData<string>("First_Detected");
                                        item.Last_Detected = dr.GetData<string>("Last_Detected");
                                        item.NivelSeveridad = dr.GetData<int>("NivelSeveridad");
                                        item.Nombre = dr.GetData<string>("Nombre");
                                        item.Port = dr.GetData<string>("Port");
                                        item.Producto = dr.GetData<string>("Producto");
                                        item.Protocol = dr.GetData<string>("Protocol");
                                        item.Solucion = dr.GetData<string>("Solucion");
                                        item.Subdominio = dr.GetData<string>("Subdominio");
                                        item.Times_Detected = dr.GetData<int>("Times_Detected");
                                        item.Times_Reopened = dr.GetData<int>("Times_Reopened");
                                        item.Titulo = dr.GetData<string>("Titulo");
                                        item.VulnStatusId = dr.GetData<int>("VulnStatusId");
                                        item.Codigo = dr.GetData<string>("Codigo");
                                        item.GestionadoPor = dr.GetData<string>("GestionadoPor");
                                        item.TeamSquad = dr.GetData<string>("TeamSquad");
                                        item.Categoria = dr.GetData<string>("Categoria");

                                        totalRows = dr.GetData<int>("totalFilas");

                                        registros.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    
                    cnx.Close();

                     return registros;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , $"Error en el metodo: List<VulnerabilidadEquipoDto> GetVulnerabilidadesAplicacion() {ex.Message}"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , $"Error en el metodo: List<VulnerabilidadEquipoDto> GetVulnerabilidadesAplicacion() {ex.Message}"
                    , new object[] { null });
            }
        }
    }
}
