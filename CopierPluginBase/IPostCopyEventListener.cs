#region Header
// =====================================================================
// File Name                      : PostCopyEventListener.cs
// Date Created                 : 30/05/2019 - 11:42
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

namespace CopierPluginBase
{
    public interface IPostCopyEventListener
    {
        void OnPostCopy(string filePath);
    }
}