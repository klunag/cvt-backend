using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface.ITManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service.ITManagement
{
    public class SigaMgtSvc : SigaMgtDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override EmployeeSigaMgtDTO GetEmployeeSigaMgtDTOByMatricula(string codMatricula)
        {
            try
            {
                EmployeeSigaMgtDTO item = null;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHEMPLEADOS_BUSCAR_EMPLEADO_SIGA_POR_MATRICULA]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codMatricula", codMatricula));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        if (reader.Read())
                        {
                            item = new EmployeeSigaMgtDTO();
                            
                            item.samAccountName = reader.GetData<string>("samAccountName");
                            item.name = reader.GetData<string>("name");
                            item.lastName = reader.GetData<string>("lastName");                            
                            item.organization = reader.GetData<string>("organization");
                            item.jobTitleCode = reader.GetData<string>("jobTitleCode");
                            item.jobTitle = reader.GetData<string>("jobTitle");
                            item.unitOrganizationCode = reader.GetData<string>("unitOrganizationCode");
                            item.unitOrganization = reader.GetData<string>("unitOrganization");
                            item.serviceCode = reader.GetData<string>("serviceCode");
                            item.service = reader.GetData<string>("service");
                            item.areaCode = reader.GetData<string>("areaCode");
                            item.area = reader.GetData<string>("area");
                            item.divisionCode = reader.GetData<string>("divisionCode");
                            item.division = reader.GetData<string>("division");
                            item.samAccountNameManager = reader.GetData<string>("samAccountNameManager");
                            item.nameManager = reader.GetData<string>("nameManager");
                            string dateRegistrationStr = reader.GetData<string>("dateRegistration");
                            item.dateRegistration = DateTime.ParseExact(dateRegistrationStr, "yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("es-PE"));
                            string dateLastModificationStr = reader.GetData<string>("dateLastModification");
                            item.dateLastModification = DateTime.ParseExact(dateLastModificationStr, "yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("es-PE"));
                            string dateUnsubscribeStr = reader.GetData<string>("dateUnsubscribe");
                            item.dateUnsubscribe = dateUnsubscribeStr == new String('0', dateUnsubscribeStr.Length) ? null : (DateTime?)DateTime.ParseExact(dateUnsubscribeStr, "yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("es-PE"));
                        }
                        reader.Close();
                    }

                    return item;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: EmployeeSigaMgtDTO GetEmployeeSigaMgtDTOByMatricula(string codMatricula)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: EmployeeSigaMgtDTO GetEmployeeSigaMgtDTOByMatricula(string codMatricula)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeProductoPorFiltro(string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_TRIBUCOE_PRODUCTO_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@filtro", filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                //Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                //Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                //value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadProductoPorFiltro(string codigoUnidad, string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_SQUAD_PRODUCTO_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codUnidad", codigoUnidad));
                        comando.Parameters.Add(new SqlParameter("@filtro", string.IsNullOrEmpty(filtro) ? DBNull.Value : (object)filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeTecnologiaPorFiltro(string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_TRIBUCOE_TECNOLOGIA_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@filtro", filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                //Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                //Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                //value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadTecnologiaPorFiltro(string codigoUnidad, string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_SQUAD_TECNOLOGIA_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codUnidad", codigoUnidad));
                        comando.Parameters.Add(new SqlParameter("@filtro", string.IsNullOrEmpty(filtro) ? DBNull.Value : (object)filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadTribuCoeTecnologiaKPIPorFiltro(string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_TRIBUCOE_TECNOLOGIAKPI_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@filtro", filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                //Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                //Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                //value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutocomplete> BuscarUnidadTribuCoePorFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadTecnologiaKPIPorFiltro(string codigoUnidad, string filtro)
        {
            try
            {
                var dataList = new List<CustomAutoCompleteBCPUnidad>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_BCPCATGGHUNIDADES_BUSCAR_SQUAD_TECNOLOGIAKPI_POR_FILTRO]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codUnidad", codigoUnidad));
                        comando.Parameters.Add(new SqlParameter("@filtro", string.IsNullOrEmpty(filtro) ? DBNull.Value : (object)filtro));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutoCompleteBCPUnidad()
                            {
                                Id = reader.GetValueString(reader.GetOrdinal("COD_UNIDAD")),
                                Descripcion = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD")),
                                CodigoPersonalResponsable = reader.GetValueString(reader.GetOrdinal("COD_PERSONAL_RESPONSABLE")),
                                NombresPersonalResponsable = reader.GetValueString(reader.GetOrdinal("NOMBRES_PERSONAL_RESPONSABLE")),
                                MatriculaPersonalResponsable = reader.GetValueString(reader.GetOrdinal("MATRICULA_PERSONAL_RESPONSABLE")),
                                value = reader.GetValueString(reader.GetOrdinal("DES_UNIDAD"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorBCPUnidades
                    , "Error en el metodo: List<CustomAutoCompleteBCPUnidad> BuscarUnidadSquadPorFiltro(string codigoUnidad, string filtro)"
                    , new object[] { null });
            }
        }
    }
}
