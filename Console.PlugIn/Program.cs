//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.PlugIn
{
    /* Imports from NET Framework */
    using System;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Windows;

    using Console.PlugInContract;

    public class Program
    {
        public static HashSet<IPlugIn> Plugins { get; } = new();
        private static BufferedFileSystemWatcher bfsw;
        private static string path = @"c:\_DownLoads\";


        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }
        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("PlugIn Reader");
            mainMenu.AddItem("Start PlugIn Reader", MenuPoint1);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            bfsw = new BufferedFileSystemWatcher(path);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Created", OnPlugInReader);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Deleted", OnPlugInReader);
            bfsw.SetBufferedChangeTypes(BufferedChangeTypes.Created | BufferedChangeTypes.Deleted);
            bfsw.StartFileSystemWatcher();

            Console.Wait();
        }

        private static void ReadPlugIn()
        {
            try
            {

            }
            catch (Exception ex)
            {
                string errorText = $"Fehler: {ex.Message}";
            }
        }

        private static void OnPlugInReader(object sender, FileSystemEventArgs e)
        {
            string file = Path.Combine(path, e.Name);
            WatcherChangeTypes changeType = e.ChangeType;
            Console.WriteLine($"Read PlugIn '{file}' (ChangeType: {changeType})");
            try
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                if (assembly != null)
                {
                    var types = assembly.GetTypes().Where(t => typeof(IPlugIn).IsAssignableFrom(t) && t.IsInterface == false);

                    foreach (var type in types)
                    {
                        if (Activator.CreateInstance(type) is IPlugIn plugin)
                        {
                            if (Plugins.Contains(plugin) == false)
                            {
                                Plugins.Add(plugin);
                            }
                            else
                            {
                                Console.WriteLine($"PlugIn '{plugin.Description}' (Version: {plugin.Version}) ist bereits geladen.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorText = $"Fehler: {ex.Message}";
            }
        }

    }
}
