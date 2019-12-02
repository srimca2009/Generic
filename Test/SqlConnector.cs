using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Test
{
    public static class SqlConnector
    {
        public static SqlParameter AddSQLParameter(string name, object value, DbType dbType)
        {
            return AddSQLParameter(name, 0, value, dbType, ParameterDirection.Input);
        }

        public static SqlParameter AddSQLParameter(string name, int size, object value, DbType dbType)
        {
            return AddSQLParameter(name, size, value, dbType, ParameterDirection.Input);
        }

        public static SqlParameter AddSQLParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }

        public static List<T> GetAll<T>(string connection, string sqlText, CommandType commandType, List<SqlParameter> parameters)
        {
            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    using (var command = new SqlCommand(sqlText, conn))
                    {
                        command.CommandType = commandType;
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        var dataset = new DataSet();

                        var dataAdaper = new SqlDataAdapter(command);

                        dataAdaper.Fill(dataset);

                        var list = dataset.Tables[0].ToList<T>();

                        return list;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private static List<T> ToList<T>(this DataTable dt)
        {
            var fields = typeof(T).GetProperties();

            List<T> list = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];

                            // Set the value into the object
                            fieldInfo.SetValue(ob, value);
                            break;
                        }
                    }
                }
                list.Add(ob);
            }
            return list;
        }

    }
}
