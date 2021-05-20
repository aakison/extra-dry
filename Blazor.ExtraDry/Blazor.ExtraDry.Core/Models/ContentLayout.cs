#nullable enable

using System;
using System.Collections.ObjectModel;

namespace Blazor.ExtraDry {

    public class ContentLayout {

        public Collection<ContentSection> Sections { get; set; } = new Collection<ContentSection>();

    }

    public class ContentSection {

        public SectionLayout Layout { get; set; } = SectionLayout.Single;

        public ContentPadding Padding { get; set; } = ContentPadding.Single;

        public Collection<ContentContainer> Containers { get; set; } = new Collection<ContentContainer>();

    }

    public class ContentContainer {

        public Guid Id { get; set; } = Guid.NewGuid();

        public ContentAlignment Alignment { get; set; } = ContentAlignment.Top;

        /// <summary>
        /// The HTML for the content element.  This HTML should be kept very clean, most tags and styles are invalid.
        /// </summary>
        public string Html { get; set; } = string.Empty;

    }

    public enum ContentAlignment {
        Top,
        Center,
        Bottom,
    }

    public enum ContentTheme {
        Light,
        Dark,
        Accent,
    }

    public enum SectionLayout {
        Single,
        Double,
        DoubleWeightedLeft,
        DoubleWeightedRight,
        Triple,
        Quadruple,
    }

    public enum ContentPadding {
        Single,
        Double,
        None,
    }
}
