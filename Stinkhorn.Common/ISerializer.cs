using System;

namespace Stinkhorn.Common
{
    public interface ISerializer : IDisposable
    {
        byte[] Serialize<T>(T instance);

        T Deserialize<T>(byte[] bytes);
    }
}