﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SorterService.ClassLibrary
{
    public interface IFileSystemWorker
    {
        bool IsDirectoryExists(string path);
        bool IsFileExists(string path);
        void MoveFile(string sourcePath, string destinationPath);
        void CreateDirectory(string path);
    }
}
