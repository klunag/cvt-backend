using BCP.PAPP.Common.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface.PortafolioAplicaciones
{
    public abstract partial class CommonDAO : ServiceProvider
    {
        public abstract FiltersAppStepOne GetListStepOne();

        public abstract string ValidateActivationJenkins();

        public abstract FiltersAppStepTwo GetListStepTwo();

        public abstract FiltersAppStepTwo GetListStepTwo_Admin();

        public abstract FilterGestionadoPorExterna GetListExterna();

        public abstract FilterArquitectoEvaluador GetListArquitectoEvaluador();

        public abstract FilterArquitectoEvaluador GetListArquitectoEvaluadorExterna();

        public abstract FilterArquitectoEvaluador GetListArquitectoEvaluador_Admin();

        public abstract FilterArquitectoTI GetListArquitectoTI();

        public abstract FilterArquitectoTI GetListArquitectoTI_Admin();

        public abstract FiltersDevSecOps GetListDevSecOps();

        public abstract FilterArquitectoEvaluador GetListArquitectoEvaluador(int area);

        public abstract FilterArquitectoEvaluador GetListArquitectoEvaluadorAdmin(int area);

        public abstract FilterArquitectoTI GetListSubclasificacion(int clasificacion);

        public abstract FilterArquitectoTI GetListSubclasificacionAdmin(int clasificacion);

        public abstract FilterAppsGestionado GetEquipos(int gestionado);

        public abstract FilterAppsGestionado GetEquiposAdmin(int gestionado);

        public abstract FilterArquitectoEvaluador GetListArquitectoJefatura(int jefatura);

        public abstract FilterArquitectoEvaluador GetListArquitectoJefaturaAdmin(int jefatura);

        public abstract FilterAdmin GetListsAdmin(bool withGestionadoPor = false);
        public abstract FilterAdmin GetListsDivisionByGerente(int idGerente);
        public abstract FilterAdmin GetListsAreaByDivision(int idDivison);
        public abstract FilterAdmin GetListsAdminConditions(bool withDivision, bool withArea, bool withUnidad, bool withGestionadoPor);
        public abstract FilterAdmin GetListsDivisionByGerenteMulti(int[] idsGerente);
        public abstract FilterAdmin GetListsAreaByDivisionMulti(int[] idsDivision);
        public abstract FilterAdmin GetListsUnidadesByAreaMulti(int[] idsArea);

        
    }
}
