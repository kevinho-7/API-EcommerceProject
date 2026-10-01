using API.DTOS;
using FluentValidation;

public class LoginValidation : AbstractValidator<LoginDto>
{
    public LoginValidation()
    {
        string invalidMsg = "Email ou Senha invalido";

        RuleFor(c => c.email)
            .NotEmpty()
            .WithMessage(invalidMsg)
            .EmailAddress()
            .WithMessage(invalidMsg);

        RuleFor(c => c.password)
            .NotEmpty()
            .WithMessage(invalidMsg);
    }
}