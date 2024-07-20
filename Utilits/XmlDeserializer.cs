using System;
using System.Drawing.Text;
using System.Reflection;
using System.Xml;
using HRApp.Models;
using NPOI.Util.ArrayExtensions;
namespace HRApp.Utilits
{
    public static class XmlDeserializer
    {
        public static List<T> Deserialize<T>(string URLString) where T : new()
        {
            XmlTextReader reader = new XmlTextReader (URLString);

            List<T> xmlModels = [];
            T model = new();

            string propName = string.Empty;
            bool isInProperty = false; 

            while (reader.Read())
            {          
                if(reader.NodeType == XmlNodeType.Element && IsHaveProperty(reader.Name))
                {
                    isInProperty = true;
                    propName = reader.Name;
                }
                if(reader.NodeType == XmlNodeType.Text && isInProperty)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(propName);
                    if(propertyInfo.GetValue(model).ToString() != string.Empty)
                    {
                        xmlModels.Add(model);
                        model = new();
                    }
                    propertyInfo.SetValue(model, Convert.ChangeType(reader.Value, propertyInfo.PropertyType));

                    isInProperty = false;
                    propName = string.Empty;
                }
            }
            return xmlModels;
        }
        private static bool IsHaveProperty(string propartyName)
        {
            var propertyNames = typeof(XmlModel).GetProperties()
                                                .Select(prop => prop.Name);

            foreach (var name in propertyNames)
            {
                if(name == propartyName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}