using System.Collections.Specialized;
using System.IO;

namespace Stinkhorn.Util
{
    public static class DictExtensions
    {
        public static string TryGetPath(this NameValueCollection dict, string key, string defVal = null)
        {
            var value = dict[key];
            return string.IsNullOrWhiteSpace(value) ? defVal : Path.GetFullPath(value);
        }
    }
}