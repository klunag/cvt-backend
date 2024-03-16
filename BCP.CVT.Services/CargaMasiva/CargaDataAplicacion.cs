using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.PAPP.Common.Custom;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;


namespace BCP.CVT.Services.CargaMasiva
{
    public class CargaDataAplicacion
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string CREADOPOR = string.Empty;
        private string MODIFICADO_POR = string.Empty;
        private List<string> ERROR_INSERT_EQUIPO = new List<string>();
        private List<string> ERROR_INSERT_TNR = new List<string>();
        private string _LOG_ERRORES = string.Empty;
        //private string _CadenaConexionBDLocal = ConfigurationManager.ConnectionStrings["GestionCMDB_ProdEntities"].ConnectionString;
        //private string _CadenaConexionBDLocal = "metadata=res://*/ModelDB.ModelCVT.csdl|res://*/ModelDB.ModelCVT.ssdl|res://*/ModelDB.ModelCVT.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=tcp:mssteue195p01.database.windows.net,1433;initial catalog=CVT_Prod;persist security info=True;user id=UserCVT;password=L1m@2020;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        private string _CadenaConexionBDLocal = string.Empty;





        public CargaDataAplicacion()
        {
            _CadenaConexionBDLocal = Constantes.CadenaConexion;
            //_CadenaConexionBDLocal = "data source=tcp:mssteue195p01.database.windows.net,1433;initial catalog=CVT_Prod;user id=UserCVT;password=L1m@2020;MultipleActiveResultSets=True;max pool size=200;connect timeout=0;";
        }


        #region PRIVATE_METHODS
        private IExcelDataReader ControlarExtension(string ext, Stream stream)
        {
            //if (ext.ToLower().Equals(Utilitarios.EXTENSION_EXCEL))
            //{
            //    return ExcelReaderFactory.CreateOpenXmlReader(stream);
            //}
            //else
            //{
            //    return ExcelReaderFactory.CreateBinaryReader(stream, true);
            //}
            if (ext.ToLower().Equals(Utilitarios.EXTENSION_EXCEL))
            {
                return ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
                return null;
        }

        private SqlConnection ObtenerConexionLocal()
        {
            SqlConnection conxBDCMDB;
            conxBDCMDB = new SqlConnection(_CadenaConexionBDLocal);
            conxBDCMDB.Open();
            return conxBDCMDB;
        }

        private void TruncarTabla(string tabla)
        {
            using (var cnx = ObtenerConexionLocal())
            {
                using (var cmd = new SqlCommand(string.Format("truncate table {0};", tabla), cnx))
                {
                    cmd.ExecuteNonQuery();
                }
                cnx.Close();
            }
        }
        #endregion

        private T ObtenerRegistroEntidad<T>(List<string> detalleFilaExcel, int TipoCargaMasiva, int? fila, out string errorFila)
        {
            errorFila = string.Empty;


            T objEntidad = default(T);

            switch ((ETipoCargaMasiva)TipoCargaMasiva)
            {

                case ETipoCargaMasiva.UpdateAplicacion:
                    objEntidad = (T)(object)ObtenerEntidadAplicacionUpdate(detalleFilaExcel, true);
                    errorFila = string.Format(errorFila, fila);
                    break;
                case ETipoCargaMasiva.UpdateAplicacionHistorico:
                    objEntidad = (T)(object)ObtenerEntidadAplicacionUpdate2(detalleFilaExcel, true);
                    errorFila = string.Format(errorFila, fila);
                    break;
            }

            return objEntidad;
        }

        private ApplicationDetail ObtenerEntidadAplicacionUpdate(List<string> detalleFilaExcel, bool estado)
        {
            ApplicationDetail entidad = new ApplicationDetail();
            entidad.ValidarExcel = estado;
            DateTime FECHA_FIN = DateTime.Now;
            bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (FLAGCONTINUAR)
                {
                    switch (i)
                    {


                        //case 0: { entidad.applicationId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 1: { entidad.applicationName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        //case 2: { entidad.description = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 3: { entidad.managedName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 4: { entidad.implementationTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 5: { entidad.deploymentTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 6: { entidad.statusName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 7: { entidad.parentAPTCode = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 8: { entidad.interfaceId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 9: { entidad.ArquitectoNegocio = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////27 Gerencia central
                        //case 10: { entidad.GerenciaCentral = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////28 Division
                        //case 11: { entidad.DivisionDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////29 Area
                        //case 12: { entidad.AreaDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 13: { entidad.unitName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 14: { entidad.teamName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////12 Experto/Líder técnico
                        //case 15: { entidad.Experto = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 16: { entidad.userEntityName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 17: { entidad.developmentTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 18: { entidad.developmentProvider = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 19: { entidad.replacementApplication = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 20: { entidad.infrastructureName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 21: { entidad.authenticationMethodName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 22: { entidad.authorizationMethodName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 23: { entidad.grupoTicketRemedyName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 24: { entidad.webDomain = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 25: { entidad.tipoActivoName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 26: { entidad.areaBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 27: { entidad.dominioBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 28: { entidad.jefaturaATIName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //20
                        //case 29: { entidad.TOBEName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 30: { entidad.categoriaTecnologicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        //case 31: { entidad.clasificacionTecnicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok  
                        //case 32: { entidad.subClasificacionTecnicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        //case 33: { entidad.tierProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 34: { entidad.tierPreProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////34 Lider Usuario
                        //case 35: { entidad.LiderUsuarioDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////26 Usuario autorizador/Gestor
                        //case 36: { entidad.UsuarioAutorizador = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////31 Tribe Lead
                        //case 37: { entidad.TribeLead = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////32 Tribe Technical Lead
                        //case 38: { entidad.TribeTechnicalLead = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////33 Jefe de Equipo
                        //case 39: { entidad.JefeEquipo = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////30 Broker de Sistemas
                        //case 40: { entidad.BrokerSistemas = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 41: { entidad.starProduct = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 42: { entidad.MenorRTOName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 43: { entidad.MayorGradoInterrupcionName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //40
                        //case 44: { entidad.criticidadBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 45: { entidad.clasificacionActivoName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 46: { entidad.nuevaCriticidadName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;


                        ////48 Flag Pirata
                        //case 47: { entidad.FlagPirata = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////49 Fecha (Flag Pirata)
                        //case 48: { entidad.FechaFlagPirata = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////47 Situación de registro
                        //case 49: { entidad.SituacionRegistro = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 50: { entidad.registerDate = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;


                        //case 51: { entidad.dateFirstRelease = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;

                        //case 52: { entidad.summaryStandard = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //case 53: { entidad.complianceLevelName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        ////54 Solicitante
                        //case 54: { entidad.SolicitanteDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;



                        case 0: { entidad.applicationId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1: { entidad.applicationName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 2: { entidad.interfaceId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 3: { entidad.description = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 4: { entidad.statusName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 5: { entidad.implementationTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 6: { entidad.tipoActivoName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 7: { entidad.GerenciaCentral = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //28 Division
                        case 8: { entidad.DivisionDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //29 Area
                        case 9: { entidad.AreaDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 10: { entidad.unitName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //34 Lider Usuario
                        case 11: { entidad.LiderUsuarioDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //26 Usuario autorizador/Gestor
                        case 12: { entidad.UsuarioAutorizador = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        case 13: { entidad.managedName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 14: { entidad.teamName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //31 Tribe Lead
                        case 15: { entidad.TribeLead = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //32 Tribe Technical Lead
                        case 16: { entidad.TribeTechnicalLead = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //33 Jefe de Equipo
                        case 17: { entidad.JefeEquipo = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //30 Broker de Sistemas
                        case 18: { entidad.BrokerSistemas = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //12 Experto/Líder técnico
                        case 19: { entidad.Experto = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 20: { entidad.areaBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 21: { entidad.dominioBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 22: { entidad.TOBEName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 23: { entidad.jefaturaATIName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //20
                        case 24: { entidad.ArquitectoNegocio = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 25: { entidad.userEntityName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 26: { entidad.criticidadBIANName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 27: { entidad.clasificacionActivoName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 28: { entidad.nuevaCriticidadName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 29: { entidad.starProduct = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 30: { entidad.MenorRTOName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 31: { entidad.MayorGradoInterrupcionName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //40
                        case 32: { entidad.tipoPCI = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //40
                        case 33: { entidad.tierPreProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 34: { entidad.tierProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 35: { entidad.deploymentTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 36: { entidad.categoriaTecnologicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 37: { entidad.webDomain = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 38: { entidad.clasificacionTecnicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok  
                        case 39: { entidad.subClasificacionTecnicaName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 40: { entidad.developmentTypeName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 41: { entidad.developmentProvider = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 42: { entidad.infrastructureName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 43: { entidad.authenticationMethodName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 44: { entidad.authorizationMethodName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 45: { entidad.grupoTicketRemedyName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 46: { entidad.summaryStandard = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 47: { entidad.complianceLevelName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        //48 Flag Pirata
                        case 48: { entidad.FlagPirata = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //49 Fecha (Flag Pirata)
                        case 49: { entidad.FechaFlagPirata = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 50: { entidad.parentAPTCode = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        case 51: { entidad.replacementApplication = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        //54 Solicitante
                        case 52: { entidad.SolicitanteDetail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 53: { entidad.registerDate = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;



                        //47 Situación de registro
                        case 54: { entidad.SituacionRegistro = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;


                        case 55: { entidad.dateFirstRelease = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;

                    }
                }
            }
            //entidad.Activo = true;
            //entidad.UsuarioCreacion = MODIFICADO_POR;
            //entidad.UsuarioModificacion = MODIFICADO_POR;
            if (!FLAGCONTINUAR) entidad = null;

            return entidad;
        }
        private ApplicationHistoricoDetail ObtenerEntidadAplicacionUpdate2(List<string> detalleFilaExcel, bool estado)
        {
            ApplicationHistoricoDetail entidad = new ApplicationHistoricoDetail();
            entidad.ValidarExcel = estado;
            DateTime FECHA_FIN = DateTime.Now;
            bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (FLAGCONTINUAR)
                {
                    switch (i)
                    {
                        case 0: { entidad.applicationId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1: { entidad.applicationName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 2: { entidad.description = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 3: { entidad.interfaceId = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 4: { entidad.parentAPTCode = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 5: { entidad.gerencia = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 6: { entidad.division = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 7: { entidad.area = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 8: { entidad.unity = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 9: { entidad.managed = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;//10

                        case 10: { entidad.implementationType = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 11: { entidad.deploymentType = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 12: { entidad.status = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 13: { entidad.teamName = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 14: { entidad.userEntity = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 15: { entidad.developmentType = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 16: { entidad.developmentProvider = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 17: { entidad.replacementApplication = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 18: { entidad.infrastructure = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 19: { entidad.authenticationMethod = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //20

                        case 20: { entidad.authorizationMethod = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 21: { entidad.groupTicketRemedy = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 22: { entidad.webDomain = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 23: { entidad.complianceLevel = Convert.ToDecimal(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 24: { entidad.summaryStandard = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 25: { entidad.assetType = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 26: { entidad.BIANArea = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 27: { entidad.BIANdomain = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 28: { entidad.mainOffice = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 29: { entidad.tobe = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //30

                        case 30: { entidad.technologyCategory = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 31: { entidad.technicalSubclassification = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 32: { entidad.technicalClassification = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 33: { entidad.tierProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 34: { entidad.tierPreProduction = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 35: { entidad.applicationCriticalityBIA = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 36: { entidad.classification = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 37: { entidad.finalCriticality = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 38: { entidad.starProduct = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 39: { entidad.shorterApplicationResponseTime = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;//40

                        case 40: { entidad.highestDegreeInterruption = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 41: { entidad.dateFirstRelease = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 42: { entidad.tribeTechnicalLeadId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 43: { entidad.tribeTechnicalLeadNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 44: { entidad.tribeTechnicalLeadEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 45: { entidad.jefeDeEquipoId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 46: { entidad.jefeDeEquipoNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 47: { entidad.jefeDeEquipoEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 48: { entidad.brokerId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 49: { entidad.brokerNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;//50

                        case 50: { entidad.brokerEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 51: { entidad.ownerId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 52: { entidad.ownerNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 53: { entidad.ownerEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 54: { entidad.gestorId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 55: { entidad.gestorNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 56: { entidad.gestorEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 57: { entidad.expertoId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 58: { entidad.expertoNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 59: { entidad.expertoEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;//60

                        case 60: { entidad.tribeLeadId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 61: { entidad.tribeLeadNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 62: { entidad.tribeLeadEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 63: { entidad.solicitanteId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 64: { entidad.solicitanteNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 65: { entidad.solicitanteEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 66: { entidad.arquitectoEvaluadorId = Convert.ToInt32(detalleFilaExcel[i] == "" ? null : detalleFilaExcel[i]); } break;
                        case 67: { entidad.arquitectoEvaluadorNombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 68: { entidad.arquitectoEvaluadorEmail = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;//69


                    }
                }
            }
            //entidad.Activo = true;
            //entidad.UsuarioCreacion = MODIFICADO_POR;
            //entidad.UsuarioModificacion = MODIFICADO_POR;
            if (!FLAGCONTINUAR) entidad = null;

            return entidad;
        }
        private EstadoFila ValidarFilaAplicacionUpdate(List<string> detalleFilaExcel)
        {
            var objStatus = new EstadoFila()
            {
                Mensaje = string.Empty,
                Estado = true
            };

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (objStatus.Estado)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                objStatus.Estado = false;
                                objStatus.Mensaje = string.Format("Registro incompleto: Fila {0} - Columna {1}", "{0}", i + 1);
                            }
                            break;
                    }
                }

                if (!objStatus.Estado) break;
            }

            return objStatus;
        }

        public DataResults CargaMasivaAplicacionUpdate(string ext, Stream stream, string modificadoPor, string NombreUsuario, string Matricula)
        {
            try
            {
                MODIFICADO_POR = modificadoPor;
                Dictionary<string, int> COLUMNAS_APLICACION = Utilitarios.CargarColumnasExcelAplicacionesUpdate2();
                List<ApplicationDetail> ListObjRegistro = null;
                string logInsert = string.Empty;
                int fila = 2;
                int flag = 0;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<ApplicationDetail>();
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ApplicationDetail ObjRegistro = ObtenerRegistroEntidad<ApplicationDetail>(DevolverColumnasAplicacionUpdate(dr, COLUMNAS_APLICACION), (int)ETipoCargaMasiva.UpdateAplicacion, fila, out logInsert);
                        if (ObjRegistro != null && flag != 0)
                        {
                            ObjRegistro.FilaExcel = fila;
                            ObjRegistro.FlagRegistroValido = false;
                            ListObjRegistro.Add(ObjRegistro);
                        }
                        flag++;
                        fila++;
                    }
                    excelReader.Close();
                }
                PreparaXMLEnviarSPAplicacionUpdate(ListObjRegistro);
                ValidaryActualizarData(NombreUsuario, Matricula);
                return GetResultsCargaPortafolio();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public DataResults CargaMasivaAplicacionUpdate2(string ext, Stream stream, string modificadoPor)
        {
            try
            {
                MODIFICADO_POR = modificadoPor;
                Dictionary<string, int> COLUMNAS_APLICACION = Utilitarios.CargarColumnasExcelAplicacionesUpdate3();
                List<ApplicationHistoricoDetail> ListObjRegistro = null;
                string logInsert = string.Empty;
                int fila = 2;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<ApplicationHistoricoDetail>();
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ApplicationHistoricoDetail ObjRegistro = ObtenerRegistroEntidad<ApplicationHistoricoDetail>(DevolverColumnasAplicacionUpdate2(dr, COLUMNAS_APLICACION), (int)ETipoCargaMasiva.UpdateAplicacionHistorico, fila, out logInsert);
                        if (ObjRegistro != null)
                        {
                            ObjRegistro.FilaExcel = fila;
                            ObjRegistro.FlagRegistroValido = false;
                            ListObjRegistro.Add(ObjRegistro);
                        }
                        fila++;
                    }
                    excelReader.Close();
                }
                PreparaXMLEnviarSPAplicacionUpdate2(ListObjRegistro);
                ValidaryActualizarData2();
                return GetResultsCargaPortafolio2();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void PreparaXMLEnviarSPAplicacionUpdate2(List<ApplicationHistoricoDetail> ListObjRegistro)
        {
            DataSet ds = new DataSet("APLICACIONES");
            DataTable tbl = new DataTable("APLICACION");

            //Nombres en la BD
            #region COLUMNAS_BD
            tbl.Columns.Add("applicationId", typeof(string));
            tbl.Columns.Add("applicationName", typeof(string));
            tbl.Columns.Add("description", typeof(string));
            tbl.Columns.Add("interfaceId", typeof(string));
            tbl.Columns.Add("parentAPTCode", typeof(string));
            tbl.Columns.Add("gerencia", typeof(string));
            tbl.Columns.Add("division", typeof(string)); //DateTime
            tbl.Columns.Add("area", typeof(string));
            tbl.Columns.Add("unity", typeof(string));
            tbl.Columns.Add("managed", typeof(string)); //10

            tbl.Columns.Add("implementationType", typeof(string));
            tbl.Columns.Add("deploymentType", typeof(string));
            tbl.Columns.Add("status", typeof(string));
            tbl.Columns.Add("teamName", typeof(string));
            tbl.Columns.Add("userEntity", typeof(string));
            tbl.Columns.Add("developmentType", typeof(string));
            tbl.Columns.Add("developmentProvider", typeof(string));
            tbl.Columns.Add("replacementApplication", typeof(string));
            tbl.Columns.Add("infrastructure", typeof(string));
            tbl.Columns.Add("authenticationMethod", typeof(string)); //20

            tbl.Columns.Add("authorizationMethod", typeof(string));
            tbl.Columns.Add("groupTicketRemedy", typeof(string));
            tbl.Columns.Add("webDomain", typeof(string));
            tbl.Columns.Add("complianceLevel", typeof(string));
            tbl.Columns.Add("summaryStandard", typeof(string));
            tbl.Columns.Add("assetType", typeof(string));
            tbl.Columns.Add("BIANArea", typeof(string));
            tbl.Columns.Add("BIANdomain", typeof(string));
            tbl.Columns.Add("mainOffice", typeof(string));
            tbl.Columns.Add("tobe", typeof(string)); //30

            tbl.Columns.Add("technologyCategory", typeof(string));
            tbl.Columns.Add("technicalSubclassification", typeof(string));
            tbl.Columns.Add("technicalClassification", typeof(string));
            tbl.Columns.Add("tierProduction", typeof(string));
            tbl.Columns.Add("tierPreProduction", typeof(string));
            tbl.Columns.Add("applicationCriticalityBIA", typeof(string));
            tbl.Columns.Add("classification", typeof(string));
            tbl.Columns.Add("finalCriticality", typeof(string));
            tbl.Columns.Add("starProduct", typeof(string));
            tbl.Columns.Add("shorterApplicationResponseTime", typeof(string));//40

            tbl.Columns.Add("highestDegreeInterruption", typeof(string));
            tbl.Columns.Add("dateFirstRelease", typeof(DateTime));
            tbl.Columns.Add("tribeTechnicalLeadId", typeof(int));
            tbl.Columns.Add("tribeTechnicalLeadNombre", typeof(string));
            tbl.Columns.Add("tribeTechnicalLeadEmail", typeof(string));
            tbl.Columns.Add("jefeDeEquipoId", typeof(int));
            tbl.Columns.Add("jefeDeEquipoNombre", typeof(string));
            tbl.Columns.Add("jefeDeEquipoEmail", typeof(string));
            tbl.Columns.Add("brokerId", typeof(int));
            tbl.Columns.Add("brokerNombre", typeof(string));//50

            tbl.Columns.Add("brokerEmail", typeof(string));
            tbl.Columns.Add("ownerId", typeof(string));
            tbl.Columns.Add("ownerNombre", typeof(string));
            tbl.Columns.Add("ownerEmail", typeof(string));
            tbl.Columns.Add("gestorId", typeof(int));
            tbl.Columns.Add("gestorNombre", typeof(string));
            tbl.Columns.Add("gestorEmail", typeof(string));
            tbl.Columns.Add("expertoId", typeof(int));
            tbl.Columns.Add("expertoNombre", typeof(string));
            tbl.Columns.Add("expertoEmail", typeof(string));//60

            tbl.Columns.Add("tribeLeadId", typeof(int));
            tbl.Columns.Add("tribeLeadNombre", typeof(string));
            tbl.Columns.Add("tribeLeadEmail", typeof(string));
            tbl.Columns.Add("solicitanteId", typeof(int));
            tbl.Columns.Add("solicitanteNombre", typeof(string));
            tbl.Columns.Add("solicitanteEmail", typeof(string));
            tbl.Columns.Add("arquitectoEvaluadorId", typeof(int));
            tbl.Columns.Add("arquitectoEvaluadorNombre", typeof(string));
            tbl.Columns.Add("arquitectoEvaluadorEmail", typeof(string));//69

            tbl.Columns.Add("FilaExcel", typeof(int));
            tbl.Columns.Add("FlagRegistroValido", typeof(int));


            #endregion

            ds.Tables.Add(tbl);

            if (ListObjRegistro.Count > 0)
            {
                log.DebugFormat("Se van a procesar un total de {0} aplicaciones", ListObjRegistro.Count);

                TruncarTabla("[data].[ApplicationHistorico]");
                TruncarTabla("[app].[ErrorCargaMasiva]");

                foreach (var item in ListObjRegistro)
                {
                    var gerenciaId = ServiceManager<ApplicationDAO>.Provider.GetGereciaId(item.gerencia);
                    var divisionId = ServiceManager<ApplicationDAO>.Provider.GetDivisionId(item.division);
                    var unityId = ServiceManager<ApplicationDAO>.Provider.GetUnitId(item.unity);
                    var managedId = ServiceManager<ApplicationDAO>.Provider.GetManagedId(item.managed);
                    var implementationTypeId = ServiceManager<ApplicationDAO>.Provider.GetImplementationTypeId(item.implementationType);

                    var deploymentTypeId = ServiceManager<ApplicationDAO>.Provider.GetDeploymentTypeId(item.deploymentType);
                    var statusId = ServiceManager<ApplicationDAO>.Provider.GetStatusId(item.status);
                    var developmentTypeId = ServiceManager<ApplicationDAO>.Provider.GetDevTypeId(item.developmentType);
                    var infrastructureId = ServiceManager<ApplicationDAO>.Provider.GetInfrastructureId(item.infrastructure);
                    var authenticationMethodId = ServiceManager<ApplicationDAO>.Provider.GetAuthenticationMethodId(item.authenticationMethod);
                    var authorizationMethodId = ServiceManager<ApplicationDAO>.Provider.GetAuthorizationMethodId(item.authorizationMethod);
                    var groupTicketRemedyId = ServiceManager<ApplicationDAO>.Provider.GetGroupTicketRemedyId(item.groupTicketRemedy);
                    var assetTypeId = ServiceManager<ApplicationDAO>.Provider.GetAssetTypeId(item.assetType);
                    var BIANAreaId = ServiceManager<ApplicationDAO>.Provider.GetBIANAreaId(item.BIANArea);
                    var BIANdomainId = ServiceManager<ApplicationDAO>.Provider.GetBianDomainId(item.BIANdomain);
                    var mainOfficeId = ServiceManager<ApplicationDAO>.Provider.GetMainOfficeId(item.mainOffice);
                    var tobeId = ServiceManager<ApplicationDAO>.Provider.GetTobeId(item.tobe);
                    var technologyCategoryId = ServiceManager<ApplicationDAO>.Provider.GetTechnologyCategoryId(item.technologyCategory);

                    var technicalSubclassificationId = ServiceManager<ApplicationDAO>.Provider.GetTechnicalSubclassificationId(item.technicalSubclassification);
                    var technicalClassificationId = ServiceManager<ApplicationDAO>.Provider.GetTechnicalClassificationId(item.technicalClassification);


                    var userEntityId = ServiceManager<ApplicationDAO>.Provider.GetUserEntityId(item.userEntity);
                    var applicationCriticalityBIAId = ServiceManager<ApplicationDAO>.Provider.GetapplicationCriticalityBIAId(item.applicationCriticalityBIA);
                    //var classificationId = ServiceManager<ApplicationDAO>.Provider.GetClassificationId(item.classification);
                    //var finalCriticality = ServiceManager<ApplicationDAO>.Provider.GetFinalCriticalityId(item.finalCriticality);








                    //var architectId = ServiceManager<ApplicationDAO>.Provider.GetArchitectId(item.architectName);

                    DataRow dr = ds.Tables["APLICACION"].NewRow();
                    dr["applicationId"] = Utilitarios.GetStringDefault(item.applicationId);
                    dr["applicationName"] = Utilitarios.GetStringDefault(item.applicationName);
                    dr["description"] = Utilitarios.GetStringDefault(item.description);
                    dr["interfaceId"] = Utilitarios.GetStringDefault(item.interfaceId);
                    dr["parentAPTCode"] = Utilitarios.GetStringDefault(item.parentAPTCode);
                    dr["gerencia"] = gerenciaId != null ? (object)gerenciaId : DBNull.Value;
                    dr["division"] = divisionId != null ? (object)divisionId : DBNull.Value;
                    //dr["area"] = areaId != null ? (object)areaId : DBNull.Value;
                    dr["unity"] = unityId != null ? (object)unityId : DBNull.Value;
                    dr["managed"] = managedId != null ? (object)managedId : DBNull.Value;

                    //10

                    dr["implementationType"] = implementationTypeId != null ? (object)implementationTypeId : DBNull.Value;
                    dr["deploymentType"] = deploymentTypeId != null ? (object)deploymentTypeId : DBNull.Value;
                    dr["status"] = statusId != null ? (object)statusId : DBNull.Value;
                    dr["teamName"] = Utilitarios.GetStringDefault(item.teamName);
                    dr["userEntity"] = userEntityId != null ? (object)userEntityId : DBNull.Value;
                    dr["developmentType"] = developmentTypeId != null ? (object)developmentTypeId : DBNull.Value;
                    dr["developmentProvider"] = Utilitarios.GetStringDefault(item.developmentProvider);
                    dr["replacementApplication"] = Utilitarios.GetStringDefault(item.replacementApplication);
                    dr["infrastructure"] = infrastructureId != null ? (object)infrastructureId : DBNull.Value;
                    dr["authenticationMethod"] = authenticationMethodId != null ? (object)authenticationMethodId : DBNull.Value;

                    //20

                    dr["authorizationMethod"] = authorizationMethodId != null ? (object)authorizationMethodId : DBNull.Value;
                    dr["groupTicketRemedy"] = groupTicketRemedyId != null ? (object)groupTicketRemedyId : DBNull.Value;
                    dr["webDomain"] = Utilitarios.GetStringDefault(item.webDomain);
                    dr["complianceLevel"] = item.complianceLevel != null ? (object)item.complianceLevel : DBNull.Value;

                    dr["summaryStandard"] = Utilitarios.GetStringDefault(item.summaryStandard);
                    dr["assetType"] = assetTypeId != null ? (object)assetTypeId : DBNull.Value;
                    dr["BIANArea"] = BIANAreaId != null ? (object)BIANAreaId : DBNull.Value;
                    dr["BIANdomain"] = BIANdomainId != null ? (object)BIANdomainId : DBNull.Value;
                    dr["mainOffice"] = mainOfficeId != null ? (object)mainOfficeId : DBNull.Value;
                    dr["tobe"] = tobeId != null ? (object)tobeId : DBNull.Value;

                    //30

                    dr["technologyCategory"] = technologyCategoryId != null ? (object)technologyCategoryId : DBNull.Value;
                    dr["technicalSubclassification"] = technicalSubclassificationId != null ? (object)technicalSubclassificationId : DBNull.Value;
                    dr["technicalClassification"] = technicalClassificationId != null ? (object)technicalClassificationId : DBNull.Value;
                    dr["tierProduction"] = Utilitarios.GetStringDefault(item.tierProduction);
                    dr["tierPreProduction"] = Utilitarios.GetStringDefault(item.tierPreProduction);
                    dr["applicationCriticalityBIA"] = applicationCriticalityBIAId != null ? (object)applicationCriticalityBIAId : DBNull.Value;
                    dr["classification"] = Utilitarios.GetStringDefault(item.classification);
                    dr["finalCriticality"] = Utilitarios.GetStringDefault(item.tierProduction);
                    dr["starProduct"] = Utilitarios.GetStringDefault(item.tierPreProduction);
                    dr["shorterApplicationResponseTime"] = Utilitarios.GetStringDefault(item.shorterApplicationResponseTime);

                    //40

                    dr["highestDegreeInterruption"] = Utilitarios.GetStringDefault(item.highestDegreeInterruption);
                    dr["dateFirstRelease"] = item.dateFirstRelease.HasValue ? (object)item.dateFirstRelease : DBNull.Value;
                    dr["tribeTechnicalLeadId"] = item.tribeTechnicalLeadId != null ? (object)item.tribeTechnicalLeadId : DBNull.Value;
                    dr["tribeTechnicalLeadNombre"] = Utilitarios.GetStringDefault(item.tribeTechnicalLeadNombre);
                    dr["tribeTechnicalLeadEmail"] = Utilitarios.GetStringDefault(item.tribeTechnicalLeadEmail);
                    dr["jefeDeEquipoId"] = item.jefeDeEquipoId != null ? (object)item.jefeDeEquipoId : DBNull.Value;
                    dr["jefeDeEquipoNombre"] = Utilitarios.GetStringDefault(item.jefeDeEquipoNombre);
                    dr["jefeDeEquipoEmail"] = Utilitarios.GetStringDefault(item.jefeDeEquipoEmail);
                    dr["brokerId"] = item.brokerId != null ? (object)item.brokerId : DBNull.Value;
                    dr["brokerNombre"] = Utilitarios.GetStringDefault(item.brokerNombre);

                    //50

                    dr["brokerEmail"] = Utilitarios.GetStringDefault(item.brokerEmail);
                    dr["ownerId"] = item.ownerId != null ? (object)item.ownerId : DBNull.Value;
                    dr["ownerNombre"] = Utilitarios.GetStringDefault(item.ownerNombre);
                    dr["ownerEmail"] = Utilitarios.GetStringDefault(item.ownerEmail);
                    dr["gestorId"] = item.gestorId != null ? (object)item.gestorId : DBNull.Value;
                    dr["gestorNombre"] = Utilitarios.GetStringDefault(item.gestorNombre);
                    dr["gestorEmail"] = Utilitarios.GetStringDefault(item.gestorEmail);
                    dr["expertoId"] = item.expertoId != null ? (object)item.expertoId : DBNull.Value;
                    dr["expertoNombre"] = Utilitarios.GetStringDefault(item.expertoNombre);
                    dr["expertoEmail"] = Utilitarios.GetStringDefault(item.expertoEmail);

                    //60


                    dr["tribeLeadId"] = item.tribeLeadId != null ? (object)item.tribeLeadId : DBNull.Value;
                    dr["tribeLeadNombre"] = Utilitarios.GetStringDefault(item.tribeLeadNombre);
                    dr["tribeLeadEmail"] = Utilitarios.GetStringDefault(item.tribeLeadEmail);
                    dr["solicitanteId"] = item.solicitanteId != null ? (object)item.solicitanteId : DBNull.Value;
                    dr["solicitanteNombre"] = Utilitarios.GetStringDefault(item.solicitanteNombre);
                    dr["solicitanteEmail"] = Utilitarios.GetStringDefault(item.solicitanteEmail);
                    dr["arquitectoEvaluadorId"] = item.arquitectoEvaluadorId != null ? (object)item.arquitectoEvaluadorId : DBNull.Value;
                    dr["arquitectoEvaluadorNombre"] = Utilitarios.GetStringDefault(item.arquitectoEvaluadorNombre);
                    dr["arquitectoEvaluadorEmail"] = Utilitarios.GetStringDefault(item.arquitectoEvaluadorEmail);

                    //69


                    dr["FilaExcel"] = item.FilaExcel ?? 0;

                    dr["FlagRegistroValido"] = item.FlagRegistroValido ?? false;

                    ds.Tables["APLICACION"].Rows.Add(dr);
                }

                using (var sqlBulk = new SqlBulkCopy(_CadenaConexionBDLocal))
                {
                    sqlBulk.BatchSize = 5000;
                    sqlBulk.DestinationTableName = "data.ApplicationHistorico";
                    sqlBulk.BulkCopyTimeout = 0;

                    #region CAMPOS_BD
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationId", "applicationId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationName", "applicationName"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("description", "description"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("interfaceId", "interfaceId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("parentAPTCode", "parentAPTCode"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gerencia", "gerencia"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("division", "division"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("area", "area"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("unity", "unity"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("managed", "managed")); //10

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("implementationType", "implementationType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("deploymentType", "deploymentType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("status", "status"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("teamName", "teamName"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("userEntity", "userEntity"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("developmentType", "developmentType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("developmentProvider", "developmentProvider"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("replacementApplication", "replacementApplication"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("infrastructure", "infrastructure"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("authenticationMethod", "authenticationMethod")); //20

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("authorizationMethod", "authorizationMethod"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("groupTicketRemedy", "groupTicketRemedy"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("webDomain", "webDomain"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("complianceLevel", "complianceLevel"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("summaryStandard", "summaryStandard"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("assetType", "assetType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BIANArea", "BIANArea"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BIANdomain", "BIANdomain"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("mainOffice", "mainOffice"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tobe", "tobe")); //30

                    //30

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technologyCategory", "technologyCategory"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technicalSubclassification", "technicalSubclassification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technicalClassification", "technicalClassification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tierProduction", "tierProduction"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tierPreProduction", "tierPreProduction"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationCriticalityBIA", "applicationCriticalityBIA"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("classification", "classification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("finalCriticality", "finalCriticality"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("starProduct", "starProduct"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("shorterApplicationResponseTime", "shorterApplicationResponseTime"));

                    //40

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("highestDegreeInterruption", "highestDegreeInterruption"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dateFirstRelease", "dateFirstRelease"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeTechnicalLeadId", "tribeTechnicalLeadId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeTechnicalLeadNombre", "tribeTechnicalLeadNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeTechnicalLeadEmail", "tribeTechnicalLeadEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("jefeDeEquipoId", "jefeDeEquipoId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("jefeDeEquipoNombre", "jefeDeEquipoNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("jefeDeEquipoEmail", "jefeDeEquipoEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("brokerId", "brokerId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("brokerNombre", "brokerNombre"));

                    //50

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("brokerEmail", "brokerEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ownerId", "ownerId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ownerNombre", "ownerNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ownerEmail", "ownerEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gestorId", "gestorId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gestorNombre", "gestorNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gestorEmail", "gestorEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("expertoId", "expertoId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("expertoNombre", "expertoNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("expertoEmail", "expertoEmail"));

                    //60

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeLeadId", "tribeLeadId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeLeadNombre", "tribeLeadNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tribeLeadEmail", "tribeLeadEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("solicitanteId", "solicitanteId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("solicitanteNombre", "solicitanteNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("solicitanteEmail", "solicitanteEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("arquitectoEvaluadorId", "arquitectoEvaluadorId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("arquitectoEvaluadorNombre", "arquitectoEvaluadorNombre"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("arquitectoEvaluadorEmail", "arquitectoEvaluadorEmail"));

                    //69

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FilaExcel", "FilaExcel"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FlagRegistroValido", "FlagRegistroValido"));

                    #endregion

                    sqlBulk.WriteToServer(ds.Tables[0]);
                    sqlBulk.Close();
                }
            }
            else
            {
                throw new ArgumentException("El archivo que se está procesando no tiene filas por lo que las aplicaciones no se actualizarán.");
            }
        }
        private void PreparaXMLEnviarSPAplicacionUpdate(List<ApplicationDetail> ListObjRegistro)
        {
            DataSet ds = new DataSet("APLICACIONES");
            DataTable tbl = new DataTable("APLICACION");

            //Nombres en la BD
            #region COLUMNAS_BD
            tbl.Columns.Add("applicationId", typeof(string));
            tbl.Columns.Add("applicationName", typeof(string));
            tbl.Columns.Add("description", typeof(string));
            tbl.Columns.Add("implementationType", typeof(int));
            tbl.Columns.Add("managed", typeof(int));
            tbl.Columns.Add("deploymentType", typeof(int));
            tbl.Columns.Add("parentAPTCode", typeof(string)); //DateTime
            tbl.Columns.Add("status", typeof(int));
            tbl.Columns.Add("interfaceId", typeof(string));
            tbl.Columns.Add("registerDate", typeof(DateTime)); //10

            //tbl.Columns.Add("registerBy", typeof(string)); //10

            tbl.Columns.Add("unit", typeof(int));
            tbl.Columns.Add("teamName", typeof(string));
            tbl.Columns.Add("userEntity", typeof(string));
            tbl.Columns.Add("developmentType", typeof(int));
            tbl.Columns.Add("developmentProvider", typeof(string));
            tbl.Columns.Add("infrastructure", typeof(int));
            tbl.Columns.Add("replacementApplication", typeof(string));
            tbl.Columns.Add("assetType", typeof(int));
            tbl.Columns.Add("BIANdomain", typeof(int));
            tbl.Columns.Add("mainOffice", typeof(int)); //20

            tbl.Columns.Add("technologyCategory", typeof(int));
            tbl.Columns.Add("technicalSubclassification", typeof(int));
            tbl.Columns.Add("technicalClassification", typeof(int));
            tbl.Columns.Add("area", typeof(int));
            tbl.Columns.Add("groupTicketRemedy", typeof(int));
            tbl.Columns.Add("webDomain", typeof(string));
            tbl.Columns.Add("dateFirstRelease", typeof(DateTime));
            tbl.Columns.Add("starProduct", typeof(string));
            tbl.Columns.Add("shorterApplicationResponseTime", typeof(string));
            tbl.Columns.Add("highestDegreeInterruption", typeof(string)); //30



            tbl.Columns.Add("tobe", typeof(int));
            tbl.Columns.Add("tierPreProduction", typeof(string));
            tbl.Columns.Add("tierProduction", typeof(string));

            //tbl.Columns.Add("registrationSituation", typeof(string));
            //tbl.Columns.Add("isFormalApplication", typeof(string));
            //tbl.Columns.Add("regularizationDate", typeof(string));

            tbl.Columns.Add("authorizationMethod", typeof(int));
            tbl.Columns.Add("authenticationMethod", typeof(int)); //40

            //tbl.Columns.Add("hasInterfaceId", typeof(string));

            tbl.Columns.Add("architectId", typeof(int));
            tbl.Columns.Add("FilaExcel", typeof(int));
            tbl.Columns.Add("FlagRegistroValido", typeof(int));

            tbl.Columns.Add("classification", typeof(int));
            tbl.Columns.Add("applicationCriticalityBIA", typeof(int));

            tbl.Columns.Add("finalCriticality", typeof(int));

            tbl.Columns.Add("complianceLevel", typeof(string));

            tbl.Columns.Add("summaryStandard", typeof(string));
            tbl.Columns.Add("areaBIAN", typeof(string));

            tbl.Columns.Add("Experto", typeof(string));

            tbl.Columns.Add("ArquitectoNegocio", typeof(string));
            tbl.Columns.Add("ArquitectoNegocioMatricula", typeof(string));
            tbl.Columns.Add("ArquitectoNegocioEmail", typeof(string));

            tbl.Columns.Add("UsuarioAutorizador", typeof(string));

            tbl.Columns.Add("BrokerSistemas", typeof(string));
            tbl.Columns.Add("BrokerSistemasMatricula", typeof(string));
            tbl.Columns.Add("BrokerSistemasEmail", typeof(string));

            tbl.Columns.Add("TribeLead", typeof(string));
            tbl.Columns.Add("TribeLeadMatricula", typeof(string));
            tbl.Columns.Add("TribeLeadEmail", typeof(string));

            tbl.Columns.Add("TribeTechnicalLead", typeof(string));
            tbl.Columns.Add("TribeTechnicalLeadMatricula", typeof(string));
            tbl.Columns.Add("TribeTechnicalLeadEmail", typeof(string));

            tbl.Columns.Add("JefeEquipo", typeof(string));
            tbl.Columns.Add("JefeEquipoMatricula", typeof(string));
            tbl.Columns.Add("JefeEquipoEmail", typeof(string));

            tbl.Columns.Add("LiderUsuario", typeof(string));
            tbl.Columns.Add("LiderUsuarioMatricula", typeof(string));
            tbl.Columns.Add("LiderUsuarioEmail", typeof(string));

            tbl.Columns.Add("FlagRegistroDistinto", typeof(int));

            tbl.Columns.Add("Division", typeof(int));
            tbl.Columns.Add("Gerencia", typeof(int));

            tbl.Columns.Add("FlagPirata", typeof(string));

            tbl.Columns.Add("tipoPCI", typeof(string));

            //tbl.Columns.Add("teamId", typeof(string));
            //tbl.Columns.Add("isApproved", typeof(string));
            //tbl.Columns.Add("approvedBy", typeof(string));
            //tbl.Columns.Add("dateApproved", typeof(string));
            //tbl.Columns.Add("isImported", typeof(string));
            //tbl.Columns.Add("isActive", typeof(string));
            //tbl.Columns.Add("desactivatedBy", typeof(string));
            //tbl.Columns.Add("dateDesactivated", typeof(string)); //50

            //tbl.Columns.Add("deploymentTypeOriginal", typeof(string));
            //tbl.Columns.Add("isObserved", typeof(string));
            //tbl.Columns.Add("commentsObserved", typeof(string));


            #endregion

            ds.Tables.Add(tbl);

            if (ListObjRegistro.Count > 0)
            {
                log.DebugFormat("Se van a procesar un total de {0} aplicaciones", ListObjRegistro.Count);

                TruncarTabla("[app].[ApplicationCargaMasiva]");
                TruncarTabla("[app].[ErrorCargaMasiva]");

                var ListaParametricaDetalle = ServiceManager<ApplicationDAO>.Provider.GetAllParametricaDetalle();
                var ListaGestionadoPor = ServiceManager<ApplicationDAO>.Provider.GetAllGestionadoPor();
                var ListaUnidades = ServiceManager<ApplicationDAO>.Provider.GetAllUnit();
                var ListaTipoActivo = ServiceManager<ApplicationDAO>.Provider.GetAllAssetType();
                var ListaDominioBian = ServiceManager<ApplicationDAO>.Provider.GetAllBianDomain();
                var ListaJefaturaAti = ServiceManager<ApplicationDAO>.Provider.GetAllMainOffice();
                var ListaClasificacionTecnica = ServiceManager<ApplicationDAO>.Provider.GetAllClasificacionTecnica();
                var ListaSubClasificacionTecnica = ServiceManager<ApplicationDAO>.Provider.GetAllSubClasificacionTecnica();
                var ListaGrupoRemedy = ServiceManager<ApplicationDAO>.Provider.GetAllGrupoRemedy();
                var ListaTobe = ServiceManager<ApplicationDAO>.Provider.GetAllTobe();
                var ListaArea = ServiceManager<ApplicationDAO>.Provider.GetAllAreas();
                var ListaAreaBian = ServiceManager<ApplicationDAO>.Provider.GetAllAreaBian();

                var ListaPersonas = ServiceManager<ApplicationDAO>.Provider.GetAllApplicationManager();
                var ListaDivisiones = ServiceManager<ApplicationDAO>.Provider.GetAllDivision();
                var ListaGerencias = ServiceManager<ApplicationDAO>.Provider.GetAllGerencia();

                foreach (var item in ListObjRegistro)
                {
                    //var implementationType = ServiceManager<ApplicationDAO>.Provider.GetImplementationTypeId(item.implementationTypeName);
                    //var managed = ServiceManager<ApplicationDAO>.Provider.GetManagedId(item.managedName);
                    //var deploymentType = ServiceManager<ApplicationDAO>.Provider.GetDeploymentTypeId(item.deploymentTypeName);
                    //var unit = ServiceManager<ApplicationDAO>.Provider.GetUnitId(item.unitName);
                    //var developmentType = ServiceManager<ApplicationDAO>.Provider.GetDevTypeId(item.developmentTypeName);
                    //var infrastructure = ServiceManager<ApplicationDAO>.Provider.GetInfrastructureId(item.infrastructureName);
                    //var assetType = ServiceManager<ApplicationDAO>.Provider.GetAssetTypeId(item.tipoActivoName);
                    //var BIANdomain = ServiceManager<ApplicationDAO>.Provider.GetBianDomainId(item.dominioBIANName);
                    //var mainOffice = ServiceManager<ApplicationDAO>.Provider.GetMainOfficeId(item.jefaturaATIName);
                    //var technologyCategory = ServiceManager<ApplicationDAO>.Provider.GetTechnologyCategoryId(item.categoriaTecnologicaName);
                    //var technicalSubclassification = ServiceManager<ApplicationDAO>.Provider.GetTechnicalSubclassificationId(item.subClasificacionTecnicaName);
                    //var technicalClassification = ServiceManager<ApplicationDAO>.Provider.GetTechnicalClassificationId(item.clasificacionTecnicaName);
                    //var groupTicketRemedy = ServiceManager<ApplicationDAO>.Provider.GetGroupTicketRemedyId(item.grupoTicketRemedyName);
                    //var tobe = ServiceManager<ApplicationDAO>.Provider.GetTobeId(item.TOBEName);
                    //var authorizationMethod = ServiceManager<ApplicationDAO>.Provider.GetAuthorizationMethodId(item.authorizationMethodName);
                    //var authenticationMethod = ServiceManager<ApplicationDAO>.Provider.GetAuthenticationMethodId(item.authenticationMethodName);
                    //var area = ServiceManager<ApplicationDAO>.Provider.GetAreaId(item.AreaDetail);
                    //var areaBIAN = ServiceManager<ApplicationDAO>.Provider.GetBIANAreaId(item.areaBIANName);

                    var implementationType = ListaParametricaDetalle.Where(x => x.Valor == item.implementationTypeName && x.FlagActivo == true && x.Descripcion == "Tipo de implementacion").Select(x => x.ParametricaDetalleId).FirstOrDefault();
                   
                    var managed = ListaGestionadoPor.Where(x => x.Nombre == item.managedName).Select(x => x.GestionadoPorId).FirstOrDefault();
              
                    var deploymentType = ListaParametricaDetalle.Where(x => x.Valor == item.deploymentTypeName && x.FlagActivo == true && x.Descripcion == "Modelo de entrega").Select(x => x.ParametricaDetalleId).FirstOrDefault();
                    var status = ServiceManager<ApplicationDAO>.Provider.GetStatusId(item.statusName);
               
                   
                    var userEntity = ServiceManager<ApplicationDAO>.Provider.GetUserEntityId(item.userEntityName);

                    var tipoPCI = ServiceManager<ApplicationDAO>.Provider.GetPCIId(item.tipoPCI);

                    var developmentType = ListaParametricaDetalle.Where(x => x.Valor == item.developmentTypeName && x.FlagActivo == true && x.Descripcion == "Tipo de desarrollo").Select(x => x.ParametricaDetalleId).FirstOrDefault();
                 
                    var infrastructure = ListaParametricaDetalle.Where(x => x.Valor == item.infrastructureName && x.FlagActivo == true && x.Descripcion == "Infraestructura de la aplicación").Select(x => x.ParametricaDetalleId).FirstOrDefault();
                  
                    var assetType = ListaTipoActivo.Where(x => x.Nombre == item.tipoActivoName).Select(x => x.TipoActivoInformacionId).FirstOrDefault();
                  
                    var BIANdomain = ListaDominioBian.Where(x => x.Nombre == item.dominioBIANName).Select(x => x.DominioBianId).FirstOrDefault();
                 
                    var mainOffice = ListaJefaturaAti.Where(x => x.Nombre == item.jefaturaATIName).Select(x => x.JefaturaAtiId).FirstOrDefault();
      
                    var technologyCategory = ListaParametricaDetalle.Where(x => x.Valor == item.categoriaTecnologicaName && x.FlagActivo == true && x.Descripcion == "Categoria tecnologica").Select(x => x.ParametricaDetalleId).FirstOrDefault();
                
                    var technicalSubclassification = ListaSubClasificacionTecnica.Where(x => x.Nombre == item.subClasificacionTecnicaName).Select(x => x.SubClasificacionTecnicaId).FirstOrDefault();
          
                    var technicalClassification = ListaClasificacionTecnica.Where(x => x.Nombre == item.clasificacionTecnicaName).Select(x => x.ClasificacionTecnicaId).FirstOrDefault();
               
                    var groupTicketRemedy = ListaGrupoRemedy.Where(x => x.Nombre == item.grupoTicketRemedyName).Select(x => x.GrupoRemedyId).FirstOrDefault();
                 
                    var tobe = ListaTobe.Where(x => x.Nombre == item.TOBEName).Select(x => x.PlataformaBcpId).FirstOrDefault();
          
                    var authorizationMethod = ListaParametricaDetalle.Where(x => x.Valor == item.authorizationMethodName && x.FlagActivo == true && x.Descripcion == "Método de autorización").Select(x => x.ParametricaDetalleId).FirstOrDefault();
            
                    var authenticationMethod = ListaParametricaDetalle.Where(x => x.Valor == item.authenticationMethodName && x.FlagActivo == true && x.Descripcion == "Método de autenticación").Select(x => x.ParametricaDetalleId).FirstOrDefault();

                    var Gerencia = ListaGerencias.Where(x => x.Nombre == item.GerenciaCentral).Select(x => x.GerenciaId).FirstOrDefault();

                    var Division = ListaDivisiones.Where(x => x.Nombre == item.DivisionDetail && x.GerenciaId==Gerencia).Select(x => x.DivisionId).FirstOrDefault();

                    var area = ListaArea.Where(x => x.Nombre == item.AreaDetail && x.DivisionId== Division).Select(x => x.AreaId).FirstOrDefault();

                    var unit = ListaUnidades.Where(x => x.Nombre == item.unitName && x.AreaId==area).Select(x => x.UnidadId).FirstOrDefault();

                    var areaBIAN = ListaAreaBian.Where(x => x.Nombre == item.areaBIANName).Select(x => x.AreaBianId).FirstOrDefault();

                    var classification = ServiceManager<ApplicationDAO>.Provider.GetAssetClassificationId(item.clasificacionActivoName);
                    var applicationCriticalityBIA = ServiceManager<ApplicationDAO>.Provider.GetAssetBIACriticalityId(item.criticidadBIANName);

                    //Roles
                    //var ExpertoMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.Experto);
                    //var ExpertoEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.Experto);

                    //var ArquitectoNegocioMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.ArquitectoNegocio);
                    //var ArquitectoNegocioEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.ArquitectoNegocio);
                    var ArquitectoNegocioMatricula = ListaPersonas.Where(x => x.managerName == item.ArquitectoNegocio).Select(x => x.username).FirstOrDefault();
               
                    var ArquitectoNegocioEmail = ListaPersonas.Where(x => x.managerName == item.ArquitectoNegocio).Select(x => x.email).FirstOrDefault();


                    //var UsuarioAutorizadorMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.UsuarioAutorizador);
                    //var UsuarioAutorizadorEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.UsuarioAutorizador);


                    //var BrokerSistemasMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.BrokerSistemas);
                    //var BrokerSistemasEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.BrokerSistemas);
                    var BrokerSistemasMatricula = ListaPersonas.Where(x => x.managerName == item.BrokerSistemas).Select(x => x.username).FirstOrDefault();

                    var BrokerSistemasEmail = ListaPersonas.Where(x => x.managerName == item.BrokerSistemas).Select(x => x.email).FirstOrDefault();

                    //var TribeLeadMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.TribeLead);
                    //var TribeLeadEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.TribeLead);

                    var TribeLeadMatricula = ListaPersonas.Where(x => x.managerName == item.TribeLead).Select(x => x.username).FirstOrDefault();

                    var TribeLeadEmail = ListaPersonas.Where(x => x.managerName == item.TribeLead).Select(x => x.email).FirstOrDefault();

                    //var TribeTechnicalLeadMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.TribeTechnicalLead);
                    //var TribeTechnicalLeadEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.TribeTechnicalLead);

                    var TribeTechnicalLeadMatricula = ListaPersonas.Where(x => x.managerName == item.TribeTechnicalLead).Select(x => x.username).FirstOrDefault();

                    var TribeTechnicalLeadEmail = ListaPersonas.Where(x => x.managerName == item.TribeTechnicalLead).Select(x => x.email).FirstOrDefault();

                    //var JefeEquipoMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.JefeEquipo);
                    //var JefeEquipoEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.JefeEquipo);

                    var JefeEquipoMatricula = ListaPersonas.Where(x => x.managerName == item.JefeEquipo).Select(x => x.username).FirstOrDefault();

                    var JefeEquipoEmail = ListaPersonas.Where(x => x.managerName == item.JefeEquipo).Select(x => x.email).FirstOrDefault();

                    //var LiderUsuarioMatricula = ServiceManager<ApplicationDAO>.Provider.GetMatriculaByName(item.LiderUsuarioDetail);
                    //var LiderUsuarioEmail = ServiceManager<ApplicationDAO>.Provider.GetEmailByName(item.LiderUsuarioDetail);


                    var LiderUsuarioMatricula = ListaPersonas.Where(x => x.managerName == item.LiderUsuarioDetail).Select(x => x.username).FirstOrDefault();

                    var LiderUsuarioEmail = ListaPersonas.Where(x => x.managerName == item.LiderUsuarioDetail).Select(x => x.email).FirstOrDefault();

                   

                    DataRow dr = ds.Tables["APLICACION"].NewRow();
                    dr["applicationId"] = Utilitarios.GetStringDefault(item.applicationId);
                    dr["applicationName"] = Utilitarios.GetStringDefault(item.applicationName);
                    dr["description"] = Utilitarios.GetStringDefault(item.description);
                    dr["implementationType"] = implementationType != 0 ? (object)implementationType : DBNull.Value;
                    dr["managed"] = managed != 0 ? (object)managed : DBNull.Value;
                    dr["deploymentType"] = deploymentType != 0 ? (object)deploymentType : DBNull.Value;
                    dr["parentAPTCode"] = Utilitarios.GetStringDefault(item.parentAPTCode);
                    dr["status"] = status != null ? (object)status : DBNull.Value;
                    dr["interfaceId"] = Utilitarios.GetStringDefault(item.interfaceId);
                    dr["registerDate"] = item.registerDate.HasValue ? (object)item.registerDate : DBNull.Value;

                    //10

                    dr["unit"] = unit != 0 ? (object)unit : DBNull.Value;

                    dr["area"] = area != 0 ? (object)area : DBNull.Value;

                    dr["teamName"] = Utilitarios.GetStringDefault(item.teamName);
                    dr["userEntity"] = userEntity != null ? (object)userEntity : DBNull.Value;
                    dr["developmentType"] = developmentType != 0 ? (object)developmentType : DBNull.Value;
                    dr["developmentProvider"] = Utilitarios.GetStringDefault(item.developmentProvider);
                    dr["infrastructure"] = infrastructure != 0 ? (object)infrastructure : DBNull.Value;
                    dr["replacementApplication"] = Utilitarios.GetStringDefault(item.replacementApplication);
                    dr["assetType"] = assetType != 0 ? (object)assetType : DBNull.Value;
                    dr["BIANdomain"] = BIANdomain != 0 ? (object)BIANdomain : DBNull.Value;
                    dr["mainOffice"] = mainOffice != 0 ? (object)mainOffice : DBNull.Value;

                    //20

                    dr["technologyCategory"] = technologyCategory != 0 ? (object)technologyCategory : DBNull.Value;
                    dr["technicalSubclassification"] = technicalSubclassification != 0 ? (object)technicalSubclassification : DBNull.Value;
                    dr["technicalClassification"] = technicalClassification != 0 ? (object)technicalClassification : DBNull.Value;
                    dr["groupTicketRemedy"] = groupTicketRemedy != 0 ? (object)groupTicketRemedy : DBNull.Value;
                    dr["webDomain"] = Utilitarios.GetStringDefault(item.webDomain);
                    dr["dateFirstRelease"] = item.dateFirstRelease.HasValue ? (object)item.dateFirstRelease : DBNull.Value;
                    dr["starProduct"] = Utilitarios.GetStringDefault(item.starProduct);
                    //var shorterApplicationResponseTime = item.MenorRTOName == "" ? (int?)null : Convert.ToInt32(item.MenorRTOName);
                    //var highestDegreeInterruption = item.MayorGradoInterrupcionName == "" ? (int?)null : Convert.ToInt32(item.MayorGradoInterrupcionName);
                    dr["shorterApplicationResponseTime"] = item.MenorRTOName != null ? (object)item.MenorRTOName : DBNull.Value;
                    dr["highestDegreeInterruption"] = item.MayorGradoInterrupcionName != null ? (object)item.MayorGradoInterrupcionName : DBNull.Value;

                    dr["tipoPCI"] = tipoPCI != null ? (object)tipoPCI : DBNull.Value;

                    //30

                    dr["tobe"] = tobe != 0 ? (object)tobe : DBNull.Value;
                    dr["tierPreProduction"] = Utilitarios.GetStringDefault(item.tierPreProduction);
                    dr["tierProduction"] = Utilitarios.GetStringDefault(item.tierProduction);
                    dr["authorizationMethod"] = authorizationMethod != 0 ? (object)authorizationMethod : DBNull.Value;
                    dr["authenticationMethod"] = authenticationMethod != 0 ? (object)authenticationMethod : DBNull.Value;

                    dr["FilaExcel"] = item.FilaExcel ?? 0;
                    //dr["UsuarioModificacion"] = Utilitarios.GetStringDefault(item.UsuarioModificacion);
                    dr["FlagRegistroValido"] = item.FlagRegistroValido ?? false;

                    dr["classification"] = classification != null ? (object)classification : DBNull.Value;


                    dr["applicationCriticalityBIA"] = applicationCriticalityBIA != null ? (object)applicationCriticalityBIA : DBNull.Value;


                    int? finalCriticality = null;
                    if (classification != null && applicationCriticalityBIA != null ) { 
                    finalCriticality = ServiceManager<ApplicationDAO>.Provider.GetFinalCriticality(classification, applicationCriticalityBIA);
                    }
                    dr["finalCriticality"] = finalCriticality != null ? (object)finalCriticality : DBNull.Value;

                    //var complianceLevel = item.complianceLevelName == null ? (decimal?)null : Convert.ToDecimal(item.complianceLevelName);
                    dr["complianceLevel"] = item.complianceLevelName != null ? (object)item.complianceLevelName : DBNull.Value;

                    dr["summaryStandard"] = Utilitarios.GetStringDefault(item.summaryStandard);

                    dr["areaBIAN"] = areaBIAN != 0 ? (object)areaBIAN : DBNull.Value;

                    //Roles
                    dr["Experto"] = item.Experto != null ? (object)item.Experto : DBNull.Value;

                    //dr["ExpertoMatricula"] = ExpertoMatricula != null ? (object)ExpertoMatricula : DBNull.Value;

                    //dr["ExpertoEmail"] = ExpertoEmail != null ? (object)ExpertoEmail : DBNull.Value;

                    dr["ArquitectoNegocio"] = item.ArquitectoNegocio != null ? (object)item.ArquitectoNegocio : DBNull.Value;

                    dr["ArquitectoNegocioMatricula"] = ArquitectoNegocioMatricula != null ? (object)ArquitectoNegocioMatricula : DBNull.Value;

                    dr["ArquitectoNegocioEmail"] = ArquitectoNegocioEmail != null ? (object)ArquitectoNegocioEmail : DBNull.Value;

                    dr["UsuarioAutorizador"] = item.UsuarioAutorizador != null ? (object)item.UsuarioAutorizador : DBNull.Value;

                    //dr["UsuarioAutorizadorMatricula"] = UsuarioAutorizadorMatricula != null ? (object)UsuarioAutorizadorMatricula : DBNull.Value;

                    //dr["UsuarioAutorizadorEmail"] = UsuarioAutorizadorEmail != null ? (object)UsuarioAutorizadorEmail : DBNull.Value;

                    dr["BrokerSistemas"] = item.BrokerSistemas != null ? (object)item.BrokerSistemas : DBNull.Value;

                    dr["BrokerSistemasMatricula"] = BrokerSistemasMatricula != null ? (object)BrokerSistemasMatricula : DBNull.Value;

                    dr["BrokerSistemasEmail"] = BrokerSistemasEmail != null ? (object)BrokerSistemasEmail : DBNull.Value;

                    dr["TribeLead"] = item.TribeLead != null ? (object)item.TribeLead : DBNull.Value;

                    dr["TribeLeadMatricula"] = TribeLeadMatricula != null ? (object)TribeLeadMatricula : DBNull.Value;

                    dr["TribeLeadEmail"] = TribeLeadEmail != null ? (object)TribeLeadEmail : DBNull.Value;

                    dr["TribeTechnicalLead"] = item.TribeTechnicalLead != null ? (object)item.TribeTechnicalLead : DBNull.Value;

                    dr["TribeTechnicalLeadMatricula"] = TribeTechnicalLeadMatricula != null ? (object)TribeTechnicalLeadMatricula : DBNull.Value;

                    dr["TribeTechnicalLeadEmail"] = TribeTechnicalLeadEmail != null ? (object)TribeTechnicalLeadEmail : DBNull.Value;

                    dr["JefeEquipo"] = item.JefeEquipo != null ? (object)item.JefeEquipo : DBNull.Value;

                    dr["JefeEquipoMatricula"] = JefeEquipoMatricula != null ? (object)JefeEquipoMatricula : DBNull.Value;

                    dr["JefeEquipoEmail"] = JefeEquipoEmail != null ? (object)JefeEquipoEmail : DBNull.Value;

                    dr["LiderUsuario"] = item.LiderUsuarioDetail != null ? (object)item.LiderUsuarioDetail : DBNull.Value;

                    dr["LiderUsuarioMatricula"] = LiderUsuarioMatricula != null ? (object)LiderUsuarioMatricula : DBNull.Value;

                    dr["LiderUsuarioEmail"] = LiderUsuarioEmail != null ? (object)LiderUsuarioEmail : DBNull.Value;

                    dr["FlagRegistroDistinto"] = item.FlagRegistroValido ?? false;

                    dr["Division"] = area != 0 ? (object)Division : DBNull.Value;

                    dr["Gerencia"] = area != 0 ? (object)Gerencia : DBNull.Value;

                    dr["FlagPirata"] = item.FlagPirata != null ? (object)item.FlagPirata : DBNull.Value;



                    ds.Tables["APLICACION"].Rows.Add(dr);
                }

                using (var sqlBulk = new SqlBulkCopy(_CadenaConexionBDLocal))
                {
                    sqlBulk.BatchSize = 5000;
                    sqlBulk.DestinationTableName = "app.ApplicationCargaMasiva";
                    sqlBulk.BulkCopyTimeout = 0;

                    #region CAMPOS_BD
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationId", "applicationId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationName", "applicationName"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("description", "description"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("implementationType", "implementationType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("managed", "managed"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("deploymentType", "deploymentType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("parentAPTCode", "parentAPTCode"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("status", "status"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("interfaceId", "interfaceId"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("registerDate", "registerDate")); //10

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("unit", "unit"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("teamName", "teamName"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("userEntity", "userEntity"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("developmentType", "developmentType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("developmentProvider", "developmentProvider"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("infrastructure", "infrastructure"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("replacementApplication", "replacementApplication"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("assetType", "assetType"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BIANdomain", "BIANdomain"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("mainOffice", "mainOffice")); //20

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technologyCategory", "technologyCategory"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technicalSubclassification", "technicalSubclassification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("technicalClassification", "technicalClassification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("groupTicketRemedy", "groupTicketRemedy"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("webDomain", "webDomain"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("dateFirstRelease", "dateFirstRelease"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("starProduct", "starProduct"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("shorterApplicationResponseTime", "shorterApplicationResponseTime"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("highestDegreeInterruption", "highestDegreeInterruption")); //30

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tipoPCI", "tipoPCI"));

                    //30

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tobe", "tobe"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tierPreProduction", "tierPreProduction"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("tierProduction", "tierProduction"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("authorizationMethod", "authorizationMethod"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("authenticationMethod", "authenticationMethod"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FilaExcel", "FilaExcel"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FlagRegistroValido", "FlagRegistroValido"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("classification", "classification"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("applicationCriticalityBIA", "applicationCriticalityBIA"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("finalCriticality", "finalCriticality"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("complianceLevel", "complianceLevel"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("summaryStandard", "summaryStandard"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("areaBIAN", "areaBIAN"));


                    //roles


                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Experto", "Experto"));
                    //sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExpertoMatricula", "ExpertoMatricula"));
                    //sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExpertoEmail", "ExpertoEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ArquitectoNegocio", "ArquitectoNegocio"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ArquitectoNegocioMatricula", "ArquitectoNegocioMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ArquitectoNegocioEmail", "ArquitectoNegocioEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UsuarioAutorizador", "UsuarioAutorizador"));
                    //sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UsuarioAutorizadorMatricula", "UsuarioAutorizadorMatricula"));
                    //sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UsuarioAutorizadorEmail", "UsuarioAutorizadorEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BrokerSistemas", "BrokerSistemas"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BrokerSistemasMatricula", "BrokerSistemasMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BrokerSistemasEmail", "BrokerSistemasEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeLead", "TribeLead"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeLeadMatricula", "TribeLeadMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeLeadEmail", "TribeLeadEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeTechnicalLead", "TribeTechnicalLead"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeTechnicalLeadMatricula", "TribeTechnicalLeadMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TribeTechnicalLeadEmail", "TribeTechnicalLeadEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("JefeEquipo", "JefeEquipo"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("JefeEquipoMatricula", "JefeEquipoMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("JefeEquipoEmail", "JefeEquipoEmail"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LiderUsuario", "LiderUsuario"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LiderUsuarioMatricula", "LiderUsuarioMatricula"));
                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LiderUsuarioEmail", "LiderUsuarioEmail"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FlagRegistroDistinto", "FlagRegistroDistinto"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("area", "area"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Division", "Division"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Gerencia", "Gerencia"));

                    sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("FlagPirata", "FlagPirata"));


                    #endregion

                    sqlBulk.WriteToServer(ds.Tables[0]);
                    sqlBulk.Close();
                }
            }
            else
            {
                throw new ArgumentException("El archivo que se está procesando no tiene filas por lo que las aplicaciones no se actualizarán.");
            }
        }

        public void ValidaryActualizarData(string NombreUsuario, string Matricula)
        {
            try
            {
                using (var cnx = ObtenerConexionLocal())
                {
                    using (var cmd = new SqlCommand("[app].[USP_VALIDATE_RECORDS_APPLICATIONS_Portfolio]", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd2 = new SqlCommand("[app].[USP_UPDATE_APLICACION_PORTAFOLIO2]", cnx))
                    {
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@NombreUsuario", NombreUsuario));
                        cmd2.Parameters.Add(new SqlParameter("@Matricula", Matricula));
                        cmd2.CommandTimeout = 0;
                        cmd2.ExecuteNonQuery();
                    }

                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message, ex);
            }
        }

        public void ValidaryActualizarData2()
        {
            try
            {
                using (var cnx = ObtenerConexionLocal())
                {
                    using (var cmd = new SqlCommand("[app].[USP_VALIDATE_RECORDS_APPLICATIONS_Portfolio]", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd2 = new SqlCommand("[app].[USP_UPDATE_APLICACION_PORTAFOLIO3]", cnx))
                    {
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandTimeout = 0;
                        cmd2.ExecuteNonQuery();
                    }

                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message, ex);
            }
        }

        public DataResults GetResultsCargaPortafolio() => ServiceManager<AplicacionDAO>.Provider.GetResultsCargaMasivaPortafolio();
        public DataResults GetResultsCargaPortafolio2() => ServiceManager<AplicacionDAO>.Provider.GetResultsCargaMasivaPortafolio2();

        private List<string> DevolverColumnasAplicacionUpdate(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();



            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationId", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationName", dictionary)].ToString());

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("description", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("managedName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("implementationTypeName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("deploymentTypeName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("statusName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("parentAPTCode", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("interfaceId", dictionary)].ToString());

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("arquitectoNegocio", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("GerenciaCentral", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("Division", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("Area", dictionary)].ToString());//30
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("unitName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("teamName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("Experto", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("userEntityName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentTypeName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentProvider", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("replacementApplication", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("infrastructureName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("authenticationMethodName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("authorizationMethodName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("grupoTicketRemedyName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("webDomain", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("tipoActivoName", dictionary)].ToString());//20

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("areaBIANName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("dominioBIANName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("jefaturaATIName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("TOBEName", dictionary)].ToString());

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("categoriaTecnologicaName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("clasificacionTecnicaName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("subClasificacionTecnicaName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierProduction", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierPreProduction", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("LiderUsuario", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("UsuarioAutorizador", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("TribeLead", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("TribeTechnicalLead", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("JefeEquipo", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("BrokerSistemas", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("starProduct", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("MenorRTOName", dictionary)].ToString());//40

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("MayorGradoInterrupcionName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("criticidadBIAName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("clasificacionActivoName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("nuevaCriticidadName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("FlagPirata", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFlagPirata)", dictionary)].ToString());//50
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("SituacionRegistro", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("registerDate", dictionary)].ToString()); //10
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("dateFirstRelease", dictionary)].ToString());

            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("resumenName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("nivelCumplimientoName", dictionary)].ToString());
            //listaData.Add(reader[Utilitarios.ObtenerValueByKey("Solicitante", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("interfaceId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("description", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("statusName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("implementationTypeName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tipoActivoName", dictionary)].ToString());//20
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("GerenciaCentral", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Division", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Area", dictionary)].ToString());//30
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("unitName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("LiderUsuario", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("UsuarioAutorizador", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("managedName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("teamName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TribeLead", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TribeTechnicalLead", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("JefeEquipo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("BrokerSistemas", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Experto", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("areaBIANName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("dominioBIANName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TOBEName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("jefaturaATIName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("arquitectoNegocio", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("userEntityName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("criticidadBIAName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("clasificacionActivoName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("nuevaCriticidadName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("starProduct", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("MenorRTOName", dictionary)].ToString());//40
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("MayorGradoInterrupcionName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tipoPCI", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierPreProduction", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierProduction", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("deploymentTypeName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("categoriaTecnologicaName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("webDomain", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("clasificacionTecnicaName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("subClasificacionTecnicaName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentTypeName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentProvider", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("infrastructureName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("authenticationMethodName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("authorizationMethodName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("grupoTicketRemedyName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("resumenName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("nivelCumplimientoName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FlagPirata", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFlagPirata)", dictionary)].ToString());//50
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("parentAPTCode", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("replacementApplication", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Solicitante", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("registerDate", dictionary)].ToString()); //10

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("SituacionRegistro", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("dateFirstRelease", dictionary)].ToString());

        

            return listaData;
        }
        private List<string> DevolverColumnasAplicacionUpdate2(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("description", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("interfaceId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("parentAPTCode", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("gerencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("division", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("area", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("unity", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("managed", dictionary)].ToString()); //10

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("implementationType", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("deploymentType", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("status", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("teamName", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("userEntity", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentType", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("developmentProvider", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("replacementApplication", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("infrastructure", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("authenticationMethod", dictionary)].ToString()); //20

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("authorizationMethod", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("groupTicketRemedy", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("webDomain", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("complianceLevel", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("summaryStandard", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("assetType", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("BIANArea", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("BIANdomain", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("mainOffice", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tobe", dictionary)].ToString()); //30

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("technologyCategory", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("technicalSubclassification", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("technicalClassification", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierProduction", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tierPreProduction", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("applicationCriticalityBIA", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("classification", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("finalCriticality", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("starProduct", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("shorterApplicationResponseTime", dictionary)].ToString());//40

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("highestDegreeInterruption", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("dateFirstRelease", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeTechnicalLeadId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeTechnicalLeadNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeTechnicalLeadEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("jefeDeEquipoId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("jefeDeEquipoNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("jefeDeEquipoEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("brokerId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("brokerNombre", dictionary)].ToString());//50

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("brokerEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ownerId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ownerNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ownerEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("gestorId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("gestorNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("gestorEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("expertoId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("expertoNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("expertoEmail", dictionary)].ToString());//60

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeLeadId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeLeadNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("tribeLeadEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("solicitanteId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("solicitanteNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("solicitanteEmail", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("arquitectoEvaluadorId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("arquitectoEvaluadorNombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("arquitectoEvaluadorEmail", dictionary)].ToString());//69



            return listaData;
        }
    }
}
