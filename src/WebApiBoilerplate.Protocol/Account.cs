using System;
using FluentValidation;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Protocol
{
    /// <summary>
    /// Request to authenticate on server
    /// </summary>
    public class SignInAccountRequest
    {
        /// <summary>
        /// User's login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sets persistent cookies and long term JWT token
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        [UsedImplicitly]
        public class Validator: AbstractValidator<SignInAccountRequest>
        {
            /// <summary>
            /// Validator
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Login).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
            }
        }
    }

    /// <summary>
    /// Request to create new account on server
    /// </summary>
    public class SignUpAccountRequest
    {
        /// <summary>
        /// User's login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        [UsedImplicitly]
        public class Validator : AbstractValidator<SignUpAccountRequest>
        {
            /// <summary>
            /// Validator
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Login).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
                RuleFor(x => x.FirstName).MaximumLength(50);
                RuleFor(x => x.LastName).MaximumLength(50);
            }
        }
    }

    /// <summary>
    /// Error on account locked
    /// </summary>
    public class AccountLockedError: Error
    {
        /// <summary>
        /// End of locked expiration
        /// </summary>
        public DateTimeOffset? LockedEndDate { get; set; }
    }
}
