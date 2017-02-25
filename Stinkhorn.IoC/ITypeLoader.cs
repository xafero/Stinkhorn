using System;
using System.Collections.Generic;

namespace Stinkhorn.IoC
{
    public interface ITypeLoader : IDisposable
    {
        IEnumerable<IType> FindDerived(Type type);
    }
}