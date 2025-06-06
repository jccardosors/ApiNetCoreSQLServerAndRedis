using FluentValidation;
using Project.Application.ViewModels;

namespace Project.Api.Validators
{
    public class EntryVMValidator : AbstractValidator<EntryVM>
    {
        public EntryVMValidator()
        {
            RuleFor(p => p.Description)
               .NotNull().WithMessage("Descrição do lançamento é obrigatório")
               .NotEmpty().WithMessage("Descrição do lançamento é obrigatório")
               .MinimumLength(2).WithMessage("Descrição do lançamento deve ter no mínimo 2 caracteres")
               .MaximumLength(100).WithMessage("Descrição do lançamento deve ter no máximo 100 caracteres");

            RuleFor(p => p.ValueEntry).NotNull().GreaterThan(0).WithMessage("Valor do lançamento deve ser maior que 0.");

        }
    }
}
