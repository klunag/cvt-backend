using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.SQL
{
    public class SQLManager
    {
        private string _cadenaConexion = string.Empty;
        public SQLManager()
        {
            _cadenaConexion = Constantes.CadenaConexion;
        }

        public DataTable GetDataTable(string sql)
        {
            DataSet resultado = null;
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.CommandType = CommandType.Text;
                    comando.CommandTimeout = 0;

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }
                conexion.Close();
                return resultado.Tables[0];
            }
        }

        public DataTable GetDataTable(string procedimiento, List<SQLParam> parametros)
        {
            DataSet resultado = null;
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(procedimiento, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    if (parametros != null)
                    {
                        foreach (var parametro in parametros)
                        {
                            comando.Parameters.Add(new SqlParameter(parametro.Nombre, parametro.Tipo) { Value = parametro.Valor });
                        }
                    }
                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }
                conexion.Close();
                return resultado.Tables[0];
            }
        }


        public DataSet EjecutarStoredProcedure(string sql, List<SQLParam> parametros)
        {
            DataSet resultado = null;
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    if (parametros != null)
                    {
                        foreach (var parametro in parametros)
                        {
                            comando.Parameters.Add(new SqlParameter(parametro.Nombre, parametro.Tipo) { Value = parametro.Valor });
                        }
                    }
                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }
                conexion.Close();
            }
            return resultado;
        }

        public void EjecutarStoredProcedure_2(string sql, List<SQLParam> parametros)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    if (parametros != null)
                    {
                        foreach (var parametro in parametros)
                        {
                            comando.Parameters.Add(new SqlParameter(parametro.Nombre, parametro.Tipo) { Value = parametro.Valor });
                        }
                    }
                    comando.ExecuteNonQuery();
                }
                conexion.Close();
            }
        }

    }
}
