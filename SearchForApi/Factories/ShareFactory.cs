using System;
using SearchForApi.Models.Entities;
using SearchForApi.Utilities;

namespace SearchForApi.Factories
{
    public class ShareFactory : IShareFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public ShareFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public Share CreateNewShareInstance(Guid userId, Guid sceneId, string keyword)
        {
            return new Share
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                SceneId = sceneId,
                ViewCount = 0,
                Keyword = keyword,
                Token = StringNormalize.RandomToken
            };
        }

        public void UpdateShareItemView(Share existItem)
        {
            existItem.ViewCount += 1;
            existItem.LastViewOn = _dateTimeFactory.UtcNow;
        }
    }
}