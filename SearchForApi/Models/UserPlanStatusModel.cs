using System;
using System.Collections.Generic;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models
{
    public class UserPlanStatusModel
    {
        public UserPlanStatusModel()
        {
            SearchedItems = new List<string>();
        }

        public DateTime CacheLifeTime { get; set; }
        public int SearchPerDayRemain { get; set; }
        public List<String> SearchedItems { get; set; }
        public int QuoteViewPerSearch { get; set; }
        public int ShareQuoteRemain { get; set; }
        public int BookmarkedRemain { get; set; }
        public int LastHistoriesDays { get; set; }
        public bool KidsMode { get; set; }
        public int TrialDaysRemain { get; set; }

        private PlanType _Type { get; set; }
        public PlanType Type
        {
            get => _Type == PlanType.Basic && TrialDaysRemain > 0 ? PlanType.Trial : _Type;
            set => _Type = value;
        }
    }

    public enum UserPlanStatusType
    {
        Search = 1,
        Bookmark,
        Share
    }
}