using System;

namespace IniWatcherLib
{
    public class IniChangedEventArgs : EventArgs
    {
        public string FilePath { get; }
        public IniFile NewContent { get; }

        public IniChangedEventArgs(string filePath, IniFile newContent)
        {
            FilePath = filePath;
            NewContent = newContent;
        }
    }
}
