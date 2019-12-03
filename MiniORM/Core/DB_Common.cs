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
    }
}

