using System;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace WXPlugin.PluginCore
{
    public interface IPluginBase
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Page UiPage { get; set; }
        public NavigationItem NavIcon { get; set; }
        public void Initialize();
        public void Execute();
    }
}