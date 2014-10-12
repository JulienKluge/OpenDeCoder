using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using OpenDeCoder.Coding;
using OpenDeCoder.Misc;

namespace OpenDeCoder
{
    public static class Program
    {
        public static MainWindow window;
        public static ICoder[] Coder;
        public static List<ProgramDebugEntry> DebugEntrys;
        public static Pattern[] Patterns;

        [STAThread]
        public static void Main()
        {
            SplashScreen splash = new SplashScreen("Res/SplashImage.png");
            splash.Show(true);
            DebugEntrys = new List<ProgramDebugEntry>();
            Coder = GatherAllCoder();
            IngressKeywords.LoadKeyWords();
            Patterns = PatternLoader.LoadPatterns();
            Application app = new Application();
            app.Resources = new ResourceDictionary() { Source = new Uri("/OpenDeCoder;component/AppResources.xaml", UriKind.Relative) };
            window = new MainWindow();
            app.Run(window);
        }

        private static ICoder[] GatherAllCoder()
        {
            List<ICoder> coderList = new List<ICoder>();
            List<Assembly> searchAssemblies = new List<Assembly>();
            searchAssemblies.Add(Assembly.GetExecutingAssembly());
            string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "coder");
            int loadedAssemblies = 0;
            if (Directory.Exists(pluginDirectory))
            {
                string[] files = Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; ++i)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(files[i]);
                        searchAssemblies.Add(assembly);
                        loadedAssemblies++;
                        Program.DebugEntrys.Add(new ProgramDebugEntry("Assembly loaded: \"" + files[i] + "\"", false));
                    }
                    catch (Exception e)
                    {
                        Program.DebugEntrys.Add(new ProgramDebugEntry("Error while loading assembly: \"" + files[i] + "\" (" + e.Message + ")", true));
                    }
                }
                for (int i = 0; i < searchAssemblies.Count; ++i)
                {
                    Type[] types = searchAssemblies[i].GetTypes();
                    for (int j = 0; j < types.Length; ++j)
                    {
                        Type[] interfaces = types[j].GetInterfaces();
                        for (int k = 0; k < interfaces.Length; ++k)
                        {
                            if (interfaces[k].Name == typeof(ICoder).Name)
                            {
                                try
                                {
                                    ICoder obj = (ICoder)Activator.CreateInstance(types[j]);
                                    coderList.Add(obj);
                                    StringBuilder messageBuilder = new StringBuilder();
                                    messageBuilder.Append(("Coder loaded: \"" + types[j].Name + "\"").PadRight(45, ' ') + " (" + obj.Name + ")");
                                    bool Insert = obj.Insert; bool BruteForce = obj.BruteForceAble;
                                    if (Insert || BruteForce)
                                    {
                                        if (Insert)
                                        { messageBuilder.Append(" ;Inserted"); }
                                        if (BruteForce)
                                        { messageBuilder.Append(" ;Brute Force Able"); }
                                    }
                                    else
                                    {
                                        messageBuilder.Append(" ;DISABLED");
                                    }
                                    Program.DebugEntrys.Add(new ProgramDebugEntry(messageBuilder.ToString(), false));
                                }
                                catch (Exception e)
                                {
                                    Program.DebugEntrys.Add(new ProgramDebugEntry("Error while creating instance of coder: \"" + types[j].Name + "\" (" + e.Message + ")", true));
                                }
                            }
                        }
                    }
                }
            }
            Program.DebugEntrys.Add(new ProgramDebugEntry(coderList.Count.ToString() + " coder loaded from " + loadedAssemblies.ToString() + " assemblies", false));
            return coderList.ToArray();
        }
    }
}
