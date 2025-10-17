
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

