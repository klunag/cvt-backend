using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
	public abstract partial class ParametricasDAO : ServiceProvider
	{
		public abstract List<ParametricasDTO> GetParametricas(Paginacion pag, out int totalRows);
		public abstract List<ParametricasDTO> GetParametricas();
		public abstract List<CustomAutocomplete> GetEntidades(int idParametrica);
		public abstract List<CustomAutocomplete> GetTablas(int idParametrica);

		public abstract int AddOrEditParametricas(ParametricasDTO objeto);
		public abstract ParametricasDTO GetParametricasById(int id);
		public abstract bool CambiarEstado(int id, bool estado, string usuario);
		public abstract List<CustomAutocomplete> GetParametricasByTabla(string filtro, int? entidadId);
		//public abstract bool ExisteCodigoByFiltro(int codigo, int id);
		//public abstract int UpdateVentana(ParametricasDTO objeto);
	}
}