using System.Security.Cryptography.X509Certificates;
using API.Models;
using FluentValidation;

public class ProductValidator : AbstractValidator<Product>
{
    private static readonly HashSet<string?> ValidExtensions = [".png", ".jpg", ".jpeg"];

    public ProductValidator()
    {
        ValidationConstants vc = new ValidationConstants();

        RuleFor(p => p.title)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotNullMsg());

        RuleFor(p => p.description)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotNullMsg());

        RuleFor(p => p.price)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotNullMsg())
            .GreaterThan(0)
            .WithMessage("O preço tem que ser maior que 0")
            .PrecisionScale(10, 2, false)
            .WithMessage(vc.InvalidFormatMsg());

        RuleFor(p => p.image_path)
            .Custom((path, context) => {
                var extension = Path.GetExtension(path)?.ToLower();

                if (!ValidExtensions.Contains(extension))
                {
                    context.AddFailure(vc.InvalidFormatMsg());
                }
                
            });

        RuleFor(p => p.quantity)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotNullMsg())
            .GreaterThan(1)
            .WithMessage("Valor Invalido")        
            .LessThan(999999)
            .WithMessage("Valor Invalido");

        RuleFor(p => p.category_id)
            .NotNull()
            .NotEmpty()
            .WithMessage(vc.NotNullMsg());

        RuleFor(p => p.company_id)
            .NotNull()
            .NotEmpty()    
            .WithMessage(vc.NotNullMsg());
    }
}