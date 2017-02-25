using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Stinkhorn.IoC
{
    public static class ServiceLoader
    {
        public static ServiceLoader<T> Load<T>(ITypeLoader loader = null)
        {
            if (loader == null)
            {
                var asses = new[]
                {
                    Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly(),
                    Assembly.GetEntryAssembly(), Assembly.GetAssembly(typeof(T)),
                };
                var refs = asses.Where(a => a != null)
                    .SelectMany(a => a.GetReferencedAssemblies()).Distinct()
                    .Select(a => Assembly.Load(a)).Where(a => !a.GlobalAssemblyCache);
                loader = new AssemblyLoader(asses.Concat(refs));
            }
            return new ServiceLoader<T>(loader);
        }
    }

    public class ServiceLoader<T> : IEnumerable<T>
    {
        readonly ITypeLoader[] loaders;
        readonly Type baseType;

        internal ServiceLoader(params ITypeLoader[] loaders)
        {
            this.loaders = loaders;
            baseType = typeof(T);
        }

        IEnumerable<T> GetLoader()
        {
            foreach (var loader in loaders)
                foreach (var iType in loader.FindDerived(baseType))
                    yield return iType.CreateInst<T>();
        }

        public IEnumerator<T> GetEnumerator() => GetLoader().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetLoader().GetEnumerator();
    }
}