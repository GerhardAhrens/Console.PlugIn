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
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Windows;

    using Console.PlugInContract;

    public class Program
    {
        public static HashSet<IPlugIn> Plugins { get; } = new();
        private static BufferedFileSystemWatcher bfsw;


        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }
        private static void Main(string[] args)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            PlugInPath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "PlugIns");
            if (Directory.Exists(PlugInPath) == false)
            {
                Directory.CreateDirectory(PlugInPath);
            }

            CMenu mainMenu = new CMenu("PlugIn Reader");
            mainMenu.AddItem("Start PlugIn Reader", MenuPoint1);
            mainMenu.AddItem("PlugIn ausführen", MenuPoint2);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        internal static string PlugInPath { get; private set; }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            Console.WriteText($"PlugIn Reader gestartet; Verzeichnis: {PlugInPath}", ConsoleColor.Green);

            /* Lesen der bereits vorhandenen PlugIns */
            ReadPlugIn();

            bfsw = new BufferedFileSystemWatcher(PlugInPath);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Created", OnPlugInReader);
            WeakEventManager<BufferedFileSystemWatcher, FileSystemEventArgs>.AddHandler(bfsw, "Deleted", OnPlugInReader);
            bfsw.SetBufferedChangeTypes(BufferedChangeTypes.Created | BufferedChangeTypes.Deleted);
            bfsw.StartFileSystemWatcher();

            Console.Wait();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            if (Plugins.Count > 0)
            {
                Console.WriteText($"Anzahl Plugins: {CountPlugIns()}", ConsoleColor.Green);
                Console.Line('-');
                foreach (IPlugIn plugin in Plugins)
                {
                    decimal result = plugin.Calculate(100,19);
                    Console.WriteText(plugin.ShortDescription, ConsoleColor.Yellow);
                    Console.WriteText($"Ergebnis aus {plugin.Modul}; {result.ToString("#,00", CultureInfo.CurrentCulture)}", ConsoleColor.Yellow);
                    Console.Line('-');
                }
            }

            Console.Wait();
        }

        private static int CountPlugIns()
        {
           return Plugins.Count;
        }

        private static void ReadPlugIn()
        {
            try
            {
                IEnumerable<string> plugIns = MultiEnumerateFiles(PlugInPath, "*.dll");
                if (plugIns != null)
                {
                    foreach (string file in plugIns.AsParallel())
                    {
                        Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                        if (assembly != null)
                        {
                            var types = assembly.GetTypes().Where(t => typeof(IPlugIn).IsAssignableFrom(t) && t.IsInterface == false);

                            foreach (Type type in types.AsParallel())
                            {
                                if (type.IsAbstract == false)
                                {
                                    if (Activator.CreateInstance(type) is IPlugIn plugin)
                                    {
                                        Plugins.Add(plugin);
                                        Console.WriteText($"Load PlugIn '{Path.GetFileName(file)}'; (Modul: {plugin.Modul})", ConsoleColor.Yellow);
                                        Console.Line('-');
                                    }
                                }
                            }
                        }
                    }

                    Console.WriteText($"Anzahl Plugins: {CountPlugIns()}", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                string errorText = $"Fehler: {ex.Message}";
            }
        }

        private static IEnumerable<string> MultiEnumerateFiles(string path, string patterns)
        {
            EnumerationOptions eo = new EnumerationOptions();
            eo.RecurseSubdirectories = true;
            eo.IgnoreInaccessible = true;
            eo.RecurseSubdirectories = true; /*wichtige Option, wenn die dateien auf einem Netzwerkpfad liegen */
            eo.ReturnSpecialDirectories = false;

            foreach (var pattern in patterns.Split('|'))
            {
                foreach (var file in Directory.EnumerateFiles(path, pattern, eo))
                {
                    yield return file;
                }
            }
        }

        private static void OnPlugInReader(object sender, FileSystemEventArgs e)
        {
            string file = Path.Combine(PlugInPath, e.Name);
            WatcherChangeTypes changeType = e.ChangeType;

            try
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                if (assembly != null)
                {
                    var types = assembly.GetTypes().Where(t => typeof(IPlugIn).IsAssignableFrom(t) && t.IsInterface == false);

                    foreach (var type in types.AsParallel())
                    {
                        if (type.IsAbstract == false)
                        {
                            if (Activator.CreateInstance(type) is IPlugIn plugin)
                            {
                                if (Plugins.Add(plugin) == false)
                                {
                                    Console.WriteText($"PlugIn '{plugin.Description}' (Version: {plugin.Version}) ist bereits geladen.", ConsoleColor.Red);
                                    Console.Line('-');
                                }
                                else
                                {
                                    Console.WriteText($"Read PlugIn '{file}' (ChangeType: {changeType})", ConsoleColor.Yellow);
                                    Console.Line('-');
                                }
                            }
                        }
                    }

                    Console.WriteText($"Anzahl Plugins: {CountPlugIns()}", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                string errorText = $"Fehler: {ex.Message}";
            }
        }
    }
}
