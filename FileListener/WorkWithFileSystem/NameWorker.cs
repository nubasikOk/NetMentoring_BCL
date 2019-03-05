using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Config = SorterService.Configuration.SorterServiceConfiguration;

namespace SorterService.ClassLibrary
{
    public static class NameWorker
    {

        public static string GetFileNameWithoutFolders(string path) => Path.GetFileName(path);
        public static string GetPathWithoutFile(string path) => Path.GetDirectoryName(path);


        public static string GenerateNewFileName(FileRelocationInfo fileRelocationInfo)
        {
            var newFileName = fileRelocationInfo.FileName;
            if (Config.Configuration.Rules.EnableCreateDateAddition)
            {
                var dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
                var date = DateTime.Now.ToString("G", dateTimeFormat);
                var separatorList = date.Where(x => !char.IsNumber(x)).ToList();
                date = separatorList.Aggregate(date, (current, separator) => current.Replace(separator, '_'));
                newFileName = $"{date}_{newFileName}";
            }
            if (!Config.Configuration.Rules.EnableAddFileIndex)
            {
                return newFileName;
            }

            var fileCount = Directory.GetFiles(fileRelocationInfo.DestinationPath).Length;
            newFileName = $"{++fileCount}_{newFileName}";

            return newFileName;
        }
    }
}
