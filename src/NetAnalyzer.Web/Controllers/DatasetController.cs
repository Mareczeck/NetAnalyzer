using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetAnalyzer.Web.Models;

namespace NetAnalyzer.Web.Controllers;

/// <summary>
/// For this app I make do with one controller
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Statistics()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upload(DataSetUpload model)
    {
        TempData.Clear();
        TempData.TryAdd("result", "uploaded");

        return RedirectToAction(nameof(Index));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
