console.log(`Blazor Extra Dry by @aakison - https://github.com/akison/extra-dry License - https://github.com/akison/extra-dry/blob/main/LICENSE (MIT License)`);

export function TriCheck_SetIndeterminate(id, value) {
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
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

// Keep the old function names for backward compatibility
export function Button_RegisterShortcut(shortcut, dotnetRef) {
    return Shortcut_RegisterShortcut(shortcut, dotnetRef);
}

export function Button_UnregisterShortcut(shortcut) {
    return Shortcut_UnregisterShortcut(shortcut);
}