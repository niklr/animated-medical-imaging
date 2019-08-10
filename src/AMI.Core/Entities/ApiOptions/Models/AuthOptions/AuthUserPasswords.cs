using System;
using System.Runtime.Serialization;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The passwords used to configure system users.
    /// </summary>
    [Serializable]
    public class AuthUserPasswords : IAuthUserPasswords
    {
        [NonSerialized]
        private string svcPassword;

        [NonSerialized]
        private string adminPassword;

        /// <inheritdoc/>
        [IgnoreDataMember]
        public string Svc
        {
            get
            {
                return svcPassword;
            }

            set
            {
                svcPassword = value;
            }
        }

        /// <inheritdoc/>
        [IgnoreDataMember]
        public string Admin
        {
            get
            {
                return adminPassword;
            }

            set
            {
                adminPassword = value;
            }
        }
    }
}
