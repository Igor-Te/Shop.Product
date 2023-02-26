namespace Shop.ProductTestWork.Core.Interface.InterfaceClasses
{
    public class InterfaceClassProductType
    {
        public string? Caption { get; set; }

        public List<string> TagsProductType { get; set; }

        public InterfaceClassProductType(string? caption) 
        {
            Caption=caption;
            TagsProductType=new List<string>();
        }

        public void DropConnectionText()
        {
            TagsProductType.Clear();
          
            TagsProductType = new List<string>();
  
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
    }
}
