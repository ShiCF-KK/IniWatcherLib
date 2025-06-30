using System;
using System.IO;
using System.Threading;

namespace IniWatcherLib
{
    public class IniWatcher : IIniWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly Timer _debounceTimer;
        //private readonly object _lock = new();
        private readonly object _lock = new object();
        private IniFile _current;
        private bool _changed;

        public string FilePath { get; }
        public IniFile Current => _current;

        public event EventHandler<IniChangedEventArgs> Changed;

        public IniWatcher(string filePath)
        {
            FilePath = Path.GetFullPath(filePath);
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(FilePath), Path.GetFileName(FilePath))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };

            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = false;

            _debounceTimer = new Timer(_ => ReloadIfChanged(), null, Timeout.Infinite, Timeout.Infinite);

            if (File.Exists(FilePath))
            {
                _current = IniFile.Parse(File.ReadAllText(FilePath));
            }
        }

        public void Start() => _watcher.EnableRaisingEvents = true;

        public void Stop() => _watcher.EnableRaisingEvents = false;

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                _changed = true;
                _debounceTimer.Change(500, Timeout.Infinite);
            }
        }

        private void ReloadIfChanged()
        {
            lock (_lock)
            {
                if (!_changed) return;
                _changed = false;
            }

            try
            {
                var content = File.ReadAllText(FilePath);
                var parsed = IniFile.Parse(content);
                _current = parsed;
                Changed?.Invoke(this, new IniChangedEventArgs(FilePath, parsed));
            }
            catch
            {
                // swallow or log error
            }
        }

        public void Dispose()
        {
            _watcher.Dispose();
            _debounceTimer.Dispose();
        }
    }
}
