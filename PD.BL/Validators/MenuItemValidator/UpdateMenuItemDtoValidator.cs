using Dtos.MenuItemDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.MenuItemValidator
{
    public class UpdateMenuItemDtoValidator : AbstractValidator<UpdateMenuItemDto>
    {
        public UpdateMenuItemDtoValidator() {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Menu Item Name is required.")
               .MaximumLength(100).WithMessage("Menu Item Name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");
            RuleFor(x => x.Ingeredents)
                .MaximumLength(500).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("You must add photo url");
        }
    }
}
