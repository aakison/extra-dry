var roosterEditors = [];
var roosterActiveDiv = null;

function startEditing(name) {
    console.log("cheat into JS");
    console.log(name);

    //var roosterjs = require('roosterjs');
    var editorDiv = document.getElementById(name);
    var editor = roosterjs.createEditor(editorDiv);
    editor.setContent('Welcome to <b>RoosterJs</b>!');

    editor.dryId = name;

    editorDiv.roosterEditor = editor;
    editorDiv.addEventListener("focus", roosterEditorFocus);

    roosterEditors.push(editor);
}

function roosterEditorFocus(focusArgs) {
    var editorDiv = focusArgs.target;
    var editor = editorDiv.roosterEditor;
    roosterActiveDiv = editorDiv;
}

function roosterToggleBold() {
    var editor = roosterActiveDiv.roosterEditor;
    if (editor) {
        roosterjs.toggleBold(editor);
    }
}

function roosterToggleItalic() {
    var editor = roosterActiveDiv.roosterEditor;
    if (editor) {
        roosterjs.toggleItalic(editor);
    }
}

function roosterCurrentEditorId() {
    return roosterActiveDiv.roosterEditor.dryId;
}
