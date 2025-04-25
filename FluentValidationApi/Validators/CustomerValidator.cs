using FluentValidation;
using FluentValidationApi.Models;
using System;

namespace FluentValidationApi.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(2, 50).WithMessage("O nome deve ter entre 2 e 50 caracteres");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("O sobrenome é obrigatório")
                .Length(2, 50).WithMessage("O sobrenome deve ter entre 2 e 50 caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("O email deve ser um endereço válido");

            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage("O telefone é obrigatório")
                .Matches(@"^\d{10,15}$").WithMessage("O telefone deve conter entre 10 e 15 dígitos");

            RuleFor(c => c.DateOfBirth)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória")
                .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser no passado")
                .Must(BeAtLeast18YearsOld).WithMessage("O cliente deve ter pelo menos 18 anos");

            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("O endereço é obrigatório")
                .MinimumLength(10).WithMessage("O endereço deve ter pelo menos 10 caracteres");
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Now.AddYears(-18);
        }
    }
} 