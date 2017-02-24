using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Stinkhorn.IoC
{
    class AssemblyLoader : ITypeLoader
    {
        readonly SortedSet<Assembly> asses;

        public AssemblyLoader(IEnumerable<Assembly> asses = null)
        {
            var args = asses?.Where(a => a != null) ?? new Assembly[0];
            this.asses = new SortedSet<Assembly>(args);
        }

        public IEnumerable<IType> FindDerived(Type type)
        {
            foreach (var ass in asses)
                foreach (var found in ass.GetTypes())
                    if (type.IsAssignableFrom(found))
                        yield return new AssemblyType(type);
        }

        public void AddAssembly(params Assembly[] asses)
        {
            foreach (var ass in asses.Where(a => a != null))
                this.asses.Add(ass);
        }

        public void Dispose()
        {
            asses.Clear();
        }
    }
}