using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IShareHistoryFactory
    {
        ShareHistory CreateNewShareHistoryInstance(Guid shareId);
    }
}
