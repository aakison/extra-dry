var roosterEditors = [];
var roosterActiveDiv = null;

function startEditing(name) {
    //var roosterjs = require('roosterjs');
    var editorDiv = document.getElementById(name);
    var editor = roosterjs.createEditor(editorDiv);

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

function roosterToggleHeader(level) {
    var editor = roosterActiveDiv.roosterEditor;
    if (editor) {
        roosterjs.toggleHeader(editor, level);
    }
}

function roosterClearFormat() {
    var editor = roosterActiveDiv.roosterEditor;
    if (editor) {
        roosterjs.clearFormat(editor);
    }
}

function roosterGetContent(id) {
    var div = document.getElementById(id);
    return div.innerHTML;
}

function roosterSanitize() {
    var sanitizer = new roosterjs.HtmlSanitizer();
    roosterEditors.forEach(e => e.setContent(sanitizer.sanitize(e.getContent())));
}

// Adapted from https://github.com/microsoft/roosterjs/blob/cfe4f3515833480b66f4c0214f93bc337410bddb/packages/roosterjs-editor-api/lib/format/toggleHeader.ts
function roosterTestSanitize(editor) {
    var editor = roosterActiveDiv.roosterEditor;

    editor.addUndoSnapshot(() => {
        editor.focus();

        let wrapped = false;
        editor.queryElements('*', 1 /* QueryScope.OnSelection */, header => {
            if (!wrapped) {
                editor.getDocument().execCommand("formatBlock" /* DocumentCommand.FormatBlock */, false, '<DIV>');
                wrapped = true;
            }

            let div = editor.getDocument().createElement('div');
            while (header.firstChild) {
                div.appendChild(header.firstChild);
            }
            editor.replaceNode(header, div);
        });

        let traverser = editor.getSelectionTraverser();
        let blockElement = traverser ? traverser.currentBlockElement : null;
        let sanitizer = new roosterjs.HtmlSanitizer({
            cssStyleCallbacks: {
                '*': () => false,
            },
        });
        while (blockElement) {
            let element = blockElement.collapseToSingleElement();
            sanitizer.sanitize(element);
            blockElement = traverser.getNextBlockElement();
        }
        //editor.getDocument().execCommand(DocumentCommand.FormatBlock, false, `<H${level}>`);
    }, "Format" /* ChangeSource.Format */);
}