using FluentValidation;
using ServicePhoto.WebApi.Models.Requests;

namespace ServicePhoto.WebApi.Validator
{
    public class PersonalPhotoValidator : AbstractValidator<PersonalPhotoRequest>
    {
        public PersonalPhotoValidator()
        {
            RuleFor(p => p.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId не заполнен.");
        }
    }
}
