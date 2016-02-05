using System.Data;
using System.Data.SqlClient;

namespace Messy.AdoPersistence.Extensions
{
    public static class SqlParameterCollectionExtensions
    {
        public static SqlParameter AddTypedWithValue(this SqlParameterCollection collection, string name, SqlDbType type, object value)
        {
            var parameter = new SqlParameter(name, type) {Value = value};

            collection.Add(parameter);

            return parameter;
        }

        public static SqlParameter AddTableValue(this SqlParameterCollection collection, string name, string typeName, object value)
        {
            var parameter = new SqlParameter(name, SqlDbType.Structured) {Value = value, TypeName = typeName};

            collection.Add(parameter);

            return parameter;
        }
    }
}