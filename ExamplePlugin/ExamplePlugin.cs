using System;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using WXPlugin.PluginCore;

namespace ExamplePlugin
{
    public class ExamplePlugin : IPluginBase
    {
        public String Name { get; set; } = "Example Plugin";
        public String Description { get; set; } = "This is an example plugin.";

        public Page UiPage { get; set; } = new ExampleUi();
        public NavigationItem NavIcon { get; set; } = new NavigationItem
        {
            Content = "Plugin",
            Icon = SymbolRegular.Syringe20,
            PageTag = "examplePlugin",
            PageType = typeof(ExampleUi),
        };

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}