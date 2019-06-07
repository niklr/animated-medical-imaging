using System;
using AMI.Core.Entities.Models;
using SharpCompress.Common;

namespace AMI.Compress.Mappers
{
    internal static class EntryMapper
    {
        internal static CompressedEntryModel Map(IEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return new CompressedEntryModel()
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
