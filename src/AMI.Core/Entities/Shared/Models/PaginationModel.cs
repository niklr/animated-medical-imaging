namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the pagination.
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// Gets or sets the limit to constrain the number of items.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the total amount of items.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; }
    }
}
