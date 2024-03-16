using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BCP.CVT.Cross
{
    public static class Settings
    {
        public static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) return default(T);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

        public static T ConvertirEntidad<T>(string data)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            JavaScriptSerializer jsserialize = new JavaScriptSerializer();
            var entidad = (T)JsonConvert.DeserializeObject(data, typeof(T));

            return entidad;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetSql(string file)
        {
            var directorio = Settings.Get<string>("CMDB.RutaConsultas");
            var rutaFinal = Path.Combine(directorio, file);

            return File.ReadAllText(rutaFinal);
        }
    }
}
