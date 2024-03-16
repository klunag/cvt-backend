using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.Services.ModelDB;
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
    public class ApplicationMgtSvc : ApplicationMgtDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override ApplicationIdMgtDTO GetApplicationById(string applicationId)
        {
            try
            {
                var dataList = new List<ApplicationIdMgtDTO>();
                ApplicationIdMgtDTO response = null;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetApplicationById]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@applicationId", applicationId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApplicationIdMgtDTO()
                            {
                                applicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                applicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                                interfaceId = reader.IsDBNull(reader.GetOrdinal("interfaceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("interfaceId")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                assetType = reader.IsDBNull(reader.GetOrdinal("assetType")) ? string.Empty : reader.GetString(reader.GetOrdinal("assetType")),
                                centralGerence = reader.IsDBNull(reader.GetOrdinal("centralGerence")) ? string.Empty : reader.GetString(reader.GetOrdinal("centralGerence")),
                                division = reader.IsDBNull(reader.GetOrdinal("division")) ? string.Empty : reader.GetString(reader.GetOrdinal("division")),
                                area = reader.IsDBNull(reader.GetOrdinal("area")) ? string.Empty : reader.GetString(reader.GetOrdinal("area")),
                                unit = reader.IsDBNull(reader.GetOrdinal("unit")) ? string.Empty : reader.GetString(reader.GetOrdinal("unit")),
                                BIANarea = reader.IsDBNull(reader.GetOrdinal("BIANarea")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANarea")),
                                BIANdomain = reader.IsDBNull(reader.GetOrdinal("BIANdomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANdomain")),
                                mainOffice = reader.IsDBNull(reader.GetOrdinal("mainOffice")) ? string.Empty : reader.GetString(reader.GetOrdinal("mainOffice")),
                                technologyCategory = reader.IsDBNull(reader.GetOrdinal("technologyCategory")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyCategory")),
                                technicalClassification = reader.IsDBNull(reader.GetOrdinal("technicalClassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalClassification")),
                                technicalSubclassification = reader.IsDBNull(reader.GetOrdinal("technicalSubclassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalSubclassification")),
                                managed = reader.IsDBNull(reader.GetOrdinal("managed")) ? string.Empty : reader.GetString(reader.GetOrdinal("managed"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (dataList.Count > 0) response = dataList[0];
                    if (response != null) response.portfolioManager = GetPortfolioManagerByApplicationId(applicationId);

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

        private ApplicationStateDTO GetObsolescenceAppById(string applicationId, DateTime date)
        {
            try
            {
                var dataList = new List<ApplicationStateDTO>();
                ApplicationStateDTO response = null;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_AplicacionEstado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoApt", applicationId));
                        comando.Parameters.Add(new SqlParameter("@fecha", date));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApplicationStateDTO()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("Estado")).ToString()
                            };

                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (dataList.Count > 0) response = dataList[0];

                    return response;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationStateDTO GetObsolescenceAppById(string applicationId, DateTime date)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationStateDTO GetObsolescenceAppById(string applicationId, DateTime date)"
                    , new object[] { null });
            }
        }

        public override ApplicationObsMgtDTO GetApplicationObsolescenceById(string applicationId, DateTime date)
        {
            try
            {
                ApplicationObsMgtDTO response = null;
                var INIT_DATE = date;
                var FIRST_PERIOD_DATE = date.AddMonths(12);
                var SECOND_PERIOD_DATE = date.AddMonths(24);

                var current_obs = GetObsolescenceAppById(applicationId, INIT_DATE);
                var firstPeriod_obs = GetObsolescenceAppById(applicationId, FIRST_PERIOD_DATE);
                var secondPeriod_obs = GetObsolescenceAppById(applicationId, SECOND_PERIOD_DATE);

                response = new ApplicationObsMgtDTO(applicationId,
                    date.ToString("dd/MM/yyyy"), 
                    current_obs.Estado, 
                    firstPeriod_obs.Estado, 
                    secondPeriod_obs.Estado);

                return response;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationObsMgtDTO GetApplicationObsolescenceById(string applicationId, DateTime date)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ApplicationObsMgtDTO GetApplicationObsolescenceById(string applicationId, DateTime date)"
                    , new object[] { null });
            }
        }

        public override List<ApplicationRelationsMgtDTO> GetApplicationRelationsById(ApplicationRelationsPag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<ApplicationRelationsMgtDTO>();
                if (pag == null) return dataList;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetApplicationRelations]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@consultedDate", pag.date));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@status", Utilitarios.FullStr(pag.status)));
                        comando.Parameters.Add(new SqlParameter("@type", Utilitarios.FullStr(pag.type)));
                        comando.Parameters.Add(new SqlParameter("@applicationId", Utilitarios.FullStr(pag.applicationId)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApplicationRelationsMgtDTO()
                            {
                                applicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                relationType = reader.IsDBNull(reader.GetOrdinal("relationType")) ? string.Empty : reader.GetString(reader.GetOrdinal("relationType")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                itResourceId = reader.IsDBNull(reader.GetOrdinal("itResourceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("itResourceId")),
                                technology = reader.IsDBNull(reader.GetOrdinal("technology")) ? string.Empty : reader.GetString(reader.GetOrdinal("technology")),
                                relavance = reader.IsDBNull(reader.GetOrdinal("relavance")) ? string.Empty : reader.GetString(reader.GetOrdinal("relavance")),
                                isResourceOwner = reader.IsDBNull(reader.GetOrdinal("isResourceOwner")) ? (bool?)null : bool.Parse(reader.GetString(reader.GetOrdinal("isResourceOwner"))),
                                component = reader.IsDBNull(reader.GetOrdinal("component")) ? string.Empty : reader.GetString(reader.GetOrdinal("component")),
                                dateCreation = reader.IsDBNull(reader.GetOrdinal("dateCreation")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateCreation")),
                                createdBy = reader.IsDBNull(reader.GetOrdinal("createdBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("createdBy")),
                                dateModification = reader.IsDBNull(reader.GetOrdinal("dateModification")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateModification")),
                                modifiedBy = reader.IsDBNull(reader.GetOrdinal("modifiedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("modifiedBy"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }
                    //if (dataList.Count > 0)
                    //    totalRows = dataList[0].TotalFilas;

                    return dataList;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<ApplicationRelationsMgtDTO> GetApplicationRelationsById(ApplicationRelationsPag pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ApplicationMgtDTO> GetApplications(ApplicationPag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<ApplicationMgtDTO>();
                if (pag == null) return dataList;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetApplications]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencia", Utilitarios.FullStr(pag.gerence)));
                        comando.Parameters.Add(new SqlParameter("@division", Utilitarios.FullStr(pag.division)));
                        comando.Parameters.Add(new SqlParameter("@area", Utilitarios.FullStr(pag.area)));
                        comando.Parameters.Add(new SqlParameter("@unidad", Utilitarios.FullStr(pag.unit)));
                        comando.Parameters.Add(new SqlParameter("@estado", Utilitarios.FullStr(pag.status)));
                        comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", Utilitarios.FullStr(pag.technicalClassification)));
                        comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", Utilitarios.FullStr(pag.technicalSubClassification)));
                        comando.Parameters.Add(new SqlParameter("@applicationId", Utilitarios.FullStr(pag.applicationId)));
                        comando.Parameters.Add(new SqlParameter("@owner", Utilitarios.FullStr(pag.owner)));
                        comando.Parameters.Add(new SqlParameter("@tipoActivo", Utilitarios.FullStr(pag.assetType)));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new ApplicationMgtDTO()
                            {
                                applicationId = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                                applicationName = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                                description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                                implementationType = reader.IsDBNull(reader.GetOrdinal("implementationType")) ? string.Empty : reader.GetString(reader.GetOrdinal("implementationType")),
                                managed = reader.IsDBNull(reader.GetOrdinal("managed")) ? string.Empty : reader.GetString(reader.GetOrdinal("managed")),
                                deploymentType = reader.IsDBNull(reader.GetOrdinal("deploymentType")) ? string.Empty : reader.GetString(reader.GetOrdinal("deploymentType")),
                                parentAPTCode = reader.IsDBNull(reader.GetOrdinal("parentAPTCode")) ? string.Empty : reader.GetString(reader.GetOrdinal("parentAPTCode")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                interfaceId = reader.IsDBNull(reader.GetOrdinal("interfaceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("interfaceId")),
                                registerDate = reader.IsDBNull(reader.GetOrdinal("registerDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("registerDate")),
                                unit = reader.IsDBNull(reader.GetOrdinal("unit")) ? string.Empty : reader.GetString(reader.GetOrdinal("unit")),
                                teamName = reader.IsDBNull(reader.GetOrdinal("teamName")) ? string.Empty : reader.GetString(reader.GetOrdinal("teamName")),
                                expert = reader.IsDBNull(reader.GetOrdinal("expert")) ? string.Empty : reader.GetString(reader.GetOrdinal("expert")),
                                userEntity = reader.IsDBNull(reader.GetOrdinal("userEntity")) ? string.Empty : reader.GetString(reader.GetOrdinal("userEntity")),
                                developmentType = reader.IsDBNull(reader.GetOrdinal("developmentType")) ? string.Empty : reader.GetString(reader.GetOrdinal("developmentType")),
                                developmentProvider = reader.IsDBNull(reader.GetOrdinal("developmentProvider")) ? string.Empty : reader.GetString(reader.GetOrdinal("developmentProvider")),
                                infrastructure = reader.IsDBNull(reader.GetOrdinal("infrastructure")) ? string.Empty : reader.GetString(reader.GetOrdinal("infrastructure")),
                                replacementApplication = reader.IsDBNull(reader.GetOrdinal("replacementApplication")) ? string.Empty : reader.GetString(reader.GetOrdinal("replacementApplication")),
                                assetType = reader.IsDBNull(reader.GetOrdinal("assetType")) ? string.Empty : reader.GetString(reader.GetOrdinal("assetType")),
                                BIANarea = reader.IsDBNull(reader.GetOrdinal("BIANarea")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANarea")),
                                BIANdomain = reader.IsDBNull(reader.GetOrdinal("BIANdomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANdomain")),
                                mainOffice = reader.IsDBNull(reader.GetOrdinal("mainOffice")) ? string.Empty : reader.GetString(reader.GetOrdinal("mainOffice")),
                                technologyCategory = reader.IsDBNull(reader.GetOrdinal("technologyCategory")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyCategory")),
                                technicalClassification = reader.IsDBNull(reader.GetOrdinal("technicalClassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalClassification")),
                                technicalSubclassification = reader.IsDBNull(reader.GetOrdinal("technicalSubclassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalSubclassification")),
                                authorizingUser = reader.IsDBNull(reader.GetOrdinal("authorizingUser")) ? string.Empty : reader.GetString(reader.GetOrdinal("authorizingUser")),
                                centralGerence = reader.IsDBNull(reader.GetOrdinal("centralGerence")) ? string.Empty : reader.GetString(reader.GetOrdinal("centralGerence")),
                                division = reader.IsDBNull(reader.GetOrdinal("division")) ? string.Empty : reader.GetString(reader.GetOrdinal("division")),
                                area = reader.IsDBNull(reader.GetOrdinal("area")) ? string.Empty : reader.GetString(reader.GetOrdinal("area")),
                                systemBroker = reader.IsDBNull(reader.GetOrdinal("systemBroker")) ? string.Empty : reader.GetString(reader.GetOrdinal("systemBroker")),
                                tribeLead = reader.IsDBNull(reader.GetOrdinal("tribeLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("tribeLead")),
                                tribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("tribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("tribeTechnicalLead")),
                                teamLeader = reader.IsDBNull(reader.GetOrdinal("teamLeader")) ? string.Empty : reader.GetString(reader.GetOrdinal("teamLeader")),
                                owner = reader.IsDBNull(reader.GetOrdinal("owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("owner")),
                                groupTicketRemedy = reader.IsDBNull(reader.GetOrdinal("groupTicketRemedy")) ? string.Empty : reader.GetString(reader.GetOrdinal("groupTicketRemedy")),
                                webDomain = reader.IsDBNull(reader.GetOrdinal("webDomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("webDomain")),
                                dateFirstRelease = reader.IsDBNull(reader.GetOrdinal("dateFirstRelease")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateFirstRelease")),
                                starProduct = reader.IsDBNull(reader.GetOrdinal("starProduct")) ? string.Empty : reader.GetString(reader.GetOrdinal("starProduct")),
                                shorterApplicationResponseTime = reader.IsDBNull(reader.GetOrdinal("shorterApplicationResponseTime")) ? string.Empty : reader.GetString(reader.GetOrdinal("shorterApplicationResponseTime")),
                                highestDegreeInterruption = reader.IsDBNull(reader.GetOrdinal("highestDegreeInterruption")) ? string.Empty : reader.GetString(reader.GetOrdinal("highestDegreeInterruption")),
                                applicationCriticalityBIA = reader.IsDBNull(reader.GetOrdinal("applicationCriticalityBIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationCriticalityBIA")).ToString(),
                                classification = reader.IsDBNull(reader.GetOrdinal("classification")) ? string.Empty : reader.GetString(reader.GetOrdinal("classification")).ToString(),
                                finalCriticality = reader.IsDBNull(reader.GetOrdinal("finalCriticality")) ? string.Empty : reader.GetString(reader.GetOrdinal("finalCriticality")).ToString(),
                                tobe = reader.IsDBNull(reader.GetOrdinal("tobe")) ? string.Empty : reader.GetString(reader.GetOrdinal("tobe")).ToString(),
                                tierPreProduction = reader.IsDBNull(reader.GetOrdinal("tierPreProduction")) ? string.Empty : reader.GetString(reader.GetOrdinal("tierPreProduction")),
                                tierProduction = reader.IsDBNull(reader.GetOrdinal("tierProduction")) ? string.Empty : reader.GetString(reader.GetOrdinal("tierProduction")),
                                registrationSituation = reader.IsDBNull(reader.GetOrdinal("registrationSituation")) ? string.Empty : reader.GetString(reader.GetOrdinal("registrationSituation")),
                                isFormalApplication = true,
                                regularizationDate = reader.IsDBNull(reader.GetOrdinal("regularizationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("regularizationDate")),
                                authorizationMethod = reader.IsDBNull(reader.GetOrdinal("authorizationMethod")) ? string.Empty : reader.GetString(reader.GetOrdinal("authorizationMethod")),
                                authenticationMethod = reader.IsDBNull(reader.GetOrdinal("authenticationMethod")) ? string.Empty : reader.GetString(reader.GetOrdinal("authenticationMethod"))
                            };
                            dataList.Add(objeto);
                        }
                        reader.Close();
                    }
                    //if (dataList.Count > 0)
                    //    totalRows = dataList[0].TotalFilas;

                    return dataList;
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

        public override List<PortfolioManagerDTO> GetPortfolioManagerByApplicationId(string applicationId)
        {
            try
            {
                var dataList = new List<PortfolioManagerDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetPortfolioManagerByApplicationId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@applicationId", applicationId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new PortfolioManagerDTO()
                            {
                                employeeId = reader.IsDBNull(reader.GetOrdinal("employeeId")) ? string.Empty : reader.GetString(reader.GetOrdinal("employeeId")),
                                fullName = reader.IsDBNull(reader.GetOrdinal("fullName")) ? string.Empty : reader.GetString(reader.GetOrdinal("fullName")),
                                roleId = reader.IsDBNull(reader.GetOrdinal("roleId")) ? 0 : reader.GetInt32(reader.GetOrdinal("roleId")),
                                role = reader.IsDBNull(reader.GetOrdinal("role")) ? string.Empty : reader.GetString(reader.GetOrdinal("role")),
                                email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString(reader.GetOrdinal("email"))
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
    }
}
