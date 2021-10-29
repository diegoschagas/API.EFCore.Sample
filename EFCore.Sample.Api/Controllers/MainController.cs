using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Notifications;
using System.Linq;

namespace EFCore.Sample.Api.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        public readonly INotificator _notificator;

        public MainController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool OperationValidate()
        {
            return !_notificator.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationValidate())
            {
                return Ok(new { success = true, data = result });
            }

            return BadRequest(new { success = false, errors = _notificator.GetNotifications().Select(n => n.mensagem) });
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelstate) {
            if (!modelstate.IsValid)
                NotifyErrorModelInvalidated(modelstate);
            return CustomResponse();
        }

        protected void NotifyErrorModelInvalidated(ModelStateDictionary modelstate)
        {
            var errors = modelstate.Values.SelectMany(e => e.Errors);

            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string errorMsg)
        {
            _notificator.Handle(new Notification(errorMsg));
        }
    }
}
