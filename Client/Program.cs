using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;

namespace Client
{
    class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions, ConfigFileCommandOptions>(args)
                .WithParsed<CommandOptions>(StartWatching)
                .WithParsed<ConfigFileCommandOptions>(StartWatchingWithConfigurationFile)
                .WithNotParsed(a => { Environment.Exit(1); });

            Console.WriteLine("Please press any key to exit.");
            Console.ReadLine();
        }

        private static void StartWatchingWithConfigurationFile(ConfigFileCommandOptions configFileOptions)
        {
            ILogger logger = new ConsoleLogger();

            if (File.Exists(configFileOptions.ConfigFilePath))
            {
                var options = GetCommandOptionsFromConfigFile(configFileOptions);

                Parser.Default.ParseArguments<CommandOptions>(options)
                    .WithParsed(StartWatching)
                    .WithNotParsed(
                        a => { Environment.Exit(1); });
            }
            else
            {
                logger.LogError(
                    $"Cannot find {configFileOptions.ConfigFilePath}! Please make sure the file exists in the given location.");
            }
        }

        private static List<string> GetCommandOptionsFromConfigFile(ConfigFileCommandOptions options)
        {
            var configContent = File.ReadAllLines(options.ConfigFilePath);
            var trimmedConfig = configContent.SelectMany(a =>
            {
                var result = Regex.Match(a, "\"(.*?)\"");
                if (result.Success)
                {
                    var option = a.Replace(result.Value, "");
                    return new[] {option.Trim(), result.Value.Trim().Replace("\"", "")};
                }

                return new[] {a.Trim()};
            }).ToList();
            return trimmedConfig;
        }

        private static void StartWatching(CommandOptions options)
        {
            ILogger logger = new ConsoleLogger();

            logger.LogInfo("Watching has started..");

            options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
                ? Directory.GetCurrentDirectory()
                : options.SourceDirectoryPath;

            IPluginLoader loader = new PluginLoader(logger, options.Debug);

            var fileCopier = new FileCopier(logger, options);
            IFileCopier copier = fileCopier;

            if (options.Delay > 0)
            {
                copier = new QueuedFileCopier(fileCopier, logger, options);
            }

            IFileWatcher fileWatcher = new FileWatcher(copier, logger);

            loader.Subscribe((IPreCopyEventBroadcaster) copier, (IPostCopyEventBroadcaster) copier);

            fileWatcher.Watch(options);
        }
    }
}