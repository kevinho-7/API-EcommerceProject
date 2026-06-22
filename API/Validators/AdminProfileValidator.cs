using FluentValidation;

public class AdminProfileValidator : AbstractValidator<AdminUpdateProfileDto>
{
    public AdminProfileValidator()
    {
        ValidationConstants vc = new ValidationConstants();

        RuleFor(a => a.street)
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(10)
            .WithMessage(vc.InvalidFormatMsg())
            .MaximumLength(50)
            .WithMessage(vc.InvalidFormatMsg())
            .Must(s => 
                s.All(c => 
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-'))
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(a => a.neighborhood)
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(10)
            .WithMessage(vc.InvalidFormatMsg())
            .MaximumLength(50)
            .WithMessage(vc.InvalidFormatMsg())
            .Must(n => 
                n.All(c => 
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-'))
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(a => a.complement)
            .MinimumLength(4)
            .WithMessage(vc.InvalidFormatMsg())
            .MaximumLength(50)
            .WithMessage(vc.InvalidFormatMsg())
            .Must(c => 
                c.All(c => 
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-'))
            .WithMessage(vc.InvalidFormatMsg());
            
        RuleFor(a => a.city)
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(4)
            .WithMessage(vc.InvalidFormatMsg())
            .MaximumLength(20)
            .WithMessage(vc.InvalidFormatMsg())
            .Must(c => 
                c.All(c => 
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c) ||
                    c == '-'))
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(a => a.state)
            .NotEmpty()
            .WithMessage(vc.NotEmptyMsg())
            .MinimumLength(2)
            .WithMessage(vc.InvalidFormatMsg())
            .MaximumLength(30)
            .WithMessage(vc.InvalidFormatMsg())
            .Must(c => 
                c.All(c => 
                    char.IsLetter(c) ||
                    char.IsWhiteSpace(c)))
            .WithMessage(vc.InvalidFormatMsg());
    }
}