console.log(`Blazor Extra Dry by @aakison - https://github.com/fmi-works/extra-dry License - https://github.com/fmi-works/extra-dry/blob/main/LICENSE (MIT License)`);


function lerp(value, from, to) {
    return value * (to - from) + from;
}

function ilerp(value, from, to) {
    if (value < from) {
        return from;
    }
    if (value > to) {
        return to;
    }
    return (value - from) / (to - from);
}

export function DryNavigation_HorizontalScrollNav() {
    var li = document.querySelector("nav li.active");
    if (li == null) {
        // Nothing is active, typically when first loaded and script is called before navigation rendered.
        return;
    }
    var ul = li.parentElement;
    var nav = ul.parentElement;

    if (nav.offsetWidth >= ul.offsetWidth) {
        ul.style.transform = "translateX(0px)";
    }
    else {
        var firstLi = ul.firstElementChild;
        var lastLi = ul.lastElementChild;

        var firstScreenPosition = firstLi.offsetLeft;
        var lastScreenPosition = nav.clientWidth - lastLi.clientWidth;

        var percent = ilerp(li.offsetLeft, firstLi.offsetLeft, lastLi.offsetLeft);
        var position = lerp(percent, firstScreenPosition, lastScreenPosition);

        var offset = position - li.offsetLeft;

        ul.style.transform = `translateX(${offset}px)`;
    }
}

export function TriCheck_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}

export function TriSwitch_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}

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
            var code = document.querySelector(selector);
            hljs.highlightElement(code);
        }
    );
}

export function DropDown_ScrollIntoView(id) {
    var element = document.getElementById(id);
    var options = { behavior: 'auto', block: 'nearest', inline: 'nearest' };
    if(element != null) {
        element.scrollIntoView(options);
    }
}
