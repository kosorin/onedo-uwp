using System;
using OneDo.Domain.Model.Entities;

namespace OneDo.Application.Notifications
{
    public interface INotificationService
    {
        void Reschedule(Note note);

        void Schedule(Note note);

        void CancelScheduled(Guid id);

        void ClearSchedule();
    }
}