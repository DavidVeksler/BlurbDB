//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E1Blurbs.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductCulture_lnk
    {
        public int ProductCultureId { get; set; }
        public int ProductId { get; set; }
        public int CultureId { get; set; }
        public System.DateTime InsertDate { get; set; }
        public System.DateTime SaveDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Culture Culture { get; set; }
        public virtual Product Product { get; set; }
    }
}
