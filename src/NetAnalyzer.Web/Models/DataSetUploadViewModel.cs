using System.ComponentModel.DataAnnotations;

namespace NetAnalyzer.Web.Models;

public class DataSetUploadViewModel
{

    [MaxLength(256)]
    public required string DatasetName { get; set; }

    [Required]
    public required IFormFile FormFile { get; set; }
}
