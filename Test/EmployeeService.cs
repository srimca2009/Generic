using Hiro.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Test
{
    public class EmployeeService
    {
        public string _connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();

        public EmployeeService()
        {
        }

        public List<Employee> GetAll()
        {
            var sql = StoredProcedure.Employee.GetAll;
            var result = SqlConnector.GetAll<Employee>(_connectionString, sql, System.Data.CommandType.StoredProcedure, null);

            //var parameters = new List<SqlParameter>();
            //parameters.Add(SqlConnector.AddSQLParameter("@FirstName", 50, "Test", DbType.String));
            //var list = SqlConnector.GetAll<Employee>(_connectionString, sql, System.Data.CommandType.StoredProcedure, parameters);

            return result;
        }

        public List<Employee> GetDataTable()
        {
            List<Employee> result = new List<Employee>();
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string sql = null;
            sql = StoredProcedure.Employee.GetAll;

            sqlCnn = new SqlConnection(_connectionString);
            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(sql, sqlCnn);
                adapter.SelectCommand = sqlCmd;
                adapter.Fill(ds);
                adapter.Dispose();
                sqlCmd.Dispose();
                sqlCnn.Close();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var row = ds.Tables[0].Rows[i];
                    Employee emp = new Employee();
                    emp.Id = (int)row["Id"];
                    emp.FirstName = row["FirstName"].ToString();
                    emp.LastName = row["LastName"].ToString();
                    emp.Address = row["Address"].ToString();
                    emp.IsActive =(bool)row["IsActive"];
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
