#region Header

// =====================================================================
// File Name                      : FileCopier.cs
// Date Created                 : 30/05/2019 - 08:28
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

using System;
using System.IO;

namespace Client
{
    public class FileCopier : IFileCopier, IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {
        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopyEvent = delegate { };

        private readonly ILogger _logger;
        private readonly CommandOptions _options;

        public FileCopier(ILogger logger, CommandOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public void CopyFile(string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(_options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(_options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteTargetFilePath) && !_options.OverrideTargetFiles)
            {
                _logger.LogInfo(
                    $"{fileName} exists. Skipped the copy operation because overwriteTargetFile is set to false.");
                return;
            }

            PreCopyEvent(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, _options.OverrideTargetFiles);
            PostCopyEvent(absoluteTargetFilePath);
        }
    }

    public interface IPostCopyEventBroadcaster
    {
        event Action<string> PostCopyEvent;
    }

    public interface IPreCopyEventBroadcaster
    {
        event Action<string> PreCopyEvent;
    }
}