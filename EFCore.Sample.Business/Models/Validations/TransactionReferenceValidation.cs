using FluentValidation;
using System.Text.RegularExpressions;

namespace EFCore.Sample.Business.Models.Validations
{
    public class TransactionReferenceValidation : AbstractValidator<TransactionReference>
    {
        public TransactionReferenceValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Please Enter Reference Description")
               .Must(VerifyCallBack).WithMessage("Enter Valid Reference Description");
        }
        private static bool VerifyCallBack(TransactionReference transaction)
        {
            bool isUrlCallBack = false;
            if (!string.IsNullOrEmpty(transaction.ReferenceDescription))
            {
                var callBack = @"callbacks.com.br";

                Regex ER = new Regex(callBack, RegexOptions.None);
                isUrlCallBack = ER.IsMatch(transaction.ReferenceDescription);

                
            }

            return isUrlCallBack;

        }

    }
}
