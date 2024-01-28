using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetAnalyzer.Web.Models;

namespace NetAnalyzer.Web.Controllers;

/// <summary>
/// For this app I make do with one controller
/// </summary>
public class DatasetController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Statistics()
    {
        return View();
    }


}
