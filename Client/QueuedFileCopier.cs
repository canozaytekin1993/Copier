#region Header

// =====================================================================
// File Name                      : QueuedFileCopier.cs
// Date Created                 : 30/05/2019 - 19:09
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public class QueuedFileCopier : IFileCopier, IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {
        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopyEvent = delegate { };
        private readonly IFileCopier _fileCopier;
        private readonly ILogger _logger;
        private readonly CommandOptions _options;

        private HashSet<string> _fileNameQueue = new HashSet<string>();
        private Task _copyTask;

        public QueuedFileCopier(IFileCopier fileCopier, ILogger logger, CommandOptions options)
        {
            _fileCopier = fileCopier;
            _logger = logger;
            _options = options;

            if (_options.Debug)
            {
                logger.LogInfo("Delay option has beeen specified. QueudFileCopier is chosen as the copier strategy.");
            }
        }

        public void CopyFile(string fileName)
        {
            if (_copyTask == null)
            {
                _copyTask = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(_options.Delay));
                    if (_options.Verbose || _options.Debug)
                    {
                        _logger.LogInfo($"{_options.Delay} miliseconds have passed.The copy operattion has started.");
                    }

                    PreCopyEvent("");

                    foreach (var item in _fileNameQueue)
                    {
                        _fileCopier.CopyFile(item);
                    }

                    PostCopyEvent("");

                    _copyTask = null;

                    if (_options.Verbose || _options.Debug)
                    {
                        _logger.LogInfo($"The copy operation has finished...");
                        _logger.LogInfo($"The file queue has ben emptied.");
                    }
                });
            }

            if (!_fileNameQueue.Contains(fileName))
            {
                if (_options.Verbose || _options.Debug)
                {
                    _logger.LogInfo(
                        $"{fileName} has been added to the file queue and will be copied over in {_options.Delay} miliseconds.");
                }

                _fileNameQueue.Add(fileName);
            }
            else if (_options.Debug)
            {
                _logger.LogInfo($"{fileName} exists in the file queue, thereby skipped.");
            }
        }
    }
}