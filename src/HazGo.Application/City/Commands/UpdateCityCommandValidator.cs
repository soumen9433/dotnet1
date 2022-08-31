using FluentValidation;

namespace HazGo.Application.Cities.Commands
{
    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(v => v.CityDto.Name).Length(3, 100)
           .NotEmpty().WithName("City Name");
        }
    }
}
