#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.ExtraDry {

    public class ContentLayout {

        public Collection<ContentSection> Sections { get; set; } = new Collection<ContentSection>();

    }

    public class ContentSection {

        public SectionLayout Layout { get; set; } = SectionLayout.Single;

        public ContentTheme Theme { get; set; } = ContentTheme.Light;

        public ContentPadding Padding { get; set; } = ContentPadding.Single;

        public Collection<ContentContainer> Containers { get; set; } = new Collection<ContentContainer>();

        public IEnumerable<ContentContainer> DisplayContainers {
            get {
                while(Containers.Count < ContainerCount) {
                    Containers.Add(new ContentContainer());
                }
                return Containers.Take(ContainerCount);
            }
        }

        private int ContainerCount => Layout switch {
            SectionLayout.Single => 1,
            SectionLayout.Triple => 3,
            SectionLayout.Quadruple => 4,
            _ => 2,
        };

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
        Banner,
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
