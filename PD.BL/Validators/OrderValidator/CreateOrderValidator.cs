using Dtos.OrderDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.OrderValidator
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.TableNumber)
               .NotEmpty().WithMessage("Table Number is required.");

            RuleFor(x => x.Notes)
               .NotEmpty().WithMessage("Notes is required.");
            RuleFor(x => x.OrderItems)
               .NotEmpty().WithMessage("Order Items is required.");

        }
    }
}
