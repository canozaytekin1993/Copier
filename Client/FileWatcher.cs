using System;
using System.IO;

namespace Client
{
    class FileWatcher : IFileWatcher
    {
        private readonly IFileCopier _fileCopier;
        private readonly ILogger _logger;

        public FileWatcher(IFileCopier fileCopier, ILogger logger)
        {
            _fileCopier = fileCopier;
            _logger = logger;
        }

        #region Implementation of IFileWatcher

        public void Watch(CommandOptions options)
        {
            var watcher = new FileSystemWatcher
            {
                Path = options.SourceDirectoryPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = options.FileGlobPattern
            };

            watcher.Changed += (sender, args) =>
            {
                if (args.ChangeType != WatcherChangeTypes.Changed) return;

                if (options.Verbose)
                {
                    _logger.Write($"{args.Name} file has changed.");
                }

                _fileCopier.CopyFile(options,args.Name);
            };

            watcher.Renamed += (sender, args) =>
            {
                if (options.Verbose)
                {
                    _logger.Write($"{args.OldName} has been renamed.");
                }

                _fileCopier.CopyFile(options,args.Name);
            };

            watcher.EnableRaisingEvents = true;
        }

        #endregion
    }
}