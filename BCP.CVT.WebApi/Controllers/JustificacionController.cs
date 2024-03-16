using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Exportar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using BCP.PAPP.Common.Dto;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using System.Data.SqlClient;
using System.Data;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Justificacion")]
    public class JustificacionController : BaseController
    {
        [Route("Listado")]
        [HttpPost]
		[Authorize]
		public IHttpActionResult PostListarArchivos(PaginacionMotorJustificacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<MotorJustificacionDAO>.Provider.GetRegistros(pag, out totalRows);

            if (registros == null)
                return NotFound();

            dynamic reader = new BootstrapTable<MotorJustificacionDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombos()
        {
            HttpResponseMessage response = null;

            var listProceso = ServiceManager<MotorProcesoDAO>.Provider.Proceso_List_Combo();
            var listProtocolo = ServiceManager<ProtocoloDAO>.Provider.Protocolo_List_Combo();
            var listPuerto = ServiceManager<PuertoDAO>.Provider.Puerto_List_Combo();

            var dataRpta = new
            {
                Proceso = listProceso,
                Puerto = listPuerto,
                Protocolo = listProtocolo
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("Exportar")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult ExportarJustificacion(int ProductoId, int TecnologiaId, int EquipoId, string Aplicacion, int ProcesoId, int PuertoId, int ProtocoloId, string Fecha, string sortName, string sortOrder)
        {
            var objeto = new PaginacionMotorJustificacion
            {
                ProductoId = ProductoId,
                TecnologiaId = TecnologiaId,
                EquipoId = EquipoId,
                Aplicacion = Aplicacion,
                ProcesoId = ProcesoId,
                PuertoId = PuertoId,
                ProtocoloId = ProtocoloId,
                Fecha = Fecha,
                sortName = sortName,
                sortOrder = sortOrder,
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            string nomArchivo = "";
            var data = new ExportarData().ExportarJustificacion(objeto);
            nomArchivo = "ListaJustificion";
            nomArchivo = string.Format("{0}_{1}.xlsx", nomArchivo, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

    }
}
