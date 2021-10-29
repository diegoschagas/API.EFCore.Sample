using EFCore.Sample.Business.Notifications;
using System.Collections.Generic;

namespace EFCore.Sample.Business.Interfaces
{
    public interface INotificator
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
