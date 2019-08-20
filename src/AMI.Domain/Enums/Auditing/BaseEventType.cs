namespace AMI.Domain.Enums.Auditing
{
    /// <summary>
    /// A type to describe a base event.
    /// </summary>
    public enum BaseEventType
    {
        /// <summary>
        /// The base event not representing any operation.
        /// </summary>
        None,

        /// <summary>
        /// The base event representing a create operation.
        /// </summary>
        Create,

        /// <summary>
        /// The base event representing a read operation.
        /// </summary>
        Read,

        /// <summary>
        /// The base event represeting an update operation.
        /// </summary>
        Update,

        /// <summary>
        /// The base event represeting a delete operation.
        /// </summary>
        Delete
    }
}
