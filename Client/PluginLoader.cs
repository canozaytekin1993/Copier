#region Header

// =====================================================================
// File Name                    : PluginLoader.cs
// Date Created                 : 30/05/2019 - 11:29
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

#region Reference

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CopierPluginBase;

#endregion

namespace Client
{
    public class PluginLoader : IPluginLoader
    {
        private readonly ILogger _debugLogger;
        private readonly List<Type> _preCopyListeners = new List<Type>();
        private readonly List<Type> _postCopyListeners = new List<Type>();

        private bool ShowDebugMessages { get; }

        public PluginLoader(ILogger debugLogger, bool showDebugMessages) : this()
        {
            _debugLogger = debugLogger;
            ShowDebugMessages = showDebugMessages && debugLogger != null;
            Initialize();
        }

        public PluginLoader()
        {
            Initialize();
        }

        private static string GetPluginsDirectory()
        {
            #if DEBUG
                return Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\netcoreapp2.1", "plugins");
            #else
                 return Path.Combine(Directory.GetCurrentDirectory(), "plugins");
            #endif
        }

        private void Initialize()
        {
            var pluginDirectory = GetPluginsDirectory();
            
            if (Directory.Exists(pluginDirectory))
            {
                if (ShowDebugMessages)
                {
                    _debugLogger.LogWarning("Cannot find plugins folder.");
                }

                return;
            }

            var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

            foreach (var assemblyName in assemblyFiles)
            {
                var pluginAssembly = Assembly.LoadFile(assemblyName);

                if (ShowDebugMessages) _debugLogger.LogDebug($"Loaded {assemblyName}");

                var existingTypes = pluginAssembly.GetTypes();

                bool TypePredicate(Type child, Type parent)
                {
                    return child.IsPublic && !child.IsAbstract && child.IsClass && child.IsAssignableFrom(child);
                }

                var postCopyListenerTypes =
                    existingTypes.Where(a => TypePredicate(a, typeof(IPostCopyEventListener))).ToList();
                _postCopyListeners.AddRange(postCopyListenerTypes);

                var preCopyListenerTypes =
                    existingTypes.Where(a => TypePredicate(a, typeof(IPreCopyEventListener))).ToList();
                _preCopyListeners.AddRange(preCopyListenerTypes);

                // If enabled, logging debug messages for the found types in the iterated assembly.
                if (ShowDebugMessages)
                {
                    _debugLogger.LogDebug($"Found the following PostCopy types from plugin {assemblyName}:");
                    _debugLogger.LogDebug(string.Join("\n", postCopyListenerTypes.Select(x => x.Name).ToArray()));

                    _debugLogger.LogDebug($"Found the following PreCopy types from plugin {assemblyName}:");
                    _debugLogger.LogDebug(string.Join("\n", preCopyListenerTypes.Select(x => x.Name).ToArray()));
                }
            }
        }

        public void Subscribe(IPreCopyEventBroadcaster pre, IPostCopyEventBroadcaster post)
        {
            _preCopyListeners.ForEach(a =>
            {
                var listenerObject = (IPreCopyEventListener) Activator.CreateInstance(a);
                pre.PreCopyEvent += listenerObject.OnPreCopy;
            });

            _postCopyListeners.ForEach(a =>
            {
                var listenerObject = (IPostCopyEventListener) Activator.CreateInstance(a);
                post.PostCopyEvent += listenerObject.OnPostCopy;
            });
        }
    }
}