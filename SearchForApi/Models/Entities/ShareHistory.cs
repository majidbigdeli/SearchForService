using System;
namespace SearchForApi.Models.Entities
{
    public class ShareHistory : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid ShareId { get; set; }

        public Share Share { get; set; }
    }
}
