//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Realtors_Portal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RP_propertyTypes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RP_propertyTypes()
        {
            this.RP_property = new HashSet<RP_property>();
        }
    
        public int pt_id { get; set; }
        public string pt_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RP_property> RP_property { get; set; }
    }
}
