using MarketData.IG;
using MarketData.ML;

namespace MarketData.Services
{
    public class MLTrendLineService
    {
        private readonly IGClient ig;
        private readonly MLPredictionContext ml;

        public MLTrendLineService(IGClient ig, MLPredictionContext ml)
        {
            this.ig = ig;
            this.ml = ml;
        }

        public async Task<MLPredictionResult> GetTrendLine(string epic, string resolution)
        {
            var startFrom = DateTime.Today.AddYears(-1);
            var endAt = DateTime.Today.AddDays(1);

            // Valid resolutions ...
            // MINUTE, MINUTE_2, MINUTE_3, MINUTE_5, MINUTE_10, MINUTE_15, MINUTE_30, HOUR, HOUR_2, HOUR_3, HOUR_4, DAY, WEEK, MONTH
            var history = await ig.GetMarketHistory(epic, resolution, startFrom, endAt);

            return new MLPredictionResult
            {
                Resolution = resolution,
                Values = ml.Predict(history.Select(r => new TimeSeriesData { Value = r.highPrice.ask }))
            };
        }

        public async Task<MLPredictionResult[]> GetShortTermTrendLine(string epic) => new[]
        {
            await GetTrendLine(epic, "MINUTE"),
            await GetTrendLine(epic, "HOUR"),
            await GetTrendLine(epic, "HOUR_4")
        };

        public async Task<MLPredictionResult[]> GetLongTermTrendLine(string epic) => new[]
        {
            await GetTrendLine(epic, "DAY"),
            await GetTrendLine(epic, "WEEK"),
            await GetTrendLine(epic, "MONTH")
        };
    }

    public class MLPredictionResult
    { 
        public string Resolution { get; set; }
        public decimal[] Values { get; set; }
    }
}