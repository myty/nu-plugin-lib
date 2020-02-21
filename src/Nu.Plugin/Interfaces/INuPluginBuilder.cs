using System;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin
{
    public interface INuPluginBuilder
    {
        INuPluginBuilder Name(string name);

        INuPluginBuilder Usage(string usage);

        INuPluginBuilder IsFilter<T>() where T: INuPluginFilter, new();

        Task RunAsync();
    }
}
