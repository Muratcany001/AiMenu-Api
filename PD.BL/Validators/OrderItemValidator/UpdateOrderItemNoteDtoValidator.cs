using Dtos.OrderItemDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.OrderItemValidator
{
    public class UpdateOrderItemNoteDtoValidator : AbstractValidator<UpdateOrderItemNoteDto>
    {
        public UpdateOrderItemNoteDtoValidator()
        {
            RuleFor(x => x.OrderItemId)
               .NotEmpty().WithMessage("Order Item Id is required.");
            RuleFor(x => x.Note)
               .MaximumLength(500).WithMessage("Note cannot exceed 500 characters.");
        }
    }
}
