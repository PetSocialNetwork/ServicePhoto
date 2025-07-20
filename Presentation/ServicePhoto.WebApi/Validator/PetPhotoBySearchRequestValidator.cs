using FluentValidation;
using ServicePhoto.WebApi.Models.Requests;

namespace ServicePhoto.WebApi.Validator
{
    public class PetPhotoBySearchRequestValidator : AbstractValidator<PetPhotoBySearchRequest>
    {
        public PetPhotoBySearchRequestValidator()
        {
            RuleFor(p => p.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId не заполнен.");

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithMessage("PetId не заполнен.");
        }
    }
}
