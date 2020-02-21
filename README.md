# Nu.Plugin (.Net Standard 2.0)

A .Net Standard class library for the sole purpose of building plugins for Nu Shell, https://github.com/nushell/nushell

A plugin for Nu Shell is basically a console application that reads the standard input stream and writes to the standard output stream.

## Motivations

To see how feasible it would be to create a plugin for Nu Shell.  Nu Shell uses JsonRpc to communicate with plugins.  The biggest hurdle was figuring out how to communicate with the correct Json structure protocol. I personally like the vision for Nu Shell and am looking for ways to explore various ways of getting others involved in the ecosystem.

The sample plugin is based on the Python example from the Nu Shell repo: https://github.com/nushell/contributor-book/blob/master/en/plugins.md#creating-a-plugin-in-python

## Getting Started

TODO