#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryContent : ComponentBase {

        [Parameter]
        public ContentLayout? Content { get; set; }

        [Parameter]
        public string? ContentName { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        [Command]
        public async Task StartEdit()
        {
            if(Content == null) {
                return;
            }
            foreach(var section in Content.Sections) {
                foreach(var container in section.Containers) {
                    await JSRuntime!.InvokeVoidAsync("startEditing", container.Id);
                }
            }
        }

        [Command]
        public void AddSection()
        {
            if(Content == null) {
                return;
            }
            Content.Sections.Add(new ContentSection { 
                Containers = { 
                    new ContentContainer { Html = "New Section" } 
                } 
            });
            StateHasChanged();
        }

        [Command(Icon = "bold", Collapse = CommandCollapse.Always)]
        public async Task ToggleBold()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleBold");
        }

        [Command]
        public async Task ToggleItalic()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleItalic");
        }

        [Command(Name = "H1")]
        public async Task ToggleHeader1()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 1);
        }

        [Command(Name = "H2")]
        public async Task ToggleHeader2()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 2);
        }

        [Command(Name = "H3")]
        public async Task ToggleHeader3()
        {
            await JSRuntime!.InvokeVoidAsync("roosterToggleHeader", 3);
        }

        [Command(Name = "H4")]
        public async Task ToggleHeader4()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 4);
        }

        [Command(Name = "H5")]
        public async Task ToggleHeader5()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 5);
        }

        [Command(Name = "H6")]
        public async Task ToggleHeader6()
        {
            await JSRuntime!.InvokeVoidAsync("roosterToggleHeader", 6);
        }

        [Command(Name = "HR")]
        public async Task HorizontalRule()
        {
            await JSRuntime.InvokeVoidAsync("roosterHorizontalRule");
        }

        [Command(Icon = "Eraser", Collapse = CommandCollapse.Always)]
        public async Task ClearFormat()
        {
            await JSRuntime.InvokeVoidAsync("roosterClearFormat");
        }

        [Command]
        public async Task AddHyperlink()
        {
            var className = HyperlinkClass;
            var title = HyperlinkTitle;
            var href = HyperlinkHref;
            Console.WriteLine($"values: {className}, {title}, {href}");
            await JSRuntime.InvokeVoidAsync("roosterInsertHyperlink", HyperlinkClass, HyperlinkHref, HyperlinkTitle);
        }

        public ContentSection? CurrentSection { get; set; }

        public ContentContainer? CurrentContainer { get; set; }

        public ContentTheme CurrentSectionTheme {
            get => CurrentSection?.Theme ?? ContentTheme.Light;
            set {
                if(CurrentSection != null) {
                    CurrentSection.Theme = value;
                    StateHasChanged();
                }
            }
        }

        public SectionLayout CurrentSectionLayout {
            get => CurrentSection?.Layout ?? SectionLayout.Single;
            set {
                if(CurrentSection != null) {
                    CurrentSection.Layout = value;
                    StateHasChanged();
                }
            }
        }

        public ContentAlignment CurrentContainerAlignment {
            get => CurrentContainer?.Alignment ?? ContentAlignment.TopLeft;
            set {
                if(CurrentContainer != null) {
                    CurrentContainer.Alignment = value;
                    StateHasChanged();
                }
            }
        }

        public ContentPadding CurrentContainerPadding {
            get => CurrentContainer?.Padding ?? ContentPadding.None;
            set {
                if(CurrentContainer != null) {
                    CurrentContainer.Padding = value;
                    StateHasChanged();
                }
            }
        }

        public string HyperlinkClass { get; set; } = string.Empty;

        public string HyperlinkTitle { get; set; } = string.Empty;

        public string HyperlinkHref { get; set; } = string.Empty;

        private void EditorFocus(ContentSection section, ContentContainer container, FocusEventArgs args)
        {
            CurrentSection = section;
            CurrentContainer = container;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(Content == null) {
                return;
            }
            foreach(var section in Content.Sections) {
                foreach(var container in section.Containers) {
                    if(!roosterIsCanonical.Contains(container.Id)) {
                        await JSRuntime.InvokeVoidAsync("roosterSetContent", container.Id, container.Html);
                        roosterIsCanonical.Add(container.Id);
                    }
                }
            }
        }
        private List<Guid> roosterIsCanonical = new List<Guid>();

        private async Task EditorFocusOut(ContentSection section, ContentContainer container, FocusEventArgs args)
        {
            var value = await JSRuntime.InvokeAsync<string>("roosterGetContent", container.Id);
            container.Html = value;
        }

    }

    //[Display(Name = "Add/Edit Hyperlink")]
    //public class Hyperlink {

    //    [Required]
    //    public string Caption { get; set; } = string.Empty;

    //    [Url]
    //    public string Url { get; set; } = string.Empty;
    //}
}
