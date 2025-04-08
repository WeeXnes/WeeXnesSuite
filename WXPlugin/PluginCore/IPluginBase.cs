using System;

namespace WXPlugin.PluginCore
{
    public interface IPluginBase
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public void Initialize();
        public void Execute();
    }
}