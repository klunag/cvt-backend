using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.Services.Exportar;
using System.Globalization;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/instanciasBD")]
    public class InstanciasBDController : BaseController
    {
        [Route("listar")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult listarInstanciasBD(PaginacionEquipoInstanciaBD pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<InstanciasBDDAO>.Provider.GetInstanciasBd(pag, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<EquipoInstanciaBdDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ValidarCambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetValidarCambiarEstado(int Id)
        {
            HttpResponseMessage response = null;
            var retorno = ServiceManager<InstanciasBDDAO>.Provider.ValidarCambiarEstado(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("cambiarEstado")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage cambiarEstado(EquipoInstanciaBdDTO pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.UsuarioModificacion = user.Matricula;

            HttpResponseMessage response = null;
            ServiceManager<InstanciasBDDAO>.Provider.CambiarEstado(pag);
            response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;                      
        }

        [Route("Exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarInstanciasBD(string Nombre, int EquipoId, int Procedencia, string CodigoApt, string sortName, string sortOrder)
        {
            var objeto = new PaginacionEquipoInstanciaBD
            {
                Nombre = Nombre,
                EquipoId = EquipoId,
                Procedencia = Procedencia,
                CodigoApt = CodigoApt,
                sortName = sortName,
                sortOrder = sortOrder,
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            string nomArchivo = "";
            var data = new ExportarData().ExportarInstanciaBD(objeto);
            nomArchivo = "ListaInstanciasBD";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }        
    }
}