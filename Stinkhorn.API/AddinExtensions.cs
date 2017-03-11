using Mono.Addins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stinkhorn.API
{
    public static class AddinExtensions
    {
        public static IEnumerable<T> GetFiltered<T>() => GetFiltered(typeof(T)).Cast<T>();

        public static IEnumerable<object> GetFiltered(Type type)
        {
            foreach (var raw in AddinManager.GetExtensionNodes(type))
            {
                var node = raw as TypeExtensionNode<ReqHandlerFilterAttribute>;
                if (node != null)
                {
                    var attr = node.Data;
                    if (!attr.IsSuitable())
                        continue;
                }
                yield return node.CreateInstance(type);
            }
        }

        public static bool Compare(object filter, object value)
        {
            var cmp = StringComparison.InvariantCultureIgnoreCase;
            var filterStr = filter + string.Empty;
            var valueStr = value + string.Empty;
            if (valueStr.Equals(filterStr, cmp))
                return true;
            var likeBeg = filterStr.StartsWith("%", cmp);
            var likeEnd = filterStr.EndsWith("%", cmp);
            filterStr = filterStr.Replace("%", string.Empty);
            if (likeBeg && likeEnd)
                return valueStr.Contains(filterStr);
            if (likeEnd)
                return valueStr.StartsWith(filterStr, cmp);
            if (likeBeg)
                return valueStr.EndsWith(filterStr, cmp);
            return filter == null;
        }
    }
}