using System.ComponentModel;

namespace BCP.PAPP.Common.Cross
{
    public static class ConstantsPortfolio
    {
        public const string TipoImplementacion = "Tipo de implementacion";
        public const string ModeloEntrega = "Modelo de entrega";
        public const string TipoDesarrollo = "Tipo de desarrollo";
        public const string Infraestructura = "Infraestructura de la aplicación";
        public const string TipoActivo = "Tipo de activo de informacion";
        public const string MetodoAutenticacion = "Método de autenticación";
        public const string MetodoAutorizacion = "Método de autorización";
        public const string CategoriaTecnologia = "Categoria tecnologica";
        public const string ClasificacionTecnica = "Clasificacion tecnica";
        public const string HorarioFuncionamiento = "Horario Funcionamiento";

        public const string EntidadesUsuarias = "Entidades usuarias";
    }


    public enum TipoAsignacionMDR
    {
        [Description("Creacion de Rol")]
        CreacionRol = 1,
        [Description("Asignacion de Funcion Owner")]
        AsignacionFuncionOwmer = 2,
        [Description("Asignacion de Funcion Seguridad")]
        AsignacionFuncionSeguridad = 3

    }
    public enum EstadoSolicitudMDR
    {
        [Description("Pendiente")]
        Pendiente = 1,
        [Description("Aprobado Owner")]
        AprobadoOwner = 2,
        [Description("Aprobado Seguridad")]
        AprobadoSeguridad = 3,
        [Description("Rechazado Owner")]
        RechazadoOwner = 4,
        [Description("Rechazado Seguridad")]
        RechazadoSeguridad = 5,
        [Description("Eliminado")]
        Eliminado = 6,
        [Description("Atendido")]
        Atendido = 7
    }
    public enum EstadoSolicitudRolFuncion
    {
        [Description("Pendiente")]
        Pendiente = 0,
        [Description("Aprobado Seguridad")]
        AprobadoSeguridad = 1,
        [Description("Observado Seguridad")]
        ObservadoSeguridad = 2,
        [Description("Aprobado Owner")]
        AprobadoOwner = 3,
        [Description("Observado Owner")]
        ObservadoOwner = 4,
        [Description("Atendido")]
        Atendido = 5,
        //[Description("Rechazado")]
        //Rechazado = 6
    }
    public enum EstadoRolesChapter
    {
        [Description("Pendiente Registro")]
        PendienteRegistro = 0,
        [Description("Aprobado Registro")]
        AprobadoRegistro = 1,
        [Description("Pendiente Eliminación")]
        PendienteEliminacion = 2,
        [Description("Aprobado Eliminación")]
        AprobadoEliminacion = 3,
        [Description("Pendiente Registro")]
        ObservadoRegistro = 4
    }
    public enum TipoSolicitudRolFunciones
    {
        [Description("Sin Tipo")]
        SinTipo = 0,
        [Description("Registro")]
        Registro = 1,
        [Description("Eliminacion")]
        Eliminacion = 2,
    }
    public enum TipoResponsableSolicitud
    {
        [Description("Owner de Producto")]
        Owner = 1,
        [Description("Analista de Seguridad")]
        Seguridad = 2,
    }
    public enum InfraestructurasSeleccionadas
    {
        [Description("CLOUD PAAS")]
        CloudPAAS = 185,
        [Description("CLOUD IASS")]
        CloudIASS = 190
    }
    public enum TipoCodigoReservado
    {
        [Description("Código de Aplicación")]
        CodigoApp = 1,
        [Description("Código de Interfaz")]
        CodigoInterfaz = 2
    }
    public enum TippRegistroDato
    {
        [Description("Obligatorio")]
        Obligatorio = 1,
        [Description("Opcional")]
        Opcional = 2
    }
    public enum NivelConfiabilidad
    {
        [Description("Alto")]
        Alto = 1,
        [Description("Medio")]
        Medio = 2,
        [Description("Bajo")]
        Bajo = 3,
        [Description("Ninguno")]
        Ninguno = -1
    }

    public enum ModoLlenado
    {
        [Description("Automático")]
        Automatico = 1,
        [Description("Manual")]
        Manual = 2,
        [Description("Manual - Automático")]
        ManualAutomatico = 3
    }
    public enum ActivoAplica
    {
        [Description("Para Todas")]
        ParaTodas = 1,
        [Description("Solo APP IT")]
        SoloAPPIT = 2,
        [Description("Solo User IT")]
        SoloUserIT = 3
    }

    public enum EstadoReactivacion
    {
        [Description("Aplicación Reactivada")]
        AplicaciónReactivada = 1,
        [Description("Aplicación en Proceso de Reactivacion")]
        AplicaciónProcesoReactivacion = 2
    }
    public enum TipoEliminacion
    {
        [Description("Eliminación Administrativa")]
        EliminacionAdministrativa = 1,
        [Description("Pasó por proceso de eliminación")]
        PasoProcesoEliminacion = 2
    }
    public enum TipoConsulta
    {
        [Description("Consulta general")]
        ConsultaGeneral = 1,
        [Description("Consulta de roles de una aplicación")]
        ConsultaRolesAplicacion = 2,
        [Description("Consulta sobre el proceso")]
        ConsultaProceso = 3,
        [Description("Consulta por información histórica")]
        ConsultaInformacionHistorica = 4,
        [Description("Comentarios")]
        Comentarios = 5
    }
    public enum ApplicationState
    {
        [Description("Error")]
        Error = 0,
        [Description("En Desarrollo")]
        EnDesarrollo = 1,
        [Description("Vigente")]
        Vigente = 2,
        [Description("No Vigente")]
        NoVigente = 3,
        [Description("Eliminada")]
        Eliminada = 4
    }

    public enum EstadoConsulta
    {
        [Description("Respondida")]
        Respondida = 1,
        [Description("Sin Responder")]
        SinResponder = 2
    }


    public enum FileType
    {
        [Description("Desestimado")]
        ArchivoDesestimacion = 1,
        [Description("Seguridad")]
        ArchivoSeguridad = 2,
        [Description("SeguridadTemporal")]
        ArchivoSeguridadTemporal = 3
    }


    public enum ApplicationSituationRegister
    {
        [Description("Registro parcial")]
        RegistroParcial = 1,
        [Description("Registro completo")]
        RegistroCompleto = 2
    }

    public enum ClaseSolicitud
    {
        [Description("Registro")]
        Registro = 1,
        [Description("Modificación")]
        Modificacion = 2
    }
    public enum TipoSolicitud
    {
        [Description("No Vigente")]
        NoVigente = 1,
        [Description("Regreso de No Vigente")]
        RegresoNoVigente = 2,
        [Description("Eliminacion")]
        Eliminacion = 3,
        [Description("Modificacion")]
        Modificacion = 4,
        [Description("RevertirEliminacion")]
        RevertirEliminacion = 5
    }

    public enum EstadoSolicitud
    {
        [Description("Aprobada")]
        Aprobada = 1,
        [Description("Pendiente por el portafolio")]
        Pendiente = 2,
        [Description("Rechazada")]
        Rechazada = 3,
        [Description("Pendiente por el custodio")]
        PendienteCustodio = 4,
        [Description("Desestimada")]
        Desestimada = 5,
        [Description("Observada")]
        Observada = 6,
        [Description("Observada por Owner / Líder usuario")]
        ObservadaPorCustodio = 7
    }

    public enum ApplicationManagerRole
    {
        [Description("Tribe Technical Lead")]
        TTL = 1,
        [Description("Jefe de Equipo")]
        JefeDeEquipo = 2,
        [Description("Bróker de Sistemas")]
        Broker = 3,
        [Description("Owner/Líder Usuario")]
        Owner = 4,
        [Description("Usuario Autorizador")]
        UsuarioAutorizador = 5,
        [Description("Experto/Especialista/Lider Técnico")]
        Experto = 6,
        [Description("Tribe Lead")]
        TL = 7,
        [Description("Analista de Riesgo")]
        AnalistaRiesgo = 8,
        [Description("Solicitante")]
        Solicitante = 9,
        [Description("Gobierno User IT")]
        GobiernoUserIT = 10,
        [Description("Arquitecto evaluador")]
        ArquitectoEvaluador = 11,
        [Description("Arquitecto de Tecnología")]
        ArquitectoTI = 12,
        [Description("DevSecOps")]
        DevSecOps = 13,
        [Description("Administrador")]
        Administrador = 14,
        [Description("AIO")]
        AIO = 15,
        [Description("Administrador Portafolio")]
        AdministradorPortafolio = 16,
        [Description("Arquitecto de Soluciones")]
        ArquitectoSoluciones = 17

    }

    public enum Flow
    {
        [Description("Registro de aplicación")]
        Registro = 1,
        [Description("Actualización de aplicación")]
        Modificacion = 2,
        [Description("Eliminación de aplicación")]
        Eliminacion = 3
    }

    public enum ActionManager
    {
        Aprobar = 1,
        Rechazar = 2,
        Transferir = 3,
        Observar = 4
    }

    public enum NotificationFlow
    {
        M1RegistroAplicacion = 1,
        M1CompletarRegistroAplicacion = 2,
        M1NotificacionRegistroAplicaciónCustodioInformacion = 3,
        M2AprobacionArquitectoEvaluador = 4,
        M2RechazoArquitectoEvaluador = 5,
        M2TransferenciaArquitectoEvaluador = 6,
        M2AprobacionGobiernoUserIT = 7,
        M2RechazoGobiernoUserIT = 8,
        M2AprobacionArquitectoTecnologia = 9,
        M2RechazoArquitectoTecnologia = 10,
        M2ConfirmacionRegistroDevsecops = 11,
        M2ConfirmacionRegistroAIO = 12,
        M2AprobacionLiderUsuario = 13,
        M2RechazoLiderUsuario = 14,
        M2AprobacionJefeEquipo = 15,
        M2TransferenciaJefeEquipo = 16,
        M2RechazoJefeEquipo = 17,
        M2AprobacionTTL = 18,
        M2TransferenciaTTL = 19,
        M2RechazoTTL = 20,
        M2AplicacionSituaciónRegistroCompleto = 21,
        M2AprobacionPortafolio = 22,
        M2RechazoPortafolio = 23,
        M2AsignacionOwner = 24,
        M2AsignacionJefeEquipo = 25,
        M2AsignacionTTL = 26,
        M2AsignacionNuevoArquitectoEvaluador = 27,
        M2AsignacionNuevoJefeEquipo = 28,
        M2AsignacionNuevoTTL = 29,
        M2ModificacionModeloEntrega = 30,
        ActualizacionAIO = 32,
        M3ARO = 36,
        M3CriticidadFinal = 37,
        M2AsignacionUsuarioAutorizador = 38,
        RecurrenteSolicitantes = 39,
        RecurrenteAprobadores = 40,
        M2ObservacionAdministradorSolicitante = 41,
        RecurrenteAdministradores = 42,
        ActualizacionArquitectoEvaluador = 43,
        ActualizacionTransferenciaArquitectoEvaluador = 44,
        ActualizacionAsignacionNuevoArquitectoEvaluador = 45,
        ActualizacionRechazoArquitectoEvaluador = 46,
        ActualizacionArquitectoTecnologia = 47,
        ActualizacionAsignacionNuevoOwner = 48,
        ActualizacionCreaciónSolicitudModificación = 49,
        ActualizacionActualizacionDatosNoRequeridosAprobacion = 50,
        ActualizacionAprobacionSolicitudModificacionPortafolio = 51,
        ActualizacionRechazoSolicitudModificacionPortafolio = 52,
        ActualizacionAsignacionSolicitudModificacionDevSecOps = 53,
        ActualizacionAprobacionSolicitudModificacionDevSecOps = 54,
        ActualizacionRechazoSolicitudModificacionDevSecOps = 55,
        ActualizacionModificacionSquad = 56,
        ActualizacionAsignacionSolicitudModificacionEquipo = 57,
        ActualizacionAprobacionSolicitudModificacionEquipo = 58,
        ActualizacionRechazoSolicitudModificacionEquipo = 59,
        ActualizacionAsignacionSolicitudModificacionOwner = 60,
        ActualizacionAprobacionSolicitudModificacionOwner = 61,
        ActualizacionRechazoSolicitudModificacionOwner = 62,
        ActualizacionModificaciónEquipoOriginal = 63,
        ActualizacionModificaciónEquipoDestino = 64,
        ActualizacionAprobacionSolicitudModificacionSquad = 65,
        ActualizacionRechazoSolicitudModificacionSquad = 66,
        EliminacionCreacionSolicitud = 67,
        EliminacionAprobacionSolicitud = 68,
        EliminacionRechazoSolicitud = 69,
        EnvioConsulta = 70,
        RespuestaConsulta = 71,
        ActualizacionPorGobiernoUserIT = 72,
        ActualizacionUsuariosAutorizadores = 73,
        ActualizacionEstadoParaGobiernoUserIT = 74,
        ActualizacionGestionadoPor = 75,
        ActualizacionConfirmacionCamposGobiernoUserIT = 76,
        ActualizacionRechazoCamposGobiernoUserIT = 77,
        DesestimacionSolicitante = 78,
        ReversionAplicacionNoVigente = 79,
        ConformidadActualizacionApp = 80,
        ActualizacionCamposPortafolio = 81,
        AprobacionSsolicitudEliminaciónCustodios = 82,
        RechazoSolicitudEliminaciónCustodios = 83,
        ObservaciónSolicitudEliminación = 84,
        ReenvíoSolicitudEliminacion = 85,
        ActualizacionResponsableAplicacion = 86,
        ReversiónEliminaciónSolicitud = 87,
        AprobacionReversiónEliminaciónSolicitud = 88,
        RechazoReversiónEliminaciónSolicitud = 89,
        DesestimacionEliminacion = 90,
        Reactivacion = 91,
        CustodiosReactivacion = 92,
        DesestimacionEliminacionSolicitante = 93,
        RecurrenteOwnersEliminacionPendientes = 94,
        RecurrentePortafolioEliminacionPendientes = 95,
        RecurrentePortafolioReactivacionPendientes = 96,
        RecurrenteSolicitanteEliminacionPendientes = 97,
        RecurrenteSolicitanteReactivacionPendientes = 98,
        ModificacionConsultasPortafolio = 99,
        EliminacionConsultasPortafolio = 100,
        RegistroAplicacionGrupoRemedy = 101,
        RegistroParcialAplicacion = 102,
        ObservacionArquitectoEvaluador = 103,
        ActualizacionObservaciónArquitectoEvaluador = 104,
        ObservacionArquitectoTecnologia = 105,
        ActualizacionObervacionArquitectoTecnologia = 106,
        ObservacionDevSecOps = 107,
        ActualizacionObervacionDevSecOps = 108,
        ObservacionJefeEquipo = 109,
        ActualizacionObervacionJefeEquipo = 110,
        ObservacionTTL = 111,
        ActualizacionObervacionTTL = 112,
        ObservacionLiderUsuario = 113,
        ActualizacionObervacionLiderUsuario = 114,
        ObservacionUserIT = 115,
        ActualizacionObervacionUserIT = 117,
        ObservacionSolicitudEliminaciónCustodios = 118,
        RecurrenteQualysEquiposNoEncontradosTecnologiasInstaladasNoRegistradas = 119,
        EnvioSolicitudAprobacionRol = 120,
        M2AprobacionArquitectoSolucion = 121,
        M2CambioArquitectoSolucion = 122,
        M2CambioArquitectoEvaluador = 123,
        M2TransferenciaArquitectoTecnologia= 124,
        M2AsignacionNuevoArquitectoTecnologia = 125,
        M2TransferenciaOwner = 126,
        M2AsignacionNuevoOwner = 127,
        M2TransferenciaArquitectoSolucion = 128,
        M2AsignacionNuevoArquitectoSolucion = 129,
        ActualizacionTransferenciaArquitectoTecnologia = 130,
        ActualizacionAsignacionNuevoArquitectoTecnologia = 131,
        ActualizacionTransferenciaOwner = 132,
        ActualizacionTransferenciaTTL = 133,
        ActualizacionAsignacionNuevoTLL = 134,
        ActualizacionTransferenciaArquitectoSolucion = 135,
        ActualizacionAsignacionNuevoArquitectoSolucion = 136,
        ActualizacionArquitectoSolucion = 137,
        M2ActivacionRegistroPiezaCross = 140,
        ActualizacionRegistroPiezaCross = 141,
    }

    public enum BIA
    {
        [Description("Muy Alta")]
        MuyAlta = 4,
        [Description("Alta")]
        Alta = 3,
        [Description("Media")]
        Media = 2,
        [Description("Baja")]
        Baja = 1
    }

    public enum ClasificacionActivos
    {
        [Description("Restringido")]
        Restringido = 1,
        [Description("Uso Interno")]
        UsoInterno = 2,
        [Description("Público")]
        Publico = 3
    }

    public enum CriticidadFinal
    {
        [Description("Muy Alta")]
        MuyAlta = 4,
        [Description("Alta")]
        Alta = 3,
        [Description("Media")]
        Media = 2,
        [Description("Baja")]
        Baja = 1
    }

    public enum Campos
    {
        CodigoAPT = 1,
        NombreAplicacion = 2,
        Descripcion = 3,
        TipoImplementacion = 4,
        GestionadoPor = 5,
        ModeloEntrega = 6,
        CódigoAPTPadre = 7,
        EstadoAplicacion = 8,
        CodigoInterfaz = 9,
        FechaRegistro = 10,
        UnidadUsuaria = 11,
        NombreEquipo = 12,
        Experto = 13,
        EntidadesUsuarias = 14,
        TipoDesarrollo = 15,
        ProveedorDesarrollo = 16,
        InfraestructuraAplicacion = 17,
        CodAppReemplazo = 18,
        ArquitectoNegocio = 19,
        TipoActivoInformacion = 20,
        AreaBIAN = 21,
        DominioBIAN = 22,
        JefaturaATI = 23,
        CategoriaTecnologica = 24,
        ClasificacionTecnica = 25,
        SubClasificacionTecnica = 26,
        Gestor = 27,
        GerenciaCentral = 28,
        Division = 29,
        Area = 30,
        BrokerSistemas = 31,
        TribeLead = 32,
        TribeTechnicalLead = 33,
        JefeEquipo = 34,
        LiderUsuario = 35,
        GrupoTicketRemedy = 36,
        URLCertificadosDigitales = 37,
        FechaPrimerPase = 38,
        ProductoRepresentativo = 39,
        MenorRTO = 40,
        MayorGradoInterrupción = 41,
        CriticidadAppBIA = 42,
        ClasificacionActivo = 43,
        NuevaCriticidadFinal = 44,
        TOBE = 45,
        TIERPreProduccion = 46,
        TIERProduccion = 47,
        SituacionRegistro = 48,
        FlagPirata = 49,
        FechaFlagPirata = 50,
        MetodoAutenticacion = 51,
        MétodoAutorizacion = 52,
        ResumenEstandares = 53,
        NivelCumplimientoSeguridad = 54,
        ArchivoAdjuntoSeguridad = 55,
        CapaFuncional = 67,
        ArquitectoSolucion = 68,
        HorarioFuncionamiento = 69
    }
    public enum FlujoTipoSolicitud
    {
        [Description("Actualizacion")]
        Actualizacion = 1,
        [Description("Equivalencias")]
        Equivalencias = 2,
        [Description("Desactivacion")]
        Desactivacion = 3
    }

    public enum FlujoEstadoSolicitud
    {
        [Description("Pendiente")]
        Pendiente = 1,
        [Description("Aprobado")]
        Aprobado = 2,
        [Description("Rechazado")]
        Rechazado = 3
    }
    public enum FlujoTipo
    {
        //[Description("UnaAprobacion")]
        //UnaAprobacion = 1,
        //[Description("DobleAprobacion")]
        //DobleAprobacion = 2
        [Description("Estandares")]
        Estandares = 1,
        [Description("CVT")]
        CVT = 2,
        [Description("Squad")]
        Squad = 3
    }

    public enum TipoActivoProducto
    {
        [Description("Tecnologia")]
        Tecnologia = 1,
        [Description("Herramienta")]
        Herramienta = 2,
    }

    public enum FlujoConfiguracionTecnologiaCampos
    {
        [Description("Fabricante")]
        Fabricante = 1,
        [Description("NombreTecnologia")]
        NombreTecnologia = 2,
        [Description("Dominio")]
        Dominio = 3,
        [Description("Subdominio")]
        Subdominio = 4,
        [Description("TipoProducto")]
        TipoProducto = 5,
        [Description("TipoCicloVida")]
        TipoCicloVida = 6,
        [Description("TribuCOE")]
        TribuCOE = 7,
        [Description("SquadEquipo")]
        SquadEquipo = 8,
        [Description("ResponsableUnidad")]
        ResponsableUnidad = 9,
        [Description("Version")]
        Version = 10,
        [Description("IndicarPlataforma")]
        IndicarPlataforma = 11,
        [Description("TipoTecnologia")]
        TipoTecnologia = 12,
        [Description("AplicacionAsociada")]
        AplicacionAsociada = 13,
        [Description("FechaLanzamiento")]
        FechaLanzamiento = 14,
        [Description("FlagFechaFinSoporte")]
        FlagFechaFinSoporte = 15,
        [Description("MotivoFechaIndefinida")]
        MotivoFechaIndefinida = 16,
        [Description("UrlFechaIndefinida")]
        UrlFechaIndefinida = 17,
        [Description("Fuente")]
        Fuente = 18,
        [Description("FechaCalculo")]
        FechaCalculo = 19,
        [Description("FechaExtendida")]
        FechaExtendida = 20,
        [Description("FechaFinSoporte")]
        FechaFinSoporte = 21,
        [Description("FechaFinInterna")]
        FechaFinInterna = 22,
        [Description("ComentariosFechaFin")]
        ComentariosFechaFin = 23,
        [Description("FlagTieneEquivalencias")]
        FlagTieneEquivalencias = 24,
        [Description("MotivoNoEquivalencias")]
        MotivoNoEquivalencias = 25,
        [Description("ActualizarEquivalencias")]
        ActualizarEquivalencias = 26,
        [Description("Estado")]
        Estado = 27,
        //[Description("TipoActivo")]
        //TipoActivo = 28,
        //[Description("MenorRTO")]
        //MenorRTO = 29,
        //[Description("MayorRTO")]
        //MayorRTO = 30,
        //[Description("TierPreProduccion")]
        //TierPreProduccion = 31,
        //[Description("TierProduccion")]
        //TierProduccion = 32,
        //[Description("ModeloEntrega")]
        //ModeloEntrega = 33,
        [Description("DescripcionTecnologia")]
        DescripcionTecnologia = 28,
        [Description("EsquemaLicenciamiento")]
        EsquemaLicenciamiento = 29,
        [Description("UrlConfluenceId")]
        UrlConfluenceId = 30,
        [Description("UrlConfluence")]
        UrlConfluence = 31,
        [Description("CasoUso")]
        CasoUso = 32,
        [Description("CompatibilidadSoId")]
        CompatibilidadSoId = 33,
        [Description("CompatibilidadCloudId")]
        CompatibilidadCloudId = 34,
        [Description("EquipoAprovisionamiento")]
        EquipoAprovisionamiento = 35,
        [Description("AutomatizacionImplementacionId")]
        AutomatizacionImplementacionId = 36,
        [Description("EsquemaMonitoreo")]
        EsquemaMonitoreo = 37,
        [Description("GrupoSoporteRemedy")]
        GrupoSoporteRemedy = 38,
        //[Description("MetodoAutenticacion")]
        //MetodoAutenticacion = 45,
        //[Description("MetodoAutorizacion")]
        //MetodoAutorizacion = 46,
        //[Description("ExpertoLiderTecnico")]
        //ExpertoLiderTecnico = 47,
        [Description("FlagSiteEstandar")]
        FlagSiteEstandar = 39,
        [Description("RevisionSeguridad")]
        RevisionSeguridad = 40, //40
        [Description("LineaBaseSeg")]
        LineaBaseSeg = 41,
        [Description("ClaveTecnologia")]
        ClaveTecnologia = 42,
        [Description("FechaRegistro")]
        FechaRegistro = 43,
        [Description("UsuarioRegistro")]
        UsuarioRegistro = 44,
        [Description("EstadoTecnologia")]
        EstadoTecnologia = 45,
        [Description("CodigoProducto")]
        CodigoProducto = 46,
        [Description("ConfArqSeqId")]
        ConfArqSeqId = 47,
        [Description("ConfArqTecId")]
        ConfArqTecId = 48,
        //[Description("CodigoInterfaz")]
        //CodigoInterfaz = 58,
        //[Description("CriticidadBIA")]
        //CriticidadBIA = 59,
        //[Description("ClasificacionActivo")]
        //ClasificacionActivo = 60,
        //[Description("CriticidadFinal")]
        //CriticidadFinal = 61,
        [Description("TribuCOENombre")]
        TribuCOENombre = 49,
        [Description("ResponsableUnidadNombre")]
        ResponsableUnidadNombre = 50,
        [Description("SquadEquipoNombre")]
        SquadEquipoNombre = 51,
        [Description("ResponsableSquad")]
        ResponsableSquad = 52,
        [Description("ResponsableSquadNombre")]
        ResponsableSquadNombre = 53,
        [Description("ResponsableSquadMatricula")]
        ResponsableSquadMatricula = 54,
        [Description("EliminarEquivalencias")]
        EliminarEquivalencias = 55,
        [Description("AplicacionEliminada")]
        AplicacionEliminada = 56,
        //[Description("GrupoSoporteRemedyId")]
        //GrupoSoporteRemedyId = 70,
        [Description("ListaExpertos")]
        ListaExpertos = 57,
        [Description("ListaExpertosEliminar")]
        ListaExpertosEliminar = 58,
        [Description("MotivoEliminacionTecnologia")]
        MotivoEliminacionTecnologia = 59,
        [Description("DescripcionEliminacionTecnologia")]
        DescripcionEliminacionTecnologia = 60,
        [Description("NombreProducto")]
        NombreProducto  = 61,
        [Description("EstadoId")]
        EstadoId = 62,
        [Description("FechaDeprecada")]
        FechaDeprecada = 63,
        [Description("TecReemplazoDepId")]
        TecReemplazoDepId = 64
    }
    public enum TipoCuenta
    {
        [Description("Integrado con AD")]
        IntegradoAD = 1,
        [Description("No integrado con AD")]
        NoIntegradoAD = 2
    }
    public enum AmbienteRolProducto
    {
        [Description("Desarrollo")]
        Desarrollo = 1,
        [Description("Certificación")]
        Certificacion = 2,
        [Description("Producción")]
        Produccion = 3
    }
}
