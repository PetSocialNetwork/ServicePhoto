using FluentValidation;
using ServicePhoto.WebApi.Models.Requests;

namespace ServicePhoto.WebApi.Validator
{
    public class PetMainPhotoRequestValidator : AbstractValidator<PetMainPhotoRequest>
    {
        public PetMainPhotoRequestValidator()
        {
            RuleFor(p => p.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId не заполнен.");

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithMessage("PetId не заполнен.");

            RuleFor(p => p.PhotoId)
                .NotEmpty()
                .WithMessage("PhotoId не заполнен.");
        }
    }
}
