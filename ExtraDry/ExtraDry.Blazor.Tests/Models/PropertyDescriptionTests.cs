using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;

namespace ExtraDry.Blazor.Tests.Models;

public class PropertyDescriptionTests
{
    [Fact]
    public void CreateObject()
    {
        var propertyDescription = new PropertyDescription(typeof(SizeTestModel).GetProperty(nameof(SizeTestModel.Unset))!);

        Assert.NotNull(propertyDescription);
    }

    #region Size tests

    [Theory]
    [InlineData(nameof(SizeTestModel.Unset), PropertySize.Jumbo)]
    [InlineData(nameof(SizeTestModel.SmallStringLength), PropertySize.Small)]
    [InlineData(nameof(SizeTestModel.MediumStringLength), PropertySize.Medium)]
    [InlineData(nameof(SizeTestModel.LargeStringLength), PropertySize.Large)]
    [InlineData(nameof(SizeTestModel.JumboStringLength), PropertySize.Jumbo)]
    [InlineData(nameof(SizeTestModel.SmallMaxLength), PropertySize.Small)]
    [InlineData(nameof(SizeTestModel.MediumMaxLength), PropertySize.Medium)]
    [InlineData(nameof(SizeTestModel.LargeMaxLength), PropertySize.Large)]
    [InlineData(nameof(SizeTestModel.JumboMaxLength), PropertySize.Jumbo)]
    public void Size(string propertyName, PropertySize expectedSize)
    {
        var propertyDescription = new PropertyDescription(typeof(SizeTestModel).GetProperty(propertyName)!);

        Assert.Equal(expectedSize, propertyDescription.Size);
        //Assert.Equal(expectedSize, propertyDescription.Length);
    }

    [Theory]
    [InlineData(nameof(SizeTestModel.UnsetSetSmall), PropertySize.Small)]
    [InlineData(nameof(SizeTestModel.SmallStringLengthSetMedium), PropertySize.Medium)]
    [InlineData(nameof(SizeTestModel.MediumStringLengthSetLarge), PropertySize.Large)]
    [InlineData(nameof(SizeTestModel.LargeStringLengthSetJumbo), PropertySize.Jumbo)]
    [InlineData(nameof(SizeTestModel.JumboStringLengthSetSmall), PropertySize.Small)]
    [InlineData(nameof(SizeTestModel.SmallMaxLengthSetMedium), PropertySize.Medium)]
    [InlineData(nameof(SizeTestModel.MediumMaxLengthSetLarge), PropertySize.Large)]
    [InlineData(nameof(SizeTestModel.LargeMaxLengthSetJumbo), PropertySize.Jumbo)]
    [InlineData(nameof(SizeTestModel.JumboMaxLengthSetSmall), PropertySize.Small)]
    public void SizeWithOverride(string propertyName, PropertySize expectedSize)
    {
        var propertyDescription = new PropertyDescription(typeof(SizeTestModel).GetProperty(propertyName)!);

        Assert.Equal(expectedSize, propertyDescription.Size);
    }

    [Fact]
    public void SizeNotSet()
    {
        var propertyDescription = new PropertyDescription(typeof(SizeTestModel).GetProperty(nameof(SizeTestModel.NotSet))!);

        Assert.Equal(PropertySize.Jumbo, propertyDescription.Size);
    }

    private sealed class SizeTestModel
    {
        public string Unset { get; set; } = string.Empty;

        [StringLength(25)]
        public string SmallStringLength { get; set; } = string.Empty;

        [StringLength(50)]
        public string MediumStringLength { get; set; } = string.Empty;

        [StringLength(100)]
        public string LargeStringLength { get; set; } = string.Empty;

        [StringLength(101)]
        public string JumboStringLength { get; set; } = string.Empty;

#pragma warning disable DRY1310 // Prefer the use of StringLength instead of MaxLength.
        [MaxLength(25)]
        public string SmallMaxLength { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MediumMaxLength { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LargeMaxLength { get; set; } = string.Empty;

        [MaxLength(101)]
        public string JumboMaxLength { get; set; } = string.Empty;
#pragma warning restore DRY1310 // Prefer the use of StringLength instead of MaxLength.

        [InputFormat(Size = PropertySize.Small)]
        public string UnsetSetSmall { get; set; } = string.Empty;

        [StringLength(25)]
        [InputFormat(Size = PropertySize.Medium)]
        public string SmallStringLengthSetMedium { get; set; } = string.Empty;

        [StringLength(50)]
        [InputFormat(Size = PropertySize.Large)]
        public string MediumStringLengthSetLarge { get; set; } = string.Empty;

        [StringLength(100)]
        [InputFormat(Size = PropertySize.Jumbo)]
        public string LargeStringLengthSetJumbo { get; set; } = string.Empty;

        [StringLength(101)]
        [InputFormat(Size = PropertySize.Small)]
        public string JumboStringLengthSetSmall { get; set; } = string.Empty;

#pragma warning disable DRY1310 // Prefer the use of StringLength instead of MaxLength.
        [MaxLength(25)]
        [InputFormat(Size = PropertySize.Medium)]
        public string SmallMaxLengthSetMedium { get; set; } = string.Empty;

        [MaxLength(50)]
        [InputFormat(Size = PropertySize.Large)]
        public string MediumMaxLengthSetLarge { get; set; } = string.Empty;

        [MaxLength(100)]
        [InputFormat(Size = PropertySize.Jumbo)]
        public string LargeMaxLengthSetJumbo { get; set; } = string.Empty;

        [MaxLength(101)]
        [InputFormat(Size = PropertySize.Small)]
        public string JumboMaxLengthSetSmall { get; set; } = string.Empty;
#pragma warning restore DRY1310 // Prefer the use of StringLength instead of MaxLength.

        [InputFormat()]
        public string NotSet { get; set; } = string.Empty;

    }

    #endregion
}
