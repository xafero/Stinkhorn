
namespace Stinkhorn.IoC
{
    public interface IType
    {
        string Name { get; }

        T CreateInst<T>();
    }
}