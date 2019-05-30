#region Header

// =====================================================================
// File Name                      : FileCopier.cs
// Date Created                 : 30/05/2019 - 08:28
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

using System;
using System.IO;
using CopierPluginBase;

namespace Client
{
    public class FileCopier : IFileCopier, IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {
        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopy = delegate { };
        
        private readonly ILogger _logger;

        public FileCopier(ILogger logger)
        {
            _logger = logger;
        }

        public void CopyFile(CommandOptions options, string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteTargetFilePath) && !options.OverrideTargetFiles)
            {
                _logger.Write($"{fileName} exists. Skipped because overwriteTargetFile is set to false.");
                return;
            }

            PreCopyEvent(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, options.OverrideTargetFiles);
            PostCopy(absoluteTargetFilePath);
        }
    }

    public interface IPostCopyEventBroadcaster
    {
        event Action<string> PostCopy;
    }

    public interface IPreCopyEventBroadcaster
    {
        event Action<string> PreCopyEvent;
    }
}