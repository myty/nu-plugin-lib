using System;
using System.IO;
using System.Threading.Tasks;

namespace Nu.Plugin
{
    internal class PluginHandler<TPluginType> : IDisposable
        where TPluginType : new()
    {
        private readonly TPluginType  _plugin;
        private readonly StreamReader _stdinReader;
        private readonly StreamWriter _stdoutWriter;

        private PluginHandler(Stream stdin, Stream stdout)
        {
            _plugin       = new TPluginType();
            _stdinReader  = new StreamReader(stdin, Console.InputEncoding);
            _stdoutWriter = new StreamWriter(stdout, Console.OutputEncoding) {AutoFlush = true};
        }

        public static PluginHandler<TPluginType> Create(Stream stdin, Stream stdout)
            => new PluginHandler<TPluginType>(stdin, stdout);

        internal async Task<bool> HandleNextRequestAsync(
            Action<JsonRpcRequest, PluginResponse<TPluginType>> pluginRes
        )
        {
            var json = await _stdinReader.ReadLineAsync();

            if (string.IsNullOrEmpty(json?.Trim())) return false;

            var request = new JsonRpcRequest(json);

            if (request.IsValid != true) return false;

            var responseHandler = new PluginResponse<TPluginType>(_stdoutWriter, _plugin);

            pluginRes(request, responseHandler);

            return await responseHandler.RespondAsync();
        }

        public void Dispose()
        {
            _stdinReader?.Dispose();
            _stdoutWriter?.Dispose();
        }
    }
}
