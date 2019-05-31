#region Header

// =====================================================================
// File Name                      : ConfigFileCommandOptions.cs
// Date Created                 : 31/05/2019 - 10:22
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

using CommandLine;

namespace Client
{
    [Verb("watchWithConfigFile", HelpText =
        "If you want to pass a config file that has options, then initiate watching and copying operation.")]
    public class ConfigFileCommandOptions
    {
        [Option('c', "configFilePath", Default = "config.txt",
            HelpText = "To be used instead of passing all the options manually thru the command line.")]
        public string ConfigFilePath { get; set; }
    }
}