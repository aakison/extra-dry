var roosterEditors = [];
var roosterActiveDiv = null;

var SamplePlugin = (function () {

    function SamplePlugin() {
    }

    //function
    SamplePlugin.prototype.getName = function () {
        return 'SamplePlugin';
    };

    SamplePlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };

    SamplePlugin.prototype.dispose = function () {
        this.editor = null;
    };

    SamplePlugin.prototype.onPluginEvent = function (event) {
        // Check if the event is BeforePasteEvent
        if (event.eventType == 10 /*PluginEventType.BeforePaste*/) {
            let beforePasteEvent = /*BeforePasteEvent*/ event;
            // Check if pasting image
            if (beforePasteEvent.clipboardData.image != null) {
                let image = beforePasteEvent.clipboardData.image;
                let placeholder = this.createPlaceholder(image);

                // Modify the pasting content and option
                let originalImage = beforePasteEvent.fragment.children[0];
                let container = document.createElement("div");
                container.appendChild(originalImage);
                beforePasteEvent.fragment.appendChild(container);
                container.appendChild(placeholder);
                beforePasteEvent.clipboardData.html = placeholder.outerHTML;
                //beforePasteEvent.clipboardData.image = null;
                beforePasteEvent.pasteOption = 0 /*PasteOption.PasteHtml*/;

                // Start upload image and handle async result
                DotNet.invokeMethodAsync("Blazor.ExtraDry", "UploadImage", beforePasteEvent.clipboardData.imageDataUri).then((blob) => {
                    // Check editor availability in async callback
                    if(this.editor) {
                        originalImage.src = blob.url;
                        placeholder.remove();
                    }
                });
            }
        }
    }

    SamplePlugin.prototype.createPlaceholder = function(img) {
        var paragraph = document.createElement("P");
        paragraph.style = "color: white;";
        paragraph.innerHTML = "Uploading...";
        return paragraph;
    }

    return SamplePlugin;
}());

function startEditing(name) {
    //var roosterjs = require('roosterjs');
    var editorDiv = document.getElementById(name);
    var editor = roosterjs.createEditor(editorDiv, new SamplePlugin());

    editor.dryId = name;

    editorDiv.roosterEditor = editor;
    editorDiv.addEventListener("focus", roosterEditorFocus);

    roosterEditors.push(editor);
}

function roosterEditorFocus(focusArgs) {
    var editorDiv = focusArgs.target;
    if(roosterActiveDiv) {
        roosterActiveDiv.classList.remove("rooster-selected");
        roosterActiveDiv.parentNode.classList.remove("rooster-selected");
        roosterActiveDiv.parentNode.parentNode.classList.remove("rooster-selected");
    }
    roosterActiveDiv = editorDiv;
    if(roosterActiveDiv) {
        roosterActiveDiv.classList.add("rooster-selected");
        roosterActiveDiv.parentNode.classList.add("rooster-selected");
        roosterActiveDiv.parentNode.parentNode.classList.add("rooster-selected");
    }
}

function roosterToggleBold() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleBold(editor);
    }
}

function roosterToggleItalic() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleItalic(editor);
    }
}

function roosterToggleHeader(level) {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleHeader(editor, level);
    }
}

function roosterClearFormat() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.clearFormat(editor);
    }
}

function roosterSetContent(id, html) {
    var div = document.getElementById(id);
    if(div) {
        div.innerHTML = html;
    }
}

function roosterHorizontalRule() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        editor.insertContent("<hr />");
    }
}

function roosterInsertHyperlink(className, title, hyperlink) {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
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
