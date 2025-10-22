using Dtos.UserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Validators.UserValidator
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {


        public UpdateUserDtoValidator() {

            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.");
        }
    }
}
