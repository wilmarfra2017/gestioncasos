using FluentValidation;
using GestionCasos.Api.Dtos;

namespace GestionCasos.Api.Validators;
public class FileDetailsDtoValidator : AbstractValidator<FileDetailsDto>
{
    public FileDetailsDtoValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.ContentType).NotEmpty();
        RuleFor(x => x.File.Length).NotNull();
    }
}
