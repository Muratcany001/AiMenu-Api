using Dtos.OrderDto;
using Dtos.OrderItemDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.OrderItemValidator
{
    public class SetQuantityValidator : AbstractValidator<SetQuantityDto>
    {
        public SetQuantityValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
