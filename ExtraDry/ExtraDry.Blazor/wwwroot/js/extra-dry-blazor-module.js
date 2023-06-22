console.log(`Blazor Extra Dry by @aakison - https://github.com/fmi-works/extra-dry License - https://github.com/fmi-works/extra-dry/blob/main/LICENSE (MIT License)`);


export function TriCheck_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}

export function DropDown_ScrollIntoView(id) {
    var element = document.getElementById(id);
    var options = { behavior: 'auto', block: 'nearest', inline: 'nearest' };
    if(element != null) {
        element.scrollIntoView(options);
    }
}
