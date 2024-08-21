using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;

namespace ExtraDry.Blazor.Tests.Models;

public class PropertyDescriptionTests
{
    [Fact]
    public void CreateObject()
    {
        var propertyDescription = new PropertyDescription(typeof(SizeAndLengthTestModel).GetProperty(nameof(SizeAndLengthTestModel.Unset))!);

        Assert.NotNull(propertyDescription);
    }

    #region Size and Length tests

    [Theory]
    [InlineData(nameof(SizeAndLengthTestModel.Unset), PropertySize.Jumbo)]
    [InlineData(nameof(SizeAndLengthTestModel.SmallStringLength), PropertySize.Small)]
    [InlineData(nameof(SizeAndLengthTestModel.MediumStringLength), PropertySize.Medium)]
    [InlineData(nameof(SizeAndLengthTestModel.LargeStringLength), PropertySize.Large)]
    [InlineData(nameof(SizeAndLengthTestModel.JumboStringLength), PropertySize.Jumbo)]
    [InlineData(nameof(SizeAndLengthTestModel.SmallMaxLength), PropertySize.Small)]
    [InlineData(nameof(SizeAndLengthTestModel.MediumMaxLength), PropertySize.Medium)]
    [InlineData(nameof(SizeAndLengthTestModel.LargeMaxLength), PropertySize.Large)]
    [InlineData(nameof(SizeAndLengthTestModel.JumboMaxLength), PropertySize.Jumbo)]
    public void SizeAndLength(string propertyName, PropertySize expectedSize)
    {
        var propertyDescription = new PropertyDescription(typeof(SizeAndLengthTestModel).GetProperty(propertyName)!);

        Assert.Equal(expectedSize, propertyDescription.Size);
        Assert.Equal(expectedSize, propertyDescription.Length);
    }

    [Theory]
    [InlineData(nameof(SizeAndLengthTestModel.UnsetSetSmall), PropertySize.Jumbo, PropertySize.Small)]
    [InlineData(nameof(SizeAndLengthTestModel.SmallStringLengthSetMedium), PropertySize.Small, PropertySize.Medium)]
    [InlineData(nameof(SizeAndLengthTestModel.MediumStringLengthSetLarge), PropertySize.Medium, PropertySize.Large)]
    [InlineData(nameof(SizeAndLengthTestModel.LargeStringLengthSetJumbo), PropertySize.Large, PropertySize.Jumbo)]
    [InlineData(nameof(SizeAndLengthTestModel.JumboStringLengthSetSmall), PropertySize.Jumbo, PropertySize.Small)]
    [InlineData(nameof(SizeAndLengthTestModel.SmallMaxLengthSetMedium), PropertySize.Small, PropertySize.Medium)]
    [InlineData(nameof(SizeAndLengthTestModel.MediumMaxLengthSetLarge), PropertySize.Medium, PropertySize.Large)]
    [InlineData(nameof(SizeAndLengthTestModel.LargeMaxLengthSetJumbo), PropertySize.Large, PropertySize.Jumbo)]
    [InlineData(nameof(SizeAndLengthTestModel.JumboMaxLengthSetSmall), PropertySize.Jumbo, PropertySize.Small)]
    public void SizeAndLengthWithOverride(string propertyName, PropertySize expectedSize, PropertySize expectedLength)
    {
        var propertyDescription = new PropertyDescription(typeof(SizeAndLengthTestModel).GetProperty(propertyName)!);

        Assert.Equal(expectedSize, propertyDescription.Size);
        Assert.Equal(expectedLength, propertyDescription.Length);
    }

    private class SizeAndLengthTestModel
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

    }

    #endregion
}
