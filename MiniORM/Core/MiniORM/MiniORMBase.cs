﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace Zetawars.ORM
{
    public partial class MiniORM : DBCommon
    {
        private string ConnectionString { get; set; }

        public MiniORM(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        #region Private methods   
        private SqlCommand GetSqlCommandWithParams(string query, SqlConnection Connection, object Params)
        {
            SqlCommand cmd = new SqlCommand(query, Connection);
            if (Params != null)
            {
                foreach (var element in Params.GetType().GetProperties())
                {
                    cmd.Parameters.AddWithValue(element.Name, element.GetValue(Params));
                }
            }
            return cmd;
        }
        private void DBReader(object _Object, PropertyInfo property, SqlDataReader reader)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
            {
                Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(_Object, Convert.ChangeType(reader[property.Name], convertTo), null);
            }
            else
            {
                property.SetValue(_Object, null);
            }
        }
        private object ConvertTo(Type convertTo, object value)
        {
            convertTo = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
            return Convert.ChangeType(value, convertTo);
        }
        #endregion Private methods
    }

}

