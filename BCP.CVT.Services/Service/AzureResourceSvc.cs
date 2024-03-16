using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Data;

namespace BCP.CVT.Services.Service
{
    public class AzureResourceSvc : AzureResourceDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<AzureResourcesDTO> GetAllAzureResources(PaginationAzure pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lstResponse = new List<AzureResourcesDTO>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_T4_ListarAzureResources_Por_Fecha]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@fechaConsulta", pag.DateFilter));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            var objeto = new AzureResourcesDTO()
                            {
                                SuscriptionId = reader.IsDBNull(reader.GetOrdinal("SuscriptionId")) ? string.Empty : reader.GetString(reader.GetOrdinal("SuscriptionId")),
                                SuscriptionDisplayName = reader.IsDBNull(reader.GetOrdinal("SuscriptionDisplayName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SuscriptionDisplayName")),
                                SuscriptionState = reader.IsDBNull(reader.GetOrdinal("SuscriptionState")) ? string.Empty : reader.GetString(reader.GetOrdinal("SuscriptionState")),
                                ResourcesName = reader.IsDBNull(reader.GetOrdinal("ResourcesName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourcesName")),
                                ResourcesType = reader.IsDBNull(reader.GetOrdinal("ResourcesType")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourcesType")),
                                ResourcesLocation = reader.IsDBNull(reader.GetOrdinal("ResourcesLocation")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourcesLocation")),
                                ResourcesState = reader.IsDBNull(reader.GetOrdinal("ResourcesState")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourcesState")),
                                ResourceGroup = reader.IsDBNull(reader.GetOrdinal("ResourceGroup")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourceGroup")),
                                ResourcesSO = reader.IsDBNull(reader.GetOrdinal("ResourcesSO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourcesSO")),
                                VMId = reader.IsDBNull(reader.GetOrdinal("VMId")) ? string.Empty : reader.GetString(reader.GetOrdinal("VMId")),
                                AKSVersion = reader.IsDBNull(reader.GetOrdinal("AKSVersion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AKSVersion")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Equivalence = reader.IsDBNull(reader.GetOrdinal("Equivalence")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equivalence")),
                                Hostnames = reader.IsDBNull(reader.GetOrdinal("Hostnames")) ? string.Empty : reader.GetString(reader.GetOrdinal("Hostnames")),
                                Day = reader.IsDBNull(reader.GetOrdinal("Day")) ? 0 : reader.GetInt32(reader.GetOrdinal("Day")),
                                Month = reader.IsDBNull(reader.GetOrdinal("Month")) ? 0 : reader.GetInt32(reader.GetOrdinal("Month")),
                                Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : reader.GetInt32(reader.GetOrdinal("Year")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                ClaveTecnologia = reader.IsDBNull(reader.GetOrdinal("ClaveTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClaveTecnologia")),
                                Analizar = reader.IsDBNull(reader.GetOrdinal("Analizar")) ? string.Empty : reader.GetString(reader.GetOrdinal("Analizar")),
                                TipoEquipo = reader.IsDBNull(reader.GetOrdinal("TipoEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEquipo")),
                                Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observaciones"))
                            };

                            totalRows = reader.GetInt32(reader.GetOrdinal("TotalFilas"));
                            lstResponse.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lstResponse;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AzureResourceTypeDto> GetAzureTypes(Paginacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AzureResourceType
                                     where (u.Name.ToUpper().Contains(pag.nombre.ToUpper())
                                     || string.IsNullOrEmpty(pag.nombre))
                                     orderby u.Name
                                     select new AzureResourceTypeDto()
                                     {
                                         ResourceTypeId = u.ResourceTypeId,
                                         Name = u.Name,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         CreationDate = u.CreationDate,
                                         CreatedBy = u.CreatedBy,
                                         ModificationDate = u.ModificationDate,
                                         ModifiedBy = u.ModifiedBy,
                                         IsVirtualMachine = u.IsVirtualMachine
                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder);
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: List<AzureResourceTypeDto> GetAzureTypes()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: List<AzureResourceTypeDto> GetAzureTypes()"
                    , new object[] { null });
            }
        }        

        public override void UpdateActiveStatus(int resourceId, string user)
        {
            try
            {                
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var row = ctx.AzureResourceType.FirstOrDefault(x => x.ResourceTypeId == resourceId);
                    if (row != null)
                    {
                        row.IsActive = !row.IsActive;
                        row.ModificationDate = DateTime.Now;
                        row.ModifiedBy = user;

                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: void UpdateActiveStatus()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: void UpdateActiveStatus()"
                    , new object[] { null });
            }
        }

        public override bool UpdateVirtualMachineStatus(int resourceId, string user)
        {
            try
            {
                int rowsAffected = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var row = ctx.AzureResourceType.FirstOrDefault(x => x.ResourceTypeId == resourceId);
                    if (row != null)
                    {
                        row.IsVirtualMachine = row.IsVirtualMachine.HasValue ? !row.IsVirtualMachine.Value : true;
                        row.ModificationDate = DateTime.Now;
                        row.ModifiedBy = user;

                        rowsAffected = ctx.SaveChanges();
                    }

                    return rowsAffected > 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: void UpdateActiveStatus()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAzureDTO
                    , "Error en el metodo: void UpdateActiveStatus()"
                    , new object[] { null });
            }
        }
    }
}
