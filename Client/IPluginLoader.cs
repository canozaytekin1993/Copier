#region Header
// =====================================================================
// File Name                      : IPluginLoader.cs
// Date Created                 : // - 
// Created User                 : Can Özaytekin
// =====================================================================
#endregion
namespace Client
{
    public interface IPluginLoader
    {
        void Subscribe(IPreCopyEventBroadcaster pre, IPostCopyEventBroadcaster post);
    }
}