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
        return [['bold', 'italic', 'strike', 'subscript', 'superscript']];
    }
    // Block mode
    const buttons = [
        ['bold', 'italic', 'strike', 'subscript', 'superscript', 'link'],
        ['formatBlock'],
        ['bulletList', 'numberedList', 'removeFormat']
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

function customizeLinkDialog(editor, linkBookmarks) {
    const core = editor.core;
    const dialogEl = core.context.dialog.modal;
    if (!dialogEl) return;

    const linkContent = dialogEl.querySelector('.se-dialog-content');
    if (!linkContent) return;

    // Change "URL to link" label
    const urlLabel = linkContent.querySelector('.se-dialog-form label');
    if (urlLabel && linkBookmarks) {
        urlLabel.textContent = 'Page or URL to link';
    }

    // Change "Bookmark" button title to "Pages" when bookmarks provided
    const bookmarkBtn = linkContent.querySelector('._se_bookmark_button');
    if (bookmarkBtn && linkBookmarks) {
        bookmarkBtn.title = 'Pages';
        bookmarkBtn.setAttribute('aria-label', 'Pages');
    }

    // Change "Submit" button to "Ok"
    const submitBtn = linkContent.querySelector('.se-btn-primary');
    if (submitBtn) {
        const span = submitBtn.querySelector('span');
        if (span) span.textContent = 'Ok';
        submitBtn.title = 'Ok';
        submitBtn.setAttribute('aria-label', 'Ok');
    }

    // Observe dialog inner to switch display from block to flex for vertical centering
    const origOpen = core.plugins.dialog.open;
    core.plugins.dialog.open = function (e, t) {
        const result = origOpen.call(this, e, t);
        if (dialogEl.style.display === 'block') {
            dialogEl.style.display = 'flex';
        }
        return result;
    };

    // Override the link open to trim whitespace from the selection BEFORE
    // the dialog captures it. When double-clicking to select a word, the
    // browser often includes a trailing space. SunEditor trims this from
    // "Text to display" but then replaces the full selection (including the
    // space) with the anchor, destroying the whitespace. By shrinking the
    // selection here — while it's still in the editor's wysiwyg area —
    // SunEditor saves the trimmed range and insertNode only replaces the
    // non-whitespace portion.
    const origLinkOpen = core.plugins.link.open;
    core.plugins.link.open = function () {
        const range = this.getRange();
        if (range && !range.collapsed) {
            let sc = range.startContainer;
            let so = range.startOffset;
            let ec = range.endContainer;
            let eo = range.endOffset;
            let changed = false;
            // Trim trailing whitespace from end of selection
            if (ec.nodeType === 3) {
                while (eo > 0 && /\s/.test(ec.textContent[eo - 1])) {
                    eo--;
                    changed = true;
                }
            }
            // Trim leading whitespace from start of selection
            if (sc.nodeType === 3) {
                while (so < sc.textContent.length && /\s/.test(sc.textContent[so])) {
                    so++;
                    changed = true;
                }
            }
            if (changed) {
                this.setRange(sc, so, ec, eo);
            }
        }
        return origLinkOpen.call(this);
    };
}

function setupCustomBookmarks(editor, linkBookmarks) {
    const core = editor.core;
    const anchor = core.plugins.anchor;
    if (!anchor) return;

    const bookmarkEntries = Object.entries(linkBookmarks);

    // Override createHeaderList to show site pages instead of document headers
    anchor.createHeaderList = function (ctx, selectMenuCtx, filterText) {
        const filter = (filterText || '').replace(/^#/, '').toLowerCase();
        const matches = [];
        let html = '';
        for (let i = 0; i < bookmarkEntries.length; i++) {
            const [name, url] = bookmarkEntries[i];
            if (filter && !name.toLowerCase().includes(filter)) continue;
            const el = document.createElement('div');
            el.textContent = name;
            el.setAttribute('data-url', url);
            el.setAttribute('data-index', i.toString());
            matches.push(el);
            html += '<li class="se-select-item" data-index="' + i + '">' + name + '</li>';
        }
        if (matches.length === 0) {
            core.plugins.selectMenu.close.call(core, selectMenuCtx);
        } else {
            core.plugins.selectMenu.createList(selectMenuCtx, matches, html);
            core.plugins.selectMenu.open.call(core, selectMenuCtx,
                anchor._setMenuListPosition.bind(core, ctx));
        }
    };

    // Only return true for #-prefixed bookmark anchors. Page paths like
    // "/contact-us" are regular URLs, not bookmarks. Returning true for
    // page paths causes SunEditor to mangle the URL through
    // t.substr(t.lastIndexOf("#")) when editing an existing link.
    anchor.selfPathBookmark = function (url) {
        if (!url) return false;
        return url.startsWith('#');
    };

    // Replace the bookmark button event listener since the original was
    // bound at init time and our override of onClick_bookmarkButton
    // won't be called through the old binding
    const ctx = core.context.anchor.caller.link;
    if (ctx && ctx.bookmarkButton) {
        const newBtn = ctx.bookmarkButton.cloneNode(true);
        ctx.bookmarkButton.parentNode.replaceChild(newBtn, ctx.bookmarkButton);
        ctx.bookmarkButton = newBtn;
        newBtn.addEventListener('click', function () {
            const isActive = core.util.hasClass(newBtn, 'active');
            if (isActive) {
                ctx.bookmark.style.display = 'none';
                core.util.removeClass(newBtn, 'active');
                core.plugins.selectMenu.close.call(core, core.context.selectMenu.callerContext);
            } else {
                ctx.bookmark.style.display = 'block';
                core.util.addClass(newBtn, 'active');
                anchor.createHeaderList.call(core, ctx, core.context.selectMenu.callerContext, '');
            }
        });
    }

    // Override setHeaderBookmark to fill in URL and text from site pages
    anchor.setHeaderBookmark = function (item) {
        const callerCtx = core.context.anchor.callerContext
            || core.context.anchor.caller.link;
        if (!callerCtx) return;
        const index = parseInt(item.getAttribute('data-index'), 10);
        if (isNaN(index) || index >= bookmarkEntries.length) return;
        const [name, url] = bookmarkEntries[index];
        callerCtx.urlInput.value = url;
        if (!callerCtx.anchorText.value.trim()) {
            callerCtx.anchorText.value = name;
        }
        callerCtx.bookmark.style.display = 'none';
        core.util.removeClass(callerCtx.bookmarkButton, 'active');
        core.plugins.anchor.setLinkPreview.call(core, callerCtx, url);
        core.plugins.selectMenu.close.call(core, core.context.selectMenu.callerContext);
        callerCtx.urlInput.focus();
    };

    // Also intercept selectMenu click to ensure our handler fires
    const selectMenuCtx = core.context.selectMenu.caller.link;
    if (selectMenuCtx) {
        const listEl = selectMenuCtx.form;
        if (listEl) {
            listEl.addEventListener('click', function (e) {
                let target = e.target;
                let dataIndex = null;
                while (target && target !== listEl) {
                    dataIndex = target.getAttribute('data-index');
                    if (dataIndex !== null) break;
                    target = target.parentNode;
                }
                if (dataIndex === null) return;
                const idx = parseInt(dataIndex, 10);
                if (isNaN(idx) || idx >= bookmarkEntries.length) return;
                const callerCtx = core.context.anchor.callerContext
                    || core.context.anchor.caller.link;
                if (!callerCtx) return;
                const [name, url] = bookmarkEntries[idx];
                callerCtx.urlInput.value = url;
                if (!callerCtx.anchorText.value.trim()) {
                    callerCtx.anchorText.value = name;
                }
                callerCtx.bookmark.style.display = 'none';
                core.util.removeClass(callerCtx.bookmarkButton, 'active');
                core.plugins.anchor.setLinkPreview.call(core, callerCtx, url);
                core.plugins.selectMenu.close.call(core, core.context.selectMenu.callerContext);
                callerCtx.urlInput.focus();
            });
        }
    }
}

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
    const linkBookmarks = options?.linkBookmarks || null;
    const buttonList = buildButtonList(mode, enableImage);

    const isCharacterMode = mode === 'Character';

    const editor = SUNEDITOR.create(element, {
        plugins: buildCustomPlugins(),
        buttonList: buttonList,
        mode: 'inline',
        formats: isCharacterMode ? ['p'] : ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'blockquote'],
        placeholder: placeholder,
        width: '100%',
        minHeight: isCharacterMode ? '1em' : '100px',
        defaultStyle: 'font-family: inherit; font-size: inherit;',
        linkNoPrefix: true,
    });

    // In Character mode, prevent Enter/Tab and strip newlines from pasted content.
    // Use capture phase so our handler fires before SunEditor's keydown handler.
    if (isCharacterMode) {
        const wysiwygBody = editor.core.context.element.wysiwyg;
        if (wysiwygBody) {
            wysiwygBody.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                }
                if (e.key === 'Tab') {
                    e.stopImmediatePropagation();
                }
            }, true);
            wysiwygBody.addEventListener('paste', function (e) {
                const paste = (e.clipboardData || window.clipboardData)?.getData('text');
                if (paste && /[\r\n]/.test(paste)) {
                    e.preventDefault();
                    const cleaned = paste.replace(/[\r\n]+/g, ' ');
                    document.execCommand('insertText', false, cleaned);
                }
            });
        }
    }

    customizeLinkDialog(editor, linkBookmarks);

    if (linkBookmarks) {
        setupCustomBookmarks(editor, linkBookmarks);
    }

    // Prevent navigation when clicking links in the editor. All link clicks
    // should show the edit controller, not navigate. SunEditor already
    // prevents navigation for external links but relative links can trigger
    // Blazor's client-side navigation before the controller appears.
    const wysiwygEl = editor.core.context.element.wysiwyg;
    if (wysiwygEl) {
        wysiwygEl.addEventListener('click', function (e) {
            const anchor = e.target.closest ? e.target.closest('a') : null;
            if (anchor) {
                e.preventDefault();
            }
        });
    }

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
