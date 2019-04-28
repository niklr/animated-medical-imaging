using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AMI.NetCore.Tests")]
[assembly: InternalsVisibleTo("AMI.NetFramework.Tests")]
[assembly: InternalsVisibleTo("AMI.Portable")]

namespace AMI.Core.Helpers
{
    internal static class FileSystemHelper
    {
        internal static void ClearDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        internal static string BuildAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.GetFullPath(path);
            }
        }

        internal static string BuildCurrentPath(string folderName)
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
