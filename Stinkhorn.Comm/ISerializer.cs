using System;

namespace Stinkhorn.Comm
{
    public interface ISerializer : IDisposable
    {
        byte[] Serialize<T>(T instance);

        T Deserialize<T>(byte[] bytes);
    }
}