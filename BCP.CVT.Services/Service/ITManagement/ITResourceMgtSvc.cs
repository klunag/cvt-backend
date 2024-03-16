using BCP.CVT.Cross;
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
    public class ITResourceMgtSvc : ITResourceMgtDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override ItResourceIdMgtDTO GetItResourceById(string itResourceId)
        {
            try
            {
                var dataList = new List<ItResourceIdMgtDTO>();
                ItResourceIdMgtDTO response = null;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetItResourceById]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@itResourceId", itResourceId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItResourceIdMgtDTO()
                            {
                                itResourceId = reader.IsDBNull(reader.GetOrdinal("itResourceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("itResourceId")),
                                type = reader.IsDBNull(reader.GetOrdinal("type")) ? string.Empty : reader.GetString(reader.GetOrdinal("type")),
                                discovery = reader.IsDBNull(reader.GetOrdinal("discovery")) ? string.Empty : reader.GetString(reader.GetOrdinal("discovery")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                enviroment = reader.IsDBNull(reader.GetOrdinal("enviroment")) ? string.Empty : reader.GetString(reader.GetOrdinal("enviroment")),
                                domain = reader.IsDBNull(reader.GetOrdinal("domain")) ? string.Empty : reader.GetString(reader.GetOrdinal("domain")),
                                operatingSystem = reader.IsDBNull(reader.GetOrdinal("operatingSystem")) ? string.Empty : reader.GetString(reader.GetOrdinal("operatingSystem")),
                                registerDate = reader.IsDBNull(reader.GetOrdinal("registerDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("registerDate"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (dataList.Count > 0) response = dataList[0];

                    if (response != null)
                    {
                        response.relatedApplications = GetRelatedApplicationsByItResourceById(itResourceId);
                        response.technologies = GetTechnologiesByItResourceById(itResourceId);
                    }

                    return response;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationIdMgtDTO GetApplicationById(string applicationId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationIdMgtDTO GetApplicationById(string applicationId)"
                    , new object[] { null });
            }
        }

        public override List<ItResourceRelationsMgtDTO> GetItResourceRelations(ItResourceRelationsPag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<ItResourceRelationsMgtDTO>();
                var dateCreated = pag.dateCreatedBy.HasValue ? pag.dateCreatedBy.Value.Date.ToString() : string.Empty;
                var dateModified = pag.dateModifiedBy.HasValue ? pag.dateModifiedBy.Value.Date.ToString() : string.Empty;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetItResourceRelations]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@consultedDate", pag.date));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@status", Utilitarios.FullStr(pag.status)));
                        comando.Parameters.Add(new SqlParameter("@type", Utilitarios.FullStr(pag.type)));
                        comando.Parameters.Add(new SqlParameter("@applicationId", Utilitarios.FullStr(pag.applicationId)));
                        comando.Parameters.Add(new SqlParameter("@itResourceId", Utilitarios.FullStr(pag.itResourceId)));
                        comando.Parameters.Add(new SqlParameter("@dateCreatedBy", dateCreated));
                        comando.Parameters.Add(new SqlParameter("@dateModifiedBy", dateModified));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItResourceRelationsMgtDTO()
                            {
                                applicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                relationType = reader.IsDBNull(reader.GetOrdinal("relationType")) ? string.Empty : reader.GetString(reader.GetOrdinal("relationType")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                itResourceId = reader.IsDBNull(reader.GetOrdinal("itResourceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("itResourceId")),
                                itResourceType = reader.IsDBNull(reader.GetOrdinal("itResourceType")) ? string.Empty : reader.GetString(reader.GetOrdinal("itResourceType")),
                                technology = reader.IsDBNull(reader.GetOrdinal("technology")) ? string.Empty : reader.GetString(reader.GetOrdinal("technology")),
                                relavance = reader.IsDBNull(reader.GetOrdinal("relavance")) ? string.Empty : reader.GetString(reader.GetOrdinal("relavance")),
                                isResourceOwner = reader.IsDBNull(reader.GetOrdinal("isResourceOwner")) ? (bool?)null : bool.Parse(reader.GetString(reader.GetOrdinal("isResourceOwner"))),
                                component = reader.IsDBNull(reader.GetOrdinal("component")) ? string.Empty : reader.GetString(reader.GetOrdinal("component")),
                                dateCreation = reader.IsDBNull(reader.GetOrdinal("dateCreation")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateCreation")),
                                createdBy = reader.IsDBNull(reader.GetOrdinal("createdBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("createdBy")),
                                dateModification = reader.IsDBNull(reader.GetOrdinal("dateModification")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateModification")),
                                modifiedBy = reader.IsDBNull(reader.GetOrdinal("modifiedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("modifiedBy")),
                                domain = reader.IsDBNull(reader.GetOrdinal("domain")) ? string.Empty : reader.GetString(reader.GetOrdinal("domain"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<ItResourceRelationsMgtDTO> GetItResourceRelations(ItResourceRelationsPag pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ItResourceMgtDTO> GetItResources(ItResourcePag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<ItResourceMgtDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetItResources]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@type", Utilitarios.FullStr(pag.types)));
                        comando.Parameters.Add(new SqlParameter("@status", Utilitarios.FullStr(pag.status)));
                        comando.Parameters.Add(new SqlParameter("@discovery", Utilitarios.FullStr(pag.discovery)));
                        comando.Parameters.Add(new SqlParameter("@operatingSystem", Utilitarios.FullStr(pag.operatingSystem)));
                        comando.Parameters.Add(new SqlParameter("@domain", Utilitarios.FullStr(pag.domain)));
                        comando.Parameters.Add(new SqlParameter("@itResourceId", Utilitarios.FullStr(pag.itResourceId)));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pag.pageSize));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ItResourceMgtDTO()
                            {
                                itResourceId = reader.IsDBNull(reader.GetOrdinal("itResourceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("itResourceId")),
                                type = reader.IsDBNull(reader.GetOrdinal("type")) ? string.Empty : reader.GetString(reader.GetOrdinal("type")),
                                discovery = reader.IsDBNull(reader.GetOrdinal("discovery")) ? string.Empty : reader.GetString(reader.GetOrdinal("discovery")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                enviroment = reader.IsDBNull(reader.GetOrdinal("enviroment")) ? string.Empty : reader.GetString(reader.GetOrdinal("enviroment")),
                                domain = reader.IsDBNull(reader.GetOrdinal("domain")) ? string.Empty : reader.GetString(reader.GetOrdinal("domain")),
                                operatingSystem = reader.IsDBNull(reader.GetOrdinal("operatingSystem")) ? string.Empty : reader.GetString(reader.GetOrdinal("operatingSystem")),
                                registerDate = reader.IsDBNull(reader.GetOrdinal("registerDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("registerDate")),
                                ipAddress = reader.IsDBNull(reader.GetOrdinal("ipAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("ipAddress"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<ItResourceMgtDTO> GetItResources(ItResourcePag pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<RelatedApplicationDTO> GetRelatedApplicationsByItResourceById(string itResourceId)
        {
            try
            {
                var dataList = new List<RelatedApplicationDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetRelatedApplicationsByItResourceId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@itResourceId", itResourceId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelatedApplicationDTO()
                            {
                                applicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                applicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                enviroment = reader.IsDBNull(reader.GetOrdinal("enviroment")) ? string.Empty : reader.GetString(reader.GetOrdinal("enviroment")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status"))
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
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationIdMgtDTO GetApplicationById(string applicationId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationIdMgtDTO GetApplicationById(string applicationId)"
                    , new object[] { null });
            }
        }

        public override List<TechnologyResourceDTO> GetTechnologiesByItResourceById(string itResourceId)
        {
            try
            {
                var dataList = new List<TechnologyResourceDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetTechnologiesByItResourceId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@itResourceId", itResourceId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TechnologyResourceDTO()
                            {
                                technologyDomain = reader.IsDBNull(reader.GetOrdinal("technologyDomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyDomain")),
                                technologySubdomain = reader.IsDBNull(reader.GetOrdinal("technologySubdomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologySubdomain")),
                                technologyFullName = reader.IsDBNull(reader.GetOrdinal("technologyFullName")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyFullName")),
                                technologyType = reader.IsDBNull(reader.GetOrdinal("technologyType")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyType")),
                                endOfSupportTechnology = reader.IsDBNull(reader.GetOrdinal("endOfSupportTechnology")) ? string.Empty : reader.GetString(reader.GetOrdinal("endOfSupportTechnology")),
                                currentObsolescence = reader.IsDBNull(reader.GetOrdinal("currentObsolescence")) ? string.Empty : reader.GetString(reader.GetOrdinal("currentObsolescence"))
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
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TechnologyResourceDTO> GetTechnologiesByItResourceById(string itResourceId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TechnologyResourceDTO> GetTechnologiesByItResourceById(string itResourceId)"
                    , new object[] { null });
            }
        }
    }
}
