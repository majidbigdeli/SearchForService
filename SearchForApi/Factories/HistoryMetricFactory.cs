using System;
using System.Collections.Generic;
using System.Linq;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class HistoryMetricFactory : IHistoryMetricFactory
    {
        public void AddEmptySearchHistory(List<HistoryMetric> result)
        {
            var items = new[] { true, false };
            foreach (var item in items)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(History.SearchIsFound) && item.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.Search,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(History.SearchIsFound), item }
                        }
                    });
            }
        }

        public void AddEmptySearchReferHistory(List<HistoryMetric> result)
        {
            var referTypes = Enum.GetValues<HistoryReferType>();
            foreach (var referType in referTypes)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(History.ReferType) && referType.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.SearchRefer,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(History.ReferType), referType }
                        }
                    });
            }
        }

        public void AddEmptyUserHistory(List<HistoryMetric> result)
        {
            var platformTypes = new List<PlatformType>() {
                PlatformType.Android, PlatformType.PWA, PlatformType.WebApp };
            var registerMethodTypes = Enum.GetValues<RegisterMethodType>();

            foreach (var platformType in platformTypes)
            {
                foreach (var registerMethodType in registerMethodTypes)
                {
                    var isExist = result.Any(p =>
                        p.Properties.Any(q =>
                            q.Key == nameof(User.RegisterPlatform) && platformType.Equals(q.Value)) &&
                        p.Properties.Any(q =>
                            q.Key == nameof(User.RegisterMethod) && registerMethodType.Equals(q.Value))
                    );

                    if (!isExist)
                        result.Add(new HistoryMetric
                        {
                            Type = HistoryMetricType.Register,
                            Count = 0,
                            Properties = new()
                            {
                                { nameof(User.RegisterPlatform), platformType },
                                { nameof(User.RegisterMethod), registerMethodType }
                            }
                        });
                }
            }
        }

        public void AddEmptyUserPlanHistory(List<HistoryMetric> result)
        {
            var planTypes = Enum.GetValues<PlanType>();
            foreach (var planType in planTypes)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(User.PlanId) && planType.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.ActivatePlan,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(User.PlanId), planType }
                        }
                    });
            }
        }

        public void AddEmptyReportHistory(List<HistoryMetric> result)
        {
            var reportTypes = Enum.GetValues<ReportType>();
            foreach (var reportType in reportTypes)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(Report.Type) && reportType.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.Report,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(Report.Type), reportType }
                        }
                    });
            }
        }

        public void AddEmptyPaymentHistory(List<HistoryMetric> result)
        {
            var paymentStatuses = Enum.GetValues<PaymentStatus>();
            foreach (var paymentStatus in paymentStatuses)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(Payment.Status) && paymentStatus.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.Payment,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(Payment.Status), paymentStatus }
                        }
                    });
            }
        }

        public void AddEmptyDiscountHistory(List<HistoryMetric> result, List<string> discountCodes)
        {
            foreach (var discountCode in discountCodes)
            {
                var isExist = result.Any(p =>
                    p.Properties.Any(q =>
                        q.Key == nameof(Payment.DiscountCode) && discountCode.Equals(q.Value))
                    );

                if (!isExist)
                    result.Add(new HistoryMetric
                    {
                        Type = HistoryMetricType.Discount,
                        Count = 0,
                        Properties = new()
                        {
                            { nameof(Payment.DiscountCode), discountCode }
                        }
                    });
            }
        }
    }
}
