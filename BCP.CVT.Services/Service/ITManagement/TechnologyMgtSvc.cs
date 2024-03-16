using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.ITManagement;
using BCP.CVT.Services.ModelDB;
using BCP.CVT.Services.SQL;
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
    public class TechnologyMgtSvc : TechnologyMgtDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ResponseTechnologyPost PrepararXMLandSendToSP(List<TechnologyPostDTO> ListObjRegistro)
        {
            DataSet ds = new DataSet("TECNOLOGIAS");
            DataTable tbl = new DataTable("TECNOLOGIA");

            //Nombre en la BD
            tbl.Columns.Add("technologyId", typeof(int));
            tbl.Columns.Add("vendor", typeof(string));
            tbl.Columns.Add("name", typeof(string));
            tbl.Columns.Add("version", typeof(string));
            tbl.Columns.Add("description", typeof(string));            
            tbl.Columns.Add("technologyTypeId", typeof(string));
            tbl.Columns.Add("nameTechnologyType", typeof(string));
            tbl.Columns.Add("hasEndDateSupport", typeof(bool));
            tbl.Columns.Add("endDateSupportSource", typeof(int));

            tbl.Columns.Add("typeEndDateSupport", typeof(int)); 
            tbl.Columns.Add("endDateSupport", typeof(string));
            tbl.Columns.Add("useCase", typeof(string));
            
            tbl.Columns.Add("confluenceUrl", typeof(string));
            tbl.Columns.Add("complianceSecurityArchitect", typeof(string));
            tbl.Columns.Add("complianceTechnologyArchitect", typeof(string));
            tbl.Columns.Add("remedySupportGroup", typeof(string));
            tbl.Columns.Add("managementTeam", typeof(string));

            tbl.Columns.Add("shownStandarsSite", typeof(bool));
            tbl.Columns.Add("codeStatus", typeof(int));
            tbl.Columns.Add("descriptionStatus", typeof(string));
            
            
            tbl.Columns.Add("existenceValueParam", typeof(int));
            tbl.Columns.Add("usabilityValueParam", typeof(int));
            tbl.Columns.Add("riskValueParam", typeof(int));
            tbl.Columns.Add("vulnerabilityValueParam", typeof(int));

            tbl.Columns.Add("productId", typeof(int));
            tbl.Columns.Add("reference", typeof(string));
            tbl.Columns.Add("sourceId", typeof(int));
            tbl.Columns.Add("hasEquivalence", typeof(bool));
            tbl.Columns.Add("productCode", typeof(string));
            tbl.Columns.Add("implementationScript", typeof(int));

            tbl.Columns.Add("internalDateType", typeof(string));
            tbl.Columns.Add("undefinedReasonId", typeof(string));
            tbl.Columns.Add("urlUndefinedDate", typeof(string));
            tbl.Columns.Add("applyTo", typeof(string));
            tbl.Columns.Add("technologyReview", typeof(int));
            tbl.Columns.Add("compatibilitySO", typeof(string));
            tbl.Columns.Add("compatibilityCloud", typeof(string));
            tbl.Columns.Add("licenseScheme", typeof(int));
            tbl.Columns.Add("endDateComments", typeof(string));
            tbl.Columns.Add("monitoringScheme", typeof(string));

            tbl.Columns.Add("userModification", typeof(string));

            ds.Tables.Add(tbl);

            if (ListObjRegistro.Count > 0)
            {
                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["TECNOLOGIA"].NewRow();
                    dr["technologyId"] = item.technologyId;
                    dr["vendor"] = string.IsNullOrWhiteSpace(item.vendor) ? string.Empty : item.vendor.Trim();
                    dr["name"] = string.IsNullOrWhiteSpace(item.name) ? string.Empty : item.name.Trim();
                    dr["version"] = string.IsNullOrWhiteSpace(item.version) ? string.Empty : item.version.Trim();
                    dr["description"] = string.IsNullOrWhiteSpace(item.description) ? string.Empty : item.description.Trim();                    
                    dr["technologyTypeId"] = item.type == null || string.IsNullOrWhiteSpace(item.type.technologyTypeId) ? string.Empty : item.type.technologyTypeId.Trim();
                    dr["nameTechnologyType"] = item.type == null || string.IsNullOrWhiteSpace(item.type.name) ? string.Empty : item.type.name.Trim();
                    dr["hasEndDateSupport"] = item.hasEndDateSupport ?? false ;
                    dr["endDateSupportSource"] = item.endDateSupportSource ?? 0 ; //TODO-reglas
                    dr["typeEndDateSupport"] = item.typeEndDateSupport ?? 0 ; //TODO-reglas
                    dr["endDateSupport"] = string.IsNullOrWhiteSpace(item.endDateSupport) ? string.Empty : item.endDateSupport.Trim();
                    dr["useCase"] = string.IsNullOrWhiteSpace(item.useCase) ? string.Empty : item.useCase.Trim();                    
                    dr["confluenceUrl"] = string.IsNullOrWhiteSpace(item.confluenceUrl) ? string.Empty : item.confluenceUrl.Trim();
                    dr["complianceSecurityArchitect"] = string.IsNullOrWhiteSpace(item.complianceSecurityArchitect) ? string.Empty : item.complianceSecurityArchitect.Trim();
                    dr["complianceTechnologyArchitect"] = string.IsNullOrWhiteSpace(item.complianceTechnologyArchitect) ? string.Empty : item.complianceTechnologyArchitect.Trim();
                    dr["remedySupportGroup"] = string.IsNullOrWhiteSpace(item.remedySupportGroup) ? string.Empty : item.remedySupportGroup.Trim();
                    dr["managementTeam"] = string.IsNullOrWhiteSpace(item.managementTeam) ? string.Empty : item.managementTeam.Trim();
                    dr["shownStandarsSite"] = item.shownStandarsSite ?? false;
                    dr["codeStatus"] = 1;
                    dr["descriptionStatus"] = string.Empty;
                    dr["existenceValueParam"] = item.existenceValueParam ?? 1 ;
                    dr["usabilityValueParam"] = item.usabilityValueParam ?? 1 ;
                    dr["riskValueParam"] = item.riskValueParam ?? 1 ;
                    dr["vulnerabilityValueParam"] = item.vulnerabilityValueParam ?? 1 ;

                    dr["productId"] = item.productId ?? -1;
                    dr["reference"] = string.IsNullOrWhiteSpace(item.reference) ? string.Empty : item.reference.Trim();
                    dr["sourceId"] = item.sourceId ?? -1;
                    dr["hasEquivalence"] = item.hasEquivalence ?? false;
                    dr["productCode"] = string.IsNullOrWhiteSpace(item.productCode) ? string.Empty : item.productCode.Trim();
                    dr["implementationScript"] = item.implementationScript ?? -1;

                    dr["internalDateType"] = string.IsNullOrWhiteSpace(item.internalDateType) ? string.Empty : item.internalDateType.Trim();
                    dr["undefinedReasonId"] = string.IsNullOrWhiteSpace(item.undefinedReasonId) ? string.Empty : item.undefinedReasonId.Trim(); 
                    dr["urlUndefinedDate"] = string.IsNullOrWhiteSpace(item.urlUndefinedDate) ? string.Empty : item.urlUndefinedDate.Trim(); 
                    dr["applyTo"] = string.IsNullOrWhiteSpace(item.applyTo) ? string.Empty : item.applyTo.Trim(); 
                    dr["technologyReview"] = item.technologyReview;
                    dr["compatibilitySO"] = string.IsNullOrWhiteSpace(item.compatibilitySO) ? string.Empty : item.compatibilitySO.Trim(); 
                    dr["compatibilityCloud"] = string.IsNullOrWhiteSpace(item.compatibilityCloud) ? string.Empty : item.compatibilityCloud.Trim(); 
                    dr["licenseScheme"] = item.licenseScheme ?? 1;
                    dr["endDateComments"] = string.IsNullOrWhiteSpace(item.endDateComments) ? string.Empty : item.endDateComments.Trim(); 
                    dr["monitoringScheme"] = string.IsNullOrWhiteSpace(item.monitoringScheme) ? string.Empty : item.monitoringScheme.Trim();

                    dr["userModification"] = string.IsNullOrWhiteSpace(item.userModification) ? string.Empty : item.userModification.Trim();

                    ds.Tables["TECNOLOGIA"].Rows.Add(dr);
                }
            }

            string xmlStr = ds.GetXml();
            List<SQLParam> listSQLParam = new List<SQLParam>();
            listSQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
            DataTable data = new SQLManager().GetDataTable("[app].[USP_InsertTechnology]", listSQLParam);
            var responseMessage = data.Rows[0][0].ToString();
            var responseCode = data.Rows[0][1].ToString();

            return new ResponseTechnologyPost(responseCode, responseMessage);
        }

        public override ResponseTechnologyPost AddTechnology(TechnologyPostDTO objDTO)
        {
            try
            {
                var dataSource = new List<TechnologyPostDTO>();
                dataSource.Add(objDTO);
                var response = PrepararXMLandSendToSP(dataSource);

                return response;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: AddTechnology(TechnologyPostDTO objDTO)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: AddTechnology(TechnologyPostDTO objDTO)"
                    , new object[] { null });
            }
        }

        public override List<DomainTechnologyDTO> GetDomainTechnologies()
        {
            try
            {
                var dataList = new List<DomainTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetDomainTechnologies]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        //comando.Parameters.Add(new SqlParameter("@itResourceId", itResourceId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new DomainTechnologyDTO()
                            {
                                domainId = reader.IsDBNull(reader.GetOrdinal("domainId")) ? 0 : reader.GetInt32(reader.GetOrdinal("domainId")),
                                name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
                                description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description"))
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
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
        }

        public override List<SubdomainTechnologyDTO> GetSubdomainsByDomainTechnologyId(int domainId)
        {
            try
            {
                var dataList = new List<SubdomainTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetSubdomainsByDomainTechnologyId]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@domainId", domainId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new SubdomainTechnologyDTO()
                            {
                                subDomainId = reader.IsDBNull(reader.GetOrdinal("subDomainId")) ? 0 : reader.GetInt32(reader.GetOrdinal("subDomainId")),
                                domainId = reader.IsDBNull(reader.GetOrdinal("domainId")) ? 0 : reader.GetInt32(reader.GetOrdinal("domainId")),
                                name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
                                description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description"))
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
                    , "Error en el metodo: List<SubdomainTechnologyDTO> GetSubdomainsByDomainTechnologyId(int domainId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<SubdomainTechnologyDTO> GetSubdomainsByDomainTechnologyId(int domainId)"
                    , new object[] { null });
            }
        }

        public override List<TechnologyDTO> GetTechnologies(TechnologyPag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var totalRegistros = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var dataList = new List<TechnologyDTO>();

                var statusId = -1;
                var status = string.IsNullOrEmpty(pag.status) ? -1 : int.TryParse(pag.status, out statusId) ? statusId : statusId;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetTechnologies]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@domain", Utilitarios.FullStr(pag.domain)));
                        comando.Parameters.Add(new SqlParameter("@subDomain", Utilitarios.FullStr(pag.subDomain)));
                        comando.Parameters.Add(new SqlParameter("@status", status));
                        comando.Parameters.Add(new SqlParameter("@family", Utilitarios.FullStr(pag.family)));
                        comando.Parameters.Add(new SqlParameter("@type", Utilitarios.FullStr(pag.type)));
                        comando.Parameters.Add(new SqlParameter("@technologyObsolescence", Utilitarios.FullStr(pag.technologyObsolescence)));
                        comando.Parameters.Add(new SqlParameter("@name", Utilitarios.FullStr(pag.name)));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@sortName", Utilitarios.FullStr(pag.sortName)));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", Utilitarios.FullStr(pag.sortOrder)));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            totalRegistros = reader.IsDBNull(reader.GetOrdinal("TotalRows")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalRows"));

                            var objeto = new TechnologyDTO()
                            {
                                tecnologyId = reader.IsDBNull(reader.GetOrdinal("tecnologyId")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("tecnologyId")).ToString(),
                                endDateSupport = reader.IsDBNull(reader.GetOrdinal("endDateSupport")) ? string.Empty : reader.GetString(reader.GetOrdinal("endDateSupport")),
                                extendedDate = reader.IsDBNull(reader.GetOrdinal("extendedDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("extendedDate")),
                                agreementDate = reader.IsDBNull(reader.GetOrdinal("agreementDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("agreementDate")),
                                hasEndDateSupport = reader.IsDBNull(reader.GetOrdinal("hasEndDateSupport")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("hasEndDateSupport")),
                                sourceId = reader.IsDBNull(reader.GetOrdinal("sourceId")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("sourceId")).ToString(),
                                technologyObsolescence = reader.IsDBNull(reader.GetOrdinal("technologyObsolescence")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyObsolescence")),
                                status = new StatusDTO()
                                {
                                    code = reader.IsDBNull(reader.GetOrdinal("statusId")) ? 0 : reader.GetInt32(reader.GetOrdinal("statusId")),
                                    description = reader.IsDBNull(reader.GetOrdinal("statusDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("statusDescription"))
                                },
                                name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
                                type = new TypeTechnologyDTO()
                                {
                                    technologyTypeId = reader.IsDBNull(reader.GetOrdinal("typeId")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("typeId")).ToString(),
                                    name = reader.IsDBNull(reader.GetOrdinal("typeDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("typeDescription"))
                                },
                                subDomain = reader.IsDBNull(reader.GetOrdinal("subDomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("subDomain")),
                                domain = reader.IsDBNull(reader.GetOrdinal("domain")) ? string.Empty : reader.GetString(reader.GetOrdinal("domain")),
                                family = reader.IsDBNull(reader.GetOrdinal("family")) ? string.Empty : reader.GetString(reader.GetOrdinal("family")),
                                endDateSet = reader.IsDBNull(reader.GetOrdinal("endDateSet")) ? string.Empty : reader.GetString(reader.GetOrdinal("endDateSet")),
                                roadmap = reader.IsDBNull(reader.GetOrdinal("roadmap")) ? string.Empty : reader.GetString(reader.GetOrdinal("roadmap")),
                                tribuCoeId = reader.IsDBNull(reader.GetOrdinal("tribuCoeId")) ? string.Empty : reader.GetString(reader.GetOrdinal("tribuCoeId")),
                                tribuCoeName = reader.IsDBNull(reader.GetOrdinal("tribuCoeName")) ? string.Empty : reader.GetString(reader.GetOrdinal("tribuCoeName")),
                                squadId = reader.IsDBNull(reader.GetOrdinal("squadId")) ? string.Empty : reader.GetString(reader.GetOrdinal("squadId")),
                                squadName = reader.IsDBNull(reader.GetOrdinal("squadName")) ? string.Empty : reader.GetString(reader.GetOrdinal("squadName")),
                                ownerId = reader.IsDBNull(reader.GetOrdinal("ownerId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ownerId")),
                                ownerName = reader.IsDBNull(reader.GetOrdinal("ownerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ownerName")),
                                technologyStatusId = reader.IsDBNull(reader.GetOrdinal("technologyStatusId")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("technologyStatusId")).ToString(),
                            };


                            dataList.Add(objeto);
                        }

                        if (dataList.Count > 0)
                            totalRows = totalRegistros;

                        reader.Close();
                    }

                    return dataList;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<TechnologyDTO> GetTechnologies(TechnologyPag pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TypeTechnologyDTO> GetTypeTechnologies()
        {
            try
            {
                var dataList = new List<TypeTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[app].[USP_GetTypeTechnologies]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        //comando.Parameters.Add(new SqlParameter("@itResourceId", itResourceId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TypeTechnologyDTO()
                            {
                                technologyTypeId = reader.IsDBNull(reader.GetOrdinal("technologyTypeId")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("technologyTypeId")).ToString(),
                                name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name"))
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
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
        }

        public override List<ReasonDTO> GetReasons()
        {
            try
            {
                var dataList = new List<DomainTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Motivo
                                    where u.FlagActivo                                    
                                    select new ReasonDTO
                                    {
                                        reasonId = u.MotivoId,
                                        name = u.Nombre
                                    }).ToList();

                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
        }

        public override List<ProductDTO> GetProducts(ProductPag pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                var dataList = new List<DomainTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Producto
                                    join u2 in ctx.Subdominio on u.SubDominioId equals u2.SubdominioId
                                    join u3 in ctx.Dominio on u.DominioId equals u3.DominioId
                                    join u4 in ctx.TipoCicloVida on u.TipoCicloVidaId equals u4.TipoCicloVidaId into tcv
                                    from x in tcv.DefaultIfEmpty()
                                    //join u5 in ctx.Tipo on u.TipoProductoId equals u5.TipoId into tp
                                    //from y in tp.DefaultIfEmpty()
                                    where u.FlagActivo && (u.Codigo == pag.productCode || string.IsNullOrEmpty(pag.productCode))
                                    orderby u.Fabricante ascending, u.Nombre ascending
                                    select new ProductDTO
                                    {
                                        description = u.Descripcion,
                                        developmentCompany = u.Fabricante,
                                        name = u.Nombre,
                                        productId = u.ProductoId,
                                        productCode = u.Codigo,
                                        groupTicketRemedy = u.GrupoTicketRemedyNombre,
                                        ownerName = u.OwnerDisplayName,
                                        unitOrganizational = u.TribuCoeDisplayName,
                                        subunitOrganizational = u.SquadDisplayName,
                                        domain = u3.Nombre,
                                        subdomain = u2.Nombre,
                                        lifeCycle = x.Nombre,
                                        //productType = y.Nombre
                                    });

                    totalRows = registro.Count();
                    var resultado = registro.Skip((pag.pageNumber.Value - 1) * pag.pageSize.Value).Take(pag.pageSize.Value);
                    return resultado.ToList();                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<ProductDTO> GetProducts(ProductPag pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<ProductDTO> GetProducts(ProductPag pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ProductDTO GetProductById(string productCode)
        {
            try
            {                
                var dataList = new List<DomainTechnologyDTO>();
                var cadenaConexion = Constantes.CadenaConexion;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registro = (from u in ctx.Producto
                                    join u2 in ctx.Subdominio on u.SubDominioId equals u2.SubdominioId
                                    join u3 in ctx.Dominio on u.DominioId equals u3.DominioId
                                    join u4 in ctx.TipoCicloVida on u.TipoCicloVidaId equals u4.TipoCicloVidaId into tcv
                                    from x in tcv.DefaultIfEmpty()
                                    //join u5 in ctx.Tipo on u.TipoProductoId equals u5.TipoId into tp
                                    //from y in tp.DefaultIfEmpty()
                                    where u.Codigo.ToUpper() == productCode.ToUpper()
                                    select new ProductDTO
                                    {
                                        description = u.Descripcion,
                                        developmentCompany = u.Fabricante,
                                        name = u.Nombre,
                                        productId = u.ProductoId,
                                        productCode = u.Codigo,
                                        groupTicketRemedy = u.GrupoTicketRemedyNombre,
                                        ownerName = u.OwnerDisplayName,
                                        unitOrganizational = u.TribuCoeDisplayName,
                                        subunitOrganizational = u.SquadDisplayName,
                                        domain = u3.Nombre,
                                        subdomain = u2.Nombre,
                                        lifeCycle = x.Nombre,
                                        //productType = y.Nombre
                                    }).FirstOrDefault();
                    
                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProductDTO GetProductById(string productCode)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: ProductDTO GetProductById(string productCode)"
                    , new object[] { null });
            }
        }

        public override List<UndefinedReasonDTO> GetUndefinedReasons()
        {
            try
            {
                var dataList = new List<UndefinedReasonDTO>();
                var paramSustentoMotivo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_SUSTENTO_MOTIVO");
                var listSustentoMotivo = (paramSustentoMotivo.Valor ?? "").Split('|').ToList();
                var retorno = (from u in listSustentoMotivo
                               select new UndefinedReasonDTO() { 
                                   name = u.Trim(),
                                   undefinedReasonId = u.Trim()
                               }).ToList();

                return retorno;

            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TypeTechnologyDTO> GetTypeTechnologies()"
                    , new object[] { null });
            }
        }
    }
}
