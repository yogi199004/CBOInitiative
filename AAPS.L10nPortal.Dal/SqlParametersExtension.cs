using AAPS.L10nPortal.Entities.Attributes;

namespace AAPS.L10nPortal.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    namespace AAPS.L10nPortal.Dal.Extensions
    {
        public static class SqlParametersExtension
        {
            public static void AddWithUdtValue<T>(this SqlParameterCollection parameters, string parameterName, IEnumerable<T> list) where T : new()
            {
                if (list == null)
                {
                    return;
                }

                if (typeof(T).IsEnum)
                {
                    AddWithUdtValue(parameters, parameterName, list.Cast<int>().ToList());
                    return;
                }

                AddWithUdtMultiColumnValue(parameters, parameterName, list);
            }

            public static void AddWithUdtValue(this SqlParameterCollection parameters, string parameterName, IEnumerable<bool> list)
            {
                AddWithUdtValue(parameters, parameterName, list?.Select(v => v ? 1 : 0));
            }

            public static void AddWithUdtValue(this SqlParameterCollection parameters, string parameterName, IEnumerable<int> list)
            {
                AddWithUdtSingleColumnValue(parameters, parameterName, list);
            }

            public static void AddWithUdtValue(this SqlParameterCollection parameters, string parameterName, IEnumerable<string> list)
            {
                AddWithUdtSingleColumnValue(parameters, parameterName, list);
            }

            public static void AddWithUdtValue(this SqlParameterCollection parameters, string parameterName, IEnumerable<Guid> list)
            {
                AddWithUdtSingleColumnValue(parameters, parameterName, list);
            }

            private static void AddWithUdtSingleColumnValue<T>(SqlParameterCollection parameters, string parameterName, IEnumerable<T> list)
            {
                if (list == null)
                {
                    return;
                }

                if (parameters == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(parameterName))
                {
                    throw new ArgumentNullException(nameof(parameterName));
                }

                var dataTable = new DataTable();

                dataTable.Columns.Add(new DataColumn("value") { DataType = typeof(T) });

                foreach (var value in list)
                {
                    dataTable.Rows.Add(value);
                }

                parameters.Add(new SqlParameter(parameterName, SqlDbType.Structured) { Value = dataTable });
            }

            private static void AddWithUdtMultiColumnValue<T>(SqlParameterCollection parameters, string parameterName, IEnumerable<T> list) where T : new()
            {
                if (parameters == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(parameterName))
                {
                    throw new ArgumentNullException(nameof(parameterName));
                }

                var properties =
                    typeof(T).GetProperties()
                        .Where(p => p.GetCustomAttribute<SqlColumn>() != null)
                        .Select(p => new XmlProperty { PropertyInfo = p, SqlColumn = p.GetCustomAttribute<SqlColumn>() })
                        .ToDictionary(p => p.SqlColumn.Order, p => p);

                if (!properties.Any())
                {
                    throw new Exception("Not found properties for serialization");
                }

                var columnsCount = properties.Count;
                var dataTable = new DataTable();

                for (var i = 1; i <= columnsCount; i++)
                {
                    dataTable.Columns.Add(new DataColumn(properties[i].PropertyInfo.Name));
                }

                foreach (var element in list)
                {
                    var row = new object[columnsCount];

                    for (var i = 1; i <= columnsCount; i++)
                    {
                        var property = properties[i];

                        var value = property.PropertyInfo.GetValue(element);

                        row[i - 1] = value == null ? null : property.SqlColumn.GetSqlValue(value);
                    }

                    dataTable.Rows.Add(row);
                }

                parameters.Add(new SqlParameter(parameterName, SqlDbType.Structured) { Value = dataTable });
            }
        }
    }
}
