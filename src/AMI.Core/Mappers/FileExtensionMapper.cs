using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AMI.Domain.Attributes;
using AMI.Domain.Enums;
using RNS.Framework.Tools;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// A mapper for file extensions.
    /// </summary>
    /// <seealso cref="IFileExtensionMapper" />
    public class FileExtensionMapper : IFileExtensionMapper
    {
        private IDictionary<string, FileExtensionMappingResult> results;
        private HashSet<string> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensionMapper"/> class.
        /// </summary>
        public FileExtensionMapper()
        {
            results = new Dictionary<string, FileExtensionMappingResult>();
            extensions = new HashSet<string>();
            Init();
        }

        /// <inheritdoc/>
        public FileExtensionMappingResult Map(string filename)
        {
            // The file format is DICOM by default to support files without extensions.
            var defaultResult = new FileExtensionMappingResult()
            {
                Extension = ".dcm",
                FileExtensionType = FileExtensionType.Default,
                FileFormat = FileFormat.Dicom,
                IsArchive = false
            };

            try
            {
                string extension = MapExtension(filename);

                if (results.TryGetValue(extension, out FileExtensionMappingResult result))
                {
                    return result;
                }

                return defaultResult;
            }
            catch (Exception)
            {
                return defaultResult;
            }
        }

        private void Init()
        {
            IEnumerable<FileFormat> fileFormats = EnumUtil.GetValues<FileFormat>();
            Type fileFormatType = typeof(FileFormat);
            foreach (FileFormat fileFormat in fileFormats)
            {
                MemberInfo member = fileFormatType.GetMember(fileFormat.ToString()).FirstOrDefault();
                FieldInfo field = fileFormatType.GetField(fileFormat.ToString());
                if (member != null && field != null)
                {
                    FileFormatExtensionAttribute[] attributes = member.GetCustomAttributes(typeof(FileFormatExtensionAttribute), false)
                        .OfType<FileFormatExtensionAttribute>().ToArray();

                    if (attributes != null)
                    {
                        foreach (FileFormatExtensionAttribute attribute in attributes)
                        {
                            FileExtensionMappingResult result = new FileExtensionMappingResult()
                            {
                                Extension = attribute.Extension,
                                FileExtensionType = attribute.FileExtensionType,
                                FileFormat = fileFormat,
                                IsArchive = field.IsDefined(typeof(ArchiveFileFormatAttribute), false)
                            };

                            results.Add(result.Extension, result);
                        }
                    }
                }
            }

            extensions = new HashSet<string>(results.Select(e => e.Key));
        }

        private string MapExtension(string filename)
        {
            return extensions.Where(filename.ToLowerInvariant().EndsWith).FirstOrDefault();
        }
    }
}
