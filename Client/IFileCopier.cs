#region Header
// =====================================================================
// File Name                      : IFileCopier.cs
// Date Created                 : 30/05/2019 - 08:22
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

using System.IO;

namespace Client
{
    public interface IFileCopier
    {
        void CopyFile(string fileName);
    }
}