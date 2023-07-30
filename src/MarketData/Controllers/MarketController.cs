using MarketData.IG;
using MarketData.ML;
using MarketData.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketData.Controllers
{
    [Route("Markets")]
    public class MarketController : Controller
    {
        readonly IGClient ig;

        public MarketController(IGClient ig) =>
            this.ig = ig;

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term) =>
            Ok(await ig.SearchMarkets(term));

        [HttpGet("History")]
        public async Task<IActionResult> History(string epic, string resolution, DateTime startDate, DateTime endDate) =>
            Ok(await ig.GetMarketHistory(epic, resolution, startDate, endDate));

        [HttpGet("TrendLine")]
        public async Task<IActionResult> PredictedTrendLine(string epic, string resolution)
        {
            var trendLineService = new MLTrendLineService(ig, new MLPredictionContext());
            return Ok(await trendLineService.GetTrendLine(epic, resolution));
        }
    }
}