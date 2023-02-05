using System;
using System.Collections.Generic;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IHistoryMetricFactory
    {
        void AddEmptySearchHistory(List<HistoryMetric> result);
        void AddEmptySearchReferHistory(List<HistoryMetric> result);
        void AddEmptyUserHistory(List<HistoryMetric> result);
        void AddEmptyUserPlanHistory(List<HistoryMetric> result);
        void AddEmptyReportHistory(List<HistoryMetric> result);
        void AddEmptyPaymentHistory(List<HistoryMetric> result);
        void AddEmptyDiscountHistory(List<HistoryMetric> result, List<string> discountCodes);
    }
}
