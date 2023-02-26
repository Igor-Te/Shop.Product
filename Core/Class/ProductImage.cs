using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.ProductTestWork.Core.Class
{
    public class ProductImage
    {
        [Key]
        public Guid ProductId { get; set; }

        public byte[] ImageByte { get; set; }


        private IFormFile fiale { get; set; }

        public void SetFile (IFormFile file)
        {
            fiale = file;
        }

        public string GetFile()
        {
           return Convert.ToBase64String(ImageByte);
        }

    }
}
