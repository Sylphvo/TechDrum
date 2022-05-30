using Microsoft.AspNetCore.Mvc;
using TechDrum.Core.Constants;
using TechDrum.Core.Exceptions;
using TechDrum.Web.Utils.Notification.Models.Constants;
using Invedia.Web.ITempDataDictionaryUtils;
using TechDrum.Web.Utils.Notification.Models;


namespace TechDrum.Web.Utils.Notification
{
    public static class ControllerExtensions
    {
        public static void SetNotification(this Controller controller, string title, string message,
            NotificationStatus status)
        {
            controller.TempData.Set(TempDataKey.Notify, new NotificationModel(title, message, status));
        }

        public static void SetNotification(this Controller controller, string title, CoreException exception,
            NotificationStatus status = NotificationStatus.Error)
        {
            var errorCode = new ErrorModel(exception);

            var message = errorCode.Message;

            controller.SetNotification(title, message, status);
        }

        public static void RemoveNotify(this Controller controller)
        {
            controller.TempData.Remove(TempDataKey.Notify);
        }
    }
}