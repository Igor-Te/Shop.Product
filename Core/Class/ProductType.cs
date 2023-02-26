using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.ProductTestWork.Core.Class;

public partial class ProductType
{

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Caption { get; set; }
}
