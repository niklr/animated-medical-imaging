﻿using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AMI.CLI")]
[assembly: InternalsVisibleTo("AMI.NetCore.Tests")]
[assembly: InternalsVisibleTo("AMI.NetFramework.Tests")]
[assembly: InternalsVisibleTo("AMI.Portable")]

namespace AMI.Core.Helpers
{
    internal static class FileSystemHelper
    {
        private const string RootFolderName = "animated-medical-imaging";

        internal static string BuildCurrentPath(string folderName = RootFolderName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return path;
            }
            else
            {
                return TraversePathRecursive(path, folderName);
            }
        }

        private static string TraversePathRecursive(string path, string folderName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            if (path.Split(Path.DirectorySeparatorChar).Length <= 0)
            {
                throw new ArgumentException("The current path does not contain a folder with the provided name.");
            }

            if (new DirectoryInfo(path).Name.Equals(folderName))
            {
                return path;
            }
            else
            {
                return TraversePathRecursive(Directory.GetParent(path).FullName, folderName);
            }
        }
    }
}
