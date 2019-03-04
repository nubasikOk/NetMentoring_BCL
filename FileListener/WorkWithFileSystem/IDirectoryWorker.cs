using System.IO;

namespace FileListener
{
    public interface IDirectoryWorker
    {
        event FileSystemEventHandler Created;
        event FileSystemEventHandler Deleted;
        event FileSystemEventHandler Changed;
        bool IncludeSubdirectories { get; set; }
        bool EnableRaisingEvents { get; set; }
        string Path { get; }
    }
}