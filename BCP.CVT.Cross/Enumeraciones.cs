using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross
{

    public enum TippRegistroDato
    {
        [Description("Ninguna")]
        Ninguna = -1,
        [Description("Obligatorio")]
        Obligatorio = 1,
        [Description("Opcional")]
        Opcional = 2
    }
    public enum NivelConfiabilidad
    {
        [Description("Ninguna")]
        Ninguna = -1,
        [Description("Alto")]
        Alto = 1,
        [Description("Medio")]
        Medio = 2,
        [Description("Bajo")]
        Bajo = 3
    }

    public enum ModoLlenado
    {
        [Description("Ninguna")]
        Ninguna = -1,
        [Description("Automático")]
        Automatico = 1,
        [Description("Manual")]
        Manual = 2,
        [Description("Manual - Automático")]
        ManualAutomatico = 3
    }
    public enum ActivoAplica
    {
        [Description("Ninguna")]
        Ninguna = -1,
        [Description("Para Todas")]
        ParaTodas = 1,
        [Description("Solo APP IT")]
        SoloAPPIT = 2,
        [Description("Solo User IT")]
        SoloUserIT = 3
    }
    public enum Alerta
    {
        [Description("Registro de una tecnología con estado pendiente")]
        TecnologiaEstadoPendiente = 1,
        [Description("Presencia de una tecnología sin equivalencia en el catálogo")]
        TecnologiaSinEquivalencia = 2,
        [Description("Equipos sin sistema operativo")]
        EquipoSinSO = 3,
        [Description("Equipos sin tecnologías instaladas")]
        EquipoSinTecnologia = 4,
        [Description("Equipos (del tipo servidor) sin relación a aplicaciones del portafolio")]
        EquipoSinRelacionAplicaciones = 5,
        [Description("Procesamiento de información desde ADDM/Remedy")]
        NoAddmRemedy = 6,
        [Description("Procesamiento de información desde SNOW")]
        NoSnow = 7,
        [Description("Procesamiento del portafolio de aplicaciones")]
        NoPortafolioAplicaciones = 8,
        [Description("Procesamiento de los incidentes asociados a obsolescencia")]
        NoIncidentes = 9,
        [Description("Procesamiento de las vulnerabilidades de las tecnologías")]
        NoVulnerabilidades = 10,
        [Description("Tecnologías sin fechas de soporte configuradas en la solución")]
        TecnologiasSinFechaSoporte = 11,
        [Description("Procesamiento de las IPs")]
        NoIP = 12,
        [Description("Procesamiento de Storage")]
        NoStorage = 13,
        [Description("Riesgos")]
        NoRiesgos = 14,
        [Description("Procesamiento de archivos Azure")]
        Paas = 15,
        [Description("Equipos (del tipo servicio en nube) sin relación a aplicaciones del portafolio")]
        ServicioEnNubeSinRelacionAplicaciones = 16,
        [Description("Url sin relacion con aplicativo")]
        UrlHuerfanas = 17,
        [Description("Equipos no registrados en el catalogo")]
        EquipoNoRegistrado = 18,
        [Description("Generación de Reporte Qualys Consolidado")]
        QualysReporteConsolidado = 19
    }

    public enum AlertaCriticidad
    {
        Critico = 1,
        NoCritico = 0
    }

    public enum TipoDato
    {
        Cadena = 1,
        Numero = 2,
        Formula = 3
    }

    public enum EstadoTecnologia
    {
        [Description("Registrado")]
        Registrado = 1,
        [Description("En proceso de revisión")]
        EnRevision = 2,
        [Description("Aprobado")]
        Aprobado = 3,
        [Description("Observado")]
        Observado = 4
    }
    public enum ETipoTarea
    {
        CargaDatos = 1,
        CargaIncidentes = 2,
        CargaVulnerabilidades = 3
    }
    public enum EResultadoEjecucion
    {
        Pendiente = 1,
        Ok = 2,
        Error = 3
    }

    public enum Fuente
    {
        [Description("ADDM")]
        ADDM = 1,
        [Description("SNOW")]
        SNOW = 2,
        [Description("Manual")]
        Manual = 3
    }

    public enum FechaCalculoTecnologia
    {
        //[Description("Fecha de lanzamiento")]
        //FechaLanzamiento = 1,
        [Description("Fecha extendida")]
        FechaExtendida = 2,
        [Description("Fecha fin de soporte")]
        FechaFinSoporte = 3,
        [Description("Fecha interna")]
        FechaInterna = 4
    }

    public enum ETipoFechaInterna
    {
        [Description("Fecha Interna 1")]
        FechaInterna1 = 1,
        [Description("Fecha Interna 2")]
        FechaInterna2 = 2,
    }

    public enum ETecnologiaTipo
    {
        [Description("Estándar")]
        Estandar = 1,
        [Description("No Estándar")]
        NoEstandar = 4,
        [Description("Estándar Restringido")]
        EstandarRestringido = 3
    }
    public enum ETecnologiaEstado
    {
        //[Description("Por asignar")]
        //PorAsignar = -1,
        [Description("Vigente")]
        Vigente = 1,
        //[Description("Vigente (En salida/Por Vencer)")]
        //VigentePorVencer = 2,
        [Description("Deprecado")]
        Deprecado = 2,
        [Description("Obsoleto")]
        Obsoleto = 3,
        [Description("Restringido")]
        Restringido = 4
    }

    public enum ETecnologiaEstadoEstandar
    {
        [Description("Por asignar")]
        PorAsignar = -1,
        [Description("Vigente")]
        Vigente = 1,
        [Description("Vigente (En salida/Por Vencer)")]
        VigentePorVencer = 15,
        [Description("Deprecado")]
        Deprecado = 2,
        [Description("Deprecado (En salida/Por Vencer)")]
        DeprecadoPorVencer = 25,
        [Description("Obsoleto")]
        Obsoleto = 3,
        [Description("Restringido")]
        Restringido = 4,
        [Description("Restringido (En salida/Por Vencer)")]
        RestringidoPorVencer = 45,
    }

    public enum ETipoTecnologia
    {
        Estandar = 1,
        NoEstandar = 4
    }

    public enum EFechaFinSoporte
    {
        [Description("Si")]
        Si = 1,
        [Description("No")]
        No = 2
    }

    public enum ETipoAlerta
    {
        [Description("Técnica")]
        Tecnica = 1,
        [Description("Funcional")]
        Funcional = 2
    }
    public enum ETipoFrecuencia
    {
        [Description("Diaria")]
        Diaria = 1,
        [Description("Semanal")]
        Semanal = 2,
        [Description("Quincenal")]
        Quincenal = 3,
        [Description("Mensual")]
        Mensual = 4,
        [Description("Trimestral")]
        Trimestral = 5,
        [Description("Semestral")]
        Semestral = 6,
        [Description("A demanda")]
        ADemanda = 7
    }
    public enum EAlertaFuncional
    {
        AlertaFuncional1 = 1,
        AlertaFuncional2 = 2,
        AlertaFuncional3 = 3,
        AlertaFuncional4 = 4,
        AlertaFuncional5 = 5,
        AlertaFuncional6 = 11,
        AlertaFuncional7 = 16,
        AlertaFuncional8 = 17,
        AlertaFuncional9 = 18
    }

    public enum EEquipoDescubierto
    {
        NoAplica = -1,
        [Description("Descubierto automáticamente")]
        Automaticamente = 0,
        [Description("Manual")]
        Manual = 1,
        [Description("Descubierto automáticamente")]
        Valor2 = 2,
        [Description("Descubierto automáticamente")]
        Valor3 = 3

    }

    public enum EDescubrimiento
    {
        [Description("Descubierto automáticamente")]
        Automaticamente = 0,
        [Description("Carga manual - CVT")]
        Manual = 1
    }

    public enum EDescubrimientoAnterior
    {
        NoAplica = -1,
        [Description("Descubierto automáticamente")]
        Automaticamente = 0,
        [Description("Carga manual")]
        Manual = 1
    }

    public enum EDias
    {
        [Description("Lunes")]
        Lunes = 1,
        [Description("Martes")]
        Martes = 2,
        [Description("Miercoles")]
        Miercoles = 3,
        [Description("Jueves")]
        Jueves = 4,
        [Description("Viernes")]
        Viernes = 5,
        [Description("Sabado")]
        Sabado = 6,
        [Description("Domingo")]
        Domingo = 7
    }

    #region GESTION_EQUIPOS

    public enum EEstadoCalculo
    {
        [Description("Si")]
        Si = 1,
        [Description("No")]
        No = 0
    }

    public enum ECaracteristicaEquipo
    {
        [Description("Equipo Físico")]
        Fisico = 1,
        [Description("Equipo Virtual")]
        Virtual = 2,
        [Description("Servicio PaaS")]
        PaaS = 3
    }

    public enum EControlAprovisionamientoEquipo
    {
        [Description("Estándar")]
        Regular = 1,
        [Description("Temporal")]
        Temporal = 2,
        [Description("Autogestionado")]
        Autogestionado = 3
    }

    #endregion



    #region Relacion
    public enum ETipoRelacion
    {
        [Description("Equipo")]
        Equipo = 1,
        [Description("Tecnología")]
        Tecnologia = 2,
        [Description("Otra aplicación")]
        Aplicacion = 3,
        [Description("Servicio en la nube")]
        ServicioNube = 4,
        [Description("Servicio Broker")]
        ServicioBroker = 5,
        [Description("Api")]
        Api = 6,
        [Description("Certificados Digitales (CA)")]
        CertificadoDigitales = 7,
        [Description("Certificados Digitales & Client Secrets (Cloud)")]
        ClientSecrets = 8
    }
    public enum ERelevancia
    {
        [Description("Alta")]
        Alta = 1,
        [Description("Media")]
        Media = 2,
        [Description("Baja")]
        Baja = 3,
        [Description("Irrelevante")]
        Irrelevante = 4
    }
    public enum EEstadoRelacion
    {
        [Description("Pendiente")]
        Pendiente = 1,
        [Description("Aprobado")]
        Aprobado = 2,
        [Description("Desaprobado")]
        Desaprobado = 3,
        [Description("Pendiente de Eliminación")]
        PendienteEliminacion = 0,
        [Description("Eliminado")]
        Eliminado = -1,
        [Description("Suspendido")]
        Cuarentena = 6
    }
    public enum ETipoCargaMasiva
    {
        Aplicacion = 1,
        RoadMap = 2,
        Equipo = 3,
        UpdateEquipo = 4,
        TecnologiaNoRegistrada = 5,
        Appliance = 6,
        Tecnologia = 7,
        UpdateAplicacion = 8,
        UpdateAplicacionHistorico = 9
    }
    public enum ERelacionAplicacion
    {
        [Description("Primaria")]
        Primario = 1,
        [Description("Secundaria")]
        Secundario = 2
    }
    public enum ETipoRelacionComponente
    {
        [Description("Dependencia")]
        Dependencia = 1,
        [Description("Relación")]
        Relación = 3
    }
    #endregion
    #region Excepción
    public enum ETipoRiesgo : int
    {
        [Description("Permanente")]
        Permanente = 1,
        [Description("Temporal")]
        Temporal = 2
    }
    public enum ETipoExcepcion : int
    {
        [Description("Riesgo")]
        Riesgo = 1,
        [Description("Tipo")]
        Tipo = 2
    }
    #endregion
    #region Procedencia
    public enum ETablaProcedencia : int
    {
        [Description("CMDB_SERVIDOR")]
        CMDB_SERVIDOR = 1,
        [Description("CVT_Arquetipo")]
        CVT_Arquetipo = 2,
        [Description("CVT_Excepcion")]
        CVT_Excepcion = 3,
        [Description("CVT_Tecnologia")]
        CVT_Tecnologia = 4,
        [Description("CVT_Otros")]
        CVT_Otros = 5,
        [Description("CVT_Aplicacion")]
        CVT_Aplicacion = 6,
        [Description("CVT_Producto")]
        CVT_Producto = 7
    }
    #endregion
    #region DashboardTecnologia
    public enum EDashboardEstadoTecnologia
    {
        [Description("Cerca de fin de soporte")]
        CercaFinSoporte = 1,
        [Description("No vigente")]
        NoVigente = 2,
        [Description("Vigente")]
        Vigente = 3,
        [Description("Sin información")]
        SinInformacion = 4
    }

    public enum EDashboardEstadoTecnologiaColor
    {
        [Description("#fbee4d")]
        CercaFinSoporte = 1,
        [Description("#dc6967")]
        NoVigente = 2,
        [Description("#7ddc67")]
        Vigente = 3,
        [Description("#e0e0e0")]
        SinInformacion = 4
    }

    public enum ColoresSemaforo
    {
        Verde = 1,
        Amarillo = 0,
        Rojo = -1
    }

    public enum Agrupacion
    {
        [Description("Gerencia central")]
        GerenciaCentral = 1,
        [Description("Division")]
        Division = 2,
        [Description("Soportado por - Tribu")]
        SoportadoPor = 3,
        [Description("Jefe de equipo")]
        JefeEquipo = 4,
        [Description("Owner - LiderUsuario - Product Owner")]
        LiderUsuario = 5,
        [Description("Gestor - Usuario Autorizador - Product Owner")]
        Gestor = 6,
        [Description("Tipo de activo de información")]
        TipoActivo = 7
    }
    #endregion
    public enum ETipoNotificacion
    {
        [Description("Alerta Crítica - Portafolio")]
        AlertaCriticaPortafolio = 1,
        [Description("Alerta Crítica - SNOW/ADDM")]
        AlertaCritcaSNOWADDM = 2,
        [Description("Alerta No Crítica")]
        AlertaNoCritica = 3,
        [Description("Sincronización de responsables")]
        SincronizacionResponsables = 4,
        [Description("Tecnología sin fecha fin e indefinida")]
        TecnologiaSinFecha = 5,
        [Description("Roadmap de tecnologías")]
        RoadmapTecnologias = 6,
        [Description("Relaciones pendientes de aprobación")]
        RelacionesPendientes = 7,
        [Description("Usuarios deshabilitados")]
        UsuariosDeshabilitados = 8,
        [Description("Servidores huérfanos")]
        ServidoresHuerfanos = 9,
        [Description("Servicios en nube huérfanos")]
        ServiciosEnNubeHuerfanos = 10,
        [Description("Tecnologías huérfanas")]
        TecnologiasHuerfanas = 11,
        [Description("Tecnologías no registradas")]
        TecnologiaNoRegistrada = 12,
        [Description("Riesgo de obsolescencia de aplicaciones")]
        RiesgoObsolescenciaAplicaciones = 13,
        [Description("Tecnologías con estado pendiente")]
        TecnologiaPendiente = 14,
        [Description("Equipos sin tecnologías asociadas")]
        EquiposSinTecnologias = 15,
        [Description("Alerta de estándares")]
        AlertaEstandares = 16,
        [Description("Alerta de tecnologías vencidas o por vencer usadas por las aplicaciones")]
        AlertaTecnologia = 17,
        [Description("Validación técnica de aplicación por expertos")]
        ValidacionTecnicaExpertos = 18,
        [Description("Alerta Crítica - IP")]
        AlertaIP = 19,
        [Description("Alerta Crítica - Storage")]
        AlertaStorage = 20,
        [Description("Alerta Crítica - Recursos Azure")]
        AlertaRecursosAzure = 21,
        [Description("Alerta Crítica - Archivo de Riesgos")]
        AlertaRiesgos = 22,
        [Description("Registro de solicitud de transferencia Activo TSI")]
        RegistroSolicitudTSI = 29,
        [Description("Desestimación de solicitud de transferencia Activo TSI")]
        DesestimacionSolicitudTSI = 30,
        [Description("Aprobación de solicitud de transferencia Activo TSI")]
        AprobacionSolicitudTSI = 31,
        [Description("Rechazo de solicitud de transferencia Activo TSI")]
        RechazoSolicitudTSI = 32
    }
    public enum PoliticasAzman : int
    {
        OP_Consulta = 0,
        OP_Detalle = 1,
        OP_Mantenimiento = 2,
        OP_Ejecucion = 3,
        OP_PrincipalAdministrador = 4,
        OP_PrincipalConsultor = 5,
        OP_PrincipalOperador = 6,
        OP_PrincipalCoordinador = 7,
        OP_PrincipalETI_CMDB = 8,
        OP_PrincipalArquitectoTecnologia = 9,
        OP_PrincipalGestorTecnologia = 10,
        OP_PrincipalSubsidiaria = 11,
        OP_PrincipalSeguridad = 12,
        OP_PrincipalGerente = 13,
        OP_PrincipalAuditoria = 14

    }
    public enum Mes
    {
        [Description("Enero")]
        Enero = 1,
        [Description("Febrero")]
        Febrero = 2,
        [Description("Marzo")]
        Marzo = 3,
        [Description("Abril")]
        Abril = 4,
        [Description("Mayo")]
        Mayo = 5,
        [Description("Junio")]
        Junio = 6,
        [Description("Julio")]
        Julio = 7,
        [Description("Agosto")]
        Agosto = 8,
        [Description("Setiembre")]
        Setiembre = 9,
        [Description("Octubre")]
        Octubre = 10,
        [Description("Noviembre")]
        Noviembre = 11,
        [Description("Diciembre")]
        Diciembre = 12
    }

    public enum EAccion
    {
        [Description("Inserción (I)")]
        Insercion = 1,
        [Description("Actualización (U)")]
        Actualizacion = 2
    }

    public enum EPerfilBCP
    {
        [Description("Administrador")]
        Administrador = 1,
        [Description("Consultor")]
        Consultor = 2,
        Operador = 3,
        Coordinador = 4,
        ETICMDB = 5,
        ArquitectoTecnologia = 6,
        GestorTecnologia = 7,
        Subsidiaria = 8,
        Seguridad = 9,
        Gerente = 10,
        Auditoria = 11,
        NoAutorizado = 12,
        ArquitectoFuncional = 13,
        ArquitectoSeguridad = 14,
        PortafolioAplicaciones = 15,
        SuperAdmin = 16,
        MDR_Owner_Producto = 17,
        MDR_Owner_Tribu = 18,
        MDR_Owner_Chapter = 19,
        GestorUnit = 20,
        GestorCVT_CatalogoTecnologias = 21
    }

    public enum ETipoMensaje
    {
        [Description("Sugerencia")]
        Sugerencia = 1,
        [Description("Consulta")]
        Consulta = 2,
        [Description("Soporte")]
        Soporte = 3
    }

    public enum ETipoExperto
    {
        JefeEquipoPO = 1,
        ExpertoAplicacion = 2,
        LiderTribuTTL = 3,
        SupervidorBCP = 4,
        AnalistaCDS = 8,
        ChapterQA = 9,
        LiderUsuario = 10,
        LiderUsuarioAutorizador = 11,
        ChapterDesarrollo = 12,
        LiderTecnico = 13,
        SupervidorProveedor = 14,

        Proveedor = 5,
        AprobadorCDS = 6,
        JefeCDS = 7
    }

    public enum ETipoConsolidado
    {
        [Description("Aplicaciones")]
        Aplicaciones = 1,
        [Description("Tecnologias")]
        Tecnologias = 2,
        [Description("Relaciones")]
        Relaciones = 3,
        [Description("Servidores")]
        Servidores = 4
    }

    public enum ENombreConsolidado
    {
        [Description("ConsolidadoAplicaciones.csv")]
        Aplicaciones = 1,
        [Description("ConsolidadoTecnologias.csv")]
        Tecnologias = 2,
        [Description("ConsolidadoRelaciones.csv")]
        Relaciones = 3,
        [Description("ConsolidadoEquipos.csv")]
        Servidores = 4
    }

    public enum ETipoConsulta
    {
        [Description("Todos")]
        Todos = 1,
        [Description("Tecnologias en equipos")]
        TecnologiasEquipos = 2,
        [Description("Tecnologias en aplicaciones")]
        TecnologiasAplicaciones = 3
    }

    public enum ETipoEquipo
    {
        Ninguno = 0,
        [Description("Servidor")]
        Servidor = 1,
        [Description("Servidor de agencia")]
        ServidorAgencia = 2,
        [Description("Computador personal (PC)")]
        PC = 3,
        [Description("Servicio en la nube")]
        ServicioNube = 4,
        [Description("Storage")]
        Storage = 5,
        [Description("Appliance")]
        Appliance = 6,
        [Description("Red y comunicación")]
        RedComunicacion = 7
    }

    public enum ETipoActivoInformacion
    {
        [Description("APPIT")]
        APPIT = 1,
        [Description("APPIT - Herramienta")]
        APPITHerramienta = 2,
        [Description("APPIT - Información (DATA)")]
        APPITInformación = 3,
        [Description("APPIT - Negocio")]
        APPITNegocio = 4,
        [Description("APPIT - Táctico")]
        APPITTactico = 5,
        [Description("USER IT")]
        UserIT = 6
    }

    public enum EGerenciaCentral
    {
        [Description("BCP BOLIVIA")]
        BCPBolivia = 1,
        [Description("CREDICORP CAPITAL")]
        CredicorpCapital = 2,
        [Description("G. CENTRAL COMER. Y DE SEGM BANCA MINORISTA")]
        GCentralComercial = 3,
        [Description("G. CENTRAL DE BANCA MAYORISTA")]
        GCentralBancaMayorista = 4,
        [Description("G. CENTRAL DE BANCA MINORISTA")]
        GcentralBancaMinorista = 5,
        [Description("G. CENTRAL DE BANCA MINORISTA DE SEGMENTOS Y CANALES COMERCIALES")]
        GCentralBancaMinoristaSegmentos = 6,
        [Description("G. CENTRAL DE PLANEAMIENTO Y FIN")]
        GCentralPlaneamientoFin = 7,
        [Description("G. CENTRAL DE PLANEAMIENTO Y FINANZAS")]
        GCentralPlaneamientoFinanzas = 8,
        [Description("G. CENTRAL DE RIESGOS")]
        GCentralRiesgos = 9,
        [Description("G. CENTRAL DE TRANSFORMACIÓN")]
        GCentralTransformacion = 10,
        [Description("GERENCIA GENERAL")]
        GerenciaGeneral = 11,
        [Description("PRIMA AFP")]
        PrimaAFP = 12,
        [Description("STAFF")]
        Staff = 13
    }

    public enum EMotivoCreacion
    {
        [Description("Administración ADS")]
        AdministracionADS = 1,
        [Description("App Bolsa")]
        AppBolsa = 2,
        [Description("Automatización de proceso")]
        AutoProceso = 3,
        [Description("Blanqueamiento")]
        Blanqueamiento = 4,
        [Description("Consolidación")]
        Consolidacion = 5,
        [Description("Estrategia de Negocio")]
        EstrategiaNegocio = 6,
        [Description("Estrategia Tecnologica")]
        EstrategiaTecnologica = 7,
        [Description("Excepcion DWH")]
        ExcepcionDWH = 8,
        [Description("Mejora operativa")]
        MejoraOperativa = 9,
        [Description("Necesidad Usuaria")]
        NecesidadUsuaria = 10,
        [Description("Regulatorio")]
        Regulatorio = 11,
        [Description("Renovación Tecnológica")]
        RenovacionTecnologica = 12,
        [Description("Sin Motivo")]
        SinMotivo = 13
    }

    public enum ECategoriaTecnologica
    {
        [Description("App Movil")]
        AppMovil = 1,
        [Description("Digitadores Host")]
        DigitadoresHost = 2,
        [Description("Digitadores Web")]
        DigitadoresWeb = 3,
        [Description("Escritorio (C/S)")]
        Escritorio = 4,
        [Description("Escritorio (C/S), Mainframe")]
        EscritorioMainframe = 5,
        [Description("Escritorio (C/S), Web")]
        EscritorioWeb = 6,
        [Description("Escritorio (C/S), Web, Mainframe")]
        EscritorioWebMainframe = 7,
        [Description("Formularios Sharepoint")]
        FormulariosSharepoint = 8,
        [Description("Macros (VBA)")]
        Macros = 9,
        [Description("Mainframe")]
        Mainframe = 10,
        [Description("Modelos de Información")]
        ModelosInformacion = 11,
        [Description("Motores SQL")]
        MotoresSQL = 12,
        [Description("Otro")]
        Otro = 13,
        [Description("Paquete")]
        Paquete = 14,
        [Description("Web")]
        Web = 15,
        [Description("Web, Cliente SSH")]
        WebClienteSSH = 16,
        [Description("Web, Mainframe")]
        WebMainframe = 17,
        [Description("Web, App Móvil")]
        WebAppMovil = 18,
        [Description("Sin Categoria")]
        SinCategoria = 19
    }

    public enum EModeloEntrega
    {
        [Description("DEVSECOPS")]
        DEVSECOPS = 1,
        [Description("HIBRIDO")]
        HIBRIDO = 2,
        [Description("LEGACY")]
        LEGACY = 3,
        [Description("NO APLICA")]
        NoAplica = 4
    }


    public enum EPlataformaBCP
    {
        [Description("ATLAS")]
        ATLAS = 1,
        [Description("BACKOFFICE DIGITAL")]
        BACKOFFICEDIGITAL = 2,
        [Description("LEGACY")]
        LEGACY = 3,
        [Description("BANCA DIGITAL CIX")]
        BANCADIGITALCIX = 4,
        [Description("BANCA DIGITAL EMPRESARIAL WEB / MÓVIL")]
        BANCADIGITALEMPRESARIAL = 5,
        [Description("BANCA DIGITAL HOMEBANKING")]
        BANCADIGITALHOMEBANKING = 6,
        [Description("BANCA DIGITAL MOBILE")]
        BANCADIGITALMOBILE = 7,
        [Description("BANCA DIGITAL PYME Y NEGOCIOS")]
        BANCADIGITALPYME = 8,
        [Description("BANCA DIGITAL VIABCP")]
        BANCADIGITALVIABCP = 9,
        [Description("BANCA DIGITAL WEB")]
        BANCADIGITALWEB = 10,
        [Description("BANCA SEGUROS")]
        BANCASEGUROS = 11,
        [Description("CANALES ALTERNATIVOS")]
        CANALESALTERNATIVOS = 12,
        [Description("CANALES PRESENCIALES")]
        CANALESPRESENCIALES = 13,
        [Description("CENTRO DE CONTACTO MINORISTA")]
        CENTROCONTACTOMINORISTA = 14,
        [Description("COBRANZAS")]
        COBRANZAS = 15,
        [Description("COMUNICACIÓN (CCM)")]
        COMUNICACIÓN = 16,
        [Description("CRÉDITOS Y SERVICIOS MAYORISTA")]
        CRÉDITOSSERVICIOSMAYORISTA = 17,
        [Description("CRÉDITOS Y SERVICIOS MINORISTA")]
        CRÉDITOSSERVICIOSMINORISTA = 18,
        [Description("CRM ANALÍTICO")]
        CRMANALÍTICO = 19,
        [Description("CRM OPERATIVO")]
        CRMOPERATIVO = 20,
        [Description("CROSS CANAL")]
        CROSSCANAL = 21,
        [Description("CUMPLIMIENTO Y REGULATORIOS")]
        CUMPLIMIENTOREGULATORIOS = 22,
        [Description("DATA REFERENCE")]
        DATAREFERENCE = 23,
        [Description("DEPÓSITOS")]
        DEPÓSITOS = 24,
        [Description("ESTRATEGIA COMERCIAL TESORERÍA")]
        ESTRATEGIACOMERCIALTESORERÍA = 25,
        [Description("FRONTEND")]
        FRONTEND = 26,
        [Description("GDH + GESTIÓN DEL CONOCIMIENTO")]
        GDHGC = 27,
        [Description("GESTIÓN DE ALM")]
        GESTIÓNALM = 28,
        [Description("GESTION DE INVERSIONES")]
        GESTIONINVERSIONES = 29,
        [Description("GESTIÓN DOCUMENTAL CENTRALIZADA")]
        GESTIÓNDOCUMENTALCENTRALIZADA = 30,
        [Description("GESTIÓN DOCUMENTAL DIGITAL")]
        GESTIÓNDOCUMENTALDIGITAL = 31,
        [Description("GESTIÓN FINANCIERA")]
        GESTIÓNFINANCIERA = 32,
        [Description("INTERCONEXIÓN EMPRESARIAL INTERNACIONAL")]
        INTERCONEXIÓNEMPRESARIALINTERNACIONAL = 33,
        [Description("INTERCONEXIÓN EMPRESARIAL NACIONAL")]
        INTERCONEXIÓNEMPRESARIALNACIONAL = 34,
        [Description("MIDDLEWARE")]
        MIDDLEWARE = 35,
        [Description("NO APLICA")]
        NoAplica = 36,
        [Description("PAGOS, TRANSFERENCIAS Y TARJETA DE DÉBITO")]
        PAGOSTRANSFERENCIASTB = 37,
        [Description("PREVENCIÓN DE FRAUDES")]
        PREVENCIÓNFRAUDES = 38,
        [Description("PRODUCTOS DE TESORERÍA")]
        PRODUCTOSTESORERÍA = 39,
        [Description("RIESGO CREDITICIO")]
        RIESGOCREDITICIO = 40,
        [Description("RIESGO DE MERCADO Y LIQUIDEZ")]
        RIESGOMERCADOLIQUIDEZ = 41,
        [Description("RIESGO OPERACIONAL")]
        RIESGOOPERACIONAL = 42,
        [Description("RIESGOS TRANSVERSALES")]
        RIESGOSTRANSVERSALES = 43,
        [Description("SEGURIDAD DATOS & APLICACIÓN")]
        SEGURIDADDATOSAPLICACIÓN = 44,
        [Description("SEGURIDAD ENDPOINT")]
        SEGURIDADENDPOINT = 45,
        [Description("SEGURIDAD IDENTIDAD")]
        SEGURIDADIDENTIDAD = 46,
        [Description("SEGURIDAD NETWORK & CLOUD")]
        SEGURIDADNETWORKCLOUD = 47,
        [Description("SERVICIOS COMPARTIDOS")]
        SERVICIOSCOMPARTIDOS = 48,
        [Description("SERVICIOS CORPORATIVOS")]
        SERVICIOSCORPORATIVOS = 49,
        [Description("TARJETA DE CRÉDITO")]
        TARJETACRÉDITO = 50,
        [Description("TI PARA TI")]
        TIPARATI = 51,
    }

    public enum EEntidadUso
    {
        [Description("BCP")]
        BCP = 1,
        [Description("BCB")]
        BCB = 2,
        [Description("ASB")]
        ASB = 3,
        [Description("AFP")]
        AFP = 4,
        [Description("PPS")]
        PPS = 5,
        [Description("MIA")]
        MIA = 6,
        [Description("CAP")]
        CAP = 7
    }

    public enum EOOR
    {
        [Description("Si")]
        Si = 1,
        [Description("No")]
        No = 0
    }

    public enum EAmbienteInstalacion
    {
        [Description("Desarrollo")]
        Desarrollo = 1,
        [Description("Certificación")]
        Certificacion = 2,
        [Description("Producción")]
        Produccion = 3
    }

    public enum EMetodoAutenticacion
    {
        [Description("(Credenciales DebtManager)")]
        CredencialesDebtManager = 1,
        [Description("(SISECOB)")]
        SISECOB = 2,
        [Description("Active Directory")]
        AD = 3,
        [Description("Active Directory (Canal y Datapower) /RACF (Host)")]
        ADRACF = 4,
        [Description("Active Directory (Intranet) / App Móvil (N° Tarjeta y clave web)")]
        ADAppMovil = 5,
        [Description("Active Directory (Servidor BPC) / Login propio (Servidor DM)")]
        ADLoginPropio = 6,
        [Description("Active Directory Azure")]
        ActiveDirectoryAzure = 7
    }

    public enum EMetodoAutorizacion
    {
        [Description("(Credenciales DebtManager)")]
        CredencialesDebtManager = 1,
        [Description("(SISECOB)")]
        SISECOB = 2,
        [Description("Active Directory")]
        AD = 3,
        [Description("Active Directory (Canal y Datapower) /RACF (Host)")]
        ADCanalDatapowerRACF = 4,
        [Description("Active Directory (Intranet) / App Móvil (N° Tarjeta y clave web)")]
        ADIntranetAppMovil = 5,
        [Description("Active Directory (Servidor BPC) / Login propio (Servidor DM)")]
        ADLoginPropio = 6,
        [Description("Active Directory Azure")]
        ActiveDirectoryAzure = 7,
        [Description("Active Directory Cloud (Azure AD)")]
        ADCloud = 8,
        [Description("Active Directory y RACF")]
        ADRACF = 9,
        [Description("AD - Active Directory")]
        ADActiveDirectory = 10,
        [Description("ALM")]
        ALM = 11,
        [Description("Archivo Hash")]
        ArchivoHash = 12,
        [Description("Autenticacion por AD y Autorizacion por el módulo de seguridad del propio aplicativo")]
        AutenticacionAD = 13,
        [Description("AWS Cognito")]
        AWSCognito = 14,
        [Description("Azman")]
        Azman = 15
    }

    public enum EContingencia
    {
        [Description("Active Directory")]
        ActiveDirectory = 1,
        [Description("No aplica")]
        NoAplica = 2,
        [Description("No definido")]
        NoDefinido = 3,
        [Description("Si")]
        Si = 4
    }

    public enum EUbicacion
    {
        [Description("Escenario 1")]
        Escenario1 = 1,
        [Description("Escenario 2")]
        Escenario2 = 2,
        [Description("Escenario 3")]
        Escenario3 = 3,
        [Description("Externo")]
        Externo = 4
    }

    public enum ETipoDesarrollo
    {
        [Description("In House / In")]
        InHouseIn = 1,
        [Description("Híbrido / CE")]
        HibridoCE = 2,
        [Description("Nativo/ TE")]
        NativoTE = 3,
        [Description("Paquete Customizado")]
        PaqueteCustomizado = 4,
        [Description("Paquete No customizado")]
        PaqueteNoCustomizado = 5
    }

    public enum EInfraestructuraAplicacion
    {
        [Description("Cloud - IaaS")]
        CloudIaaS = 1,
        [Description("Cloud - PaaS")]
        CloudPaaS = 2,
        [Description("File Server")]
        FileServer = 3,
        [Description("Hosting Externo")]
        HostingExterno = 4,
        [Description("Local")]
        Local = 5,
        [Description("Pc Usuario")]
        PcUsuario = 6,
        [Description("Plataforma tecnológica")]
        PlataformaTecnologica = 7,
        [Description("Servidor")]
        Servidor = 8,
        [Description("Servidor autogestionado")]
        ServidorAutogestionado = 9,
        [Description("Servidor Consolidado AIO")]
        ServidorConsolidadoAIO = 10,
        [Description("Servidor propio")]
        ServidorPropio = 11,
        [Description("Servidores AIO")]
        ServidoresAIO = 12,
        [Description("Sin Información")]
        SinInformacion = 13
    }

    public enum EEstadoSolicitudAplicacion
    {
        [Description("Registrado")]
        Registrado = 1,
        [Description("En proceso de revisión")]
        EnRevision = 2,
        [Description("Aprobado")]
        Aprobado = 3,
        [Description("Observado")]
        Observado = 4
    }

    public enum EDivision
    {
        [Description("BCP BOLIVIA")]
        BCPBolivia = 1,
        [Description("CREDICORP CAPITAL")]
        CredicorpCapital = 2,
        [Description("CRM OPERACIONAL")]
        CRMOperacional = 3,
        [Description("D. ADMINISTRACIÓN DE RIESGOS")]
        DAdmRiesgos = 4,
        [Description("D. ADMINISTRACIÓN Y PROCESOS")]
        DAdmProcesos = 5,
        [Description("D. ASUNTOS CORPORATIVOS")]
        DAsuntosCorporativos = 6,
        [Description("D. AUDITORÍA")]
        DAuditoria = 7,
        [Description("D. BANCA CORP. E INTERNACIONAL")]
        DBancaCorpInternacional = 8,
        [Description("D. BANCA CORPORATIVA")]
        DBancaCorporativa = 9,
        [Description("D. BANCA EMPRESARIAL")]
        DBancaEmpresarial = 10,
        [Description("D. BANCA PERSONAS")]
        DBancaPersonas = 11,
        [Description("D. CANALES DE ATENCIÓN")]
        DCanalesAtencion = 12,
        [Description("D. CLIENTES CONTENTOS")]
        DCliente = 13,
        [Description("D. CONTABILIDAD GENERAL")]
        DContabilidadGeneral = 14,
        [Description("D. CUMPLIMIENTO Y ÉTICA CORPORATIVO")]
        DCumplimientoEtica = 15,
        [Description("D. DATA & ANALYTICS")]
        DDataAnalytics = 16,
        [Description("D. DE CRÉDITOS")]
        DCreditos = 17,
        [Description("D. DE RIESGOS BANCA MINORISTA")]
        DRiesgosBancaMinorista = 18,
        [Description("D. GESTIÓN DE PATRIMONIOS")]
        DGestionPatrimonios = 19,
        [Description("D. GESTIÓN Y DESARROLLO HUMANO Y ADMINISTRACIÓN")]
        DGestionDesarrolloHumanoAdm = 20,
        [Description("D. LEGAL Y SECRETARÍA GENERAL")]
        DLegalSecretariaGeneral = 21,
        [Description("D. PLANEAMIENTO Y CONTROL FINANCIERO")]
        DPlaneamientoControlFinanciero = 22,
        [Description("D. PRODUCTOS Y CANALES DIGITALES")]
        DProductosCanalesDigitales = 23,
        [Description("D. TECNOLOGÍAS DE INFORMACIÓN")]
        DTecnologiasInformacion = 24,
        [Description("PLANEAMIENTO Y SOPORTE DE NEGOCIOS")]
        PlaneamientoSoporteNegocios = 25,
        [Description("PRIMA AFP")]
        PrimaAFP = 26,
        [Description("SIN DIVISIÓN")]
        SinDivision = 27
    }

    public enum EArea
    {
        [Description("A. ADM. RIESGO DE CRÉDITO Y GESTIÓN CORPORATIVA DE RIESGOS")]
        area1 = 1,
        [Description("A. ADM. RIESGOS DE OP Y GESTIÓN DE SEGUROS")]
        area2 = 2,
        [Description("A. ADM. RIESGOS DE OPERACIÓN")]
        area3 = 3,
        [Description("A. ADMINISTRACIÓN")]
        area4 = 4,
        [Description("A. ALIANZAS COMERCIALES")]
        area5 = 5,
        [Description("A. ALM")]
        area6 = 6,
        [Description("A. ALM BUSINESS UNIT")]
        area7 = 7,
        [Description("A. ALM Y CAPITAL")]
        area8 = 8,
        [Description("A. ANÁLISIS ESTRATÉG. Y CRM")]
        area9 = 9,
        [Description("A. ANÁLISIS Y CONTROL FINANCIERO")]
        area10 = 10,
        [Description("A. ARQUITECTURAS DE TI")]
        area11 = 11,
        [Description("A. ASESORÍA DE INVERSIONES")]
        area12 = 12,
        [Description("A. ASESORÍA LEGAL")]
        area13 = 13,
        [Description("A. AUDITORÍA CONTÍNUA Y DESARROLLO CORPORATIVO")]
        area14 = 14,
        [Description("A. AUDITORÍA DE RIESGOS CORPORATIVOS")]
        area15 = 15,
        [Description("A. BANCA AFLUENTE")]
        area16 = 16,
        [Description("A. BANCA DE NEGOCIOS")]
        area17 = 17,
        [Description("A. BANCA PRIVADA")]
        area18 = 18,
        [Description("A. BANCA PYME")]
        area19 = 19,
        [Description("A. CANALES ALTERNATIVOS")]
        area20 = 20,
        [Description("A. CENTRO DE CONTACTO Y VENTAS")]
        area21 = 21,
        [Description("A. CENTRO FINANCIAMIENTO AUTOMOTRIZ")]
        area22 = 22,
        [Description("A. CENTRO INNOVACXIÓN")]
        area23 = 23,
        [Description("A. COBRANZA BANCA MINORISTA")]
        area24 = 24,
        [Description("A. CONTABILIDAD CORPORATIVA")]
        area25 = 25,
        [Description("A. CRÉDITOS BANCA CORPORATIVA")]
        area26 = 26,
        [Description("A. CRÉDITOS BANCA MINORISTA")]
        area27 = 27,
        [Description("A. CRÉDITOS EMPRESAS, BCOS Y CORPORATIVOS DEL EXT.")]
        area28 = 28,
        [Description("A. CREDITOS PERSONALES Y VEHICULARES")]
        area29 = 29,
        [Description("A. CUENTAS ESPECIALES Y SGTO DE CRÉDITOS")]
        area30 = 30,
        [Description("A. DATA")]
        area31 = 31,
        [Description("A. DE BANCA SEGUROS")]
        area32 = 32,
        [Description("A. ESTUDIOS ECONÓMICOS")]
        area33 = 33,
        [Description("A. EXPERIENCIA DEL COLABORADOR")]
        area34 = 34,
        [Description("A. GESTIÓN DE CUMPLIMIENTO")]
        area35 = 35,
        [Description("A. GESTIÓN DE OPS, SISTEMAS Y ADM.")]
        area36 = 36,
        [Description("A. GESTIÓN DE TRANSFORMACIÓN BCP WOW")]
        area37 = 37,
        [Description("A. GESTIÓN DEL TALENTO CORP.")]
        area38 = 38,
        [Description("A. INFRAESTRUCTURA")]
        area39 = 39,
        [Description("A. INFRAESTRUCTURA Y OPERACIONES DE TI")]
        area40 = 40,
        [Description("A. INGENIERÍA Y DESARROLLO DE TI")]
        area41 = 41,
        [Description("A. MARKETING Y EXP. DEL CLIENTE")]
        area42 = 42,
        [Description("A. MEDIOS DE PAGO")]
        area43 = 43,
        [Description("A. MEDIOS DE PAGO Y FINANC. AL CONSUMO")]
        area44 = 44,
        [Description("A. MESA DE DISTRIBUCIÓN")]
        area45 = 45,
        [Description("A. MODELOS Y METODOLOGÍAS PARA LA GESTIÓN DEL RIESGO")]
        area46 = 46,
        [Description("A. NEGOCIOS BANCA MINORISTA")]
        area47 = 47,
        [Description("A. NEGOCIOS HIPOTECARIOS")]
        area48 = 48,
        [Description("A. NEGOCIOS INTERNAC. Y LEASING")]
        area49 = 49,
        [Description("A. PLANEAMIENTO BANCA MINORISTA")]
        area50 = 50,
        [Description("A. PLANEAMIENTO DE COMPENSACIONES")]
        area51 = 51,
        [Description("A. PLANEAMIENTO ESTRAT. Y DESARROLLO DE NEG.")]
        area52 = 52
    }

    public enum EGenericoApp : int
    {
        [Description("SI")]
        SI = 1,
        [Description("NO")]
        NO = 2,
        [Description("NO APLICA")]
        NOAPLICA = 3
    }

    public enum ECompatibilidadNavegador : int
    {
        [Description("Chrome")]
        Chrome = 1,
        [Description("IE8")]
        IE8 = 2,
        [Description("IE8 y Chrome")]
        IE8yChrome = 3,
        [Description("Microsoft Edge y Chrome")]
        MicrosoftEdgeyChrome = 4,
        [Description("NO")]
        No = 5,
        [Description("NO APLICA")]
        NoAplica = 6
    }

    public enum EGrupoTicketRemedy : int
    {
        [Description("AET-TI-GUTI-EVERIS")]
        GTR1 = 1,
        [Description("AET-TI-GUTI-TCS")]
        GTR2 = 2,
        [Description("AIO - ETI")]
        GTR3 = 3,
        [Description("AIO – SOPORTE DEVSECOPS")]
        GTR4 = 4,
        [Description("AIO-Gestión de Servicios AIO-OPI-PROV")]
        GTR5 = 5,
        [Description("AIO-OPI-HBI")]
        GTR6 = 6,
        [Description("AIO-OPI-HBI-PROV")]
        GTR7 = 7,
        [Description("ATI - ARQUITECTURA - SOFTWARE")]
        GTR8 = 8,
        [Description("BCP")]
        GTR9 = 9,
        [Description("BCP - AIO - OPI - HBI")]
        GTR10 = 10,
        [Description("BCP - ETI - ESTRATEGIAS TECNOLOGICAS E INNOVACION")]
        GTR11 = 11,
        [Description("BCP - GSO - PRE-PRODUCCION")]
        GTR12 = 12,
        [Description("CIX - NECESIDADES FINANCIERAS NFBI")]
        GTR13 = 13,
        [Description("CIX: CENTRO DE INNOVACION")]
        GTR14 = 14,
        [Description("DOPS – TORRE DE CONTROL")]
        GTR15 = 15,
        [Description("EQUIPOS DE USUARIO")]
        GTR16 = 16,
        [Description("GICC - CALL CENTER")]
        GTR17 = 17,
        [Description("HELPDESK ACCESOS Y PERMISOS")]
        GTR18 = 18,
        [Description("HELPDESK OPERADORES")]
        GTR19 = 19,
        [Description("IBM - MON - MONITOREO CANALES Y APLICACIONES")]
        GTR20 = 20
    }

    public enum EEstadoCriticidad : int
    {
        [Description("Alto")]
        Alto = 1,
        [Description("Medio")]
        Medio = 2,
        [Description("Bajo")]
        Bajo = 3,
        [Description("No aplica")]
        NoAplica = 4
    }

    public enum EClasificacionCriticidad : int
    {
        [Description("Público")]
        Publico = 1,
        [Description("Restringido")]
        Restringido = 2,
        [Description("Uso interno")]
        UsoInterno = 3,
        [Description("No aplica")]
        NoAplica = 4
    }

    public enum EEstadoRoadmap : int
    {
        [Description("Cerrado")]
        Cerrado = 1,
        [Description("En proceso")]
        EnProceso = 2,
        [Description("No aplica")]
        NoAplica = 3,
        [Description("Pendiente")]
        Pendiente = 4,
        [Description("Suspendido")]
        Suspendido = 5,
        [Description("Swat Team Planificado")]
        STP = 6
    }

    public enum ECodigoRetorno : int
    {
        [Description("OK, campos correctos")]
        OKCamposCorrectos = 201,
        [Description("OK")]
        OK = 200,
        [Description("Error general, campos vacios")]
        ErrorGeneral = -500,
        [Description("El equipo ya existe")]
        EquipoExiste = -501,
        [Description("El equipo no existe")]
        EquipoNoExiste = -502,
        [Description("CodigoAPT no encontrado")]
        CodigoAPT = -503,
        [Description("Dominio no encontrado")]
        Dominio = -504,
        [Description("Relevancia no encontrado")]
        Relevancia = -505,
        [Description("Ambiente no encontrado")]
        Ambiente = -506,
        [Description("DominioServidor no encontrado")]
        DominioServidor = -507,
        [Description("Clave tecnología no encontrada")]
        Tecnologia = -508,
        [Description("TipoEquipo no encontrado")]
        TipoEquipo = -509,
        [Description("Caracteristica no encontrada")]
        Caracteristica = -510,
        [Description("La relación ya existe")]
        RelacionExiste = -511
    }

    public enum EVistaGestionAplicacion
    {
        [Description("Nueva aplicación")]
        Registro = 1,
        [Description("Catálogo detalle aplicación")]
        Catalogo = 2,
        [Description("Bandeja aprobación solicitudes")]
        Bandeja = 3,
        [Description("Bandeja aprobadores")]
        BandejaAprobador = 4,
        [Description("Solicitud de modificación de aplicación")]
        Modificar = 5
    }

    public enum EPortafolioResponsable : int
    {
        TTL = 1,
        JdE = 2,
        Broker = 3,
        Owner = 4,
        Gestor = 5,
        Experto = 6,
    }

    public enum ETipoSolicitudAplicacion
    {
        [Description("Creación")]
        CreacionAplicacion = 1,
        [Description("Modificación")]
        ModificacionAplicacion = 2,
        [Description("Eliminación")]
        EliminacionAplicacion = 3
    }

    public enum EHora
    {
        [Description("00:00")]
        CeroHoras = 0,
        [Description("01:00")]
        UnaHoras = 1,
        [Description("02:00")]
        DosHoras = 2,
        [Description("03:00")]
        TresHoras = 3,
        [Description("04:00")]
        CuatroHoras = 4,
        [Description("05:00")]
        CincoHoras = 5,
        [Description("06:00")]
        SeisHoras = 6,
        [Description("07:00")]
        SieteHoras = 7,
        [Description("08:00")]
        OchoHoras = 8,
        [Description("09:00")]
        NueveHoras = 9,
        [Description("10:00")]
        DiezHoras = 10,
        [Description("11:00")]
        OnceHoras = 11,
        [Description("12:00")]
        DoceHoras = 12,
        [Description("13:00")]
        TreceHoras = 13,
        [Description("14:00")]
        CatorceHoras = 14,
        [Description("15:00")]
        QuinceHoras = 15,
        [Description("16:00")]
        DeiciseisHoras = 16,
        [Description("17:00")]
        DiecisieteHoras = 17,
        [Description("18:00")]
        DieciochoHoras = 18,
        [Description("19:00")]
        DiecinueveHoras = 19,
        [Description("20:00")]
        VeinteHoras = 20,
        [Description("21:00")]
        VeintiunHoras = 21,
        [Description("22:00")]
        VeintidosHoras = 22,
        [Description("23:00")]
        VeintitresHoras = 23,
    }

    public enum ETablaProcedenciaAplicacion
    {
        [Description("cvt.Aplicacion")]
        Aplicacion = 1,
        [Description("app.AplicacionDetalle")]
        AplicacionDetalle = 2,
        [Description("data.Aplicacion")]
        AplicacionData = 3,
        [Description("app.InfoCampoPortafolio")] //Para campos nuevos
        InfoCampoPortafolio = 4
    }

    public enum ETipoComentarioSolicitud
    {
        [Description("Observación")]
        Observacion = 1,
        [Description("Mis comentarios")]
        Respuesta = 2
    }

    public enum ELetraColumnaExcel : int
    {
        [Description("A")]
        A = 1,
        [Description("B")]
        B = 2,
        [Description("C")]
        C = 3,
        [Description("D")]
        D = 4,
        [Description("E")]
        E = 5,
        [Description("F")]
        F = 6,
        [Description("G")]
        G = 7,
        [Description("H")]
        H = 8,
        [Description("I")]
        I = 9,
        [Description("J")]
        J = 10,
        [Description("K")]
        K = 11,
        [Description("L")]
        L = 12,
        [Description("M")]
        M = 13,
        [Description("N")]
        N = 14,
        [Description("O")]
        O = 15,
        [Description("P")]
        P = 16,
        [Description("Q")]
        Q = 17,
        [Description("R")]
        R = 18,
        [Description("S")]
        S = 19,
        [Description("T")]
        T = 20,
        [Description("U")]
        U = 21,
        [Description("V")]
        V = 22,
        [Description("W")]
        W = 23,
        [Description("X")]
        X = 24,
        [Description("Y")]
        Y = 25,
        [Description("Z")]
        Z = 26
    }

    public enum EGenericFlag
    {
        [Description("Si")]
        Si = 1,
        [Description("No")]
        No = 2
    }

    public enum EEntidadRelacion
    {
        [Description("RoadMap")]
        RoadMap = 23,
        [Description("Criticidad")]
        Criticidad = 21
    }

    public enum ETablaProcedenciaModificacion
    {
        [Description("Aplicación")]
        Aplicacion = 1,
        [Description("Módulo")]
        Modulo = 2
    }

    public enum EEstadoEquipo
    {
        [Description("Activos")]
        Activo = 1,
        [Description("Inactivos")]
        Inactivo = 0
    }

    public enum EMotivoEliminacionRelacion
    {
        [Description("El servidor está en proceso de baja")]
        Motivo1 = 1,
        [Description("El servidor nunca estuvo relacionado a la aplicación")]
        Motivo2 = 2,
        [Description("El servidor dejará de ser utilizado por la aplicación")]
        Motivo3 = 3,
        [Description("Otros")]
        Motivo4 = 4
    }

    public enum EMotivoEliminacionRelacionAplicaciones
    {
        [Description("La aplicación está en proceso de baja")]
        Motivo1 = 1,
        [Description("Las aplicaciones no están relacionadas")]
        Motivo2 = 2,
        [Description("La aplicación relacionada será reemplazada por otra")]
        Motivo3 = 3,
        [Description("Otros")]
        Motivo4 = 4
    }

    public enum EEntidadPortafolio
    {
        [Description("Tipo Activo Información")]
        TipoActivoInformacion = 1,
        [Description("Gerencia Central")]
        GerenciaCentral = 2,
        [Description("Motivo Creación")]
        MotivoCreacion = 3,
        [Description("Categoría Tecnológica")]
        CategoriaTecnologica = 4,
        [Description("Modelo Entrega")]
        ModeloEntrega = 5,
        [Description("Ambiente Instalación")]
        AmbienteInstalacion = 6,
        [Description("OOR")]
        OOR = 7,
        [Description("Método Autenticación")]
        MetodoAutenticacion = 8,
        [Description("Método Autorización")]
        MetodoAutorizacion = 9,
        [Description("Contingencia")]
        Contingencia = 10,
        [Description("Ubicación")]
        Ubicacion = 11,
        [Description("Tipo Desarrollo")]
        TipoDesarrollo = 12,
        [Description("Infraestructura Aplicación")]
        InfraestructuraAplicacion = 13,
        [Description("Genérico")]
        Generico = 14,
        [Description("Compatibilidad Navegador")]
        CompatibilidadNavegador = 15,
        [Description("Grupo Ticket Remedy")]
        GrupoTicketRemedy = 16,
        [Description("Clasificación Criticidad")]
        ClasificacionCriticidad = 17,
        [Description("Estado Roadmap")]
        EstadoRoadmap = 18,
        [Description("Estado Criticidad")]
        EstadoCriticidad = 19,
        [Description("Clasificación Técnica")]
        ClasificacionTecnica = 20,
        [Description("Entidad Responsable")]
        EntidadResponsable = 21,
        [Description("Estado")]
        EstadoAplicacion = 22,
        [Description("Criticidad Aplicación")] //TODO
        CriticidadAplicacion = 23,
        [Description("Capa Funcional")]
        CapaFuncional = 24
    }

    #region GESTION_APPLIANCE

    public enum EEntidadAppliance
    {
        [Description("Dimensión")]
        Dimension = 1,
        [Description("Tipo activo equipo")]
        TipoActivo = 2,
        [Description("Sede")]
        Sede = 3,
        [Description("Backup")]
        Backup = 4,
        [Description("Backup Frecuencia")]
        BackupFrecuencia = 5,
        [Description("Integración Gestor Inteligencia")]
        IntegracionGestorInteligencia = 6,
        [Description("CONA")]
        CONA = 7,
        [Description("Criticidad")]
        Criticidad = 8,
        [Description("CyberSoc")]
        CyberSoc = 9,
        [Description("Estado Integración SIEM")]
        EstadoIntegracionSIEM = 10,
        [Description("CONA Avanzado")]
        CONAAvanzado = 11,
        [Description("Estado Appliance")]
        EstadoAppliance = 12

    }

    public enum ETipoActivoEquipo
    {
        [Description("Físico")]
        Fisico = 1,
        [Description("Virtual (On Premise)")]
        Virtual = 2,
        [Description("PaaS")]
        Pass = 3,
        [Description("IaaS")]
        Iaas = 4,
        [Description("SaaS")]
        Saas = 5
    }

    public enum EDimensionEquipo
    {
        [Description("Dimensión 1")]
        Dimension1 = 1,
        [Description("Dimensión 2")]
        Dimension2 = 2,
        [Description("Dimensión 3")]
        Dimension3 = 3
    }

    public enum ESedeEquipo
    {
        [Description("Sede Chorrillos")]
        Sede1 = 1,
        [Description("Sede La Molina")]
        Sede2 = 2
    }

    public enum EBackup
    {
        [Description("Manual")]
        Manual = 1,
        [Description("Automatizado")]
        Automatizado = 2,
        [Description("No aplica")]
        NoAplica = 3
    }

    public enum EBackupFrecuencia
    {
        [Description("Diario")]
        Diario = 1,
        [Description("Semanal")]
        Semanal = 2,
        [Description("Quincenal")]
        Quincenal = 3,
        [Description("Mensual")]
        Mensual = 4,
        [Description("No aplica")]
        NoAplica = 5
    }

    public enum EIntegracionGestorInteligencia
    {
        [Description("SIEM")]
        Siem = 1,
        [Description("SIEM - SOAR")]
        SiemSoar = 2,
        [Description("SOAR")]
        Soar = 3,
        [Description("No aplica")]
        NoAplica = 4
    }

    public enum ECONA
    {
        [Description("No integrado")]
        NoIntegrado = 1,
        [Description("Básico")]
        Basico = 2,
        [Description("Avanzado")]
        Avanzado = 3
    }

    public enum ECriticidadEquipo
    {
        [Description("Crítico")]
        Critico = 1,
        [Description("Alto")]
        Alto = 2,
        [Description("Medio")]
        Medio = 3,
        [Description("Bajo")]
        Bajo = 4
    }

    public enum ECyberSoc
    {
        [Description("Si")]
        Si = 1,
        [Description("No")]
        No = 2
    }

    #endregion

    public enum EEntidadParametrica
    {
        [Description("APPLIANCE")]
        APPLIANCE = 1,
        [Description("PORTAFOLIO")]
        PORTAFOLIO = 2
    }

    public enum EUsuarioNotificacion
    {
        TTL = 1,
        LiderUsuario = 2
    }

    public enum EAplicaATecnologia
    {
        [Description("On-Premise")]
        OnPremise = 1,
        [Description("SaaS")]
        Saas = 2,
        [Description("PaaS")]
        Paas = 3,
        [Description("IaaS")]
        Iaas = 4,
        [Description("End User")]
        EndUser = 5
    }

    public enum EUbicacionStorage
    {
        [Description("La Molina")]
        LaMolina = 1,
        [Description("Chorrillos")]
        Chorrillos = 2
    }

    public enum EAPIMetodo
    {
        [Description("Get")]
        Get = 1,
        [Description("Post")]
        Post = 2
    }

    public enum EFlujoRegistro
    {
        [Description("FNA")]
        FNA = 1,
        [Description("PAE")]
        PAE = 2
    }

    public enum EConfiguracionPortafolio
    {
        [Description("Area BIAN")]
        AreaBian = 1,
        [Description("Dominio BIAN")]
        DominioBian = 2,
        [Description("Tipo activo Información")]
        TAI = 3,
        [Description("Gerencia")]
        Gerencia = 4,
        [Description("División")]
        Division = 5,
        [Description("Área")]
        Area = 6,
        [Description("Unidad")]
        Unidad = 7,
        [Description("CuestionarioPae")]
        CuestionarioPae = 8,
        [Description("PreguntaPae")]
        PreguntaPae = 9,
        [Description("Bandeja")]
        Bandeja = 10,
        [Description("BandejaAprobacion")]
        BandejaAprobacion = 11,
        [Description("JefaturaAti")]
        JefaturaAti = 12,
        [Description("ArquitectoTi")]
        ArquitectoTi = 13,
        [Description("PlataformaBcp")]
        PlataformaBcp = 14,
        [Description("GestionadoPor")]
        GestionadoPor = 15,
        [Description("ProcesoVital")]
        ProcesoVital = 16,
        [Description("Estandar")]
        Estandar = 17,
        [Description("Parametrica")]
        Parametrica = 18,
        [Description("GestionCampo")]
        GestionCampo = 19,
        [Description("RolGestion")]
        RolGestion = 20,
        [Description("Clasificación técnica")]
        ClasificacionTecnica = 21,
        [Description("Subclasificación técnica")]
        SubclasificacionTecnica = 22,
        [Description("Codigo Reservado")]
        CodigoReservado = 23
    }

    public enum ETreeLevel
    {
        Uno = 1,
        Dos = 2,
        Tres = 3,
        Cuatro = 4
    }

    public enum ECuestionarioPae
    {
        Confidencialidad = 1,
        Integridad = 2,
        Disponibilidad = 3,
        Privacidad = 4
    }

    public enum EBandejaAprobadorAplicacion
    {
        [Description("Administrador")]
        Administrador = 0,
        [Description("Arquitectura de TI")]
        ArquitecturaTI = 1,
        [Description("Clasificación Técnica")]
        ClasificacionTecnica = 2,
        [Description("DevSecOps")]
        DevSecOps = 3,
        [Description("Usuario Autorizador / PO")]
        PO = 4,
        [Description("TTL")]
        TTL = 5,
        [Description("Gestor User IT")]
        GestorUserIT = 6,
        [Description("Registro")]
        Registro = 7
    }

    public enum ETipoEstandarPortafolio
    {
        [Description("Sistema operativo")]
        SistemaOperativo = 1,
        [Description("Herramientas de programación")]
        HerramientaProgramacion = 2,
        [Description("Gestor de base de datos")]
        GestorBaseDatos = 3,
        [Description("Framework")]
        Framework = 4
    }

    public enum ECodigoInternoArchivo
    {
        Servidor = 1,
        Arquetipo = 2,
        Excepcion = 3,
        Tecnologia = 4,
        Otros = 5,
        Aplicacion = 6
    }

    public enum EReferenciaFileAplicacion
    {
        CheckList = 1,
        Conformidad = 2,
        Matriz = 3,
        ConformidadMatriz = 4,
        SolicitudModificacion = 5
    }

    public enum ETipoInputHTML
    {
        [Description("TextBox")]
        TextBox = 1,
        [Description("ListBox")]
        ListBox = 2,
        [Description("Otros")]
        Otros = 3
    }

    public enum ETipoNotificacionAplicacion
    {
        [Description("Solicitud de creación de aplicación")]
        RegistroSolicitudCreacion = 1,
        [Description("Solicitud de modificación de aplicación")]
        RegistroSolicitudModificacion = 2,
        [Description("Solicitud de eliminación de aplicación")]
        RegistroSolicitudEliminacion = 3,
        [Description("Aprobación de solicitud de creación")]
        AprobacionSolicitudCreacion = 4,
        [Description("Aprobación de solicitud de modificación")]
        AprobacionSolicitudModificacion = 5,
        [Description("Aprobación de solicitud de eliminación")]
        AprobacionSolicitudEliminacion = 6,
        [Description("Cambio de estado por Arquitectura de TI")]
        CambioEstadoArquitecturaTI = 7,
        [Description("Cambio de estado por Arquitectura (Clasificación técnica)")]
        CambioEstadoClasificacion = 8,
        [Description("Cambio de estado por DevOps")]
        CambioEstadoDevOps = 9,
        [Description("Cambio de estado por Líder Usuario")]
        CambioEstadoLiderUsuario = 10,
        [Description("Cambio de estado por TTL")]
        CambioEstadoTTL = 11,
        [Description("Cambio de estado por Gestor UserIT")]
        CambioEstadoUserIT = 12,
        [Description("Notificación de creación de nueva aplicación")]
        NotificacionBuzonesCreacion = 13,
        [Description("Notificación de eliminación de aplicación")]
        NotificacionBuzonesEliminacion = 14
    }

    public enum ETipoErrorCargaMasiva
    {
        [Description("Entidad no encontrada")]
        EntidadNoEncontrada = 1,
        [Description("Registro incompleto")]
        RegistroIncompleto = 2,
        [Description("Error Interno")]
        ErrorInterno = 3,
        [Description("Código de mas de 4 digitos ")]
        Mas4Digitos = 4,
        [Description("Registro debe ser un número ")]
        NoNumero = 5,
        [Description("No debe tener un equipo asignado ")]
        NoTeam = 6,
        [Description("Se necesita tener un equipo asignado ")]
        NeedTeam = 7,
        [Description("Entidad no encontrada o equipo no relacionado al Gestionado Por ")]
        NoFoundTeam = 8
    }

    public enum ETipoBitacora
    {
        [Description("El usuario {0} registró una solicitud para la creación de la aplicación con código {1}.")]
        SolicitudRegistroAplicacion = 1,
        [Description("El usuario {0} registró una solicitud para la eliminación de la aplicación con código {1}.")]
        SolicitudEliminacionAplicacion = 2,
        [Description("El usuario {0} registró una solicitud para la modificación de la aplicación con código {1}.")]
        SolicitudModificacionAplicacion = 3,
        [Description("El usuario {0} ({3}) {1} la solicitud asociada al flujo de registro de la aplicación con código {2}.")]
        AprobacionSolicitudFlujoAplicacion = 4,
        [Description("El usuario {0} ({4}) observó el flujo de {1} de la aplicación con código {2}, con los siguientes comentarios: {3}")]
        ObservacionFlujoAplicacion = 5,
        [Description("El usuario {0} aprobó el registro de la aplicación con código {1}.")]
        AprobacionRegistroAplicacion = 6,
        [Description("El usuario {0} aprobó la eliminación de la aplicación con código {1}.")]
        AprobacionEliminacionAplicacion = 7,
        [Description("El usuario {0} aprobó la modificación de la aplicación con código {1}.")]
        AprobacionModificacionAplicacion = 8,
        [Description("El usuario {0} actualizó la solicitud de {1} de la aplicación con código {2}.")]
        ActualizacionSolicitud = 9,
        [Description("El usuario {0} {1} la solicitud asociada al flujo de registro de la aplicación ({4})  con código {2}, con los siguientes comentarios: {3}")]
        ObservacionSolicitudFlujoAplicacion = 10,
        [Description("El usuario {0} modificó los siguientes campos de la aplicación {1}: {2}")]
        ModificacionAplicacion = 11
    }

    public enum ENivelValidacionPortafolio
    {
        [Description("No existe dependencias")]
        NivelCero = 0,
        [Description("Existen entidades dependientes que estan usando este registro")]
        NivelUno = 1,
        [Description("Existen aplicaciones registradas y aprobadas que estan usando este registro")]
        NivelDos = 2,
        [Description("Existen aplicaciones en proceso de revisión que estan usando este registro")]
        NivelTres = 3
    }

    public enum EIdResponsablePortafolio
    {
        [Description("Lider usuario")]
        Owner_LiderUsuario_ProductOwner = 18,
        [Description("Usuario autorizador")]
        Gestor_UsuarioAutorizador_ProductOwner = 19,
        [Description("TTL")]
        TribeTechnicalLead = 14,
        [Description("Experto especialista")]
        Experto_Especialista = 33,
        [Description("Broker de sistemas")]
        BrokerSistemas = 37,
        [Description("Jefe de equipo")]
        JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = 15
    }

    public enum EResultCode : int
    {
        [Description("OK")]
        OK = 200,
        [Description("El tipo de tecnología no existe en CVT")]
        TipoNoExiste = -1,
        [Description("El dominio/subdominio no existen en CVT o no tiene relación")]
        DominioSubdominioNoExiste = -2,
        [Description("El estado de la tecnología no es el correcto")]
        EstadoNoExiste = -3,
        [Description("La clave de la tecnología ya existe en CVT")]
        ClaveExiste = -4,
        [Description("La fuente de la fecha fin no es la correcta")]
        FuenteFechaFinIncorrecta = -5,
        [Description("El tipo de fecha de fin no es el correcto")]
        TipoFechaFinSoporteIncorrecta = -6,
        [Description("Los campos requeridos estan incompletos")]
        CamposRequeridosIncompletos = -7,
        [Description("El valor de los criterios es incorrecto")]
        CriterioIncorrecto = -8
    }

    public enum ERoles
    {
        [Description("Arquitecto de tecnología")]
        ArquitectoTecnologia = 1,
        [Description("DevSecOps")]
        DevSecOps = 2,
        [Description("Gobierno User IT")]
        GobiernoUserIT = 3,
        [Description("AIO")]
        AIO = 4,
        [Description("Administrador del portafolio")]
        Administrador = 5
    }

    public enum EAPIResponseCode
    {
        [Description("Operación exitosa")]
        TL0001 = 1,
        [Description("Los datos proporcionados no son válidos")]
        TL0003 = 3,
        [Description("Ocurrió un error en el servicio Externo consumido")]
        TL0004 = 4,
        [Description("Ocurrió un error en la comunicación con el BackEnd")]
        TL0005 = 5,
        [Description("Los datos proporcionados no cumplen con los criterios para ser procesado")]
        TL0006 = 6,
        [Description("Ocurrió un error en el Backend")]
        TL0007 = 7,
        [Description("Ocurrió un error inesperado. Por favor contactarse con Soporte Técnico")]
        TL9999 = 9999
    }

    public enum EUrlFuente
    {
        [Description("Carga masiva")]
        CargaMasiva = 1,
        [Description("Azure")]
        Azure = 2,
        [Description("Servidor DNS")]
        ServidorDNS = 3
    }

    public enum EStatusCodeSP
    {
        [Description("Operación exitosa")]
        OperacionExitosa = 1,
        [Description("Los datos proporcionados no son válidos")]
        DatosInconsistentes = -1,
        [Description("La clave de la tecnología ya se encuentra registrada")]
        ClaveTecnologiaExistente = -2,
        [Description("Error interno")]
        ErrorInterno = -3
    }

    public enum ETipoExportarReportePortafolioEstado
    {
        [Description("Distribuciones de aplicaciones en el banco")]
        DistribucionAplicacionesBanco = 1,
        [Description("Distribución de aplicaciones en la unidad jerárquica seleccionada")]
        DistribucionAplicaciones = 2,
        [Description("Distribución de aplicaciones en la unidad jerárquica seleccionada: por estado")]
        DistribucionAplicacionesByEstado = 3,
        [Description("Distribución de aplicaciones en la unidad jerárquica seleccionada: por criticidad")]
        DistribucionAplicacionesByCriticidad = 4,
        [Description("Distribución de aplicaciones en la unidad jerárquica seleccionada: por categoría tecnológica")]
        DistribucionAplicacionesByCategoria = 5,
        [Description("Salud de aplicaciones en la unidad jerárquica seleccionada")]
        SaludAplicacion = 6
    }

    public enum ETipoExportarReportePortafolioVariacion
    {
        [Description("Evolución de solicitudes de creación")]
        SolicitudesCreadas = 1,
        [Description("Evolución de solicitudes de eliminación")]
        SolicitudesEliminadas = 2,
        [Description("Aplicaciones nuevas y eliminadas por periodo de tiempo")]
        SolicitudesCreadasEliminadas = 3,
        [Description("Estados por periodo de tiempo")]
        SolicitudesXEstado = 4,
        [Description("Distribucion por Gerencia")]
        DistribucionesXGerencia = 5
    }
    public enum EEstadoObsolescenciaProducto
    {
        [Description("Obsoleto")]
        Obsoleto = 1,
        [Description("Vigente")]
        Vigente = 2
    }

    public enum EAutomatizacionImplementada
    {
        [Description("Totalmente Automatizado")]
        TotalmenteAutomatizado = 1,
        [Description("Parcialmente Automatizado")]
        ParcialmenteAutomatizado = 2,
        [Description("Manual No Automatizado")]
        ManualNoAutomatizado = 3
    }

    public enum ETipo
    {
        [Description("Estándar Restringido")]
        EstandarRestringido = 2
    }

    public enum ETipoExportarReportePortafolioPedidos
    {
        [Description("Distribuciones por tipo de atención (acumulado)")]
        EstadoAtencion = 1,
        [Description("Distribuciones por tipo de atención (por periodo de tiempo)")]
        EstadoAtencionXTiempo = 2,
        [Description("Datos de consultas al portafolio")]
        ConsultasPortafolio = 3,
        [Description("Dias reales de registro de app (situacion de registro)")]
        RegistroApp = 4,
        [Description("Campos más requeridos (top 5) para actualización en promedio")]
        CamposMasActualizados = 5,
        [Description("Reporte de SLA (promedio de días)")]
        ReporteSLA = 6,
        [Description("Reporte de Consultas")]
        Consultas = 7
    }

    public enum RevisionSeguridad
    {
        [Description("Sí Aplica")]
        SiAplica = 1,
        [Description("No Aplica")]
        NoAplica = 2,
        [Description("Pendiente de revisión")]
        PendienteRevision = 3
    }

    public enum UrlConfluence
    {
        [Description("Sí Aplica")]
        SiAplica = 1,
        [Description("No Aplica")]
        NoAplica = 2,
        [Description("Pendiente de revisión")]
        PendienteRevision = 3
    }

    public enum EEsquemaLicenciamientoSuscripcion
    {
        //[Description("Propietario")]
        //Propietario = 1,
        [Description("Licenciado")]
        Licenciado = 3,
        [Description("Suscripción")]
        Suscripcion = 4,
        [Description("Open Source")]
        OpenSource = 2,
    }

    //Asociados a las nuevas vistas
    public enum OrigenEquipoNoRegistrado
    {
        Qualys = 1,
        Guardicore = 2,
        Snow = 3,
        EquipoManual = 4
    }

    public enum MotivoEquipoNoRegistrado
    {
        [Description("El equipo no existe en CVT")]
        EquipoNoExiste = 1,
        [Description("El equipo esta inactivo en CVT")]
        EquipoInactivo = 2,
        [Description("El equipo esta cargado manualmente en CVT")]
        EquipoCargadoManualmente = 3
    }

    public enum EstadoEquipoNoRegistrado
    {
        Registrado = 1,
        Procesado = 2,
    }

    public enum UnidadesTamañoDigital
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }

    public enum VulnStatus
    {
        //[Description("")]
        //Vacío = 1,
        [Description("Active")]
        Active = 2,
        [Description("Fixed")]
        Fixed = 3,
        [Description("New")]
        New = 4,
        [Description("Re-Opened")]
        ReOpened = 5
    }

    public enum TipoAplicacionRelacionEtiquetado
    {
        [Description("De 2 a 6 aplicaciones")]
        DeDosaSeis = 1,
        [Description("De 7 a Mas aplicaciones")]
        DeSieteaMas = 2,
        [Description("-")]
        vacio = 0
    }

    public enum OrigenTecnologiaInstaladaNoRegistrada
    {
        Qualys = 1,
        //Guardicore = 2,
        //Snow = 3,
        //EquipoManual = 4
    }

    public enum MotivoTecnologiaInstaladaNoRegistrada
    {
        [Description("La tecnología no se encuentra registrado en el servidor en CVT")]
        TecnologiaInstaladaNoRegistradaEnEquipo = 1,
    }

    public enum EstadoVulnerabilidades
    {
        [Description("Active")]
        Active = 1,
        [Description("Fixed")]
        Fixed = 2,
        [Description("New")]
        New = 3,
        [Description("Re-Opened")]
        ReOpened = 4
    }

    public enum Arquitectos
    {
        [Description("ArquitectoNegocio")]
        ArquitectoNegocio = 12,
        [Description("ArquitectoTecnologia")]
        ArquitectoTecnologia = 1,
        [Description("ArquitectoSolucion")]
        ArquitectoSolucion = 13,
    }

    public enum TipoReporteVulnerabilidades
    {
        Administradores = 1,
        Seguridad = 2
    }

    public enum EstadoSolicitudActivosTSI
    {
        Ninguna = 0,
        [Description("Registrado")]
        Registrado = 1,
        [Description("Pendiente atención Owner")]
        PendienteAtencionOwner = 2,
        [Description("Aprobado Owner")]
        AprobadoOwner = 3,
        [Description("Rechazado Owner")]
        RechazadoOwner = 4,
        [Description("Desestimada")]
        Desestimado = 5,
        [Description("Pendiente atención CVT")]
        PendienteAtencionCVT = 6,
        [Description("Aprobado CVT")]
        AprobadoCVT = 7,
        [Description("Rechazado CVT")]
        RechazadoCVT = 8,
    }

    public enum MotorRelacionamientoAccion
    {
        [Description("Relación registrada")]
        Registrado = 1,
        [Description("Relación actualizada")]
        Actualizado = 2
    }

    public enum MotorReglaProcesoTipo
    {
        [Description("Proceso Origen")]
        Origen = 1,
        [Description("Proceso Destino")]
        Destino = 2
    }

    public enum MotorTipoRelacionamiento
    {
        [Description("App Origen - Servidor Destino")]
        OrigenDestino = 1,
        [Description("App Destino - Servidor Destino")]
        DestinoDestino = 2,
        [Description("App Origen - Servidor Origen")]
        OrigenOrigen = 3,
        [Description("App Destino - Servidor Origen")]
        DestinoOrigen = 4
    }
    public enum EquipoInstanciaBD_Inventario
    {
        [Description("Helix")]
        Helix = 1,
        [Description("Azure")]
        Azure = 2,
        [Description("Manual")]
        Manual = 3
    }
    public enum EstadoObsolescenciaHardware
    {
        [Description("Obsoleto")]
        Obsoleto = -1,
        [Description("Vence en 12 meses")]
        Vence12Meses = 0,
        //[Description("Vence en 24 meses")]
        //Vence24Meses = 2,
        [Description("Vigente")]
        Vigente = 1
    }
}