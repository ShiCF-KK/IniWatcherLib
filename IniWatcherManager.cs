using System;
using System.Collections.Generic;

namespace IniWatcherLib
{
    public class IniWatcherManager : IDisposable
    {
        //private readonly Dictionary<string, IIniWatcher> _watchers = new();
        private readonly Dictionary<string, IIniWatcher> _watchers = new Dictionary<string, IIniWatcher>();


        public event EventHandler<IniChangedEventArgs> FileChanged;

        public bool Add(string path)
        {
            var fullPath = System.IO.Path.GetFullPath(path);
            if (_watchers.ContainsKey(fullPath))
                return false;

            var watcher = new IniWatcher(fullPath);
            watcher.Changed += (_, e) => FileChanged?.Invoke(this, e);
            _watchers[fullPath] = watcher;
            watcher.Start();
            return true;
        }

        public bool Remove(string path)
        {
            var fullPath = System.IO.Path.GetFullPath(path);
            if (_watchers.TryGetValue(fullPath, out var watcher))
            {
                watcher.Dispose();
                _watchers.Remove(fullPath);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            foreach (var w in _watchers.Values)
                w.Dispose();
            _watchers.Clear();
        }
    }
}
