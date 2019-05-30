using System;

namespace Client
{
    class ConsoleLogger : ILogger
    {
        #region Implementation of IOutputChannel

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        #endregion
    }
}