
namespace Stinkhorn.API
{
    public class RegistryRequest : IRequest
    {
        public RegistryRoot Root { get; set; }
        public string Path { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class RegistryResponse : IResponse
    {
        public object Value { get; set; }
    }

    public enum RegistryRoot
    {
        CurrentUser,
        LocalMachine,
        ClassesRoot,
        Users,
        PerformanceData,
        CurrentConfig,
        DynData
    }
}