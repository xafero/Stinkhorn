using System;
using Mono.Addins;
using System.Globalization;
using static Stinkhorn.API.AddinExtensions;
using System.Linq;

namespace Stinkhorn.API
{
    public interface IFiltered
    {
        bool IsSuitable();
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ReqHandlerFilterAttribute : CustomExtensionAttribute, IFiltered
    {
        [NodeAttribute]
        public string Platform { get; set; }
        [NodeAttribute]
        public string Version { get; set; }
        [NodeAttribute]
        public string Arch { get; set; }
        [NodeAttribute]
        public string Locale { get; set; }
        [NodeAttribute]
        public string HasVar { get; set; }

        public bool IsSuitable()
        {
            var plat = Environment.OSVersion.Platform;
            var ver = Environment.OSVersion.Version;
            var arch = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            var loc = CultureInfo.InstalledUICulture;
            var keys = Environment.GetEnvironmentVariables().Keys.OfType<string>();
            var vars = HasVar?.Split(' ');
            return Compare(Platform, plat) && Compare(Version, ver)
                && Compare(Arch, arch) && Compare(Locale, loc)
                && (string.IsNullOrWhiteSpace(HasVar)
                    || vars.Any(v => keys.Any(k => Compare(v, k))));
        }
    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(ReqHandlerFilterAttribute))]
    public interface IRequestHandler
    {

    }

    public interface IRequestHandler<I> : IRequestHandler
        where I : IRequest
    {
        IResponse Process(I input);
    }
}