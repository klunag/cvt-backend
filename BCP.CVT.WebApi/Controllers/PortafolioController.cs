using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Portafolio")]
    public class PortafolioController : BaseController
    {
        [Route("ReporteBIAN")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetReporteBIAN(string Gerencia, string Estado)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAreaBian(Gerencia, Estado);

            if (registros == null)
                return NotFound();

            totalRows = registros.Count;
            dynamic reader = new BootstrapTable<AreaBianDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteBIAN/BT")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteBIAN(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAreaBianBT(pag);
            totalRows = registros.Count;

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<AreaBianDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteBIAN/DetalleAplicacion")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteBIANDetalle(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAreaBianDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAplicacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteEstado")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetReporteEstado(string Gerencia)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicaciones(Gerencia);

            if (registros == null)
                return NotFound();

            totalRows = registros.Count;
            dynamic reader = new BootstrapTable<AplicacionReporteDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteEstado/BT")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteEstado(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicacionesBT(pag);
            //var itemTotal = new AplicacionReporteDto
            //{
            //    Categoria = "Total",
            //    TotalEnDesarrollo = registros.LastOrDefault().SumaTotalEnDesarrollo,
            //    TotalVigentes = registros.LastOrDefault().SumaTotalVigentes,
            //    AppsOnCloud = registros.LastOrDefault().SumaAppsOnCloud,
            //    AppsOnPremise = registros.LastOrDefault().SumaAppsOnPremise,
            //    AppsDesarrolladasOnCloud = registros.LastOrDefault().SumaAppsDesarrolladasOnCloud,
            //    AppsDesarrolladasOnPremise = registros.LastOrDefault().SumaAppsDesarrolladasOnPremise,
            //    AppsPaquetesOnPremise = registros.LastOrDefault().SumaAppsPaquetesOnPremise,
            //    AppsPaquetesOnCloud = registros.LastOrDefault().SumaAppsPaquetesOnCloud,
            //    AppsObsolescenciaOnPremise = registros.LastOrDefault().SumaAppsObsolescenciaOnPremise,
            //    AppsObsolescenciaOnCloud = registros.LastOrDefault().SumaAppsObsolescenciaOnCloud

            //};
            //registros.Add(itemTotal);

            if (registros == null)
                return NotFound();

            totalRows = registros.Count;
            dynamic reader = new BootstrapTable<AplicacionReporteDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarInfraestructuraObsolecencia")]
        [HttpGet]
        public IHttpActionResult GetExportarInfraObsolecencia(string management, string functionalLayer, string sortName, string sortOrder)
        {
            if (string.IsNullOrEmpty(management))
                management = null;
            var data = new ExportarData().GetExportarInfraObsolecencia(management, functionalLayer, sortName, sortOrder);
            var file = string.Format("Reporte_Infraestructura_Obsolecencia_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = file });
        }

        [Route("ReporteEstado/TotalDetalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteEstadoTotalDetalle(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicacionesTotalDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAplicacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteEstado/InfraestructuraTotalDetalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteEstadoInfraestructuraTotalDetalle(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicacionesInfraestructuraDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAplicacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteEstado/InfraestructuraDesarrolloDetalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteEstadoInfraestructuraDesarrolloDetalle(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicacionesInfraestructuraDesarrolloDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAplicacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ReporteEstado/ObsolescenciaDetalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteEstadoObsolescenciaDetalle(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAplicacionesObsolescenciaDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAplicacionDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Alerta/ReporteUserIT")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostReporteAlertaUserIT(PaginacionAplicacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetReporteAlertas(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ReporteAlertasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Alerta/Exportar")]
        [HttpGet]
        public IHttpActionResult GetExportarAlertas()
        {
            string nomArchivo = "";
            var filtro = new PaginacionAplicacion()
            {
                pageNumber = 1,
                pageSize = int.MaxValue
            };
            var data = new ExportarData().ExportarAlertasUserIT(filtro);
            nomArchivo = string.Format("ListadoAlertasUserIT_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        //Instancias
        [Route("Instancias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstancias(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstancias(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        //Instancias
        [Route("Instancias/Grafico")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasGrafico(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;

            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasGrafico(pag);

            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("Instancias/Productos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasProductos(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasProductos(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Instancias/Productos/Grafico")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasProductosGrafico(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;

            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasProductosGrafico(pag);

            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }

        [Route("Instancias/Equipos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasEquipos(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasEquipos(pag, out totalRows);

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }


        [Route("Instancias/Equipos/Grafico")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasEquiposGrafico(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;

            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasEquiposGrafico(pag);

            response = Request.CreateResponse(HttpStatusCode.OK, registros);
            return response;
        }


        [Route("Instancias/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteInstanciasAplicaciones(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasEquiposAplicaciones(pag, out totalRows);

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        //Productos
        [Route("Productos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteProductos(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasFamilias(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Productos/Familias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteProductosFamilias(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasFamiliasProductos(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Productos/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteProductosAplicaciones(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasFamiliasAplicaciones(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Productos/Tecnologias")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostReporteProductosTecnologias(PaginacionEquipo pag)
        {
            HttpResponseMessage response = null;
            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetInstanciasFamiliasTecnologias(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Aplicaciones")]
        [HttpPost]
        //[Authorize]
        public HttpResponseMessage PostResumenAplicaciones(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            var totalRows = 0;

            if (pag.PerfilId == (int)EPerfilBCP.ETICMDB
                || pag.PerfilId == (int)EPerfilBCP.ArquitectoTecnologia
                || pag.PerfilId == (int)EPerfilBCP.ArquitectoFuncional
                || pag.PerfilId == (int)EPerfilBCP.ArquitectoSeguridad
                || pag.PerfilId == (int)EPerfilBCP.Seguridad
                || pag.PerfilId == (int)EPerfilBCP.Auditoria
                || pag.PerfilId == (int)EPerfilBCP.Gerente
                )
                pag.PerfilId = (int)EPerfilBCP.Administrador;

            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetResumenAplicaciones(pag, out totalRows);
            totalRows = registros.Count;

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }

        [Route("Aplicaciones/Detalle")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostResumenAplicacionesDetalle(PaginacionAplicacion pag)
        {
            var user = TokenGenerator.GetCurrentUser();
            pag.Matricula = user.Matricula;
            pag.PerfilId = user.PerfilId;

            HttpResponseMessage response = null;
            if (pag.PerfilId == 10 || pag.PerfilId == 9)
                pag.PerfilId = 1;

            var totalRows = 0;
            var registros = ServiceManager<ReportePortafolioDAO>.Provider.GetResumenAplicacionesDetalle(pag, out totalRows);
            totalRows = registros.Count;

            registros.ForEach(x =>
            {
                x.ClaveTecnologia = HttpUtility.HtmlEncode(x.ClaveTecnologia);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.NombreProducto = HttpUtility.HtmlEncode(x.NombreProducto);
                x.TipoComponente = HttpUtility.HtmlEncode(x.TipoComponente);
            });

            var reader = new BootstrapTable<InstanciasDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            response = Request.CreateResponse(HttpStatusCode.OK, reader);
            return response;
        }
    }
}
