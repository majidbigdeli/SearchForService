using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
	public interface IFeatureService
	{
		Task<Feature> GetById(Guid id);
		Task<(long total, List<FeatureItem>)> GetItems(Guid id, Guid? userId, int skip, int take);
		Task<SearchResultModel> GetItemScenes(Guid itemId, Guid? userId, int skip);
		Task<SerarchResultItemModel> GetItemScene(Guid itemId, Guid? userId);
	}
}

