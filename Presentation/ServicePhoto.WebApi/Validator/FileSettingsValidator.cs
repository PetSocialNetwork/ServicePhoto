using FluentValidation;
using ServicePhoto.FileStorage.Configurations;

namespace ServicePhoto.WebApi.Validator
{
    public class FileSettingsValidator : AbstractValidator<FileSettings>
    {
        public FileSettingsValidator()
        {
            RuleFor(x => x.WebRootPath)
                .NotEmpty().WithMessage("WebRootPath не может быть пустым.")
                .Must(Directory.Exists).WithMessage("Указанный путь WebRootPath не существует.");

            RuleFor(x => x.PhotoBaseUrl)
                .NotEmpty().WithMessage("PhotoBaseUrl не может быть пустым.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("PhotoBaseUrl должно быть допустимым абсолютным URL.");

            RuleFor(x => x.PhotoDirectory)
                .NotEmpty().WithMessage("PhotoDirectory не может быть пустым.");
        }
    }
}
