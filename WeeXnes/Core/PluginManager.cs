﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WXPlugin.PluginCore;

namespace WeeXnes.Core;

public class PluginManager
{
    private HashSet<String> Directories = new HashSet<String>();
    public HashSet<IPluginBase> CurrentPlugins { get; set; } = new HashSet<IPluginBase>();
    public PluginManager(String pluginPath)
    {
        
        Console.WriteLine("Plugin Manager Initialized");
        Directories.Add(pluginPath);
    }

    public void LoadPlugins()
    {
        
        Console.WriteLine("Plugin Manager Loading Plugins");
        CurrentPlugins = new HashSet<IPluginBase>();
        foreach (var dir in Directories)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            foreach (var file in dirInfo.GetFiles("*.dll"))
            {
                Assembly asm = Assembly.LoadFrom(file.FullName);
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(IPluginBase)) || type.GetInterfaces().Contains(typeof(IPluginBase)) && type.IsAbstract == false)
                    {
                        IPluginBase newPlugin = type.InvokeMember(
                                null,
                                BindingFlags.CreateInstance,
                                null,
                                null,
                                null)
                            as IPluginBase;
                        Console.WriteLine("Loaded: " + newPlugin.Name);
                        Console.WriteLine(newPlugin.UiPage.Content.ToString());
                        CurrentPlugins.Add(newPlugin);
                    }
                }
            }
        }
    }
}