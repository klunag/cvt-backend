using BCP.CVT.DTO;
using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Services.ModelDB;

namespace BCP.CVT.Services.Interface
{
	public abstract partial class ActivosDAO : ServiceProvider
	{
		public abstract List<ActivosDTO> GetActivos(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
		public abstract List<ActivosDTO> GetActivos();
		public abstract List<FlujosRegistroDTO> GetFlujos();

        public abstract List<AuditoriaTipoActivoDTO> GetHistoricoModificacionTAI(PaginacionHistoricoModificacion pag, out int totalRows);

        public abstract int AddOrEditActivos(ActivosDTO objeto);
		public abstract ActivosDTO GetActivosById(int id);
        public abstract ActivosDTO GetActivosByUserIT();
        public abstract ActivosDTO GetActivosByNombre(string nombre);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract List<CustomAutocomplete> GetActivosByFiltro(string filtro);

        #region CONFIGURACION_PORTAFOLIO
        public abstract List<AreaBianDTO> GetAreaBian(Paginacion pag, out int totalRows);
        public abstract List<DominioBianDTO> GetDominioBian(Paginacion pag, out int totalRows);
        public abstract List<PlataformaBcpDTO> GetPlataformaBcp(Paginacion pag, out int totalRows);

        public abstract List<JefaturaAtiDTO> GetJefaturaAti(Paginacion pag, out int totalRows);
        public abstract int GetArquitectoByCorreo(string correo);
        public abstract List<ArquitectoTiDTO> GetArquitectoTi(Paginacion pag, out int totalRows);

        public abstract List<GerenciaDTO> GetGerencia(Paginacion pag, out int totalRows);
        public abstract List<AreaDTO> GetArea(Paginacion pag, out int totalRows);
        public abstract List<DivisionDTO> GetDivision(Paginacion pag, out int totalRows);
        public abstract List<UnidadDTO> GetUnidad(Paginacion pag, out int totalRows);
        public abstract List<TeamSquadDTO> GetTeamSquad(int gestionado);
        public abstract TeamSquadDTO GetTeamSquadId(int id);

        public abstract List<CuestionarioPaeDTO> GetCuestionarioPae(Paginacion pag, out int totalRows);
        public abstract List<PreguntaPaeDTO> GetPreguntaPae(Paginacion pag, out int totalRows);

        public abstract List<BandejaDTO> GetBandeja(Paginacion pag, out int totalRows);
        public abstract List<BandejaAprobacionDTO> GetBandejaAprobacion(Paginacion pag, out int totalRows);

        public abstract int AddOrEditAreaBian(AreaBianDTO objeto);
        public abstract int AddOrEditDominioBian(DominioBianDTO objeto);
        public abstract int AddOrEditPlataformaBcp(PlataformaBcpDTO objeto);

        public abstract int AddOrEditJefaturaAti(JefaturaAtiDTO objeto);
        public abstract int AddOrEditArquitectoTi(ArquitectoTiDTO objeto);

        public abstract int AddOrEditGerencia(GerenciaDTO objeto);
        public abstract int AddOrEditDivision(DivisionDTO objeto);
        public abstract int AddOrEditArea(AreaDTO objeto);
        public abstract int AddOrEditUnidad(UnidadDTO objeto);

        public abstract int AddOrEditCuestionarioPae(CuestionarioPaeDTO objeto);
        public abstract int AddOrEditPreguntaPae(PreguntaPaeDTO objeto);

        public abstract int AddOrEditBandeja(BandejaDTO objeto);
        public abstract int AddOrEditBandejaAprobacion(BandejaAprobacionDTO objeto);
        public abstract bool CambiarEstadoBandejaAprobacion(int id, bool estado, string usuario);

        public abstract AreaBianDTO GetAreaBianById(int id);
        public abstract DominioBianDTO GetDominioBianById(int id);
        public abstract PlataformaBcpDTO GetPlataformaBcpById(int id);

        public abstract JefaturaAtiDTO GetJefaturaAtiById(int id);
        public abstract ArquitectoTiDTO GetArquitectoTiById(int id);

        public abstract GerenciaDTO GetGerenciaById(int id);
        public abstract AreaDTO GetAreaById(int id);
        public abstract DivisionDTO GetDivisionById(int id);
        public abstract UnidadDTO GetUnidadById(int id);
        public abstract CuestionarioAplicacionDTO GetCuestionarioAplicacionByCodigoAPT(string codigoAPT);

        public abstract CuestionarioPaeDTO GetCuestionarioPaeById(int id);
        public abstract PreguntaPaeDTO GetPreguntaPaeById(int id);

        public abstract BandejaDTO GetBandejaById(int id);
        public abstract BandejaAprobacionDTO GetBandejaAprobacionById(int id);

        public abstract List<CustomAutocomplete> GetAreaBianByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDominioBianByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetPlataformaBcpByFiltro(string filtro);

        public abstract List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetJefaturaAtiByFiltro(string filtro1, string filtro);
        public abstract List<CustomAutocomplete> GetArquitectoTiByFiltro(string filtro);

        public abstract List<CustomAutocomplete> GetGerenciaByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDivisionByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetAreaByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetUnidadByFiltro(string filtroUnidad, string filtroPadre);


        public abstract int AddOrEditCuestionarioAplicacion(CuestionarioAplicacionDTO objeto);
        public abstract int AddOrEditCuestionarioAplicacionDetalle(CuestionarioAplicacionDetalleDTO objeto, GestionCMDB_ProdEntities _ctx);

        public abstract bool CambiarEstadoAreaBian(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoDominioBian(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoPlataformaBcp(int id, bool estado, string usuario);

        public abstract bool CambiarEstadoJefaturaAti(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoArquitectoTi(int id, bool estado, string usuario);

        public abstract bool CambiarEstadoGerencia(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoDivision(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoArea(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoUnidad(int id, bool estado, string usuario);

        public abstract bool CambiarEstadoCuestionarioPae(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoPreguntaPae(int id, bool estado, string usuario);

        public abstract bool ExisteMatriculaEnBandeja(string filtro, int bandejaId, int id);
        public abstract bool ExisteMatriculaEnJefaturaAti(string filtro, int entidadRelacionId, int id);

        public abstract List<EstandarDTO> GetEstandarPortafolio(Paginacion pag, out int totalRows);
        public abstract List<EstandarPortafolioDTO> GetEstandarPortafolioTecnologia(PaginacionEstandar pag, out int totalRows);
        public abstract int AddOrEditEstandarPortafolio(EstandarDTO objeto);
        public abstract EstandarDTO GetEstandarPortafolioById(int id);
        public abstract bool CambiarEstadoEstandarPortafolio(int id, bool estado, string usuario);

        public abstract List<ProcesoVitalDTO> GetProcesoVital(Paginacion pag, out int totalRows);
        public abstract int AddOrEditProcesoVital(ProcesoVitalDTO objeto);
        public abstract ProcesoVitalDTO GetProcesoVitalById(int id);
        public abstract bool CambiarEstadoProcesoVital(int id, bool estado, string usuario);
        public abstract List<CustomAutocomplete> GetProcesoVitalByFiltro(string filtro);
        public abstract bool? ProcesoClaveEsVital(string filtro);

        public abstract int GetPuntuacionByEstandar(int tipoEstandarId, string nombreEstandar, int flagPc);
        public abstract GrupoNivelCumplimiento CalcularPorcentajeEstandar(int? id_aplicacion, int flagPc, double[] L, double[] M, double N,
            string _SO, string _HP, string _BD, string _FW, int ncls);

        public abstract List<ServidorAplicacionDTO> GetServidorAplicacion(Paginacion pag, out int totalRows);
        public abstract int AddOrEditServidorAplicacion(ServidorAplicacionDTO objeto);
        public abstract bool AddOrEditListServidorAplicacion(DataServidorAplicacion objeto);
        public abstract bool CambiarEstadoServidorAplicacion(int id, bool estado, string usuario);
        public abstract bool ExisteServidorByCodigApt(string codigoApt, string nombreServidor);
        public abstract string GetUltimoCodigoAptPAE();

        public abstract List<GestionadoPorDTO> GetGestionadoPor(Paginacion pag, out int totalRows);
        public abstract int AddOrEditGestionadoPor(GestionadoPorDTO objeto);
        public abstract GestionadoPorDTO GetGestionadoPorById(int id);
        public abstract bool CambiarEstadoGestionadoPor(int id, bool estado, string usuario);
        public abstract List<CustomAutocomplete> GetGestionadoPorByFiltro(string filtro);
        public abstract GestionadoPorDTO GetGestionadoPorByNombre(string filtro);
        public abstract List<TeamSquadDTO> GetTeamSquadByGestionadoPor(PaginationTeamSquad pag, out int totalRows);
        public abstract List<TribeLeaderDTO> GetTribeLeaderByGestionadoPor(PaginationTeamSquad pag, out int totalRows);
        public abstract bool UpdateResponsibleTeamSquad(TeamSquadDTO obj);
        public abstract bool UpdateResponsibleTribeLeader(TribeLeaderDTO obj, out string message);

        //InfoCampoPortafolio
        public abstract int AddOrEditInfoCampoPortafolio(InfoCampoPortafolioDTO objeto, GestionCMDB_ProdEntities context);
        public abstract List<CustomAutocomplete> GetAllToolTipPortafolio(int? tipoFlujoId = null, bool? IsNewField = null, string codigoAPT = null);

        //GENERICOS
        //Validaciones opcion eliminar
        public abstract DataValidacion ExisteRelacionByConfiguracion(int idEntidad, int idConfiguracion, int? idEntidadRelacion);
        public abstract bool EliminarRegistroConfiguracion(int idEntidad, int idConfiguracion, string usuario, string NombreUsuario);
        //Validacion existe nombre
        public abstract bool ExisteNombreEntidadByConfiguracion(int idEntidad, string nombreFiltro, int idConfiguracion, int? idEntidadRelacion);

        public abstract int AddOrEditNuevoCampoPortafolio(NuevoCampoPortafolioDTO objeto, GestionCMDB_ProdEntities _ctx);

        public abstract string[] GetDataInfoCampoPortafolioByConfiguracion(int idEntidad);
        public abstract List<DataListBox> GetDataListBoxByConfiguracion(int idEntidad);
        public abstract List<CustomDataListBox> GetDataListBoxNewByFiltro();

        public abstract bool CambiarFlagMostrarCampo(int id, bool estado, string usuario, string NombreUsuario);

        /*roles gestion*/

        public abstract List<RolesGestionDTO> GetRolesGestion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<RolesGestionDTO> GetRolesGestion();

        public abstract int AddOrEditRolesGestion(RolesGestionDTO objeto);
        public abstract RolesGestionDTO GetRolesGestionById(int id);
        public abstract bool CambiarEstadoRolesGestion(int id, bool estado, string usuario);
        /*exportar gerenfia*/
        public abstract List<GerenciaAllDTO> GetTAllGDAU();


        #endregion

        public abstract int AddOrEditClasificacion(ClasificacionTecnicaDTO objeto);
        public abstract int AddOrEditSubclasificacion(SubClasificacionTecnicaDTO objeto);
        public abstract ClasificacionTecnicaDTO GetClasificacionById(int id);
        public abstract SubClasificacionTecnicaDTO GetSubclasificacionById(int id);
        public abstract bool CambiarEstadoClasificacion(int id, bool estado, string usuario);
        public abstract bool CambiarEstadoSubclasificacion(int id, bool estado, string usuario);
        public abstract List<ClasificacionTecnicaDTO> GetClasificacion (Paginacion pag, out int totalRows);
        public abstract List<SubClasificacionTecnicaDTO> GetSubclasificacion(Paginacion pag, out int totalRows);

        public abstract List<CustomAutocomplete> GetCamposByFiltro(string filtro);
    }
}