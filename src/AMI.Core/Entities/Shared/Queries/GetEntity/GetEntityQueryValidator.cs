using System;
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
            RuleFor(x => x.Id).NotEmpty().Must(x =>
            {
                return Guid.TryParse(x, out Guid result);
            });
        }
    }
}
