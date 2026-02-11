const editors = {};
let sunEditorLoaded = false;
let sunEditorLoading = false;
let loadCallbacks = [];

function getBasePath() {
    const scripts = document.querySelectorAll('script[src]');
    for (const script of scripts) {
        const src = script.getAttribute('src');
        if (src && src.includes('_content/ExtraDry.Blazor')) {
            const idx = src.indexOf('_content/ExtraDry.Blazor');
            return src.substring(0, idx) + '_content/ExtraDry.Blazor';
        }
    }
    return '/_content/ExtraDry.Blazor';
}

function ensureSunEditorLoaded() {
    return new Promise((resolve, reject) => {
        if (sunEditorLoaded && window.SUNEDITOR) {
            resolve();
            return;
        }
        if (sunEditorLoading) {
            loadCallbacks.push({ resolve, reject });
            return;
        }
        sunEditorLoading = true;
        loadCallbacks.push({ resolve, reject });

        const basePath = getBasePath();
        const head = document.getElementsByTagName('head')[0];

        // Load CSS
        if (!document.querySelector('link[href*="suneditor.min.css"]')) {
            const link = document.createElement('link');
            link.rel = 'stylesheet';
            link.href = basePath + '/js/suneditor/dist/css/suneditor.min.css';
            head.appendChild(link);
        }

        // Load JS
        if (!document.querySelector('script[src*="suneditor.min.js"]')) {
            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = basePath + '/js/suneditor/dist/suneditor.min.js';
            script.onload = () => {
                sunEditorLoaded = true;
                sunEditorLoading = false;
                for (const cb of loadCallbacks) {
                    cb.resolve();
                }
                loadCallbacks = [];
            };
            script.onerror = (err) => {
                sunEditorLoading = false;
                for (const cb of loadCallbacks) {
                    cb.reject(err);
                }
                loadCallbacks = [];
            };
            document.body.appendChild(script);
        } else {
            // Script tag exists but may not have loaded yet
            const checkInterval = setInterval(() => {
                if (window.SUNEDITOR) {
                    clearInterval(checkInterval);
                    sunEditorLoaded = true;
                    sunEditorLoading = false;
                    for (const cb of loadCallbacks) {
                        cb.resolve();
                    }
                    loadCallbacks = [];
                }
            }, 50);
        }
    });
}

function buildButtonList(mode, enableImage) {
    if (mode === 'Character') {
        return [['bold', 'italic', 'strike', 'link']];
    }
    // Block mode
    const buttons = [
        ['bold', 'italic', 'strike', 'link'],
        ['formatBlock', 'blockquote'],
        ['list'],
        ['codeView']
    ];
    if (enableImage) {
        buttons.push(['image']);
    }
    return buttons;
}

let debounceTimers = {};

export async function initialize(elementId, dotNetRef, options) {
    await ensureSunEditorLoaded();

    const element = document.getElementById(elementId);
    if (!element) {
        console.error('[markdown-editor] Element not found: ' + elementId);
        return;
    }

    const mode = options?.mode || 'Block';
    const enableImage = options?.enableImage || false;
    const placeholder = options?.placeholder || '';
    const buttonList = buildButtonList(mode, enableImage);

    const editor = SUNEDITOR.create(element, {
        buttonList: buttonList,
        mode: 'balloon',
        placeholder: placeholder,
        width: '100%',
        minHeight: '100px',
        defaultStyle: 'font-family: inherit; font-size: inherit;'
    });

    editor.onChange = (contents) => {
        if (debounceTimers[elementId]) {
            clearTimeout(debounceTimers[elementId]);
        }
        debounceTimers[elementId] = setTimeout(() => {
            dotNetRef.invokeMethodAsync('OnContentChanged', contents);
            delete debounceTimers[elementId];
        }, 300);
    };

    editors[elementId] = { editor, dotNetRef };
}

export function getContent(elementId) {
    const entry = editors[elementId];
    if (!entry) {
        return '';
    }
    return entry.editor.getContents() || '';
}

export function setContent(elementId, html) {
    const entry = editors[elementId];
    if (!entry) {
        return;
    }
    entry.editor.setContents(html);
}

export function destroy(elementId) {
    const entry = editors[elementId];
    if (!entry) {
        return;
    }
    if (debounceTimers[elementId]) {
        clearTimeout(debounceTimers[elementId]);
        delete debounceTimers[elementId];
    }
    entry.editor.destroy();
    delete editors[elementId];
}
