//
// After a CodeBlock is rendered, the text in the <pre><code> block should be highlighted.
// Quite a bit of work, so use existing JS library instead of doing it in C#.
// As CodeBlocks might never appear, defer the load of the library until the first page that needs it.
//
export function CodeBlock_AfterRender(id) {
    // Defer load of highlight library until actually needed on a page.
    import('./highlight-module.min.js').then(
        ({ hljs }) => {
            var selector = `#${id} pre code`;
            console.log("highlight.js", selector);
            var code = document.querySelector(selector);
            hljs.highlightElement(code);
        }
    );
}
