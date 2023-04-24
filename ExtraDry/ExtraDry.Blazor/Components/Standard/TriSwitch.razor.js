export function TriSwitch_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}
