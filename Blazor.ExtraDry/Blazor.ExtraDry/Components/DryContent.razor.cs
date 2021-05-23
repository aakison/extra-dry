#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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
        private IJSRuntime? JSRuntime { get; set; }

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

        [Command]
        public async Task ToggleBold()
        {
            await JSRuntime!.InvokeVoidAsync("roosterToggleBold");
        }

        [Command]
        public async Task ToggleItalic()
        {
            await JSRuntime!.InvokeVoidAsync("roosterToggleItalic");
        }

        public ContentSection? CurrentSection { get; set; }

        public ContentContainer? CurrentContainer { get; set; }

        public ContentTheme CurrentSectionTheme { get; set; }

        private void SectionThemeChanged(EventArgs args)
        {
            if(CurrentSection != null) {
                CurrentSection.Theme = CurrentSectionTheme;
            }
        }

        private void EditorFocus(FocusEventArgs args, ContentSection section, ContentContainer container)
        {
            Console.WriteLine($"Editor Focus: {args.Type}, {container.Id}");
            CurrentSection = section;
            CurrentContainer = container;
            CurrentSectionTheme = section.Theme;
        }

    }
}
