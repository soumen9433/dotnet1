namespace HazGo.Infrastructure.Services
{
    using HazGo.Application.Common.Interfaces;
    using System;
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
