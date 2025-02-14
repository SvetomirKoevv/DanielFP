using System.ComponentModel.DataAnnotations;

namespace MVCApplication.Models
{
    public class BufferedFileUploadDb
    {
        [Display(Name = "File")]
        public List<IFormFile> FormFile { get; set; }
    }
}
