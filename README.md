# Nu.Plugin (.Net Standard 2.0)

A .Net Standard class library for the sole purpose of building plugins for Nu Shell, https://github.com/nushell/nushell

A .Net plugin for Nu Shell is a console application that reads the standard input stream and writes to the standard output stream. The communication protocol is done via [JSON-RPC](https://www.jsonrpc.org/).

## Motivations

To see how feasible it would be to create a plugin for Nu Shell in a language I am familiar with as well enjoy working in, C#. The biggest hurdle was figuring out how to communicate with the correct Json structure protocol. I personally like the vision for Nu Shell and am looking for ways to explore various avenues of getting others involved in the ecosystem.

I used the [sample Python plugin](https://github.com/nushell/contributor-book/blob/master/en/plugins.md#creating-a-plugin-in-python) as the starting point and went from there.

Eventually I abstracted the plugin bits into their own class library, which should make it much easier for others to consume and begin making .Net Core Nu Shell plugins.

## Creating Your First Plugin

`TODO: Add directions here`