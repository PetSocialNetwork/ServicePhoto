using FluentValidation;
using ServicePhoto.WebApi.Models.Requests;

namespace ServicePhoto.WebApi.Validator
{
    public class AddPersonalPhotoValidator : AbstractValidator<AddPersonalPhotoRequest>
    {
        public AddPersonalPhotoValidator()
        {
            RuleFor(p => p.FileBytes)
                .NotEmpty()
                .WithMessage("Файл не передан.")
                .Must(bytes => bytes.Length > 0)
                .WithMessage("Файл не содержит данных.");

            RuleFor(p => p.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId не заполнен.");

            RuleFor(p => p.OriginalFileName)
               .NotEmpty()
               .WithMessage("OriginalFileName не заполнен.");
        }
    }
}
