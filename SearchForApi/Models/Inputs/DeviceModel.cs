using System;
using System.ComponentModel.DataAnnotations;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Inputs
{
    public class DeviceModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public MobilePlatfrom Platform { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Os { get; set; }

        [Required]
        public string OsVersion { get; set; }

        [Required]
        public string AppVersion { get; set; }

        public string NotificationToken { get; set; }
    }
}