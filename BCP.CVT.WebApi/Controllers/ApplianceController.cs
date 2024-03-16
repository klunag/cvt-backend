using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BCP.CVT.Cross;
using BCP.CVT.Services.Exportar;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.Services;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/ApplianceSolicitud")]
    public class ApplianceController : BaseController
    {
        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(EquipoSolicitudDTO))]
        [Authorize]
        public IHttpActionResult GetApplianceSolicitudById(int id)
        {
            var objDominio = ServiceManager<ApplianceDAO>.Provider.GetSolicitudById(id);
            if (objDominio == null)
                return NotFound();

            return Ok(objDominio);
        }

        [Route("ValidarEquipo/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(EquipoSolicitudDTO))]
        [Authorize]
        public IHttpActionResult GetValidarEquipo(int id)
        {
            var objDominio = ServiceManager<ApplianceDAO>.Provider.GetValidacion(id);
            if (objDominio == null)
                return NotFound();

            return Ok(objDominio);
        }

        // GET: api/Dominio/CambiarEstado/5
        [Route("CambiarEstado")]
        [HttpPost]
        [ResponseType(typeof(DominioDTO))]
        [Authorize]
        public IHttpActionResult PostSolicitudCambiarEstado(EquipoSolicitudDTO solicitud)
        {
            var user = TokenGenerator.GetCurrentUser();
            solicitud.AprobadoRechazadoPor = user.Matricula;
            solicitud.AprobadorMatricula = user.Matricula;
            solicitud.NombreUsuarioAprobadoRechazo = user.Nombres;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(solicitud.EstadoSolicitud.ToString()))
                ServiceManager<ApplianceDAO>.Provider.CambiarEstadoSolicitud(solicitud.Id, solicitud.EstadoSolicitud, solicitud.AprobadoRechazadoPor, solicitud.NombreUsuarioAprobadoRechazo, solicitud.ComentariosAprobacionRechazo, solicitud.AprobadorMatricula, string.Empty);
            else
                return BadRequest(ModelState);

            return Ok(solicitud.Id);
        }

        // POST: api/Dominio
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DominioDTO))]
        [Authorize]
        public IHttpActionResult PostSolicitud(EquipoSolicitudDTO solicitud)
        {
            var user = TokenGenerator.GetCurrentUser();
            solicitud.UsuarioCreacion = user.Matricula;
            solicitud.UsuarioModificacion = user.Matricula;
            solicitud.CorreoSolicitante = user.CorreoElectronico;
            solicitud.NombreUsuarioCreacion = user.Nombres;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdSolicitud = ServiceManager<ApplianceDAO>.Provider.AddOrEditSolicitud(solicitud);

            if (IdSolicitud == 0)
                return NotFound();

            return Ok(IdSolicitud);
        }

        // POST: api/Dominio
        [Route("Desestimar")]
        [HttpPost]
        [ResponseType(typeof(DominioDTO))]
        [Authorize]
        public IHttpActionResult PostSolicitudDesestimar(EquipoSolicitudDTO solicitud)
        {
            var user = TokenGenerator.GetCurrentUser();
            solicitud.UsuarioModificacion = user.Matricula;
            solicitud.NombreUsuarioModificacion = user.Nombres;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceManager<ApplianceDAO>.Provider.CambiarEstadoSolicitud(solicitud.Id, (int)EstadoSolicitudActivosTSI.Desestimado, solicitud.UsuarioModificacion, solicitud.NombreUsuarioModificacion, solicitud.ComentariosDesestimacion, solicitud.AprobadorMatricula, solicitud.CodigoAPT);

            return Ok(solicitud.Id);
        }

        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListSolicitudes(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.RolUsuario = user.Perfil;

            var totalRows = 0;
            var registros = ServiceManager<ApplianceDAO>.Provider.GetSolicitudes(pag.nombre, pag.tipoId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.RolUsuario, pag.Matricula, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.NombreEquipo = HttpUtility.HtmlEncode(x.NombreEquipo);
                x.Comentarios = HttpUtility.HtmlEncode(x.Comentarios);
                x.ComentariosAprobacionRechazo = HttpUtility.HtmlEncode(x.ComentariosAprobacionRechazo);
            });

            dynamic reader = new BootstrapTable<EquipoSolicitudDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportSolicitudes(string nombre, int estado)
        {
            var user = TokenGenerator.GetCurrentUser();
            string perfil = user.Perfil;
            string usuario = user.Matricula;
            string nomArchivo = "";
            if (string.IsNullOrEmpty(nombre)) nombre = null;
            var data = new ExportarData().ExportarSolicitudesAppliance(nombre, estado, perfil, usuario);
            nomArchivo = string.Format("SolicitudesActivosTSI_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        // POST: api/Dominio
        [Route("Revertir")]
        [HttpPost]
        [ResponseType(typeof(DominioDTO))]
        [Authorize]
        public IHttpActionResult PostSolicitudRevertir(EquipoSolicitudDTO solicitud)
        {
            var user = TokenGenerator.GetCurrentUser();
            solicitud.NombreUsuarioModificacion = user.Nombres;
            solicitud.UsuarioModificacion = user.Matricula;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceManager<ApplianceDAO>.Provider.RevertirSolicitud(solicitud.EquipoId.Value, solicitud.UsuarioModificacion, solicitud.NombreUsuarioModificacion);

            return Ok(solicitud.Id);
        }

        /// <summary>
        /// ListarSolicitudCvt
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        [Route("ListarSolicitudCvt")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListarSolicitudCvt(Paginacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.RolUsuario = user.Perfil;
            pag.Matricula = user.Matricula;

            var totalRows = 0;
            var registros = ServiceManager<ApplianceDAO>.Provider.GetSolicitudPendientesXAprobarCvt(pag.nombre, pag.tipoId, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, pag.RolUsuario, pag.Matricula, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.NombreEquipo = HttpUtility.HtmlEncode(x.NombreEquipo); });

            dynamic reader = new BootstrapTable<EquipoSolicitudDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
    }
}