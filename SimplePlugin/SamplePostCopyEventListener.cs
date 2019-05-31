#region Header
// =====================================================================
// File Name                      : SamplePostCopyEventListener.cs
// Date Created                 : 30/05/2019 - 17:36
// Created User                 : Can Özaytekin
// =====================================================================
#endregion

using System;
using CopierPluginBase;

namespace SimplePlugin
{
    public class SamplePostCopyEventListener : IPostCopyEventListener
    {
        #region Implementation of IPostCopyEventListener

        // todo ignore abstract plugin classes by checking if the 
        public void OnPostCopy(string filePath)
        {
            Console.WriteLine("SamplePostCopyEventListener");
        }

        #endregion
    }
}