# Nu.Plugin (.Net Standard 2.0)

A .Net Standard class library for the sole purpose of building plugins for Nu Shell, https://github.com/nushell/nushell

A Nu Shell plugin is any executable that macthes the pattern of `nu_plugin_*` so in essence, a .Net plugin for Nu Shell is a console application. At it's simplest form, a nu plugin is an application that reads the standard input stream and writes to the standard output stream. The communication protocol is done via [JSON-RPC](https://www.jsonrpc.org/). For more information: [Nu Plugins - Discovery](https://github.com/nushell/contributor-book/blob/master/en/plugins.md#discovery)

## Motivations

To see how feasible it would be to create a plugin for Nu Shell in a language I am familiar with as well enjoy working in, C#. The biggest hurdle was figuring out how to communicate with the correct Json structure protocol. I personally like the vision for Nu Shell and am looking for ways to explore various avenues of getting others involved in the ecosystem.

I used the [sample Python plugin](https://github.com/nushell/contributor-book/blob/master/en/plugins.md#creating-a-plugin-in-python) as the starting point and went from there.

Eventually I abstracted the plugin bits into their own class library, which should make it much easier for others to consume and begin making .Net Core Nu Shell plugins.

## A Walk Through Creating Your First Plugin

This will go through the steps that would have gone into creating this "len" plugin

From the command line:

```cmd
dotnet new console -o Nu.Plugin.Len
cd Nu.Plugin.Len
dotnet add package Nu.Plugin
```

Open `Program.cs` and update to the following:

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nu.Plugin.Interfaces;

namespace Nu.Plugin.Len
{
    class Program : INuPluginFilter
    {
        static async Task Main(string[] args) => await NuPlugin.Create()
            .Name("len")
            .Usage("Return the length of a string")
            .IsFilter<Program>()
            .RunAsync();

        public object BeginFilter() => Array.Empty<string>();

        public JsonRpcParams Filter(JsonRpcParams requestParams)
        {
            var stringLength = requestParams.Value.Primitive["String"].ToString().Length;

            requestParams.Value.Primitive = new Dictionary<string, object>{
                {"Int", stringLength}
            };

            return requestParams;
        }

        public object EndFilter() => Array.Empty<string>();
    }
}
```

Open `Nu.Plugin.Len.csproj` and add the `AssemblyName` element to the `PropertyGroup` like so:

```xml
<PropertyGroup>
    <AssemblyName>nu_plugin_len</AssemblyName>
    <OutputType>Exe</OutputType>
    <TargetFramework>...</TargetFramework>
</PropertyGroup>
```

The key takeaway is that Nu searches for any executable file within the path named `nu_plugin_*` to be included as plugins. By updating the assembly name to this pattern, it will create an executable with the name, `nu_plugin_len`, For more information: [Nu Plugins - Discovery](https://github.com/nushell/contributor-book/blob/master/en/plugins.md#discovery)

Build the project:
```cmd
dotnet build
```

** Note: Update your OS settings to include the PATH to the debug directory where the `nu_plugin_len` executable was created.  Next time you start Nu Shell, it will now discover your newly created .Net Core executable as Nu plugin.

Run from within Nu shell:

```shell
echo "Hello, world" | len
```

![Nu Shell Len Plugin](/assets/img/dotnet-nu-plugin-len.gif)
