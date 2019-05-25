using System;
using AMI.Core.Models;
using SharpCompress.Common;

namespace AMI.Compress.Mappers
{
    internal static class EntryMapper
    {
        internal static CompressedEntry Map(IEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return new CompressedEntry()
            {
                ArchivedTime = entry.ArchivedTime,
                CompressedSize = entry.CompressedSize,
                CreatedTime = entry.CreatedTime,
                IsDirectory = entry.IsDirectory,
                IsEncrypted = entry.IsEncrypted,
                Key = entry.Key,
                LastAccessedTime = entry.LastAccessedTime,
                LastModifiedTime = entry.LastModifiedTime,
                Size = entry.Size
            };
        }
    }
}
