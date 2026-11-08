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

function showEditorToast(anchor, message) {
    const host = anchor.closest('.input') || anchor.parentElement;
    if (!host) return;
    if (!document.getElementById('markdown-editor-toast-styles')) {
        const style = document.createElement('style');
        style.id = 'markdown-editor-toast-styles';
        style.textContent = `
            .markdown-editor-toast {
                position: absolute; right: 0.5rem; bottom: 0.5rem; z-index: 10;
                padding: 0.25rem 0.75rem; border-radius: 0.25rem; font-size: 0.85em;
                color: var(--warning-dark); background: var(--warning-light);
                border: 1px solid var(--warning-dark); pointer-events: none;
                animation: markdown-toast-in 0.4s ease-out both;
            }
            .markdown-editor-toast.leaving { animation: markdown-toast-out 0.4s ease-in both; }
            @keyframes markdown-toast-in {
                0% { opacity: 0; transform: translateX(0); }
                20% { opacity: 1; transform: translateX(-6px); }
                40% { transform: translateX(6px); }
                60% { transform: translateX(-4px); }
                80% { transform: translateX(3px); }
                100% { opacity: 1; transform: translateX(0); }
            }
            @keyframes markdown-toast-out {
                from { opacity: 1; transform: translateY(0); }
                to { opacity: 0; transform: translateY(-1rem); }
            }`;
        document.head.appendChild(style);
    }
    if (getComputedStyle(host).position === 'static') {
        host.style.position = 'relative';
    }
    host.querySelectorAll(':scope > .markdown-editor-toast').forEach(t => t.remove());
    const toast = document.createElement('div');
    toast.className = 'markdown-editor-toast';
    toast.setAttribute('role', 'status');
    toast.textContent = message;
    host.appendChild(toast);
    setTimeout(() => toast.classList.add('leaving'), 2000);
    setTimeout(() => toast.remove(), 2400);
}

// Shows an "Uploading Image..." alert if the upload takes longer than 100ms. Returns a function
// to call when the upload finishes: it cancels the alert if it was never shown, otherwise it
// swipes the alert up and fades it out.
function showUploadingToast(anchor) {
    const host = anchor.closest('.input') || anchor.parentElement;
    if (!host) return () => { };
    if (!document.getElementById('markdown-editor-upload-toast-styles')) {
        const style = document.createElement('style');
        style.id = 'markdown-editor-upload-toast-styles';
        style.textContent = `
            .markdown-editor-upload-toast {
                position: absolute; right: 0.5rem; bottom: 0.5rem; z-index: 10;
                padding: 0.25rem 0.75rem; border-radius: 0.25rem; font-size: 0.85em;
                color: var(--info-dark); background: var(--info-light);
                border: 1px solid var(--info-dark); pointer-events: none;
                animation: markdown-upload-toast-in 0.4s ease-out both;
            }
            .markdown-editor-upload-toast.leaving { animation: markdown-upload-toast-out 0.4s ease-in both; }
            @keyframes markdown-upload-toast-in {
                from { opacity: 0; transform: translateY(1rem); }
                to { opacity: 1; transform: translateY(0); }
            }
            @keyframes markdown-upload-toast-out {
                from { opacity: 1; transform: translateY(0); }
                to { opacity: 0; transform: translateY(-1rem); }
            }`;
        document.head.appendChild(style);
    }
    let toast = null;
    let finished = false;
    // Resolves after the 100ms delay and, if the alert was shown, after it has been painted. The
    // upload runs on the same thread as the page (Blazor WASM), so callers must await this before
    // starting heavy work or the alert would never get a chance to render.
    const shown = new Promise(resolve => {
        setTimeout(() => {
            if (finished) { resolve(); return; }
            if (getComputedStyle(host).position === 'static') {
                host.style.position = 'relative';
            }
            toast = document.createElement('div');
            toast.className = 'markdown-editor-upload-toast';
            toast.setAttribute('role', 'status');
            toast.textContent = 'Uploading Image...';
            host.appendChild(toast);
            requestAnimationFrame(() => requestAnimationFrame(resolve));
        }, 100);
    });
    const done = () => {
        finished = true;
        if (!toast) return;
        const visible = toast;
        toast = null;
        visible.classList.add('leaving');
        setTimeout(() => visible.remove(), 400);
    };
    return { shown, done };
}

// Resolves when the images with the given sources, once inserted, have finished loading.
function waitForImages(root, urls) {
    const pending = Array.from(root.querySelectorAll('img'))
        .filter(img => urls.includes(img.getAttribute('src')) && !img.complete);
    const loaded = Promise.all(pending.map(img => new Promise(resolve => {
        img.addEventListener('load', resolve, { once: true });
        img.addEventListener('error', resolve, { once: true });
    })));
    return Promise.race([loaded, new Promise(resolve => setTimeout(resolve, 60000))]);
}

function buildButtonList(mode, enableImage, hasImageFiles) {
    if (mode === 'Character') {
        return [['bold', 'italic', 'strike', 'subscript', 'superscript']];
    }
    // Block mode
    const buttons = [
        ['bold', 'italic', 'link'],
        ['formatBlock'],
        ['moreMenu']
    ];
    if (hasImageFiles || enableImage) {
        buttons[0].push('image');
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
        },
        buildMenuPlugin('moreMenu', 'More',
            '<svg viewBox="0 0 24 24"><circle cx="5" cy="12" r="2"/><circle cx="12" cy="12" r="2"/><circle cx="19" cy="12" r="2"/></svg>',
            [
                styleMenuItem('Strikethrough', ['DEL', 'S', 'STRIKE'], 'DEL', null,
                    '<text x="3" y="18" font-size="16" font-weight="bold" font-family="sans-serif">S</text><rect x="2" y="11" width="20" height="2"/>'),
                styleMenuItem('Superscript', ['SUP'], 'SUP', ['SUB'],
                    '<text x="2" y="20" font-size="16" font-weight="bold" font-family="sans-serif">X</text><text x="14" y="10" font-size="10" font-weight="bold" font-family="sans-serif">2</text>'),
                styleMenuItem('Subscript', ['SUB'], 'SUB', ['SUP'],
                    '<text x="2" y="16" font-size="16" font-weight="bold" font-family="sans-serif">X</text><text x="14" y="22" font-size="10" font-weight="bold" font-family="sans-serif">2</text>'),
                { divider: true },
                {
                    label: 'Bullets',
                    icon: '<path d="M4 10.5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3zm0-6a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3zm0 12a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3zM8 19h12v-2H8zm0-6h12v-2H8zm0-8v2h12V5z"/>',
                    run(core) { applyList(core, 'UL'); }
                },
                {
                    label: 'Numbers',
                    icon: '<path d="M2 17h2v.5H3v1h1v.5H2v1h3v-4H2zm1-9h1V4H2v1h1zm-1 3h1.8L2 13.1v.9h3v-1H3.2L5 10.9V10H2zm5-6v2h14V5zm0 14h14v-2H7zm0-6h14v-2H7z"/>',
                    run(core) { applyList(core, 'OL'); }
                },
                { divider: true },
                {
                    label: 'Clear',
                    icon: '<path d="M16.24 3.56l4.95 4.94c.78.79.78 2.05 0 2.84L12 20.53a4.01 4.01 0 0 1-5.66 0L2.81 17c-.78-.79-.78-2.05 0-2.84l10.6-10.6c.79-.78 2.05-.78 2.83 0zM4.22 15.58l3.54 3.53c.78.79 2.04.79 2.83 0l3.53-3.53-6.36-6.36-3.54 3.36c-.78.79-.78 2.05 0 2.84z"/>',
                    run(core) { core.removeFormat(); core.focus(); core.history.push(false); }
                },
            ]),
    ];
}

// Menu item that toggles an inline style element (e.g. DEL, SUB, SUP) on the selection.
function styleMenuItem(label, tags, create, clears, icon) {
    return {
        label: label,
        icon: icon,
        run(core) {
            const node = core.getSelectionNode();
            const active = core.util.getParentElement(node, e => tags.includes((e.nodeName || '').toUpperCase()));
            if (active) {
                core.nodeChange(null, null, tags, false);
            }
            else {
                core.nodeChange(core.util.createElement(create), null, clears, false);
            }
        }
    };
}

function applyList(core, tag) {
    const range = core.plugins.list.editList.call(core, tag, null, false);
    if (range) core.setRange(range.sc, range.so, range.ec, range.eo);
    core.history.push(false);
}

// Builds a SunEditor submenu plugin: one toolbar button that opens a dropdown of text commands.
function buildMenuPlugin(name, title, icon, items) {
    return {
        name: name,
        display: 'submenu',
        title: title,
        buttonClass: '',
        innerHTML: icon,
        add: function (core, targetElement) {
            const listDiv = core.util.createElement('DIV');
            listDiv.className = 'se-submenu se-list-layer';
            const buttons = items.map((item, index) => item.divider
                ? '<li role="separator" style="border-top:1px solid #ddd;margin:4px 0;height:0;"></li>'
                : `<li><button type="button" class="se-btn-list" data-index="${index}" title="${item.label}" style="display:flex;align-items:center;justify-content:flex-start;text-align:left;width:100%;gap:8px;"><svg viewBox="0 0 24 24" style="flex:0 0 var(--icon-small);width:var(--icon-small);height:var(--icon-small);fill:currentColor;">${item.icon}</svg><span style="flex:1 1 auto;text-align:left;">${item.label}</span></button></li>`).join('');
            listDiv.innerHTML = `<div class="se-list-inner"><ul class="se-list-basic">${buttons}</ul></div>`;
            listDiv.addEventListener('click', e => {
                const button = e.target.closest('button[data-index]');
                if (!button) return;
                e.preventDefault();
                e.stopPropagation();
                core.focus();
                items[Number(button.getAttribute('data-index'))].run(core);
                core.submenuOff();
            });
            core.initMenuTarget(name, targetElement, listDiv);
        }
    };
}

let debounceTimers = {};

// Hides the "Insert description" (caption) checkbox from the image dialog and
// relabels the Submit button to Ok. No SunEditor options exist for either.
function customizeImageDialog(editor) {
    const imageCtx = editor.core.context.image;
    if (!imageCtx) return;

    // Hide caption / "Insert description" row — no option, hardcoded in template.
    const captionCheck = imageCtx.captionCheckEl;
    if (captionCheck) {
        const formRow = captionCheck.closest('.se-dialog-form');
        if (formRow) formRow.style.display = 'none';
    }

    // Rename Submit → Ok on the image dialog footer button.
    const modal = imageCtx.modal;
    if (modal) {
        const submitBtn = modal.querySelector('.se-dialog-footer .se-btn-primary');
        if (submitBtn) {
            const span = submitBtn.querySelector('span');
            if (span) span.textContent = 'Ok';
            submitBtn.title = 'Ok';
            submitBtn.setAttribute('aria-label', 'Ok');
        }
    }

    // Hide unwanted image controller buttons (mirror, caption, revert) via scoped CSS.
    // These are hardcoded in the controller template with no SunEditor option to suppress them.
    const editorContainer = editor.core.context.element.relative;
    if (editorContainer && !editorContainer.querySelector('#se-image-controller-overrides')) {
        const style = document.createElement('style');
        style.id = 'se-image-controller-overrides';
        style.textContent =
            '.se-controller-resizing [data-command="mirror"],' +
            '.se-controller-resizing [data-command="caption"],' +
            '.se-controller-resizing [data-command="revert"] { display: none !important; }';
        editorContainer.appendChild(style);
    }
}

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

    // Change "Submit" button to "Ok".
    const submitBtn = linkContent.querySelector('.se-btn-primary');
    if (submitBtn) {
        const span = submitBtn.querySelector('span');
        if (span) span.textContent = 'Ok';
        submitBtn.title = 'Ok';
        submitBtn.setAttribute('aria-label', 'Ok');
    }

    // Observe dialog inner
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
    const imageFiles = options?.imageFiles || null;
    const uploadEnabled = options?.enableImageUpload || false;
    const buttonList = buildButtonList(mode, enableImage, imageFiles && imageFiles.length > 0);

    const isCharacterMode = mode === 'Character';

    // Build the image gallery data for SunEditor's native gallery browser.
    // The dialog template renders the gallery button only when imageGalleryUrl is truthy;
    // the actual data comes from imageGalleryData (directData), which takes priority over
    // a URL fetch so the placeholder URL is never requested.
    const imageGalleryData = (imageFiles && imageFiles.length > 0)
        ? imageFiles.map(f => ({ src: f.url, thumbnail: f.thumbnailUrl || f.url, name: f.title || '&lt;untitled&gt;', alt: f.title || ' ' }))
        : null;

    const editor = SUNEDITOR.create(element, {
        plugins: buildCustomPlugins(),
        buttonList: buttonList,
        mode: 'inline',
        formats: isCharacterMode ? ['p'] : [
            { tag: 'p', command: 'replace', class: '', name: 'Normal Text' },
            { tag: 'h2', command: 'replace', class: '', name: 'Main Heading' },
            { tag: 'h3', command: 'replace', class: '', name: 'Subheading' },
            { tag: 'blockquote', command: 'range', class: '', name: 'Quote' },
        ],
        placeholder: placeholder,
        width: '100%',
        minHeight: isCharacterMode ? '1em' : '100px',
        defaultStyle: 'font-family: inherit; font-size: inherit;',
        linkNoPrefix: true,
        // Image dialog: only keep URL, alt text and gallery; markdown storage makes
        // file-upload, resizing, alignment and caption irrelevant.
        imageFileInput: uploadEnabled,
        imageMultipleFile: uploadEnabled,
        imageAccept: 'image/*',
        imageResizing: false,
        imageAlignShow: false,
        imageGalleryUrl: imageGalleryData ? '/_image-gallery_' : null,
        imageGalleryData: imageGalleryData,
    });

    // SunEditor only reports format elements, so text inside a blockquote shows the paragraph caption.
    // Wrap the format dropdown's active handler so the caption reads Quote inside a blockquote.
    const formatPlugin = editor.core.plugins.formatBlock;
    if (formatPlugin && !isCharacterMode) {
        const originalActive = formatPlugin.active;
        formatPlugin.active = function (element) {
            const result = originalActive.call(this, element);
            if (element && this.util.getParentElement(element, 'blockquote')) {
                const target = this.context.formatBlock.targetText;
                this.util.changeTxt(target, 'Quote');
                target.setAttribute('data-value', 'blockquote');
                target.setAttribute('data-class', '');
                return true;
            }
            return result;
        };
    }

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
                    e.stopImmediatePropagation();
                    const cleaned = paste.replace(/[\r\n]+/g, ' ');
                    const textNode = wysiwygBody.ownerDocument.createTextNode(cleaned);
                    editor.core.insertNode(textNode, null, false);
                    editor.core.setRange(textNode, textNode.length, textNode, textNode.length);
                    editor.core.history.push(false);
                }
            }, true);

            // Auto-hide the toolbar when focus leaves via keyboard (e.g. Tab).
            // Mouse-based dismissal is handled by a document mousedown listener
            // registered in toggleToolbar, so no timeouts are needed.
            const wrapperDiv = element.parentElement;
            const inputContainer = wrapperDiv ? (wrapperDiv.closest('.input') || wrapperDiv) : null;
            wysiwygBody.addEventListener('focusout', function (e) {
                if (!editors[elementId] || !editors[elementId].toolbarForceShown) return;
                // relatedTarget is null for mouse clicks on non-focusable elements;
                // those are handled by the document mousedown listener instead.
                if (!e.relatedTarget) return;
                if (inputContainer && inputContainer.contains(e.relatedTarget)) return;
                // Focus moved to a focusable element outside the editor.
                if (wrapperDiv) {
                    wrapperDiv.classList.remove('toolbar-visible');
                }
                if (editors[elementId]) {
                    editors[elementId].toolbarForceShown = false;
                }
                if (editors[elementId] && editors[elementId]._hideOnClickOutside) {
                    document.removeEventListener('mousedown', editors[elementId]._hideOnClickOutside, true);
                    editors[elementId]._hideOnClickOutside = null;
                }
            });
        }
    }

    customizeLinkDialog(editor, linkBookmarks);

    if (imageGalleryData) {
        customizeImageDialog(editor);
    }

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

    // Pasted, dropped and chosen image files are uploaded as attachments by .NET, and the
    // resulting stable URL is inserted. Returning false stops SunEditor embedding base-64.
    // Without upload support (no image scope) images are rejected outright with a toast.
    if (!uploadEnabled) {
        const rejectImages = function (e) {
            const data = e.clipboardData || e.dataTransfer;
            if (!data) return;
            const hasImageFile = Array.from(data.items || []).some(i => i.kind === 'file' && i.type.startsWith('image/'))
                || Array.from(data.files || []).some(f => f.type.startsWith('image/'));
            const html = data.getData ? data.getData('text/html') : '';
            if (hasImageFile || /<img[\s>]/i.test(html)) {
                e.preventDefault();
                e.stopImmediatePropagation();
                showEditorToast(element, "This field doesn't support images");
            }
        };
        editor.core.context.element.wysiwyg.addEventListener('paste', rejectImages, true);
        editor.core.context.element.wysiwyg.addEventListener('drop', rejectImages, true);
    }

    if (uploadEnabled) {
        editor.onImageUploadBefore = (files, info, core) => {
            (async () => {
                const results = [];
                const upload = showUploadingToast(element);
                await upload.shown;
                try {
                for (const file of files) {
                    try {
                        const content = new Uint8Array(await file.arrayBuffer());
                        const image = await dotNetRef.invokeMethodAsync('UploadImage', file.name || 'image', file.type || 'application/octet-stream', content);
                        if (image && image.url) {
                            results.push({ url: image.url, name: image.title || file.name, size: file.size });
                        }
                    } catch (err) {
                        console.error('[markdown-editor] Image upload failed', err);
                    }
                }
                if (results.length > 0) {
                    core.plugins.image.register.call(core, info, { result: results });
                    await waitForImages(core.context.element.wysiwyg, results.map(r => r.url));
                }
                } finally {
                    upload.done();
                    core.closeLoading();
                }
            })();
            return false;
        };
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

export function toggleToolbar(elementId) {
    console.log("Toggling toolbar for", elementId);
    const entry = editors[elementId];
    if (!entry) return;

    const element = document.getElementById(elementId);
    const wrapperDiv = element?.parentElement;
    if (!wrapperDiv) return;

    if (entry.toolbarForceShown) {
        wrapperDiv.classList.remove('toolbar-visible');
        entry.toolbarForceShown = false;
        if (entry._hideOnClickOutside) {
            document.removeEventListener('mousedown', entry._hideOnClickOutside, true);
            entry._hideOnClickOutside = null;
        }
    } else {
        wrapperDiv.classList.add('toolbar-visible');
        entry.toolbarForceShown = true;
        entry.editor.core.context.element.wysiwyg.focus();

        // Dismiss immediately when clicking outside the input container
        // (which includes both the editor and the affordance icon).
        const inputContainer = wrapperDiv.closest('.input') || wrapperDiv;
        const hideOnClickOutside = function (e) {
            if (!inputContainer.contains(e.target)) {
                wrapperDiv.classList.remove('toolbar-visible');
                entry.toolbarForceShown = false;
                document.removeEventListener('mousedown', hideOnClickOutside, true);
                entry._hideOnClickOutside = null;
            }
        };
        entry._hideOnClickOutside = hideOnClickOutside;
        document.addEventListener('mousedown', hideOnClickOutside, true);
    }
}

export function destroy(elementId) {
    const entry = editors[elementId];
    if (!entry) {
        return;
    }
    if (entry._hideOnClickOutside) {
        document.removeEventListener('mousedown', entry._hideOnClickOutside, true);
    }
    if (debounceTimers[elementId]) {
        clearTimeout(debounceTimers[elementId]);
        delete debounceTimers[elementId];
    }
    entry.editor.destroy();
    delete editors[elementId];
}
