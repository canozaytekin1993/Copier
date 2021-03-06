#region Header

// =====================================================================
// File Name                    : CommandOptions.cs
// Date Created                 : 28/05/2019 - 20:52
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

#region Referance

using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

#endregion

namespace Client
{
    [Verb("watch", HelpText = "If you want to pass options manually, use this command to begin watches.")]
    public class CommandOptions
    {
        [Option('s', "sourceDirectoryPath", HelpText =
            "Parent directory where the files will be searched. If skipped, the current directory will be searched.")]
        public string SourceDirectoryPath { get; set; }

        [Option('f', "fileGlobPattern", Required = true,
            HelpText = "Files to be searched. Accepts glob patter for pattern  matching.")]
        public string FileGlobPattern { get; set; }

        [Option('d', "destinationDirectoryPath", Required = true, HelpText = "Destination directory path")]
        public string DestinationDirectoryPath { get; set; }

        [Option('o', "overwriteTargetFiles", Default = false, Required = false,
            HelpText = "If passed true, copier will overwrite existing files at the target location.")]
        public bool OverrideTargetFiles { get; set; }

        [Option('v', "verbose", Default = false, Required = false,
            HelpText = "If passed true, more information will be outputted to the console.")]
        public bool Verbose { get; set; }

        [Option('e', "debug", Default = false, Required = false, HelpText = "Shows debug information")]
        public bool Debug { get; set; }

        [Option('t', "delay", Default = 0, Required = false, HelpText = "Delays copy operation for a given time.")]
        public int Delay { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>
        {
            new Example("Starts the copier", new UnParserSettings {PreferShortName = true}, new CommandOptions
            {
                SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                FileGlobPattern = "*.jpg",
                DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages"
            }),
            new Example("Starts the copier and overwrites the target files.",
                new UnParserSettings {PreferShortName = true}, new CommandOptions
                {
                    SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                    FileGlobPattern = "*.jpg",
                    DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages"
                }),
            new Example("Starts the copier and overwrites the target files and outputs verbose.",
                new UnParserSettings {PreferShortName = true}, new CommandOptions
                {
                    SourceDirectoryPath = "C:/Users/MyDocuments/Images",
                    FileGlobPattern = "*.jpg",
                    DestinationDirectoryPath = "C:/Users/MyDocuments/NewImages",
                    Verbose = true
                })
        };
    }
}