console.log(`Blazor Extra Dry by @aakison - https://github.com/akison/extra-dry License - https://github.com/akison/extra-dry/blob/main/LICENSE (MIT License)`);
export function TriCheck_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}
export function CopyHelper_CopyToClipboard(text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
        navigator.clipboard.writeText(text);
    }
}

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

        var body = document.getElementsByTagName('body')[0];
        // Script is not a module, so load globally...

        var script = document.createElement('script');
        var autoloader = document.createElement('script');

        script.type = 'text/javascript';
        script.src = `${baseUrl}/components/prism-core.min.js`;
        script.setAttribute("data-manual", "true");
        script.onload = function () {
            // Load the core script first, since it's async wait to load autoloader so that it has its dependencies.
            body.appendChild(autoloader);
        };

        autoloader.type = 'text/javascript';
        autoloader.src = `${baseUrl}/plugins/autoloader/prism-autoloader.min.js`;
        autoloader.onload = function () {
            loaded = true;
            for (var i = 0; i < deferredIds.length; ++i) {
                FormatCode(deferredIds[i]);
            }
        };

        body.appendChild(script);
    }
}

//
// After a CodeBlock is rendered, the text in the <pre><code> block should be highlighted.
// Quite a bit of work, so use existing JS library instead of doing it in C#.
// As CodeBlocks might never appear, defer the load of the library until the first page that needs it.
//
export function CodeBlock_AfterRender(id) {
    LoadScriptAndFormat(id);
}

export function DropDown_ScrollIntoView(id) {
    var element = document.getElementById(id);
    var options = { behavior: 'auto', block: 'nearest', inline: 'nearest' };
    if(element != null) {
        element.scrollIntoView(options);
    }
}
export function ToggleField_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}

//
// Best-effort detection of whether the current device is a mobile/tablet device, where the
// `capture` attribute on an <input type="file"> is honored by the OS to open the native
// camera app directly (as opposed to desktop browsers, where `capture` is ignored and a plain
// file picker is shown regardless of any webcam being present). There is no standard API that
// reports how `capture` will be handled, so this falls back to user-agent sniffing, which is
// acceptable here since the result only affects a UX convenience (hiding irrelevant buttons),
// not any security or functional decision.
//
export function ImageField_IsMobileDevice() {
    const uaData = navigator.userAgentData;
    if (uaData && typeof uaData.mobile === "boolean") {
        return uaData.mobile;
    }
    const userAgent = navigator.userAgent || "";
    return /Android|iPhone|iPad|iPod|Windows Phone|Mobi/i.test(userAgent);
}

//
// Programmatically opens the file/camera picker for the given <input type="file"> element, so
// that clicking anywhere on the control (not just its dedicated affordance button) triggers the
// same "best" input (e.g. rear camera on mobile, plain file picker on desktop).
//
export function ImageField_ClickInput(id) {
    const input = document.getElementById(id);
    if (input != null) {
        input.click();
    }
}


// Keyboard shortcut handling
var shortcutHandlers = new Map();

function parseShortcut(shortcut) {
    var parts = shortcut.toLowerCase().split('+');
    var result = {
        ctrl: false,
        shift: false,
        alt: false,
        meta: false,
        key: ''
    };

    parts.forEach(part => {
        part = part.trim();
        if (part === 'ctrl' || part === 'control') {
            result.ctrl = true;
        } else if (part === 'shift') {
            result.shift = true;
        } else if (part === 'alt') {
            result.alt = true;
        } else if (part === 'meta' || part === 'cmd' || part === 'command') {
            result.meta = true;
        } else {
            result.key = part;
        }
    });

    return result;
}

function handleKeyDown(event) {
    var key = event.key.toLowerCase();

    shortcutHandlers.forEach((dotnetRef, shortcut) => {
        var parsed = parseShortcut(shortcut);

        if (parsed.key === key &&
            parsed.ctrl === event.ctrlKey &&
            parsed.shift === event.shiftKey &&
            parsed.alt === event.altKey &&
            parsed.meta === event.metaKey) {

            event.preventDefault();
            dotnetRef.invokeMethodAsync('OnShortcutPressed');
        }
    });
}

export function Shortcut_RegisterShortcut(shortcut, dotnetRef) {
    if (!shortcutHandlers.has(shortcut)) {
        shortcutHandlers.set(shortcut, dotnetRef);

        // Only add the event listener once
        if (shortcutHandlers.size === 1) {
            document.addEventListener('keydown', handleKeyDown, true);
        }
    }
}

export function Shortcut_UnregisterShortcut(shortcut) {
    if (shortcutHandlers.has(shortcut)) {
        shortcutHandlers.delete(shortcut);

        // Remove the event listener if no more shortcuts are registered
        if (shortcutHandlers.size === 0) {
            document.removeEventListener('keydown', handleKeyDown, true);
        }
    }
}

