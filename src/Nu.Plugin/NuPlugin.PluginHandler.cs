using System;
using System.IO;
using System.Threading.Tasks;

namespace Nu.Plugin
{
    public partial class NuPlugin
    {
        private async Task CommandHandler<TPluginType>(
            Action<JsonRpcRequest, PluginResponse<TPluginType>> pluginRes
        ) where TPluginType : new()
        {
            using (var handler = PluginHandler<TPluginType>.Create(_stdin, _stdout))
                while (await handler.HandleNextRequestAsync(pluginRes)) ;
        }

        internal class PluginHandler<TPluginType> : IDisposable
             where TPluginType : new()
        {
            private readonly StreamReader _stdinReader;
            private readonly StreamWriter _stdoutWriter;
            private readonly TPluginType _plugin;

            private PluginHandler(Stream stdin, Stream stdout)
            {
                _stdinReader = new StreamReader(stdin, Console.InputEncoding);
                _stdoutWriter = new StreamWriter(stdout, Console.OutputEncoding) { AutoFlush = true };

                _plugin = new TPluginType();
            }

            public static PluginHandler<TPluginType> Create(Stream stdin, Stream stdout)
            {
                var handler = new PluginHandler<TPluginType>(stdin, stdout);

                return handler;
            }

            internal async Task<bool> HandleNextRequestAsync(
                Action<JsonRpcRequest, PluginResponse<TPluginType>> pluginRes
            )
            {
                var json = await _stdinReader.ReadLineAsync();

                if (!string.IsNullOrEmpty(json?.Trim()))
                {
                    var request = new JsonRpcRequest(json);
                    if (request?.IsValid == true)
                    {
                        var responseHandler = new PluginResponse<TPluginType>(_stdoutWriter, _plugin);

                        pluginRes(request, responseHandler);

                        return await responseHandler.RespondAsync();
                    }
                }

                return false;
            }

            public void Dispose()
            {
                _stdinReader?.Dispose();
                _stdoutWriter?.Dispose();
            }
        }
    }
}
