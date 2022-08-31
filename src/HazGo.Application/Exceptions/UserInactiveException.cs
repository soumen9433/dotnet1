namespace HazGo.Application.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation.Results;

    /// <summary>
    /// UserInactiveException.
    /// </summary>
    public class UserInactiveException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInactiveException"/> class.
        /// </summary>
        public UserInactiveException()
            : base("User is inactive.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInactiveException"/> class.
        /// </summary>
        /// <param name="failures">failures.</param>
        public UserInactiveException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            this.Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
