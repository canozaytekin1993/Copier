#region Header
// =====================================================================
// File Name                      : IOutputChannel.cs
// Date Created                 : 30/05/2019 - 08:42
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

namespace Client
{
    public interface ILogger
    {
        void Write(string message);
    }
}