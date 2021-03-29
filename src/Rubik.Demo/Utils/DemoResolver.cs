using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Rubik.Service;
using Rubik.Util.Log;

namespace Rubik.Demo.Utils
{
    /// <summary>
    /// Demo Resolver, Ref: https://github.com/PrismLibrary/Prism/blob/master/src/Wpf/Prism.Wpf/Modularity/DirectoryModuleCatalog.netcore.cs
    /// </summary>
    public class DemoResolver
    {
        public string DemoPath { get; private set; }

        public DemoResolver(string demoPath)
        {
            DemoPath = demoPath;
        }

        public IDemoModel[] LoadDemoModels()
        {
            if (string.IsNullOrWhiteSpace(DemoPath) || !Directory.Exists(DemoPath))
                return null;

            var childDomain = AppDomain.CurrentDomain;

            try
            {
                var loaderType = typeof(InnerDemoInfoLoader);
                if (loaderType.Assembly != null)
                {
                    var loader = (InnerDemoInfoLoader)childDomain.CreateInstanceFrom(loaderType.Assembly.Location, loaderType.FullName).Unwrap();

                    return loader.GetDemoModels(DemoPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Main.Error($"[ DemoResolver ] GetDemoModels, Error = {ex.Message}");
            }

            return null;
        }

        private class InnerDemoInfoLoader : MarshalByRefObject
        {
            internal IDemoModel[] GetDemoModels(string path)
            {
                var directory = new DirectoryInfo(path);

                ResolveEventHandler resolveEventHandler =
                    delegate (object sender, ResolveEventArgs args) { return OnReflectionOnlyResolve(args, directory); };

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

                var moduleReflectionOnlyAssembly = AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.FullName == typeof(IDemoModel).Assembly.FullName);
                var idemoType = moduleReflectionOnlyAssembly.GetType(typeof(IDemoModel).FullName);

                var modules = GetNotAlreadyLoadedDemoModels(directory, idemoType);

                var array = modules.ToArray();
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
                return array;
            }

            private static IEnumerable<IDemoModel> GetNotAlreadyLoadedDemoModels(DirectoryInfo directory, Type idemoType)
            {
                var validAssemblies = new List<Assembly>();
                var alreadyLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic).ToArray();

                var fileInfos = directory.GetFiles("*.dll")
                    .Where(file => alreadyLoadedAssemblies.FirstOrDefault(
                        assembly => String.Compare(Path.GetFileName(assembly.Location),
                        file.Name, StringComparison.OrdinalIgnoreCase) == 0) == null).ToList();

                foreach (FileInfo fileInfo in fileInfos)
                {
                    try
                    {
                        validAssemblies.Add(Assembly.LoadFrom(fileInfo.FullName));
                    }
                    catch (BadImageFormatException)
                    {
                        // skip non-.NET Dlls
                    }
                }

                return validAssemblies.SelectMany(assembly => assembly
                            .GetExportedTypes()
                            .Where(idemoType.IsAssignableFrom)
                            .Where(t => t != idemoType)
                            .Where(t => !t.IsAbstract)
                            .Select(type => CreateDemoModel(type)));
            }

            private static Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
            {
                var loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(asm => string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));
                if (loadedAssembly != null)
                    return loadedAssembly;

                var assemblyName = new AssemblyName(args.Name);
                var dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");

                if (File.Exists(dependentAssemblyFilename))
                    return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);

                return Assembly.ReflectionOnlyLoad(args.Name);
            }

            internal void LoadAssemblies(IEnumerable<string> assemblies)
            {
                foreach (string assemblyPath in assemblies)
                {
                    try
                    {
                        Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                    }
                    catch (FileNotFoundException)
                    {
                        // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain
                    }
                }
            }

            private static IDemoModel CreateDemoModel(Type type)
            {
                return (IDemoModel)Activator.CreateInstance(type);
            }
        }
    }
}
