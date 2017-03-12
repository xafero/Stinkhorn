using Stinkhorn.API;
using Microsoft.Win32;

namespace Stinkhorn.Core
{
    class RegistryHandler //: //IMessageHandler<RegistryRequest, RegistryResponse>
    {
        public RegistryResponse Process(RegistryRequest input)
        {
            var root = (RegistryKey)typeof(Registry).GetField(input.Root + "").GetValue(null);
            var writable = input.Value != null;
            var path = input.Path.Trim();
            using (var key = root.OpenSubKey(path, writable))
            {
                var myKey = input.Key.Trim();
                var ret = new RegistryResponse { Value = key.GetValue(myKey) };
                if (writable)
                    key.SetValue(myKey, input.Value);
                return ret;
            }
        }
    }
}