using FluentValidation;
using Zamin.Extensions.Translations.Abstractions;

namespace Miniblog.Core.RequestResponse.Peaple.Commands.ChangeName;

public class ChangePersonNameValidator:AbstractValidator<ChangePersonName>
{
    public ChangePersonNameValidator(ITranslator translator)
    {
        RuleFor(p => p.FirstName)
            .NotNull()
            .WithMessage(translator["Required", "FirstName"])
            .MinimumLength(2)
            .WithMessage(translator["MinimumLength", "FirstName", "2"])
            .MaximumLength(50)
            .WithMessage(translator["MaximumLength", "FirstName", "50"]);
    }
}