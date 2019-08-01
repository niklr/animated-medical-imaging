using System;
using System.Collections.Generic;
using AMI.Core.Constants;
using AMI.Core.Entities.Shared.Models;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the task.
    /// </summary>
    public class UserModel : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email address is confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Creates a model based on the given domain entity.
        /// </summary>
        /// <param name="entity">The domain entity.</param>
        /// <param name="constants">The application constants.</param>
        /// <returns>The domain entity as a model.</returns>
        public static UserModel Create(UserEntity entity, IApplicationConstants constants)
        {
            if (entity == null || constants == null)
            {
                return null;
            }

            var model = new UserModel
            {
                Id = entity.Id.ToString(),
                Username = entity.Username,
                Email = entity.Email,
                EmailConfirmed = entity.EmailConfirmed,
                Roles = string.IsNullOrWhiteSpace(entity.Roles) ?
                    Array.Empty<string>() : entity.Roles.Replace(constants.RoleNameSeparator, string.Empty).Split(',')
            };

            return model;
        }
    }
}
