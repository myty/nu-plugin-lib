using System.IO;
using System.Threading.Tasks;

namespace Nu.Plugin
{
    internal static class StreamReaderExtension
    {
        internal static async Task<JsonRpcRequest> GetNextRequestAsync(this StreamReader reader)
        {
            var json = await reader.ReadLineAsync();

            if (!string.IsNullOrEmpty(json?.Trim()))
            {
                return new JsonRpcRequest(json);
            }

            return null;
        }
    }
}