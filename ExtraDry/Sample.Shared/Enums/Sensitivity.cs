#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Sample.Shared;

public enum Sensitivity {
    
    [Display(ShortName = "Low", Name = "Low", Order = 0)]
    Low = 1,

    [Display(ShortName = "Normal", Name = "Normal", Order = 1)]
    Normal = 0, // 0 to make normal the default

    [Display(ShortName = "High", Name = "High", Order = 2)]
    High = 2,

}
