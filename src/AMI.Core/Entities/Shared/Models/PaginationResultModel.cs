using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the pagination result.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated list.</typeparam>
    public class PaginationResultModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationResultModel{T}"/> class.
        /// </summary>
        public PaginationResultModel()
        {
            Items = new List<T>();
            Pagination = new PaginationModel();
        }

        /// <summary>
        /// Gets or sets the list of items.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the pagination information.
        /// </summary>
        public PaginationModel Pagination { get; set; }

        /// <summary>
        /// Creates the pagination result based on the provided information.
        /// </summary>
        /// <typeparam name="U">The type of items in the list.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="limit">The limit to constrain the number of items.</param>
        /// <param name="total">The total amount of items.</param>
        /// <returns>The pagination result.</returns>
        public static PaginationResultModel<U> Create<U>(IEnumerable<U> items, int page, int limit, int total)
        {
            var result = new PaginationResultModel<U>()
            {
                Items = items ?? new List<U>(),
                Pagination = new PaginationModel()
                {
                    Page = page,
                    Limit = limit,
                    Total = total
                }
            };

            return result;
        }
    }
}
