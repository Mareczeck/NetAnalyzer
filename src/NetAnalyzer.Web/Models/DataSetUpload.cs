using System.ComponentModel.DataAnnotations;

namespace NetAnalyzer.Web;

public class DataSetUploadModel
{
    [Required]
    public required IFormFile FormFile { get; set; }
}
