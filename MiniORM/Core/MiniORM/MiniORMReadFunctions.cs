using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection;
using Microsoft.CSharp;
namespace Zetawars.ORM
{
    public partial class MiniORM : DBCommon
    {
        #region ReadFunctions
        
        public PageListObject<T> GetPagedList<T>(string query, string sortBy, string order, int numberOfRecords, int pageNumber, object Params)
        {
            int offset = numberOfRecords * pageNumber;
            var Querries = QueryMaker.GetPagerQueries<T>(query, sortBy, order, offset, numberOfRecords);
            string mainquery = Querries.Query;
            string countquery = Querries.CountQuery;
            return PagedResults<T>(numberOfRecords, pageNumber, Params, offset, mainquery, countquery);
        }
        public PageListObject<T> GetPagedList<T>(string query, Dictionary<string, string> sortAndOrder, int numberOfRecords, int pageNumber, object Params)
        {
            int offset = numberOfRecords * pageNumber;
            var Querries = QueryMaker.GetPagerQueries<T>(query, sortAndOrder, offset, numberOfRecords);
            string mainquery = Querries.Query;
            string countquery = Querries.CountQuery;
            return PagedResults<T>(numberOfRecords, pageNumber, Params, offset, mainquery, countquery);
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


        public dynamic Get(string query = null, string whereClause = null, object Params = null)
        {
            dynamic d = new ExpandoObject();
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
                            var propName = reader.GetName(i);
                            AddProperty(d, propName, ConvertTo(reader.GetFieldType(i), reader[propName]));
                        }
                    }
                }

                Connection.Close();
            }
            return d;
        }

        public List<T> GetAll<T>(string query = null, string whereClause = null, object Params = null)
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
        public List<dynamic> GetAll(string query, object Params = null)
        {
            var ResultList = new List<dynamic>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                SqlCommand cmd = GetSqlCommandWithParams(query, Connection, Params);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var Result = new ExpandoObject();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var propName = reader.GetName(i);
                            AddProperty(Result, propName, ConvertTo(reader.GetFieldType(i), reader[propName]));
                           // AddProperty(Result, reader.GetName(i), reader[reader.GetName(i)].ToString());
                        }
                        ResultList.Add(Result);
                    }
                }
                Connection.Close();
            }
            return ResultList;
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


        #region private functions
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

        private static void AddProperty(ExpandoObject dynamicObject, string propertyName, object propertyValue)
        {
            //Take use of the IDictionary implementation
            var expandoDict = dynamicObject as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        #endregion
    }

}

