using Stinkhorn.API;
using Microsoft.Win32;

namespace Stinkhorn.Core
{
    class RegistryHandler : IMessageHandler<RegistryRequest, RegistryResponse>
    {
        public RegistryResponse Process(RegistryRequest input)
        {
            var root = (RegistryKey)typeof(Registry).GetField(input.Root + "").GetValue(null);
            var writable = input.Value != null;
            using (var key = root.OpenSubKey(input.Path, writable))
            {
                var ret = new RegistryResponse { Value = key.GetValue(input.Key) };
                if (writable)
                    key.SetValue(input.Key, input.Value);
                return ret;
            }
        }
    }
}