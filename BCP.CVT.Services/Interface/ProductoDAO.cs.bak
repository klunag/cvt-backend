using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class ProductoDAO : ServiceProvider
    {
        #region Producto
        public abstract int AddOrEditProducto(ProductoDTO objeto);
        public abstract List<ProductoDTO> GetProducto(string nombre, int? dominioId, int? subDominioId, int? tipoProdutoId, bool Activo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, bool withCantidadTecnologias = false);
        public abstract List<ProductoDTO> GetAllProducto();
        public abstract List<ProductoDTO> GetProductoByDescripcion(string descripcion, string dominioIds = null, string subDominioIds = null);
        public abstract ProductoDTO GetProductoById(int id);
        public abstract bool CambiarEstado(int id, string usuario);
        public abstract bool ExisteCodigoByFiltro(string filtro, int id);
        public abstract bool ExisteFabricanteNombre(string fabricante, string nombre, int id);
        public abstract string ObtenerCodigoSugerido();
        #endregion
        #region ProductoTecnologiaAplicacion
        public abstract List<TecnologiaAplicacionDTO> GetTecnologiaAplicaciones(int productoId);
        #endregion
        #region Producto Owner
        public abstract List<ProductoOwnerDto> BuscarProductoXOwner(string correo, int perfilId, string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        #endregion
    }
}
