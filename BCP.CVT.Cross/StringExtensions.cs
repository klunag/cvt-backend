using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Cross
{
    public static class StringExtensions
    {
        public static decimal ToSizeByte(this string text)
        {
            if (string.IsNullOrEmpty((text ?? "").Trim())) throw new Exception("No se puede convertir porque el texto está vacío.");
            var textArray = text.Split(' ');
            if (textArray.Length != 2) throw new Exception("El texto no tiene el formato correcto ({VALOR} {UNIDAD}).");
            string value = textArray[0];
            string unidadTamaño = textArray[1];
            decimal valueInt = 0;
            UnidadesTamañoDigital unidadTamañoDigital = UnidadesTamañoDigital.Byte;

            if (!decimal.TryParse(value, out valueInt)) throw new Exception($"No se puede convertir {value} a valor numérico.");
            if (!Enum.TryParse(unidadTamaño, out unidadTamañoDigital)) throw new Exception($"No se puede convertir {unidadTamaño}, los valores aceptados son: {string.Join(", ", Utilitarios.EnumToList<UnidadesTamañoDigital>().Select(x => Utilitarios.GetEnumDescription2(x)).ToArray())}.");

            int cantidadRepeticiones = (int)unidadTamañoDigital;
            //for (int i = 0; i < cantidadRepeticiones; i++) resultado = (i == 0 ? 0 : resultado) + resultado * 1024M;
            decimal resultado = decimal.Parse((valueInt * (decimal)Math.Pow(1024D, (double)cantidadRepeticiones)).ToString("0.00"));
            return resultado;
            //return (valueInt / (decimal)Math.Pow(1024, (long)unidadTamañoDigital));
            //return resultado;
        }
    }
}
