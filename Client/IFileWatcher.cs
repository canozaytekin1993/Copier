#region Header

// =====================================================================
// File Name                      : IFileWatcher.cs
// Date Created                 : 30/05/2019 - 08:30
// Created User                 : Can Özaytekin
// =====================================================================

#endregion

namespace Client
{
    public interface IFileWatcher
    {
        void Watch(CommandOptions options);
    }
}
