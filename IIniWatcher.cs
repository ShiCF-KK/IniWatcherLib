using System;

namespace IniWatcherLib
{
    public interface IIniWatcher : IDisposable
    {
        string FilePath { get; }
        IniFile Current { get; }
        event EventHandler<IniChangedEventArgs> Changed;
        void Start();
        void Stop();
    }
}
