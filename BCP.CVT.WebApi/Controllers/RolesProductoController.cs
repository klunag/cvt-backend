using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.CargaMasiva;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/RolesProducto")]
    public class RolesProductoController : BaseController
    {
        // GET: RolesProducto
        #region Bandeja de solicitudes de roles

        [Route("SolicitudesRoles")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage SolicitudesRoles(PaginacionSolicitud objDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            objDTO.Matricula = user.Matricula;
            objDTO.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var totalRows = 0;
            if (objDTO.FechaAtencionSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaAtencionSolicitud2 = null;
            }
            else objDTO.FechaAtencionSolicitud2 = DateTime.Today;
            if (objDTO.FechaRegistroSolicitud == Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                objDTO.FechaRegistroSolicitud2 = null;
            }
            else objDTO.FechaRegistroSolicitud2 = DateTime.Today;
            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetFuncionSolicitudes(objDTO, out totalRows);

            registros.ForEach(x =>
            {
                x.Producto = HttpUtility.HtmlEncode(x.Producto);
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.SubDominio = HttpUtility.HtmlEncode(x.SubDominio);
            });

            var reader = new BootstrapTable<SolicitudRolDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("RolesProductosCombo")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetRolesProductosCombo(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.Perfil = user.Perfil;

            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosCombo(pag.Perfil, pag.Matricula);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("RolesProductosComboDominio/{DominioId:int}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRolesProductosComboDominio(int DominioId)
        {
            HttpResponseMessage response = null;
            var objListas = ServiceManager<SolicitudAplicacionDAO>.Provider.GetRolesProductosComboDominio(DominioId);

            response = Request.CreateResponse(HttpStatusCode.OK, objListas);
            return response;
        }

        [Route("GetProductoByFiltro")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetProd(PaginacionSolicitud pag)
        {

            var colorStatus = ServiceManager<SolicitudAplicacionDAO>.Provider.GetProd(pag.Producto);
            return Ok(colorStatus);
        }

        [Route("ListadoByDescripcion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListProductosByDescripcion(string descripcion, string dominioIds = null, string subDominioIds = null)
        {
            var registros = ServiceManager<ProductoDAO>.Provider.GetProductoByDescripcion(descripcion, dominioIds, subDominioIds);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("DetalleProductosRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleProductosRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetProductoRolesDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<RolesProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("DetalleFuncionesProductosRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListDetalleFuncionesProductosRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetDetalleFuncionesProductosRoles(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("AprobarSolicitudOwner")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAprobarSolicitudOwner(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudRolDAO>.Provider.AprobarSolicitudOwner(pag.SolDetalleId, pag.Matricula, pag.NombreUsuario, pag.Comentario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ObservarSolicitudOwner")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObservarSolicitudOwner(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudRolDAO>.Provider.ObservarSolicitudOwner(pag.SolDetalleId, pag.Matricula, pag.NombreUsuario, pag.Comentario, pag.ProductoId);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("AprobarSolicitudSeguridad")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostAprobarSolicitudSeguridad(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudRolDAO>.Provider.AprobarSolicitudSeguridad(pag.SolDetalleId, pag.Matricula, pag.NombreUsuario, pag.Comentario);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("ObservarSolicitudSeguridad")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObservarSolicitudSeguridad(PaginacionSolicitud pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.NombreUsuario = user.Nombres;
            pag.Matricula = user.Matricula;

            HttpResponseMessage response = null;
            var dataResult = ServiceManager<SolicitudRolDAO>.Provider.ObservarSolicitudSeguridad(pag.SolDetalleId, pag.Matricula, pag.NombreUsuario, pag.Comentario, pag.ProductoId);

            response = Request.CreateResponse(HttpStatusCode.OK, dataResult);

            return response;
        }

        [Route("HistorialSolicitud")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostHistorialSolicitud(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetHistorialSolicitud(pag, out totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x => { x.Comentario = HttpUtility.HtmlEncode(x.Comentario); });

            var reader = new BootstrapTable<SolRolHistorialDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ResponsablesPorSolicitud")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ResponsablesPorSolicitud(PaginacionSolicitud pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetResponsablesPorSolicitud(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<SolicitudRolResponsablesDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("VerSolicitud/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(SolicitudRolDTO))]
        [Authorize]
        public IHttpActionResult GetSolicitudPorId(int id)
        {
            var objProd = ServiceManager<SolicitudRolDAO>.Provider.GetSolicitudPorId(id);
            if (objProd == null)
                return NotFound();

            //if (objProd.AplicacionId.HasValue)
            //    objProd.Aplicacion = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(objProd.AplicacionId.Value);

            return Ok(objProd);
        }

        [Route("DetalleSolicitudesRoles")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostDetalleSolicitudesRoles(PaginacionSolicitud pag)
        {
            var totalRows = 0;

            var registros = ServiceManager<SolicitudRolDAO>.Provider.GetDetalleSolicitudRoles(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<FuncionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }
        [Route("ObtenerAmbienteRolProducto/{id:int}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAmbienteRolProductoDetail(int id)
        {
            var objRol = ServiceManager<SolicitudRolDAO>.Provider.GetAmbienteRolProductoByIdSolicitud(id);
            if (objRol == null)
                return NotFound();

            return Ok(objRol);
        }

        #endregion
    }
}