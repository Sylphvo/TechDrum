using TechDrum.Web.Utils.Notification.Models.Constants;

namespace TechDrum.Web.Utils.Notification.Models
{
    public class NotificationModel
    {
        private string _message;

        public NotificationModel()
        {
        }

        public NotificationModel(string title, string message, NotificationStatus status)
        {
            _message = message;
            Title = title;
            Status = status;
        }

        public string Title { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                _message = _message?.Replace(@"'", @"\'")?.Replace(@"""", @"\""");
            }
        }

        public NotificationStatus Status { get; set; }
    }
}