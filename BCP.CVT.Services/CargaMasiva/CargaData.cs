using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Service;
using BCP.CVT.Services.SQL;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using Excel;
using System.IO;
using System.Linq;

namespace BCP.CVT.Services.CargaMasiva
{
    public class CargaData
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, int> LISTA_CRITICIDAD = null;
        private Dictionary<string, int> LISTA_ROADMAP = null;
        private List<RoadMapDTO> ListObjRegistro = new List<RoadMapDTO>();
        private string CREADOPOR = string.Empty;
        private string MODIFICADO_POR = string.Empty;
        private List<string> ERROR_INSERT_EQUIPO = new List<string>();
        private List<string> ERROR_INSERT_TNR = new List<string>();
        private string _LOG_ERRORES = string.Empty;
        private int CRITICIDADID_DEFAULT = 0;
        private List<DominioDTO> DOMINIOS = new List<DominioDTO>();
        private List<SubdominioDTO> SUBDOMINIOS = new List<SubdominioDTO>();
        private List<TipoDTO> TIPOS = new List<TipoDTO>();

        public CargaData()
        {
            DOMINIOS = ServiceManager<DominioDAO>.Provider.GetAllDominio();
            SUBDOMINIOS = ServiceManager<SubdominioDAO>.Provider.GetSubdominio();
            TIPOS = ServiceManager<TipoDAO>.Provider.GetAllTipo();
        }

        public string CargaMasivaExcel(string rutaArchivo, int TipoCargaMasiva)
        {
            try
            {
                FileStream stream = System.IO.File.Open(rutaArchivo, FileMode.Open, FileAccess.Read);
                FileInfo fileInfo = new FileInfo(rutaArchivo);
                string ext = Path.GetExtension(fileInfo.Name.ToString());
                switch ((ETipoCargaMasiva)TipoCargaMasiva)
                {
                    case ETipoCargaMasiva.Aplicacion: CargaMasivaAplicacion(ext, stream); break;
                    case ETipoCargaMasiva.RoadMap: CargaMasivaRoadMap(ext, stream); break;
                    default:
                        break;
                }
                return _LOG_ERRORES;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }        

        private string CargaMasivaAplicacion(string ext, Stream stream)
        {
            try
            {
                CREADOPOR = Utilitarios.USERNAME_AUTO;                
                ParametroDTO PARAM_CRITICIDADID_DEFAULT = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("APLICACIONES_CRITICIDADID_DEFAULT_CARGA");

                CRITICIDADID_DEFAULT = Convert.ToInt16(PARAM_CRITICIDADID_DEFAULT.Valor);
                Dictionary<string, int> COLUMNAS_APLICACION = Utilitarios.CargarColumnasExcelAplicaciones();
                var DATA_CRITICIDAD = ServiceManager<CriticidadSvc>.Provider.GetAllCriticidad();
                if (DATA_CRITICIDAD != null)
                    LISTA_CRITICIDAD = DATA_CRITICIDAD.ToDictionary(x => x.DetalleCriticidad, x => x.Id);
                else LISTA_CRITICIDAD = new Dictionary<string, int>();

                var DATA_ROADMAP = ServiceManager<RoadMapSvc>.Provider.GetAllRoadMap();
                if (DATA_ROADMAP != null)
                    LISTA_ROADMAP = DATA_ROADMAP.ToDictionary(x => x.Nombre, x => x.Id);
                else LISTA_ROADMAP = new Dictionary<string, int>();

                List<AplicacionDTO> ListObjRegistro = null;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<AplicacionDTO>();
                    string error = string.Empty;
                    //excelReader.IsFirstRowAsColumnNames = true;
                    //DataSet ds = excelReader.AsDataSet(true);
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AplicacionDTO ObjRegistro = ObtenerRegistroEntidad<AplicacionDTO>(DevolverColumnasAplicacion(dr, COLUMNAS_APLICACION), (int)ETipoCargaMasiva.Aplicacion, null, out error);
                        if (ObjRegistro != null)
                        {
                            ListObjRegistro.Add(ObjRegistro);
                        }
                    }
                    excelReader.Close();
                }
                PreparaXMLEnviarSP(ListObjRegistro);
                return _LOG_ERRORES;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private void PreparaXMLEnviarSP(List<AplicacionDTO> ListObjRegistro)
        {
            try
            {
                DataSet ds = new DataSet("APLICACIONES");
                DataTable tbl = new DataTable("APLICACION");

                tbl.Columns.Add("CodigoAPT", typeof(string));
                tbl.Columns.Add("Nombre", typeof(string));
                tbl.Columns.Add("TipoActivoInformacion", typeof(string));
                tbl.Columns.Add("GerenciaCentral", typeof(string));
                tbl.Columns.Add("Division", typeof(string));

                tbl.Columns.Add("Area", typeof(string));
                tbl.Columns.Add("Unidad", typeof(string));
                tbl.Columns.Add("DescripcionAplicacion", typeof(string));
                tbl.Columns.Add("EstadoAplicacion", typeof(string));
                tbl.Columns.Add("FechaRegistroProcedencia", typeof(DateTime));
                tbl.Columns.Add("FechaCreacionProcedencia", typeof(DateTime));

                tbl.Columns.Add("AreaBIAN", typeof(string));
                tbl.Columns.Add("DominioBIAN", typeof(string));
                tbl.Columns.Add("JefaturaATI", typeof(string));
                tbl.Columns.Add("TribeTechnicalLead", typeof(string));
                tbl.Columns.Add("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner", typeof(string));

                tbl.Columns.Add("NombreEquipo_Squad", typeof(string));
                tbl.Columns.Add("GestionadoPor", typeof(string));
                tbl.Columns.Add("Owner_LiderUsuario_ProductOwner", typeof(string));
                tbl.Columns.Add("Gestor_UsuarioAutorizador_ProductOwner", typeof(string));
                tbl.Columns.Add("Experto_Especialista", typeof(string));
                tbl.Columns.Add("EntidadResponsable", typeof(string));

                tbl.Columns.Add("CriticidadId", typeof(int));
                tbl.Columns.Add("CategoriaTecnologica", typeof(string));
                tbl.Columns.Add("RoadMapId", typeof(int));

                tbl.Columns.Add("FechaCreacion", typeof(DateTime));
                tbl.Columns.Add("CreadoPor", typeof(string));
                ds.Tables.Add(tbl);

                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["APLICACION"].NewRow();
                    dr["CodigoAPT"] = item.CodigoAPT;
                    dr["Nombre"] = item.Nombre;
                    dr["TipoActivoInformacion"] = item.TipoActivoInformacion;
                    dr["GerenciaCentral"] = item.GerenciaCentral;
                    dr["Division"] = item.Division;

                    dr["Area"] = item.Area;
                    dr["Unidad"] = item.Unidad;
                    dr["DescripcionAplicacion"] = item.DescripcionAplicacion;
                    dr["EstadoAplicacion"] = item.EstadoAplicacion;
                    dr["FechaRegistroProcedencia"] = item.FechaRegistroProcedencia.HasValue ? (object)item.FechaRegistroProcedencia : DBNull.Value;
                    dr["FechaCreacionProcedencia"] = item.FechaCreacionProcedencia.HasValue ? (object)item.FechaCreacionProcedencia : DBNull.Value;

                    dr["AreaBIAN"] = item.AreaBIAN;
                    dr["DominioBIAN"] = item.DominioBIAN;
                    dr["JefaturaATI"] = item.JefaturaATI;
                    dr["TribeTechnicalLead"] = item.TribeTechnicalLead;
                    dr["JefeEquipo_ExpertoAplicacionUserIT_ProductOwner"] = item.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner;

                    dr["NombreEquipo_Squad"] = item.NombreEquipo_Squad;
                    dr["GestionadoPor"] = item.GestionadoPor;
                    dr["Owner_LiderUsuario_ProductOwner"] = item.Owner_LiderUsuario_ProductOwner;
                    dr["Gestor_UsuarioAutorizador_ProductOwner"] = item.Gestor_UsuarioAutorizador_ProductOwner;
                    dr["Experto_Especialista"] = item.Experto_Especialista;
                    dr["EntidadResponsable"] = item.EntidadResponsable;

                    dr["CriticidadId"] = item.CriticidadId;
                    dr["CategoriaTecnologica"] = item.CategoriaTecnologica;
                    dr["RoadMapId"] = item.RoadMapId;
                    dr["FechaCreacion"] = DateTime.Now;

                    dr["CreadoPor"] = item.UsuarioCreacion;
                    ds.Tables["APLICACION"].Rows.Add(dr);
                }
                string xmlStr = ds.GetXml();
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_INSERT_APLICACION_FROM_EXCEL]", ListsQLParam);
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private string CargaMasivaRoadMap(string ext, Stream stream)
        {
            try
            {
                CREADOPOR = Utilitarios.USERNAME_AUTO;
                ListObjRegistro = new List<RoadMapDTO>();
                //var _LISTA_ROADMAP = ServiceManager<RoadMapSvc>.Provider.GetAllRoadMap();
                //if (_LISTA_ROADMAP.Count > 0)
                //{
                LISTA_ROADMAP = ServiceManager<RoadMapSvc>.Provider.GetAllRoadMap().ToDictionary(x => x.Nombre, x => x.Id);
                //}
                Dictionary<string, int> dictionary = Utilitarios.CargarColumnasExcelAplicaciones();
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    //excelReader.IsFirstRowAsColumnNames = true;
                    //DataSet ds = excelReader.AsDataSet(true);
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    string error = string.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RoadMapDTO ObjRegistro = ObtenerRegistroEntidad<RoadMapDTO>(DevolverColumnasRoadMap(dr, dictionary), (int)ETipoCargaMasiva.RoadMap, null, out error);
                        if (ObjRegistro != null && !string.IsNullOrEmpty(ObjRegistro.Nombre))
                        {
                            ListObjRegistro.Add(ObjRegistro);
                        }
                    }
                    excelReader.Close();
                }
                ListObjRegistro = ListObjRegistro.Distinct().ToList();
                foreach (var item in ListObjRegistro)
                {
                    ServiceManager<RoadMapSvc>.Provider.AddOrEdit(item);
                }
                return _LOG_ERRORES;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private IExcelDataReader ControlarExtension(string ext, Stream stream)
        {
            //if (ext.ToLower().Equals(Utilitarios.EXTENSION_EXCEL))
            //{
            //    return ExcelReaderFactory.CreateOpenXmlReader(stream);
            //}
            //else
            //{
            //    return ExcelReaderFactory.CreateBinaryReader(stream, true);
            //}
            if (ext.ToLower().Equals(Utilitarios.EXTENSION_EXCEL))
            {
                return ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
                return null;
        }
        private T ObtenerRegistroEntidad<T>(List<string> detalleFilaExcel, int TipoCargaMasiva, int? fila, out string errorFila)
        {
            errorFila = string.Empty;
            //bool flagValidar = true;
            EstadoFila flagValidar = new EstadoFila();

            switch ((ETipoCargaMasiva)TipoCargaMasiva)
            {
                case ETipoCargaMasiva.Aplicacion:
                    flagValidar = ValidarFilaAplicacion(detalleFilaExcel);
                    break;
                case ETipoCargaMasiva.Equipo:
                    flagValidar = ValidarFilaEquipo(detalleFilaExcel);
                    break;
                case ETipoCargaMasiva.UpdateEquipo:
                    flagValidar = ValidarFilaEquipoUpdate(detalleFilaExcel);
                    break;
                case ETipoCargaMasiva.TecnologiaNoRegistrada:
                    flagValidar = ValidarFilaTecnologiaNoRegistrada(detalleFilaExcel);
                    break;
                case ETipoCargaMasiva.Tecnologia:
                    flagValidar = ValidarFilaTecnologia(detalleFilaExcel);
                    break;
            }

            T objEntidad = default(T);
            if (flagValidar.Estado)
            {
                switch ((ETipoCargaMasiva)TipoCargaMasiva)
                {
                    case ETipoCargaMasiva.Aplicacion:
                        objEntidad = (T)(object)ObtenerEntidadAplicacion(detalleFilaExcel, true);
                        break;
                    case ETipoCargaMasiva.RoadMap:
                        objEntidad = (T)(object)ObtenerEntidadRoadMap(detalleFilaExcel);
                        break;
                    case ETipoCargaMasiva.Equipo:
                        objEntidad = (T)(object)ObtenerEntidadEquipo(detalleFilaExcel, true);
                        //logInsert = flagValidar.Mensaje;
                        break;
                    case ETipoCargaMasiva.UpdateEquipo:
                        objEntidad = (T)(object)ObtenerEntidadEquipoUpdate(detalleFilaExcel, true);
                        break;
                    case ETipoCargaMasiva.TecnologiaNoRegistrada:
                        objEntidad = (T)(object)ObtenerEntidadTecnologiaNoRegistrada(detalleFilaExcel, true, out errorFila);
                        errorFila = string.Format(errorFila, fila);
                        break;
                    case ETipoCargaMasiva.Tecnologia:
                        objEntidad = (T)(object)ObtenerEntidadTecnologia(detalleFilaExcel, true, out errorFila);
                        errorFila = string.Format(errorFila, fila);
                        break;                        
                }
            }
            else
            {
                switch ((ETipoCargaMasiva)TipoCargaMasiva)
                {
                    case ETipoCargaMasiva.Aplicacion:
                        objEntidad = (T)(object)ObtenerEntidadAplicacion(detalleFilaExcel, false);
                        break;
                    case ETipoCargaMasiva.Equipo:
                        errorFila = string.Format(flagValidar.Mensaje, fila);
                        break;
                    case ETipoCargaMasiva.UpdateEquipo:
                        errorFila = string.Format(flagValidar.Mensaje, fila);
                        break;
                    case ETipoCargaMasiva.TecnologiaNoRegistrada:
                        errorFila = string.Format(flagValidar.Mensaje, fila);
                        break;
                    case ETipoCargaMasiva.Tecnologia:
                        errorFila = string.Format(flagValidar.Mensaje, fila);
                        break;
                }
            }
            return objEntidad;
        }
        #region APLICACION
        private EstadoFila ValidarFilaAplicacion(List<string> detalleFilaExcel)
        {
            //bool estado = true;
            EstadoFila flagValidar = new EstadoFila();
            flagValidar.Estado = true;

            DateTime DATETIME_OUT = DateTime.Now;
            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                //estado = true;
                if (flagValidar.Estado)
                {
                    switch (i)
                    {
                        //case 1:
                        //case 2:
                        //case 4:
                        //case 5:
                        //case 6:
                        //case 7:
                        //case 8:
                        //case 9:
                        //case 10:
                        //case 13:
                        //case 14:
                        //case 16:
                        //case 17:
                        //case 18:
                        //case 20:
                        //case 21:
                        //case 22:
                        //case 23:
                        //case 25:
                        //case 35: { estado = !string.IsNullOrEmpty(detalleFilaExcel[i]); } break;
                        case 9:
                        case 24: { flagValidar.Estado = DateTime.TryParse(detalleFilaExcel[i], out DATETIME_OUT); } break;
                    }
                }
            }

            return flagValidar;
        }
        private AplicacionDTO ObtenerEntidadAplicacion(List<string> detalleFilaExcel, bool estado)
        {
            AplicacionDTO entidad = new AplicacionDTO();
            entidad.ValidarExcel = estado;
            bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (FLAGCONTINUAR)
                {
                    switch (i)
                    {
                        case 0: { entidad.CodigoAPT = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1: { entidad.Nombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 2: { entidad.TipoActivoInformacion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 3: { entidad.GerenciaCentral = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 4: { entidad.Division = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        case 5: { entidad.Area = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 6: { entidad.Unidad = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 7: { entidad.DescripcionAplicacion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 8: { entidad.EstadoAplicacion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 9: { entidad.FechaCreacionProcedencia = (estado ? Convert.ToDateTime(detalleFilaExcel[i]) : (DateTime?)null); } break;

                        case 10: { entidad.AreaBIAN = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 11: { entidad.DominioBIAN = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 12: { entidad.JefaturaATI = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 13: { entidad.TribeTechnicalLead = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 14: { entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        case 15: { entidad.NombreEquipo_Squad = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 16: { entidad.GestionadoPor = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 17: { entidad.Owner_LiderUsuario_ProductOwner = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 18: { entidad.Gestor_UsuarioAutorizador_ProductOwner = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 19: { entidad.Experto_Especialista = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 20: { entidad.EntidadResponsable = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                        case 21: { entidad.CriticidadId = ObtenerIdCriticidad(detalleFilaExcel[i].Trim()); } break;
                        case 22: { entidad.CategoriaTecnologica = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 23: { entidad.RoadMapId = ObtenerIdRoadMap(detalleFilaExcel[i]); } break;
                        case 24: { entidad.FechaRegistroProcedencia = (estado ? Convert.ToDateTime(detalleFilaExcel[i]) : (DateTime?)null); } break;
                    }
                }
            }
            entidad.Activo = true;
            entidad.UsuarioCreacion = CREADOPOR;
            if (!FLAGCONTINUAR) entidad = null;

            return entidad;
        }

        private int ObtenerIdCriticidad(string Nombre)
        {
            int ID = Utilitarios.ObtenerValueByKey(Nombre, LISTA_CRITICIDAD);
            if (ID == 0)
            {
                ID = CRITICIDADID_DEFAULT; // REPORTAR ALERTA Y ASIGNAR => NO DETERMINADA
            }
            return ID;
        }
        private int ObtenerIdRoadMap(string Nombre)
        {
            int ID = 0;
            //if (LISTA_ROADMAP != null)
            //{
            ID = Utilitarios.ObtenerValueByKey(Nombre, LISTA_ROADMAP);
            //}
            return ID;
        }
        private List<string> DevolverColumnasAplicacion(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CodigoAPT", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("NombreAplicacion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TipoActivoInformacion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("GerenciaCentral", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Division", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Area", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Unidad", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("DescripcionAplicacion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("EstadoAplicacionProcedencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaCreacionProcedencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("AreaBIAN", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("DominioBIAN", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("JefaturaATI", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TribeTechnicalLead", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("NombreEquipo_Squad", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("GestionadoPor", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Owner_LiderUsuario_ProductOwner", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Gestor_UsuarioAutorizador_ProductOwner", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Experto_Especialista", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("EntidadResponsable", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CriticidadId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CategoriaTecnologica", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("RoadMapId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaRegistroProcedencia", dictionary)].ToString());
            return listaData;
        }
        #endregion
        #region ROADMAP
        private bool ValidarFilaRoadMap(List<string> detalleFilaExcel)
        {
            for (int i = 0; i < detalleFilaExcel.Count(); i++)
            {
                if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                    return false;
            }
            return true;
        }
        private RoadMapDTO ObtenerEntidadRoadMap(List<string> detalleFilaExcel)
        {
            RoadMapDTO entidad = null;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                int cant = ListObjRegistro.Where(x => x.Nombre.Trim().ToUpper() == detalleFilaExcel[i].Trim().ToUpper()).Count();
                bool FlagExiste = cant > 0 || Utilitarios.ObtenerValueByKey(detalleFilaExcel[i], LISTA_ROADMAP) > 0;
                if (!FlagExiste)
                {
                    entidad = new RoadMapDTO();
                    entidad.Nombre = detalleFilaExcel[i].Trim();
                    entidad.Activo = true;
                    entidad.UsuarioCreacion = CREADOPOR;
                }
            }
            return entidad;
        }
        private List<string> DevolverColumnasRoadMap(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("RoadMapId", dictionary)].ToString());
            return listaData;
        }
        #endregion

        #region GESTION_EQUIPOS
        private int ObtenerIdAmbiente(string nombre)
        {
            int ID = 0;
            var ambiente = ServiceManager<EquipoDAO>.Provider.ExisteAmbienteByNombre(nombre);
            ID = (ambiente != null) ? ambiente.Id : 6 ;

            return ID;
        }

        private int ObtenerIdSubsidiaria(string nombre)
        {
            int ID = 0;
            var entidad = ServiceManager<EquipoDAO>.Provider.ExisteDominioServidorByNombre(nombre);
            ID = (entidad != null) ? entidad.Id : 0;

            return ID;
        }

        private int ObtenerIdEquipo(string nombre)
        {
            int ID = 0;
            var equipo = ServiceManager<EquipoDAO>.Provider.ExisteEquipoAllByHostname(nombre);
            //ID = (equipo != null) ? equipo.EquipoId : 0;
            if (equipo != null) ID = equipo.EquipoId;

            return ID;
        }

        private int ObtenerIdTipoEquipo(string nombre)
        {
            int ID = 0;
            var equipo = ServiceManager<EquipoDAO>.Provider.GetTipoEquipoByNombre(nombre);
            if (equipo != null) ID = equipo.Id;

            return ID;
        }

        private string ModelStr(string nombre)
        {
            string modelStr = string.Empty;

            if (nombre.ToUpper().Contains("FISICO") || nombre.ToUpper().Contains("FÍSICO"))
                return modelStr;
            else
            {
                modelStr = "Virtual Machine";
                return modelStr;
            }  
        }

        private bool EstadoStr(string nombre)
        {
            string modelStr = string.Empty;
            bool estado = false;
            switch (nombre.ToUpper())
            {
                case "ACTIVO":
                    estado = true;
                    break;
                case "INACTIVO":
                    estado = false;
                    break;
            }
            return estado;
        }

        private bool ValidarIdTipoEquipo(string nombre) => ServiceManager<EquipoDAO>.Provider.ExisteTipoEquipoByNombre(nombre.Trim());
        private bool ValidarIdServidor(string nombre) => ServiceManager<EquipoDAO>.Provider.ExisteServidorByNombre(nombre.Trim());
        private bool ValidarIdEquipo(string nombre) => ServiceManager<EquipoDAO>.Provider.ExisteEquipoByNombre(nombre.Trim());

        //Validacion Enums Appliance
        private int ObtenerIdTipoActivo(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<ETipoActivoEquipo>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();
            
            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdDimension(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<EDimensionEquipo>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdFechaCalculo(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<FechaCalculoTecnologia>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdSede(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<ESedeEquipo>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdBackup(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<EBackup>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdBackupFrecuencia(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<EBackupFrecuencia>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdIntegracionGestor(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<EIntegracionGestorInteligencia>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdCona(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<ECONA>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private int ObtenerIdCriticidadEquipo(string elemento)
        {
            int ID = 0;
            var lGeneral = Utilitarios.EnumToList<ECriticidadEquipo>();
            var listaEnum = lGeneral.Select(x => new { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
            var entidad = listaEnum.Where(x => x.Descripcion.ToUpper().Contains(elemento.ToUpper())).FirstOrDefault();

            return entidad != null ? entidad.Id : ID;
        }

        private EstadoFila ValidarFilaEquipo(List<string> detalleFilaExcel)
        {
            //bool estado = true;
            EstadoFila obj = new EstadoFila();
            obj.Mensaje = string.Empty;
            obj.Estado = true;
            int TipoEquipoId = 0;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                //estado = true;
                if (obj.Estado)
                {
                    switch (i)
                    {
                        case 4:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Nombre de {0} vacío, fila {1} del archivo", "equipo", "{0}");
                            }
                            else
                            {
                                switch (TipoEquipoId)
                                {
                                    case (int)ETipoEquipo.Servidor:
                                        if (ValidarIdServidor(detalleFilaExcel[i]))
                                        {
                                            obj.Estado = false;
                                            obj.Mensaje = string.Format("Nombre de {0} ya existe, fila {1} del archivo", "servidor", "{0}");//"Equipo ya existe";
                                        }
                                        break;
                                    case (int)ETipoEquipo.ServidorAgencia:
                                    case (int)ETipoEquipo.PC:
                                    case (int)ETipoEquipo.Storage:
                                    case (int)ETipoEquipo.Appliance:
                                    case (int)ETipoEquipo.ServicioNube:
                                        if (ValidarIdEquipo(detalleFilaExcel[i]))
                                        {
                                            obj.Estado = false;
                                            obj.Mensaje = string.Format("Nombre de {0} ya existe, fila {1} del archivo", "equipo", "{0}");//"Equipo ya existe";
                                        }
                                        break;
                                }                          
                            }
                            break;

                        case 1:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Nombre de {0} vacío, fila {1} del archivo", "ambiente", "{0}");
                            }
                            break;

                        case 2:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Nombre de {0} vacío, fila {1} del archivo", "model", "{0}");
                            }
                            break;

                        case 3:
                            break;

                        case 0:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Nombre de {0} vacío, fila {1} del archivo", "Tipo equipo", "{0}");
                            }
                            else
                            {
                                var ID = ObtenerIdTipoEquipo(detalleFilaExcel[i]);
                                if (ID == 0)
                                {
                                    obj.Estado = false;
                                    obj.Mensaje = string.Format("Nombre de {0} no existe, fila {1} del archivo", "Tipo equipo", "{0}");
                                }
                                else
                                    TipoEquipoId = ID;
                            }
                            break;

                        case 5:
                            break;
                    }
                }
            }

            return obj;
        }

        private EstadoFila ValidarFilaEquipoUpdate(List<string> detalleFilaExcel)
        {
            //bool estado = true;
            EstadoFila obj = new EstadoFila();
            obj.Mensaje = string.Empty;
            obj.Estado = true;

            //DateTime DATETIME_OUT = DateTime.Now;
            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                //estado = true;
                if (obj.Estado)
                {
                    switch (i)
                    {
                        //case 0:
                        //    //if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                        //    //{
                        //    //    obj.Estado = false;
                        //    //    obj.Mensaje = string.Format("Fila {0} incompleta del archivo, Columna {1}", "{0}", i + 1);
                        //    //}
                        //    //else
                        //    //{
                        //    //    var estadoEquipo = ServiceManager<EquipoDAO>.Provider.ExisteEquipoByNombre(detalleFilaExcel[i]);
                        //    //    if (!estadoEquipo)
                        //    //    {
                        //    //        obj.Estado = false;
                        //    //        obj.Mensaje = string.Format("Nombre de {0} no existe en la fila {1} del archivo", "equipo", "{0}");
                        //    //    }
                        //    //}
                        //    break;

                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Fila {0} incompleta del archivo, Columna {1}", "{0}", i + 1);
                            }
                            break;

                        //case 2:
                        //    if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                        //    {
                        //        obj.Estado = false;
                        //        obj.Mensaje = string.Format("Nombre de {0} vacío en la fila {1} del archivo", "subsidiaria", "{0}");
                        //    }
                        //    break;

                        //case 3:
                        //    if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                        //    {
                        //        obj.Estado = false;
                        //        obj.Mensaje = string.Format("Nombre de {0} vacío en la fila {1} del archivo", "estado", "{0}");
                        //    }
                        //    break;
                    }
                }
            }

            return obj;
        }

        private EstadoFila ValidarFilaTecnologiaNoRegistrada(List<string> detalleFilaExcel)
        {
            EstadoFila obj = new EstadoFila();
            obj.Mensaje = string.Empty;
            obj.Estado = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (obj.Estado)
                {
                    switch (i)
                    {
                        case 0:
                        //case 1:
                        //case 2:
                        case 3:
                        case 4: 
                        case 5:
                        case 6: //Familia
                        case 7: //Tipo
                        case 8: //Dominio
                        case 9: //Subdominio
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Fila {0} incompleta del archivo, Columna {1}", "{0}", i + 1);
                            }
                            break;
                    }
                }
            }

            return obj;
        }

        private EstadoFila ValidarFilaTecnologia(List<string> detalleFilaExcel)
        {
            EstadoFila obj = new EstadoFila();
            obj.Mensaje = string.Empty;
            obj.Estado = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (obj.Estado)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6: 
                        case 7: 
                        case 13:                        
                            if (string.IsNullOrEmpty(detalleFilaExcel[i]))
                            {
                                obj.Estado = false;
                                obj.Mensaje = string.Format("Fila {0} incompleta del archivo, Columna {1}", "{0}", i + 1);
                            }
                            break;
                    }
                }
            }

            return obj;
        }

        private void PreparaXMLEnviarSPEquipo(List<EquipoDTO> ListObjRegistro)
        {
            try
            {
                DataSet ds = new DataSet("EQUIPOS");
                DataTable tbl = new DataTable("EQUIPO");

                tbl.Columns.Add("TipoEquipo", typeof(int));
                tbl.Columns.Add("Ambiente", typeof(string));
                tbl.Columns.Add("Model", typeof(string));
                tbl.Columns.Add("Dominio", typeof(string));
                tbl.Columns.Add("Equipo", typeof(string));
                tbl.Columns.Add("SO", typeof(string));

                ds.Tables.Add(tbl);

                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["EQUIPO"].NewRow();
                    dr["TipoEquipo"] = item.TipoEquipoId;
                    dr["Ambiente"] = item.AmbienteId;
                    dr["Model"] = item.Model;
                    dr["Dominio"] = item.Dominio;
                    dr["Equipo"] = item.Nombre;
                    dr["SO"] = item.SistemaOperativo;

                    ds.Tables["EQUIPO"].Rows.Add(dr);
                }
                string xmlStr = ds.GetXml();
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_INSERT_EQUIPO_FROM_EXCEL]", ListsQLParam);
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        //TODO
        private void PreparaXMLEnviarSPTecnologiaNoRegistrada(List<TecnologiaCargaDto> ListObjRegistro)
        {
            try
            {
                DataSet ds = new DataSet("TECNOLOGIAS");
                DataTable tbl = new DataTable("TECNOLOGIA");

                tbl.Columns.Add("Tecnologia", typeof(string));
                tbl.Columns.Add("FechaFinSoporte", typeof(DateTime));
                tbl.Columns.Add("FechaFinSoporteExtendido", typeof(DateTime));
                tbl.Columns.Add("Fabricante", typeof(string));
                tbl.Columns.Add("Nombre", typeof(string));
                tbl.Columns.Add("Version", typeof(string));
                tbl.Columns.Add("Familia", typeof(int));
                tbl.Columns.Add("Tipo", typeof(int));
                tbl.Columns.Add("Dominio", typeof(int));
                tbl.Columns.Add("Subdominio", typeof(int));
                tbl.Columns.Add("Clave", typeof(string));

                ds.Tables.Add(tbl);

                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["TECNOLOGIA"].NewRow();
                    dr["Tecnologia"] = item.TecnologiaEquivalenteStr;
                    //dr["FechaFinSoporte"] = item.FechaFinSoporte;
                    //dr["FechaFinSoporteExtendido"] = item.FechaFinSoporteExtendida;
                    dr["FechaFinSoporte"] = item.FechaFinSoporte.HasValue ? (object)item.FechaFinSoporte : DBNull.Value;
                    dr["FechaFinSoporteExtendido"] = item.FechaFinSoporteExtendida.HasValue ? (object)item.FechaFinSoporteExtendida : DBNull.Value;
                    dr["Fabricante"] = item.Fabricante;
                    dr["Nombre"] = item.Nombre;
                    dr["Version"] = item.Version;
                    dr["Familia"] = item.FamiliaId;
                    dr["Tipo"] = item.TipoId;
                    dr["Dominio"] = item.DominioId;
                    dr["Subdominio"] = item.SubdominioId;
                    dr["Clave"] = item.ClaveTecnologia;

                    ds.Tables["TECNOLOGIA"].Rows.Add(dr);
                }
                string xmlStr = ds.GetXml();
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_INSERT_TECNOLOGIA_NO_REGISTRADA_FROM_EXCEL]", ListsQLParam);
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void PreparaXMLEnviarSPTecnologia(List<TecnologiaCargaDto> ListObjRegistro, string usuario)
        {
            try
            {
                DataSet ds = new DataSet("TECNOLOGIAS");
                DataTable tbl = new DataTable("TECNOLOGIA");

                tbl.Columns.Add("ClaveTecnologia", typeof(string));
                tbl.Columns.Add("Dominio", typeof(int));
                tbl.Columns.Add("Subdominio", typeof(int));
                tbl.Columns.Add("Estado", typeof(string));
                tbl.Columns.Add("Fabricante", typeof(string));
                tbl.Columns.Add("Nombre", typeof(string));
                tbl.Columns.Add("Version", typeof(string));
                tbl.Columns.Add("TipoTecnologia", typeof(int));
                tbl.Columns.Add("CodigoTecnologia", typeof(string));
                tbl.Columns.Add("MostrarSite", typeof(string));
                tbl.Columns.Add("CasoUso", typeof(string));
                tbl.Columns.Add("Descripcion", typeof(string));
                tbl.Columns.Add("FechaLanzamiento", typeof(DateTime));
                tbl.Columns.Add("TieneFechaFin", typeof(string));
                tbl.Columns.Add("Fuente", typeof(string));
                tbl.Columns.Add("FechaFinSoporte", typeof(DateTime));
                tbl.Columns.Add("FechaFinExtendida", typeof(DateTime));
                tbl.Columns.Add("FechaFinInterna", typeof(DateTime));
                tbl.Columns.Add("EquipoAdministracion", typeof(string));
                tbl.Columns.Add("SoporteRemedy", typeof(string));
                tbl.Columns.Add("ConformidadSeguridad", typeof(string));
                tbl.Columns.Add("ConformidadArquitecto", typeof(string));
                tbl.Columns.Add("Confluence", typeof(string));
                tbl.Columns.Add("UsuarioModificacion", typeof(string));

                ds.Tables.Add(tbl);

                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["TECNOLOGIA"].NewRow();
                    dr["ClaveTecnologia"] = item.ClaveTecnologiaValidar;
                    dr["Dominio"] = item.DominioId;
                    dr["Subdominio"] = item.SubdominioId;
                    dr["Estado"] = item.Estado;
                    dr["Fabricante"] = item.Fabricante;
                    dr["Nombre"] = item.Nombre;
                    dr["Version"] = item.Version;
                    dr["TipoTecnologia"] = item.TipoId;
                    dr["CodigoTecnologia"] = item.CodigoTecnologia;
                    dr["MostrarSite"] = item.MostrarSite;
                    dr["CasoUso"] = item.CasoUso;
                    dr["Descripcion"] = item.Descripcion;
                    dr["FechaLanzamiento"] = item.FechaLanzamiento.HasValue ? (object)item.FechaLanzamiento : DBNull.Value;
                    dr["TieneFechaFin"] = item.TieneFechaFinSoporte;
                    dr["Fuente"] = item.Fuente;
                    dr["FechaFinSoporte"] = item.FechaFinSoporte.HasValue ? (object)item.FechaFinSoporte : DBNull.Value;
                    dr["FechaFinExtendida"] = item.FechaFinSoporteExtendida.HasValue ? (object)item.FechaFinSoporteExtendida : DBNull.Value;
                    dr["FechaFinInterna"] = item.FechaFinInterna.HasValue ? (object)item.FechaFinInterna : DBNull.Value;
                    dr["EquipoAdministracion"] = item.EquipoAdministracion;
                    dr["SoporteRemedy"] = item.GrupoRemedy;
                    dr["ConformidadSeguridad"] = item.ConformidadSeguridad;
                    dr["ConformidadArquitecto"] = item.ConformidadTecnologia;
                    dr["Confluence"] = item.Confluence;
                    dr["UsuarioModificacion"] = usuario;

                    ds.Tables["TECNOLOGIA"].Rows.Add(dr);
                }
                string xmlStr = ds.GetXml();
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_INSERT_TECNOLOGIA_ACTUALIZACION_FROM_EXCEL]", ListsQLParam);
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private void PreparaXMLEnviarSPEquipoUpdate(List<EquipoDTO> ListObjRegistro)
        {
            try
            {
                DataSet ds = new DataSet("EQUIPOS");
                DataTable tbl = new DataTable("EQUIPO");

                tbl.Columns.Add("Equipo", typeof(int));
                tbl.Columns.Add("Ambiente", typeof(int));
                tbl.Columns.Add("Subsidiaria", typeof(int));
                tbl.Columns.Add("Estado", typeof(bool));
                tbl.Columns.Add("Usuario", typeof(string));

                ds.Tables.Add(tbl);

                foreach (var item in ListObjRegistro)
                {
                    DataRow dr = ds.Tables["EQUIPO"].NewRow();
                    dr["Equipo"] = item.EquipoId;
                    dr["Ambiente"] = item.AmbienteId;
                    dr["Subsidiaria"] = item.DominioServidorId;
                    dr["Estado"] = item.Activo;
                    dr["Usuario"] = item.UsuarioModificacion;

                    ds.Tables["EQUIPO"].Rows.Add(dr);
                }
                string xmlStr = ds.GetXml();
                List<SQLParam> ListsQLParam = new List<SQLParam>();
                ListsQLParam.Add(new SQLParam("@LISTA_XML", xmlStr, SqlDbType.NText));
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_UPDATE_EQUIPO_FROM_EXCEL]", ListsQLParam);
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public string EjecutarSPServidorEquipo()
        {
            try
            {
                DataTable data = new SQLManager().GetDataTable("[CVT].[USP_INSERT_SERVIDOR_INTO_EQUIPO]");
                string mensaje = data.Rows[0][0].ToString();
                mensaje = mensaje + data.Rows[0][1].ToString();
                return mensaje;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private EquipoDTO ObtenerEntidadEquipo(List<string> detalleFilaExcel, bool estado)
        {
            EquipoDTO entidad2 = new EquipoDTO();
            //AplicacionDTO entidad = new AplicacionDTO();
            //entidad.ValidarExcel = estado;
           // bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                //if (FLAGCONTINUAR)
                //{
                    switch (i)
                    {
                        case 4: { entidad2.Nombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1: { entidad2.AmbienteId = ObtenerIdAmbiente(detalleFilaExcel[i].Trim()); } break;
                        case 2: { entidad2.Model = ModelStr(detalleFilaExcel[i].Trim()); } break;
                        case 3: { entidad2.Dominio = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 0: { entidad2.TipoEquipoId = ObtenerIdTipoEquipo(detalleFilaExcel[i].Trim()); } break;
                        case 5: { entidad2.SistemaOperativo = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                    }
                //}
            }
            entidad2.Activo = true;
            entidad2.UsuarioCreacion = CREADOPOR;
            //if (!FLAGCONTINUAR) entidad = null;

            return entidad2;
        }
        private EquipoDTO ObtenerEntidadEquipoUpdate(List<string> detalleFilaExcel, bool estado)
        {
            EquipoDTO entidad2 = new EquipoDTO();
            //AplicacionDTO entidad = new AplicacionDTO();
            //entidad.ValidarExcel = estado;
            // bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                //if (FLAGCONTINUAR)
                //{
                switch (i)
                {
                    case 0: { entidad2.EquipoId = ObtenerIdEquipo(detalleFilaExcel[i].Trim()); } break;
                    case 1: { entidad2.AmbienteId = ObtenerIdAmbiente(detalleFilaExcel[i].Trim()); } break;
                    case 2: { entidad2.DominioServidorId = ObtenerIdSubsidiaria(detalleFilaExcel[i].Trim()); } break;
                    case 3: { entidad2.Activo = EstadoStr(detalleFilaExcel[i].Trim()); } break;
                }
                //}
            }
            //entidad2.Activo = true;
            entidad2.UsuarioModificacion = MODIFICADO_POR;
            //if (!FLAGCONTINUAR) entidad = null;

            return entidad2;
        }

        private TecnologiaCargaDto ObtenerEntidadTecnologiaNoRegistrada(List<string> detalleFilaExcel, bool estado, out string errorFila)
        {
            errorFila = "";
            TecnologiaCargaDto entidad2 = new TecnologiaCargaDto();
            DateTime FECHA_FIN = DateTime.Now;
            bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (FLAGCONTINUAR)
                {
                    switch (i)
                    {
                        case 0: { entidad2.TecnologiaEquivalenteStr = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1: { entidad2.FechaFinSoporte = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 2: { entidad2.FechaFinSoporteExtendida = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 3: { entidad2.Fabricante = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 4: { entidad2.Nombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 5: { entidad2.Version = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 6: {
                                if (!ExisteClave(entidad2.ClaveTecnologia))
                                {
                                    entidad2.FamiliaId = ObtenerIdFamilia(detalleFilaExcel[i].Trim());
                                    if (entidad2.FamiliaId == 0) {
                                        errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Familia");
                                        FLAGCONTINUAR = false;
                                    }
                                }
                                else
                                {
                                    errorFila = string.Format("Clave existente, Fila {0}", "{0}");
                                    FLAGCONTINUAR = false;
                                }
                        }
                        break;
                        case 7: {
                                entidad2.TipoId = ObtenerIdTipo(detalleFilaExcel[i].Trim());
                                if (entidad2.TipoId == 0) {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Tipo de tecnología");
                                    FLAGCONTINUAR = false;
                                } 
                        } break;
                        case 8: {
                                entidad2.DominioId = ObtenerIdDominio(detalleFilaExcel[i].Trim());
                                if (entidad2.DominioId == 0)
                                {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Dominio");
                                    FLAGCONTINUAR = false;
                                }
                        } break;
                        case 9: {
                                entidad2.SubdominioId = ObtenerIdSubdominio(detalleFilaExcel[i].Trim());
                                if (entidad2.SubdominioId == 0)
                                {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Subdominio");
                                    FLAGCONTINUAR = false;
                                }
                                else
                                {
                                    if(!ExisteDominio(entidad2.DominioId, entidad2.SubdominioId))
                                    {
                                        errorFila = string.Format("Campo {2} no encontrado en el dominio, Fila {0} - Columna {1}", "{0}", i + 1, "Subdominio");
                                        FLAGCONTINUAR = false;
                                    }
                                }
                        } break;
                    }
                }
            }
            entidad2.Activo = true;
            entidad2.UsuarioModificacion = MODIFICADO_POR;
            if (!FLAGCONTINUAR) entidad2 = null;

            return entidad2;
        }

        private TecnologiaCargaDto ObtenerEntidadTecnologia(List<string> detalleFilaExcel, bool estado, out string errorFila)
        {
            errorFila = "";
            TecnologiaCargaDto entidad2 = new TecnologiaCargaDto();
            DateTime FECHA_FIN = DateTime.Now;
            bool FLAGCONTINUAR = true;

            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (FLAGCONTINUAR)
                {
                    switch (i)
                    {
                        case 0: { entidad2.ClaveTecnologiaValidar = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 1:
                            {
                                entidad2.DominioId = ObtenerIdDominioTecnologia(detalleFilaExcel[i].Trim());
                                if (entidad2.DominioId == 0)
                                {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Dominio");
                                    FLAGCONTINUAR = false;
                                }
                            }
                            break;
                        case 2:
                            {
                                entidad2.SubdominioId = ObtenerIdSubdominioTecnologia(detalleFilaExcel[i].Trim(), entidad2.DominioId);
                                if (entidad2.SubdominioId == 0)
                                {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Subdominio");
                                    FLAGCONTINUAR = false;
                                }                                
                            }
                            break;
                        case 3: { entidad2.Estado = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 4: { entidad2.Fabricante = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 5: { entidad2.Nombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 6: { entidad2.Version = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 7:
                            {
                                entidad2.TipoId = ObtenerIdTipoTecnologia(detalleFilaExcel[i].Trim());
                                if (entidad2.TipoId == 0)
                                {
                                    errorFila = string.Format("Campo {2} no encontrado, Fila {0} - Columna {1}", "{0}", i + 1, "Tipo de tecnología");
                                    FLAGCONTINUAR = false;
                                }
                            }
                            break;
                        case 8: { entidad2.CodigoTecnologia = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 9: { entidad2.MostrarSite = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 10: { entidad2.CasoUso = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 11: { entidad2.Descripcion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 12: { entidad2.FechaLanzamiento = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 13: { entidad2.TieneFechaFinSoporte = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 14: { entidad2.Fuente = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 15: { entidad2.FechaFinSoporte = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 16: { entidad2.FechaFinSoporteExtendida = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 17: { entidad2.FechaFinInterna = (DateTime.TryParse(detalleFilaExcel[i].Trim(), out FECHA_FIN) ? FECHA_FIN : (DateTime?)null); } break;
                        case 18: { entidad2.EquipoAdministracion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 19: { entidad2.GrupoRemedy = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 20: { entidad2.ConformidadSeguridad = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 21: { entidad2.ConformidadTecnologia = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                        case 22: { entidad2.Confluence = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                    }
                }
            }
            entidad2.Activo = true;
            entidad2.UsuarioModificacion = MODIFICADO_POR;
            if (!FLAGCONTINUAR) entidad2 = null;

            return entidad2;
        }

        private List<string> DevolverColumnasEquipos(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TipoEquipo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Ambiente", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Model", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Dominio", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Equipo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("SO", dictionary)].ToString());

            return listaData;
        }
        private List<string> DevolverColumnasEquiposUpdate(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Equipo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Ambiente", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Subsidiaria", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Estado", dictionary)].ToString());

            return listaData;
        }
        private List<string> DevolverColumnasTecnologiasNoRegistradas(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Tecnologia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinSoporte", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinSoporteExtendido", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Fabricante", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Nombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Version", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Familia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Tipo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Dominio", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Subdominio", dictionary)].ToString());

            return listaData;
        }
        private List<string> DevolverColumnasTecnologias(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ClaveTecnologia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Dominio", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Subdominio", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Estado", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Fabricante", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Nombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Version", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TipoTecnologia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CodigoTecnologia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("MostrarSite", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CasoUso", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Descripcion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaLanzamiento", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TieneFechaFin", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Fuente", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinSoporte", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinExtendida", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinInterna", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("EquipoAdministracion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("SoporteRemedy", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ConformidadSeguridad", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ConformidadArquitecto", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Confluence", dictionary)].ToString());            

            return listaData;
        }

        public EstadoCargaMasiva CargaMasivaEquipos(string ext, Stream stream)
        {
            try
            {
                CREADOPOR = Utilitarios.USERNAME_AUTO;

                Dictionary<string, int> COLUMNAS_EQUIPOS = Utilitarios.CargarColumnasExcelEquipos();
               
                List<EquipoDTO> ListObjRegistro = null;
                int fila = 2;
                string logInsert = string.Empty;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<EquipoDTO>();
                    //excelReader.IsFirstRowAsColumnNames = true;
                    //DataSet ds = excelReader.AsDataSet(true);
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    var RowsCount = ds.Tables.Count;
                    if (RowsCount > 0)
                    {
                        var DataRowList = ds.Tables[0].Rows;
                        foreach (DataRow dr in DataRowList)
                        {
                            EquipoDTO ObjRegistro = ObtenerRegistroEntidad<EquipoDTO>(DevolverColumnasEquipos(dr, COLUMNAS_EQUIPOS), (int)ETipoCargaMasiva.Equipo, fila, out logInsert);
                            if (ObjRegistro != null)
                            {
                                ListObjRegistro.Add(ObjRegistro);
                            }
                            else
                            {
                                ERROR_INSERT_EQUIPO.Add(logInsert);
                            }
                            fila++;
                        }
                    }
                    excelReader.Close();
                }

                var estadoCM = new EstadoCargaMasiva();
                if (ListObjRegistro.Count > 0)
                {
                    PreparaXMLEnviarSPEquipo(ListObjRegistro);
                    estadoCM.Errores = ERROR_INSERT_EQUIPO.Count > 0 ? ERROR_INSERT_EQUIPO : new List<string>();
                    estadoCM.TotalRegistros = fila - 2;
                }
                else
                {
                    estadoCM.TotalRegistros = 0;
                    estadoCM.Errores = ERROR_INSERT_EQUIPO.Count > 0 ? ERROR_INSERT_EQUIPO : new List<string>();
                }

                return estadoCM;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public EstadoCargaMasiva CargaMasivaTecnologiasNoRegistradas(string ext, Stream stream)
        {
            try
            {
                CREADOPOR = Utilitarios.USERNAME_AUTO;

                Dictionary<string, int> COLUMNAS_TECNOLOGIAS = Utilitarios.CargarColumnasExcelTecnologiasNoRegistradas();

                List<TecnologiaCargaDto> ListObjRegistro = null;
                int fila = 2;
                string logInsert = string.Empty;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<TecnologiaCargaDto>();
                    //excelReader.IsFirstRowAsColumnNames = true;
                    //DataSet ds = excelReader.AsDataSet(true);
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    var RowsCount = ds.Tables.Count;
                    if (RowsCount > 0)
                    {
                        var DataRowList = ds.Tables[0].Rows;
                        foreach (DataRow dr in DataRowList)
                        {
                            TecnologiaCargaDto ObjRegistro = ObtenerRegistroEntidad<TecnologiaCargaDto>(DevolverColumnasTecnologiasNoRegistradas(dr, COLUMNAS_TECNOLOGIAS), (int)ETipoCargaMasiva.TecnologiaNoRegistrada, fila, out logInsert);
                            if (ObjRegistro != null)
                                ListObjRegistro.Add(ObjRegistro);
                            else
                                ERROR_INSERT_TNR.Add(logInsert);

                            fila++;
                        }
                    }
                    excelReader.Close();
                }

                var estadoCM = new EstadoCargaMasiva();
                if (ListObjRegistro.Count > 0)
                {
                    PreparaXMLEnviarSPTecnologiaNoRegistrada(ListObjRegistro);
                    estadoCM.Errores = ERROR_INSERT_TNR.Count > 0 ? ERROR_INSERT_TNR : new List<string>();
                    estadoCM.TotalRegistros = fila - 2;
                }
                else
                {
                    estadoCM.TotalRegistros = 0;
                    estadoCM.Errores = ERROR_INSERT_TNR.Count > 0 ? ERROR_INSERT_TNR : new List<string>();
                }

                return estadoCM;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public EstadoCargaMasiva CargaMasivaTecnologias(string ext, Stream stream, string usuario)
        {
            try
            {
                CREADOPOR = Utilitarios.USERNAME_AUTO;

                Dictionary<string, int> COLUMNAS_TECNOLOGIAS = Utilitarios.CargarColumnasExcelActualizarTecnologias();

                List<TecnologiaCargaDto> ListObjRegistro = null;
                int fila = 2;
                string logInsert = string.Empty;
                using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
                {
                    ListObjRegistro = new List<TecnologiaCargaDto>();
                    //excelReader.IsFirstRowAsColumnNames = true;
                    //DataSet ds = excelReader.AsDataSet(true);
                    DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    var RowsCount = ds.Tables.Count;
                    if (RowsCount > 0)
                    {
                        var DataRowList = ds.Tables[0].Rows;
                        foreach (DataRow dr in DataRowList)
                        {
                            TecnologiaCargaDto ObjRegistro = ObtenerRegistroEntidad<TecnologiaCargaDto>(DevolverColumnasTecnologias(dr, COLUMNAS_TECNOLOGIAS), (int)ETipoCargaMasiva.Tecnologia, fila, out logInsert);
                            if (ObjRegistro != null)
                                ListObjRegistro.Add(ObjRegistro);
                            else
                                ERROR_INSERT_TNR.Add(logInsert);

                            fila++;
                        }
                    }
                    excelReader.Close();
                }

                var estadoCM = new EstadoCargaMasiva();
                if (ListObjRegistro.Count > 0)
                {
                    PreparaXMLEnviarSPTecnologia(ListObjRegistro, usuario);                    
                    estadoCM.Errores = ERROR_INSERT_TNR.Count > 0 ? ERROR_INSERT_TNR : new List<string>();
                    estadoCM.TotalRegistros = fila - 2;
                }
                else
                {
                    estadoCM.TotalRegistros = 0;
                    estadoCM.Errores = ERROR_INSERT_TNR.Count > 0 ? ERROR_INSERT_TNR : new List<string>();
                }

                return estadoCM;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public EstadoCargaMasiva UpdateMasivoEquipos(string ext, Stream stream, string username)
        {
            MODIFICADO_POR = username;
            Dictionary<string, int> COLUMNAS_EQUIPOS = Utilitarios.CargarColumnasExcelEquiposUpdate();
            List<EquipoDTO> ListObjRegistro = null;
            string logInsert = string.Empty;
            int fila = 2;
            using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
            {
                ListObjRegistro = new List<EquipoDTO>();
                //excelReader.IsFirstRowAsColumnNames = true;
                //DataSet ds = excelReader.AsDataSet(true);
                DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var RowsCount = ds.Tables.Count;
                if (RowsCount > 0)
                {
                    var DataRowList = ds.Tables[0].Rows;
                    foreach (DataRow dr in DataRowList)
                    {
                        EquipoDTO ObjRegistro = ObtenerRegistroEntidad<EquipoDTO>(DevolverColumnasEquiposUpdate(dr, COLUMNAS_EQUIPOS), (int)ETipoCargaMasiva.UpdateEquipo, fila, out logInsert);
                        if (ObjRegistro != null)
                        {
                            ListObjRegistro.Add(ObjRegistro);
                        }
                        else
                        {
                            ERROR_INSERT_EQUIPO.Add(logInsert);
                        }
                        fila++;
                    }
                }
                excelReader.Close();
            }

            var estadoCM = new EstadoCargaMasiva();
            if (ListObjRegistro.Count > 0)
            {
                PreparaXMLEnviarSPEquipoUpdate(ListObjRegistro);
                estadoCM.Errores = ERROR_INSERT_EQUIPO.Count > 0 ? ERROR_INSERT_EQUIPO : new List<string>();
                estadoCM.TotalRegistros = fila - 2;
            }
            else
            {
                estadoCM.TotalRegistros = 0;
                estadoCM.Errores = ERROR_INSERT_EQUIPO.Count > 0 ? ERROR_INSERT_EQUIPO : new List<string>();
            }

            return estadoCM;
        }
        #endregion

        #region TECNOLOGIA_NO_REGISTRADA

        private int ObtenerIdFamilia(string nombre) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteFamiliaByNombre(nombre);
        private int ObtenerIdTipo(string nombre) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteTipoByNombre(nombre);
        private int ObtenerIdTipoTecnologia(string nombre)
        {
            var existe = TIPOS.FirstOrDefault(x => x.Nombre.ToUpper() == nombre.ToUpper());
            if (existe != null)
                return existe.Id;
            else
                return 0;
        }
        private int ObtenerIdDominio(string nombre) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteDominioByNombre(nombre);
        private int ObtenerIdDominioTecnologia(string nombre)
        {
            var existe = DOMINIOS.FirstOrDefault(x => x.Nombre.ToUpper() == nombre.ToUpper());
            if (existe != null)
                return existe.Id;
            else
                return 0;
        }
        private int ObtenerIdSubdominio(string nombre) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteSubdominioByNombre(nombre);
        private int ObtenerIdSubdominioTecnologia(string nombre, int dominio)
        {
            var existe = SUBDOMINIOS.FirstOrDefault(x => x.Nombre.ToUpper() == nombre.ToUpper() && x.DomIdAsociado == dominio);
            if (existe != null)
                return existe.Id;
            else
                return 0;
        }
        private bool ExisteClave(string nombre) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteClaveByNombre(nombre);
        private bool ExisteDominio(int iddominio, int idsubdominio) => ServiceManager<TecnologiaNoRegistradaDAO>.Provider.ExisteDominioBySubdominio(iddominio, idsubdominio);

        #endregion

        private List<string> DevolverColumnasAppliance(DataRow reader, Dictionary<string, int> dictionary)
        {
            List<string> listaData = new List<string>();

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Nombre", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TipoEquipoId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("AmbienteId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("DominioServidorId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("TipoActivo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Serial", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Modelo", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Vendor", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Tecnologia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Version", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Hostname", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("IP", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Dimension", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ArquitectoSeguridad", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("SoportePrimerNivel", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Proveedor", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaLanzamiento", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinSoporte", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinExtendida", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaFinInterna", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ComentariosFechaFin", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaCalculoId", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaAdquisicion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaImplementacion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FechaUltimaRenovacion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("VencimientoLicencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CantidadLicencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("FormaLicenciamiento", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CodigoInventario", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CyberSOC", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Sede", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Sala", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("RACK", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Ubicacion", dictionary)].ToString());

            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Backup", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("BackupFrecuencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("BackupDescripcion", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("IntegracionGestorInteligencia", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("ConectorSIEM", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("CONA", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("UmbralCPU", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("UmbralMemoria", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("UmbralDisco", dictionary)].ToString());
            listaData.Add(reader[Utilitarios.ObtenerValueByKey("Criticidad", dictionary)].ToString());

            return listaData;
        }

        private ApplianceCargaDto ObtenerEntidadAppliance(List<string> detalleFilaExcel, bool estado)
        {
            ApplianceCargaDto entidad = new ApplianceCargaDto();
            entidad.ValidarExcel = estado;
            bool FLAGCONTINUAR = true;
            DateTime DATETIME_OUT = DateTime.Now;
            int INT_OUT = 0;

            if (entidad.ValidarExcel)
            {
                for (int i = 0; i < detalleFilaExcel.Count; i++)
                {
                    if (FLAGCONTINUAR)
                    {
                        switch (i)
                        {
                            case 0: { entidad.Nombre = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 1: { entidad.TipoEquipo = ObtenerIdTipoEquipo(detalleFilaExcel[i].Trim()); if (entidad.TipoEquipo == 0) FLAGCONTINUAR = false; } break; //Ok
                            case 2: { entidad.Ambiente = ObtenerIdAmbiente(detalleFilaExcel[i].Trim()); } break; //Ok
                            case 3: { entidad.Dominio = ObtenerIdSubsidiaria(detalleFilaExcel[i].Trim()); } break; //Ok
                            case 4: { entidad.TipoActivo = ObtenerIdTipoActivo(detalleFilaExcel[i].Trim()); if (entidad.TipoActivo == 0) FLAGCONTINUAR = false; } break; //Ok
                            case 5: { entidad.Serial = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 6: { entidad.Modelo = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 7: { entidad.Vendor = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 8: { entidad.Tecnologia = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 9: { entidad.Version = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 10: { entidad.Hostname = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 11: { entidad.IP = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 12: { entidad.Dimension = ObtenerIdDimension(detalleFilaExcel[i].Trim()) == 0 ? (int?)null : ObtenerIdDimension(detalleFilaExcel[i].Trim()); } break; //Ok
                            case 13: { entidad.ArquitectoSeguridad = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 14: { entidad.SoportePrimerNivel = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 15: { entidad.Proveedor = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok

                            case 16: { entidad.FechaLanzamiento = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 17: { entidad.FechaFinSoporte = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 18: { entidad.FechaFinExtendida = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 19: { entidad.FechaFinInterna = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 20: { entidad.ComentariosFechaFin = detalleFilaExcel[i].Trim().Replace("'", "''"); } break; //Ok
                            case 21: { entidad.FechaCalculoId = ObtenerIdFechaCalculo(detalleFilaExcel[i].Trim()); if (entidad.FechaCalculoId == 0) FLAGCONTINUAR = false; } break;
                            case 22: { entidad.FechaAdquisicion = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 23: { entidad.FechaImplementacion = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 24: { entidad.FechaUltimaRenovacion = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 25: { entidad.VencimientoLicencia = DateTime.TryParse(detalleFilaExcel[i].Trim(), out DATETIME_OUT) ? DATETIME_OUT : (DateTime?)null; } break; //Ok
                            case 26: { entidad.CantidadLicencia = int.TryParse(detalleFilaExcel[i].Trim(), out INT_OUT) ? INT_OUT : (int?)null; } break;
                            case 27: { entidad.FormaLicenciamiento = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                            case 28: { entidad.CodigoInventario = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 29: { entidad.CyberSOC = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 30: { entidad.Sede = ObtenerIdSede(detalleFilaExcel[i].Trim()); if (entidad.Sede == 0) FLAGCONTINUAR = false; } break;
                            case 31: { entidad.Sala = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 32: { entidad.RACK = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 33: { entidad.Ubicacion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;

                            case 34: { entidad.Backup = ObtenerIdBackup(detalleFilaExcel[i].Trim()) != 0 ? ObtenerIdBackup(detalleFilaExcel[i].Trim()) : (int?)null; } break;
                            case 35: { entidad.BackupFrecuencia = ObtenerIdBackupFrecuencia(detalleFilaExcel[i].Trim()) != 0 ? ObtenerIdBackupFrecuencia(detalleFilaExcel[i].Trim()) : (int?)null; } break;
                            case 36: { entidad.BackupDescripcion = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 37: { entidad.IntegracionGestorInteligencia = ObtenerIdIntegracionGestor(detalleFilaExcel[i].Trim()); if (entidad.IntegracionGestorInteligencia == 0) FLAGCONTINUAR = false; } break;
                            case 38: { entidad.ConectorSIEM = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 39: { entidad.CONA = ObtenerIdCona(detalleFilaExcel[i].Trim().Replace("'", "''")); if (entidad.CONA == 0) FLAGCONTINUAR = false; } break;
                            case 40: { entidad.UmbralCPU = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 41: { entidad.UmbralMemoria = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 42: { entidad.UmbralMemoria = detalleFilaExcel[i].Trim().Replace("'", "''"); } break;
                            case 43: { entidad.Criticidad = ObtenerIdCriticidadEquipo(detalleFilaExcel[i].Trim()) != 0 ? ObtenerIdCriticidadEquipo(detalleFilaExcel[i].Trim()) : (int?)null; } break;
                        }
                    }
                }
            }
            else
                FLAGCONTINUAR = false;

            entidad.Activo = true;
            entidad.UsuarioCreacion = "auto";
            if (!FLAGCONTINUAR) entidad = null;

            return entidad;
        }

        private EstadoFila ValidarFilaAppliance(List<string> detalleFilaExcel)
        {
            EstadoFila flagValidar = new EstadoFila();
            flagValidar.Estado = true;

            int INT_OUT = 0;
            DateTime DATETIME_OUT = DateTime.Now;
            for (int i = 0; i < detalleFilaExcel.Count; i++)
            {
                if (flagValidar.Estado)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 11:
                        case 13:
                        case 14:
                        case 15: 
                        case 21:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 37:
                        case 38:
                        case 39: { flagValidar.Estado = !string.IsNullOrEmpty(detalleFilaExcel[i]); } break;
                        case 22:
                        case 24:
                        case 25: { flagValidar.Estado = DateTime.TryParse(detalleFilaExcel[i], out DATETIME_OUT); } break;
                        case 26: { flagValidar.Estado = int.TryParse(detalleFilaExcel[i], out INT_OUT); } break;
                    }
                }
            }

            return flagValidar;
        }

        private T ObtenerRegistroEntidad2<T>(List<string> detalleFilaExcel, int TipoCargaMasiva)
        {
            EstadoFila flagValidar = new EstadoFila();

            switch ((ETipoCargaMasiva)TipoCargaMasiva)
            {
                case ETipoCargaMasiva.Appliance:
                    flagValidar = ValidarFilaAppliance(detalleFilaExcel);
                    break;
            }

            T objEntidad = default(T);
            if (flagValidar.Estado)
            {
                switch ((ETipoCargaMasiva)TipoCargaMasiva)
                {
                    case ETipoCargaMasiva.Appliance:
                        objEntidad = (T)(object)ObtenerEntidadAppliance(detalleFilaExcel, true);
                        break;
                }
            }
            else
            {
                switch ((ETipoCargaMasiva)TipoCargaMasiva)
                {
                    case ETipoCargaMasiva.Appliance:
                        objEntidad = (T)(object)ObtenerEntidadAppliance(detalleFilaExcel, false);
                        break;
                }
            }
            return objEntidad;
        }

        private void CargaMasivaAppliance(string ext, Stream stream)
        {
            Dictionary<string, int> COLUMNAS_APLICACION = Utilitarios.CargarColumnasExcelAppliance();
            List<ApplianceCargaDto> ListObjRegistro = null;
            using (IExcelDataReader excelReader = ControlarExtension(ext, stream))
            {
                ListObjRegistro = new List<ApplianceCargaDto>();
                DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ApplianceCargaDto ObjRegistro = ObtenerRegistroEntidad2<ApplianceCargaDto>(DevolverColumnasAppliance(dr, COLUMNAS_APLICACION), (int)ETipoCargaMasiva.Appliance);
                    if (ObjRegistro != null)
                    {
                        ListObjRegistro.Add(ObjRegistro);
                    }
                }
                excelReader.Close();
            }
            //PreparaXMLEnviarApplianceSP(ListObjRegistro);
        }        
    }
}
