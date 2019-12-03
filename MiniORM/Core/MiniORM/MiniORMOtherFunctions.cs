using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
//Author : Shazaki Zetawars //
namespace MiniORM
{
    public partial class MiniORM : DB_Common
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
        public bool ExecuteQuery(string query, object Params = null)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                int rowseffected = cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
        }
        public bool ExecuteTransactionQuery(string query, object Params = null)
        {
            query = $"{QueryMaker.BeginTransQuery()} {query} {QueryMaker.CommitTransQuery()}";
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                int rowseffected = cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
        }
        #endregion

    }

}

