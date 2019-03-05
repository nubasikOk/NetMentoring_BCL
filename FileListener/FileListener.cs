using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SorterService.ClassLibrary
{
    public class FileListener
    {
        private readonly string defaultDestinationDirectoryPath;
        private readonly IFileSystemWorker fileSystemWorker;
        private IDictionary<Regex, string> rules;
        private Func<FileRelocationInfo, string> generateNewFileName;

        public event EventHandler<RuleFoundEventArgs> RuleFound;
        public event EventHandler<RuleNotFoundEventArgs> RuleNotFound;
        public IDirectoryWorker directoryWorker { get; }
        
        public FileListener(IDirectoryWorker directoryWorker, string defaultDestinationDirectoryPath, IFileSystemWorker fileSystemWorker)
        {
            this.directoryWorker = directoryWorker ?? throw new ArgumentNullException();
            this.defaultDestinationDirectoryPath = defaultDestinationDirectoryPath ?? throw new ArgumentNullException();
            this.fileSystemWorker = fileSystemWorker ?? throw new ArgumentNullException();

            if (directoryWorker.Path == null)
            {
                throw new ArgumentNullException();
            }

            if (!this.fileSystemWorker.IsDirectoryExists(directoryWorker.Path))
            {
                throw new ArgumentOutOfRangeException();
            }

            directoryWorker.Created += OnCreated;
            generateNewFileName = fileRelocationInfo => fileRelocationInfo.FileName;
            rules = new Dictionary<Regex, string>();
        }


        protected virtual void OnRuleFound(RuleFoundEventArgs e)
        {
            RuleFound?.Invoke(this, e);
        }
        protected  virtual void OnRuleNotFound(RuleNotFoundEventArgs e)
        {
            RuleNotFound?.Invoke(this, e);
        }

        
        protected virtual void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (!fileSystemWorker.IsFileExists(e.FullPath))
            {
                return;
            }

            MoveFile(new FileRelocationInfo { FileName = e.Name, DestinationPath = GetPathToMove(e.Name) });
        }
        public IDictionary<Regex, string> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException();
        }
        public Func<FileRelocationInfo, string> GenerateNewFileName
        {
            get => generateNewFileName;
            set => generateNewFileName = value ?? throw new ArgumentNullException();
        }
        private void MoveFile(FileRelocationInfo fileRelocationInfo)
        {
            var destinationPath = fileRelocationInfo.DestinationPath;
            if (!fileSystemWorker.IsDirectoryExists(destinationPath))
            {
                fileSystemWorker.CreateDirectory(destinationPath);
            }

            var newFileName = GenerateNewFileName(fileRelocationInfo);
            var targetPath = Path.Combine(destinationPath, newFileName);

            if (fileSystemWorker.IsFileExists(targetPath))
            {
                targetPath = Path.Combine(destinationPath, $"{Guid.NewGuid()}_{newFileName}");
            }

            var sourcePath = Path.Combine(directoryWorker.Path, fileRelocationInfo.FileName);
            fileSystemWorker.MoveFile(sourcePath, targetPath);
        }
        private string GetPathToMove(string fileName)
        {
            foreach (var rule in Rules)
            {
                var filePattern = rule.Key;

                if (!filePattern.IsMatch(fileName))
                {
                    continue;
                }

                OnRuleFound(new RuleFoundEventArgs
                {
                    PathToMove = rule.Value,
                    Rule = filePattern,
                    FileName = fileName,
                    CreatedOn = DateTimeOffset.Now
                });

                return rule.Value;
            }

            OnRuleNotFound(new RuleNotFoundEventArgs
            {
                FileName = fileName,
                DefaultPath = defaultDestinationDirectoryPath,
                CreatedOn = DateTimeOffset.Now
            });

            return defaultDestinationDirectoryPath;
        }
    }
}
