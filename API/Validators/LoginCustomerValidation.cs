using FluentValidation;

public class LoginCustomerValidation : AbstractValidator<Customer>
{
    public LoginCustomerValidation()
    {
        string invalidMsg = "Email ou Senha invalido";

        RuleFor(c => c.email)
            .NotEmpty()
            .WithMessage(invalidMsg)
            .EmailAddress()
            .WithMessage(invalidMsg);

        RuleFor(c => c.password_hash)
            .NotEmpty()
            .WithMessage(invalidMsg);
    }
}