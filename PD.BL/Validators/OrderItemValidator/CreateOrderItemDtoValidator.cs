using Dtos.OrderItemDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.OrderItemValidator
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.MenuItemId)
               .NotEmpty().WithMessage("Menu Item Id is required.");
            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.OrderId)
               .NotEmpty().WithMessage("Order Id is required.");
            RuleFor(x => x.Note)
                .MaximumLength(500);
        }
    }
}
