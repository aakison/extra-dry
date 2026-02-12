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
        return [['bold', 'italic', 'strike']];
    }
    // Block mode
    const buttons = [
        ['bold', 'italic', 'strike', 'subscript', 'superscript', 'link'],
        ['formatBlock'],
        ['bulletList', 'numberedList']
    ];
    if (enableImage) {
        buttons.push(['image']);
    }
    return buttons;
}

function buildCustomPlugins() {
    return [
        {
            name: 'bulletList',
            display: 'command',
            title: 'Bullet List',
            buttonClass: '',
            innerHTML: '<svg viewBox="0 0 24 24"><path d="M4 10.5c-.83 0-1.5.67-1.5 1.5s.67 1.5 1.5 1.5 1.5-.67 1.5-1.5-.67-1.5-1.5-1.5zm0-6c-.83 0-1.5.67-1.5 1.5S3.17 7.5 4 7.5 5.5 6.83 5.5 6 4.83 4.5 4 4.5zm0 12c-.83 0-1.5.68-1.5 1.5s.68 1.5 1.5 1.5 1.5-.68 1.5-1.5-.67-1.5-1.5-1.5zM8 19h12v-2H8v2zm0-6h12v-2H8v2zm0-8v2h12V5H8z"/></svg>',
            add: function (core, targetElement) {
                core.context.bulletList = { targetButton: targetElement };
            },
            active: function (element) {
                if (element && /^UL$/i.test(element.nodeName)) {
                    this.util.addClass(this.context.bulletList.targetButton, 'active');
                    return true;
                }
                this.util.removeClass(this.context.bulletList.targetButton, 'active');
                return false;
            },
            action: function () {
                const range = this.plugins.list.editList.call(this, 'UL', null, false);
                if (range) this.setRange(range.sc, range.so, range.ec, range.eo);
                this.history.push(false);
            }
        },
        {
            name: 'numberedList',
            display: 'command',
            title: 'Numbered List',
            buttonClass: '',
            innerHTML: '<svg viewBox="0 0 24 24"><path d="M2 17h2v.5H3v1h1v.5H2v1h3v-4H2v1zm1-9h1V4H2v1h1v3zm-1 3h1.8L2 13.1v.9h3v-1H3.2L5 10.9V10H2v1zm5-6v2h14V5H7zm0 14h14v-2H7v2zm0-6h14v-2H7v2z"/></svg>',
            add: function (core, targetElement) {
                core.context.numberedList = { targetButton: targetElement };
            },
            active: function (element) {
                if (element && /^OL$/i.test(element.nodeName)) {
                    this.util.addClass(this.context.numberedList.targetButton, 'active');
                    return true;
                }
                this.util.removeClass(this.context.numberedList.targetButton, 'active');
                return false;
            },
            action: function () {
                const range = this.plugins.list.editList.call(this, 'OL', null, false);
                if (range) this.setRange(range.sc, range.so, range.ec, range.eo);
                this.history.push(false);
            }
        }
    ];
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
        plugins: buildCustomPlugins(),
        buttonList: buttonList,
        mode: 'inline',
        formats: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'blockquote'],
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
