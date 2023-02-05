using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using SearchForApi.Factories;
using SearchForApi.Repositories;
using SearchForApi.Utilities;

namespace SearchForApi.Services
{
    public class MetricService : IMetricService
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly HistoryRepository _historyRepository;
        private readonly UserRepository _userRepository;
        private readonly BookmarkRepository _bookmarkRepository;
        private readonly ReportRepository _reportRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly ShareHistoryRepository _shareHistoryRepository;
        private readonly DiscountRepository _discountRepository;
        private readonly IHistoryMetricFactory _historyMetricFactory;

        public MetricService(IDateTimeFactory dateTimeFactory, HistoryRepository historyRepository, UserRepository userRepository, BookmarkRepository bookmarkRepository, ReportRepository reportRepository, PaymentRepository paymentRepository, ShareHistoryRepository shareHistoryRepository, IHistoryMetricFactory historyMetricFactory, DiscountRepository discountRepository)
        {
            _dateTimeFactory = dateTimeFactory;
            _historyRepository = historyRepository;
            _userRepository = userRepository;
            _bookmarkRepository = bookmarkRepository;
            _reportRepository = reportRepository;
            _paymentRepository = paymentRepository;
            _shareHistoryRepository = shareHistoryRepository;
            _historyMetricFactory = historyMetricFactory;
            _discountRepository = discountRepository;
        }

        [Time]
        public async Task<string> Get()
        {
            var start = _dateTimeFactory.UtcNow.AddMinutes(-1);

            var searchMetrics = await _historyRepository.GetSearchMetrics(start);
            _historyMetricFactory.AddEmptySearchHistory(searchMetrics);

            var searchReferMetrics = await _historyRepository.GetSearchReferMetrics(start);
            _historyMetricFactory.AddEmptySearchReferHistory(searchReferMetrics);

            var sceneMetrics = await _historyRepository.GetSceneMetrics(start);

            var shareHistoryMetrics = await _shareHistoryRepository.GetShareHistoryMetrics(start);

            var userMetrics = await _userRepository.GetUserMetrics(start);
            _historyMetricFactory.AddEmptyUserHistory(userMetrics);

            var userPlanMetrics = await _userRepository.GetUserPlanMetrics(start);
            _historyMetricFactory.AddEmptyUserPlanHistory(userPlanMetrics);

            var bookmarkMetrics = await _bookmarkRepository.GetBookmarkMetrics(start);

            var reportMetrics = await _reportRepository.GetReportMetrics(start);
            _historyMetricFactory.AddEmptyReportHistory(reportMetrics);

            var paymentMetrics = await _paymentRepository.GetPaymentMetrics(start);
            _historyMetricFactory.AddEmptyPaymentHistory(paymentMetrics);

            var incomeMetrics = await _paymentRepository.GetIncomeMetrics(start);

            var discountMetrics = await _paymentRepository.GetDiscountMetrics(start);
            var discountCodes = await _discountRepository.GetCodes();
            _historyMetricFactory.AddEmptyDiscountHistory(discountMetrics, discountCodes);

            var mergedResult = searchMetrics
                .Union(sceneMetrics)
                .Union(shareHistoryMetrics)
                .Union(userMetrics)
                .Union(userPlanMetrics)
                .Union(bookmarkMetrics)
                .Union(reportMetrics)
                .Union(paymentMetrics)
                .Union(incomeMetrics)
                .Union(discountMetrics)
                .Union(searchReferMetrics)
                .ToList();

            return mergedResult.FlatItems();
        }
    }
}