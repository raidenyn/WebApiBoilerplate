﻿using System;
using System.Collections.Generic;

namespace WebApiBoilerplate.Protocol
{
    /// <summary>
    /// Server error description
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Unique ID of current error
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        /// <summary>
        /// Code of error type
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Human readable error description
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Error of user validation
    /// </summary>
    public class ValidationError: Error
    {
        /// <summary>
        /// Validation messages
        /// </summary>
        public List<string> Validations { get; set; }
    }
}
