namespace HazGo.Domain.Entities
{
    using MediatR;

    public class CreateCityResponse : IRequest
    {
        public string TradingProviderCode { get; set; }

        public int TradingProviderId { get; set; }

        public string TradingProviderErrorMessage { get; set; }
    }
}
