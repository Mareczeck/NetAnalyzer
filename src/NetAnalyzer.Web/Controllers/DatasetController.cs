using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetAnalyzer.Business;
using NetAnalyzer.Domain.Dataset;
using NetAnalyzer.Web.Models;

namespace NetAnalyzer.Web.Controllers;

/// <summary>
/// For this app I make do with one controller
/// </summary>
public class DatasetController : Controller
{
    private readonly IDatasetService datasetService;

    public DatasetController(IDatasetService datasetService)
    {
        this.datasetService = datasetService;
    }

    public IActionResult Index()
    {
        var model = new DatasetStatisticsViewModel()
        {
            Statistics = datasetService.LoadDatasetStatistics()
        };
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        var model = datasetService.LoadDatasetStatistic(id);
        return View(model);
    }
    
    public IActionResult GetGraphModel(int id)
    {
        return Json(datasetService.LoadDatasetWithMaxDistance(id));
    }

    public IActionResult GetAverageLinksForDistance(int id, int distance)
    {
        return Json(datasetService.GetAverageLinksByDistance(id, distance));
    }

    public IActionResult GetReachableNodesForNodeByDistance(int id, int nodeId, int distance)
    {
        return Json(datasetService.GetReachableNodesForNode(id, nodeId, distance));
    }
}
