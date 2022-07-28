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

function DryHorizontalScrollNav() {
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
