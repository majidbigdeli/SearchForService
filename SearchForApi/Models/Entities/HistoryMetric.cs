using System.Collections.Generic;
using System.Linq;
using SearchForApi.Utilities;

namespace SearchForApi.Models.Entities
{
    public class HistoryMetric
    {
        public HistoryMetricType Type { get; set; }
        public long Count { get; set; }
        public bool IsTotal { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public override string ToString()
        {
            var normalizedType = Type.ToString().ToSnakeCase();
            if (!IsTotal)
            {
                var propertiesString = string.Join(',', Properties.Select(p => $"{p.Key.ToSnakeCase()}=\"{p.Value}\""));
                return string.Format("searchforapi_{0}{{{1}}} {2}", normalizedType, propertiesString, Count);
            }
            else
            {
                return string.Format("searchforapi_{0}_total {1}", normalizedType, Count);
            }
        }
    }

    public enum HistoryMetricType
    {
        Search,
        Share,
        Register,
        Report,
        Bookmark,
        ActivatePlan,
        Payment,
        Income,
        Discount,
        Scene,
        SearchRefer
    }
}