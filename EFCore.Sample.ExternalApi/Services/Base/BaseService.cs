using FluentValidation;
using FluentValidation.Results;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Notifications;

namespace EFCore.Sample.ExternalApi.Services.Base
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;

        protected BaseService(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected void Notify(ValidationResult validationResult) {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }
        protected void Notify(string message) {
            _notificator.Handle(new Notification(message));
        }

        protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : class {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}
