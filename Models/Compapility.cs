﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace F_LocalBrand.Models;

public partial class Compapility
{
    public int CompapilityId { get; set; }

    public int? ProductId { get; set; }

    public int? RecommendedProductId { get; set; }

    public virtual Product Product { get; set; }

    public virtual Product RecommendedProduct { get; set; }
}