using System.ComponentModel.DataAnnotations;

namespace NetAnalyzer.Web.Models;

public class DataSetUploadViewModel
{
    [Required]
    public required IFormFile FormFile { get; set; }
}
