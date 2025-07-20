using FluentValidation;
using ServicePhoto.WebApi.Models.Requests;

namespace ServicePhoto.WebApi.Validator
{
    public class PersonalPhotoBySearchValidator : AbstractValidator<PersonalPhotoBySearchRequest>
    {
        public PersonalPhotoBySearchValidator()
        {
            RuleFor(p => p.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId не заполнен.");
        }
    }
}
