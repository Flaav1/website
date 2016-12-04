using System.Collections.Generic;
using System.Web;

namespace scrolltest.Models
{
    public class Product
    {
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string filePath { get; set; }
    }

    public class Picture
    {

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }

    public class PostedProduct
    {
        public string title { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public IEnumerable<HttpPostedFileBase> files { get; set; }

    }



}