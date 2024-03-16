using System;
using System.Data;

namespace BCP.CVT.Cross
{
    public static class DataReaderExtensions
    {
        public static Int16 GetValueInt16(this IDataReader reader, int ordinal)
        {
            Int16 value = 0;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetInt16(ordinal);
            }

            return value;
        }
        public static int GetValueInt32(this IDataReader reader, int ordinal)
        {
            int value = 0;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetInt32(ordinal);
            }

            return value;
        }
        public static int? GetValueNullInt32(this IDataReader reader, int ordinal)
        {
            int? value = null;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetInt32(ordinal);
            }

            return value;
        }
        public static bool? GetValueNullBoolean(this IDataReader reader, int ordinal)
        {
            bool? value = null;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetBoolean(ordinal);
            }

            return value;
        }
        public static decimal GetValueDecimal(this IDataReader reader, int ordinal)
        {
            decimal value = 0;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDecimal(ordinal);
            }

            return value;
        }
        public static double GetValueDouble(this IDataReader reader, int ordinal)
        {
            double value = 0;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDouble(ordinal);
            }

            return value;
        }
        public static double? GetValueNullDouble(this IDataReader reader, int ordinal)
        {
            double? value = null;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDouble(ordinal);
            }
            return value;
        }
        public static decimal? GetValueNullDecimal(this IDataReader reader, int ordinal)
        {
            decimal? value = null;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDecimal(ordinal);
            }

            return value;
        }
        public static string GetValueString(this IDataReader reader, int ordinal)
        {
            string value = string.Empty;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetString(ordinal).Trim();
            }

            return value;
        }
        public static DateTime GetValueDateTime(this IDataReader reader, int ordinal)
        {
            DateTime value = DateTime.MinValue;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDateTime(ordinal);
            }

            return value;
        }
        public static DateTime? GetValueNullDateTime(this IDataReader reader, int ordinal)
        {
            DateTime? value = null;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDateTime(ordinal);
            }

            return value;
        }        
        public static Boolean GetValueBoolean(this IDataReader reader, int ordinal)
        {
            bool value = false;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetBoolean(ordinal);
            }

            return value;
        }
        public static Guid GetValueGuid(this IDataReader reader, int ordinal)
        {
            Guid value = default(Guid);

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetGuid(ordinal);
            }

            return value;
        }
        public static byte GetValueByte(this IDataReader reader, int ordinal)
        {
            byte value = 0;

            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetByte(ordinal);
            }

            return value;
        }
        public static T GetData<T>(this IDataReader dr, string field)
        {
            T value = default(T);

            if (!DBNull.Value.Equals(dr[field])) value = (T)dr[field];

            return value;
        }
    }
}
