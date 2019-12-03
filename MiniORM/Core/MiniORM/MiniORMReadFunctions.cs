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
        #region ReadFunctions
        public List<Dictionary<string, string>> QueryList(string query, object Params = null)
        {
            List<Dictionary<string, string>> appList = new List<Dictionary<string, string>>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> app = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            app.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        appList.Add(app);
                    }
                }
                Connection.Close();
            }
            return appList;
        }
        public PageListObject<T> GetPagedList<T>(string query, string sortBy, string order, int numberOfRecords, int pageNumber, Dictionary<string, string> Params)
        {
            int offset = numberOfRecords * pageNumber;
            var Querries = QueryMaker.GetPagerQueries<T>(query, sortBy, order, offset, numberOfRecords);
            string mainquery = Querries.Query;
            string countquery = Querries.CountQuery;
            return PagedResults<T>(numberOfRecords, pageNumber, Params, offset, mainquery, countquery);
        }
        public PageListObject<T> GetPagedList<T>(string query, Dictionary<string, string> sortAndOrder, int numberOfRecords, int pageNumber, Dictionary<string, string> Params)
        {
            int offset = numberOfRecords * pageNumber;
            var Querries = QueryMaker.GetPagerQueries<T>(query, sortAndOrder, offset, numberOfRecords);
            string mainquery = Querries.Query;
            string countquery = Querries.CountQuery;
            return PagedResults<T>(numberOfRecords, pageNumber, Params, offset, mainquery, countquery);
        }
        private PageListObject<T> PagedResults<T>(int numberOfRecords, int pageNumber, object Params, int offset, string mainquery, string countquery)
        {

            int count = 0;
            List<T> results = new List<T>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(mainquery, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                var properties = GetReadableProperties<T>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var item = Activator.CreateInstance<T>();
                        foreach (var property in properties)
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                DBReader(item, property, reader);
                            }
                        }
                        results.Add(item);
                    }
                }
                reader.Close();
                cmd = GetSqlCommandWithParams(countquery, Connection, Params);
                count = (int)cmd.ExecuteScalar();
                Connection.Close();
                Pager pager = GetPagerSettings(numberOfRecords, pageNumber, count, offset);
                return new PageListObject<T> { pager = pager, Results = results };
            }
        }
        private Pager GetPagerSettings(int numberOfRecords, int pageNumber, int count, int offset)
        {
            Pager pager = new Pager();
            pager.RecordsPerPage = numberOfRecords;
            pager.Offset = offset;
            pager.Fetch = numberOfRecords;
            pager.TotalRecords = count;
            pager.PageNumber = pageNumber;
            double d = pager.TotalRecords / pager.RecordsPerPage;
            pager.TotalPages = Convert.ToInt32(Math.Ceiling(d));
            return pager;
        }


        public Dictionary<string, string> QueryRow(string query, object Params = null)
        {
            Dictionary<string, string> appList = new Dictionary<string, string>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            appList.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        break;
                    }
                }
                Connection.Close();
            }
            return appList;
        }
        public List<string> QueryColumn(string query, object Params)
        {
            List<string> list = new List<string>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(reader[reader.GetName(0)].ToString());
                    }
                }
                Connection.Close();
            }
            return list;
        }
        public T Get<T>(string query = null, string whereClause = null, object Params = null)
        {
            T t = Activator.CreateInstance<T>();
            List<PropertyInfo> properties = GetReadableProperties<T>();
            query = QueryMaker.SelectQuery<T>(query, whereClause);
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        foreach (var property in properties)
                        {
                            DBReader(t, property, reader);
                        }
                    }
                }
                Connection.Close();
            }
            return t;
        }
        public bool Get<T>(T _Object, string query = "", string where_clause = "", object Params = null)
        {
            List<PropertyInfo> _properties = GetReadableProperties<T>();
            query = QueryMaker.SelectQuery<T>(query, where_clause);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        foreach (PropertyInfo pi in _properties)
                        {
                            DBReader(_Object, pi, reader);
                        }
                    }
                }
                connection.Close();
                return true;
            }
        }
        public List<T> GetList<T>(string query = null, string whereClause = null, object Params = null)
        {
            var results = new List<T>();
            var properties = GetReadableProperties<T>();
            query = QueryMaker.SelectQuery<T>(query, whereClause);
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var item = Activator.CreateInstance<T>();
                        foreach (var property in properties)
                        {
                            DBReader(item, property, reader);
                        }
                        results.Add(item);
                    }
                }
                Connection.Close();
                return results;
            }
        }
        public DataTable ReadDataTable(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Connection = new SqlConnection())
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }

        }
        #endregion
    }

}

