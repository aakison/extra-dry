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

    console.log("Scrolling the Nav...")

    var li = document.querySelector("nav li.active");
    if (li == null) {
        // Nothing is active, typically when first loaded and script is called before navigation rendered.
        return;
    }
    var ul = li.parentElement;
    var nav = ul.parentElement;
    var firstLi = ul.firstElementChild;
    var lastLi = ul.lastElementChild;

    var firstScreenPosition = nav.offsetLeft + 20;
    var lastScreenPosition = nav.clientWidth - li.clientWidth - 20;

    var percent = ilerp(li.offsetLeft, firstLi.offsetLeft, lastLi.offsetLeft)
    var position = lerp(percent, firstScreenPosition, lastScreenPosition);

    var offset = position - li.offsetLeft;

    nav.style.transform = `translateX(${offset}px)`;
}
