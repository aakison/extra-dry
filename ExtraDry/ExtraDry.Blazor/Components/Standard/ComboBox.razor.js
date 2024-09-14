export function DropDown_ScrollIntoView(id) {
    var element = document.getElementById(id);
    var options = { behavior: 'auto', block: 'nearest', inline: 'nearest' };
    if(element != null) {
        element.scrollIntoView(options);
    }
}
