function startEditing(name) {
    console.log("cheat into JS");
    console.log(name);

    //var roosterjs = require('roosterjs');
    var editorDiv = document.getElementById(name);
    var editor = roosterjs.createEditor(editorDiv);
    editor.setContent('Welcome to <b>RoosterJs</b>!');

}
