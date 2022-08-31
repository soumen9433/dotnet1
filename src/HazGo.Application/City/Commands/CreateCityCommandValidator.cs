using FluentValidation;

namespace HazGo.Application.Cities.Commands
{
    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(v => v.CityDto.Name)
           .NotEmpty().WithName("City Name").Length(3, 100);
        }
    }
}
