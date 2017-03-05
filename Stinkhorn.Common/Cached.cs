using System.Collections.Generic;
using System.Reflection;

namespace Stinkhorn.Common
{
    public class Cached<I, O>
    {
        protected static O CreateObject(I input)
            => (O)typeof(O).GetConstructor(BindingFlags.NonPublic
                | BindingFlags.Instance, null, new[] { typeof(I) }, null)
            .Invoke(new object[] { input });

        protected static IDictionary<I, O> cached = new Dictionary<I, O>();

        protected static O GetOrCreate(I id)
        {
            O adr;
            if (cached.TryGetValue(id, out adr))
                return adr;
            return (cached[id] = CreateObject(id));
        }
    }
}