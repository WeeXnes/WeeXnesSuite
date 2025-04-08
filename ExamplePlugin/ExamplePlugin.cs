using System;
using WXPlugin.PluginCore;

namespace ExamplePlugin
{
    public class ExamplePlugin : IPluginBase
    {
        public String Name { get; set; } = "Example Plugin";
        public String Description { get; set; } = "This is an example plugin.";

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}