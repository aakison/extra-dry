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

function roosterSetContent(id, html) {
    var div = document.getElementById(id);
    if (div) {
        div.innerHTML = html;
    }
}

function roosterHorizontalRule() {
    var editor = roosterActiveDiv.roosterEditor;
    if (editor) {
        editor.insertContent("<hr />");
    }
}

function roosterInsertHyperlink(className, title, hyperlink) {
    var editor = roosterActiveDiv.roosterEditor;
    console.log(title);
    if (editor) {
        editor.insertContent(`<a class=''${className}'' href=''${hyperlink}''>${title}</a>`);
    }
}

function roosterGetContent(id) {
    var div = document.getElementById(id);
    return div.innerHTML;
}

//// Adapted from https://github.com/microsoft/roosterjs/blob/cfe4f3515833480b66f4c0214f93bc337410bddb/packages/roosterjs-editor-api/lib/format/toggleHeader.ts
//function roosterTestSanitize(editor) {
//    var editor = roosterActiveDiv.roosterEditor;

//    // Don't allow undo to de-sanitized state...
//    editor.focus();

//    //let wrapped = false;
//    //editor.queryElements('*', 0 /* QueryScope.Body */, header => {
//    //    if (!wrapped) {
//    //        editor.getDocument().execCommand("formatBlock" /* DocumentCommand.FormatBlock */, false, '<DIV>');
//    //        wrapped = true;
//    //    }

//    //    let div = editor.getDocument().createElement('div');
//    //    while (header.firstChild) {
//    //        div.appendChild(header.firstChild);
//    //    }
//    //    editor.replaceNode(header, div);
//    //});

//    let traverser = editor.getBodyTraverser();
//    let blockElement = traverser ? traverser.currentBlockElement : null;
//    let sanitizer = new roosterjs.HtmlSanitizer({
//        cssStyleCallbacks: {
//            'font-size': () => { console.log("font-size Callback"); return false; },
//            'display': () => { console.log("display Callback"); return false; },
//            'color': () => { console.log("color Callback"); return false; },
//        },
//    });
//    while (blockElement) {
//        console.log(blockElement);
//        let element = blockElement.collapseToSingleElement();

//        blockElement.innerHTML = blockElement.getTextContent();
//        sanitizer.sanitize(element);
//        blockElement = traverser.getNextBlockElement();
//    }
//}
