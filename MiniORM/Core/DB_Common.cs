using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Zetawars.ORM
{
    public class DBCommon
    {
        protected static PropertyInfo GetKeyProperty<T>()
        {
            return typeof(T).GetProperties().FirstOrDefault(x => Attribute.IsDefined(x, typeof(PrimaryKey)));
        }

        protected static List<PropertyInfo> GetReadableProperties<T>()
        {
            return typeof(T).GetProperties().Where(x => !(Attribute.IsDefined(x, typeof(DontRead)))).ToList();
        }

        protected static string GetColumnName(PropertyInfo pi)
        {
            if (Attribute.IsDefined(pi, typeof(Column)))
            {
                Column k = (Column)Attribute.GetCustomAttribute(pi, typeof(Column));
                return $"[{k.Name}]";
            }
            else
            {
                return $"[{pi.Name}]";
            }
        }
    }
}

