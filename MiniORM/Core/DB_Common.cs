using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//Author : Shazaki Zetawars //
namespace MiniORM
{
    public class DB_Common
    {
        protected static List<PropertyInfo> GetReadableProperties<T>()
        {
            return typeof(T).GetProperties().Where(x => !(Attribute.IsDefined(x, typeof(DontLoad)))).ToList();
        }
    }

}

