using System;

namespace SearchForApi.Models.Entities
{
    public class UserDevice : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public string DeviceId { get; set; }
        public MobilePlatfrom Platform { get; set; }
        public string Model { get; set; }
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string AppVersion { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActiveStatusChangedOn { get; set; }
        public string NotificationToken { get; set; }
        public DateTime DataChangedOn { get; set; }

        public User User { get; set; }
    }

    public enum MobilePlatfrom
    {
        Android = 1,
        IOS = 2
    }

    public enum UserDeviceStatus
    {
        Active = 1,
        Deactive = 2
    }
}