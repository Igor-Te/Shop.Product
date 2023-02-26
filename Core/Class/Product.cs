using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace Shop.ProductTestWork.Core.Class;

public partial class Product
{
    
    //public Guid Id { get; set; }
    //[Key]

    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

}
