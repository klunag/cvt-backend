using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class ModeloSvc : ModeloDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int Agregarodelo(ModeloDTO modDTO)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_Insertar_ModeloHardware]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", modDTO.nombre));
                        comando.Parameters.Add(new SqlParameter("@fabricante", modDTO.fabricante));
                        comando.Parameters.Add(new SqlParameter("@flagfechafinsoporte", modDTO.FlagFechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@fechafinsorporteextendida", modDTO.FechaFinSoporteExtendida));
                        comando.Parameters.Add(new SqlParameter("@fechafinsoporte", modDTO.FechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@fechainterna", modDTO.FechaInterna));
                        comando.Parameters.Add(new SqlParameter("@tipofechainterna", modDTO.TipoFechaInterna));
                        comando.Parameters.Add(new SqlParameter("@comentraiofechafinsoporte", modDTO.ComentarioFechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@motivofechaindefinida", modDTO.MotivoFechaIndefinida));
                        comando.Parameters.Add(new SqlParameter("@urlfechaindefinida", modDTO.urlFechaIndefinida));
                        comando.Parameters.Add(new SqlParameter("@usuario", modDTO.Usuario));
                        comando.Parameters.Add(new SqlParameter("@fechafabricacion", modDTO.FechaFabricacion));
                        comando.Parameters.Add(new SqlParameter("@tipoequipo", modDTO.tipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@urlSharepoint", modDTO.UrlSharepoint));
                        comando.Parameters.Add(new SqlParameter("@urlLineamientos", modDTO.UrlLineamiento));
                        comando.Parameters.Add(new SqlParameter("@Remedy", modDTO.Remedy));
                        comando.Parameters.Add(new SqlParameter("@RemedyNombre", modDTO.RemedyNombre));

                        comando.Parameters.Add(new SqlParameter("@Categoria", modDTO.Categoria));
                        comando.Parameters.Add(new SqlParameter("@Tipo", modDTO.Tipo));
                        comando.Parameters.Add(new SqlParameter("@Item", modDTO.Item));
                        comando.Parameters.Add(new SqlParameter("@TipoFechaCalculo", modDTO.tipofechacalculo));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
                return 1;
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ModeloDTO> LeerModelo(string nombre, int tipo, string codigo, string nroSerie, string hostName, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<ModeloDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Leer]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        comando.Parameters.Add(new SqlParameter("@criterio", nombre));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@codigoEquipo", codigo));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortOrder));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortName));
                        comando.Parameters.Add(new SqlParameter("@nroSerie", nroSerie));
                        comando.Parameters.Add(new SqlParameter("@hostName", hostName));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ModeloDTO()
                            {
                                idmodelo = reader.IsDBNull(reader.GetOrdinal("ModeloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModeloId")),
                                tipoEquipoNombre = reader.IsDBNull(reader.GetOrdinal("tipoequipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipoequipo")),
                                fabricante = reader.IsDBNull(reader.GetOrdinal("fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("fabricante")),
                                nombre = reader.IsDBNull(reader.GetOrdinal("modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modelo")),
                                FechaCreacionToString = reader.IsDBNull(reader.GetOrdinal("fechacreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("fechacreacion")),
                                FechaFinToString = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaFin")),
                                Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Categoria")),
                                indicador = reader.IsDBNull(reader.GetOrdinal("indicador")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicador")),
                                indicadorproyectado = reader.IsDBNull(reader.GetOrdinal("indicadorproyectado")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicadorproyectado")),
                                indicadorproyectado2 = reader.IsDBNull(reader.GetOrdinal("indicadorproyectado2")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicadorproyectado2")),
                                totalRows = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("flagtemporal")) ? false : reader.GetBoolean(reader.GetOrdinal("flagtemporal"))
                            };
                            lista.Add(objeto);
                        }
                    }

                    cnx.Close();
                    if (lista.Count > 0)
                        totalRows = lista[0].totalRows;
                }
                return lista;
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override void DeleteModelo(int id, string usuario)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Delete]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));
                        comando.Parameters.Add(new SqlParameter("@usuario", usuario));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override ModeloDTO BuscarModelo(int id)
        {
            try
            {
                var objeto = new ModeloDTO();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Buscar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", id));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            objeto.idmodelo = reader.IsDBNull(reader.GetOrdinal("ModeloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModeloId"));
                            objeto.tipoEquipoId = reader.IsDBNull(reader.GetOrdinal("tipoequipoid")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipoequipoid"));
                            objeto.fabricante = reader.IsDBNull(reader.GetOrdinal("fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("fabricante"));
                            objeto.UrlSharepoint = reader.IsDBNull(reader.GetOrdinal("urlSharepoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("urlSharepoint"));
                            objeto.UrlLineamiento = reader.IsDBNull(reader.GetOrdinal("urlLineamientos")) ? string.Empty : reader.GetString(reader.GetOrdinal("urlLineamientos"));
                            objeto.Remedy = reader.IsDBNull(reader.GetOrdinal("Remedy")) ? 0 : reader.GetInt32(reader.GetOrdinal("Remedy"));
                            objeto.RemedyNombre = reader.IsDBNull(reader.GetOrdinal("RemedyNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("RemedyNombre"));
                            objeto.nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre"));
                            objeto.FechaFabricacion = reader.IsDBNull(reader.GetOrdinal("fechafabricacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafabricacion"));
                            objeto.FlagFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("flagfinsoporte")) ? false : reader.GetBoolean(reader.GetOrdinal("flagfinsoporte"));
                            objeto.FechaFinSoporteExtendida = reader.IsDBNull(reader.GetOrdinal("fechafinsoporteextendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafinsoporteextendida"));
                            objeto.FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("fechafinsoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafinsoporte"));
                            objeto.TipoFechaInterna = reader.IsDBNull(reader.GetOrdinal("tipointerna")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipointerna"));
                            objeto.ComentarioFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("comentariofechafin")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentariofechafin"));
                            objeto.MotivoFechaIndefinida = reader.IsDBNull(reader.GetOrdinal("motivofechaindefinida")) ? string.Empty : reader.GetString(reader.GetOrdinal("motivofechaindefinida"));
                            objeto.urlFechaIndefinida = reader.IsDBNull(reader.GetOrdinal("motivourl")) ? string.Empty : reader.GetString(reader.GetOrdinal("motivourl"));
                            objeto.FechaInterna = reader.IsDBNull(reader.GetOrdinal("fechainterna")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechainterna"));
                            objeto.tipofechacalculo = reader.IsDBNull(reader.GetOrdinal("tipofechacalculo")) ? -1 : reader.GetInt32(reader.GetOrdinal("tipofechacalculo"));
                            objeto.Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Categoria"));
                            objeto.Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo"));
                            objeto.Item = reader.IsDBNull(reader.GetOrdinal("Item")) ? string.Empty : reader.GetString(reader.GetOrdinal("Item"));
                            objeto.FlagTemporal = reader.IsDBNull(reader.GetOrdinal("flagtemporal")) ? false : reader.GetBoolean(reader.GetOrdinal("flagtemporal"));
                        }
                    }
                    cnx.Close();
                    return objeto;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override void EditarModelo(ModeloDTO modDTO)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Editar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@id", modDTO.idmodelo));
                        comando.Parameters.Add(new SqlParameter("@nombre", modDTO.nombre));
                        comando.Parameters.Add(new SqlParameter("@fabricante", modDTO.fabricante));
                        comando.Parameters.Add(new SqlParameter("@flagfechafinsoporte", modDTO.FlagFechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@fechafinsorporteextendida", modDTO.FechaFinSoporteExtendida));
                        comando.Parameters.Add(new SqlParameter("@fechafinsoporte", modDTO.FechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@fechainterna", modDTO.FechaInterna));
                        comando.Parameters.Add(new SqlParameter("@tipofechainterna", modDTO.TipoFechaInterna));
                        comando.Parameters.Add(new SqlParameter("@comentraiofechafinsoporte", modDTO.ComentarioFechaFinSoporte));
                        comando.Parameters.Add(new SqlParameter("@motivofechaindefinida", modDTO.MotivoFechaIndefinida));
                        comando.Parameters.Add(new SqlParameter("@urlfechaindefinida", modDTO.urlFechaIndefinida));
                        comando.Parameters.Add(new SqlParameter("@usuario", modDTO.Usuario));
                        comando.Parameters.Add(new SqlParameter("@fechafabricacion", modDTO.FechaFabricacion));
                        comando.Parameters.Add(new SqlParameter("@tipoequipo", modDTO.tipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@urlSharepoint", modDTO.UrlSharepoint));
                        comando.Parameters.Add(new SqlParameter("@urlLineamientos", modDTO.UrlLineamiento));
                        comando.Parameters.Add(new SqlParameter("@Remedy", modDTO.Remedy));
                        comando.Parameters.Add(new SqlParameter("@RemedyNombre", modDTO.RemedyNombre));

                        comando.Parameters.Add(new SqlParameter("@Categoria", modDTO.Categoria));
                        comando.Parameters.Add(new SqlParameter("@Tipo", modDTO.Tipo));
                        comando.Parameters.Add(new SqlParameter("@Item", modDTO.Item));
                        comando.Parameters.Add(new SqlParameter("@TipoFechaCalculo", modDTO.tipofechacalculo));

                        comando.ExecuteNonQuery();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<ModeloDTO> ExportarModelo(string criterio, int tipo, string nroSerie, string hostName)
        {
            try
            {
                var lista = new List<ModeloDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Exportar]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@criterio", string.IsNullOrEmpty(criterio) ? string.Empty : criterio));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@nroSerie", string.IsNullOrEmpty(nroSerie) ? string.Empty : nroSerie));
                        comando.Parameters.Add(new SqlParameter("@hostName", string.IsNullOrEmpty(hostName) ? string.Empty : hostName));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ModeloDTO()
                            {
                                idmodelo = reader.IsDBNull(reader.GetOrdinal("ModeloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModeloId")),
                                fabricante = reader.IsDBNull(reader.GetOrdinal("fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("fabricante")),
                                tipoEquipoNombre = reader.IsDBNull(reader.GetOrdinal("tipoequipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipoequipo")),
                                nombre = reader.IsDBNull(reader.GetOrdinal("modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modelo")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("flagactivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("flagactivo")),
                                Usuario = reader.IsDBNull(reader.GetOrdinal("usuariocreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("usuariocreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("fechacreacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechacreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("usuariomodificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("usuariomodificacion")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("fechamodificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechamodificacion")),
                                capacidades = reader.IsDBNull(reader.GetOrdinal("capacidades")) ? string.Empty : reader.GetString(reader.GetOrdinal("capacidades")),
                                FechaFabricacion = reader.IsDBNull(reader.GetOrdinal("fechafabricacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafabricacion")),
                                FechaFinSoporteExtendida = reader.IsDBNull(reader.GetOrdinal("fechafinsoporteextendida")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafinsoporteextendida")),
                                FechaInterna = reader.IsDBNull(reader.GetOrdinal("fechainterna")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechainterna")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("fechafinsoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechafinsoporte")),
                                tipofechacalculo = reader.IsDBNull(reader.GetOrdinal("tipofechacalculo")) ? 0 : reader.GetInt32(reader.GetOrdinal("tipofechacalculo")),
                                TipoFechaInterna = reader.IsDBNull(reader.GetOrdinal("tipointerna")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipointerna")),
                                FlagFinSoporte = reader.IsDBNull(reader.GetOrdinal("flagfinsoporte")) ? string.Empty : reader.GetString(reader.GetOrdinal("flagfinsoporte")),
                                FlagTemporalToString = reader.IsDBNull(reader.GetOrdinal("flagtemporal")) ? string.Empty : reader.GetString(reader.GetOrdinal("flagtemporal")),
                                ComentarioFechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("comentariofechafin")) ? string.Empty : reader.GetString(reader.GetOrdinal("comentariofechafin")),
                                MotivoFechaIndefinida = reader.IsDBNull(reader.GetOrdinal("motivofechaindefinida")) ? string.Empty : reader.GetString(reader.GetOrdinal("motivofechaindefinida")),
                                urlFechaIndefinida = reader.IsDBNull(reader.GetOrdinal("motivourl")) ? string.Empty : reader.GetString(reader.GetOrdinal("motivourl")),
                                UrlSharepoint = reader.IsDBNull(reader.GetOrdinal("urlSharepoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("urlSharepoint")),
                                UrlLineamiento = reader.IsDBNull(reader.GetOrdinal("urlLineamientos")) ? string.Empty : reader.GetString(reader.GetOrdinal("urlLineamientos")),
                                RemedyNombre = reader.IsDBNull(reader.GetOrdinal("RemedyNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("RemedyNombre")),
                                indicador = reader.IsDBNull(reader.GetOrdinal("indicador")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicador")),
                                indicadorproyectado = reader.IsDBNull(reader.GetOrdinal("indicadorproyectado")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicadorproyectado")),
                                indicadorproyectado2 = reader.IsDBNull(reader.GetOrdinal("indicadorproyectado2")) ? 0 : reader.GetInt32(reader.GetOrdinal("indicadorproyectado2")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                FechaUltimoEscaneoCorrecto = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                EstadoEquipo = reader.IsDBNull(reader.GetOrdinal("EstadoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoEquipo")),
                                NumeroSerie = reader.IsDBNull(reader.GetOrdinal("NumeroSerie")) ? string.Empty : reader.GetString(reader.GetOrdinal("NumeroSerie")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("OwnerNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerNombre")),
                                OwnerContacto = reader.IsDBNull(reader.GetOrdinal("OwnerContacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerContacto")),
                                FechaDespliegue = reader.IsDBNull(reader.GetOrdinal("FechaDespliegue")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDespliegue")),
                                FechaDisponible = reader.IsDBNull(reader.GetOrdinal("FechaDisponible")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDisponible")),
                                Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Categoria")),
                                Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                Item = reader.IsDBNull(reader.GetOrdinal("Item")) ? string.Empty : reader.GetString(reader.GetOrdinal("Item")),
                                Equipos = new EquipoDTO()
                                {
                                    AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? string.Empty : reader.GetString(reader.GetOrdinal("AssetId")),
                                    TagNumber = reader.IsDBNull(reader.GetOrdinal("TagNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("TagNumber")),
                                    SystemRole = reader.IsDBNull(reader.GetOrdinal("SystemRole")) ? string.Empty : reader.GetString(reader.GetOrdinal("SystemRole")),
                                    PartNumber = reader.IsDBNull(reader.GetOrdinal("PartNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("PartNumber")),
                                    Urgency = reader.IsDBNull(reader.GetOrdinal("Urgency")) ? string.Empty : reader.GetString(reader.GetOrdinal("Urgency")),
                                    SystemEnvironment = reader.IsDBNull(reader.GetOrdinal("SystemEnvironment")) ? string.Empty : reader.GetString(reader.GetOrdinal("SystemEnvironment")),
                                    VersionNumber = reader.IsDBNull(reader.GetOrdinal("VersionNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("VersionNumber")),
                                    Building = reader.IsDBNull(reader.GetOrdinal("Building")) ? string.Empty : reader.GetString(reader.GetOrdinal("Building")),
                                    Room = reader.IsDBNull(reader.GetOrdinal("Room")) ? string.Empty : reader.GetString(reader.GetOrdinal("Room")),
                                    EnvironmentSpecification = reader.IsDBNull(reader.GetOrdinal("EnvironmentSpecification")) ? string.Empty : reader.GetString(reader.GetOrdinal("EnvironmentSpecification")),
                                    ConfigurationOptions = reader.IsDBNull(reader.GetOrdinal("ConfigurationOptions")) ? string.Empty : reader.GetString(reader.GetOrdinal("ConfigurationOptions")),
                                    Expansion = reader.IsDBNull(reader.GetOrdinal("Expansion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Expansion")),
                                    ExpansionInterface = reader.IsDBNull(reader.GetOrdinal("ExpansionInterface")) ? string.Empty : reader.GetString(reader.GetOrdinal("ExpansionInterface")),
                                    IsVirtual = reader.IsDBNull(reader.GetOrdinal("IsVirtual")) ? string.Empty : reader.GetString(reader.GetOrdinal("IsVirtual")),
                                    CreateDate = reader.IsDBNull(reader.GetOrdinal("CreateDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreateDate")),
                                    DisposalDate = reader.IsDBNull(reader.GetOrdinal("DisposalDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("DisposalDate")),
                                    ContractId = reader.IsDBNull(reader.GetOrdinal("ContractId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContractId")),
                                    ContractDesc = reader.IsDBNull(reader.GetOrdinal("ContractDesc")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContractDesc")),
                                    CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? string.Empty : reader.GetString(reader.GetOrdinal("CustomerID")),
                                    ContractStartDate = reader.IsDBNull(reader.GetOrdinal("ContractStartDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContractStartDate")),
                                    ContractExpDate = reader.IsDBNull(reader.GetOrdinal("ContractExpDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContractExpDate")),
                                    AssignedSupportCompany = reader.IsDBNull(reader.GetOrdinal("AssignedSupportCompany")) ? string.Empty : reader.GetString(reader.GetOrdinal("AssignedSupportCompany")),
                                    AssignedSupportOrganization = reader.IsDBNull(reader.GetOrdinal("AssignedSupportOrganization")) ? string.Empty : reader.GetString(reader.GetOrdinal("AssignedSupportOrganization")),
                                    NotificationGroup = reader.IsDBNull(reader.GetOrdinal("NotificationGroup")) ? string.Empty : reader.GetString(reader.GetOrdinal("NotificationGroup")),
                                    Domain = reader.IsDBNull(reader.GetOrdinal("Domain")) ? string.Empty : reader.GetString(reader.GetOrdinal("Domain")),
                                    PrimaryCapability = reader.IsDBNull(reader.GetOrdinal("PrimaryCapability")) ? string.Empty : reader.GetString(reader.GetOrdinal("PrimaryCapability")),
                                }
                            };
                            lista.Add(objeto);
                        }
                    }
                    cnx.Close();
                }

                return lista;
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: ActualizarEstadoTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<EquipoDTO> ObtenerEquipos(int modelo, string fabricante, int tipoId, string nombre, string nroSerie, string hostName, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerModelosEquipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@modelo", modelo));
                        comando.Parameters.Add(new SqlParameter("@fabricante", fabricante));
                        comando.Parameters.Add(new SqlParameter("@tipoId", tipoId));
                        comando.Parameters.Add(new SqlParameter("@codigo", nombre));
                        comando.Parameters.Add(new SqlParameter("@nroSerie", nroSerie));
                        comando.Parameters.Add(new SqlParameter("@hostName", hostName));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Id = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                FlagTemporal = reader.IsDBNull(reader.GetOrdinal("FlagTemporal")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagTemporal")),
                                Ubicacion = reader.IsDBNull(reader.GetOrdinal("Ubicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ubicacion")),
                                FechaUltimoEscaneoCorrectoStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoCorrecto")),
                                FechaUltimoEscaneoErrorStr = reader.IsDBNull(reader.GetOrdinal("FechaUltimoEscaneoError")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaUltimoEscaneoError")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                Activo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                Ambiente = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CaracteristicaEquipo = reader.IsDBNull(reader.GetOrdinal("CaracteristicaEquipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CaracteristicaEquipo")),
                                Subsidiaria = reader.IsDBNull(reader.GetOrdinal("Subsidiaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subsidiaria")),
                                Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Modelo")),
                                CodigoEquipo = reader.IsDBNull(reader.GetOrdinal("CodigoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoEquipo")),
                                NumeroSerie = reader.IsDBNull(reader.GetOrdinal("NumeroSerie")) ? string.Empty : reader.GetString(reader.GetOrdinal("NumeroSerie")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("OwnerNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerNombre")),
                                OwnerContacto = reader.IsDBNull(reader.GetOrdinal("OwnerContacto")) ? string.Empty : reader.GetString(reader.GetOrdinal("OwnerContacto")),
                                Domain = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0)
                        totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override void ActualizarCodigo(int id, string codigo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var configuracion = (from u in ctx.EquipoConfiguracion
                                         where u.EquipoId == id && u.Componente == "CODIGO_UNICO"
                                         && u.FlagActivo
                                         select u).FirstOrDefault();
                    if (configuracion == null)
                    {
                        var entidad = new EquipoConfiguracion()
                        {
                            FlagActivo = true,
                            FechaCreacion = DateTime.Now,
                            EquipoId = id,
                            Componente = "CODIGO_UNICO",
                            DetalleComponente = codigo,
                            CreadoPor = "auto"
                        };
                        ctx.EquipoConfiguracion.Add(entidad);

                        ctx.SaveChanges();
                    }
                    else
                    {
                        configuracion.DetalleComponente = codigo;
                        configuracion.FechaModificacion = DateTime.Now;
                        configuracion.ModificadoPor = "auto";
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: int AddOrEditTecnologia(TecnologiaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> ModeloHardware_Combo()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ModeloHardware_Combo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                
                                Id = (reader.IsDBNull(reader.GetOrdinal("ModeloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ModeloId"))).ToString(),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                                TipoDescripcion = reader.IsDBNull(reader.GetOrdinal("fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("fabricante"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> ModeloHardware_Combo()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> FabricanteHardware_Combo()
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<CustomAutocomplete>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_FabricanteHardware_Combo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocomplete()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fabricante"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> ModeloHardware_Combo()"
                    , new object[] { null });
            }
        }
    }
}
