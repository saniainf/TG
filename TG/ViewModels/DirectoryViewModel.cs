using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TG.ViewModels.Base;

namespace TG.ViewModels
{
    class DirectoryViewModel : ViewModel
    {
        private readonly DirectoryInfo directoryInfo;

        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return directoryInfo
                          .EnumerateDirectories()
                          .Select(dirInfo => new DirectoryViewModel(dirInfo.FullName));
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }

                return Enumerable.Empty<DirectoryViewModel>();
            }
        }

        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    var files = directoryInfo
                               .EnumerateFiles()
                               .Select(fileInfo => new FileViewModel(fileInfo.FullName));
                    return files;
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<FileViewModel>();
            }
        }

        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    return SubDirectories.Cast<object>().Concat(Files);
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<object>();
            }
        }

        public string Name => directoryInfo.Name;

        public string Path => directoryInfo.FullName;

        public DateTime CreationDate => directoryInfo.CreationTime;

        public DirectoryViewModel(string path) => directoryInfo = new DirectoryInfo(path);
    }

    class FileViewModel : ViewModel
    {
        private readonly FileInfo fileInfo;

        public string Name => fileInfo.Name;

        public string Path => fileInfo.FullName;

        public DateTime CreationDate => fileInfo.CreationTime;

        public FileViewModel(string path) => fileInfo = new FileInfo(path);
    }
}
