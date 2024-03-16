using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using BCP.CVT.WebApi.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using System.Web;

namespace BCP.CVT.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Producto")]
    public class ProductoController : ApiController
    {
        [Route("")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostProducto(ProductoDTO productoDTO)
        {
            var user = TokenGenerator.GetCurrentUser();
            productoDTO.UsuarioCreacion = user.Matricula;
            productoDTO.UsuarioModificacion = user.Matricula;
            // DANIELQG93
            if (productoDTO.ListaTecnologia != null && productoDTO.ListaTecnologia.Count > 0)
            {
                foreach (var item in productoDTO.ListaTecnologia)
                {
                    item.UsuarioCreacion = user.Matricula;
                    item.UsuarioModificacion = user.Matricula;
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int IdArq = ServiceManager<ProductoDAO>.Provider.AddOrEditProducto(productoDTO);

            if (IdArq == 0)
                return NotFound();

            return Ok(IdArq);
        }

        [Route("Listado")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostListProductos(Paginacion pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<ProductoDAO>.Provider.GetProducto(pag.nombre, pag.dominioId, pag.subDominioId, pag.Activos, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows, true);
            //var registros = ServiceManager<ProductoDAO>.Provider.GetProducto(pag.nombre, pag.estadoObsolescenciaId, pag.dominioId, pag.subDominioId, pag.tipoProductoId, pag.Activos, pag.pageNumber, pag.pageSize, pag.sortName, pag.sortOrder, out totalRows, true);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
                x.Descripcion = HttpUtility.HtmlEncode(x.Descripcion);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.ProductoStr = HttpUtility.HtmlEncode(x.ProductoStr);
                x.DominioStr = HttpUtility.HtmlEncode(x.DominioStr);
                x.SubDominioStr = HttpUtility.HtmlEncode(x.SubDominioStr);
                x.TipoProductoStr = HttpUtility.HtmlEncode(x.TipoProductoStr);
                x.GrupoTicketRemedyNombre = HttpUtility.HtmlEncode(x.GrupoTicketRemedyNombre);
                x.TipoCicloVidaStr = HttpUtility.HtmlEncode(x.TipoCicloVidaStr);
            });


            dynamic reader = new BootstrapTable<ProductoDTO>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Exportar")]
        [HttpPost]
        public IHttpActionResult GetExportarProducto(FiltroProductoDTO filtro)
        {
            string nomArchivo = "";
            if (string.IsNullOrEmpty(filtro.nombre)) filtro.nombre = null;

            var data = new ExportarData().ExportarProducto(filtro.nombre, filtro.dominioId, filtro.subdominioId, filtro.activo, filtro.sortName, filtro.sortOrder);
            nomArchivo = string.Format("ListaProductos_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
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

        [Route("GetByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetByFiltro(string filtro)
        {
            HttpResponseMessage response = null;
            var list = ServiceManager<ProductoDAO>.Provider.GetByFiltro(filtro);
            response = Request.CreateResponse(HttpStatusCode.OK, list);
            return response;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ResponseType(typeof(ProductoDTO))]
        [Authorize]
        public IHttpActionResult GetProductoById(int id)
        {
            var objProd = ServiceManager<ProductoDAO>.Provider.GetProductoById(id);
            if (objProd == null)
                return NotFound();

            if (objProd.AplicacionId.HasValue)
                objProd.Aplicacion = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(objProd.AplicacionId.Value);

            return Ok(objProd);
        }

        [Route("CambiarEstado")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetCambiarEstado(int Id)
        {
            var user = TokenGenerator.GetCurrentUser();
            string Usuario = user.Matricula;

            HttpResponseMessage response = null;
            var retorno = ServiceManager<ProductoDAO>.Provider.CambiarEstado(Id, Usuario);

            response = Request.CreateResponse(HttpStatusCode.OK, retorno);
            return response;
        }

        [Route("ObtenerCodigoInterno")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostObtenerCodigoInterno()
        {
            HttpResponseMessage response = null;
            var paramSustentoMotivo = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_SUSTENTO_MOTIVO");
            var paramTipoFechaInterna = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("GESTION_TECNOLOGIAS_TIPO_FECHA_INTERNA");

            var listSustentoMotivo = (paramSustentoMotivo.Valor ?? "").Split('|');
            var listTipoFechaInterna = (paramTipoFechaInterna.Valor ?? "").Split('|');

            //var listTipoArquetipo = ServiceManager<TipoPro>.Provider.GetTipoArquetipoByFiltro(null);
            //var listEntorno = ServiceManager<EntornoDAO>.Provider.GetEntornoByFiltro(null);

            var listFuente = Utilitarios.EnumToList<Fuente>();
            var listFechaCalculo = Utilitarios.EnumToList<FechaCalculoTecnologia>();
            var listEstadoObsolescencia = Utilitarios.EnumToList<EEstadoObsolescenciaProducto>().Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) });
            //var listTipoProducto = ServiceManager<TipoDAO>.Provider.GetAllTipo();
            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);
            var listAutomatizacionImplementada = Utilitarios.EnumToList<EAutomatizacionImplementada>().Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) });
            var listTipoCicloVida = ServiceManager<TipoCicloVidaDAO>.Provider.GetTipoCicloVidaByFiltro(true);
            var listEsquemaLicenciamientoSuscripcion = Utilitarios.EnumToList<EEsquemaLicenciamientoSuscripcion>().Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) });
            //var codigoSugerido = ServiceManager<ProductoDAO>.Provider.ObtenerCodigoSugerido();

            var dataRpta = new
            {
                //TipoRiesgo = listTipoRiesgo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                CodigoInterno = (int)ETablaProcedencia.CVT_Producto,
                Fuente = listFuente.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                FechaCalculo = listFechaCalculo.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList(),
                EstadoObsolescencia = listEstadoObsolescencia,
                //Tipo = listTipoProducto,
                Dominio = listDominio,
                SubDominio = listSubDominio,
                AutomatizacionImplementada = listAutomatizacionImplementada,
                TipoCicloVida = listTipoCicloVida,
                EsquemaLicenciamientoSuscripcion = listEsquemaLicenciamientoSuscripcion,
                SustentoMotivo = listSustentoMotivo,
                TipoFechaInterna = listTipoFechaInterna,
                //TipoArquetipo = listTipoArquetipo,
                //Entorno = listEntorno
                //CodigoSugerido = codigoSugerido
            };
            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ObtenerCodigoSugerido")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ObtenerCodigoSugerido()
        {
            HttpResponseMessage response = null;
            bool estado = true;
            string data = null;

            do
            {
                data = ServiceManager<ProductoDAO>.Provider.ObtenerCodigoSugerido();
                estado = ServiceManager<ApplicationDAO>.Provider.ExisteCodigoAPT(data);

            } while (estado);

            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ExisteCodigoByFiltro")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteCodigoByFiltro(string codigo, int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ProductoDAO>.Provider.ExisteCodigoByFiltro(codigo, id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ExisteFabricanteNombre")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteFabricanteNombre(string fabricante, string nombre, int id)
        {
            HttpResponseMessage response = null;
            var data = ServiceManager<ProductoDAO>.Provider.ExisteFabricanteNombre(fabricante, nombre, id);
            response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [Route("ExisteProductoCodigo")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ExisteProductoCodigo(string id)
        {
            HttpResponseMessage response = null;
            bool estado = ServiceManager<ApplicationDAO>.Provider.ExisteCodigoAPT(id);
            response = Request.CreateResponse(HttpStatusCode.OK, estado);
            return response;
        }

        #region Producto Tecnología Aplicación
        [Route("TecnologiaAplicacion/Listado")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListAplicacionesTecnologia(int productoId)
        {
            var registros = ServiceManager<ProductoDAO>.Provider.GetTecnologiaAplicaciones(productoId);

            if (registros == null)
                return NotFound();

            return Ok(registros);
        }

        [Route("TecnologiaAplicacion/ExportarListado")]
        [HttpGet]
        public IHttpActionResult PostExportarListAplicacionesTecnologia(int productoId, string productoStr, string filtro)
        {
            string nomArchivo = string.Format("ListaAplicacionesXProducto_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListAplicacionesProducto(productoId, productoStr, filtro);

            return Ok(new { excel = data, name = nomArchivo });
        }
        #endregion

        #region Tecnología Owner
        [Route("TecnologiaOwner/ListarCombos")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostListCombosTecnologiaOwner()
        {
            HttpResponseMessage response = null;

            var listDominio = ServiceManager<DominioDAO>.Provider.GetAllDominioByFiltro(null);
            var listSubDominio = ServiceManager<SubdominioDAO>.Provider.GetAllSubdominioByFiltro(null);

            var dataRpta = new
            {
                Dominio = listDominio,
                SubDominio = listSubDominio,
            };

            response = Request.CreateResponse(HttpStatusCode.OK, dataRpta);
            return response;
        }

        [Route("ListadoXOwner")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult PostListProductoOwner(string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, int pageNumber, int pageSize, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;

            var registros = ServiceManager<ProductoDAO>.Provider.BuscarProductoXOwner(correo, perfilId, dominioIds, subDominioIds, productoStr, tribuCoeId, squadId, pageNumber, pageSize, sortName, sortOrder, out int totalRows);

            if (registros == null)
                return NotFound();

            registros.ForEach(x =>
            {
                x.Dominio = HttpUtility.HtmlEncode(x.Dominio);
                x.Subdominio = HttpUtility.HtmlEncode(x.Subdominio);
                x.ProductoStr = HttpUtility.HtmlEncode(x.ProductoStr);
                x.Fabricante = HttpUtility.HtmlEncode(x.Fabricante);
                x.Nombre = HttpUtility.HtmlEncode(x.Nombre);
            });

            var reader = new BootstrapTable<ProductoOwnerDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("ExportarListadoXOwnerConsolidado")]
        [HttpGet]
        public IHttpActionResult PostExportarListProductoOwnerConsolidado(string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;

            string nomArchivo = string.Format("ListaProductoPorOwnerConsolidado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListProductoOwnerConsolidado(correo, perfilId, dominioIds, subDominioIds, productoStr, tribuCoeId, squadId, sortName, sortOrder);

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("ExportarListadoXOwnerDetallado")]
        [HttpGet]
        public IHttpActionResult PostExportarListProductoOwnerDetallado(string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, string sortName, string sortOrder)
        {
            var user = TokenGenerator.GetCurrentUser();
            string correo = user.CorreoElectronico;
            int perfilId = user.PerfilId;

            string nomArchivo = string.Format("ListaProductoPorOwnerDetallado_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var data = new ExportarData().ExportarListProductoOwnerDetallado(correo, perfilId, dominioIds, subDominioIds, productoStr, tribuCoeId, squadId, null, sortName, sortOrder);

            return Ok(new { excel = data, name = nomArchivo });
        }
        #endregion

    }
}
