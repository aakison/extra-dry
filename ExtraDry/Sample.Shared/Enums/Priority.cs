#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Sample.Shared;

public enum Priority {
    
    [Display(ShortName = "P1", Name = "P1 (could cause physical harm)", Order = 1)]
    P1 = 1,

    [Display(ShortName = "P2", Name = "P2 (risk of financial loss)", Order = 2)]
    P2 = 2,
    
    [Display(ShortName = "P3", Name = "P3 (routine maintenancne)", Order = 3)]
    P3 = 0, // 0 to make this the default

    [Display(ShortName = "P4", Name = "P4 (routine / deferable)", Order = 4)]
    P4 = 3,
    
}

