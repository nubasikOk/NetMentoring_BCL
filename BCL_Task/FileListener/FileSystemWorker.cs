
using System.IO;

namespace FileListener
{
    class FileSystemWorker
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
        public bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        public bool IsFileExists(string path)
        {
            return File.Exists(path);
        }
        public void MoveFile(string sourcePath, string destinationPath)
        {
            Directory.Move(sourcePath, destinationPath);
        }
    }
}
