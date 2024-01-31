using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetAnalyzer.Business;
using NetAnalyzer.Web.Models;

namespace NetAnalyzer.Web.Controllers;

public class HomeController : Controller
{
    private readonly IDatasetService datasetService;

    public HomeController(IDatasetService datasetService)
    {
        this.datasetService = datasetService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upload(DataSetUploadViewModel model)
    {
        try
        {
            var id = datasetService.CreateDataset(model.DatasetName);
            datasetService.ProcessDataset(id, model.FormFile.OpenReadStream());
            
            if (TempData["SuccessMessage"] == null)
            {
                TempData["SuccessMessage"] = "Dataset was processed sucessfully";
            }
        }
        catch(Exception ex)
        {
            if (TempData["ErrorMessage"] == null)
            {
                TempData["ErrorMessage"] = "An error occurred during process.";
            }
        }
        // TODO - Service na uložení do souboru a zafrontování zpracování
        return RedirectToAction(nameof(Index));
    }

    
    public IActionResult Clean()
    {
        datasetService.CleanData();
        return Ok();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
