
namespace Stinkhorn.IoC
{
    interface IType
    {
        string Name { get; }

        T CreateInst<T>();
    }
}