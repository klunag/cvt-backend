using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ClientSecretDTO //: BaseDTO
    {
        public string NombreClientSecret { get; set; }
        public string Aplicacion { get; set; }
        public string NombreCloud { get; set; }
        public string Suscripcion { get; set; }
        public string TipoRecurso { get; set; }
        public string AKSsecret { get; set; }
        public string Recurso { get; set; }
        public string TipoClientSecret { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaCreacionStr { get { return FechaCreacion.ToString("dd/MM/yyyy"); } }
        public string FechaFinSoporteStr
        {
            get
            {
                if (FechaFinSoporte.HasValue)
                    return FechaFinSoporte.Value.ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
        }
        public DateTime? FechaCalculoBase { get; set; }
        public string FechaCalculoBaseStr
        {
            get
            {
                if (FechaCalculoBase.HasValue)
                    return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("dd/MM/yyyy") : string.Empty;
                else
                    return string.Empty;

            }
        }
        public int TotalFilas { get; set; }
        public int? Obsoleto { get; set; }
        public int Meses { get; set; }
        public int MesesObsolescencia { get; set; }
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }
        public string FactoresKPI_FLooking { get; set; }
        public int IndicadorObsolescencia
        {
            get
            {

                var indicador = 1;
                if (this.Obsoleto == 1)
                    indicador = -1;
                else
                {
                    var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia);
                    if (this.FechaCalculoBase.HasValue)
                        indicador = this.FechaCalculoBase.Value > fechaValidacion ? 1 : 0;
                    else
                        indicador = -1;
                }

                return indicador;
            }
        }

        public int IndicadorObsolescencia_Proyeccion1
        {
            get
            {
                var indicador = 1;
                if (this.Obsoleto == 1)
                    indicador = -1;
                else
                {
                    var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia + this.IndicadorMeses1);
                    if (this.FechaCalculoBase.HasValue)
                    {
                        var indObs = IndicadorObsolescencia;
                        if (indObs == 0 || indObs == -1)
                            indicador = -1;
                        else
                        {
                            if (this.FechaCalculoBase.Value > fechaValidacion)
                                indicador = 1;
                            else
                            {
                                var fechaValidacionDos = DateTime.Now.AddMonths(this.IndicadorMeses1);
                                indicador = this.FechaCalculoBase.Value > fechaValidacionDos ? 0 : -1;
                            }
                        }
                    }
                    else
                        indicador = -1;
                }

                return indicador;
            }
        }

        public int IndicadorObsolescencia_Proyeccion2
        {
            get
            {
                var indicador = 1;
                if (this.Obsoleto == 1)
                    indicador = -1;
                else
                {
                    var fechaValidacion = DateTime.Now.AddMonths(this.MesesObsolescencia + this.IndicadorMeses2);
                    if (this.FechaCalculoBase.HasValue)
                    {
                        var indObs = IndicadorObsolescencia_Proyeccion1;
                        if (indObs == 0 || indObs == -1)
                            indicador = -1;
                        else
                        {
                            if (this.FechaCalculoBase.Value > fechaValidacion)
                                indicador = 1;
                            else
                            {
                                var fechaValidacionDos = DateTime.Now.AddMonths(this.IndicadorMeses2);
                                indicador = this.FechaCalculoBase.Value > fechaValidacionDos ? 0 : -1;
                            }
                        }
                    }
                    else
                        indicador = -1;
                }

                return indicador;
            }
        }
    }
}

