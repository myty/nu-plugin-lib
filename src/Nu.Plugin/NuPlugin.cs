using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public class NuPlugin
    {
        private readonly Stream _stdin;
        private readonly Stream _stdout;
        private Signature _signature = Signature.Create();

        private NuPlugin(Stream stdin, Stream stdout, string name)
        {
            _stdout = stdout;
            _stdin = stdin;
            _signature = _signature.WithName(name);
        }

        public static NuPlugin Build(string name)
        {
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            return new NuPlugin(stdin, stdout, name);
        }

        public NuPlugin Description(string description)
        {
            _signature = _signature.WithDescription(description);
            return this;
        }

        public NuPlugin Required(SyntaxShape syntaxShape, string name, string description)
        {
            _signature = _signature.AddRequiredPositional(syntaxShape, name, description);
            return this;
        }

        public NuPlugin Optional(SyntaxShape syntaxShape, string name, string description)
        {
            _signature = _signature.AddOptionalPositional(syntaxShape, name, description);
            return this;
        }

        public NuPlugin Switch(string name, string description, char? flag = null)
        {
            _signature = _signature.AddSwitch(name, description, flag);
            return this;
        }

        public NuPlugin Named(SyntaxShape syntaxShape, string name, string description, char? flag = null)
        {
            _signature = _signature.AddOptionalNamed(syntaxShape, name, description, flag);
            return this;
        }

        public NuPlugin RequiredNamed(SyntaxShape syntaxShape, string name, string description, char? flag = null)
        {
            _signature = _signature.AddRequiredNamed(syntaxShape, name, description, flag);
            return this;
        }

        public NuPlugin Rest(string description, SyntaxShape syntaxShape = null)
        {
            _signature = _signature.AddRestPositionalArguments(description, syntaxShape);
            return this;
        }

        public NuPlugin Yields<TInput>()
        {
            throw new NotImplementedException("To be implemented in the future");
        }

        public NuPlugin Input<TInput>()
        {
            throw new NotImplementedException("To be implemented in the future");
        }

        public async Task SinkAsync<TSinkPlugin>() where TSinkPlugin : INuPluginSink, new()
        {
            await CommandHandler<TSinkPlugin>(
                (req, res) =>
                {
                    switch (req.Method)
                    {
                        case "config":
                            res.Config(_signature);
                            break;
                        case "sink":
                            {
                                var requestParams = req.GetParams<IEnumerable<JsonRpcValue>>();
                                res.Sink(requestParams);
                                break;
                            }
                        default:
                            res.Quit();
                            break;
                    }
                }
            );
        }

        public async Task FilterAsync<TFilterPlugin>() where TFilterPlugin : INuPluginFilter, new()
        {
            await CommandHandler<TFilterPlugin>(
                (req, res) =>
                {
                    switch (req.Method)
                    {
                        case "config":
                            res.Config(_signature.WithIsFilter(true));
                            break;
                        case "begin_filter":
                            res.BeginFilter();
                            break;
                        case "filter":
                            res.Filter(req.GetParams<JsonRpcValue>());
                            break;
                        case "end_filter":
                            res.EndFilter();
                            break;
                        default:
                            res.Quit();
                            break;
                    }
                }
            );
        }

        private async Task CommandHandler<TPluginType>(
            Action<JsonRpcRequest, PluginResponse<TPluginType>> pluginRes
        ) where TPluginType : new()
        {
            using var handler = PluginHandler<TPluginType>.Create(_stdin, _stdout);
            while (await handler.HandleNextRequestAsync(pluginRes)) { }
        }
    }
}
