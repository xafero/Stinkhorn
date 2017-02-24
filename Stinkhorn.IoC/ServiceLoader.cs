using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Stinkhorn.IoC
{
    static class ServiceLoader
    {
        public static ServiceLoader<T> Load<T>(ITypeLoader loader = null)
        {
            if (loader == null)
            {
                var asses = new[]
                {
                    Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly(),
                    Assembly.GetEntryAssembly(), Assembly.GetAssembly(typeof(T))
                };
                loader = new AssemblyLoader(asses);
            }
            return new ServiceLoader<T>(loader);
        }
    }

    class ServiceLoader<T> : IEnumerable<T>
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