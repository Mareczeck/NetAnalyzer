using System.ComponentModel.DataAnnotations;

namespace NetAnalyzer.Web;

public class DataSetUpload
{
    [Display(Name = "Dataset")]
    [Required]
    public required IFormFile FormFile { get; set; }
}
