using AMI.Core.Entities.Shared.Models;
using FluentValidation;

namespace AMI.Core.Entities.Shared.Queries.GetEntity
{
    /// <summary>
    /// A validator for queries to get an entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class GetEntityQueryValidator<T> : AbstractValidator<T>
        where T : IGetEntityQuery<IEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntityQueryValidator{T}"/> class.
        /// </summary>
        public GetEntityQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().Must(x =>
            {
                if (long.TryParse(x, out long result))
                {
                    return result > 0;
                }
                return false;
            });
        }
    }
}
