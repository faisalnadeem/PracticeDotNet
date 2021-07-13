using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ImageUploader.Models
{
    public class FileUploadModel
    {
        public IFormFile FileBase{ get; set; }
    }
}
