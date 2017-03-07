namespace Stinkhorn.Util
{
    public class SystemInfoFactory : ISystemInfoFactory
    {
        public ISystemInfo GetSystemInfo() => new SystemInfo();
    }
}