using FluentValidation;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        ValidationConstants vc = new ValidationConstants();

        RuleFor(u => u.first_name)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(2)
            .WithMessage("O nome deve conter no minimo 2 caracteres")
            .MaximumLength(50)
            .WithMessage("O nome deve conter no maximo 50 caracteres")
            .Must(firstName => 
                firstName.All(c =>
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-' ||
                    c == '\''))
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(u => u.last_name)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(2)
            .WithMessage("O sobrenome deve conter no minimo 2 caracteres")
            .MaximumLength(100)
            .WithMessage("O sobrenome deve conter no maximo 100 caracteres")
            .Must(lastName => 
                lastName.All(c =>
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-' ||
                    c == '\''))
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(u => u.email)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .EmailAddress();

        RuleFor(u => u.password)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(8)
            .WithMessage("A senha deve ter pelo menos 8 caracteres")
            .MaximumLength(100)
            .WithMessage("A senha deve ter no máximo 100 caracteres")
            .Matches(@"[A-Z]")
            .WithMessage("A senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]")
            .WithMessage("A senha deve conter pelo menos uma letra minúscula")
            .Matches(@"\d")
            .WithMessage("A senha deve conter pelo menos um número")
            .Matches(@"[!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]")
            .WithMessage("A senha deve conter pelo menos um caractere especial");
    }
}