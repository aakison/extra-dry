using AngleSharp.Html.Dom;
using ExtraDry.Blazor.Forms;
using ExtraDry.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace ExtraDry.Blazor.Tests.Components.Forms;

public class DryInputDateTimeTests
{
    [Fact]
    public void DateTimeInputRendersAsDatetimeLocal()
    {
        using var context = new TestContext();
        var model = new DateTimeModel { Id = 1, DateTime = new DateTime(2024, 8, 12, 19, 30, 12, DateTimeKind.Local) };
        var description = new ViewModelDescription(typeof(DateTimeModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeModel.DateTime));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeModel>>(
            (nameof(DryInputDateTime<DateTimeModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.DateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture), input.Value);
        Assert.Equal("datetime-local", input.Type);
    }

    [Fact]
    public void DateTimeInputConvertsStoredUTCToLocalDisplayed()
    {
        using var context = new TestContext();
        var model = new DateTimeModel { Id = 1, DateTime = new DateTime(2024, 8, 12, 19, 30, 12, DateTimeKind.Utc) };
        var description = new ViewModelDescription(typeof(DateTimeModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeModel.DateTime));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeModel>>(
            (nameof(DryInputDateTime<DateTimeModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.DateTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture), input.Value);
        Assert.Equal("datetime-local", input.Type);
    }

    [Fact]
    public void DateOnlyInputRendersAsDate()
    {
        using var context = new TestContext();
        var model = new DateTimeModel { Id = 1, DateOnly = new DateOnly(2024, 8, 12) };
        var description = new ViewModelDescription(typeof(DateTimeModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeModel.DateOnly));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeModel>>(
            (nameof(DryInputDateTime<DateTimeModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.DateOnly.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), input.Value);
        Assert.Equal("date", input.Type);
    }

    [Fact]
    public void TimeOnlyInputRendersAsTime()
    {
        using var context = new TestContext();
        var model = new DateTimeModel { Id = 1, TimeOnly = new TimeOnly(19, 30) };
        var description = new ViewModelDescription(typeof(DateTimeModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeModel.TimeOnly));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeModel>>(
            (nameof(DryInputDateTime<DateTimeModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.TimeOnly, TimeOnly.Parse(input.Value, CultureInfo.InvariantCulture));

        Assert.Equal("time", input.Type);
    }

    [Fact]
    public void DateOverrideInputRendersAsDate()
    {
        using var context = new TestContext();
        var model = new DateTimeAttributedModel { Id = 1, DateOnly = new DateTime(2024, 8, 12, 19, 30, 12, DateTimeKind.Local) };
        var description = new ViewModelDescription(typeof(DateTimeAttributedModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeAttributedModel.DateOnly));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeAttributedModel>>(
            (nameof(DryInputDateTime<DateTimeAttributedModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeAttributedModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.DateOnly.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), input.Value);
        Assert.Equal("date", input.Type);
    }

    [Fact]
    public void DateTimeOverrideRendersAsDateTime()
    {
        using var context = new TestContext();
        var model = new DateTimeAttributedModel { Id = 1, DateTime = "2024-8-12T19:30" };
        var description = new ViewModelDescription(typeof(DateTimeAttributedModel), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(DateTimeAttributedModel.DateTime));

        var fragment = context.RenderComponent<DryInputDateTime<DateTimeAttributedModel>>(
            (nameof(DryInputDateTime<DateTimeAttributedModel>.Model), model),
            (nameof(DryInputDateTime<DateTimeAttributedModel>.Property), property)
            );

        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(DateTime.Parse(model.DateTime, CultureInfo.InvariantCulture),
            DateTime.Parse(input.Value, CultureInfo.InvariantCulture));
        Assert.Equal("datetime-local", input.Type);
    }

    public class DateTimeModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public TimeOnly TimeOnly { get; set; }

        public DateOnly DateOnly { get; set; }
    }

    public class DateTimeAttributedModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [InputFormat(DataTypeOverride = typeof(DateTime))]
        public string DateTime { get; set; } = string.Empty;

        [InputFormat(DataTypeOverride = typeof(TimeOnly))]
        public DateTime TimeOnly { get; set; }

        [InputFormat(DataTypeOverride = typeof(DateOnly))]
        public DateTime DateOnly { get; set; }
    }
}
