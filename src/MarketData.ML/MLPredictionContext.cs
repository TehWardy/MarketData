using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.TimeSeries;

namespace MarketData.ML
{
    public class MLPredictionContext
    {
        public decimal[] Predict(IEnumerable<TimeSeriesData> timeSeriesDataList)
        {
            var mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromEnumerable(timeSeriesDataList);

            var pipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedValue",
                inputColumnName: "Value",
                windowSize: 5,
                seriesLength: 30,
                trainSize: 25,
                horizon: 5,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundValue",
                confidenceUpperBoundColumn: "UpperBoundValue"
            );

            var model = pipeline.Fit(dataView);

            // Create a sequence of future data points
            var future = Enumerable.Repeat(new TimeSeriesData { Value = null }, 5).ToList();
            var futureDataView = mlContext.Data.LoadFromEnumerable(future);
            var forecastsDataView = model.Transform(futureDataView);

            var forecastedValues = mlContext.Data.CreateEnumerable<TimeSeriesPrediction>(forecastsDataView, reuseRowObject: false).ToList();

            return forecastedValues
                .SelectMany(v => v.ForecastedValue)
                .ToArray();
        }

    }

    public class TimeSeriesData
    {
        public decimal? Value { get; set; }
    }

    public class TimeSeriesPrediction
    {
        [VectorType(1)]
        public decimal[] ForecastedValue { get; set; }
    }
}
