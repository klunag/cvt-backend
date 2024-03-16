using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BCP.CVT.Cross
{ 
    public static class Constantes
    {
        public static string AzureAppSecret { get; set; }
        public static string ApiAuthKey { get; set; }
        public static string UsuarioSQL { get; set; }
        public static string PasswordSQL { get; set; }
        public static string ServidorSQL { get; set; }
        public static string BaseDatosSQL { get; set; }

        public static string StorageName { get; set; }
        public static string StorageKey { get; set; }

        public static string GraphAuthToken { get; set; }

        public static string TokenJenkins { get; set; }
        public static string UsuarioJenkins { get; set; }
        public static string JwtSecretKey { get; set; }

        public static string CadenaConexion
        {
            get
            {
                return string.Format("Server={0};Initial Catalog={1};Persist Security Info=False;User ID={2};Password={3};MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=0;"
                    , ServidorSQL
                    , BaseDatosSQL
                    , UsuarioSQL
                    , PasswordSQL);
            }
        }

        public static string CadenaConexionStorage
        {
            get
            {
                return string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix=core.windows.net"
                    , StorageName
                    , StorageKey);
            }
        }


    }

    public sealed class Utilitarios
    {
        #region CONSTANTES
        public static string URL_API = Get<string>("UrlApi");
        public const string GUARDICORE_API_URL = "GUARDICORE_API_URL";
        public static int SUBDOMINIO_SO = Get<int>("Subdominio.SO");
        public static int SUBDOMINIO_BD_NONRELATIONAL = Get<int>("Subdominio.BdNonRelational");
        public static int SUBDOMINIO_BD_RELATIONAL = Get<int>("Subdominio.BdRelational");
        public static List<int> SUBDOMINIO_INDICADOR_OBS = new List<int>() { SUBDOMINIO_SO, SUBDOMINIO_BD_NONRELATIONAL, SUBDOMINIO_BD_RELATIONAL };
        public const string EXTENSION_EXCEL = ".xlsx";
        public const string USERNAME_AUTO = "AUTO";
        public const string NO_APLICA = "NO APLICA";
        public const int PERMISO_CREAR = 1;
        public const int PERMISO_EDITAR = 2;
        public const int PERMISO_ELIMINAR = 3;
        public const int PERMISO_PUBLICAR = 4;
        public const int PERMISO_EXPORTAR = 5;
        public const int PERMISO_APROBAR = 6;
        public static List<string> RUTAS_SITE = new List<string>() { "", "Vista", "Relacion", "Dashboard", "GestionTecnologia", "Alertas", "Configuracion" };
        public static Random random = new Random();

        //PARAMETROS
        public const string CODIGO_SUBDOMINIO_SISTEMA_OPERATIVO = "SUBDOMINIO_SISTEMA_OPERATIVO";
        public const string CODIGO_CONSOLIDADOS_REPORTES_RUTA = "CONSOLIDADOS_REPORTES_RUTA";
        public const string CODIGO_TIPO_EQUIPO_SERVICIONUBE = "EQUIPO_SERVICIONUBE";
        public const string CODIGO_MATRICULAS_ADMIN_PORTAFOLIO = "MATRICULA_ADMINISTRADORES";

        //PARAMETROS
        public const int TIPO_EQUIPO_SERVIDOR_ID = 1;
        public const int TIPO_EQUIPO_SERVIDOR_AGENCIA_ID = 2;

        //PORTAFOLIO
        public const string CODIGO_L_CUESTIONARIO_PORTAFOLIO = "VALOR_L_CUESTIONARIO_PORTAFOLIO";
        public const string CODIGO_M_CUESTIONARIO_PORTAFOLIO = "VALOR_M_CUESTIONARIO_PORTAFOLIO";
        public const string CODIGO_N_CUESTIONARIO_PORTAFOLIO = "VALOR_N_CUESTIONARIO_PORTAFOLIO";
        public const string CODIGO_PC_CUESTIONARIO_PORTAFOLIO = "VALOR_PC_CUESTIONARIO_PORTAFOLIO";
        public const string ESTADOS_APLICACION = "ESTADOS_APLICACION";
        public const string ULTIMO_CODIGOAPT_PAE_PORTAFOLIO = "ULTIMO_CODIGO_APT_PAE";
        public const string ULTIMO_CAMPO_NUEVOID_PORTAFOLIO = "ULTIMO_CAMPO_NUEVOID_PORTAFOLIO";
        public const string NOMBRE_ARCHIVO = "Ningún archivo seleccionado";
        public const string NOMBRE_ARCHIVO_2 = "Ningun archivo seleccionado";
         
        //NOTIFICACION APPLIANCE
        public const string NOTIFICACION_DESESTIMACION_SOLICITUD_TRANSFERENCIA_OWNER = "NOTIFICACION_DESESTIMACION_SOLICITUD_TRANSFERENCIA_OWNER";
        public const string NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_OWNER = "NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_OWNER";
        public const string NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_OWNER = "NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_OWNER";
        public const string NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_CVT = "NOTIFICACION_APROBACION_SOLICITUD_TRANSFERENCIA_CVT";
        public const string NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_CVT = "NOTIFICACION_RECHAZO_SOLICITUD_TRANSFERENCIA_CVT";
        public const string NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI = "NOTIFICACION_REGISTRO_SOLICITUD_TRANSFERENCIA_TSI";
        public const string NOTIFICACION_REGISTRO_SOLICITUD_PENDIENTE_CVT = "NOTIFICACION_REGISTRO_SOLICITUD_PENDIENTE_CVT";

        //Mensaje de registro TSI - APPLIANCE
        public const string COMENTARIO_REGISTRO_TSI_APPLIANCE = "Se requiere su aprobación para continuar con el registro del appliance en referencia. Al terminar el flujo de aprobaciones se creará la relación con la aplicación seleccionada.";
        #endregion


        private static readonly Lazy<Utilitarios> lazy =
            new Lazy<Utilitarios>(() => new Utilitarios());

        public static Utilitarios Instance { get { return lazy.Value; } }
        private Utilitarios()
        {
        }
        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        

        public static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) return default(T);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

        public string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumDescription2 (Enum value)
        {
            try
            {
                if (value.ToString() == "-1")
                    return string.Empty;

                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetEnumDescription3(Enum value)
        {
            if (value.ToString() == "-1")
                return string.Empty;

            if (value.ToString() != "0")
            {
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-Z]+$"))
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());

                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                        return attributes[0].Description;
                    else
                        return value.ToString();
                } 
            }

            return value.ToString();
        }

        public static string GetValConfigByEnum(Enum value)
        {
            var key = GetEnumDescription2(value);
            var valor = Settings.Get<string>(key);
            return valor;
        }

        public byte[] StringBase64ToBytes(string valor)
        {
            return (string.IsNullOrEmpty(valor) == false ? System.Convert.FromBase64String(valor) : null);
        }


        public string BytesToStringBase64(byte[] valor)
        {
            return ((valor != null && valor.Length >= 0) ? System.Convert.ToBase64String(valor) : string.Empty);
        }

        public static int GetMonthDifference(DateTime startDate, DateTime? endDate)
        {
            int monthsApart = 0;
            if (endDate.HasValue)
            {
                monthsApart = 12 * (startDate.Year - endDate.Value.Year) + startDate.Month - endDate.Value.Month;
            }

            return Math.Abs(monthsApart);
        }


        public static byte[] ObtenerPlantillaExportar(string nomArchivo)
        {
            var fileName = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/Plantilla"), nomArchivo);

            var archivoBytes = File.ReadAllBytes(fileName);
            return archivoBytes;
        }
         
        public static byte[] ObtenerPlantillaExportar(string rutaPlantilla, string nomArchivo)
        {
            var fileName = Path.Combine(rutaPlantilla, nomArchivo);

            var archivoBytes = File.ReadAllBytes(fileName);
            return archivoBytes;
        }

        public static string GetBodyNotification()
        {
            string fileName = "notificacionestandar.txt";
            string filePath = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/TXT"), fileName);
            string body = File.ReadAllText(@filePath);

            return body;
        }

        public static void DescargarArchivo(byte[] archivoBytes, string nomArchivoDescarga, string contentType)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Length", archivoBytes.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nomArchivoDescarga));
                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.OutputStream.Write(archivoBytes, 0, archivoBytes.Length);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception)
            {

            }
        }
        public static int ObtenerValueByKey(string nombreColumna, Dictionary<string, int> dictionary)
        {
            var item = dictionary.FirstOrDefault(p => p.Key.Trim().ToUpper() == nombreColumna.Trim().ToUpper());
            return item.Value;
        }
        public static Dictionary<string, int> CargarColumnasExcelAplicaciones()
        {
            //77 campos
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("CodigoAPT", 0);// A 1
            dictionary.Add("NombreAplicacion", 1);// B 2
            dictionary.Add("CodigoInterfaz", 2);// C 3
            dictionary.Add("TipoActivoInformacion", 3);// D 4
            dictionary.Add("GerenciaCentral", 4);// E 5
            dictionary.Add("Division", 5);// F 6
            dictionary.Add("Area", 6);// G 7
            dictionary.Add("Unidad", 7);// H 8
            dictionary.Add("DescripcionAplicacion", 8);// I 9
            dictionary.Add("EstadoAplicacionProcedencia", 9);// J 10
            dictionary.Add("FechaCreacionProcedencia", 10);// K 11

            dictionary.Add("MotivoCreacion", 11);// L 12            

            dictionary.Add("AreaBIAN", 12);// M
            dictionary.Add("DominioBIAN", 13);// N
            dictionary.Add("PlataformaBCP", 14);// O
            dictionary.Add("JefaturaATI", 15);// P
            dictionary.Add("TribeTechnicalLead", 16);// Q
            dictionary.Add("MatriculaTTL", 17);// R
            dictionary.Add("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner", 18);// S
            dictionary.Add("MatriculaJDE", 19);// T
            dictionary.Add("BrokerSistemas", 20); //U

            dictionary.Add("MatriculaBroker", 21);// V
            dictionary.Add("NombreEquipo_Squad", 22);// W
            dictionary.Add("GestionadoPor", 23);// X
            dictionary.Add("Owner_LiderUsuario_ProductOwner", 24);// Y
            dictionary.Add("MatriculaOwner", 25);// Z
            dictionary.Add("Gestor_UsuarioAutorizador_ProductOwner", 26);// AA
            dictionary.Add("MatriculaGestor", 27);// AB
            dictionary.Add("Experto_Especialista", 28);// AC
            dictionary.Add("MatriculaExperto", 29);// AD
            dictionary.Add("EntidadResponsable", 30);// AE

            dictionary.Add("EntidadUsuaria", 31);// AF
            dictionary.Add("NombreEntidadUsuaria", 32);// AG
            dictionary.Add("ProcesoClave", 33);// AH
            dictionary.Add("Clasificacion", 34);// AI
            dictionary.Add("CriticidadId", 35);// AJ
            dictionary.Add("CriticidadAplicacionBIA", 36);// AK
            dictionary.Add("NuevaCriticidadFinal", 37);// AL

            dictionary.Add("ProductoMasRepresentativo", 38);// AM
            dictionary.Add("MenorRTO", 39);// AN
            dictionary.Add("MayorGradoInterrupcion", 40);// AO
            dictionary.Add("TIERProduccion", 41);// AP
            dictionary.Add("TIERPreProduccion", 42);// AQ

            dictionary.Add("CategoriaTecnologica", 43);// AR
            dictionary.Add("TipoDesarrollo", 44);// AS
            dictionary.Add("ModeloEntrega", 45);// AT
            dictionary.Add("ProveedorDesarrollo", 46);// AU
            dictionary.Add("Ubicacion", 47);// AV

            dictionary.Add("Infraestructura", 48);// AW
            dictionary.Add("ClasificacionTecnica", 49);// AX
            dictionary.Add("SubclasificacionTecnica", 50);// AY
            dictionary.Add("NombreInterface", 51);// AZ
            dictionary.Add("NombreServidor", 52);// BA
            dictionary.Add("Repositorio", 53);// BB
            dictionary.Add("Contingencia", 54);// BC
            dictionary.Add("MetodoAutenticacion", 55);// BD
            dictionary.Add("MetodoAutorizacion", 56);// BE
            dictionary.Add("CompatibleWindows7", 57);// BF
            dictionary.Add("CompatibleNavegador", 58);// BG

            dictionary.Add("CompatibleHV", 59);// BH
            dictionary.Add("InstaladaDesarrollo", 60);// BI
            dictionary.Add("InstaladaCertificacion", 61);// BJ
            dictionary.Add("InstaladaProduccion", 62);// BK
            dictionary.Add("GTR", 63);// BL
            dictionary.Add("OOR", 64);// BM
            dictionary.Add("RatificaOOR", 65);// BN
            dictionary.Add("SWBaseSO", 66);// BO
            dictionary.Add("SWBaseHP", 67);// BP
            dictionary.Add("SWBaseLP", 68);// BQ

            dictionary.Add("SWBaseBD", 69);// BR
            dictionary.Add("SWBaseFramework", 70);// BS
            dictionary.Add("RET", 71);// BT
            dictionary.Add("NCET", 72);// BU
            dictionary.Add("RLSI", 73);// BV
            dictionary.Add("NCLS", 74);// BW
            dictionary.Add("NCG", 75);// BX
            dictionary.Add("RoadMapId", 76);// BY
            dictionary.Add("DetalleEstrategiaRoadmapPlanificado", 77);// BZ
            dictionary.Add("EstadoRoadmap", 78);// CA

            dictionary.Add("EtapaAtencionRoadmap", 79);// CB
            dictionary.Add("RoadmapEjecutado", 80);// CC
            dictionary.Add("FechaInicioRoadmapEjecutado", 81);// CD
            dictionary.Add("FechaFinRoadmapEjecutado", 82);// CE
            dictionary.Add("CodigoAplicacionReemplazo", 83);// CF
            dictionary.Add("AplicativoReemplazo", 84);// CG
            dictionary.Add("FechaRegistroProcedencia", 85);// CH

            dictionary.Add("CodigoAPTPadre", 86);// CF
            dictionary.Add("TribeLeader", 87);// CG
            dictionary.Add("MatriculaTL", 88);// CH

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelAppliance()
        {
            //44 campos
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("Nombre", 0);// A 1
            dictionary.Add("TipoEquipoId", 1);// B 2
            dictionary.Add("AmbienteId", 2);// C 3
            dictionary.Add("DominioServidorId", 3);// D 4
            dictionary.Add("TipoActivo", 4);// E 5
            dictionary.Add("Serial", 5);// F 6
            dictionary.Add("Modelo", 6);// G 7
            dictionary.Add("Vendor", 7);// H 8
            dictionary.Add("Tecnologia", 8);// I 9
            dictionary.Add("Version", 9);// J 10
            dictionary.Add("Hostname", 10);// K 11
            dictionary.Add("IP", 11);// L 12                   
            dictionary.Add("Dimension", 12);// M
            dictionary.Add("ArquitectoSeguridad", 13);// N
            dictionary.Add("SoportePrimerNivel", 14);// O
            dictionary.Add("Proveedor", 15);// P

            dictionary.Add("FechaLanzamiento", 16);// Q
            dictionary.Add("FechaFinSoporte", 17);// R
            dictionary.Add("FechaFinExtendida", 18);// S
            dictionary.Add("FechaFinInterna", 19);// T
            dictionary.Add("ComentariosFechaFin", 20); //U
            dictionary.Add("FechaCalculoId", 21);// V
            dictionary.Add("FechaAdquisicion", 22);// W
            dictionary.Add("FechaImplementacion", 23);// X
            dictionary.Add("FechaUltimaRenovacion", 24);// Y
            dictionary.Add("VencimientoLicencia", 25);// Z
            dictionary.Add("CantidadLicencia", 26);// AA
            dictionary.Add("FormaLicenciamiento", 27);// AB

            dictionary.Add("CodigoInventario", 28);// AC
            dictionary.Add("CyberSOC", 29);// AD
            dictionary.Add("Sede", 30);// AE
            dictionary.Add("Sala", 31);// AF
            dictionary.Add("RACK", 32);// AG
            dictionary.Add("Ubicacion", 33);// AH

            dictionary.Add("Backup", 34);// AI
            dictionary.Add("BackupFrecuencia", 35);// AJ
            dictionary.Add("BackupDescripcion", 36);// AK
            dictionary.Add("IntegracionGestorInteligencia", 37);// AL
            dictionary.Add("ConectorSIEM", 38);// AM
            dictionary.Add("CONA", 39);// AN
            dictionary.Add("UmbralCPU", 40);// AO
            dictionary.Add("UmbralMemoria", 41);// AP
            dictionary.Add("UmbralDisco", 42);// AQ
            dictionary.Add("Criticidad", 43);// AR

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelEquipos()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("TipoEquipo", 0);// A
            dictionary.Add("Ambiente", 1);// B
            dictionary.Add("Model", 2);// C
            dictionary.Add("Dominio", 3);// D
            dictionary.Add("Equipo", 4);// E
            dictionary.Add("SO", 5);// F

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelEquiposUpdate()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("Equipo", 0);// A
            dictionary.Add("Ambiente", 1);// B
            dictionary.Add("Subsidiaria", 2);// C
            dictionary.Add("Estado", 3);// D

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelTecnologiasNoRegistradas()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("Tecnologia", 0);// A
            dictionary.Add("FechaFinSoporte", 1);// B
            dictionary.Add("FechaFinSoporteExtendido", 2);// C
            dictionary.Add("Fabricante", 3);// D
            dictionary.Add("Nombre", 4);// E
            dictionary.Add("Version", 5);// F
            dictionary.Add("Familia", 6);// F
            dictionary.Add("Tipo", 7);// F
            dictionary.Add("Dominio", 8);// F
            dictionary.Add("Subdominio", 9);// F

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelActualizarTecnologias()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("ClaveTecnologia", 0);// A
            dictionary.Add("Dominio", 1);// B
            dictionary.Add("Subdominio", 2);// C
            dictionary.Add("Estado", 3);// D
            dictionary.Add("Fabricante", 4);// E
            dictionary.Add("Nombre", 5);// F
            dictionary.Add("Version", 6);// F
            dictionary.Add("TipoTecnologia", 7);// F
            dictionary.Add("CodigoTecnologia", 8);// F
            dictionary.Add("MostrarSite", 9);// F
            dictionary.Add("CasoUso", 10);// A
            dictionary.Add("Descripcion", 11);// B
            dictionary.Add("FechaLanzamiento", 12);// C
            dictionary.Add("TieneFechaFin", 13);// D
            dictionary.Add("Fuente", 14);// E
            dictionary.Add("FechaFinSoporte", 15);// F
            dictionary.Add("FechaFinExtendida", 16);// F
            dictionary.Add("FechaFinInterna", 17);// F
            dictionary.Add("EquipoAdministracion", 18);// F
            dictionary.Add("SoporteRemedy", 19);// F
            dictionary.Add("ConformidadSeguridad", 20);// F
            dictionary.Add("ConformidadArquitecto", 21);// F
            dictionary.Add("Confluence", 22);// F

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelAplicacionesUpdate()
        {
            //75 campos
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("CodigoAPT", 0);// A 1
            dictionary.Add("NombreAplicacion", 1);// B 2
            dictionary.Add("DescripcionAplicacion", 2);// I 9
            dictionary.Add("TipoActivoInformacion", 3);// D 4
            dictionary.Add("EstadoAplicacion", 4);// J 10
            dictionary.Add("MotivoCreacion", 5);// L 12    
            dictionary.Add("FechaRegistroAplicacion", 6);// L 12 //Fecha solicitud
            dictionary.Add("PersonaSolicitud", 7);// H 8
            dictionary.Add("CategoriaTecnologica", 8);// AO
            dictionary.Add("ModeloEntrega", 9);// AQ 

            dictionary.Add("FechaCreacionAplicacion", 10);// L 12
            dictionary.Add("CodigoInterfaz", 11);// C 3
            dictionary.Add("GerenciaCentral", 12);// E 5
            dictionary.Add("Division", 13);// F 6
            dictionary.Add("Area", 14);// G 7
            dictionary.Add("Unidad", 15);// H 8
            dictionary.Add("AreaBIAN", 16);// M
            dictionary.Add("DominioBIAN", 17);// N
            dictionary.Add("PlataformaBCP", 18);// O
            dictionary.Add("JefaturaATI", 19);// P

            dictionary.Add("ArquitectoTI", 20);// M
            dictionary.Add("GestionadoPor", 21);// X
            dictionary.Add("Owner_LiderUsuario_ProductOwner", 22);// Y
            dictionary.Add("Gestor_UsuarioAutorizador_ProductOwner", 23);// AA
            dictionary.Add("Experto_Especialista", 24);// AC
            dictionary.Add("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner", 25);// S
            dictionary.Add("BrokerSistemas", 26); //U
            dictionary.Add("TribeTechnicalLead", 27);// Q
            dictionary.Add("GestorUserIT", 28);// Q
            dictionary.Add("NombreEquipo_Squad", 29);// W

            dictionary.Add("EntidadResponsable", 30);// AE
            dictionary.Add("EntidadUsuaria", 31);// AF
            dictionary.Add("TipoDesarrollo", 32);// AP
            dictionary.Add("ProveedorDesarrollo", 33);// AR
            dictionary.Add("Ubicacion", 34);// AS
            dictionary.Add("Infraestructura", 35);// AT
            dictionary.Add("Contingencia", 36);// AZ
            dictionary.Add("MetodoAutenticacion", 37);// BA
            dictionary.Add("MetodoAutorizacion", 38);// BB
            dictionary.Add("AmbienteInstalacion", 39);// AG

            dictionary.Add("OOR", 40);// BJ
            dictionary.Add("RatificaOOR", 41);// BK
            dictionary.Add("AplicativoReemplazo", 42);// CD
            dictionary.Add("ClasificacionTecnica", 43);// AU
            dictionary.Add("SubclasificacionTecnica", 44);// AV
            dictionary.Add("RutaRepositorio", 45);// AY
            dictionary.Add("GTR", 46);// BI
            dictionary.Add("NombreInterface", 47);// AW
            dictionary.Add("CompatibleWindows7", 48);// BC
            dictionary.Add("CompatibleNavegador", 49);// BD

            dictionary.Add("ProcesoClave", 50);// AH
            dictionary.Add("SWBaseSO", 51);// BL
            dictionary.Add("SWBaseHP", 52);// BM
            dictionary.Add("SWBaseLP", 53);// BN
            dictionary.Add("SWBaseBD", 54);// BO
            dictionary.Add("SWBaseFramework", 55);// BP
            dictionary.Add("RET", 56);// BQ
            dictionary.Add("GestorAplicaciones", 57);// BQ
            dictionary.Add("Consultor", 58);// BQ
            dictionary.Add("InstaladaDesarrollo", 59);// BF

            dictionary.Add("InstaladaCertificacion", 60);// BG
            dictionary.Add("InstaladaProduccion", 61);// BH
            dictionary.Add("CriticidadAplicacionBIA", 62);// AK
            dictionary.Add("ProductoMasRepresentativo", 63);// AL
            dictionary.Add("MenorRTO", 64);// AM
            dictionary.Add("MayorGradoInterrupcion", 65);// AN
            dictionary.Add("CriticidadId", 66);// AJ
            dictionary.Add("RoadMapId", 67);// BV
            dictionary.Add("DetalleEstrategiaRoadmapPlanificado", 68);// BW
            dictionary.Add("EstadoRoadmap", 69);// BX

            dictionary.Add("EtapaAtencionRoadmap", 70);// BY
            dictionary.Add("RoadmapEjecutado", 71);// BZ
            dictionary.Add("FechaInicioRoadmapEjecutado", 72);// CA
            dictionary.Add("FechaFinRoadmapEjecutado", 73);// CB
            dictionary.Add("CodigoAplicacionReemplazo", 74);// CC

            //dictionary.Add("MatriculaTTL", 17);// R
            //dictionary.Add("MatriculaJDE", 19);// T
            //dictionary.Add("MatriculaBroker", 21);// V
            //dictionary.Add("MatriculaOwner", 25);// Z
            //dictionary.Add("MatriculaGestor", 27);// AB
            //dictionary.Add("MatriculaExperto", 29);// AD
            //dictionary.Add("NombreEntidadUsuaria", 32);// AG
            //dictionary.Add("Clasificacion", 34);// AI
            //dictionary.Add("NombreServidor", 49);// AX
            //dictionary.Add("CompatibleHV", 56);// BE
            //dictionary.Add("NCET", 69);// BR
            //dictionary.Add("RLSI", 70);// BS
            //dictionary.Add("NCLS", 71);// BT
            //dictionary.Add("NCG", 72);// BU

            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelAplicacionesUpdate2()
        {
            //75 campos
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)

            dictionary.Add("applicationId", 0);// A 1

            dictionary.Add("applicationName", 1);// B 2
            dictionary.Add("interfaceId", 2);// AO
            dictionary.Add("description", 3);// I 9
            dictionary.Add("statusName", 4);// H 8
            dictionary.Add("implementationTypeName", 5);// D 4
            dictionary.Add("tipoActivoName", 6);// N
            dictionary.Add("GerenciaCentral", 7);// O
            dictionary.Add("Division", 8);// O
            dictionary.Add("Area", 9);// O
            dictionary.Add("unitName", 10);// L 12
            dictionary.Add("LiderUsuario", 11);// O
            dictionary.Add("UsuarioAutorizador", 12);// O
            dictionary.Add("managedName", 13);// J 10
            dictionary.Add("teamName", 14);// C 3
            dictionary.Add("TribeLead", 15);// O
            dictionary.Add("TribeTechnicalLead", 16);// O
            dictionary.Add("JefeEquipo", 17);// O
            dictionary.Add("BrokerSistemas", 18);// O
            dictionary.Add("Experto", 19);// C 3
            dictionary.Add("areaBIANName", 20);// O
            dictionary.Add("dominioBIANName", 21);// O
            dictionary.Add("TOBEName", 22);// O
            dictionary.Add("jefaturaATIName", 23);// P
            dictionary.Add("ArquitectoNegocio", 24);// M
            dictionary.Add("userEntityName", 25);// E 5
            dictionary.Add("criticidadBIAName", 26);// AS 
            dictionary.Add("clasificacionActivoName", 27);// AS   
            dictionary.Add("nuevaCriticidadName", 28);// AS
            dictionary.Add("starProduct", 29);// Q
            dictionary.Add("MenorRTOName", 30);// Q
            dictionary.Add("MayorGradoInterrupcionName", 31);// W
            dictionary.Add("tipoPCI", 32); //U
            dictionary.Add("tierPreProduction", 33);// AF
            dictionary.Add("tierProduction", 34);// AP 
            dictionary.Add("deploymentTypeName", 35);// L 12 
            dictionary.Add("categoriaTecnologicaName", 36);// M
            dictionary.Add("webDomain", 37);// S
            dictionary.Add("clasificacionTecnicaName", 38);// Y 
            dictionary.Add("subClasificacionTecnicaName", 39);// X
            dictionary.Add("developmentTypeName", 40);// F 6
            dictionary.Add("developmentProvider", 41);// G 7
            dictionary.Add("infrastructureName", 42);// H 8
            dictionary.Add("authenticationMethodName", 43);// AS  
            dictionary.Add("authorizationMethodName", 44);// AR
            dictionary.Add("grupoTicketRemedyName", 45);// AC
            dictionary.Add("resumenName", 46);// AS                 
            dictionary.Add("nivelCumplimientoName", 47);// AS 
            dictionary.Add("FlagPirata", 48);// AP 
            dictionary.Add("FechaFlagPirata", 49);// AP 
            dictionary.Add("parentAPTCode", 50);//
            dictionary.Add("replacementApplication", 51);// M

            dictionary.Add("Solicitante", 52);// AS  
            dictionary.Add("registerDate", 53);// AQ  


            dictionary.Add("SituacionRegistro", 54);// AP 


            dictionary.Add("dateFirstRelease", 55); //U

         



            return dictionary;
        }

        public static Dictionary<string, int> CargarColumnasExcelAplicacionesUpdate3()
        {
            //75 campos
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // ( NOMBRE COLUMNA, POSICIÓN COLUMNA)
            dictionary.Add("applicationId", 0);// A 1
            dictionary.Add("applicationName", 1);// B 2
            dictionary.Add("description", 2);// I 9
            dictionary.Add("interfaceId", 3);// AO
            dictionary.Add("parentAPTCode", 4);// L 12 //Fecha solicitud
            dictionary.Add("gerencia", 5);// D 4
            dictionary.Add("division", 6);// J 10
            dictionary.Add("area", 7);// L 12    
            dictionary.Add("unity", 8);// H 8
            dictionary.Add("managed", 9);// AQ 
            dictionary.Add("implementationType", 10);// L 12

            dictionary.Add("deploymentType", 11);// C 3
            dictionary.Add("status", 12);// E 5
            dictionary.Add("teamName", 13);// F 6
            dictionary.Add("userEntity", 14);// G 7
            dictionary.Add("developmentType", 15);// H 8
            dictionary.Add("developmentProvider", 16);// M
            dictionary.Add("replacementApplication", 17);// N
            dictionary.Add("infrastructure", 18);// O
            dictionary.Add("authenticationMethod", 19);// P
            dictionary.Add("authorizationMethod", 20);// M

            dictionary.Add("groupTicketRemedy", 21);// X
            dictionary.Add("webDomain", 22);// Y
            dictionary.Add("complianceLevel", 23);// AA
            dictionary.Add("summaryStandard", 24);// AC
            dictionary.Add("assetType", 25);// S
            dictionary.Add("BIANArea", 26); //U
            dictionary.Add("BIANdomain", 27);// Q
            dictionary.Add("mainOffice", 28);// Q
            dictionary.Add("tobe", 29);// W
            dictionary.Add("technologyCategory", 30);// AE

            dictionary.Add("technicalSubclassification", 31);// AF
            dictionary.Add("technicalClassification", 32);// AP
            dictionary.Add("tierProduction", 33);// AR
            dictionary.Add("tierPreProduction", 34);// AS
            dictionary.Add("applicationCriticalityBIA", 34);// AS
            dictionary.Add("classification", 36);// AT
            dictionary.Add("finalCriticality", 37);// M
            dictionary.Add("starProduct]", 38);// X
            dictionary.Add("shorterApplicationResponseTime", 39);// Y
            dictionary.Add("highestDegreeInterruption", 40);// AA

            dictionary.Add("dateFirstRelease", 41);// AC
            dictionary.Add("tribeTechnicalLeadId", 42);// S
            dictionary.Add("tribeTechnicalLeadNombre", 43); //U
            dictionary.Add("tribeTechnicalLeadEmail", 44);// Q
            dictionary.Add("jefeDeEquipoId", 45);// Q
            dictionary.Add("jefeDeEquipoNombre", 46);// W
            dictionary.Add("jefeDeEquipoEmail", 47);// M
            dictionary.Add("brokerId", 48);// X
            dictionary.Add("brokerNombre", 49);// Y
            dictionary.Add("brokerEmail", 50);// AA

            dictionary.Add("ownerId", 51);// AC
            dictionary.Add("ownerNombre", 52);// S
            dictionary.Add("ownerEmail", 53); //U
            dictionary.Add("gestorId", 54);// Q
            dictionary.Add("gestorNombre", 55);// Q
            dictionary.Add("gestorEmail", 56);// W
            dictionary.Add("expertoId", 57);// X
            dictionary.Add("expertoNombre", 58);// Y
            dictionary.Add("expertoEmail", 59);// AA
            dictionary.Add("tribeLeadId", 60);// AC

            dictionary.Add("tribeLeadNombre", 61);// S
            dictionary.Add("tribeLeadEmail", 62); //U
            dictionary.Add("solicitanteId", 63);// Q
            dictionary.Add("solicitanteNombre", 64);// Q
            dictionary.Add("solicitanteEmail", 65);// W
            dictionary.Add("arquitectoEvaluadorId", 66);// Q
            dictionary.Add("arquitectoEvaluadorNombre", 67);// Q
            dictionary.Add("arquitectoEvaluadorEmail", 68);// W
            return dictionary;
        }


        public static string DevolverNombreFecha(DateTime fecha)
        {
            var mes = GetEnumDescription2((Mes)fecha.Month);
            return string.Format("{0} del {1}", mes, fecha.Year.ToString());
        }

        public static List<string> ObternerRutasByPerfil(int PerfilId)
        {
            //Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();

            var retorno = PerfilId == (int)EPerfilBCP.Administrador ? RUTAS_SITE : RUTAS_SITE.Where(x => x != "GestionTecnologia" && x != "Alertas").ToList();

            //dictionary.Add(1, RUTAS_SITE); //Perfil Administrador
            //dictionary.Add(2, RUTAS_SITE.Where(x => x != "GestionTecnologia" && x != "Alertas").ToList()); //Perfil Consultor

            return retorno;
        }

        public static Func<T, T> DynamicSelectGenerator<T>(string Fields = "")
        {
            string[] EntityFields;
            if (Fields == "")
                // get Properties of the T
                EntityFields = typeof(T).GetProperties().Select(propertyInfo => propertyInfo.Name).ToArray();
            else
                EntityFields = Fields.Split(',');

            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = EntityFields.Select(o => o.Trim())
                .Select(o =>
                {

                    // property "Field1"
                    var mi = typeof(T).GetProperty(o);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }

        public static string ObtenerLetraExcelByPos(int pos)
        {
            string letraExcel = "";
            int posAlt = 0;

            //A
            if (pos >= 1 && pos <= 26)
                letraExcel = GetEnumDescription2((ELetraColumnaExcel)pos);

            //AA
            if (pos > 26 && pos <= 52)
            {
                posAlt = pos - 26;
                letraExcel = string.Format("A{0}", GetEnumDescription2((ELetraColumnaExcel)posAlt));
            }

            //BA
            if (pos > 52 && pos <= 78)
            {
                posAlt = pos - (26 * 2);
                letraExcel = string.Format("B{0}", GetEnumDescription2((ELetraColumnaExcel)posAlt));
            }

            //CA
            if (pos > 78 && pos <= 104)
            {
                posAlt = pos - (26 * 3);
                letraExcel = string.Format("C{0}", GetEnumDescription2((ELetraColumnaExcel)posAlt));
            }

            return letraExcel;
        }

        public static string DevolverTecnologiaEstandarStr(string UrlConlfuence, string Codigo, string Nombre, string className, bool? flagMostrarEstado, string EstadoId, int tecnologiaId)
        {
            string retorno = "";
            string tecnologiaStr = "";
            string estadoStr = EstadoId;
            string estadoTecnologia = flagMostrarEstado.HasValue ? flagMostrarEstado.Value ? estadoStr : "" : "";
            estadoTecnologia = !string.IsNullOrEmpty(estadoTecnologia) ? $"({estadoTecnologia})" : "";

            if (!string.IsNullOrEmpty(Codigo))
            {
                //if (!string.IsNullOrEmpty(UrlConlfuence))
                //{
                //    //retorno = $"<a href='{UrlConlfuence}' target='_blank' title='Ir a url' class='{className}'>[{Codigo}] {Nombre} {estadoTecnologia}</a>";
                //    retorno = $"<a href='javascript: verDetalleEstandar({tecnologiaId})' title='Ver detalle estándar' class='{className}'>[{Codigo}] {Nombre} {estadoTecnologia}</a>";
                //}
                //else
                //{
                //    //retorno = $"[{Codigo}] {Nombre} {estadoTecnologia}";
                //    retorno = $"<a href='javascript: verDetalleEstandar({tecnologiaId})' title='Ver detalle estándar' class='{className}'>[{Codigo}] {Nombre} {estadoTecnologia}</a>";
                //}
                tecnologiaStr = $"[{Codigo}] {Nombre} {estadoTecnologia}";
                retorno = $"<a href='javascript: verDetalleEstandar({tecnologiaId})' title='Ver detalle estándar' class='{className}'>{tecnologiaStr}</a>";
            }
            else
            {
                //retorno = $"{Nombre} {estadoTecnologia}";
                tecnologiaStr = $"{Nombre} {estadoTecnologia}";
                retorno = $"<a href='javascript: verDetalleEstandar({tecnologiaId})' title='Ver detalle estándar' class='{className}'>{tecnologiaStr}</a>";
            }

            return retorno;
        }

        public static decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        public static double TruncateDouble(double value, int precision)
        {
            double step = Math.Pow(10, precision);
            double tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        public static int GetPosByLetter(string letter)
        {
            var lLettersList = EnumToList<ELetraColumnaExcel>();
            var lListado = lLettersList.Select(x => new { Id = (int)x, Descripcion = GetEnumDescription2(x) }).ToList();
            var retorno = lListado.FirstOrDefault(x => x.Descripcion.ToUpper().Equals(letter.ToUpper()));
            return retorno != null ? retorno.Id : 1;
        }

        public static int GetIntegerDigitCountString(int value)
        {
            return value.ToString().Length;
        }

        public static string GetStringDefault(string str) => string.IsNullOrWhiteSpace(str) ? string.Empty : str.Trim();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }

        public static string FullStr(string text) => string.IsNullOrWhiteSpace(text) ? string.Empty : text;

        public static string GetReplaceCaracterSpecial(string texto)
        {
            string newValor = texto ?? "";

            if (!string.IsNullOrEmpty(newValor))
            {
                string[] caracteres = new string[] { "" };
                for (var i = 0; i < caracteres.Length; i++)
                {
                    newValor = texto.Replace(caracteres[i], "");
                }
            }
            return newValor;
        }
    }
}
