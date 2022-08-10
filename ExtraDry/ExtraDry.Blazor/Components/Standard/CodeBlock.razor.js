//
// After a CodeBlock is rendered, the text in the <pre><code> block should be highlighted.
// Quite a bit of work, so use existing JS library instead of doing it in C#.
// As CodeBlocks might never appear, defer the load of the library until the first page that needs it.
//
export function CodeBlock_AfterRender(id) {
    console.log("defer load highlight", id);
    // Defer load of highlight library until actually needed on a page.
    //pre = document.getElementById(id);
    //console.log(pre);
    //import('./highlight-module.min.js').then(
    //    ({ hljs }) => {
    //        console.log(hljs);
    //        var div = document.getElementById(id);
    //        var pre = div.firstChild;
    //        var code = pre.firstChild;
    //        console.log(code);
    //        hljs.highlightElement(code);
    //    }
    //);
    //import('./Users.js').then(({ default: User, userDetail }) => {
    //    //This will be the code that depends on the module...
    //});
    ////hljs.HighlightAll();
    //console.log(hljs);
    //hljs.highlightElement(pre);
}
