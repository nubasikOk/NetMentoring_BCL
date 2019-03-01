using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileListener
{
    class DirectoryWorker
    {
        private readonly FileSystemWatcher fileSystemWatcher;
        public event FileSystemEventHandler Created
        {
            add => fileSystemWatcher.Created += value;
            remove => fileSystemWatcher.Created -= value;
        }
        public event FileSystemEventHandler Deleted
        {
            add => fileSystemWatcher.Deleted += value;
            remove => fileSystemWatcher.Deleted -= value;
        }
        public event FileSystemEventHandler Changed
        {
            add => fileSystemWatcher.Changed += value;
            remove => fileSystemWatcher.Changed -= value;
        }
        protected DirectoryWorker() { }
        public DirectoryWorker(string path)
        {
            fileSystemWatcher = new FileSystemWatcher(path);
        }
        public bool IncludeSubdirectories { get => fileSystemWatcher.IncludeSubdirectories; set => fileSystemWatcher.IncludeSubdirectories = value; }
        public bool EnableRaisingEvents { get => fileSystemWatcher.EnableRaisingEvents; set => fileSystemWatcher.EnableRaisingEvents = value; }
        public string Path => fileSystemWatcher.Path;
    }
}
