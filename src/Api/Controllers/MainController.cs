using Business.Interfaces;
using Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;
        public readonly IUser AppUser;

        public Guid UserId { get; set; }

        public bool AuthenticatedUser { get; set; }

        protected MainController(INotifier notifier,
                                 IUser appUser)
        {
            _notifier = notifier;
            AppUser = appUser;

            if (AppUser.IsAuthenticated())
            {
                UserId = AppUser.GetUserId();
                AuthenticatedUser = true;
            }
        }

        protected bool IsValidOperation() => !_notifier.HasNotification();

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifier.GetNotifications().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotifyError(modelState);

            return CustomResponse();
        }

        protected void NotifyError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message) => _notifier.Handle(new Notification(message));
    }
}
