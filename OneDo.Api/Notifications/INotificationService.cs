using System;
using OneDo.Domain.Model.Entities;

namespace OneDo.Application.Notifications
{
    public interface INotificationService
    {
        void Reschedule(Note note);

        void RemoveFromSchedule(Guid id);

        void ClearSchedule();
    }
}