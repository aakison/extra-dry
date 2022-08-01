function extraDry_setIndeterminate(id, value) {
    console.log("setIndeterminate", id, value);
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}
