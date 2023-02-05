using System;
using System.Linq;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class FeatureFactory : IFeatureFactory
    {
        public bool IsSupportOperation(Feature feature, params FeatureType[] supportTypes)
        {
            return supportTypes.Contains(feature.Type);
        }

        public bool FeatureIsValid(Feature feature)
        {
            return feature != null && feature.IsEnable;
        }

        public bool FeatureItemIsValid(FeatureItem featureItem)
        {
            return featureItem != null && featureItem.IsEnable;
        }
    }
}

