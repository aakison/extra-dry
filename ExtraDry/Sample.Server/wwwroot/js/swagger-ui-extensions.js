// Works with CSS to to show/hide the examples/schema for the failure data types.

function ancestorWithClass(node, className) {
    if (node == null) {
        return null;
    }
    else if (node.classList?.contains(className)) {
        return node;
    }
    else {
        return ancestorWithClass(node.parentNode, className);
    }
}

document.addEventListener("click", function (event) {
    var titleNode = ancestorWithClass(event.target, "response-col_description__inner");
    var responseNode = ancestorWithClass(titleNode, "response");
    if (responseNode != null && responseNode.dataset.code >= 400) {
        responseNode.classList.toggle("show-model");
    }

    if (event.target.classList?.contains("renderedMarkdown") && !event.target.classList?.contains("detailed")) {
        event.target.classList.add("detailed");
    }
});
