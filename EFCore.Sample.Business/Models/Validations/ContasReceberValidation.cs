using FluentValidation;

namespace EFCore.Sample.Business.Models.Validations
{
    public class ContasReceberValidation  : AbstractValidator<ContasReceber>
    {
        public ContasReceberValidation() {
            RuleFor(c => c.CnpjCpf)
                .NotEmpty().WithMessage("O campo {PropertyName}, precisa ser preenchido");
                
        }
    }
}
