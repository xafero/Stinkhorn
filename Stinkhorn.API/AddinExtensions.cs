using Mono.Addins;
using System;
using System.Collections.Generic;

namespace Stinkhorn.API
{
    public static class AddinExtensions
    {
        public static IEnumerable<KeyValuePair<A, T>> GetFiltered<A, T>()
            where A : CustomExtensionAttribute where T : class
        {
            var type = typeof(T);
            foreach (TypeExtensionNode node in AddinManager.GetExtensionNodes(type))
            {
                var attr = (node as TypeExtensionNode<A>)?.Data;
                if (!((attr as IFiltered)?.IsSuitable() ?? true))
                    continue;
                yield return new KeyValuePair<A, T>(attr, (T)node.CreateInstance(type));
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