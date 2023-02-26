using System;
using System.Collections.Generic;

namespace Shop.ProductTestWork.Core.Class;

public partial class ProductDataForOption
{
    public Guid Id { get; set; }

    public Guid IdProductUseProductType { get; set; }
    public Guid IdProductTypeDataOptions { get; set; }

    public string? Text { get; set; }
}
