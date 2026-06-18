using FluentValidation;

public class ReqLoginValidation : AbstractValidator<JwtClaimsData>
{
    public ReqLoginValidation()
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