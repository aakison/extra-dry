var loading = false;
var loaded = false;

var deferredIds = [];

function FormatCode(id) {
    var code = document.getElementById(id);
    Prism.highlightElement(code);
}

function LoadScriptAndFormat(id) {
    if (loaded) {
        FormatCode(id);
        return;
    }
    deferredIds.push(id);
    if (!loading) {
        loading = true;
        var baseUrl = "https://unpkg.com/prismjs@1.29.0";
        var head = document.getElementsByTagName('head')[0];
        // Add style sheet for basic formatting
        var link = document.createElement('link');
        link.href = `${baseUrl}/themes/prism.css`;
        link.rel = 'stylesheet';
        head.appendChild(link);
        // Script is not a module, so load globally...
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = `${baseUrl}/prism.js`;
        script.setAttribute("data-manual", "true");
        script.onload = function () {
            loaded = true;
            for (var i = 0; i < deferredIds.length; ++i) {
                FormatCode(deferredIds[i]);
            }
        };
        head.appendChild(script);
    }
}

//
// After a CodeBlock is rendered, the text in the <pre><code> block should be highlighted.
// Quite a bit of work, so use existing JS library instead of doing it in C#.
// As CodeBlocks might never appear, defer the load of the library until the first page that needs it.
//
export function CodeBlock_AfterRender(id) {
    LoadScriptAndFormat(id);
    //import * as AutoLoader from "https://unpkg.com/prismjs@1.29.0/plugins/autoloader/prism-autoloader.min.js";
}
