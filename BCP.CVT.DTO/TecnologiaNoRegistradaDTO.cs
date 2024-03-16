using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TecnologiaNoRegistradaDTO : BaseDTO
    {
        public string Aplicacion { get; set; }
        public string Equipo { get; set; }
        public int? EquipoId { get; set; }
        public int? TipoEquipoId { get; set; }
        public bool FlagAsociado { get; set; }
        public string ClasificacionSugerida { get; set; }
        public string TipoEquipoToString { get; set; }
        public DateTime? FechaEscaneo { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaFinSoporteExtendido { get; set; }
        public int? Obsoleto { get; set; }
        public int Total { get; set; }
        public int TotalFilas { get; set; }
        public string FechaEscaneoStr
        {
            get { return this.FechaEscaneo.HasValue ? this.FechaEscaneo.Value.ToString("dd/MM/yyyy") : "-"; }
        }

        public string FechaFinSoporteStr
        {
            get { return this.FechaFinSoporte.HasValue ? this.FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "-"; }
        }

        public string FechaFinSoporteExtendidoStr
        {
            get { return this.FechaFinSoporteExtendido.HasValue ? this.FechaFinSoporteExtendido.Value.ToString("dd/MM/yyyy") : "-"; }
        }

        public string ObsoletoStr
        {
            get { return this.Obsoleto.HasValue ? this.Obsoleto.Value == 1 ? "Si" : "No" : ""; }
        }

        public int Meses { get; set; }
        public int IndicadorObsolescencia
        {
            get
            {
                var indicador = 1;
                if (this.Obsoleto == 1)
                    indicador = -1;
                else
                {
                    var fechaValidacion = DateTime.Now.AddMonths(this.Meses);
                    if (this.FechaFinSoporte.HasValue)
                        indicador = this.FechaFinSoporte.Value > fechaValidacion ? 1 : 0;
                    else
                        indicador = -1;
                }

                return indicador;
            }
        }
        public int IndicadorMeses1 { get; set; }
        public int IndicadorMeses2 { get; set; }

        public int IndicadorObsolescencia_Proyeccion1
        {
            get
            {
                var indicador = 1;
                if (this.Obsoleto == 1)
                    indicador = -1;
                else
                {
                    var fechaValidacion = DateTime.Now.AddMonths(this.Meses + this.IndicadorMeses1);
                    if (this.FechaFinSoporte.HasValue)
                    {
                        if (IndicadorObsolescencia == 0 || IndicadorObsolescencia == -1)
                            indicador = -1;
                        else
                            indicador = this.FechaFinSoporte.Value > fechaValidacion ? 1 : 0;
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
                    var fechaValidacion = DateTime.Now.AddMonths(this.Meses + this.IndicadorMeses2);
                    if (this.FechaFinSoporte.HasValue)
                    {
                        if (IndicadorObsolescencia_Proyeccion1 == 0 || IndicadorObsolescencia_Proyeccion1 == -1)
                            indicador = -1;
                        else
                            indicador = this.FechaFinSoporte.Value > fechaValidacion ? 1 : 0;
                    }
                    else
                        indicador = -1;
                }

                return indicador;
            }
        }
    }

    public class TecnologiaNoRegistradaVistaDTO : BaseDTO
    {
        public string Tecnologia { get; set; }
        public string Producto { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string TribuCoe { get; set; }
        public string Squad { get; set; }
        public string Owner { get; set; }
        public string MatriculaOwner { get; set; }
        public string Correo { get; set; }
        public string TecnologiaRecomendada { get; set; }
        public string Equipo { get; set; }
        public int? EquipoId { get; set; }
        public int? TipoEquipoId { get; set; }
        public string TipoEquipoToString { get; set; }
        public DateTime? FechaEscaneo { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaFinSoporteExtendido { get; set; }
        public int Total { get; set; }
        public int TotalFilas { get; set; }
        public string TipoEnriquecido { get; set; }

        public string FechaFinSoporteStr
        {
            get { return this.FechaFinSoporte.HasValue ? this.FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "-"; }
        }

        public string FechaFinSoporteExtendidoStr
        {
            get { return this.FechaFinSoporteExtendido.HasValue ? this.FechaFinSoporteExtendido.Value.ToString("dd/MM/yyyy") : "-"; }
        }
    }
}
