using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CertificadoDigitalDTO : BaseDTO
    {
        public int TotalFilas { get; set; }
        public long RelacionId { get; set; }
        public string CodigoApt { get; set; }
        public string Aplicacion { get; set; }
        public string GestionadoPor { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
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
        public int? Obsoleto { get; set; }
        public decimal? IndiceObsolescencia { get; set; }
        public int Meses { get; set; }
        public DateTime? FechaCreacionCertificado { get; set; }
        public string FechaCreacionCertificadoStr
        {
            get
            {
                if (FechaCreacionCertificado.HasValue)
                    return FechaCreacionCertificado.HasValue ? FechaCreacionCertificado.Value.ToString("dd/MM/yyyy") : string.Empty;
                else
                    return string.Empty;

            }
        }
        public string TipoCertificado { get; set; }
        public string DescripcionTipoCertificado { get; set; }
        //Asociado a los indicadores
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }
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


        public int MesesObsolescencia { get; set; }
        public string FactoresKPI_FLooking { get; set; }

        public Boolean? FlagActivo { get; set; }

        public string FlagActivoStr
        {
            get
            {
                if (FlagActivo==true)
                    return "Activo";
                else
                    return "Inactivo";

            }
        }
    }
}
