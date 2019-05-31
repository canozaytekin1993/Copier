using System;
using CopierPluginBase;

namespace SimplePlugin
{
    public class SamplePreCopyEventListener : IPreCopyEventListener
    {
        #region Implementation of IPreCopyEventListener

        public void OnPreCopy(string filePath)
        {
            Console.WriteLine("SamplePreCopyEventListener");
        }

        #endregion
    }
}