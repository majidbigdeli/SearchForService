using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IShareFactory
    {
        Share CreateNewShareInstance(Guid userId, Guid sceneId, string keyword);
        void UpdateShareItemView(Share existItem);
    }
}