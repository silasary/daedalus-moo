﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Daedalus.PluginModel
{
    public static class PluginLoader
    {
       static IPlugin[] plugins;
       static IPlugin[] Plugins
       {
           get
           {
               if (plugins == null)
               {
                   List<IPlugin> Plugins = new List<IPlugin>();
                   foreach (string dll in Directory.GetFiles(".", "Daedalus.Plugin.*", SearchOption.AllDirectories))
                   {
                       if (Path.GetExtension(dll) != ".dll" && Path.GetExtension(dll) != ".exe")
                           continue;
                       try
                       {
                           Assembly loaded = Assembly.LoadFile(Path.GetFullPath(dll));
                           Plugins.Add(((IPlugin)loaded.GetTypes().First(t => t.GetInterfaces().Contains(typeof(IPlugin))).InvokeMember("", BindingFlags.CreateInstance, null, null, null)));
                       }
                       catch (Exception v)
                       {
                           File.WriteAllText(Path.Combine(Path.GetDirectoryName(dll), Path.GetFileName(dll) + ".log"), v.ToString());
                       }
                   }
                   plugins = Plugins.ToArray();
               }
               return plugins;
           }
       }

       internal static void DispatchNewConnection(IConnection connection)
       {
           foreach (IPlugin plugin in Plugins)
           {
               plugin.NewConnection(connection);
           }
       }

       public static List<MCP.MCPPackage> ExternalMCPPackages(MCP.MCPHandler handler)
       {
           List<MCP.MCPPackage> packages = new List<MCP.MCPPackage>();
           foreach (IPlugin plugin in Plugins)
           {
               try
               {
                   
                   foreach (Type packageType in plugin.MCPPackages)
                   {
                        packages.Add((MCP.MCPPackage)packageType.InvokeMember("",BindingFlags.CreateInstance,null,null,new object[] { handler}));
                   }
               }
               catch (NotImplementedException) { }
           }
           return packages;
       }

       internal static Type[] SerializedTypes
       {
           get
           {
               List<Type> types = new List<Type>();
               foreach (IPlugin plug in Plugins)
               {
                   try
                   {
                       types.AddRange(plug.SettingsTypes);
                   }
                   catch (NotImplementedException) { if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break(); }
               }
               return types.ToArray();
           }
       }

       internal static TabPage[] OptionsPages
       {
           get {

               List<TabPage> types = new List<TabPage>();
               foreach (IPlugin plug in Plugins)
               {
                   try
                   {
                       types.AddRange(plug.Options);
                   }
                   catch (NotImplementedException) { if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break(); }
               }
               return types.ToArray();
           }
       }
    }
}
