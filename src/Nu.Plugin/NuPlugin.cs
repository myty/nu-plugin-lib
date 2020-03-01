using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public partial class NuPlugin
    {
        private readonly Stream _stdin;
        private readonly Stream _stdout;

        private Signature _signature = Signature.Create();

        private NuPlugin(Stream stdin, Stream stdout)
        {
            _stdout = stdout;
            _stdin = stdin;
        }

        public static NuPlugin Build(string name = null)
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            return new NuPlugin(stdin, stdout).Name(name);
        }

        public NuPlugin Name(string name)
        {
            _signature = _signature.WithName(name);
            return this;
        }

        public NuPlugin Description(string description)
        {
            _signature = _signature.WithDescription(description);
            return this;
        }

        public async Task SinkPluginAsync<TSinkPlugin>() where TSinkPlugin : INuPluginSink, new()
        {
            await CommandHandler<TSinkPlugin>((req, res) =>
            {
                if (req.Method == "config")
                {
                    res.Config(_signature);
                }
                else if (req.Method == "sink")
                {
                    var requestParams = req.GetParams<IEnumerable<JsonRpcParams>>();
                    res.Sink(requestParams);
                }
                else
                {
                    res.Quit();
                }
            });
        }

        public async Task FilterPluginAsync<TFilterPlugin>() where TFilterPlugin : INuPluginFilter, new()
        {
            await CommandHandler<TFilterPlugin>((req, res) =>
            {
                if (req.Method == "config")
                {
                    res.Config(_signature.WithIsFilter(true));
                }
                else if (req.Method == "begin_filter")
                {
                    res.BeginFilter();
                }
                else if (req.Method == "filter")
                {
                    res.Filter(req.GetParams<JsonRpcParams>());
                }
                else if (req.Method == "end_filter")
                {
                    res.EndFilter();
                }
                else
                {
                    res.Quit();
                }
            });
        }
    }
}
