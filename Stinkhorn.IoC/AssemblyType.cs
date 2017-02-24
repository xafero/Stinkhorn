using System;

namespace Stinkhorn.IoC
{
    class AssemblyType : IType
    {
        readonly Type type;

        public AssemblyType(Type type)
        {
            this.type = type;
        }

        public string Name => type.Name;

        public T CreateInst<T>() => (T)Activator.CreateInstance(type);
    }
}