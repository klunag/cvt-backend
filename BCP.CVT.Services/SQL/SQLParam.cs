using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.SQL
{
    public class SQLParam
    {
        public string Nombre;
        public object Valor;
        public SqlDbType Tipo;

        public SQLParam(string nombre
            , object valor
            , SqlDbType tipo)
        {
            Nombre = nombre;
            Valor = valor;
            Tipo = tipo;
        }
    }
}
