using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
	public interface IFeatureFactory
	{
		bool IsSupportOperation(Feature feature, params FeatureType[] supportTypes);
		bool FeatureIsValid(Feature feature);
		bool FeatureItemIsValid(FeatureItem featureItem);
	}
}

