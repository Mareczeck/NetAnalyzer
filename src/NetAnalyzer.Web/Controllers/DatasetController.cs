using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetAnalyzer.Business;
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
            Statistics = datasetService.LoadDatasets()
        };
        return View(model);
    }

    public IActionResult Detail(int id)
    {
        return View();
    }


}
