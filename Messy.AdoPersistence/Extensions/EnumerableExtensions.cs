using System;
using System.Collections.Generic;
using System.Data;

namespace Messy.AdoPersistence.Extensions
{
    public static class EnumerableExtensions
    {
        public static DataTable MakeDataTable<TSource>(this IEnumerable<TSource> dataArray, string tableName = null)
        {
            var table = new DataTable(tableName);

            var type = dataArray.GetType().GetElementType();
            var properties = type.GetProperties();

            if (properties.Length > 0)
            {
                foreach (var p in properties)
                {
                    DataColumn column;

                    if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    {
                        column = new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType))
                        {
                            AllowDBNull = true
                        };
                    }
                    else
                    {
                        column = new DataColumn(p.Name, p.PropertyType);
                    }

                    table.Columns.Add(column);
                }

                foreach (var record in dataArray)
                {
                    var row = table.NewRow();

                    foreach (var p in properties)
                    {
                        var value = type.GetProperty(p.Name).GetValue(record, null);
                        row[p.Name] = value ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
            }
            else
            {
                var column = new DataColumn(tableName, type);
                table.Columns.Add(column);

                foreach (var record in dataArray)
                {
                    var row = table.NewRow();

                    row[tableName] = record;

                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}