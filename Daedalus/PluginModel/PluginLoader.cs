using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

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
                       try
                       {
                           Assembly loaded = Assembly.LoadFile(Path.GetFullPath(dll));
                           Plugins.Add(((IPlugin)loaded.GetTypes().First(t => t.GetInterfaces().Contains(typeof(IPlugin))).InvokeMember("", BindingFlags.CreateInstance, null, null, null)));
                       }
                       catch (Exception v)
                       {
                           if (File.Exists(Path.Combine(Path.GetDirectoryName(dll), "disabled." + Path.GetFileName(dll))))
                               File.Delete(Path.Combine(Path.GetDirectoryName(dll), "disabled." + Path.GetFileName(dll)));
                           File.Move(dll, Path.Combine(Path.GetDirectoryName(dll), "disabled." + Path.GetFileName(dll)));
                           File.WriteAllText(Path.Combine(Path.GetDirectoryName(dll), "disabled." + Path.GetFileName(dll) + ".log"), v.ToString());
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
    }
}
