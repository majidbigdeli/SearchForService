using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class ShareHistoryFactory: IShareHistoryFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public ShareHistoryFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public ShareHistory CreateNewShareHistoryInstance(Guid shareId)
        {
            return new ShareHistory
            {
                Id = Guid.NewGuid(),
                CreatedOn = _dateTimeFactory.UtcNow,
                ShareId= shareId
            };
        }
    }
}
