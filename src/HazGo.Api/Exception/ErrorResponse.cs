namespace HazGo.Api.Exception
{
    using System.Collections.Generic;
    using HazGo.BuildingBlocks.Core.Common;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorResponse : ProblemDetails
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
