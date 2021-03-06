﻿using System;
using FluentValidation;
using JetBrains.Annotations;
using WebApiBoilerplate.Framework.Protocol;

namespace WebApiBoilerplate.Core.Protocol
{
    /// <summary>
    /// Short user data
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Global User Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User's main name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }
    }

    /// <summary>
    /// Full user data
    /// </summary>
    public class User
    {
        /// <summary>
        /// Global User Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User's main name
        /// </summary>
        public string Login { get; set; }

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
        /// Time of user creation
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// List user request filters
    /// </summary>
    public class ListUserRequest: IPagedRequest
    {
        /// <summary>
        /// First or last name first letters
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Maximum coun of users in a response
        /// </summary>
        public int? PageSize { get; set; } = 50;

        /// <summary>
        /// Page shifting in responses
        /// </summary>
        public int? PageIndex { get; set; } = 0;

        /// <summary>
        /// Validator
        /// </summary>
        public class Validator : AbstractValidator<CreateUserRequest>
        { }
    }

    /// <summary>
    /// Get full user data
    /// </summary>
    public class GetUserRequest
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Request for creation of new user
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// User's login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        public class Validator : AbstractValidator<CreateUserRequest>
        {
            /// <summary>
            /// Default
            /// </summary>
            public Validator()
            {
                RuleFor(request => request).NotNull();
                RuleFor(request => request.Login).NotEmpty().MaximumLength(50);
                RuleFor(request => request.FirstName).MaximumLength(50);
                RuleFor(request => request.LastName).MaximumLength(50);
                RuleFor(request => request.Email).MaximumLength(50).NotEmpty().EmailAddress();
            }
        }
    }

    /// <summary>
    /// Request for updaing exists user
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Global User Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User's login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        public class Validator : AbstractValidator<UpdateUserRequest>
        {
            /// <summary>
            /// Default
            /// </summary>
            public Validator()
            {
                RuleFor(request => request).NotNull();
                RuleFor(request => request.Id).GreaterThan(0);
                RuleFor(request => request.Login).NotEmpty().MaximumLength(50);
                RuleFor(request => request.FirstName).MaximumLength(50);
                RuleFor(request => request.LastName).MaximumLength(50);
                RuleFor(request => request.Email).MaximumLength(50).NotEmpty().EmailAddress();
            }
        }
    }

    /// <summary>
    /// Request for removing exists user
    /// </summary>
    public class RemoveUserRequest
    {
        /// <summary>
        /// Global User Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        public class Validator : AbstractValidator<RemoveUserRequest>
        {
            /// <summary>
            /// Default
            /// </summary>
            public Validator()
            {
                RuleFor(request => request).NotNull();
                RuleFor(request => request.Id).GreaterThan(0);
            }
        }
    }
}
