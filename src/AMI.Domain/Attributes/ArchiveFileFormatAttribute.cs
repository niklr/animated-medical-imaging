using System;

namespace AMI.Domain.Attributes
{
    /// <summary>
    /// An attribute used to annotate enums representing archive file formats.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public class ArchiveFileFormatAttribute : Attribute
    {
    }
}
