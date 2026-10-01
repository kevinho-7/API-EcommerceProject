using API.DTOS;
using FluentValidation;

public class UpdateProfileAddressValidator : AbstractValidator<UpdateProfileAddressDto>
{
    public UpdateProfileAddressValidator()
    {
        RuleFor(a => a.street)
            .MaximumLength(100);

        RuleFor(a => a.neighborhood)
            .MaximumLength(100);

        RuleFor(a => a.complement)
            .MaximumLength(100);

        RuleFor(a => a.city)
            .MaximumLength(100);

        RuleFor(a => a.state)
            .MaximumLength(2);
    }
}