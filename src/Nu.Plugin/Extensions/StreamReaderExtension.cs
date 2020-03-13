using System.IO;
using System.Threading.Tasks;

namespace Nu.Plugin
{
    internal static class StreamReaderExtension
    {
        internal static async Task<JsonRpcRequest> GetNextRequestAsync(this StreamReader reader)
        {
            var json = await reader.ReadLineAsync();

            return !string.IsNullOrEmpty(json?.Trim()) ? new JsonRpcRequest(json) : null;
        }
    }
}
