function ScrollNav() {

    var activeNavItem = document.querySelector("nav.top li.active");
    if (activeNavItem == null) {
        // Nothing is active, typically when first loaded and script is called before navigation rendered.
        return;
    }
    var parentItem = activeNavItem.parentElement.parentElement;
    var container = parentItem.parentElement;
    var firstNavItem = container.firstElementChild.children[1].firstElementChild;
    var lastNavItem = container.lastElementChild.lastElementChild.lastElementChild;

    var firstScreenPosition = container.offsetLeft + 20;
    var lastScreenPosition = container.clientWidth - activeNavItem.clientWidth - 20;

    var percent = ilerp(activeNavItem.offsetLeft, firstNavItem.offsetLeft, lastNavItem.offsetLeft)
    var position = lerp(percent, firstScreenPosition, lastScreenPosition);

    var offset = position - activeNavItem.offsetLeft;

    container.style.transform = `translateX(${offset}px)`;

    var group = parentItem.firstElementChild;
    var headerLeft = activeNavItem.offsetLeft - group.offsetLeft;
    group.style.transform = `translateX(${headerLeft}px)`
}
