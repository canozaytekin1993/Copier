#region Header
// =====================================================================
// File Name                      : IOutputChannel.cs
// Date Created                 : 30/05/2019 - 08:42
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

using System;

namespace Client
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogDebug(string message);
    }
}