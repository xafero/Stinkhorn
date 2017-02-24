using System;
using System.Collections.Generic;

namespace Stinkhorn.IoC
{
    interface ITypeLoader : IDisposable
    {
        IEnumerable<IType> FindDerived(Type type);
    }
}