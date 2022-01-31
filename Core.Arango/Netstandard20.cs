using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Core.Arango
{
    /// <summary>
    /// Helper class to group all code that is used to facilitate backwards compatibility for dotnet Standard v2.0
    /// </summary>
    public static class Netstandard20Helper
    {
        /// <summary>
        /// for compatibility with netstandard 2.0 which does not have a Patch method
        /// </summary>
        internal static HttpMethod Patch => new HttpMethod("Patch");


        internal static List<string> RemoveEmptyEntries(this List<string> itemList)
        {
            return itemList.Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

    }
}