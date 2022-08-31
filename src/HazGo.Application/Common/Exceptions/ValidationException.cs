using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FluentValidation.Results;

namespace HazGo.Application.Common.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException()
             : base("One or more validation failures have occurred.")
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            this.Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(ValidationFailure failure)
          : this()
        {
            Errors = new Dictionary<string, string[]>();
            Errors.Add(failure.PropertyName, new string[] { failure.ErrorMessage });
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
