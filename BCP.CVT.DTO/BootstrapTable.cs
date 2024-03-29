﻿using BCP.CVT.Cross;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BootstrapTable<T>
    {
        public int Total { get; set; }
        public List<T> Rows { get; set; }
    }

    public class EntidadRetorno
    {
        public int CodigoRetorno { get; set; }
        public string Descripcion { get; set; }
    }

    public class Paginacion
    {
        public int id { get; set; }

        public int areaId { get; set; }

        public int tipoCodigo { get; set; }
        public string nombre { get; set; }

        public string IP { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sortName { get; set; }
        public string sortOrder { get; set; }
        public string username { get; set; }
        public string codigoAPT { get; set; }

        public string so { get; set; }
        public int ambienteId { get; set; }
        public int tipoId { get; set; }
        public int desId { get; set; }
        public int subsiId { get; set; }
        public int PerfilId { get; set; }
        public string Matricula { get; set; }
        public string RolUsuario { get; set; }

        public string ambienteIds { get; set; }
        public string tipoIds { get; set; }
        public string desIds { get; set; }
        public string subsiIds { get; set; }
        public string tabla { get; set; }
        public int ParametricaId { get; set; }
        public bool Activos { get; set; }

        public int[] arrTipoEquipo { get; set; }

        public string Desde { get; set; }
        public string Hasta { get; set; }
        public int Respondido { get; set; }
        public int? estadoObsolescenciaId { get; set; }
        public int? dominioId { get; set; }
        public int? subDominioId { get; set; }
        public int? tipoProductoId { get; set; }
        public int productoId { get; set; }

        public int motivo { get; set; }
        public int origen { get; set; }
        public string fabricante { get; set; }
        public string qualyId { get; set; }
        public string titulo { get; set; }
        public string nivelSeveridad { get; set; }
        public string productoStr { get; set; }
        public string equipoStr { get; set; }
        public string tecnologiaStr { get; set; }
        public string equipo { get; set; }
        public int equipoId { get; set; }
        public string tipoVulnerabilidad { get; set; }
        public bool asignadas { get; set; }
        public int? flagAprobadoEquipo { get; set; }
        public int? flagAprobado { get; set; }
        public string nroSerie { get; set; }
        public string hostName { get; set; }
        public int umbralId { get; set; }

        public int? flagActivo { get; set; }
        public string lstServidores { get; set; }
        public string NombreArchivo { get; set; }
        public string Ticket { get; set; }
        public string FechaBusIni { get; set; }
        public string FechaBusFin{ get; set; }
        public List<int> EstadoSolicitudBaja { get; set; }
        public bool isApprovedBaja { get; set; }
    }

    public class PaginationAzure : Paginacion
    {
        public DateTime DateFilter { get; set; }
    }

    public class PaginationTeamSquad : Paginacion
    {
        public int GestionadoPorId { get; set; }
    }

    public class PaginationBlob : Paginacion
    {
        public string Container { get; set; }
        public DateTime? DateFilter { get; set; }
        public string Prefix { get; set; }
    }

    public class PaginacionEstandar : Paginacion
    {
        public int? TipoTecnologiaId { get; set; }
        public int? TipoEstandarId { get; set; }
        public int? EstadoId { get; set; }
        public string SubdominioIds { get; set; }
        public string DominioIds { get; set; }
        public string EstadoIds { get; set; }
        public string TipoTecnologiaIds { get; set; }
        public string Tecnologia { get; set; }
        public bool GetAll { get; set; }
        public string AplicaIds { get; set; }
        public string CompatibilidadSOIds { get; set; }
        public string CompatibilidadCloudIds { get; set; }
    }

    public class PaginacionIP : Paginacion
    {
        public string Zona { get; set; }
        public string Addm { get; set; }
        public string Fuente { get; set; }
        public string Ips { get; set; }
        public DateTime Fecha { get; set; }
        public int Identificacion { get; set; }
        public string CMDB { get; set; }
    }

    public class PaginacionSolicitud : Paginacion
    {
        public int SolicitudAplicacionId { get; set; }
        public string CodigoApt { get; set; }
        public int TipoSolicitud { get; set; }
        public List<int> EstadoAsignacion { get; set; }
        public int EstadoAsignacionSol { get; set; }
        public DateTime FechaRegistroSolicitud { get; set; }
        public DateTime FechaAtencionSolicitud { get; set; }
        public DateTime? FechaAtencionSolicitud2 { get; set; }
        public DateTime? FechaRegistroSolicitud2 { get; set; }
        public string CodigoTribu { get; set; }
        public string RolSeguridad { get; set; }
        public int AppId { get; set; }
        public int BandejaId { get; set; }
        public List<int> EstadoSolicitud { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool? FlagAprobacion { get; set; }
        public string ModeloEntrega { get; set; }
        public int EstadoSolicitudUnico { get; set; }
        public int ItemId { get; set; }
        public string NuevoValor { get; set; }
        public string Matricula { get; set; }
        public string Perfil { get; set; }
        public string Producto { get; set; }
        public string Chapter { get; set; }
        public string Funcion { get; set; }
        public List<string> FuncionMultiple { get; set; }
        public string Descripcion { get; set; }
        public int DominioId { get; set; }
        public int SubDominioId { get; set; }
        public int ProductoId { get; set; }
        public string Rol { get; set; }
        public string GrupoRed { get; set; }
        public string NombreUsuario { get; set; }
        public int RolProductoId { get; set; }
        public int FuncionProductoId { get; set; }
        public int SolDetalleId { get; set; }
        public string Tribu { get; set; }
        public string TribuId { get; set; }
        public string SquadId { get; set; }
        public string Comentario { get; set; }
        public List<int> EstadoSolicitudFlujo { get; set; }
        public int TecnologiaId { get; set; }
        public int TipoCuenta { get; set; }
        public AmbienteGrupoRedDTO[] Ambiente { get; set; }

    }
    public class PaginacionSolicitudRol : Paginacion
    {
        public int IdProducto { get; set; }
        public string Comentario { get; set; }
        public string Owner { get; set; }
        public string IdOwner { get; set; }
        public string Seguridad { get; set; }
        public string IdSeguridad { get; set; }
        public string Matricula { get; set; }
        public int IdSolicitud { get; set; }
        public string MailEspecialista { get; set; }
        public string MailSeguridad { get; set; }
        public string MailOwner { get; set; }
        public int IdTipoSolicitud { get; set; }
    }
    public class PaginacionTecSug : Paginacion
    {
        public List<int> subIds { get; set; }
    }

    public class CambioDominio
    {
        public int SubdominioId { get; set; }
        public int DominioId { get; set; }
    }

    public class ActualizarTecnologia
    {
        public string TipoTecnologia { get; set; }
        public int TecnologiaId { get; set; }
        public string Usuario { get; set; }

    }

    public class PaginacionTec
    {
        public int domId { get; set; }
        public int subdomId { get; set; }
        public string nombre { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sortName { get; set; }
        public string sortOrder { get; set; }
        public string aplica { get; set; }
        public string codigo { get; set; }
        public string dueno { get; set; }
        public string equipo { get; set; }
    }

    public class PaginacionTecSTD : PaginacionTec
    {
        public string casoUso { get; set; }
        public int estadoId { get; set; }
        public string famId { get; set; }
        public int fecId { get; set; }
        public int tipoTecId { get; set; }
        public int estObsId { get; set; }
        public int? flagActivo { get; set; }

        public List<int> domIds { get; set; }
        public List<int> subdomIds { get; set; }
        public List<int> estadoIds { get; set; }
        public List<int> tipoTecIds { get; set; }
        public List<int> estObsIds { get; set; }
    }

    public class PaginacionNewTec : PaginacionTec
    {
        public int? tecnologiaId { get; set; }
        public string casoUso { get; set; }
        public int estadoId { get; set; }
        public int? prodId { get; set; }
        public int fecId { get; set; }
        public int tipoTecId { get; set; }
        public int estObsId { get; set; }
        public int? flagActivo { get; set; }
        public List<int> resolucionCambio { get; set; }
        public List<int> domIds { get; set; }
        public List<int> subdomIds { get; set; }
        public List<int> estadoIds { get; set; }
        public List<int> tipoTecIds { get; set; }
        public List<int> estObsIds { get; set; }
        public string tribuCoeStr { get; set; }
        public string squadStr { get; set; }
        public string Perfil { get; set; }
        public string Matricula { get; set; }
    }

    public class ParametroTec
    {
        public List<int> itemsTecId { get; set; }
        public List<int> itemsRemoveTecId { get; set; }
        public int arqId { get; set; }
    }

    public class ParametroTecEq
    {
        public List<int> itemsTecEqId { get; set; }
        public int tecId { get; set; }
        public List<int> itemsRemoveTecEqId { get; set; }
        public int Id { get; set; }
        public string Equivalencia { get; set; }
        public string Usuario { get; set; }
    }

    public class ParametroEstadoTec
    {
        public int id { get; set; }
        public string obs { get; set; }
        public int est { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class Parametro
    {
        public List<int> items { get; set; }
        public int id { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class ObjParametro : Parametro
    {
        public List<ObjTecnologiaNoRegistrada> itemsTec { get; set; }
    }

    public class ExportarSubdominio
    {
        public string nombre { get; set; }
        public int dominioId { get; set; }
    }
    public class ObjCambioEstado
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public int EstadoId { get; set; }
        public string Observacion { get; set; }
        public bool Flag { get; set; }
        public int ObjetoId { get; set; }
        public DateTime FechaFiltro { get; set; }
        public bool FlagAdmin { get; set; }
    }

    public class CustomAutocomplete
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string TipoId { get; set; }
        public string TipoDescripcion { get; set; }
        public string value { get; set; }
        public string Color { get; set; }
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public string ValorCampo { get; set; }
        public int Total { get; set; }
        public bool? FlagNuevo { get; set; }
        public string TipoFlujoId { get; set; }
        public int InfoCampoId { get; set; }
        public int? TipoInputId { get; set; }
        public int IdAplicacion { get; set; }
        public bool FlagActivo { get; set; }
        public int? EstadoId { get; set; }
        public bool? FlagObligatorio { get; set; }
        public bool? FlagMostrarCampo { get; set; }
        public string Suscripcion { get; set; }
    }
    public class CustomAutocompleteComponentes
    {
        public int ComponenteId { get; set; }
        public string Nombre { get; set; }
        public int TipoEquipoId { get; set; }
        public int Id
        {
            get
            {
                return this.ComponenteId;
            }
        }
        public string Descripcion
        {
            get
            {
                return this.Nombre;
            }
        }
        public string value
        {
            get
            {
                return this.Nombre;
            }
        }
    }

    public class CustomAutocompleteRemedy
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public string value { get; set; }

    }


    public class CustomAutocompleteAprobador
    {
        public int Id { get; set; }
        public string MatriculaList { get; set; }
    }

    public class CustomAutocompleteAplicacion : CustomAutocomplete
    {
        public int TablaProcedencia { get; set; }
        public bool FlagEdicion { get; set; }

        public string TipoActivo { get; set; }
        public string EstadoAplicacion { get; set; }
        public string CategoriaTecnologica { get; set; }
        public string Nombre { get; set; }
        //public string TipoActivo { get; set; }
        public string UsuarioLider { get; set; }
    }

    public class CustomAutocompleteFecha : CustomAutocomplete
    {
        public DateTime? Fecha { get; set; }
        public string FechaStr
        {

            get
            {
                return this.Fecha.HasValue ? this.Fecha.Value.ToString("dd/MM/yyyy") : "-";

            }
        }
    }

    public class CustomAutocompleteConsulta
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class CustomAutocompleteRelacion : CustomAutocomplete
    {
        public bool FlagSO { get; set; }
        public bool FlagComponente { get; set; }

        public string TipoTecnologia { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string Dominio { get; set; }
        public string Subdominio { get; set; }

        public string Indicador1 { get; set; }
        public string Indicador2 { get; set; }
        public string Indicador3 { get; set; }
    }

    public class CustomAutocompleteSubdominio
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string value { get; set; }
        public bool Activo { get; set; }

    }

    public class CustomAutoCompleteBCPUnidad : CustomAutocomplete
    {
        public string CodigoPersonalResponsable { get; set; }
        public string NombresPersonalResponsable { get; set; }
        public string MatriculaPersonalResponsable { get; set; }
    }

    public class TablaAsociada
    {
        public string Nombre { get; set; }
        public int CantidadRegistros { get; set; }
    }

    public class ParametroList
    {
        public string CodigoAPT { get; set; }
        public List<AplicacionExpertoDTO> ListIdsRegistrar { get; set; }
        public List<IdsEliminar> ListIdsEliminar { get; set; }
        public string UsuarioModificacion { get; set; }

        public class IdsEliminar
        {
            public int? AppExpId { get; set; }
            public int? TipExpId { get; set; }
            public string Matricula { get; set; }
        }
    }
    public class ComponenteConectaCon
    {
        public string CodigoAPT { get; set; }
        public List<RelacionReglasPorAppTablaDTO> ListIdsRegistrar { get; set; }
        public List<IdsEliminar> ListIdsEliminar { get; set; }
        public string UsuarioModificacion { get; set; }
        public class IdsEliminar
        {
            public int? RelacionReglasPorAppId { get; set; }
        }
        public List<IdsActivar> ListIdsActivar { get; set; }
        public class IdsActivar
        {
            public int? RelacionReglasPorAppId { get; set; }
        }
    }

    public class ParametroSubdomEquivalencia
    {
        public int id { get; set; }
        public string filtro { get; set; }
    }
    public class PaginacionRelacion : Paginacion
    {
        public string CodigoAPT { get; set; }
        public int? TipoRelacionId { get; set; }
        public int? EstadoId { get; set; }
        public int? DominioId { get; set; }
        public int? SubdominioId { get; set; }
        public int EquipoId { get; set; }
        //public int PerfilId { get; set; }
        public string Componente { get; set; }
        public string SubdominioIds { get; set; }
        public string Tecnologia { get; set; }
        public int? AmbienteId { get; set; }

        public string AmbienteIdStr { get; set; }
        public string EstadoIdStr { get; set; }
        public DateTime FechaConsulta { get; set; }

        public int Dia => FechaConsulta.Day;
        public int Mes => FechaConsulta.Month;
        public int Anio => FechaConsulta.Year;
        public int TipoEquipoId { get; set; }

        // Relaciones app-app
        public int? RelacionApp { get; set; }
        public string AplicacionRelacion { get; set; }
        public string CodigoAptOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
    }

    public class PaginacionRelacionesApps : Paginacion
    {
        public string CodigoAptOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public string AplicacionRelacion { get; set; }
        public int? TipoRelacionId { get; set; }
        public int? EstadoId { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int Dia => FechaConsulta.Day;
        public int Mes => FechaConsulta.Month;
        public int Anio => FechaConsulta.Year;
    }

    public class PaginacionRelacionFilter : Paginacion
    {
        public string TipoActivo { get; set; }
        public string GestionadoPor { get; set; }
        public string EstadoAplicacion { get; set; }
        public string JefeEquipo { get; set; }
        public string LiderUsuario { get; set; }
        public string TTL { get; set; }
        public bool All { get; set; }
    }

    public class PaginacionAplicacionServidor : Paginacion
    {
        public string Aplicacion { get; set; }
        public string Equipo { get; set; }
        //public int? AmbienteId { get; set; }
        //public List<int> AmbienteIds { get; set; }

        public string AmbienteIds { get; set; }
        //public List<string> PCIS { get; set; }

        public string PCIS { get; set; }

        public string GestionadoPor { get; set; }
        public string Jefe { get; set; }
        //public int PerfilId { get; set; }
        //public string Matricula { get; set; }

        public string RelacionAplicacion { get; set; }
        //public List<int> EstadoId { get; set; }
        public string EstadoId { get; set; }
        public string TipoRelacion { get; set; }
    }
    public class FiltrosDashboardAplicacion
    {
        public string[] GestionadoPor { get; set; }
        public string[] EstadoAplicacion { get; set; }
        public string[] UsuarioLider { get; set; }
        public string[] UsuarioAutorizador { get; set; }
        public string[] ExpertoEspecialista { get; set; }
        public string[] TipoActivo { get; set; }
        public string[] Gerencia { get; set; }
        public string[] ClasificacionTecnica { get; set; }
        public string[] SubclasificacionTecnica { get; set; }
        public CustomAutocomplete[] CodigoAPT { get; set; }

        public List<string> GestionadoPorFiltrar { get; set; }
        public List<string> UsuarioLiderFiltrar { get; set; }
        public List<string> UsuarioAutorizadorFiltrar { get; set; }
        public List<string> ExpertoEspecialistaFiltrar { get; set; }
        public List<string> CodigoAPTFiltrar { get; set; }
        public List<UnidadFondeoDTO> UnidadFondeo { get; set; }

        public string[] JefeEquipo { get; set; }
        public string[] LiderUsuario { get; set; }
        public string[] Division { get; set; }
        public string[] Experto { get; set; }
        public string[] Gestor { get; set; }
        public string[] TTL { get; set; }
        public string[] Area { get; set; }
        public string[] Unidad { get; set; }
        public string[] Broker { get; set; }
    }

    public class ComboSubdominioDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class PaginacionExcepcion : Paginacion
    {
        public string CodigoAPT { get; set; }
        public int? TecnologiaId { get; set; }
        public int? EquipoId { get; set; }
        public int? TipoExcepcionId { get; set; }
    }
    public class FiltrosAplicacion
    {
        public List<CustomAutocomplete> Criticidad { get; set; }
        public string[] Gerencia { get; set; }
        public string[] Division { get; set; }
        public string[] Unidad { get; set; }
        public string[] Area { get; set; }
        public string[] CategoriaTecnologica { get; set; }
        public string[] PlataformaBCP { get; set; }
        public string[] AreaBIAN { get; set; }
        public string[] DominioBIAN { get; set; }
        public string[] JefaturaEquipoATI { get; set; }
        public string[] EntidadResponsable { get; set; }
        public string[] TipoActivoInformacion { get; set; }
        public string[] EntidadUso { get; set; }
        public string[] GestionadoPor { get; set; }
        //public string[] OOR { get; set; }
        public string[] AmbienteInstalacion { get; set; }
        public string[] MetodoAutenticacion { get; set; }
        public string[] MetodoAutorizacion { get; set; }
        public string[] Contingencia { get; set; }
        public string[] Ubicacion { get; set; }
        public string[] TipoDesarrollo { get; set; }
        public string[] InfraestructuraAplicacion { get; set; }
        public string[] ModeloEntrega { get; set; }
        public string[] MotivoCreacion { get; set; }
        public string[] Estado { get; set; }
        public List<UnidadFondeoDTO> UnidadFondeo { get; set; }

        public string[] EstadoAll { get; set; }
        public List<CustomAutocomplete> TipoExperto { get; set; }
        public List<CustomAutocomplete> TipoExpertoPortafolio { get; set; }
        public string[] OOR { get; set; }
        public List<CustomAutocompleteConsulta> EstadoSolicitud { get; set; }

        public List<CustomAutocomplete> TipoPCI { get; set; }
        public string[] Generico { get; set; }
        public string[] CompatibilidadNavegador { get; set; }
        public string[] GrupoTicketRemedy { get; set; }
        public string[] EstadoCriticidad { get; set; }
        public string[] ClasificacionCriticidad { get; set; }
        public string[] EstadoRoadmap { get; set; }
        public string Filtro { get; set; }
        public string[] ClasificacionTecnica { get; set; }
        public string[] SubclasificacionTecnica { get; set; }
        public string[] ProcesoClave { get; set; }
        public List<CustomAutocomplete> DataTooltip { get; set; }
        public List<CustomAutocomplete> TipoActivoInformacionAll { get; set; }
        public List<CustomDataListBox> DataListBox { get; set; }
        public string[] CapaFuncional { get; set; }

    }

    public class FiltrosReporteAplicacion
    {
        public string[] EstadoAplicacion { get; set; }
    }

    public class FiltrosReporteEstadoPortafolio : Paginacion
    {
        public List<int> ListEstadoAplicacion { get; set; }
        public List<int> ListTipoActivo { get; set; }
        public string EstadoAplicacion { get; set; }
        public string TipoActivo { get; set; }
        public int GerenciaId { get; set; }
        public int DivisionId { get; set; }
        public int AreaId { get; set; }
        public bool FlgChange { get; set; }
        //public int TipoReporte { get; set; }

        public string TituloReporte { get; set; }
    }



    public class PaginacionAplicacion : Paginacion
    {
        public int? CriticidadId { get; set; }

        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Unidad { get; set; }
        public string Area { get; set; }
        public string Estado { get; set; }
        public string Clasificacion { get; set; }
        public string Infraestructura { get; set; }

        public string JefeEquipo { get; set; }
        public string Owner { get; set; }
        public string Aplicacion { get; set; }
        public List<int> EstadoSolicitud { get; set; }
        //public int PerfilId { get; set; }
        //public string Matricula { get; set; }
        public int ColumnaId { get; set; }
        public int FlagEditar { get; set; }
        public int FlagVerExportar { get; set; }

        public int ActivoAplica { get; set; }
        public int ModoLlenado { get; set; }
        public int TipoRegistro { get; set; }
        public int NivelConfiabilidad { get; set; }
        public string CodigoAPT { get; set; }
        public string TablaProcedencia { get; set; }

        public List<string> Gerencias { get; set; }
        public List<string> Divisiones { get; set; }
        public List<string> Unidades { get; set; }
        public List<string> Areas { get; set; }
        public List<string> Estados { get; set; }

        public List<string> PCIS { get; set; }

        public int TipoDesarrollo { get; set; }
        public int TipoInfraestructura { get; set; }

        public string JefeEquipoFiltro { get; set; }
        public string LiderUsuarioFiltro { get; set; }
        public string TTLFiltro { get; set; }
        public string EstadoFiltro { get; set; }
        public string SubdominioFiltro { get; set; }
        public int TipoExpertoId { get; set; }
        public string MatriculaExperto { get; set; }
        public int UnidadFondeoId { get; set; }
        public string CapaFuncional { get; set; }
        //public bool? Todos { get; set; }
    }

    public class PaginacionColumnaApp : Paginacion
    {

    }

    public class PaginacionMep : Paginacion
    {
        public string CodigoAPT { get; set; }
        public string Tribu { get; set; }
        public string Jde { get; set; }
        public string Experto { get; set; }
    }

    public class PaginaReporteGerencia : Paginacion
    {
        public string JefeEquipo { get; set; }
        public string Owner { get; set; }
        public string Experto { get; set; }
        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Gestionado { get; set; }
        public string Aplicacion { get; set; }
        public string Tecnologia { get; set; }
        public string Equipo { get; set; }
        public string Estado { get; set; }
        public string DominioId { get; set; }
        public List<int> DominioIds { get; set; }
        public string SubdominioId { get; set; }
        public List<int> SubdominioIds { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public string TipoActivo { get; set; }
        public string Gestor { get; set; }
        public string Fecha { get; set; }
        public string TTL { get; set; }
        public string Broker { get; set; }
        public DateTime FechaFiltro { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public string UnidadFondeo { get; set; }
        public int SegundoNivelId { get; set; }
        public string SegundoNivel { get; set; }
        public bool FlagProyeccion { get; set; }
        public string FechaProyeccion { get; set; }
        public DateTime FechaProyeccionFiltro { get; set; }
    }
    public class PaginaReporteHuerfanos : Paginacion
    {
        public string Equipo { get; set; }
        public int TipoEquipo { get; set; }
        public string SistemaOperativo { get; set; }
        public string FechaConsulta { get; set; }
        public List<int> TipoEquipoFiltrar { get; set; }
        public string TipoEquipoToString { get; set; }
    }
    public class PaginaReporteTecnologias : Paginacion
    {
        public int Dominio { get; set; }
        public int Subdominio { get; set; }
        public int EstadoAprobacion { get; set; }
        public int Familia { get; set; }
        public int Tipo { get; set; }
        public int EstadoObsolescencia { get; set; }
        public string Clave { get; set; }
        public int Tecnologia { get; set; }
    }
    public class PaginaReporteTecnologiasCustom : Paginacion
    {
        public string DominioIds { get; set; }
        public string SubdominioIds { get; set; }
        public string EstadoAprobacionIds { get; set; }
        public string Familia { get; set; }
        public string Tipos { get; set; }
        public string EstadoObsolescencias { get; set; }
        public string Clave { get; set; }
        public int Tecnologia { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }
    }

    public class PaginacionColumnaAplicacion : Paginacion
    {

    }

    public class PaginaReporteGraficoSubdominios
    {
        public int Tipo { get; set; }
        public string Subdominios { get; set; }
        public string FechaConsulta { get; set; }
    }
    public class DataTecnologiaNoRegistrada
    {
        public List<int> SubSugeridoIds { get; set; }
        public List<TecnologiaDTO> TecSugeridas { get; set; }
    }

    public class CustomAutocompleteTecnologiaVinculada : CustomAutocomplete
    {
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string Familia { get; set; }
    }
    public class FiltrosDashboardTecnologia : Paginacion
    {
        public List<CustomAutocomplete> Dominio { get; set; }
        public List<CustomAutocomplete> EstadoAplicacion { get; set; }
        public List<CustomAutocomplete> Subdominio { get; set; }
        public List<CustomAutocomplete> Equipos { get; set; }
        public List<CustomAutocomplete> Familia { get; set; }
        public List<CustomAutocomplete> TipoEquipo { get; set; }
        public List<CustomAutocomplete> Fabricante { get; set; }
        public List<CustomAutocomplete> DominioRed { get; set; }
        public List<CustomAutocomplete> TipoTecnologia { get; set; }
        public List<CustomAutocomplete> AgrupacionFiltro { get; set; }
        public List<CustomAutocompleteConsulta> TipoConsulta { get; set; }
        public List<CustomAutocompleteTecnologia> ClaveTecnologia { get; set; }
        public List<string> FabricanteFiltrar { get; set; }
        public List<string> ClaveTecnologiaFiltrar { get; set; }
        public List<string> EstadoAplicacionFiltrar { get; set; }
        public List<int> DominioFiltrar { get; set; }
        public List<int> SubdominioFiltrar { get; set; }
        public List<int> FamiliaFiltrar { get; set; }
        public List<int> TipoEquipoFiltrar { get; set; }
        public List<int> TipoTecnologiaFiltrar { get; set; }
        public List<string> SubsidiariaFiltrar { get; set; }
        public string Fecha { get; set; }

        public string TipoEquipoIds { get; set; }
        public int TipoEquipoId { get; set; }
        public int TipoConsultaId { get; set; }
        public int TipoTecnologiaId { get; set; }
        public int Subsidiaria { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public string Agrupacion { get; set; }
        public DateTime FechaFiltro { get; set; }
        public string OwnerFiltro { get; set; }
        public string SubdominioToString { get; set; }
        public string TipoTecnologiaToString { get; set; }
        public int TecnologiaId { get; set; }
        public string FabricanteTec { get; set; }
        public string NombreTec { get; set; }
        public int SubdominioId { get; set; }
    }
    public class PaginacionDetalleGraficoSubdominio : Paginacion
    {
        public List<int> SubdominioFiltrar { get; set; }
        public List<int> SubsidiariaFiltrar { get; set; }
        public List<int> TipoTecnologiaFiltrar { get; set; }
        public List<int> TipoEquipoFiltrar { get; set; }

        public string Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }
        public int TipoEquipoId { get; set; }
        public string SubdominioToString { get; set; }
        public int Subsidiaria { get; set; }
        public string SubsidiariaToString { get; set; }
        public string TipoTecnologiaToString { get; set; }
        public string TipoEquipoToString { get; set; }
    }
    public class DashboardBase : CustomAutocomplete
    {
        public List<CustomAutocomplete> Data { get; set; }
        public int Cantidad { get; set; }
    }

    public class DashboardEquipoBase : CustomAutocomplete
    {
        public List<CustomAutocompleteFecha> Data { get; set; }
        public int Cantidad { get; set; }
    }

    public class ListaServidoresCIS
    {
        public List<string> Servidores { get; set; }
        public List<string> ServidoresNoRegistrados { get; set; }
    }

    public class PaginacionAlerta : Paginacion
    {
        public DateTime fechaConsulta { get; set; }
    }

    public class PaginacionEquipo : Paginacion
    {
        public int tipoEquipoId { get; set; }
        public int exCalculoId { get; set; }
        public int tipoExclusionId { get; set; }
        public int? flagActivo { get; set; }
        public int? subsidiariaId { get; set; }

        public int? AppsId { get; set; }
        public string TipoEquipoFiltro { get; set; }
        public string SubsidiariaFiltro { get; set; }
        public int Subdominio { get; set; }
        public int Orden { get; set; }
        public int Tecnologia { get; set; }
        public string Nombre { get; set; }
        public string Fabricante { get; set; }
        public string NombreTecnologia { get; set; }

        public List<int> tipoEquipoIds { get; set; }
        public List<int> subsidiariaIds { get; set; }
        public string Aplicacion { get; set; }
        public string NombreEquipo { get; set; }
    }


    public class ServidorCISDTO : BaseDTO
    {
        public int CodigoId { get; set; }
        public string Servidor { get; set; }
        public string Hostname { get; set; }
        public string Ambiente { get; set; }
    }

    public class PostAutocomplete
    {
        public string filtro { get; set; }
        public int? id { get; set; }
        public string dominioIds { get; set; }
        public string subDominioIds { get; set; }
    }
    public class FiltrosDashboardEquipo
    {
        public List<CustomAutocomplete> Dominio { get; set; }
        public List<CustomAutocomplete> Subdominio { get; set; }
        public List<CustomAutocomplete> TipoEquipo { get; set; }
        public List<CustomAutocomplete> AplicacionEstado { get; set; }

        public List<int> DominioFiltrar { get; set; }
        public List<int> SubdominioFiltrar { get; set; }
        public int EquipoId { get; set; }
        public string EquipoNombre { get; set; }
    }

    public class FiltrosDashboardRenovacionTecnologicaTI
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
    }
    public class EquipoAutocomplete : CustomAutocomplete
    {
        public string Ambiente { get; set; }
        public string SO { get; set; }
        public string Caracteristica { get; set; }
        public string GestionadoPor { get; set; }
    }

    public class CustomAutocompleteTecnologia : CustomAutocomplete
    {
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string Familia { get; set; }
        public string ActivoDetalle { get; set; }
        public string Fabricante { get; set; }
        public int? FamiliaId { get; set; }
    }
    public class DashboardEquipoData
    {
        public FiltrosDashboardEquipo DataFiltros { get; set; }
        public List<PlotlyDataDTO> DataPlotly { get; set; }

        public List<DashboardEquipoBase> DataPie { get; set; }
        public List<CustomAutocompleteFecha> DataTecnologiasNoRegistradas { get; set; }
        public string Proyeccion1Meses { get; set; }
        public string Proyeccion2Meses { get; set; }
    }

    public class DashboardAplicacionData
    {
        public List<DashboardBase> DataBarras { get; set; }

        public List<PlotlyDataDTO> DataPromedioAplicaciones { get; set; }
        public List<PlotlyDataDTO> DataAplicaciones { get; set; }
        public string Proyeccion1Meses { get; set; }
        public string Proyeccion2Meses { get; set; }
    }

    public class BigFixConsulta
    {
        public string MensajeRespuesta { get; set; }
        public List<string> Datos { get; set; }
        public List<string> Tareas { get; set; }
        public string XMLRespuesta { get; set; }
    }

    public class EstadoFila
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        //public int AmbienteId { get; set; }
        //public string Model { get; set; }

    }

    public class EstadoCargaMasiva
    {
        public List<string> Errores { get; set; }
        public int TotalRegistros { get; set; }
    }

    public class SubdominioTecnologiaEstandar
    {
        public int DominioId { get; set; }
        public string DominioNombre { get; set; }
        public List<SubdominoTecnologiaEstandar> Subdominio { get; set; }
    }
    public class SubdominoTecnologiaEstandar
    {
        public int SubdominioId { get; set; }
        public string SubdominioNombre { get; set; }
        public List<TecnologiaEstandar> Tecnologia { get; set; }
    }
    public class TecnologiaEstandar
    {
        public int TecnologiaId { get; set; }
        public string TecnologiaNombre { get; set; }
        public int EstadoId { get; set; }
    }

    public class ObjTecnologiaVinculada
    {
        public int? Id { get; set; }
        public int[] IdsTec { get; set; }
        public string Filtro { get; set; }
    }

    public class ObjTecnologiaNoRegistrada
    {
        public string Aplicacion { get; set; }
        public int EquipoId { get; set; }
    }

    public class FiltrosDashboardTecnologiaEquipos : Paginacion
    {
        public string TecnologiaFiltrar { get; set; }
        public int TecnologiaIdFiltrar { get; set; }
        public DateTime? FechaConsulta { get; set; }
        public int IndexObs { get; set; }
        public int MesProyeccion { get; set; }
        public List<int> SubdominiosId { get; set; }
        public string Owner { get; set; }
        public int SubdominioId { get; set; }
        public string Filtro { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }
    }
    public class DashboardTecnologiaEquipoData
    {
        public TecnologiaDTO Tecnologia { get; set; }
        public List<CustomAutocomplete> DataPie { get; set; }
        public BootstrapTable<EquipoDTO> Equipos { get; set; }
        public BootstrapTable<AplicacionDTO> Aplicaciones { get; set; }
        public List<TecnologiaDTO> TecnologiaList { get; set; }
    }

    public class PaginacionHistoricoModificacion : Paginacion
    {
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class DetalleEquipoDataTecnologias
    {
        public BootstrapTable<TecnologiaDTO> Tecnologias { get; set; }
        public string Proyeccion1Meses { get; set; }
        public string Proyeccion2Meses { get; set; }
    }

    public class DetalleEquipoDataCertificadosDigitales
    {
        public BootstrapTable<CertificadoDigitalDTO> CertificadosDigitales { get; set; }
        public string Proyeccion1Meses { get; set; }
        public string Proyeccion2Meses { get; set; }
    }
    public class DetalleEquipoDataClientSecrets
    {
        public BootstrapTable<ClientSecretDTO> ClientSecrets { get; set; }
        public string Proyeccion1Meses { get; set; }
        public string Proyeccion2Meses { get; set; }
    }
    public class ParametroCertificadoDigital
    {
        public int parametro { get; set; }
    }

    public class ParametroApis
    {
        public int parametro { get; set; }
    }
    public class ObjTecnologia
    {
        public int param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }
        public string param6 { get; set; }
        public string param7 { get; set; }
        public string param8 { get; set; }
        public string param9 { get; set; }
    }

    public class FamiliaTecnologiaParam
    {
        public int TecnologiaId { get; set; }
        public int FamiliaId { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class PaginacionVisitaSite : Paginacion
    {
        //public string Matricula { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }

    public class PaginacionMensaje : Paginacion
    {
        //Matricula
        //nombre
        //tipoId
        public DateTime? FechaRegistro { get; set; }
    }

    public class PaginacionOwner : Paginacion
    {
        public List<int> SubdominioFiltrar { get; set; }
        public List<int> TipoTecnologiaFiltrar { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }
        public string SubdominioToString { get; set; }
        public string TipoTecnologiaToString { get; set; }
        public int TipoTecnologia { get; set; }
        public string Owner { get; set; }
        public string Tecnologia { get; set; }
        public int TecnologiaId { get; set; }
    }

    public class PaginacionNotificacion : Paginacion
    {
        public string Fecha { get; set; }
        public DateTime? FechaFiltro { get; set; }
        public int TipoNotificacionId { get; set; }
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string MesesTrimestre { get; set; }
        public int? AnioFiltro { get; set; }
    }

    public class PieChart
    {
        public int cantidad { get; set; }
        public string categoria { get; set; }
        public string color { get; set; }
    }

    public class IndicadorObsolescenciaSoBd
    {
        public List<CustomAutocomplete> PieSoBd { get; set; }
        public List<CustomAutocomplete> PieSo { get; set; }
        public List<CustomAutocomplete> PieBd { get; set; }
    }



    public class FiltrosIndicadoresGerencialEquipo
    {
        public string[] NroMesesConsulta { get; set; }
        public string[] NroSubdominios { get; set; }
        public List<CustomAutocomplete> ListaTipoEquipos { get; set; }
        public List<CustomAutocomplete> ListaTipoTecnologias { get; set; }
        public List<CustomAutocomplete> ListaSubsidiarias { get; set; }
        public List<CustomAutocomplete> ListaSubdominios { get; set; }

        public List<string> TipoEquipoFiltro { get; set; }
        public List<string> TipoTecnologiaFiltro { get; set; }
        public List<string> SubsidiariasFiltro { get; set; }
        public List<string> SubdominiosFiltro { get; set; }
        public List<string> SubdominiosFiltroBase { get; set; }

        public int NroMesesFiltro { get; set; }
        public int NroSubdominiosFiltro { get; set; }
        public DateTime FechaConsultaFiltro { get; set; }

        //public bool FlagSubdominios { get; set; }

        public List<CustomAutocomplete> ListaPeriodoTiempo { get; set; }
    }

    public class IndicadoresGerencialEquipoData
    {

        public List<CustomAutocomplete> DataActualPie { get; set; }
        public List<CustomAutocomplete> DataMesesAtrasPie { get; set; }

        public List<CustomAutocomplete> DataActualSubdominiosPie { get; set; }
        public List<CustomAutocomplete> DataaMesesAtrasSubdominiosPie { get; set; }


        public List<CustomAutocomplete> DataActualPieOtros { get; set; }
        public List<CustomAutocomplete> DataMesesAtrasPieOtros { get; set; }

        public List<CustomAutocomplete> DataActualSubdominiosPieOtros { get; set; }
        public List<CustomAutocomplete> DataaMesesAtrasSubdominiosPieOtros { get; set; }

        public int TotalEquiposDescubrimientoManual { get; set; }
        public int TotalEquiposDescubrimientoAutomatico { get; set; }
        public int TotalEquipos
        {

            get
            {
                return this.TotalEquiposDescubrimientoManual + this.TotalEquiposDescubrimientoAutomatico;
            }
        }
        public int TotalEquiposHuerfanos { get; set; }
    }


    public class FiltrosIndicadoresGerencialTecnologia
    {
        public string[] NroMesesConsulta { get; set; }
        public List<CustomAutocomplete> ListaTipoTecnologias { get; set; }

        public List<string> TipoTecnologiaFiltro { get; set; }
        public int NroMesesFiltro { get; set; }
        public DateTime FechaConsultaFiltro { get; set; }

    }

    public class IndicadoresGerencialTecnologiaData
    {

        public List<CustomAutocomplete> DataActualPie { get; set; }
        public List<CustomAutocomplete> DataMesesAtrasPie { get; set; }

        public List<TecnologiasXTipoEquipo> DataTipoTecnologia { get; set; }
    }



    public class FiltrosIndicadoresGerencialAplicacion
    {
        public string[] NroMesesConsulta { get; set; }
        public string[] NroSubdominios { get; set; }
        public string[] ListaEstadoAplicacion { get; set; }
        public string[] ListaGestionadoPor { get; set; }

        public List<string> EstadoAplicacionFiltro { get; set; }
        public string GestionadoPorFiltro { get; set; }
        public int NroMesesFiltro { get; set; }
        public int NroSubdominiosFiltro { get; set; }

        public string JefeEquipoFiltrar { get; set; }
        public string LiderUsuarioFiltrar { get; set; }
        public string LiderTTLFiltrar { get; set; }
        public string GerenciaFiltrar { get; set; }
        public string DivisionFiltrar { get; set; }
        public string UnidadFiltrar { get; set; }

        public DateTime FechaConsultaFiltro { get; set; }
        public List<string> SubdominiosFiltro { get; set; }

    }


    public class IndicadoresGerencialAplicacionData
    {

        public List<CustomAutocomplete> DataActualPie { get; set; }
        public List<CustomAutocomplete> DataMesesAtrasPie { get; set; }

        public List<CustomAutocomplete> DataActualSubdominiosPie { get; set; }
        public List<CustomAutocomplete> DataaMesesAtrasSubdominiosPie { get; set; }


        public List<CustomAutocomplete> DataActualPieOtros { get; set; }
        public List<CustomAutocomplete> DataMesesAtrasPieOtros { get; set; }

        public List<CustomAutocomplete> DataActualSubdominiosPieOtros { get; set; }
        public List<CustomAutocomplete> DataaMesesAtrasSubdominiosPieOtros { get; set; }

        public int TotalAplicacionesSinRelaciones { get; set; }
        public int TotalAplicacionesConRelaciones { get; set; }

        public List<CustomRetorno> DataTotalAplicaciones { get; set; }
    }

    public class TecnologiasXTipoEquipo
    {
        public int TipoEquipoId { get; set; }
        public string TipoEquipo { get; set; }
        public int TipoTecnologiaId { get; set; }
        public string TipoTecnologia { get; set; }

        public int Total { get; set; }
    }

    public class RespuestaDTO
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
    }

    public class ParametroEstadoSolicitud
    {
        public int Id { get; set; }
        public string MotivoComentario { get; set; }
        public string Observacion { get; set; }
        public int EstadoSolicitudId { get; set; }
        public int TipoSolicitudId { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int BandejaId { get; set; }
    }
    public class TipoNotificacionesData
    {
        public List<TipoNotificacionDto> ListaTipoNotificaciones { get; set; }
        public List<CustomAutocomplete> ListaFrecuenciaNotificacion { get; set; }
    }


    public class NotificacionResponsableAplicacionDataFiltros
    {

        public List<CustomAutocomplete> ListaPortafolioResponsables { get; set; }
    }
    public class NotificacionResponsableAplicacionData
    {
        public BootstrapTable<AplicacionResponsableDto> ListaNotificaciones { get; set; }
    }
    public class PaginacionResponsableAplicacion : Paginacion
    {
        public int ResponsableAplicacionId { get; set; }
    }


    public class NotificacionResponsableAplicacionDetalleData
    {
        public BootstrapTable<AplicacionDTO> Aplicaciones { get; set; }
    }

    public class NotificacionResponsableTTLJdE
    {
        public BootstrapTable<AplicacionPortafolioResponsablesDTO> Responsables { get; set; }
    }

    public class RetornoQueryApp
    {
        public string ValorActual { get; set; }
    }

    public class ItemColumnaAppJS
    {
        public int ConfiguracionId { get; set; }
        public string title { get; set; }
        public string field { get; set; }
        public int width { get; set; }
        public int colspan { get; set; }
        public string formatter { get; set; }
        public string align => "center";
        public string valign => "middle";
    }

    public class CustomRetorno
    {
        public string Estado { get; set; }
        public int TotalEstado { get; set; }
        public int TotalAplicacion { get; set; }
    }

    public class PaginacionTecnologiaPorVencer : Paginacion
    {
        public string subdominio { get; set; }
        public string tecnologia { get; set; }
    }

    public class PaginacionReporteAplicacion : Paginacion
    {
        public string TablaProcedencia { get; set; }
        public string Columnas { get; set; }
        public int Procedencia { get; set; }

        public string Gerencia { get; set; }
        public string Division { get; set; }
        public string Unidad { get; set; }
        public string Area { get; set; }
        public string Estado { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public string Aplicacion { get; set; }

        public string TipoPCI { get; set; }
        public string TipoActivo { get; set; }

        public string GestionadoPor { get; set; }
        public string LiderUsuario { get; set; }
        public string Exportar { get; set; }
        public string applicationId { get; set; }

        public string Username { get; set; }

        public int tipoid { get; set; }
        public int respondido { get; set; }

        public int AppReactivadas { get; set; }


    }

    public class DataRetornoAplicacion
    {
        public List<AplicacionTecnologiaDto> Aplicaciones { get; set; }
        public int Total { get; set; }
        public string Responsable { get; set; }
    }

    public class PaginacionStorage
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string SoftwareBase { get; set; }
        public string Equipo { get; set; }
        public string Storage { get; set; }
        public string Aplicacion { get; set; }
        public int Ambiente { get; set; }
        public int TieneReplica { get; set; }
        public int StorageId { get; set; }
        public string StorageVolumen { get; set; }
        public int EquipoId { get; set; }
        public int UbicacionId { get; set; }
        public int StorageTierId { get; set; }
        public string NombreReporte { get; set; }
        public string jobname { get; set; }
        public string codigoAPT { get; set; }
        public string server { get; set; }
        public string backupserver { get; set; }
        public string target { get; set; }
        public string levelbackup { get; set; }
        public string outcome { get; set; }
        public string groupname { get; set; }
        public int dayBackup { get; set; }
        public int monthBackup { get; set; }
        public int yearBackup { get; set; }
        public string app { get; set; }
        public string interfaceApp { get; set; }
    }

    public class GrupoNivelCumplimiento
    {
        public int NCG { get; set; }
        public int NCET { get; set; }
        public int NCLS { get; set; }
    }

    public class UpdateAplicacionBandeja
    {
        public int Id { get; set; }
        public string CodigoAPT { get; set; }
        public string Descripcion { get; set; }
        public int BandejaId { get; set; }
        public string Usuario { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public string ModeloEntrega { get; set; }

        public string AreaBian { get; set; }
        public string DominioBian { get; set; }
        public string JefaturaAti { get; set; }
        public string ArquitectoTi { get; set; }
        public string Plataforma { get; set; }

        public int SolicitudId { get; set; }
    }

    public class InputValues
    {
        public string Id { get; set; }
        public string Valor { get; set; }
    }

    public class ObjCalculoNiveles
    {
        public int? AplicacionId { get; set; }
        public string SO { get; set; }
        public string HP { get; set; }
        public string BD { get; set; }
        public string FW { get; set; }

        public int FlagPC { get; set; }
        public string StrL { get; set; }
        public string StrM { get; set; }
        public double ValorN { get; set; }
        public int NLCS { get; set; }
    }

    public class AplicacionSolicitudDto
    {
        public int SolicitudId { get; set; }
        public int AplicacionId { get; set; }
        public string CodigoAplicacion { get; set; }
        public string NombreAplicacion { get; set; }
        public string TipoActivoInformacion { get; set; }
        public int EstadoSolicitudId { get; set; }
        public string Usuario { get; set; }

        public string Owner_LiderUsuario_ProductOwner { get; set; }
        public string Gestor_UsuarioAutorizador_ProductOwner { get; set; }
        public string TribeTechnicalLead { get; set; }
        public string Experto_Especialista { get; set; }
        public string BrokerSistemas { get; set; }
        public string JefeEquipo_ExpertoAplicacionUserIT_ProductOwner { get; set; }
        public string NombreEquipo_Squad { get; set; }
        public string ModeloEntrega { get; set; }

        public string EstadoSolicitudStr => Utilitarios.GetEnumDescription2((EEstadoSolicitudAplicacion)EstadoSolicitudId);
    }

    public class DataResults
    {
        public List<ErrorCargaMasivaDTO> Errores { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalActualizados { get; set; }
    }

    public class DataListBox
    {
        public string Id { get; set; }
        public string Valor { get; set; }
    }

    public class CustomDataListBox
    {
        public string Id { get; set; }
        public string ParametricaDescripcion { get; set; }
        public string CodigoHtml { get; set; }
        public string[] DataListBox { get; set; }
    }

    public class DataPortafolioBackup
    {
        public string Comentarios { get; set; }
        public string UsuarioCreacion { get; set; }

    }

    public class PaginacionPortafolioBackup : Paginacion
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string Usuario { get; set; }

    }

    public class DataResultAplicacion
    {
        public long AplicacionId { get; set; }
        public long SolicitudId { get; set; }
        public bool EstadoTransaccion { get; set; }

        public int SolUserIt { get; set; }
        public int BitacoraId { get; set; }
    }

    public class DataAprobadorAplicacion
    {
        public int BandejaId { get; set; }
        public int EstadoAprobacionBandeja { get; set; }
        public string MatriculaAprobadores { get; set; }
    }

    public class DataArquitectoAplicacion
    {
        public int SolicitudId { get; set; }
        public string ArquitectoTI { get; set; }
    }

    public class DataServidorAplicacion
    {
        public int AplicacionId { get; set; }
        public string Usuario { get; set; }
        public List<ServidorAplicacionDTO> ServidorDetalle { get; set; }
        public int[] IdsEliminarServidor { get; set; }
    }

    public class DataValidacion
    {
        public bool FlagExisteRelacion { get; set; }
        public string MensajeAPI { get; set; }
        public bool FlagSeEjecuta { get; set; }
    }

    public class DataColumnaBD
    {
        public int ConfiguracionId { get; set; }
        public string ColumnaBD { get; set; }
    }

    public class PaginacionAuditoriaData : Paginacion
    {
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string Campo { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
    public class PaginacionAuditoriaAPIData : Paginacion
    {
        public string APIMetodo { get; set; }
        public string APINombre { get; set; }
        public string APIUsuario { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }

    public class PaginaEvolucionInstalaciones
    {

        public List<string> TipoEquipoFiltrar { get; set; }
        public List<string> SubsidariasFiltrar { get; set; }

        public DateTime Fecha { get; set; }
        public int NroMeses { get; set; }
        public bool FlagAgruparFamilia { get; set; }
        public int? IdTecnologia { get; set; }
        public string Fabricante { get; set; }
        public string NombreTecnologia { get; set; }
        public string TipoEquipoToString
        {
            get
            {
                return this.TipoEquipoFiltrar.Count != 0 ? string.Join("|", this.TipoEquipoFiltrar) : "";

            }
        }
        public string SubsidariasToString
        {
            get
            {
                return this.SubsidariasFiltrar.Count != 0 ? string.Join("|", this.SubsidariasFiltrar) : "";

            }
        }
    }

    public class ParametroConsultor
    {
        public string ArrCodigoAPT { get; set; }
        public string MatriculaExperto { get; set; }
        public int TipoExpertoId { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Nombres { get; set; }
        public string ArrIdsAplicacionExpertos { get; set; }
    }

    public class TecnologiaAutocomplete
    {
        public int Id { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string TipoTecnologia { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
    }

    public class PaginacionUrlApp : Paginacion
    {
        public string Equipo { get; set; }
        public string Aplicacion { get; set; }
        public string URL { get; set; }
        public DateTime? Fecha { get; set; }
        public int FuenteId { get; set; }
        public bool? IsOrphan { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomAutocompleteUrlFuente
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class FiltrosReporteVariacionPortafolio
    {
        public string ListTipoActivo { get; set; }
        public string ListGestionadoPor { get; set; }
        public string ListEstadoAplicacion { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int Frecuencia { get; set; }
        public int nroPeriodos { get; set; }
        public string FormatoFecha { get; set; }
        public string periodoTiempo { get; set; }
    }
    public class FiltrosReportePedidosPortafolio
    {
        public string ListGerencia { get; set; }
        public string ListDivision { get; set; }
        public string ListArea { get; set; }
        public string ListUnidad { get; set; }
        public string ListTipoActivo { get; set; }
        public string ListGestionadoPor { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }

    public class FiltrosReporteCambioBajaResponsableGDH : Paginacion
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string matricula { get; set; }
        public string IdAplicacion { get; set; }
        public string tipoCambio { get; set; }
        public bool FlgChange { get; set; }
        //public string TituloReporte { get; set; }
    }
    public class FiltrosReporteCamposPortafolio
    {

        public string ListGerencia { get; set; }
        public string ListDivision { get; set; }
        public string ListArea { get; set; }
        public string ListUnidad { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public int Frecuencia { get; set; }
        public int NroPeriodos { get; set; }

        public int AgruparPor { get; set; }
        public string AgruparPorValores { get; set; }
        public int FiltrarPor { get; set; }
        public string FiltrarPorValores { get; set; }

        //public int TipoReporte { get; set; }
    }
    public class DistribucionReponse
    {
        public int GerenciaId { get; set; }
        public string Gerencia { get; set; }
        public List<ReportePortafolioGrafico> Chart { get; set; }
    }
    public class PaginacionVulnerabilidades : Paginacion
    {
        public int Tecnologia { get; set; }
        public int QID { get; set; }
        public string subdominios { get; set; }
        public string dominios { get; set; }
        public string estado { get; set; }
        public string codigoApt { get; set; }
        public string gestionado { get; set; }
        public string squads { get; set; }
    }
    public class PaginacionSolicitudFlujos : Paginacion
    {
        public int IdTecnologia { get; set; }
        public string Comentario { get; set; }
        public string Matricula { get; set; }
        public int IdSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string UsuarioMail { get; set; }
    }
    public class PaginaReporteLogs : Paginacion
    {
        public int ProcesoId { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }
        public string Tipo { get; set; }
        public string Capa { get; set; }
    }

    public class PaginacionDependencias : Paginacion
    {
        public int TipoRelacionamientoId { get; set; }
        public int TipoConsultaId { get; set; }
        public string TipoConexionId { get; set; }
        public int TipoEtiquetaId { get; set; }
        public int RelacionId { get; set; }
    }

    public class PaginacionDependenciasComponentes : Paginacion
    {
        public int TecnologiaId { get; set; }
        public int TipoRelacionamientoId { get; set; }
    }
    public class PaginacionDependenciasApps : Paginacion
    {
        public bool FlagProceso { get; set; }
    }
    public class PaginacionEtiquetas : Paginacion
    {
        public int EtiquetaId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool FlagDefault { get; set; }
    }

    public class PaginacionTiposRelacion : Paginacion
    {
        public int TipoRelacionId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool PorDefecto { get; set; }
    }
    public class PaginacionMotorReglas : Paginacion
    {
        public int ProcesoId { get; set; }
        public int PuertoId { get; set; }
        public int ProtocoloId { get; set; }
        public string Producto { get; set; }
    }
    public class PaginacionMotorJustificacion : Paginacion
    {
        public int ProductoId { get; set; }
        public int ProcesoId { get; set; }
        public int PuertoId { get; set; }
        public int ProtocoloId { get; set; }
        public string Aplicacion { get; set; }
        public int TecnologiaId { get; set; }
        public int EquipoId { get; set; }
        public string Fecha { get; set; }
    }
    public class PaginacionDeudaTecnica : Paginacion
    {
        public int ProductoId { get; set; }
        public int ProcesoId { get; set; }
        public int PuertoId { get; set; }
        public int ProtocoloId { get; set; }
        public string Aplicacion { get; set; }
        public string Fecha { get; set; }
    }

    public class PaginacionEquipoInstanciaBD : Paginacion
    {
        public string Nombre { get; set; }
        public int EquipoId { get; set; }
        public string CodigoApt { get; set; }
        public int Procedencia { get; set; }
    }

    #region Vista Grafica Dependencias
    public class PaginacionVGD : Paginacion
    {
        public int EquipoId { get; set; }
        public int RelacionId { get; set; }
        public int Nivel { get; set; }
        public int TipoRelacionId { get; set; }
        public int TipoComponente { get; set; }
        public int TipoEtiquetaId { get; set; }
    }
    #endregion

    #region Diagrama Infraestructura
    public class PaginacionRelacionamientoFuncion : Paginacion
    {
        public int RelacionReglasGeneralesId { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
    }
    #endregion

    public class FiltrosHardware
    {
        public List<CustomAutocomplete> GestionadoPor { get; set; }
        public List<CustomAutocomplete> TeamSquad { get; set; }
        public List<CustomAutocomplete> Modelo { get; set; }
        public List<CustomAutocomplete> Fabricante { get; set; }
        public List<CustomAutocomplete> TipoHardware { get; set; }
        public List<CustomAutocompleteConsulta> EstadoObsolescencia { get; set; }
        public List<UnidadFondeoDTO> UnidadFondeo { get; set; }





        public List<CustomAutocomplete> Criticidad { get; set; }
        public string[] Gerencia { get; set; }
        public string[] Division { get; set; }
        public string[] Unidad { get; set; }
        public string[] Area { get; set; }
        public string[] CategoriaTecnologica { get; set; }
        public string[] PlataformaBCP { get; set; }
        public string[] AreaBIAN { get; set; }
        public string[] DominioBIAN { get; set; }
        public string[] JefaturaEquipoATI { get; set; }
        public string[] EntidadResponsable { get; set; }
        public string[] TipoActivoInformacion { get; set; }
        public string[] EntidadUso { get; set; }
        //public string[] OOR { get; set; }
        public string[] AmbienteInstalacion { get; set; }
        public string[] MetodoAutenticacion { get; set; }
        public string[] MetodoAutorizacion { get; set; }
        public string[] Contingencia { get; set; }
        public string[] Ubicacion { get; set; }
        public string[] TipoDesarrollo { get; set; }
        public string[] InfraestructuraAplicacion { get; set; }
        public string[] ModeloEntrega { get; set; }
        public string[] MotivoCreacion { get; set; }
        public string[] Estado { get; set; }


        public string[] EstadoAll { get; set; }
        public List<CustomAutocomplete> TipoExperto { get; set; }
        public List<CustomAutocomplete> TipoExpertoPortafolio { get; set; }
        public string[] OOR { get; set; }
        public List<CustomAutocompleteConsulta> EstadoSolicitud { get; set; }

        public List<CustomAutocomplete> TipoPCI { get; set; }
        public string[] Generico { get; set; }
        public string[] CompatibilidadNavegador { get; set; }
        public string[] GrupoTicketRemedy { get; set; }
        public string[] EstadoCriticidad { get; set; }
        public string[] ClasificacionCriticidad { get; set; }
        public string[] EstadoRoadmap { get; set; }
        public string Filtro { get; set; }
        public string[] ClasificacionTecnica { get; set; }
        public string[] SubclasificacionTecnica { get; set; }
        public string[] ProcesoClave { get; set; }
        public List<CustomAutocomplete> DataTooltip { get; set; }
        public List<CustomAutocomplete> TipoActivoInformacionAll { get; set; }
        public List<CustomDataListBox> DataListBox { get; set; }
        public string[] CapaFuncional { get; set; }

    }
    public class PaginaHardwareDetallado : Paginacion
    {
        public string UnidadFondeo { get; set; }
        public string GestionadoPor { get; set; }
        public string TeamSquad { get; set; }
        public int EquipoId { get; set; }
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public string EstadoObsolescencia { get; set; }
        public string TipoHardware { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaFiltro { get; set; }

    }
}

