namespace Shop.ProductTestWork.Core.Interface.InterfaceClasses
{
    public class InterfaceClassProduct
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public List<string> TagsProductType { get; set; }

        public List<string> CaptionForOption { get; set; }

          public String file { get; set; }


        public InterfaceClassProduct(string? title, string? description, double price)
        {
            Title = title;
            Description = description;
            Price = price;

            TagsProductType = new List<string>();

            CaptionForOption = new List<string>();
        }

        public void DropConnectionText()
        {
            TagsProductType.Clear();

            CaptionForOption.Clear();

            TagsProductType = new List<string>();

            CaptionForOption = new List<string>();
        }


        public void AddTagsProductType(string tagsProductType)
        {
                TagsProductType.Add(tagsProductType);

        }
        public void AddTagsProductType(List<string> tagsProductType)
        {
            foreach (var tag in tagsProductType)
            {
                TagsProductType.Add(tag);
            }
        }
        public void AddCaptionForOption(string captionForOption)
        {
            CaptionForOption.Add(captionForOption);          
        }
        public void AddCaptionForOption(List<string> captionForOption)
        {
            foreach (var tag in captionForOption)
            {
                CaptionForOption.Add(tag);
            }
        }


    }
}
