#region Header
// =====================================================================
// File Name                      : PreCopyEventListener.cs
// Date Created                 : 30/05/2019 - 11:43
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

namespace CopierPluginBase
{
    public interface IPreCopyEventListener
    {
        void OnPreCopy(string filePath);
    }
}