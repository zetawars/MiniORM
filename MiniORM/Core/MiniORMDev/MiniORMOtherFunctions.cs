using System;
using System.Data.SqlClient;
namespace Zetawars.ORM
{
    public partial class MiniORMDev : DBCommon
    {
        #region OtherFunctions
        public int GetScaler(string query, object Params = null)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Connection.Close();
                return count;
            }
        }
        public double GetDouble(string query, object Params = null)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                double count = Convert.ToDouble(cmd.ExecuteScalar());
                Connection.Close();
                return count;
            }
        }
        public void ExecuteQuery(string query, object Params = null)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                int rowseffected = cmd.ExecuteNonQuery();
                Connection.Close();
            }
        }
        public void ExecuteTransactionQuery(string query, object Params = null)
        {
            query = $"{QueryMaker.BeginTransQuery} {query} {QueryMaker.CommitTransQuery}";
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                int rowseffected = cmd.ExecuteNonQuery();
                Connection.Close();
            }
        }
        #endregion
    }
}

