using Dtos.OrderDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.OrderValidator
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(x => x.TableNumber)
                .NotEmpty().WithMessage("Table number is required.")
                .MaximumLength(10).WithMessage("Table number cannot exceed 10 characters.");
        }
    }
}
