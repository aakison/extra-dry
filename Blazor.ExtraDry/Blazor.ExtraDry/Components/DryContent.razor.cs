#nullable enable

using Blazor.ExtraDry.ViewModels;
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

        [Command(Category = "Section", Name = "Add New")]
        public void AddSection()
        {
            if(Content == null) {
                return;
            }
            Content.Sections.Add(new ContentSection { 
                Containers = { 
                    new ContentContainer { 
                        Html = "<div>New Section</div>",
                        Padding = ContentPadding.Single,
                    }
                } 
            });
            StateHasChanged();
        }

        [Command(Category = "Section", Name = "Remove Current")]
        public void RemoveSection()
        {
            if(Content == null) {
                return;
            }
            throw new NotImplementedException();
        }

        [Command(Category = "Selection", Icon = "bold", Collapse = CommandCollapse.Always)]
        public async Task ToggleBold()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleBold");
        }

        [Command(Category = "Selection")]
        public async Task ToggleItalic()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleItalic");
        }

        [Command(Category = "Selection", Name = "H1")]
        public async Task ToggleHeader1()
        {
            await JSRuntime.InvokeVoidAsync("roosterToggleHeader", 1);
        }

        [Command(Category = "Selection", Name = "H2")]
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

        [Control(ControlType.RadioButtons)]
        [Display(Name = "Theme")]
        public ContentTheme CurrentSectionTheme {
            get => CurrentSection?.Theme ?? ContentTheme.Light;
            set {
                if(CurrentSection != null) {
                    CurrentSection.Theme = value;
                    StateHasChanged();
                }
            }
        }

        [Control(ControlType.RadioButtons, IconTemplate = "_content/Blazor.ExtraDry/img/layout-{0}.png")]
        [Display(Name = "Layout")]
        public SectionLayout CurrentSectionLayout {
            get => CurrentSection?.Layout ?? SectionLayout.Single;
            set {
                if(CurrentSection != null) {
                    CurrentSection.Layout = value;
                    StateHasChanged();
                }
            }
        }

        [Control(ControlType.RadioButtons, CaptionTemplate = "", IconTemplate = "_content/Blazor.ExtraDry/img/alignment-{0}.png")]
        [Display(Name = "Alignment")]
        public ContentAlignment CurrentContainerAlignment {
            get => CurrentContainer?.Alignment ?? ContentAlignment.TopLeft;
            set {
                if(CurrentContainer != null) {
                    CurrentContainer.Alignment = value;
                    StateHasChanged();
                }
            }
        }

        [Control(ControlType.RadioButtons, IconTemplate = "_content/Blazor.ExtraDry/img/padding-{0}.png")]
        [Display(Name = "Padding")]
        public ContentPadding CurrentContainerPadding {
            get => CurrentContainer?.Padding ?? ContentPadding.None;
            set {
                if(CurrentContainer != null) {
                    CurrentContainer.Padding = value;
                    StateHasChanged();
                }
            }
        }

        [JSInvokable("UploadImage")]
        public static async Task<IBlob> UploadImage(string imageDataUrl)
        {
            Console.WriteLine(imageDataUrl[..50]);
            if(!imageDataUrl.StartsWith("data:")) {
                throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This URL must begin with 'data:' scheme.", "Unable to upload image. 0x0F4B39DA");
            }
            var semicolon = imageDataUrl.IndexOf(';');
            if(semicolon > 64 || semicolon < 7) {
                throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This must include the mime type between the first ':' and the first ';'", "Unable to upload image. 0x0F8A8B8C");
            }
            var base64Delimiter = imageDataUrl.IndexOf("base64,");
            if(base64Delimiter < 0) {
                throw new DryException("When posting back an image, send through the imageDataUri from the clipboard.  This must include the content of the image properly base64 encoded.", "Unable to upload image. 0x0F3CEE65");
            }
            var mimeType = imageDataUrl[6..semicolon];
            var base64 = imageDataUrl[(base64Delimiter+7)..];
            var bytes = Convert.FromBase64String(base64);
            await Task.Delay(1000);
            return new Blob() {
                Uri = "https://www.akison.com/2019/03/arduino-argb-computer-case-controller/title.jpg",
            };
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
                foreach(var container in section.DisplayContainers) {
                    if(!roosterIsCanonical.Contains(container.Id)) {
                        await JSRuntime.InvokeVoidAsync("startEditing", container.Id);
                        await JSRuntime.InvokeVoidAsync("roosterSetContent", container.Id, container.Html);
                        roosterIsCanonical.Add(container.Id);
                    }
                }
            }
        }
        private readonly List<Guid> roosterIsCanonical = new();

        private async Task EditorFocusOut(ContentSection section, ContentContainer container, FocusEventArgs args)
        {
            var value = await JSRuntime.InvokeAsync<string>("roosterGetContent", container.Id);
            container.Html = value;
        }

    }

    public class Blob : IBlob {
        public Guid UniqueId { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
    }

    //[Display(Name = "Add/Edit Hyperlink")]
    //public class Hyperlink {

    //    [Required]
    //    public string Caption { get; set; } = string.Empty;

    //    [Url]
    //    public string Url { get; set; } = string.Empty;
    //}
}
