using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using FOB_Sales_API.Models.Emails;

namespace FOB_Sales_API.Models.Reflection
{
    public class clsReflection
    {
        PropertyInfo[] propertyInfos;
        public List<string> column_names = new List<string>();

        public clsReflection(Type class_name)
        {
            object ob = Activator.CreateInstance(class_name);
            propertyInfos = ob.GetType().GetProperties();
            column_names.Clear();
           // propertyInfos = typeof(clsEmailTags).GetProperties(BindingFlags.Public);

            Array.Sort(propertyInfos, delegate (PropertyInfo prop1, PropertyInfo prop2) {
                return prop1.Name.CompareTo(prop2.Name);
            });
            
            foreach (PropertyInfo propertyinfo in propertyInfos)
            {
                column_names.Add(propertyinfo.Name);
            }
        }
        //public void LoadColumns()
        //{
        //    Array.Sort(propertyInfos,delegate(PropertyInfo prop1, PropertyInfo prop2) {
        //      return prop1.Name.CompareTo(prop2.Name);
        //    });
        //    column_names.Clear();
            
        //    propertyInfos = typeof(clsEmailTags).GetProperties(BindingFlags.Public);
        //    foreach (PropertyInfo propertyinfo in propertyInfos)
        //    {
        //        column_names.Add(propertyinfo.Name);
        //    }
        //}

    }
}