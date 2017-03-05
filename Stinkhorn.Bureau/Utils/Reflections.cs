using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stinkhorn.Bureau.Utils
{
    public static class Reflections
    {
        const char slash = '/';

        public static IDictionary<string, Type> GetTypes(object obj)
            => GetTypes(obj.GetType()).ToDictionary(k => k.Key, v => v.Value);

        static IEnumerable<KeyValuePair<string, Type>> GetTypes(Type type, string prefix = "")
            => new Dictionary<string, Type> { { prefix, type } }
            .Concat(type.GetProperties().SelectMany(p =>
                GetTypes(p.PropertyType, prefix + slash + p.Name)));

        public static IEnumerable<T> Extract<T>(object obj, string path)
        {
            var parts = path.Split(slash);
            object current = null;
            var phase = string.Empty;
            foreach (var part in parts)
            {
                if (part == string.Empty)
                {
                    current = obj;
                    continue;
                }
                phase += slash + part;
                var prop = current.GetType().GetProperty(part);
                var idx = prop.GetIndexParameters();
                if (idx.Length >= 1)
                {
                    var i = 0;
                    object item = null;
                    while ((item = prop.GetIndexerValue(current, i++)) != null)
                        foreach (var sub in Extract<T>(item, path.Substring(phase.Length)))
                            yield return sub;
                    yield break;
                }
                current = prop.GetValue(current);
            }
            yield return (T)current;
        }

        static object GetIndexerValue(this PropertyInfo prop, object obj, int index)
        {
            try
            {
                return prop.GetValue(obj, new object[] { index });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}