'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
var video = document.getElementById('video');
var canvas = document.getElementById('canvas');
var errorMsgElement = document.querySelector('span#errorMsg');
var stream;

var constraints = {
    audio: false,
    video: {
        width: 480, height: 480, facingMode: "user"
    }
};

function startCamera() {
    return regeneratorRuntime.async(function startCamera$(context$1$0) {
        while (1) switch (context$1$0.prev = context$1$0.next) {
            case 0:
                context$1$0.prev = 0;
                context$1$0.next = 3;
                return regeneratorRuntime.awrap(navigator.mediaDevices.getUserMedia(constraints));

            case 3:
                stream = context$1$0.sent;

                video.srcObject = stream;
                video.play();
                context$1$0.next = 11;
                break;

            case 8:
                context$1$0.prev = 8;
                context$1$0.t0 = context$1$0['catch'](0);

                errorMsgElement.innerHTML = 'navigator.getUserMedia error:' + context$1$0.t0.toString();

            case 11:
            case 'end':
                return context$1$0.stop();
        }
    }, null, this, [[0, 8]]);
}

function captureImage() {
    var context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, 480, 480);
}

function stopCamera() {
    stream.getTracks().forEach(function (track) {
        return track.stop();
    });
}

exports.startCamera = startCamera;
exports.captureImage = captureImage;
exports.stopCamera = stopCamera;


var roosterjs =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./packages/roosterjs/lib/index.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./packages/roosterjs-editor-api/lib/experiment/experimentCommitListChains.ts":
/*!************************************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/experiment/experimentCommitListChains.ts ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Commit changes of all list changes when experiment features are allowed
 * @param editor The Editor object
 * @param chains List chains to commit
 */
function experimentCommitListChains(editor, chains) {
    if ((chains === null || chains === void 0 ? void 0 : chains.length) > 0) {
        var range = editor.getSelectionRange();
        var start = range && roosterjs_editor_dom_1.Position.getStart(range);
        var end = range && roosterjs_editor_dom_1.Position.getEnd(range);
        chains.forEach(function (chain) { return chain.commit(); });
        editor.select(start, end);
    }
}
exports.default = experimentCommitListChains;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/changeCapitalization.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/changeCapitalization.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Change the capitalization of text in the selection
 * @param editor The editor instance
 * @param capitalization The case option
 * @param language Optional parameter for language string that should comply to "IETF BCP 47 Tags for
 * Identifying Languages". For example: 'en' or 'en-US' for English, 'tr' for Turkish.
 * Default is the host environment’s current locale.
 */
function changeCapitalization(editor, capitalization, language) {
    applyInlineStyle_1.default(editor, function (element) {
        for (var node = roosterjs_editor_dom_1.getFirstLeafNode(element); node; node = roosterjs_editor_dom_1.getNextLeafSibling(element, node)) {
            if (node.nodeType == 3 /* Text */) {
                try {
                    node.textContent = getCapitalizedText(node.textContent, language);
                }
                catch (_a) {
                    node.textContent = getCapitalizedText(node.textContent, undefined);
                }
            }
        }
    });
    function getCapitalizedText(originalText, language) {
        switch (capitalization) {
            case "lowercase" /* Lowercase */:
                return originalText.toLocaleLowerCase(language);
            case "uppercase" /* Uppercase */:
                return originalText.toLocaleUpperCase(language);
            case "capitalize" /* CapitalizeEachWord */:
                var wordArray = originalText.toLocaleLowerCase(language).split(' ');
                for (var i = 0; i < wordArray.length; i++) {
                    wordArray[i] =
                        wordArray[i].charAt(0).toLocaleUpperCase(language) + wordArray[i].slice(1);
                }
                return wordArray.join(' ');
            case "sentence" /* Sentence */:
                // TODO: Add rules on punctuation for internationalization - TASK 104769
                var punctuationMarks = '[\\.\\!\\?]';
                // Find a match of a word character either:
                // - At the beginning of a string with or without preceding whitespace, for
                // example: '  hello world' and 'hello world' strings would both match 'h'.
                // - Or preceded by a punctuation mark and at least one whitespace, for
                // example 'yes. hello world' would match 'y' and 'h'.
                var regex = new RegExp('^\\s*\\w|' + punctuationMarks + '\\s+\\w', 'g');
                return originalText.toLocaleLowerCase(language).replace(regex, function (match) {
                    return match.toLocaleUpperCase(language);
                });
        }
    }
}
exports.default = changeCapitalization;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/changeFontSize.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/changeFontSize.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Default font size sequence, in pt. Suggest editor UI use this sequence as your font size list,
 * So that when increase/decrease font size, the font size can match the sequence of your font size picker
 */
exports.FONT_SIZES = [8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72];
var MIN_FONT_SIZE = 1;
var MAX_FONT_SIZE = 1000;
/**
 * Increase or decrease font size in selection
 * @param editor The editor instance
 * @param change Whether increase or decrease font size
 * @param fontSizes A sorted font size array, in pt. Default value is FONT_SIZES
 */
function changeFontSize(editor, change, fontSizes) {
    if (fontSizes === void 0) { fontSizes = exports.FONT_SIZES; }
    var changeBase = change == 0 /* Increase */ ? 1 : -1;
    applyInlineStyle_1.default(editor, function (element) {
        var pt = parseFloat(roosterjs_editor_dom_1.getComputedStyle(element, 'font-size'));
        element.style.fontSize = getNewFontSize(pt, changeBase, fontSizes) + 'pt';
        var lineHeight = roosterjs_editor_dom_1.getComputedStyle(element, 'line-height');
        if (lineHeight != 'normal') {
            element.style.lineHeight = 'normal';
        }
    });
}
exports.default = changeFontSize;
function getNewFontSize(pt, changeBase, fontSizes) {
    pt = changeBase == 1 ? Math.floor(pt) : Math.ceil(pt);
    var last = fontSizes[fontSizes.length - 1];
    if (pt <= fontSizes[0]) {
        pt = Math.max(pt + changeBase, MIN_FONT_SIZE);
    }
    else if (pt > last || (pt == last && changeBase == 1)) {
        pt = pt / 10;
        pt = changeBase == 1 ? Math.floor(pt) : Math.ceil(pt);
        pt = Math.min(Math.max((pt + changeBase) * 10, last), MAX_FONT_SIZE);
    }
    else if (changeBase == 1) {
        for (var i = 0; i < fontSizes.length; i++) {
            if (pt < fontSizes[i]) {
                pt = fontSizes[i];
                break;
            }
        }
    }
    else {
        for (var i = fontSizes.length - 1; i >= 0; i--) {
            if (pt > fontSizes[i]) {
                pt = fontSizes[i];
                break;
            }
        }
    }
    return pt;
}
exports.getNewFontSize = getNewFontSize;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/clearBlockFormat.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/clearBlockFormat.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockFormat_1 = __webpack_require__(/*! ../utils/blockFormat */ "./packages/roosterjs-editor-api/lib/utils/blockFormat.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var TAGS_TO_UNWRAP = 'B,I,U,STRONG,EM,SUB,SUP,STRIKE,FONT,CENTER,H1,H2,H3,H4,H5,H6,UL,OL,LI,SPAN,P,BLOCKQUOTE,CODE,S,PRE'.split(',');
var ATTRIBUTES_TO_PRESERVE = ['href', 'src'];
var TAGS_TO_STOP_UNWRAP = ['TD', 'TH', 'TR', 'TABLE', 'TBODY', 'THEAD'];
/**
 * Clear all formats of selected blocks.
 * When selection is collapsed, only clear format of current block.
 * @param editor The editor instance
 */
function clearBlockFormat(editor) {
    blockFormat_1.default(editor, function (region) {
        var blocks = roosterjs_editor_dom_1.getSelectedBlockElementsInRegion(region);
        var nodes = roosterjs_editor_dom_1.collapseNodesInRegion(region, blocks);
        if (editor.contains(region.rootNode)) {
            // If there are styles on table cell, wrap all its children and move down all non-border styles.
            // So that we can preserve styles for unselected blocks as well as border styles for table
            var nonborderStyles = removeNonBorderStyles(region.rootNode);
            if (Object.keys(nonborderStyles).length > 0) {
                var wrapper = roosterjs_editor_dom_1.wrap(roosterjs_editor_dom_1.toArray(region.rootNode.childNodes));
                roosterjs_editor_dom_1.setStyles(wrapper, nonborderStyles);
            }
        }
        while (nodes.length > 0 && roosterjs_editor_dom_1.isNodeInRegion(region, nodes[0].parentNode)) {
            nodes = [roosterjs_editor_dom_1.splitBalancedNodeRange(nodes)];
        }
        nodes.forEach(clearNodeFormat);
    });
}
exports.default = clearBlockFormat;
function clearNodeFormat(node) {
    // 1. Recursively clear format of all its child nodes
    var areBlockElements = roosterjs_editor_dom_1.toArray(node.childNodes).map(clearNodeFormat);
    var areAllChildrenBlock = areBlockElements.every(function (b) { return b; });
    var returnBlockElement = roosterjs_editor_dom_1.isBlockElement(node);
    // 2. Unwrap the tag if necessary
    var tag = roosterjs_editor_dom_1.getTagOfNode(node);
    if (tag) {
        if (TAGS_TO_UNWRAP.indexOf(tag) >= 0 ||
            (areAllChildrenBlock &&
                !roosterjs_editor_dom_1.isVoidHtmlElement(node) &&
                TAGS_TO_STOP_UNWRAP.indexOf(tag) < 0)) {
            if (returnBlockElement && !areAllChildrenBlock) {
                roosterjs_editor_dom_1.wrap(node);
            }
            roosterjs_editor_dom_1.unwrap(node);
        }
        else {
            // 3. Otherwise, remove all attributes
            clearAttribute(node);
        }
    }
    return returnBlockElement;
}
function clearAttribute(element) {
    var isTableCell = roosterjs_editor_dom_1.safeInstanceOf(element, 'HTMLTableCellElement');
    for (var _i = 0, _a = roosterjs_editor_dom_1.toArray(element.attributes); _i < _a.length; _i++) {
        var attr = _a[_i];
        if (isTableCell && attr.name == 'style') {
            removeNonBorderStyles(element);
        }
        else if (ATTRIBUTES_TO_PRESERVE.indexOf(attr.name.toLowerCase()) < 0 &&
            attr.name.indexOf('data-') != 0) {
            element.removeAttribute(attr.name);
        }
    }
}
function removeNonBorderStyles(element) {
    var styles = roosterjs_editor_dom_1.getStyles(element);
    var result = {};
    Object.keys(styles).forEach(function (name) {
        if (name.indexOf('border') < 0) {
            result[name] = styles[name];
            delete styles[name];
        }
    });
    roosterjs_editor_dom_1.setStyles(element, styles);
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/clearFormat.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/clearFormat.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
var setBackgroundColor_1 = __webpack_require__(/*! ./setBackgroundColor */ "./packages/roosterjs-editor-api/lib/format/setBackgroundColor.ts");
var setFontName_1 = __webpack_require__(/*! ./setFontName */ "./packages/roosterjs-editor-api/lib/format/setFontName.ts");
var setFontSize_1 = __webpack_require__(/*! ./setFontSize */ "./packages/roosterjs-editor-api/lib/format/setFontSize.ts");
var setTextColor_1 = __webpack_require__(/*! ./setTextColor */ "./packages/roosterjs-editor-api/lib/format/setTextColor.ts");
var toggleBold_1 = __webpack_require__(/*! ./toggleBold */ "./packages/roosterjs-editor-api/lib/format/toggleBold.ts");
var toggleItalic_1 = __webpack_require__(/*! ./toggleItalic */ "./packages/roosterjs-editor-api/lib/format/toggleItalic.ts");
var toggleUnderline_1 = __webpack_require__(/*! ./toggleUnderline */ "./packages/roosterjs-editor-api/lib/format/toggleUnderline.ts");
var STYLES_TO_REMOVE = ['font', 'text-decoration', 'color', 'background'];
/**
 * Clear the format in current selection, after cleaning, the format will be
 * changed to default format. The format that get cleaned include B/I/U/font name/
 * font size/text color/background color/align left/align right/align center/superscript/subscript
 * @param editor The editor instance
 */
function clearFormat(editor) {
    editor.focus();
    editor.addUndoSnapshot(function () {
        execCommand_1.default(editor, "removeFormat" /* RemoveFormat */);
        editor.queryElements('[class]', 1 /* OnSelection */, function (node) {
            return node.removeAttribute('class');
        });
        var defaultFormat = editor.getDefaultFormat();
        var isDefaultFormatEmpty = Object.keys(defaultFormat).length === 0;
        editor.queryElements('[style]', 2 /* InSelection */, function (node) {
            STYLES_TO_REMOVE.forEach(function (style) { return node.style.removeProperty(style); });
            // when default format is empty, keep the HTML minimum by removing style attribute if there's no style
            // (note: because default format is empty, we're not adding style back in)
            if (isDefaultFormatEmpty && node.getAttribute('style') === '') {
                node.removeAttribute('style');
            }
        });
        if (!isDefaultFormatEmpty) {
            if (defaultFormat.fontFamily) {
                setFontName_1.default(editor, defaultFormat.fontFamily);
            }
            if (defaultFormat.fontSize) {
                setFontSize_1.default(editor, defaultFormat.fontSize);
            }
            if (defaultFormat.textColor) {
                if (defaultFormat.textColors) {
                    setTextColor_1.default(editor, defaultFormat.textColors);
                }
                else {
                    setTextColor_1.default(editor, defaultFormat.textColor);
                }
            }
            if (defaultFormat.backgroundColor) {
                if (defaultFormat.backgroundColors) {
                    setBackgroundColor_1.default(editor, defaultFormat.backgroundColors);
                }
                else {
                    setBackgroundColor_1.default(editor, defaultFormat.backgroundColor);
                }
            }
            if (defaultFormat.bold) {
                toggleBold_1.default(editor);
            }
            if (defaultFormat.italic) {
                toggleItalic_1.default(editor);
            }
            if (defaultFormat.underline) {
                toggleUnderline_1.default(editor);
            }
        }
    }, "Format" /* Format */);
}
exports.default = clearFormat;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/createLink.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/createLink.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
// Regex matching Uri scheme
var URI_REGEX = /^[a-zA-Z]+:/i;
// Regex matching begin of email address
var MAILTO_REGEX = /^[\w.%+-]+@/i;
// Regex matching begin of ftp, i.e. ftp.microsoft.com
var FTP_REGEX = /^ftp\./i;
var TEMP_TITLE = 'istemptitle';
function applyLinkPrefix(url) {
    if (!url) {
        return url;
    }
    // Add link prefix per rule:
    // (a) if the url always starts with a URI scheme, leave it as it is
    // (b) if the url is an email address, xxx@... add mailto: prefix
    // (c) if the url starts with ftp., add ftp:// prefix
    // (d) rest, add http:// prefix
    var prefix = '';
    if (url.search(URI_REGEX) < 0) {
        if (url.search(MAILTO_REGEX) == 0) {
            prefix = 'mailto:';
        }
        else if (url.search(FTP_REGEX) == 0) {
            prefix = 'ftp://';
        }
        else {
            // fallback to http://
            prefix = 'http://';
        }
    }
    return prefix + url;
}
/**
 * Insert a hyperlink at cursor.
 * When there is a selection, hyperlink will be applied to the selection,
 * otherwise a hyperlink will be inserted to the cursor position.
 * @param editor Editor object
 * @param link Link address, can be http(s), mailto, notes, file, unc, ftp, news, telnet, gopher, wais.
 * When protocol is not specified, a best matched protocol will be predicted.
 * @param altText Optional alt text of the link, will be shown when hover on the link
 * @param displayText Optional display text for the link.
 * If specified, the display text of link will be replaced with this text.
 * If not specified and there wasn't a link, the link url will be used as display text.
 */
function createLink(editor, link, altText, displayText) {
    editor.focus();
    var url = (checkXss(link) || '').trim();
    if (url) {
        var linkData = roosterjs_editor_dom_1.matchLink(url);
        // matchLink can match most links, but not all, i.e. if you pass link a link as "abc", it won't match
        // we know in that case, users will want to insert a link like http://abc
        // so we have separate logic in applyLinkPrefix to add link prefix depending on the format of the link
        // i.e. if the link starts with something like abc@xxx, we will add mailto: prefix
        // if the link starts with ftp.xxx, we will add ftp:// link. For more, see applyLinkPrefix
        var normalizedUrl_1 = linkData ? linkData.normalizedUrl : applyLinkPrefix(url);
        var originalUrl_1 = linkData ? linkData.originalUrl : url;
        editor.addUndoSnapshot(function () {
            var range = editor.getSelectionRange();
            var anchor = null;
            if (range && range.collapsed) {
                anchor = getAnchorNodeAtCursor(editor);
                // If there is already a link, just change its href
                if (anchor) {
                    anchor.href = normalizedUrl_1;
                    // Change text content if it is specified
                    updateAnchorDisplayText(anchor, displayText);
                }
                else {
                    anchor = editor.getDocument().createElement('A');
                    anchor.textContent = displayText || originalUrl_1;
                    anchor.href = normalizedUrl_1;
                    editor.insertNode(anchor);
                }
            }
            else {
                // the selection is not collapsed, use browser execCommand
                editor.getDocument().execCommand("createLink" /* CreateLink */, false, normalizedUrl_1);
                anchor = getAnchorNodeAtCursor(editor);
                updateAnchorDisplayText(anchor, displayText);
            }
            if (altText && anchor) {
                // Hack: Ideally this should be done by HyperLink plugin.
                // We make a hack here since we don't have an event to notify HyperLink plugin
                // before we apply the link.
                anchor.removeAttribute(TEMP_TITLE);
                anchor.title = altText;
            }
            return anchor;
        }, "CreateLink" /* CreateLink */);
    }
}
exports.default = createLink;
function getAnchorNodeAtCursor(editor) {
    return editor.queryElements('a[href]', 1 /* OnSelection */)[0];
}
function updateAnchorDisplayText(anchor, displayText) {
    if (displayText && anchor.textContent != displayText) {
        anchor.textContent = displayText;
    }
}
function checkXss(link) {
    var santizer = new roosterjs_editor_dom_1.HtmlSanitizer();
    var doc = new DOMParser().parseFromString('<a></a>', 'text/html');
    var a = doc.body.firstChild;
    a.href = link || '';
    santizer.sanitize(doc.body);
    // We use getAttribute because some browsers will try to make the href property a valid link.
    // This has unintended side effects when the link lacks a protocol.
    return a.getAttribute('href');
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/getFormatState.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/getFormatState.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Get element based Format State at cursor
 * @param editor The editor instance
 * @param event (Optional) The plugin event, it stores the event cached data for looking up.
 * In this function the event cache is used to get list state and header level. If not passed,
 * it will query the node within selection to get the info
 * @returns An ElementBasedFormatState object
 */
function getElementBasedFormatState(editor, event) {
    var listTag = roosterjs_editor_dom_1.getTagOfNode(editor.getElementAtCursor('OL,UL', null /*startFrom*/, event));
    var headerTag = roosterjs_editor_dom_1.getTagOfNode(editor.getElementAtCursor('H1,H2,H3,H4,H5,H6', null /*startFrom*/, event));
    return {
        isBullet: listTag == 'UL',
        isNumbering: listTag == 'OL',
        headerLevel: (headerTag && parseInt(headerTag[1])) || 0,
        canUnlink: !!editor.queryElements('a[href]', 1 /* OnSelection */)[0],
        canAddImageAltText: !!editor.queryElements('img', 1 /* OnSelection */)[0],
        isBlockQuote: !!editor.queryElements('blockquote', 1 /* OnSelection */)[0],
    };
}
exports.getElementBasedFormatState = getElementBasedFormatState;
/**
 * Get format state at cursor
 * A format state is a collection of all format related states, e.g.,
 * bold, italic, underline, font name, font size, etc.
 * @param editor The editor instance
 * @param event (Optional) The plugin event, it stores the event cached data for looking up.
 * In this function the event cache is used to get list state and header level. If not passed,
 * it will query the node within selection to get the info
 * @returns The format state at cursor
 */
function getFormatState(editor, event) {
    return __assign(__assign(__assign(__assign({}, roosterjs_editor_dom_1.getPendableFormatState(editor.getDocument())), getElementBasedFormatState(editor, event)), editor.getStyleBasedFormatState()), editor.getUndoState());
}
exports.default = getFormatState;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/insertEntity.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/insertEntity.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Insert an entity into editor.
 * @param editor The editor to insert entity into.
 * @param type Type of the entity
 * @param contentNode Root element of the entity
 * @param isBlock Whether the entity will be shown as a block
 * @param isReadonly Whether the entity will be a readonly entity
 * @param position (Optional) The position to insert into. If not specified, current position will be used.
 * If isBlock is true, entity will be insert below this position
 */
function insertEntity(editor, type, contentNode, isBlock, isReadonly, position) {
    var wrapper = roosterjs_editor_dom_1.wrap(contentNode, isBlock ? 'DIV' : 'SPAN');
    // For inline & readonly entity, we need to set display to "inline-block" otherwise
    // there will be some weird behavior when move cursor around the entity node.
    // And we should only do this for readonly entity since "inline-block" has some side effect
    // in IE that there will be a resize border around the inline-block element. We made some
    // workaround for readonly entity for this issue but for editable entity, keep it as "inline"
    // will just work fine.
    if (!isBlock && isReadonly) {
        wrapper.style.display = 'inline-block';
    }
    roosterjs_editor_dom_1.commitEntity(wrapper, type, isReadonly);
    if (!editor.contains(wrapper)) {
        var currentRange = void 0;
        var contentPosition = void 0;
        if (typeof position == 'number') {
            contentPosition = position;
        }
        else if (position) {
            currentRange = editor.getSelectionRange();
            var node = position.normalize().node;
            var existingEntity = node && editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node);
            // Do not insert entity into another entity
            if (existingEntity) {
                position = new roosterjs_editor_dom_1.Position(existingEntity, -3 /* After */);
            }
            editor.select(position);
            contentPosition = 3 /* SelectionStart */;
        }
        else {
            editor.focus();
            contentPosition = 3 /* SelectionStart */;
        }
        editor.insertNode(wrapper, {
            updateCursor: false,
            insertOnNewLine: isBlock,
            replaceSelection: true,
            position: contentPosition,
        });
        if (contentPosition == 3 /* SelectionStart */) {
            if (currentRange) {
                editor.select(currentRange);
            }
            else if (!isBlock) {
                editor.select(wrapper, -3 /* After */);
            }
        }
    }
    if (isBlock) {
        // Insert an extra empty line for block entity to make sure
        // user can still put cursor below the entity.
        var br = editor.getDocument().createElement('BR');
        wrapper.parentNode.insertBefore(br, wrapper.nextSibling);
    }
    var entity = roosterjs_editor_dom_1.getEntityFromElement(wrapper);
    editor.triggerContentChangedEvent("InsertEntity" /* InsertEntity */, entity);
    return entity;
}
exports.default = insertEntity;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/insertImage.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/insertImage.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
function insertImage(editor, imageFile) {
    if (typeof imageFile == 'string') {
        insertImageWithSrc(editor, imageFile);
    }
    else {
        roosterjs_editor_dom_1.readFile(imageFile, function (dataUrl) {
            if (dataUrl && !editor.isDisposed()) {
                insertImageWithSrc(editor, dataUrl);
            }
        });
    }
}
exports.default = insertImage;
function insertImageWithSrc(editor, src) {
    editor.addUndoSnapshot(function () {
        var image = editor.getDocument().createElement('img');
        image.src = src;
        image.style.maxWidth = '100%';
        editor.insertNode(image);
    }, "Format" /* Format */);
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/removeLink.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/removeLink.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Remove link at selection. If no links at selection, do nothing.
 * If selection contains multiple links, all of the link styles will be removed.
 * If only part of a link is selected, the whole link style will be removed.
 * @param editor The editor instance
 */
function removeLink(editor) {
    editor.focus();
    editor.addUndoSnapshot(function (start, end) {
        editor.queryElements('a[href]', 1 /* OnSelection */, roosterjs_editor_dom_1.unwrap);
        editor.select(start, end);
    }, "Format" /* Format */);
}
exports.default = removeLink;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/replaceWithNode.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/replaceWithNode.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
function replaceWithNode(editor, textOrRange, node, exactMatch, searcher) {
    // Make sure the text and node is valid
    if (!textOrRange || !node) {
        return false;
    }
    var range;
    if (typeof textOrRange == 'string') {
        searcher = searcher || editor.getContentSearcherOfCursor();
        range = searcher && searcher.getRangeFromText(textOrRange, exactMatch);
    }
    else {
        range = textOrRange;
    }
    if (range) {
        var backupRange = editor.getSelectionRange();
        // If the range to replace is right before current cursor, it is actually an exact match
        if (backupRange.collapsed &&
            range.endContainer == backupRange.startContainer &&
            range.endOffset == backupRange.startOffset) {
            exactMatch = true;
        }
        editor.insertNode(node, {
            position: 5 /* Range */,
            updateCursor: exactMatch,
            replaceSelection: true,
            insertOnNewLine: false,
            range: range,
        });
        return true;
    }
    return false;
}
exports.default = replaceWithNode;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/rotateElement.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/rotateElement.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Rotate an element visually
 * @param editor The editor instance
 * @param element The element that should be rotated
 * @param angle The degree at which to rotate the element from it's center
 */
function rotateElement(editor, element, angle) {
    if (element) {
        editor.addUndoSnapshot(function () {
            element.style.transform = "rotate(" + angle + "deg)";
        }, "Format" /* Format */);
    }
}
exports.default = rotateElement;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setAlignment.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setAlignment.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Set content alignment
 * @param editor The editor instance
 * @param alignment The alignment option:
 * Alignment.Center, Alignment.Left, Alignment.Right
 */
function setAlignment(editor, alignment) {
    var command = "justifyLeft" /* JustifyLeft */;
    var align = 'left';
    if (alignment == 1 /* Center */) {
        command = "justifyCenter" /* JustifyCenter */;
        align = 'center';
    }
    else if (alignment == 2 /* Right */) {
        command = "justifyRight" /* JustifyRight */;
        align = 'right';
    }
    editor.addUndoSnapshot(function () {
        execCommand_1.default(editor, command);
        editor.queryElements('[align]', 1 /* OnSelection */, function (node) { return (node.style.textAlign = align); });
    }, "Format" /* Format */);
}
exports.default = setAlignment;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setBackgroundColor.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setBackgroundColor.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
/**
 * Set background color at current selection
 * @param editor The editor instance
 * @param color One of two options:
 * The color string, can be any of the predefined color names (e.g, 'red')
 * or hexadecimal color string (e.g, '#FF0000') or rgb value (e.g, 'rgb(255, 0, 0)') supported by browser.
 * Currently there's no validation to the string, if the passed string is invalid, it won't take affect
 * Alternatively, you can pass a @typedef ModeIndepenentColor. If in light mode, the lightModeColor property will be used.
 * If in dark mode, the darkModeColor will be used and the lightModeColor will be used when converting back to light mode.
 **/
function setBackgroundColor(editor, color) {
    if (typeof color === 'string') {
        var trimmedColor_1 = color.trim();
        applyInlineStyle_1.default(editor, function (element, isInnerNode) {
            element.style.backgroundColor = isInnerNode ? '' : trimmedColor_1;
        });
    }
    else {
        var darkMode_1 = editor.isDarkMode();
        var appliedColor_1 = darkMode_1 ? color.darkModeColor : color.lightModeColor;
        applyInlineStyle_1.default(editor, function (element, isInnerNode) {
            element.style.backgroundColor = isInnerNode ? '' : appliedColor_1;
            if (darkMode_1) {
                element.dataset["ogsb" /* OriginalStyleBackgroundColor */] =
                    color.lightModeColor;
            }
        });
    }
}
exports.default = setBackgroundColor;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setDirection.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setDirection.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var collapseSelectedBlocks_1 = __webpack_require__(/*! ../utils/collapseSelectedBlocks */ "./packages/roosterjs-editor-api/lib/utils/collapseSelectedBlocks.ts");
/**
 * Change direction for the blocks/paragraph at selection
 * @param editor The editor instance
 * @param direction The direction option:
 * Direction.LeftToRight refers to 'ltr', Direction.RightToLeft refers to 'rtl'
 */
function setDirection(editor, direction) {
    editor.focus();
    editor.addUndoSnapshot(function (start, end) {
        collapseSelectedBlocks_1.default(editor, function (element) {
            element.setAttribute('dir', direction == 0 /* LeftToRight */ ? 'ltr' : 'rtl');
            element.style.textAlign = direction == 0 /* LeftToRight */ ? 'left' : 'right';
        });
        editor.select(start, end);
    }, "Format" /* Format */);
}
exports.default = setDirection;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setFontName.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setFontName.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
/**
 * Set font name at selection
 * @param editor The editor instance
 * @param fontName The fontName string, should be a valid CSS font-family style.
 * Currently there's no validation to the string, if the passed string is invalid, it won't take affect
 */
function setFontName(editor, fontName) {
    fontName = fontName.trim();
    // The browser provided execCommand creates a HTML <font> tag with face attribute. <font> is not HTML5 standard
    // (http://www.w3schools.com/tags/tag_font.asp). Use applyInlineStyle which gives flexibility on applying inline style
    // for here, we use CSS font-family style
    applyInlineStyle_1.default(editor, function (element, isInnerNode) {
        element.style.fontFamily = isInnerNode ? '' : fontName;
    });
}
exports.default = setFontName;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setFontSize.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setFontSize.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Set font size at selection
 * @param editor The editor instance
 * @param fontSize The fontSize string, should be a valid CSS font-size style.
 * Currently there's no validation to the string, if the passed string is invalid, it won't take affect
 */
function setFontSize(editor, fontSize) {
    fontSize = fontSize.trim();
    // The browser provided execCommand only accepts 1-7 point value. In addition, it uses HTML <font> tag with size attribute.
    // <font> is not HTML5 standard (http://www.w3schools.com/tags/tag_font.asp). Use applyInlineStyle which gives flexibility on applying inline style
    // for here, we use CSS font-size style
    applyInlineStyle_1.default(editor, function (element, isInnerNode) {
        element.style.fontSize = isInnerNode ? '' : fontSize;
        var lineHeight = roosterjs_editor_dom_1.getComputedStyle(element, 'line-height');
        if (lineHeight != 'normal') {
            element.style.lineHeight = 'normal';
        }
    });
}
exports.default = setFontSize;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setImageAltText.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setImageAltText.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Set image alt text for all selected images at selection. If no images is contained
 * in selection, do nothing.
 * The alt attribute provides alternative information for an image if a user for some reason
 * cannot view it (because of slow connection, an error in the src attribute, or if the user
 * uses a screen reader). See https://www.w3schools.com/tags/att_img_alt.asp
 * @param editor The editor instance
 * @param altText The image alt text
 */
function setImageAltText(editor, altText) {
    editor.focus();
    editor.addUndoSnapshot(function () {
        editor.queryElements('img', 1 /* OnSelection */, function (node) {
            return node.setAttribute('alt', altText);
        });
    }, "Format" /* Format */);
}
exports.default = setImageAltText;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setIndentation.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setIndentation.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockFormat_1 = __webpack_require__(/*! ../utils/blockFormat */ "./packages/roosterjs-editor-api/lib/utils/blockFormat.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var BlockWrapper = '<blockquote style="margin-top:0;margin-bottom:0"></blockquote>';
/**
 * Set indentation at selection
 * If selection contains bullet/numbering list, increase/decrease indentation will
 * increase/decrease the list level by one.
 * @param editor The editor instance
 * @param indentation The indentation option:
 * Indentation.Increase to increase indentation or Indentation.Decrease to decrease indentation
 */
function setIndentation(editor, indentation) {
    var handler = indentation == 0 /* Increase */ ? indent : outdent;
    blockFormat_1.default(editor, function (region, start, end) {
        var blocks = roosterjs_editor_dom_1.getSelectedBlockElementsInRegion(region, true /*createBlockIfEmpty*/);
        var blockGroups = [[]];
        for (var i = 0; i < blocks.length; i++) {
            var startNode = blocks[i].getStartNode();
            var vList = roosterjs_editor_dom_1.createVListFromRegion(region, true /*includeSiblingLists*/, startNode);
            if (vList) {
                blockGroups.push([]);
                while (blocks[i + 1] && vList.contains(blocks[i + 1].getStartNode())) {
                    i++;
                }
                vList.setIndentation(start, end, indentation);
                vList.writeBack();
            }
            else {
                blockGroups[blockGroups.length - 1].push(blocks[i]);
            }
        }
        blockGroups.forEach(function (group) { return handler(region, group); });
    });
}
exports.default = setIndentation;
function indent(region, blocks) {
    var nodes = roosterjs_editor_dom_1.collapseNodesInRegion(region, blocks);
    roosterjs_editor_dom_1.wrap(nodes, BlockWrapper);
}
function outdent(region, blocks) {
    blocks.forEach(function (blockElement) {
        var node = blockElement.collapseToSingleElement();
        var quote = roosterjs_editor_dom_1.findClosestElementAncestor(node, region.rootNode, 'blockquote');
        if (quote) {
            if (node == quote) {
                node = roosterjs_editor_dom_1.wrap(roosterjs_editor_dom_1.toArray(node.childNodes));
            }
            while (roosterjs_editor_dom_1.isNodeInRegion(region, node) && roosterjs_editor_dom_1.getTagOfNode(node) != 'BLOCKQUOTE') {
                node = roosterjs_editor_dom_1.splitBalancedNodeRange(node);
            }
            if (roosterjs_editor_dom_1.isNodeInRegion(region, node)) {
                roosterjs_editor_dom_1.unwrap(node);
            }
        }
    });
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/setTextColor.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/setTextColor.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyInlineStyle_1 = __webpack_require__(/*! ../utils/applyInlineStyle */ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts");
/**
 * Set text color at selection
 * @param editor The editor instance
 * @param color One of two options:
 * The color string, can be any of the predefined color names (e.g, 'red')
 * or hexadecimal color string (e.g, '#FF0000') or rgb value (e.g, 'rgb(255, 0, 0)') supported by browser.
 * Currently there's no validation to the string, if the passed string is invalid, it won't take affect
 * Alternatively, you can pass a @typedef ModeIndepenentColor. If in light mode, the lightModeColor property will be used.
 * If in dark mode, the darkModeColor will be used and the lightModeColor will be used when converting back to light mode.
 */
function setTextColor(editor, color) {
    if (typeof color === 'string') {
        var trimmedColor_1 = color.trim();
        applyInlineStyle_1.default(editor, function (element, isInnerNode) {
            element.style.color = isInnerNode ? '' : trimmedColor_1;
        });
    }
    else {
        var darkMode_1 = editor.isDarkMode();
        var appliedColor_1 = darkMode_1 ? color.darkModeColor : color.lightModeColor;
        applyInlineStyle_1.default(editor, function (element, isInnerNode) {
            element.style.color = isInnerNode ? '' : appliedColor_1;
            if (darkMode_1) {
                element.dataset["ogsc" /* OriginalStyleColor */] = color.lightModeColor;
            }
        });
    }
}
exports.default = setTextColor;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleBlockQuote.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleBlockQuote.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockWrap_1 = __webpack_require__(/*! ../utils/blockWrap */ "./packages/roosterjs-editor-api/lib/utils/blockWrap.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var BLOCKQUOTE_TAG = 'blockquote';
var DEFAULT_STYLER = function (element) {
    element.style.borderLeft = '3px solid';
    element.style.borderColor = '#C8C8C8';
    element.style.paddingLeft = '10px';
    element.style.color = '#666666';
};
/**
 * Toggle blockquote at selection, if selection already contains any blockquoted elements,
 * the blockquoted elements will be unblockquoted and other elements will take no affect
 * @param editor The editor instance
 * @param styler (Optional) The custom styler for setting the style for the blockquote element
 */
function toggleBlockQuote(editor, styler) {
    blockWrap_1.default(editor, function (nodes) {
        var wrapper = roosterjs_editor_dom_1.wrap(nodes, BLOCKQUOTE_TAG);
        (styler || DEFAULT_STYLER)(wrapper);
    }, function () { return editor.queryElements('blockquote', 1 /* OnSelection */, roosterjs_editor_dom_1.unwrap).length == 0; });
}
exports.default = toggleBlockQuote;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleBold.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleBold.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle bold at selection
 * If selection is collapsed, it will only affect the following input after caret
 * If selection contains only bold text, the bold style will be removed
 * If selection contains only normal text, bold style will be added to the whole selected text
 * If selection contains both bold and normal text, bold stle will be added to the whole selected text
 * @param editor The editor instance
 */
function toggleBold(editor) {
    execCommand_1.default(editor, "bold" /* Bold */);
}
exports.default = toggleBold;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleBullet.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleBullet.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var toggleListType_1 = __webpack_require__(/*! ../utils/toggleListType */ "./packages/roosterjs-editor-api/lib/utils/toggleListType.ts");
/**
 * Toggle bullet at selection
 * If selection contains bullet in deep level, toggle bullet will decrease the bullet level by one
 * If selection contains number list, toggle bullet will convert the number list into bullet list
 * If selection contains both bullet/numbering and normal text, the behavior is decided by corresponding
 * browser execCommand API
 * @param editor The editor instance
 */
function toggleBullet(editor) {
    toggleListType_1.default(editor, 2 /* Unordered */);
}
exports.default = toggleBullet;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleCodeBlock.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleCodeBlock.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockWrap_1 = __webpack_require__(/*! ../utils/blockWrap */ "./packages/roosterjs-editor-api/lib/utils/blockWrap.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var PRE_TAG = 'pre';
var CODE_TAG = 'code';
var SELECTOR = PRE_TAG + ">" + CODE_TAG;
/**
 * Toggle code block at selection, if selection already contains any code blocked elements,
 * the code block elements will be no longer be code blocked and other elements will take no affect
 * @param editor The editor instance
 * @param styler (Optional) The custom styler for setting the style for the code block element
 */
function toggleCodeBlock(editor, styler) {
    blockWrap_1.default(editor, function (nodes) {
        var code = roosterjs_editor_dom_1.wrap(nodes, CODE_TAG);
        var pre = roosterjs_editor_dom_1.wrap(code, PRE_TAG);
        styler === null || styler === void 0 ? void 0 : styler(pre);
    }, function () {
        return editor.queryElements(SELECTOR, 1 /* OnSelection */, function (code) {
            if (!code.previousSibling && !code.nextSibling) {
                var parent_1 = code.parentNode;
                roosterjs_editor_dom_1.unwrap(code);
                roosterjs_editor_dom_1.unwrap(parent_1);
            }
        }).length == 0;
    });
}
exports.default = toggleCodeBlock;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleHeader.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleHeader.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Toggle header at selection
 * @param editor The editor instance
 * @param level The header level, can be a number from 0 to 6, in which 1 ~ 6 refers to
 * the HTML header element &lt;H1&gt; to &lt;H6&gt;, 0 means no header
 * if passed in param is outside the range, will be rounded to nearest number in the range
 */
function toggleHeader(editor, level) {
    level = Math.min(Math.max(Math.round(level), 0), 6);
    editor.addUndoSnapshot(function () {
        editor.focus();
        var wrapped = false;
        editor.queryElements('H1,H2,H3,H4,H5,H6', 1 /* OnSelection */, function (header) {
            if (!wrapped) {
                editor.getDocument().execCommand("formatBlock" /* FormatBlock */, false, '<DIV>');
                wrapped = true;
            }
            var div = editor.getDocument().createElement('div');
            while (header.firstChild) {
                div.appendChild(header.firstChild);
            }
            editor.replaceNode(header, div);
        });
        if (level > 0) {
            var traverser = editor.getSelectionTraverser();
            var blockElement = traverser ? traverser.currentBlockElement : null;
            var sanitizer = new roosterjs_editor_dom_1.HtmlSanitizer({
                cssStyleCallbacks: {
                    'font-size': function () { return false; },
                },
            });
            while (blockElement) {
                var element = blockElement.collapseToSingleElement();
                sanitizer.sanitize(element);
                blockElement = traverser.getNextBlockElement();
            }
            editor.getDocument().execCommand("formatBlock" /* FormatBlock */, false, "<H" + level + ">");
        }
    }, "Format" /* Format */);
}
exports.default = toggleHeader;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleItalic.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleItalic.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle italic at selection
 * If selection is collapsed, it will only affect the input after caret
 * If selection contains only italic text, the italic style will be removed
 * If selection contains only normal text, italic style will be added to the whole selected text
 * If selection contains both italic and normal text, italic stlye will be added to the whole selected text
 * @param editor The editor instance
 */
function toggleItalic(editor) {
    execCommand_1.default(editor, "italic" /* Italic */);
}
exports.default = toggleItalic;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleNumbering.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleNumbering.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var toggleListType_1 = __webpack_require__(/*! ../utils/toggleListType */ "./packages/roosterjs-editor-api/lib/utils/toggleListType.ts");
/**
 * Toggle numbering at selection
 * If selection contains numbering in deep level, toggle numbering will decrease the numbering level by one
 * If selection contains bullet list, toggle numbering will convert the bullet list into number list
 * If selection contains both bullet/numbering and normal text, the behavior is decided by corresponding
 * realization of browser execCommand API
 * @param editor The editor instance
 * @param startNumber (Optional) Start number of the list
 */
function toggleNumbering(editor, startNumber) {
    toggleListType_1.default(editor, 1 /* Ordered */, startNumber);
}
exports.default = toggleNumbering;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleStrikethrough.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleStrikethrough.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle strikethrough at selection
 * If selection is collapsed, it will only affect the input after caret
 * If selection contains only strikethrough text, the strikethrough style will be removed
 * If selection contains only normal text, strikethrough style will be added to the whole selected text
 * If selection contains both strikethrough and normal text, strikethrough stlye will be added to the whole selected text
 * @param editor The editor instance
 */
function toggleStrikethrough(editor) {
    execCommand_1.default(editor, "strikeThrough" /* StrikeThrough */);
}
exports.default = toggleStrikethrough;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleSubscript.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleSubscript.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle subscript at selection
 * If selection is collapsed, it will only affect the input after caret
 * If selection contains only subscript text, the subscript style will be removed
 * If selection contains only normal text, subscript style will be added to the whole selected text
 * If selection contains both subscript and normal text, the subscript style will be removed from whole selected text
 * If selection contains any superscript text, the behavior is determined by corresponding realization of browser
 * execCommand API
 * @param editor The editor instance
 */
function toggleSubscript(editor) {
    execCommand_1.default(editor, "subscript" /* Subscript */);
}
exports.default = toggleSubscript;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleSuperscript.ts":
/*!***********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleSuperscript.ts ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle superscript at selection
 * If selection is collapsed, it will only affect the input after caret
 * If selection contains only superscript text, the superscript style will be removed
 * If selection contains only normal text, superscript style will be added to the whole selected text
 * If selection contains both superscript and normal text, the superscript style will be removed from whole selected text
 * If selection contains any subscript text, the behavior is determined by corresponding realization of browser
 * execCommand API
 * @param editor The editor instance
 */
function toggleSuperscript(editor) {
    execCommand_1.default(editor, "superscript" /* Superscript */);
}
exports.default = toggleSuperscript;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/format/toggleUnderline.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/format/toggleUnderline.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var execCommand_1 = __webpack_require__(/*! ../utils/execCommand */ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts");
/**
 * Toggle underline at selection
 * If selection is collapsed, it will only affect the input after caret
 * If selection contains only underlined text, the underline style will be removed
 * If selection contains only normal text, underline style will be added to the whole selected text
 * If selection contains both underlined and normal text, the underline style will be added to the whole selected text
 * @param editor The editor instance
 */
function toggleUnderline(editor) {
    execCommand_1.default(editor, "underline" /* Underline */);
}
exports.default = toggleUnderline;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/index.ts":
/*!****************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/index.ts ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var changeFontSize_1 = __webpack_require__(/*! ./format/changeFontSize */ "./packages/roosterjs-editor-api/lib/format/changeFontSize.ts");
exports.changeFontSize = changeFontSize_1.default;
exports.FONT_SIZES = changeFontSize_1.FONT_SIZES;
var clearBlockFormat_1 = __webpack_require__(/*! ./format/clearBlockFormat */ "./packages/roosterjs-editor-api/lib/format/clearBlockFormat.ts");
exports.clearBlockFormat = clearBlockFormat_1.default;
var clearFormat_1 = __webpack_require__(/*! ./format/clearFormat */ "./packages/roosterjs-editor-api/lib/format/clearFormat.ts");
exports.clearFormat = clearFormat_1.default;
var createLink_1 = __webpack_require__(/*! ./format/createLink */ "./packages/roosterjs-editor-api/lib/format/createLink.ts");
exports.createLink = createLink_1.default;
var getFormatState_1 = __webpack_require__(/*! ./format/getFormatState */ "./packages/roosterjs-editor-api/lib/format/getFormatState.ts");
exports.getFormatState = getFormatState_1.default;
exports.getElementBasedFormatState = getFormatState_1.getElementBasedFormatState;
var insertEntity_1 = __webpack_require__(/*! ./format/insertEntity */ "./packages/roosterjs-editor-api/lib/format/insertEntity.ts");
exports.insertEntity = insertEntity_1.default;
var insertImage_1 = __webpack_require__(/*! ./format/insertImage */ "./packages/roosterjs-editor-api/lib/format/insertImage.ts");
exports.insertImage = insertImage_1.default;
var insertTable_1 = __webpack_require__(/*! ./table/insertTable */ "./packages/roosterjs-editor-api/lib/table/insertTable.ts");
exports.insertTable = insertTable_1.default;
var editTable_1 = __webpack_require__(/*! ./table/editTable */ "./packages/roosterjs-editor-api/lib/table/editTable.ts");
exports.editTable = editTable_1.default;
var formatTable_1 = __webpack_require__(/*! ./table/formatTable */ "./packages/roosterjs-editor-api/lib/table/formatTable.ts");
exports.formatTable = formatTable_1.default;
var removeLink_1 = __webpack_require__(/*! ./format/removeLink */ "./packages/roosterjs-editor-api/lib/format/removeLink.ts");
exports.removeLink = removeLink_1.default;
var replaceWithNode_1 = __webpack_require__(/*! ./format/replaceWithNode */ "./packages/roosterjs-editor-api/lib/format/replaceWithNode.ts");
exports.replaceWithNode = replaceWithNode_1.default;
var rotateElement_1 = __webpack_require__(/*! ./format/rotateElement */ "./packages/roosterjs-editor-api/lib/format/rotateElement.ts");
exports.rotateElement = rotateElement_1.default;
var setAlignment_1 = __webpack_require__(/*! ./format/setAlignment */ "./packages/roosterjs-editor-api/lib/format/setAlignment.ts");
exports.setAlignment = setAlignment_1.default;
var setBackgroundColor_1 = __webpack_require__(/*! ./format/setBackgroundColor */ "./packages/roosterjs-editor-api/lib/format/setBackgroundColor.ts");
exports.setBackgroundColor = setBackgroundColor_1.default;
var setTextColor_1 = __webpack_require__(/*! ./format/setTextColor */ "./packages/roosterjs-editor-api/lib/format/setTextColor.ts");
exports.setTextColor = setTextColor_1.default;
var setDirection_1 = __webpack_require__(/*! ./format/setDirection */ "./packages/roosterjs-editor-api/lib/format/setDirection.ts");
exports.setDirection = setDirection_1.default;
var setFontName_1 = __webpack_require__(/*! ./format/setFontName */ "./packages/roosterjs-editor-api/lib/format/setFontName.ts");
exports.setFontName = setFontName_1.default;
var setFontSize_1 = __webpack_require__(/*! ./format/setFontSize */ "./packages/roosterjs-editor-api/lib/format/setFontSize.ts");
exports.setFontSize = setFontSize_1.default;
var setImageAltText_1 = __webpack_require__(/*! ./format/setImageAltText */ "./packages/roosterjs-editor-api/lib/format/setImageAltText.ts");
exports.setImageAltText = setImageAltText_1.default;
var setIndentation_1 = __webpack_require__(/*! ./format/setIndentation */ "./packages/roosterjs-editor-api/lib/format/setIndentation.ts");
exports.setIndentation = setIndentation_1.default;
var changeCapitalization_1 = __webpack_require__(/*! ./format/changeCapitalization */ "./packages/roosterjs-editor-api/lib/format/changeCapitalization.ts");
exports.changeCapitalization = changeCapitalization_1.default;
var toggleBold_1 = __webpack_require__(/*! ./format/toggleBold */ "./packages/roosterjs-editor-api/lib/format/toggleBold.ts");
exports.toggleBold = toggleBold_1.default;
var toggleBullet_1 = __webpack_require__(/*! ./format/toggleBullet */ "./packages/roosterjs-editor-api/lib/format/toggleBullet.ts");
exports.toggleBullet = toggleBullet_1.default;
var toggleItalic_1 = __webpack_require__(/*! ./format/toggleItalic */ "./packages/roosterjs-editor-api/lib/format/toggleItalic.ts");
exports.toggleItalic = toggleItalic_1.default;
var toggleNumbering_1 = __webpack_require__(/*! ./format/toggleNumbering */ "./packages/roosterjs-editor-api/lib/format/toggleNumbering.ts");
exports.toggleNumbering = toggleNumbering_1.default;
var toggleBlockQuote_1 = __webpack_require__(/*! ./format/toggleBlockQuote */ "./packages/roosterjs-editor-api/lib/format/toggleBlockQuote.ts");
exports.toggleBlockQuote = toggleBlockQuote_1.default;
var toggleCodeBlock_1 = __webpack_require__(/*! ./format/toggleCodeBlock */ "./packages/roosterjs-editor-api/lib/format/toggleCodeBlock.ts");
exports.toggleCodeBlock = toggleCodeBlock_1.default;
var toggleStrikethrough_1 = __webpack_require__(/*! ./format/toggleStrikethrough */ "./packages/roosterjs-editor-api/lib/format/toggleStrikethrough.ts");
exports.toggleStrikethrough = toggleStrikethrough_1.default;
var toggleSubscript_1 = __webpack_require__(/*! ./format/toggleSubscript */ "./packages/roosterjs-editor-api/lib/format/toggleSubscript.ts");
exports.toggleSubscript = toggleSubscript_1.default;
var toggleSuperscript_1 = __webpack_require__(/*! ./format/toggleSuperscript */ "./packages/roosterjs-editor-api/lib/format/toggleSuperscript.ts");
exports.toggleSuperscript = toggleSuperscript_1.default;
var toggleUnderline_1 = __webpack_require__(/*! ./format/toggleUnderline */ "./packages/roosterjs-editor-api/lib/format/toggleUnderline.ts");
exports.toggleUnderline = toggleUnderline_1.default;
var toggleHeader_1 = __webpack_require__(/*! ./format/toggleHeader */ "./packages/roosterjs-editor-api/lib/format/toggleHeader.ts");
exports.toggleHeader = toggleHeader_1.default;
var experimentCommitListChains_1 = __webpack_require__(/*! ./experiment/experimentCommitListChains */ "./packages/roosterjs-editor-api/lib/experiment/experimentCommitListChains.ts");
exports.experimentCommitListChains = experimentCommitListChains_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/table/editTable.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/table/editTable.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Edit table with given operation. If there is no table at cursor then no op.
 * @param editor The editor instance
 * @param operation Table operation
 */
function editTable(editor, operation) {
    var td = editor.getElementAtCursor('TD,TH');
    if (td) {
        editor.addUndoSnapshot(function (start, end) {
            var vtable = new roosterjs_editor_dom_1.VTable(td);
            vtable.edit(operation);
            vtable.writeBack();
            editor.focus();
            var cellToSelect = calculateCellToSelect(operation, vtable.row, vtable.col);
            editor.select(vtable.getCell(cellToSelect.newRow, cellToSelect.newCol).td, 0 /* Begin */);
        }, "Format" /* Format */);
    }
}
exports.default = editTable;
function calculateCellToSelect(operation, currentRow, currentCol) {
    var newRow = currentRow;
    var newCol = currentCol;
    switch (operation) {
        case 0 /* InsertAbove */:
            newCol = 0;
            break;
        case 1 /* InsertBelow */:
            newRow += 1;
            newCol = 0;
            break;
        case 2 /* InsertLeft */:
            newRow = 0;
            break;
        case 3 /* InsertRight */:
            newRow = 0;
            newCol += 1;
            break;
    }
    return {
        newRow: newRow,
        newCol: newCol,
    };
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/table/formatTable.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/table/formatTable.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Format table
 * @param editor The editor which contains the table to format
 * @param format A TableFormat object contains format information we want to apply to the table
 * @param table The table to format. This is optional. When not passed, the current table (if any) will be formatted
 */
function formatTable(editor, format, table) {
    table = table || editor.getElementAtCursor('TABLE');
    if (table) {
        editor.addUndoSnapshot(function (start, end) {
            var vtable = new roosterjs_editor_dom_1.VTable(table);
            vtable.applyFormat(format);
            vtable.writeBack();
            editor.focus();
            editor.select(start, end);
        }, "Format" /* Format */);
    }
}
exports.default = formatTable;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/table/insertTable.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/table/insertTable.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Insert table into editor at current selection
 * @param editor The editor instance
 * @param columns Number of columns in table, it also controls the default table cell width:
 * if columns &lt;= 4, width = 120px; if columns &lt;= 6, width = 100px; else width = 70px
 * @param rows Number of rows in table
 * @param format (Optional) The table format. If not passed, the default format will be applied:
 * background color: #FFF; border color: #ABABAB
 */
function insertTable(editor, columns, rows, format) {
    var document = editor.getDocument();
    var fragment = document.createDocumentFragment();
    var table = document.createElement('table');
    fragment.appendChild(table);
    table.cellSpacing = '0';
    table.cellPadding = '1';
    for (var i = 0; i < rows; i++) {
        var tr = document.createElement('tr');
        table.appendChild(tr);
        for (var j = 0; j < columns; j++) {
            var td = document.createElement('td');
            tr.appendChild(td);
            td.appendChild(document.createElement('br'));
            td.style.width = getTableCellWidth(columns);
        }
    }
    editor.focus();
    editor.addUndoSnapshot(function () {
        var vtable = new roosterjs_editor_dom_1.VTable(table);
        vtable.applyFormat(format || {
            bgColorEven: '#FFF',
            bgColorOdd: '#FFF',
            topBorderColor: '#ABABAB',
            bottomBorderColor: '#ABABAB',
            verticalBorderColor: '#ABABAB',
        });
        vtable.writeBack();
        editor.insertNode(fragment);
        editor.runAsync(function (editor) {
            return editor.select(new roosterjs_editor_dom_1.Position(table, 0 /* Begin */).normalize());
        });
    }, "Format" /* Format */);
}
exports.default = insertTable;
function getTableCellWidth(columns) {
    if (columns <= 4) {
        return '120px';
    }
    else if (columns <= 6) {
        return '100px';
    }
    else {
        return '70px';
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/applyInlineStyle.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var ZERO_WIDTH_SPACE = '\u200B';
/**
 * @internal
 * Apply inline style to current selection
 * @param editor The editor instance
 * @param callback The callback function to apply style
 */
function applyInlineStyle(editor, callback) {
    editor.focus();
    var range = editor.getSelectionRange();
    if (range && range.collapsed) {
        var node = range.startContainer;
        var isEmptySpan = roosterjs_editor_dom_1.getTagOfNode(node) == 'SPAN' &&
            (!node.firstChild ||
                (roosterjs_editor_dom_1.getTagOfNode(node.firstChild) == 'BR' && !node.firstChild.nextSibling));
        if (isEmptySpan) {
            editor.addUndoSnapshot();
            callback(node);
        }
        else {
            var isZWSNode = node &&
                node.nodeType == 3 /* Text */ &&
                node.nodeValue == ZERO_WIDTH_SPACE &&
                roosterjs_editor_dom_1.getTagOfNode(node.parentNode) == 'SPAN';
            if (!isZWSNode) {
                editor.addUndoSnapshot();
                // Create a new text node to hold the selection.
                // Some content is needed to position selection into the span
                // for here, we inject ZWS - zero width space
                node = editor.getDocument().createTextNode(ZERO_WIDTH_SPACE);
                range.insertNode(node);
            }
            roosterjs_editor_dom_1.applyTextStyle(node, callback);
            editor.select(node, -1 /* End */);
        }
    }
    else {
        // This is start and end node that get the style. The start and end needs to be recorded so that selection
        // can be re-applied post-applying style
        editor.addUndoSnapshot(function () {
            var firstNode;
            var lastNode;
            var contentTraverser = editor.getSelectionTraverser();
            var inlineElement = contentTraverser && contentTraverser.currentInlineElement;
            while (inlineElement) {
                var nextInlineElement = contentTraverser.getNextInlineElement();
                inlineElement.applyStyle(function (element, isInnerNode) {
                    callback(element, isInnerNode);
                    firstNode = firstNode || element;
                    lastNode = element;
                });
                inlineElement = nextInlineElement;
            }
            if (firstNode && lastNode) {
                editor.select(firstNode, -2 /* Before */, lastNode, -3 /* After */);
            }
        }, "Format" /* Format */);
    }
}
exports.default = applyInlineStyle;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/blockFormat.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/blockFormat.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var experimentCommitListChains_1 = __webpack_require__(/*! ../experiment/experimentCommitListChains */ "./packages/roosterjs-editor-api/lib/experiment/experimentCommitListChains.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Split selection into regions, and perform a block-wise formatting action for each region.
 */
function blockFormat(editor, callback, beforeRunCallback) {
    editor.focus();
    editor.addUndoSnapshot(function (start, end) {
        if (!beforeRunCallback || beforeRunCallback()) {
            var regions = editor.getSelectedRegions();
            var chains_1 = roosterjs_editor_dom_1.VListChain.createListChains(regions, start === null || start === void 0 ? void 0 : start.node);
            regions.forEach(function (region) { return callback(region, start, end, chains_1); });
            experimentCommitListChains_1.default(editor, chains_1);
        }
        editor.select(start, end);
    }, "Format" /* Format */);
}
exports.default = blockFormat;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/blockWrap.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/blockWrap.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockFormat_1 = __webpack_require__(/*! ./blockFormat */ "./packages/roosterjs-editor-api/lib/utils/blockFormat.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Toggle a tag at selection, if selection already contains elements of such tag,
 * the elements will be untagge and other elements will take no affect
 * @param editor The editor instance
 * @param wrapFunction  The wrap function
 * @param beforeRunCallback A callback function to run before looping all regions. If it returns false,
 * the loop for regions will be skipped
 */
function blockWrap(editor, wrapFunction, beforeRunCallback) {
    blockFormat_1.default(editor, function (region) {
        var blocks = roosterjs_editor_dom_1.getSelectedBlockElementsInRegion(region, true /*createBlockIfEmpty*/);
        var nodes = roosterjs_editor_dom_1.collapseNodesInRegion(region, blocks);
        if (nodes.length > 0) {
            if (nodes.length == 1) {
                var NodeTag = roosterjs_editor_dom_1.getTagOfNode(nodes[0]);
                if (NodeTag == 'BR') {
                    nodes = [roosterjs_editor_dom_1.wrap(nodes[0])];
                }
                else if (NodeTag == 'LI' || NodeTag == 'TD') {
                    nodes = roosterjs_editor_dom_1.toArray(nodes[0].childNodes);
                }
            }
            while (nodes[0] &&
                roosterjs_editor_dom_1.isNodeInRegion(region, nodes[0].parentNode) &&
                nodes.some(function (node) { return roosterjs_editor_dom_1.getTagOfNode(node) == 'LI'; })) {
                nodes = [roosterjs_editor_dom_1.splitBalancedNodeRange(nodes)];
            }
            wrapFunction(nodes);
        }
    }, beforeRunCallback);
}
exports.default = blockWrap;


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/collapseSelectedBlocks.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/collapseSelectedBlocks.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Collapse all selected blocks, return single HTML elements for each block
 * @param editor The editor instance
 * @param forEachCallback A callback function to invoke for each of the collapsed element
 */
function collapseSelectedBlocks(editor, forEachCallback) {
    var traverser = editor.getSelectionTraverser();
    var block = traverser && traverser.currentBlockElement;
    var blocks = [];
    while (block) {
        if (!isEmptyBlockUnderTR(block)) {
            blocks.push(block);
        }
        block = traverser.getNextBlockElement();
    }
    blocks.forEach(function (block) {
        var element = block.collapseToSingleElement();
        forEachCallback(element);
    });
}
exports.default = collapseSelectedBlocks;
function isEmptyBlockUnderTR(block) {
    var startNode = block.getStartNode();
    return (startNode == block.getEndNode() &&
        startNode.nodeType == 3 /* Text */ &&
        ['TR', 'TABLE'].indexOf(roosterjs_editor_dom_1.getTagOfNode(startNode.parentNode)) >= 0);
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/execCommand.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/execCommand.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var pendableFormatCommands = null;
/**
 * @internal
 * Execute a document command
 * @param editor The editor instance
 * @param command The command to execute
 * @param addUndoSnapshotWhenCollapsed Optional, set to true to always add undo snapshot even current selection is collapsed.
 * Default value is false.
 * @param doWorkaroundForList Optional, set to true to do workaround for list in order to keep current format.
 * Default value is false.
 */
function execCommand(editor, command) {
    editor.focus();
    var formatter = function () { return editor.getDocument().execCommand(command, false, null); };
    var range = editor.getSelectionRange();
    if (range && range.collapsed) {
        editor.addUndoSnapshot();
        formatter();
        if (isPendableFormatCommand(command)) {
            // Trigger PendingFormatStateChanged event since we changed pending format state
            editor.triggerPluginEvent(13 /* PendingFormatStateChanged */, {
                formatState: roosterjs_editor_dom_1.getPendableFormatState(editor.getDocument()),
            });
        }
    }
    else {
        editor.addUndoSnapshot(formatter, "Format" /* Format */);
    }
}
exports.default = execCommand;
function isPendableFormatCommand(command) {
    if (!pendableFormatCommands) {
        pendableFormatCommands = Object.keys(roosterjs_editor_dom_1.PendableFormatCommandMap).map(function (key) { return roosterjs_editor_dom_1.PendableFormatCommandMap[key]; });
    }
    return pendableFormatCommands.indexOf(command) >= 0;
}


/***/ }),

/***/ "./packages/roosterjs-editor-api/lib/utils/toggleListType.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-api/lib/utils/toggleListType.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var blockFormat_1 = __webpack_require__(/*! ../utils/blockFormat */ "./packages/roosterjs-editor-api/lib/utils/blockFormat.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
function toggleListType(editor, listType, startNumber) {
    blockFormat_1.default(editor, function (region, start, end, chains) {
        var _a;
        var chain = startNumber > 0 && chains.filter(function (chain) { return chain.canAppendAtCursor(startNumber); })[0];
        var vList = chain && start.equalTo(end)
            ? chain.createVListAtBlock((_a = roosterjs_editor_dom_1.getBlockElementAtNode(region.rootNode, start.node)) === null || _a === void 0 ? void 0 : _a.collapseToSingleElement(), startNumber)
            : roosterjs_editor_dom_1.createVListFromRegion(region, true /*includeSiblingLists*/);
        if (vList) {
            vList.changeListType(start, end, listType);
            vList.writeBack();
        }
    });
}
exports.default = toggleListType;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/addUndoSnapshot.ts":
/*!***********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/addUndoSnapshot.ts ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Call an editing callback with adding undo snapshots around, and trigger a ContentChanged event if change source is specified.
 * Undo snapshot will not be added if this call is nested inside another addUndoSnapshot() call.
 * @param core The EditorCore object
 * @param callback The editing callback, accepting current selection start and end position, returns an optional object used as the data field of ContentChangedEvent.
 * @param changeSource The ChangeSource string of ContentChangedEvent. @default ChangeSource.Format. Set to null to avoid triggering ContentChangedEvent
 * @param canUndoByBackspace True if this action can be undone when user press Backspace key (aka Auto Complelte).
 */
exports.addUndoSnapshot = function (core, callback, changeSource, canUndoByBackspace) {
    var undoState = core.undo;
    var isNested = undoState.isNested;
    var isShadowEdit = !!core.lifecycle.shadowEditFragment;
    var data;
    if (!isNested) {
        undoState.isNested = true;
        if (!isShadowEdit) {
            undoState.snapshotsService.addSnapshot(core.api.getContent(core, 2 /* RawHTMLWithSelection */), canUndoByBackspace);
            undoState.hasNewContent = false;
        }
    }
    try {
        if (callback) {
            var range = core.api.getSelectionRange(core, true /*tryGetFromCache*/);
            data = callback(range && roosterjs_editor_dom_1.Position.getStart(range).normalize(), range && roosterjs_editor_dom_1.Position.getEnd(range).normalize());
            if (!isNested && !isShadowEdit) {
                undoState.snapshotsService.addSnapshot(core.api.getContent(core, 2 /* RawHTMLWithSelection */), false /*isAutoCompleteSnapshot*/);
                undoState.hasNewContent = false;
            }
        }
    }
    finally {
        if (!isNested) {
            undoState.isNested = false;
        }
    }
    if (callback && changeSource) {
        var event_1 = {
            eventType: 7 /* ContentChanged */,
            source: changeSource,
            data: data,
        };
        core.api.triggerEvent(core, event_1, true /*broadcast*/);
    }
    if (canUndoByBackspace) {
        var range = core.api.getSelectionRange(core, false /*tryGetFromCache*/);
        if (range) {
            core.undo.hasNewContent = false;
            core.undo.autoCompletePosition = roosterjs_editor_dom_1.Position.getStart(range);
        }
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/attachDomEvent.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/attachDomEvent.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Attach a DOM event to the editor content DIV
 * @param core The EditorCore object
 * @param eventName The DOM event name
 * @param pluginEventType Optional event type. When specified, editor will trigger a plugin event with this name when the DOM event is triggered
 * @param beforeDispatch Optional callback function to be invoked when the DOM event is triggered before trigger plugin event
 */
exports.attachDomEvent = function (core, eventMap) {
    var disposers = Object.keys(eventMap || {}).map(function (eventName) {
        var _a = extractHandler(eventMap[eventName]), pluginEventType = _a.pluginEventType, beforeDispatch = _a.beforeDispatch;
        var onEvent = function (event) {
            if (beforeDispatch) {
                beforeDispatch(event);
            }
            if (pluginEventType != null) {
                core.api.triggerEvent(core, {
                    eventType: pluginEventType,
                    rawEvent: event,
                }, false /*broadcast*/);
            }
        };
        core.contentDiv.addEventListener(eventName, onEvent);
        return function () {
            core.contentDiv.removeEventListener(eventName, onEvent);
        };
    });
    return function () { return disposers.forEach(function (disposers) { return disposers(); }); };
};
function extractHandler(handlerObj) {
    var result = {
        pluginEventType: null,
        beforeDispatch: null,
    };
    if (typeof handlerObj === 'number') {
        result.pluginEventType = handlerObj;
    }
    else if (typeof handlerObj === 'function') {
        result.beforeDispatch = handlerObj;
    }
    else if (typeof handlerObj === 'object') {
        result = handlerObj;
    }
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/coreApiMap.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/coreApiMap.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var addUndoSnapshot_1 = __webpack_require__(/*! ./addUndoSnapshot */ "./packages/roosterjs-editor-core/lib/coreApi/addUndoSnapshot.ts");
var attachDomEvent_1 = __webpack_require__(/*! ./attachDomEvent */ "./packages/roosterjs-editor-core/lib/coreApi/attachDomEvent.ts");
var createPasteFragment_1 = __webpack_require__(/*! ./createPasteFragment */ "./packages/roosterjs-editor-core/lib/coreApi/createPasteFragment.ts");
var ensureTypeInContainer_1 = __webpack_require__(/*! ./ensureTypeInContainer */ "./packages/roosterjs-editor-core/lib/coreApi/ensureTypeInContainer.ts");
var focus_1 = __webpack_require__(/*! ./focus */ "./packages/roosterjs-editor-core/lib/coreApi/focus.ts");
var getContent_1 = __webpack_require__(/*! ./getContent */ "./packages/roosterjs-editor-core/lib/coreApi/getContent.ts");
var getSelectionRange_1 = __webpack_require__(/*! ./getSelectionRange */ "./packages/roosterjs-editor-core/lib/coreApi/getSelectionRange.ts");
var getStyleBasedFormatState_1 = __webpack_require__(/*! ./getStyleBasedFormatState */ "./packages/roosterjs-editor-core/lib/coreApi/getStyleBasedFormatState.ts");
var hasFocus_1 = __webpack_require__(/*! ./hasFocus */ "./packages/roosterjs-editor-core/lib/coreApi/hasFocus.ts");
var insertNode_1 = __webpack_require__(/*! ./insertNode */ "./packages/roosterjs-editor-core/lib/coreApi/insertNode.ts");
var restoreUndoSnapshot_1 = __webpack_require__(/*! ./restoreUndoSnapshot */ "./packages/roosterjs-editor-core/lib/coreApi/restoreUndoSnapshot.ts");
var selectRange_1 = __webpack_require__(/*! ./selectRange */ "./packages/roosterjs-editor-core/lib/coreApi/selectRange.ts");
var setContent_1 = __webpack_require__(/*! ./setContent */ "./packages/roosterjs-editor-core/lib/coreApi/setContent.ts");
var switchShadowEdit_1 = __webpack_require__(/*! ./switchShadowEdit */ "./packages/roosterjs-editor-core/lib/coreApi/switchShadowEdit.ts");
var transformColor_1 = __webpack_require__(/*! ./transformColor */ "./packages/roosterjs-editor-core/lib/coreApi/transformColor.ts");
var triggerEvent_1 = __webpack_require__(/*! ./triggerEvent */ "./packages/roosterjs-editor-core/lib/coreApi/triggerEvent.ts");
/**
 * @internal
 */
exports.coreApiMap = {
    attachDomEvent: attachDomEvent_1.attachDomEvent,
    addUndoSnapshot: addUndoSnapshot_1.addUndoSnapshot,
    createPasteFragment: createPasteFragment_1.createPasteFragment,
    ensureTypeInContainer: ensureTypeInContainer_1.ensureTypeInContainer,
    focus: focus_1.focus,
    getContent: getContent_1.getContent,
    getSelectionRange: getSelectionRange_1.getSelectionRange,
    getStyleBasedFormatState: getStyleBasedFormatState_1.getStyleBasedFormatState,
    hasFocus: hasFocus_1.hasFocus,
    insertNode: insertNode_1.insertNode,
    restoreUndoSnapshot: restoreUndoSnapshot_1.restoreUndoSnapshot,
    selectRange: selectRange_1.selectRange,
    setContent: setContent_1.setContent,
    switchShadowEdit: switchShadowEdit_1.switchShadowEdit,
    transformColor: transformColor_1.transformColor,
    triggerEvent: triggerEvent_1.triggerEvent,
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/createPasteFragment.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/createPasteFragment.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var START_FRAGMENT = '<!--StartFragment-->';
var END_FRAGMENT = '<!--EndFragment-->';
var NBSP_HTML = '\u00A0';
/**
 * @internal
 * Create a DocumentFragment for paste from a ClipboardData
 * @param core The EditorCore object.
 * @param clipboardData Clipboard data retrieved from clipboard
 * @param position The position to paste to
 * @param pasteAsText True to force use plain text as the content to paste, false to choose HTML or Image if any
 * @param applyCurrentStyle True if apply format of current selection to the pasted content,
 * false to keep original foramt
 */
exports.createPasteFragment = function (core, clipboardData, position, pasteAsText, applyCurrentStyle) {
    var _a, _b;
    if (!clipboardData) {
        return null;
    }
    // Step 1: Prepare BeforePasteEvent object
    var event = createBeforePasteEvent(core, clipboardData);
    var fragment = event.fragment, sanitizingOption = event.sanitizingOption;
    var rawHtml = clipboardData.rawHtml, text = clipboardData.text, imageDataUri = clipboardData.imageDataUri;
    var document = core.contentDiv.ownerDocument;
    var doc;
    // Step 2: Fill the BeforePasteEvent object, especially the fragment for paste
    if (!pasteAsText && !text && imageDataUri) {
        // Paste image
        var img = document.createElement('img');
        img.style.maxWidth = '100%';
        img.src = imageDataUri;
        fragment.appendChild(img);
    }
    else if (!pasteAsText &&
        rawHtml && ((_a = (doc = new DOMParser().parseFromString(rawHtml, 'text/html'))) === null || _a === void 0 ? void 0 : _a.body)) {
        // Paste HTML
        var attributes = (_b = doc.querySelector('html')) === null || _b === void 0 ? void 0 : _b.attributes;
        (attributes ? roosterjs_editor_dom_1.toArray(attributes) : []).reduce(function (attrs, attr) {
            attrs[attr.name] = attr.value;
            return attrs;
        }, event.htmlAttributes);
        roosterjs_editor_dom_1.toArray(doc.querySelectorAll('meta')).reduce(function (attrs, meta) {
            attrs[meta.name] = meta.content;
            return attrs;
        }, event.htmlAttributes);
        // Move all STYLE nodes into header, and save them into sanitizing options.
        // Because if we directly move them into a fragment, all sheets under STYLE will be lost.
        processStyles(doc, function (style) {
            doc.head.appendChild(style);
            sanitizingOption.additionalGlobalStyleNodes.push(style);
        });
        var startIndex = rawHtml.indexOf(START_FRAGMENT);
        var endIndex = rawHtml.lastIndexOf(END_FRAGMENT);
        if (startIndex >= 0 && endIndex >= startIndex + START_FRAGMENT.length) {
            event.htmlBefore = rawHtml.substr(0, startIndex);
            event.htmlAfter = rawHtml.substr(endIndex + END_FRAGMENT.length);
            doc.body.innerHTML = clipboardData.html = rawHtml.substring(startIndex + START_FRAGMENT.length, endIndex);
            // Remove style nodes just added by setting innerHTML of body since we already have all
            // style nodes in header.
            // Here we use doc.body instead of doc because we only want to remove STYLE nodes under BODY
            // and the nodes under HEAD are still used when convert global CSS to inline
            processStyles(doc.body, function (style) { var _a; return (_a = style.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(style); });
        }
        while (doc.body.firstChild) {
            fragment.appendChild(doc.body.firstChild);
        }
        if (applyCurrentStyle && position) {
            var format_1 = getCurrentFormat(core, position.node);
            roosterjs_editor_dom_1.applyTextStyle(fragment, function (node) { return roosterjs_editor_dom_1.applyFormat(node, format_1); });
        }
    }
    else if (text) {
        // Paste text
        text.split('\n').forEach(function (line, index, lines) {
            line = line
                .replace(/^ /g, NBSP_HTML)
                .replace(/\r/g, '')
                .replace(/ {2}/g, ' ' + NBSP_HTML);
            var textNode = document.createTextNode(line);
            // There are 3 scenarios:
            // 1. Single line: Paste as it is
            // 2. Two lines: Add <br> between the lines
            // 3. 3 or More lines, For first and last line, paste as it is. For middle lines, wrap with DIV, and add BR if it is empty line
            if (lines.length == 2 && index == 0) {
                // 1 of 2 lines scenario, add BR
                fragment.appendChild(textNode);
                fragment.appendChild(document.createElement('br'));
            }
            else if (index > 0 && index < lines.length - 1) {
                // Middle line of >=3 lines scenario, wrap with DIV
                fragment.appendChild(roosterjs_editor_dom_1.wrap(line == '' ? document.createElement('br') : textNode));
            }
            else {
                // All others, paste as it is
                fragment.appendChild(textNode);
            }
        });
    }
    // Step 3: Trigger BeforePasteEvent so that plugins can do proper change before paste
    core.api.triggerEvent(core, event, true /*broadcast*/);
    // Step 4. Sanitize the fragment before paste to make sure the content is safe
    var sanitizer = new roosterjs_editor_dom_1.HtmlSanitizer(event.sanitizingOption);
    sanitizer.convertGlobalCssToInlineCss(fragment);
    sanitizer.sanitize(fragment, position && roosterjs_editor_dom_1.getInheritableStyles(position.element));
    return fragment;
};
function getCurrentFormat(core, node) {
    var pendableFormat = roosterjs_editor_dom_1.getPendableFormatState(core.contentDiv.ownerDocument);
    var styleBasedForamt = core.api.getStyleBasedFormatState(core, node);
    return {
        fontFamily: styleBasedForamt.fontName,
        fontSize: styleBasedForamt.fontSize,
        textColor: styleBasedForamt.textColor,
        backgroundColor: styleBasedForamt.backgroundColor,
        textColors: styleBasedForamt.textColors,
        backgroundColors: styleBasedForamt.backgroundColors,
        bold: pendableFormat.isBold,
        italic: pendableFormat.isItalic,
        underline: pendableFormat.isUnderline,
    };
}
function createBeforePasteEvent(core, clipboardData) {
    return {
        eventType: 10 /* BeforePaste */,
        clipboardData: clipboardData,
        fragment: core.contentDiv.ownerDocument.createDocumentFragment(),
        sanitizingOption: roosterjs_editor_dom_1.createDefaultHtmlSanitizerOptions(),
        htmlBefore: '',
        htmlAfter: '',
        htmlAttributes: {},
    };
}
function processStyles(node, callback) {
    roosterjs_editor_dom_1.toArray(node.querySelectorAll('style')).forEach(callback);
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/ensureTypeInContainer.ts":
/*!*****************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/ensureTypeInContainer.ts ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * When typing goes directly under content div, many things can go wrong
 * We fix it by wrapping it with a div and reposition cursor within the div
 */
exports.ensureTypeInContainer = function (core, position, keyboardEvent) {
    position = position.normalize();
    var block = roosterjs_editor_dom_1.getBlockElementAtNode(core.contentDiv, position.node);
    var formatNode;
    if (block) {
        formatNode = block.collapseToSingleElement();
        // if the block is empty, apply default format
        // Otherwise, leave it as it is as we don't want to change the style for existing data
        // unless the block was just created by the keyboard event (e.g. ctrl+a & start typing)
        var shouldSetNodeStyles = roosterjs_editor_dom_1.isNodeEmpty(formatNode) ||
            (keyboardEvent && wasNodeJustCreatedByKeyboardEvent(keyboardEvent, formatNode));
        formatNode = formatNode && shouldSetNodeStyles ? formatNode : null;
    }
    else {
        // Only reason we don't get the selection block is that we have an empty content div
        // which can happen when users removes everything (i.e. select all and DEL, or backspace from very end to begin)
        // The fix is to add a DIV wrapping, apply default format and move cursor over
        formatNode = roosterjs_editor_dom_1.fromHtml(roosterjs_editor_dom_1.Browser.isEdge ? '<div><span><br></span></div>' : '<div><br></div>', core.contentDiv.ownerDocument)[0];
        core.api.insertNode(core, formatNode, {
            position: 1 /* End */,
            updateCursor: false,
            replaceSelection: false,
            insertOnNewLine: false,
        });
        // element points to a wrapping node we added "<div><br></div>". We should move the selection left to <br>
        position = new roosterjs_editor_dom_1.Position(formatNode.firstChild, 0 /* Begin */);
    }
    if (formatNode) {
        roosterjs_editor_dom_1.applyFormat(formatNode, core.lifecycle.defaultFormat, core.lifecycle.isDarkMode);
    }
    // If this is triggered by a keyboard event, let's select the new position
    if (keyboardEvent) {
        core.api.selectRange(core, roosterjs_editor_dom_1.createRange(position));
    }
};
function wasNodeJustCreatedByKeyboardEvent(event, formatNode) {
    return (roosterjs_editor_dom_1.safeInstanceOf(event.target, 'Node') &&
        event.target.contains(formatNode) &&
        event.key === formatNode.innerText);
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/focus.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/focus.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Focus to editor. If there is a cached selection range, use it as current selection
 * @param core The EditorCore object
 */
exports.focus = function (core) {
    if (!core.lifecycle.shadowEditFragment) {
        if (!core.api.hasFocus(core) ||
            !core.api.getSelectionRange(core, false /*tryGetFromCache*/)) {
            // Focus (document.activeElement indicates) and selection are mostly in sync, but could be out of sync in some extreme cases.
            // i.e. if you programmatically change window selection to point to a non-focusable DOM element (i.e. tabindex=-1 etc.).
            // On Chrome/Firefox, it does not change document.activeElement. On Edge/IE, it change document.activeElement to be body
            // Although on Chrome/Firefox, document.activeElement points to editor, you cannot really type which we don't want (no cursor).
            // So here we always do a live selection pull on DOM and make it point in Editor. The pitfall is, the cursor could be reset
            // to very begin to of editor since we don't really have last saved selection (created on blur which does not fire in this case).
            // It should be better than the case you cannot type
            if (!core.domEvent.selectionRange ||
                !core.api.selectRange(core, core.domEvent.selectionRange, true /*skipSameRange*/)) {
                var node = roosterjs_editor_dom_1.getFirstLeafNode(core.contentDiv) || core.contentDiv;
                core.api.selectRange(core, roosterjs_editor_dom_1.createRange(node, 0 /* Begin */), true /*skipSameRange*/);
            }
        }
        // remember to clear cached selection range
        core.domEvent.selectionRange = null;
        // This is more a fallback to ensure editor gets focus if it didn't manage to move focus to editor
        if (!core.api.hasFocus(core)) {
            core.contentDiv.focus();
        }
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/getContent.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/getContent.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Get current editor content as HTML string
 * @param core The EditorCore object
 * @param mode specify what kind of HTML content to retrieve
 * @returns HTML string representing current editor content
 */
exports.getContent = function (core, mode) {
    var content = '';
    var triggerExtractContentEvent = mode == 0 /* CleanHTML */;
    var includeSelectionMarker = mode == 2 /* RawHTMLWithSelection */;
    // When there is fragment for shadow edit, always use the cached fragment as document since HTML node in editor
    // has been changed by uncommited shadow edit which should be ignored.
    var root = core.lifecycle.shadowEditFragment || core.contentDiv;
    if (mode == 3 /* PlainText */) {
        content = roosterjs_editor_dom_1.getTextContent(root);
    }
    else if (triggerExtractContentEvent || core.lifecycle.isDarkMode) {
        var clonedRoot = cloneNode(root);
        var originalRange = core.api.getSelectionRange(core, true /*tryGetFromCache*/);
        var path = !includeSelectionMarker
            ? null
            : core.lifecycle.shadowEditFragment
                ? core.lifecycle.shadowEditSelectionPath
                : originalRange
                    ? roosterjs_editor_dom_1.getSelectionPath(core.contentDiv, originalRange)
                    : null;
        var range = path && roosterjs_editor_dom_1.createRange(clonedRoot, path.start, path.end);
        if (core.lifecycle.isDarkMode) {
            core.api.transformColor(core, clonedRoot, false /*includeSelf*/, null /*callback*/, 1 /* DarkToLight */);
        }
        if (triggerExtractContentEvent) {
            core.api.triggerEvent(core, {
                eventType: 8 /* ExtractContentWithDom */,
                clonedRoot: clonedRoot,
            }, true /*broadcast*/);
            content = clonedRoot.innerHTML;
        }
        else if (range) {
            // range is not null, which means we want to include a selection path in the content
            content = roosterjs_editor_dom_1.getHtmlWithSelectionPath(clonedRoot, range);
        }
        else {
            content = clonedRoot.innerHTML;
        }
    }
    else {
        content = roosterjs_editor_dom_1.getHtmlWithSelectionPath(root, includeSelectionMarker && core.api.getSelectionRange(core, true /*tryGetFromCache*/));
    }
    return content;
};
function cloneNode(node) {
    var clonedNode;
    if (roosterjs_editor_dom_1.safeInstanceOf(node, 'DocumentFragment')) {
        clonedNode = node.ownerDocument.createElement('div');
        clonedNode.appendChild(node.cloneNode(true /*deep*/));
    }
    else {
        clonedNode = node.cloneNode(true /*deep*/);
    }
    return clonedNode;
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/getSelectionRange.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/getSelectionRange.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Get current or cached selection range
 * @param core The EditorCore object
 * @param tryGetFromCache Set to true to retrieve the selection range from cache if editor doesn't own the focus now
 * @returns A Range object of the selection range
 */
exports.getSelectionRange = function (core, tryGetFromCache) {
    var _a;
    var result = null;
    if (core.lifecycle.shadowEditFragment) {
        result =
            core.lifecycle.shadowEditSelectionPath &&
                roosterjs_editor_dom_1.createRange(core.contentDiv, core.lifecycle.shadowEditSelectionPath.start, core.lifecycle.shadowEditSelectionPath.end);
        return result;
    }
    else {
        if (!tryGetFromCache || core.api.hasFocus(core)) {
            var selection = (_a = core.contentDiv.ownerDocument.defaultView) === null || _a === void 0 ? void 0 : _a.getSelection();
            if (selection && selection.rangeCount > 0) {
                var range = selection.getRangeAt(0);
                if (roosterjs_editor_dom_1.contains(core.contentDiv, range)) {
                    result = range;
                }
            }
        }
        if (!result && tryGetFromCache) {
            result = core.domEvent.selectionRange;
        }
        return result;
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/getStyleBasedFormatState.ts":
/*!********************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/getStyleBasedFormatState.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var ORIGINAL_STYLE_COLOR_SELECTOR = "[data-" + "ogsc" /* OriginalStyleColor */ + "],[data-" + "ogac" /* OriginalAttributeColor */ + "]";
var ORIGINAL_STYLE_BACK_COLOR_SELECTOR = "[data-" + "ogsb" /* OriginalStyleBackgroundColor */ + "],[data-" + "ogab" /* OriginalAttributeBackgroundColor */ + "]";
/**
 * @internal
 * Get style based format state from current selection, including font name/size and colors
 * @param core The EditorCore objects
 * @param node The node to get style from
 */
exports.getStyleBasedFormatState = function (core, node) {
    if (!node) {
        return {};
    }
    var styles = node ? roosterjs_editor_dom_1.getComputedStyles(node) : [];
    var isDarkMode = core.lifecycle.isDarkMode;
    var root = core.contentDiv;
    var ogTextColorNode = isDarkMode && roosterjs_editor_dom_1.findClosestElementAncestor(node, root, ORIGINAL_STYLE_COLOR_SELECTOR);
    var ogBackgroundColorNode = isDarkMode && roosterjs_editor_dom_1.findClosestElementAncestor(node, root, ORIGINAL_STYLE_BACK_COLOR_SELECTOR);
    return {
        fontName: styles[0],
        fontSize: styles[1],
        textColor: styles[2],
        backgroundColor: styles[3],
        textColors: ogTextColorNode
            ? {
                darkModeColor: styles[2],
                lightModeColor: ogTextColorNode.dataset["ogsc" /* OriginalStyleColor */] ||
                    ogTextColorNode.dataset["ogac" /* OriginalAttributeColor */],
            }
            : undefined,
        backgroundColors: ogBackgroundColorNode
            ? {
                darkModeColor: styles[3],
                lightModeColor: ogBackgroundColorNode.dataset["ogsb" /* OriginalStyleBackgroundColor */] ||
                    ogBackgroundColorNode.dataset["ogab" /* OriginalAttributeBackgroundColor */],
            }
            : undefined,
    };
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/hasFocus.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/hasFocus.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Check if the editor has focus now
 * @param core The EditorCore object
 * @returns True if the editor has focus, otherwise false
 */
exports.hasFocus = function (core) {
    var activeElement = core.contentDiv.ownerDocument.activeElement;
    return (activeElement && roosterjs_editor_dom_1.contains(core.contentDiv, activeElement, true /*treatSameNodeAsContain*/));
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/insertNode.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/insertNode.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var adjustSteps = [handleHyperLink, handleStructuredNode, handleParagraph, handleVoidElement];
function getInitialRange(core, option) {
    // Selection start replaces based on the current selection.
    // Range inserts based on a provided range.
    // Both have the potential to use the current selection to restore cursor position
    // So in both cases we need to store the selection state.
    var range = core.api.getSelectionRange(core, true /*tryGetFromCache*/);
    var rangeToRestore = null;
    if (option.position == 5 /* Range */) {
        rangeToRestore = range;
        range = option.range;
    }
    else if (range) {
        rangeToRestore = range.cloneRange();
    }
    return { range: range, rangeToRestore: rangeToRestore };
}
/**
 * @internal
 * Insert a DOM node into editor content
 * @param core The EditorCore object. No op if null.
 * @param option An insert option object to specify how to insert the node
 */
exports.insertNode = function (core, node, option) {
    option = option || {
        position: 3 /* SelectionStart */,
        insertOnNewLine: false,
        updateCursor: true,
        replaceSelection: true,
    };
    var contentDiv = core.contentDiv;
    if (option.updateCursor) {
        core.api.focus(core);
    }
    if (option.position == 4 /* Outside */) {
        contentDiv.parentNode.insertBefore(node, contentDiv.nextSibling);
        return true;
    }
    core.api.transformColor(core, node, true /*includeSelf*/, function () {
        switch (option.position) {
            case 0 /* Begin */:
            case 1 /* End */: {
                var isBegin = option.position == 0 /* Begin */;
                var block = roosterjs_editor_dom_1.getFirstLastBlockElement(contentDiv, isBegin);
                var insertedNode_1;
                if (block) {
                    var refNode = isBegin ? block.getStartNode() : block.getEndNode();
                    if (option.insertOnNewLine ||
                        refNode.nodeType == 3 /* Text */ ||
                        roosterjs_editor_dom_1.isVoidHtmlElement(refNode)) {
                        // For insert on new line, or refNode is text or void html element (HR, BR etc.)
                        // which cannot have children, i.e. <div>hello<br>world</div>. 'hello', 'world' are the
                        // first and last node. Insert before 'hello' or after 'world', but still inside DIV
                        if (roosterjs_editor_dom_1.safeInstanceOf(node, 'DocumentFragment')) {
                            // if the node to be inserted is DocumentFragment, use its childNodes as insertedNode
                            // because insertBefore() returns an empty DocumentFragment
                            insertedNode_1 = roosterjs_editor_dom_1.toArray(node.childNodes);
                            refNode.parentNode.insertBefore(node, isBegin ? refNode : refNode.nextSibling);
                        }
                        else {
                            insertedNode_1 = refNode.parentNode.insertBefore(node, isBegin ? refNode : refNode.nextSibling);
                        }
                    }
                    else {
                        // if the refNode can have child, use appendChild (which is like to insert as first/last child)
                        // i.e. <div>hello</div>, the content will be inserted before/after hello
                        insertedNode_1 = refNode.insertBefore(node, isBegin ? refNode.firstChild : null);
                    }
                }
                else {
                    // No first block, this can happen when editor is empty. Use appendChild to insert the content in contentDiv
                    insertedNode_1 = contentDiv.appendChild(node);
                }
                // Final check to see if the inserted node is a block. If not block and the ask is to insert on new line,
                // add a DIV wrapping
                if (insertedNode_1 && option.insertOnNewLine) {
                    var nodes = Array.isArray(insertedNode_1) ? insertedNode_1 : [insertedNode_1];
                    if (!roosterjs_editor_dom_1.isBlockElement(nodes[0]) || !roosterjs_editor_dom_1.isBlockElement(nodes[nodes.length - 1])) {
                        roosterjs_editor_dom_1.wrap(nodes);
                    }
                }
                break;
            }
            case 2 /* DomEnd */:
                // Use appendChild to insert the node at the end of the content div.
                var insertedNode = contentDiv.appendChild(node);
                // Final check to see if the inserted node is a block. If not block and the ask is to insert on new line,
                // add a DIV wrapping
                if (insertedNode && option.insertOnNewLine && !roosterjs_editor_dom_1.isBlockElement(insertedNode)) {
                    roosterjs_editor_dom_1.wrap(insertedNode);
                }
                break;
            case 5 /* Range */:
            case 3 /* SelectionStart */:
                var _a = getInitialRange(core, option), range = _a.range, rangeToRestore = _a.rangeToRestore;
                if (!range) {
                    return;
                }
                // if to replace the selection and the selection is not collapsed, remove the the content at selection first
                if (option.replaceSelection && !range.collapsed) {
                    range.deleteContents();
                }
                var pos_1 = roosterjs_editor_dom_1.Position.getStart(range);
                var blockElement = void 0;
                if (option.insertOnNewLine &&
                    (blockElement = roosterjs_editor_dom_1.getBlockElementAtNode(contentDiv, pos_1.normalize().node))) {
                    pos_1 = new roosterjs_editor_dom_1.Position(blockElement.getEndNode(), -3 /* After */);
                }
                else {
                    adjustSteps.forEach(function (handler) {
                        pos_1 = handler(contentDiv, node, pos_1);
                    });
                }
                var nodeForCursor = node.nodeType == 11 /* DocumentFragment */ ? node.lastChild : node;
                range = roosterjs_editor_dom_1.createRange(pos_1);
                range.insertNode(node);
                if (option.updateCursor && nodeForCursor) {
                    rangeToRestore = roosterjs_editor_dom_1.createRange(new roosterjs_editor_dom_1.Position(nodeForCursor, -3 /* After */).normalize());
                }
                core.api.selectRange(core, rangeToRestore);
                break;
        }
    }, 0 /* LightToDark */);
    return true;
};
function handleHyperLink(root, nodeToInsert, position) {
    var blockElement = roosterjs_editor_dom_1.getBlockElementAtNode(root, position.node);
    if (blockElement) {
        // Find the first <A> tag within current block which covers current selection
        // If there are more than one nested, let's handle the first one only since that is not a common scenario.
        var anchor = roosterjs_editor_dom_1.queryElements(root, 'a[href]', null /*forEachCallback*/, 1 /* OnSelection */, roosterjs_editor_dom_1.createRange(position)).filter(function (a) { return blockElement.contains(a); })[0];
        // If this is about to insert node to an empty A tag, clear the A tag and reset position
        if (anchor && roosterjs_editor_dom_1.isNodeEmpty(anchor)) {
            position = new roosterjs_editor_dom_1.Position(anchor, -2 /* Before */);
            safeRemove(anchor);
            anchor = null;
        }
        // If this is about to insert nodes which contains A tag into another A tag, need to break current A tag
        // otherwise we will have nested A tags which is a wrong HTML structure
        if (anchor &&
            nodeToInsert.querySelector &&
            nodeToInsert.querySelector('a[href]')) {
            var normalizedPosition = position.normalize();
            var parentNode = normalizedPosition.node.parentNode;
            var nextNode = normalizedPosition.node.nodeType == 3 /* Text */
                ? roosterjs_editor_dom_1.splitTextNode(normalizedPosition.node, normalizedPosition.offset, false /*returnFirstPart*/)
                : normalizedPosition.isAtEnd
                    ? normalizedPosition.node.nextSibling
                    : normalizedPosition.node;
            var splitter = root.ownerDocument.createTextNode('');
            parentNode.insertBefore(splitter, nextNode);
            while (roosterjs_editor_dom_1.contains(anchor, splitter)) {
                splitter = roosterjs_editor_dom_1.splitBalancedNodeRange(splitter);
            }
            position = new roosterjs_editor_dom_1.Position(splitter, -2 /* Before */);
            safeRemove(splitter);
        }
    }
    return position;
}
function handleStructuredNode(root, nodeToInsert, position) {
    var rootNodeToInsert = nodeToInsert;
    if (rootNodeToInsert.nodeType == 11 /* DocumentFragment */) {
        var rootNodes = roosterjs_editor_dom_1.toArray(rootNodeToInsert.childNodes).filter(function (n) { return roosterjs_editor_dom_1.getTagOfNode(n) != 'BR'; });
        rootNodeToInsert = rootNodes.length == 1 ? rootNodes[0] : null;
    }
    var tag = roosterjs_editor_dom_1.getTagOfNode(rootNodeToInsert);
    var hasBrNextToRoot = tag && roosterjs_editor_dom_1.getTagOfNode(rootNodeToInsert.nextSibling) == 'BR';
    var listItem = roosterjs_editor_dom_1.findClosestElementAncestor(position.node, root, 'LI');
    var listNode = listItem && roosterjs_editor_dom_1.findClosestElementAncestor(listItem, root, 'OL,UL');
    var tdNode = roosterjs_editor_dom_1.findClosestElementAncestor(position.node, root, 'TD,TH');
    var trNode = tdNode && roosterjs_editor_dom_1.findClosestElementAncestor(tdNode, root, 'TR');
    if (tag == 'LI') {
        tag = listNode ? roosterjs_editor_dom_1.getTagOfNode(listNode) : 'UL';
        rootNodeToInsert = roosterjs_editor_dom_1.wrap(rootNodeToInsert, tag);
    }
    if ((tag == 'OL' || tag == 'UL') && roosterjs_editor_dom_1.getTagOfNode(rootNodeToInsert.firstChild) == 'LI') {
        var shouldInsertListAsText = !rootNodeToInsert.firstChild.nextSibling && !hasBrNextToRoot;
        if (hasBrNextToRoot && rootNodeToInsert.parentNode) {
            safeRemove(rootNodeToInsert.nextSibling);
        }
        if (shouldInsertListAsText) {
            roosterjs_editor_dom_1.unwrap(rootNodeToInsert.firstChild);
            roosterjs_editor_dom_1.unwrap(rootNodeToInsert);
        }
        else if (roosterjs_editor_dom_1.getTagOfNode(listNode) == tag) {
            roosterjs_editor_dom_1.unwrap(rootNodeToInsert);
            position = new roosterjs_editor_dom_1.Position(listItem, roosterjs_editor_dom_1.isPositionAtBeginningOf(position, listItem)
                ? -2 /* Before */
                : -3 /* After */);
        }
    }
    else if (tag == 'TABLE' && trNode) {
        // When inserting a table into a table, if these tables have the same column count, and
        // current position is at beginning of a row, then merge these two tables
        var newTable = new roosterjs_editor_dom_1.VTable(rootNodeToInsert);
        var currentTable = new roosterjs_editor_dom_1.VTable(tdNode);
        if (currentTable.col == 0 &&
            tdNode == currentTable.getCell(currentTable.row, 0).td &&
            newTable.cells[0] &&
            newTable.cells[0].length == currentTable.cells[0].length &&
            roosterjs_editor_dom_1.isPositionAtBeginningOf(position, tdNode)) {
            if (roosterjs_editor_dom_1.getTagOfNode(rootNodeToInsert.firstChild) == 'TBODY' &&
                !rootNodeToInsert.firstChild.nextSibling) {
                roosterjs_editor_dom_1.unwrap(rootNodeToInsert.firstChild);
            }
            roosterjs_editor_dom_1.unwrap(rootNodeToInsert);
            position = new roosterjs_editor_dom_1.Position(trNode, -3 /* After */);
        }
    }
    return position;
}
function handleParagraph(root, nodeToInsert, position) {
    if (roosterjs_editor_dom_1.getTagOfNode(position.node) == 'P') {
        // Insert into a P tag may cause issues when the inserted content contains any block element.
        // Change P tag to DIV to make sure it works well
        var pos = position.normalize();
        var div = roosterjs_editor_dom_1.changeElementTag(position.node, 'div');
        if (pos.node != div) {
            position = pos;
        }
    }
    return position;
}
function handleVoidElement(root, nodeToInsert, position) {
    if (roosterjs_editor_dom_1.isVoidHtmlElement(position.node)) {
        position = new roosterjs_editor_dom_1.Position(position.node, position.isAtEnd ? -3 /* After */ : -2 /* Before */);
    }
    return position;
}
function safeRemove(node) {
    var _a;
    (_a = node === null || node === void 0 ? void 0 : node.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(node);
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/restoreUndoSnapshot.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/restoreUndoSnapshot.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Restore an undo snapshot into editor
 * @param core The editor core object
 * @param step Steps to move, can be 0, positive or negative
 */
exports.restoreUndoSnapshot = function (core, step) {
    if (core.undo.hasNewContent && step < 0) {
        core.api.addUndoSnapshot(core, null /*callback*/, null /*changeSource*/, false /*canUndoByBackspace*/);
    }
    var snapshot = core.undo.snapshotsService.move(step);
    if (snapshot != null) {
        try {
            core.undo.isRestoring = true;
            core.api.setContent(core, snapshot, true /*triggerContentChangedEvent*/);
        }
        finally {
            core.undo.isRestoring = false;
        }
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/selectRange.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/selectRange.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var hasFocus_1 = __webpack_require__(/*! ./hasFocus */ "./packages/roosterjs-editor-core/lib/coreApi/hasFocus.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Change the editor selection to the given range
 * @param core The EditorCore object
 * @param range The range to select
 * @param skipSameRange When set to true, do nothing if the given range is the same with current selection
 * in editor, otherwise it will always remove current selection ranage and set to the given one.
 * This parameter is always treat as true in Edge to avoid some weird runtime exception.
 */
exports.selectRange = function (core, range, skipSameRange) {
    if (!core.lifecycle.shadowEditSelectionPath && roosterjs_editor_dom_1.contains(core.contentDiv, range)) {
        roosterjs_editor_dom_1.addRangeToSelection(range, skipSameRange);
        if (!hasFocus_1.hasFocus(core)) {
            core.domEvent.selectionRange = range;
        }
        if (range.collapsed) {
            // If selected, and current selection is collapsed,
            // need to restore pending format state if exists.
            restorePendingFormatState(core);
        }
        return true;
    }
    else {
        return false;
    }
};
/**
 * Restore cached pending format state (if exist) to current selection
 */
function restorePendingFormatState(core) {
    var contentDiv = core.contentDiv, pendingFormatState = core.pendingFormatState, getSelectionRange = core.api.getSelectionRange;
    if (pendingFormatState.pendableFormatState) {
        var document_1 = contentDiv.ownerDocument;
        var formatState_1 = roosterjs_editor_dom_1.getPendableFormatState(document_1);
        Object.keys(roosterjs_editor_dom_1.PendableFormatCommandMap).forEach(function (key) {
            if (!!pendingFormatState.pendableFormatState[key] != formatState_1[key]) {
                document_1.execCommand(roosterjs_editor_dom_1.PendableFormatCommandMap[key], false, null);
            }
        });
        var range = getSelectionRange(core, true /*tryGetFromCache*/);
        pendingFormatState.pendableFormatPosition = range && roosterjs_editor_dom_1.Position.getStart(range);
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/setContent.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/setContent.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Set HTML content to this editor. All existing content will be replaced. A ContentChanged event will be triggered
 * if triggerContentChangedEvent is set to true
 * @param core The EditorCore object
 * @param content HTML content to set in
 * @param triggerContentChangedEvent True to trigger a ContentChanged event. Default value is true
 */
exports.setContent = function (core, content, triggerContentChangedEvent) {
    var contentChanged = false;
    if (core.contentDiv.innerHTML != content) {
        var range = roosterjs_editor_dom_1.setHtmlWithSelectionPath(core.contentDiv, content);
        core.api.selectRange(core, range);
        contentChanged = true;
    }
    // Convert content even if it hasn't changed.
    core.api.transformColor(core, core.contentDiv, false /*includeSelf*/, null /*callback*/, 0 /* LightToDark */);
    if (triggerContentChangedEvent && (contentChanged || core.lifecycle.isDarkMode)) {
        core.api.triggerEvent(core, {
            eventType: 7 /* ContentChanged */,
            source: "SetContent" /* SetContent */,
        }, false /*broadcast*/);
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/switchShadowEdit.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/switchShadowEdit.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 */
exports.switchShadowEdit = function (core, isOn) {
    var lifecycle = core.lifecycle, contentDiv = core.contentDiv;
    var shadowEditFragment = lifecycle.shadowEditFragment, shadowEditSelectionPath = lifecycle.shadowEditSelectionPath;
    var wasInShadowEdit = !!shadowEditFragment;
    if (isOn) {
        if (!wasInShadowEdit) {
            // Merge sibling text nodes to avoid inaccuracy of text node offset
            contentDiv.normalize();
            var range = core.api.getSelectionRange(core, true /*tryGetFromCache*/);
            shadowEditSelectionPath = range && roosterjs_editor_dom_1.getSelectionPath(contentDiv, range);
            shadowEditFragment = core.contentDiv.ownerDocument.createDocumentFragment();
            while (contentDiv.firstChild) {
                shadowEditFragment.appendChild(contentDiv.firstChild);
            }
            core.api.triggerEvent(core, {
                eventType: 17 /* EnteredShadowEdit */,
                fragment: shadowEditFragment,
                selectionPath: shadowEditSelectionPath,
            }, false /*broadcast*/);
            lifecycle.shadowEditFragment = shadowEditFragment;
            lifecycle.shadowEditSelectionPath = shadowEditSelectionPath;
        }
        contentDiv.innerHTML = '';
        contentDiv.appendChild(lifecycle.shadowEditFragment.cloneNode(true /*deep*/));
    }
    else {
        lifecycle.shadowEditFragment = null;
        lifecycle.shadowEditSelectionPath = null;
        if (wasInShadowEdit) {
            core.api.triggerEvent(core, {
                eventType: 18 /* LeavingShadowEdit */,
            }, false /*broadcast*/);
            contentDiv.innerHTML = '';
            contentDiv.appendChild(shadowEditFragment);
            core.api.focus(core);
            if (shadowEditSelectionPath) {
                core.api.selectRange(core, roosterjs_editor_dom_1.createRange(contentDiv, shadowEditSelectionPath.start, shadowEditSelectionPath.end));
            }
        }
    }
};


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/transformColor.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/transformColor.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var _a, _b;
Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var STYLE_DATASET_MAP = (_a = {},
    /**
     * Original style color
     */
    _a["ogsc" /* OriginalStyleColor */] = function (element, value) {
        return (element.style.color = value);
    },
    /**
     * Original style background color
     */
    _a["ogsb" /* OriginalStyleBackgroundColor */] = function (element, value) {
        return (element.style.backgroundColor = value);
    },
    _a);
var ATTR_DATASET_MAP = (_b = {},
    /**
     * Original attribute color
     */
    _b["ogac" /* OriginalAttributeColor */] = 'color',
    /**
     * Original attribute background color
     */
    _b["ogab" /* OriginalAttributeBackgroundColor */] = 'bgcolor',
    _b);
/**
 * @internal
 * Edit and transform color of elements between light mode and dark mode
 * @param core The EditorCore object
 * @param rootNode The root HTML elements to transform
 * @param includeSelf True to transform the root node as well, otherwise false
 * @param callback The callback function to invoke before do color transformation
 * @param direction To specify the transform direction, light to dark, or dark to light
 */
exports.transformColor = function (core, rootNode, includeSelf, callback, direction) {
    var elementsToTransform = core.lifecycle.isDarkMode ? getAll(rootNode, includeSelf) : [];
    callback === null || callback === void 0 ? void 0 : callback();
    elementsToTransform.forEach(function (element) {
        if (direction == 1 /* DarkToLight */ && (element === null || element === void 0 ? void 0 : element.dataset)) {
            // Reset color styles based on the content of the ogsc/ogsb data element.
            // If those data properties are empty or do not exist, set them anyway to clear the content.
            Object.keys(STYLE_DATASET_MAP).forEach(function (name) {
                STYLE_DATASET_MAP[name](element, getValueOrDefault(element.dataset[name], ''));
                delete element.dataset[name];
            });
            // Some elements might have set attribute colors. We need to reset these as well.
            Object.keys(ATTR_DATASET_MAP).forEach(function (name) {
                var value = element.dataset[name];
                if (getValueOrDefault(value, null)) {
                    element.setAttribute(ATTR_DATASET_MAP[name], value);
                }
                else {
                    element.removeAttribute(ATTR_DATASET_MAP[name]);
                }
                delete element.dataset[name];
            });
        }
        else if (direction == 0 /* LightToDark */ && element) {
            if (core.lifecycle.onExternalContentTransform) {
                core.lifecycle.onExternalContentTransform(element);
            }
            else {
                element.style.color = null;
                element.style.backgroundColor = null;
            }
        }
    });
};
function getValueOrDefault(value, defualtValue) {
    return value && value != 'undefined' && value != 'null' ? value : defualtValue;
}
function getAll(rootNode, includeSelf) {
    var result = [];
    if (roosterjs_editor_dom_1.safeInstanceOf(rootNode, 'HTMLElement')) {
        if (includeSelf) {
            result.push(rootNode);
        }
        var allChildren = rootNode.getElementsByTagName('*');
        roosterjs_editor_dom_1.arrayPush(result, roosterjs_editor_dom_1.toArray(allChildren));
    }
    else if (roosterjs_editor_dom_1.safeInstanceOf(rootNode, 'DocumentFragment')) {
        var allChildren = rootNode.querySelectorAll('*');
        roosterjs_editor_dom_1.arrayPush(result, roosterjs_editor_dom_1.toArray(allChildren));
    }
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/coreApi/triggerEvent.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/coreApi/triggerEvent.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Trigger a plugin event
 * @param core The EditorCore object
 * @param pluginEvent The event object to trigger
 * @param broadcast Set to true to skip the shouldHandleEventExclusively check
 */
exports.triggerEvent = function (core, pluginEvent, broadcast) {
    if (!core.lifecycle.shadowEditFragment &&
        (broadcast || !core.plugins.some(function (plugin) { return handledExclusively(pluginEvent, plugin); }))) {
        core.plugins.forEach(function (plugin) {
            if (plugin.onPluginEvent) {
                plugin.onPluginEvent(pluginEvent);
            }
        });
    }
};
function handledExclusively(event, plugin) {
    var _a;
    if (plugin.onPluginEvent && ((_a = plugin.willHandleEventExclusively) === null || _a === void 0 ? void 0 : _a.call(plugin, event))) {
        plugin.onPluginEvent(event);
        return true;
    }
    return false;
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/CopyPastePlugin.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/CopyPastePlugin.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var CONTAINER_HTML = '<div contenteditable style="width: 1px; height: 1px; overflow: hidden; position: fixed; top: 0; left; 0; -webkit-user-select: text"></div>';
/**
 * @internal
 * Copy and paste plugin for handling onCopy and onPaste event
 */
var CopyPastePlugin = /** @class */ (function () {
    /**
     * Construct a new instance of CopyPastePlugin
     * @param options The editor options
     */
    function CopyPastePlugin(options) {
        var _this = this;
        this.onPaste = function (event) {
            roosterjs_editor_dom_1.extractClipboardEvent(event, function (items) {
                if (items.rawHtml === undefined) {
                    // Can't get pasted HTML directly, need to use a temp DIV to retrieve pasted content.
                    // This is mostly for IE
                    var originalSelectionRange_1 = _this.editor.getSelectionRange();
                    var tempDiv_1 = _this.getTempDiv();
                    _this.editor.runAsync(function () {
                        items.rawHtml = tempDiv_1.innerHTML;
                        _this.cleanUpAndRestoreSelection(tempDiv_1, originalSelectionRange_1);
                        _this.paste(items);
                    });
                }
                else {
                    _this.paste(items);
                }
            }, {
                allowLinkPreview: _this.editor.isFeatureEnabled("PasteWithLinkPreview" /* PasteWithLinkPreview */),
                allowedCustomPasteType: _this.state.allowedCustomPasteType,
            });
        };
        this.state = {
            allowedCustomPasteType: options.allowedCustomPasteType || [],
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    CopyPastePlugin.prototype.getName = function () {
        return 'CopyPaste';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    CopyPastePlugin.prototype.initialize = function (editor) {
        var _this = this;
        this.editor = editor;
        this.disposer = this.editor.addDomEventHandler({
            paste: this.onPaste,
            copy: function (e) { return _this.onCutCopy(e, false /*isCut*/); },
            cut: function (e) { return _this.onCutCopy(e, true /*isCut*/); },
        });
    };
    /**
     * Dispose this plugin
     */
    CopyPastePlugin.prototype.dispose = function () {
        this.disposer();
        this.disposer = null;
        this.editor = null;
    };
    /**
     * Get plugin state object
     */
    CopyPastePlugin.prototype.getState = function () {
        return this.state;
    };
    CopyPastePlugin.prototype.onCutCopy = function (event, isCut) {
        var _this = this;
        var originalRange = this.editor.getSelectionRange();
        if (originalRange && !originalRange.collapsed) {
            var html = this.editor.getContent(2 /* RawHTMLWithSelection */);
            var tempDiv_2 = this.getTempDiv(true /*forceInLightMode*/);
            var newRange = roosterjs_editor_dom_1.setHtmlWithSelectionPath(tempDiv_2, html);
            if (newRange) {
                roosterjs_editor_dom_1.addRangeToSelection(newRange);
            }
            this.editor.triggerPluginEvent(9 /* BeforeCutCopy */, {
                clonedRoot: tempDiv_2,
                range: newRange,
                rawEvent: event,
                isCut: isCut,
            });
            this.editor.runAsync(function (editor) {
                _this.cleanUpAndRestoreSelection(tempDiv_2, originalRange);
                if (isCut) {
                    editor.addUndoSnapshot(function () {
                        var position = _this.editor.deleteSelectedContent();
                        editor.focus();
                        editor.select(position);
                    }, "Cut" /* Cut */);
                }
            });
        }
    };
    CopyPastePlugin.prototype.paste = function (clipboardData) {
        var _this = this;
        if (clipboardData.image) {
            roosterjs_editor_dom_1.readFile(clipboardData.image, function (dataUrl) {
                clipboardData.imageDataUri = dataUrl;
                _this.editor.paste(clipboardData);
            });
        }
        else {
            this.editor.paste(clipboardData);
        }
    };
    CopyPastePlugin.prototype.getTempDiv = function (forceInLightMode) {
        var _this = this;
        var div = this.editor.getCustomData('CopyPasteTempDiv', function () {
            var tempDiv = roosterjs_editor_dom_1.fromHtml(CONTAINER_HTML, _this.editor.getDocument())[0];
            _this.editor.insertNode(tempDiv, {
                position: 4 /* Outside */,
            });
            return tempDiv;
        }, function (tempDiv) { var _a; return (_a = tempDiv.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(tempDiv); });
        if (forceInLightMode) {
            div.style.backgroundColor = 'white';
            div.style.color = 'black';
        }
        div.style.display = '';
        div.focus();
        return div;
    };
    CopyPastePlugin.prototype.cleanUpAndRestoreSelection = function (tempDiv, range) {
        this.editor.select(range);
        tempDiv.style.backgroundColor = '';
        tempDiv.style.color = '';
        tempDiv.style.display = 'none';
        tempDiv.innerHTML = '';
    };
    return CopyPastePlugin;
}());
exports.default = CopyPastePlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/DOMEventPlugin.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/DOMEventPlugin.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * DOMEventPlugin handles customized DOM events, including:
 * 1. Keyboard event
 * 2. Mouse event
 * 3. IME state
 * 4. Drop event
 * 5. Focus and blur event
 * 6. Input event
 * 7. Scroll event
 */
var DOMEventPlugin = /** @class */ (function () {
    /**
     * Construct a new instance of DOMEventPlugin
     * @param options The editor options
     * @param contentDiv The editor content DIV
     */
    function DOMEventPlugin(options, contentDiv) {
        var _this = this;
        var _a;
        this.onDrop = function (e) {
            _this.editor.runAsync(function (editor) {
                editor.addUndoSnapshot(function () { }, "Drop" /* Drop */);
            });
        };
        this.onFocus = function () {
            _this.editor.select(_this.state.selectionRange);
            _this.state.selectionRange = null;
        };
        this.onBlur = function () {
            _this.state.selectionRange = _this.editor.getSelectionRange(false /*tryGetFromCache*/);
        };
        this.onScroll = function (e) {
            _this.editor.triggerPluginEvent(14 /* Scroll */, {
                rawEvent: e,
                scrollContainer: _this.state.scrollContainer,
            });
        };
        this.onKeybaordEvent = function (event) {
            if (roosterjs_editor_dom_1.isCharacterValue(event)) {
                event.stopPropagation();
            }
        };
        this.onInputEvent = function (event) {
            event.stopPropagation();
        };
        this.onContextMenuEvent = function (event) {
            var allItems = [];
            var searcher = _this.editor.getContentSearcherOfCursor();
            var elementBeforeCursor = searcher === null || searcher === void 0 ? void 0 : searcher.getInlineElementBefore();
            var eventTargetNode = event.target;
            if (event.button != 2) {
                eventTargetNode = elementBeforeCursor === null || elementBeforeCursor === void 0 ? void 0 : elementBeforeCursor.getContainerNode();
            }
            _this.state.contextMenuProviders.forEach(function (provider) {
                var items = provider.getContextMenuItems(eventTargetNode);
                if ((items === null || items === void 0 ? void 0 : items.length) > 0) {
                    if (allItems.length > 0) {
                        allItems.push(null);
                    }
                    roosterjs_editor_dom_1.arrayPush(allItems, items);
                }
            });
            _this.editor.triggerPluginEvent(16 /* ContextMenu */, {
                rawEvent: event,
                items: allItems,
            });
        };
        this.state = {
            isInIME: false,
            scrollContainer: options.scrollContainer || contentDiv,
            selectionRange: null,
            stopPrintableKeyboardEventPropagation: !options.allowKeyboardEventPropagation,
            contextMenuProviders: ((_a = options.plugins) === null || _a === void 0 ? void 0 : _a.filter(isContextMenuProvider)) || [],
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    DOMEventPlugin.prototype.getName = function () {
        return 'DOMEvent';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    DOMEventPlugin.prototype.initialize = function (editor) {
        var _a;
        var _this = this;
        this.editor = editor;
        this.disposer = editor.addDomEventHandler((_a = {
                // 1. Keyboard event
                keypress: this.getEventHandler(1 /* KeyPress */),
                keydown: this.getEventHandler(0 /* KeyDown */),
                keyup: this.getEventHandler(2 /* KeyUp */),
                // 2. Mouse event
                mousedown: 5 /* MouseDown */,
                contextmenu: this.onContextMenuEvent,
                // 3. IME state management
                compositionstart: function () { return (_this.state.isInIME = true); },
                compositionend: function (rawEvent) {
                    _this.state.isInIME = false;
                    editor.triggerPluginEvent(4 /* CompositionEnd */, {
                        rawEvent: rawEvent,
                    });
                },
                // 4. Drop event
                drop: this.onDrop,
                // 5. Focus mangement
                focus: this.onFocus
            },
            _a[roosterjs_editor_dom_1.Browser.isIEOrEdge ? 'beforedeactivate' : 'blur'] = this.onBlur,
            // 6. Input event
            _a[roosterjs_editor_dom_1.Browser.isIE ? 'textinput' : 'input'] = this.getEventHandler(3 /* Input */),
            _a));
        // 7. Scroll event
        this.state.scrollContainer.addEventListener('scroll', this.onScroll);
    };
    /**
     * Dispose this plugin
     */
    DOMEventPlugin.prototype.dispose = function () {
        this.state.scrollContainer.removeEventListener('scroll', this.onScroll);
        this.disposer();
        this.disposer = null;
        this.editor = null;
    };
    /**
     * Get plugin state object
     */
    DOMEventPlugin.prototype.getState = function () {
        return this.state;
    };
    DOMEventPlugin.prototype.getEventHandler = function (eventType) {
        return this.state.stopPrintableKeyboardEventPropagation
            ? {
                pluginEventType: eventType,
                beforeDispatch: eventType == 3 /* Input */ ? this.onInputEvent : this.onKeybaordEvent,
            }
            : eventType;
    };
    return DOMEventPlugin;
}());
exports.default = DOMEventPlugin;
function isContextMenuProvider(source) {
    var _a;
    return !!((_a = source) === null || _a === void 0 ? void 0 : _a.getContextMenuItems);
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/EditPlugin.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/EditPlugin.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Edit Component helps handle Content edit features
 */
var EditPlugin = /** @class */ (function () {
    /**
     * Construct a new instance of EditPlugin
     * @param options The editor options
     */
    function EditPlugin() {
        this.state = {
            features: {},
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    EditPlugin.prototype.getName = function () {
        return 'Edit';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    EditPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    EditPlugin.prototype.dispose = function () {
        this.editor = null;
    };
    /**
     * Get plugin state object
     */
    EditPlugin.prototype.getState = function () {
        return this.state;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    EditPlugin.prototype.onPluginEvent = function (event) {
        var hasFunctionKey = false;
        var features;
        var ctrlOrMeta = false;
        if (event.eventType == 0 /* KeyDown */) {
            var rawEvent = event.rawEvent;
            var range = this.editor.getSelectionRange();
            ctrlOrMeta = roosterjs_editor_dom_1.isCtrlOrMetaPressed(rawEvent);
            hasFunctionKey = ctrlOrMeta || rawEvent.altKey;
            features =
                this.state.features[rawEvent.which] ||
                    (range && !range.collapsed && this.state.features[258 /* RANGE */]);
        }
        else if (event.eventType == 7 /* ContentChanged */) {
            features = this.state.features[257 /* CONTENTCHANGED */];
        }
        for (var i = 0; i < (features === null || features === void 0 ? void 0 : features.length); i++) {
            var feature = features[i];
            if ((feature.allowFunctionKeys || !hasFunctionKey) &&
                feature.shouldHandleEvent(event, this.editor, ctrlOrMeta)) {
                feature.handleEvent(event, this.editor);
                break;
            }
        }
    };
    return EditPlugin;
}());
exports.default = EditPlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/EntityPlugin.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/EntityPlugin.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var ENTITY_ID_REGEX = /_\d{1,8}$/;
var ENTITY_CSS_REGEX = '^' + "_Entity" /* ENTITY_INFO_NAME */ + '$';
var ENTITY_ID_CSS_REGEX = '^' + "_EId_" /* ENTITY_ID_PREFIX */;
var ENTITY_TYPE_CSS_REGEX = '^' + "_EType_" /* ENTITY_TYPE_PREFIX */;
var ENTITY_READONLY_CSS_REGEX = '^' + "_EReadonly_" /* ENTITY_READONLY_PREFIX */;
var ALLOWED_CSS_CLASSES = [
    ENTITY_CSS_REGEX,
    ENTITY_ID_CSS_REGEX,
    ENTITY_TYPE_CSS_REGEX,
    ENTITY_READONLY_CSS_REGEX,
];
/**
 * @internal
 * Entity Plugin helps handle all operations related to an entity and generate entity specified events
 */
var EntityPlugin = /** @class */ (function () {
    /**
     * Construct a new instance of EntityPlugin
     */
    function EntityPlugin() {
        var _this = this;
        this.handleCutEvent = function (event) {
            var range = _this.editor.getSelectionRange();
            if (range && !range.collapsed) {
                _this.checkRemoveEntityForRange(event);
            }
        };
        this.state = {
            clickingPoint: null,
            knownEntityElements: [],
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    EntityPlugin.prototype.getName = function () {
        return 'Entity';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    EntityPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    EntityPlugin.prototype.dispose = function () {
        this.editor = null;
        this.state.knownEntityElements = [];
        this.state.clickingPoint = null;
    };
    /**
     * Get plugin state object
     */
    EntityPlugin.prototype.getState = function () {
        return this.state;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    EntityPlugin.prototype.onPluginEvent = function (event) {
        switch (event.eventType) {
            case 5 /* MouseDown */:
                this.handleMouseDownEvent(event.rawEvent);
                break;
            case 6 /* MouseUp */:
                this.handleMouseUpEvent(event.rawEvent);
                break;
            case 0 /* KeyDown */:
                this.handleKeyDownEvent(event.rawEvent);
                break;
            case 9 /* BeforeCutCopy */:
                if (event.isCut) {
                    this.handleCutEvent(event.rawEvent);
                }
                break;
            case 10 /* BeforePaste */:
                this.handleBeforePasteEvent(event.fragment, event.sanitizingOption);
                break;
            case 7 /* ContentChanged */:
                this.handleContentChangedEvent(event.source == "SetContent" /* SetContent */);
                break;
            case 11 /* EditorReady */:
                this.handleContentChangedEvent(true /*resetAll*/);
                break;
            case 8 /* ExtractContentWithDom */:
                this.handleExtractContentWithDomEvent(event.clonedRoot);
                break;
            case 16 /* ContextMenu */:
                this.handleContextMenuEvent(event.rawEvent);
                break;
        }
    };
    EntityPlugin.prototype.handleContextMenuEvent = function (event) {
        var node = event.target;
        var entityElement = node && this.editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node);
        if (entityElement) {
            event.preventDefault();
            this.triggerEvent(entityElement, 2 /* ContextMenu */, event);
        }
    };
    EntityPlugin.prototype.handleMouseDownEvent = function (event) {
        var target = event.target, pageX = event.pageX, pageY = event.pageY;
        var node = target;
        var entityElement = node && this.editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node);
        if (entityElement && !entityElement.isContentEditable) {
            event.preventDefault();
            this.state.clickingPoint = { pageX: pageX, pageY: pageY };
        }
    };
    EntityPlugin.prototype.handleMouseUpEvent = function (event) {
        var target = event.target, pageX = event.pageX, pageY = event.pageY;
        var node = target;
        var entityElement;
        if (this.state.clickingPoint &&
            this.state.clickingPoint.pageX == pageX &&
            this.state.clickingPoint.pageY == pageY &&
            node &&
            !!(entityElement = this.editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node))) {
            event.preventDefault();
            this.triggerEvent(entityElement, 1 /* Click */, event);
            workaroundSelectionIssueForIE(this.editor);
        }
        this.state.clickingPoint = null;
    };
    EntityPlugin.prototype.handleKeyDownEvent = function (event) {
        if (roosterjs_editor_dom_1.isCharacterValue(event) ||
            event.which == 8 /* BACKSPACE */ ||
            event.which == 46 /* DELETE */) {
            var range = this.editor.getSelectionRange();
            if (!range.collapsed) {
                this.checkRemoveEntityForRange(event);
            }
        }
    };
    EntityPlugin.prototype.handleBeforePasteEvent = function (fragment, sanitizingOption) {
        var range = this.editor.getSelectionRange();
        if (!range.collapsed) {
            this.checkRemoveEntityForRange(null /*rawEvent*/);
        }
        roosterjs_editor_dom_1.arrayPush(sanitizingOption.additionalAllowedCssClasses, ALLOWED_CSS_CLASSES);
    };
    EntityPlugin.prototype.handleContentChangedEvent = function (resetAll) {
        var _this = this;
        this.state.knownEntityElements = resetAll
            ? []
            : this.state.knownEntityElements.filter(function (node) { return _this.editor.contains(node); });
        var allId = this.state.knownEntityElements
            .map(function (e) { var _a; return (_a = roosterjs_editor_dom_1.getEntityFromElement(e)) === null || _a === void 0 ? void 0 : _a.id; })
            .filter(function (x) { return !!x; });
        this.editor.queryElements(roosterjs_editor_dom_1.getEntitySelector(), function (element) {
            if (_this.state.knownEntityElements.indexOf(element) < 0) {
                _this.state.knownEntityElements.push(element);
                var entity = roosterjs_editor_dom_1.getEntityFromElement(element);
                _this.hydrateEntity(entity, allId);
            }
        });
    };
    EntityPlugin.prototype.handleExtractContentWithDomEvent = function (root) {
        var _this = this;
        roosterjs_editor_dom_1.toArray(root.querySelectorAll(roosterjs_editor_dom_1.getEntitySelector())).forEach(function (element) {
            element.removeAttribute('contentEditable');
            _this.triggerEvent(element, 8 /* ReplaceTemporaryContent */);
        });
    };
    EntityPlugin.prototype.checkRemoveEntityForRange = function (event) {
        var _this = this;
        var editableEntityElements = [];
        var selector = roosterjs_editor_dom_1.getEntitySelector();
        this.editor.queryElements(selector, 1 /* OnSelection */, function (element) {
            if (element.isContentEditable) {
                editableEntityElements.push(element);
            }
            else {
                _this.triggerEvent(element, 6 /* Overwrite */, event);
            }
        });
        // For editable entities, we need to check if it is fully or partially covered by current selection,
        // and trigger different events;
        if (editableEntityElements.length > 0) {
            var inSelectionEntityElements_1 = this.editor.queryElements(selector, 2 /* InSelection */);
            editableEntityElements.forEach(function (element) {
                var isFullyCovered = inSelectionEntityElements_1.indexOf(element) >= 0;
                _this.triggerEvent(element, isFullyCovered ? 6 /* Overwrite */ : 7 /* PartialOverwrite */, event);
            });
        }
    };
    EntityPlugin.prototype.hydrateEntity = function (entity, knownIds) {
        var id = entity.id, type = entity.type, wrapper = entity.wrapper, isReadonly = entity.isReadonly;
        var match = ENTITY_ID_REGEX.exec(id);
        var baseId = (match ? id.substr(0, id.length - match[0].length) : id) || type;
        // Make sure entity id is unique
        var newId = '';
        for (var num = (match && parseInt(match[1])) || 0;; num++) {
            newId = num > 0 ? baseId + "_" + num : baseId;
            if (knownIds.indexOf(newId) < 0) {
                knownIds.push(newId);
                break;
            }
        }
        roosterjs_editor_dom_1.commitEntity(wrapper, type, isReadonly, newId);
        this.triggerEvent(wrapper, 0 /* NewEntity */);
    };
    EntityPlugin.prototype.triggerEvent = function (element, operation, rawEvent) {
        var entity = element && roosterjs_editor_dom_1.getEntityFromElement(element);
        if (entity) {
            this.editor.triggerPluginEvent(15 /* EntityOperation */, {
                operation: operation,
                rawEvent: rawEvent,
                entity: entity,
            });
        }
    };
    return EntityPlugin;
}());
exports.default = EntityPlugin;
/**
 * IE will show a resize border around the readonly content within content editable DIV
 * This is a workaround to remove it by temporarily move focus out of editor
 */
var workaroundSelectionIssueForIE = roosterjs_editor_dom_1.Browser.isIE
    ? function (editor) {
        editor.runAsync(function (editor) {
            var workaroundButton = editor.getCustomData('ENTITY_IE_FOCUS_BUTTON', function () {
                var button = editor.getDocument().createElement('button');
                button.style.overflow = 'hidden';
                button.style.position = 'fixed';
                button.style.width = '0';
                button.style.height = '0';
                button.style.left = '0';
                button.style.top = '-1000px';
                button.onblur = function () {
                    button.style.display = 'none';
                };
                editor.insertNode(button, {
                    position: 4 /* Outside */,
                });
                return button;
            });
            workaroundButton.style.display = '';
            var range = editor.getDocument().createRange();
            range.setStart(workaroundButton, 0);
            try {
                window.getSelection().removeAllRanges();
                window.getSelection().addRange(range);
            }
            catch (_a) { }
        });
    }
    : function () { };


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/LifecyclePlugin.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/LifecyclePlugin.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var _a, _b;
Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var CONTENT_EDITABLE_ATTRIBUTE_NAME = 'contenteditable';
var COMMANDS = roosterjs_editor_dom_1.Browser.isFirefox
    ? (_a = {},
        /**
         * Disable these object resizing for firefox since other browsers don't have these behaviors
         */
        _a["enableObjectResizing" /* EnableObjectResizing */] = false,
        _a["enableInlineTableEditing" /* EnableInlineTableEditing */] = false,
        _a) : roosterjs_editor_dom_1.Browser.isIE
    ? (_b = {},
        /**
         * Change the default paragraph separater to DIV. This is mainly for IE since its default setting is P
         */
        _b["defaultParagraphSeparator" /* DefaultParagraphSeparator */] = 'div',
        /**
         * Disable auto link feature in IE since we have our own implementation
         */
        _b["AutoUrlDetect" /* AutoUrlDetect */] = false,
        _b) : {};
var DARK_MODE_DEFAULT_FORMAT = {
    backgroundColors: {
        darkModeColor: 'rgb(51,51,51)',
        lightModeColor: 'rgb(255,255,255)',
    },
    textColors: {
        darkModeColor: 'rgb(255,255,255)',
        lightModeColor: 'rgb(0,0,0)',
    },
};
/**
 * @internal
 * Lifecycle plugin handles editor initialization and disposing
 */
var LifecyclePlugin = /** @class */ (function () {
    /**
     * Construct a new instance of LifecyclePlugin
     * @param options The editor options
     * @param contentDiv The editor content DIV
     */
    function LifecyclePlugin(options, contentDiv) {
        var _this = this;
        this.initialContent = options.initialContent || contentDiv.innerHTML || '';
        this.contentDivFormat = roosterjs_editor_dom_1.getComputedStyles(contentDiv);
        // Make the container editable and set its selection styles
        if (contentDiv.getAttribute(CONTENT_EDITABLE_ATTRIBUTE_NAME) === null) {
            this.initializer = function () {
                contentDiv.contentEditable = 'true';
                _this.setSelectStyle(contentDiv, 'text');
            };
            this.disposer = function () {
                _this.setSelectStyle(contentDiv, '');
                contentDiv.removeAttribute(CONTENT_EDITABLE_ATTRIBUTE_NAME);
            };
        }
        this.state = {
            customData: {},
            defaultFormat: options.defaultFormat || null,
            isDarkMode: !!options.inDarkMode,
            onExternalContentTransform: options.onExternalContentTransform,
            experimentalFeatures: options.experimentalFeatures || [],
            shadowEditFragment: null,
            shadowEditSelectionPath: null,
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    LifecyclePlugin.prototype.getName = function () {
        return 'Lifecycle';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    LifecyclePlugin.prototype.initialize = function (editor) {
        var _a;
        this.editor = editor;
        // Calculate default format
        this.recalculateDefaultFormat();
        // Ensure initial content and its format
        this.editor.setContent(this.initialContent, false /*triggerContentChangedEvent*/);
        // Set content DIV to be editable
        (_a = this.initializer) === null || _a === void 0 ? void 0 : _a.call(this);
        // Do proper change for browsers to disable some browser-specified behaviors.
        this.adjustBrowserBehavior();
        // Let other plugins know that we are ready
        this.editor.triggerPluginEvent(11 /* EditorReady */, {}, true /*broadcast*/);
    };
    /**
     * Dispose this plugin
     */
    LifecyclePlugin.prototype.dispose = function () {
        var _this = this;
        this.editor.triggerPluginEvent(12 /* BeforeDispose */, {}, true /*broadcast*/);
        Object.keys(this.state.customData).forEach(function (key) {
            var data = _this.state.customData[key];
            if (data && data.disposer) {
                data.disposer(data.value);
            }
            delete _this.state.customData[key];
        });
        if (this.disposer) {
            this.disposer();
            this.disposer = null;
            this.initializer = null;
        }
        this.editor = null;
    };
    /**
     * Get plugin state object
     */
    LifecyclePlugin.prototype.getState = function () {
        return this.state;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    LifecyclePlugin.prototype.onPluginEvent = function (event) {
        if (event.eventType == 7 /* ContentChanged */ &&
            (event.source == "SwitchToDarkMode" /* SwitchToDarkMode */ ||
                event.source == "SwitchToLightMode" /* SwitchToLightMode */)) {
            this.state.isDarkMode = event.source == "SwitchToDarkMode" /* SwitchToDarkMode */;
            this.recalculateDefaultFormat();
        }
    };
    LifecyclePlugin.prototype.adjustBrowserBehavior = function () {
        var _this = this;
        Object.keys(COMMANDS).forEach(function (command) {
            // Catch any possible exception since this should not block the initialization of editor
            try {
                _this.editor.getDocument().execCommand(command, false, COMMANDS[command]);
            }
            catch (_a) { }
        });
    };
    LifecyclePlugin.prototype.setSelectStyle = function (node, value) {
        node.style.userSelect = value;
        node.style.msUserSelect = value;
        node.style.webkitUserSelect = value;
    };
    LifecyclePlugin.prototype.recalculateDefaultFormat = function () {
        var _a = this.state, baseFormat = _a.defaultFormat, isDarkMode = _a.isDarkMode;
        if (isDarkMode && baseFormat) {
            if (!baseFormat.backgroundColors) {
                baseFormat.backgroundColors = DARK_MODE_DEFAULT_FORMAT.backgroundColors;
            }
            if (!baseFormat.textColors) {
                baseFormat.textColors = DARK_MODE_DEFAULT_FORMAT.textColors;
            }
        }
        if (baseFormat && Object.keys(baseFormat).length === 0) {
            return;
        }
        var _b = baseFormat || {}, fontFamily = _b.fontFamily, fontSize = _b.fontSize, textColor = _b.textColor, textColors = _b.textColors, backgroundColor = _b.backgroundColor, backgroundColors = _b.backgroundColors, bold = _b.bold, italic = _b.italic, underline = _b.underline;
        var defaultFormat = this.contentDivFormat;
        this.state.defaultFormat = {
            fontFamily: fontFamily || defaultFormat[0],
            fontSize: fontSize || defaultFormat[1],
            get textColor() {
                return textColors
                    ? isDarkMode
                        ? textColors.darkModeColor
                        : textColors.lightModeColor
                    : textColor || defaultFormat[2];
            },
            textColors: textColors,
            get backgroundColor() {
                return backgroundColors
                    ? isDarkMode
                        ? backgroundColors.darkModeColor
                        : backgroundColors.lightModeColor
                    : backgroundColor || '';
            },
            backgroundColors: backgroundColors,
            bold: bold,
            italic: italic,
            underline: underline,
        };
    };
    return LifecyclePlugin;
}());
exports.default = LifecyclePlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/MouseUpPlugin.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/MouseUpPlugin.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * MouseUpPlugin help trigger MouseUp event even when mouse up happens outside editor
 * as long as the mouse was pressed within Editor before
 */
var MouseUpPlugin = /** @class */ (function () {
    function MouseUpPlugin() {
        var _this = this;
        this.onMouseUp = function (rawEvent) {
            if (_this.editor) {
                _this.removeMouseUpEventListener();
                _this.editor.triggerPluginEvent(6 /* MouseUp */, {
                    rawEvent: rawEvent,
                });
            }
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    MouseUpPlugin.prototype.getName = function () {
        return 'MouseUp';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    MouseUpPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    MouseUpPlugin.prototype.dispose = function () {
        this.removeMouseUpEventListener();
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    MouseUpPlugin.prototype.onPluginEvent = function (event) {
        if (event.eventType == 5 /* MouseDown */ && !this.mouseUpEventListerAdded) {
            this.editor
                .getDocument()
                .addEventListener('mouseup', this.onMouseUp, true /*setCapture*/);
            this.mouseUpEventListerAdded = true;
        }
    };
    MouseUpPlugin.prototype.removeMouseUpEventListener = function () {
        if (this.mouseUpEventListerAdded) {
            this.mouseUpEventListerAdded = false;
            this.editor.getDocument().removeEventListener('mouseup', this.onMouseUp, true);
        }
    };
    return MouseUpPlugin;
}());
exports.default = MouseUpPlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/PendingFormatStatePlugin.ts":
/*!************************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/PendingFormatStatePlugin.ts ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * PendingFormatStatePlugin handles pending format state management
 */
var PendingFormatStatePlugin = /** @class */ (function () {
    /**
     * Construct a new instance of PendingFormatStatePlugin
     * @param options The editor options
     * @param contentDiv The editor content DIV
     */
    function PendingFormatStatePlugin() {
        this.state = {
            pendableFormatPosition: null,
            pendableFormatState: null,
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    PendingFormatStatePlugin.prototype.getName = function () {
        return 'PendingFormatState';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    PendingFormatStatePlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    PendingFormatStatePlugin.prototype.dispose = function () {
        this.editor = null;
        this.clear();
    };
    /**
     * Get plugin state object
     */
    PendingFormatStatePlugin.prototype.getState = function () {
        return this.state;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    PendingFormatStatePlugin.prototype.onPluginEvent = function (event) {
        switch (event.eventType) {
            case 13 /* PendingFormatStateChanged */:
                // Got PendingFormatStateChagned event, cache current position and pending format
                this.state.pendableFormatPosition = this.getCurrentPosition();
                this.state.pendableFormatState = event.formatState;
                break;
            case 0 /* KeyDown */:
            case 5 /* MouseDown */:
            case 7 /* ContentChanged */:
                // If content or position is changed (by keyboard, mouse, or code),
                // check if current position is still the same with the cached one (if exist),
                // and clear cached format if position is changed since it is out-of-date now
                if (this.state.pendableFormatPosition &&
                    !this.state.pendableFormatPosition.equalTo(this.getCurrentPosition())) {
                    this.clear();
                }
                break;
        }
    };
    PendingFormatStatePlugin.prototype.clear = function () {
        this.state.pendableFormatPosition = null;
        this.state.pendableFormatState = null;
    };
    PendingFormatStatePlugin.prototype.getCurrentPosition = function () {
        var range = this.editor.getSelectionRange();
        return range && roosterjs_editor_dom_1.Position.getStart(range).normalize();
    };
    return PendingFormatStatePlugin;
}());
exports.default = PendingFormatStatePlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/TypeAfterLinkPlugin.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/TypeAfterLinkPlugin.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * TypeAfterLinkPlugin Component helps handle typing event when cursor is right after a link.
 * When typing/pasting after a link, browser may put the new charactor inside link.
 * This plugin overrides this behavior to always insert outside of link.
 */
var TypeAfterLinkPlugin = /** @class */ (function () {
    function TypeAfterLinkPlugin() {
    }
    /**
     * Get a friendly name of  this plugin
     */
    TypeAfterLinkPlugin.prototype.getName = function () {
        return 'TypeAfterLink';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    TypeAfterLinkPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    TypeAfterLinkPlugin.prototype.dispose = function () {
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    TypeAfterLinkPlugin.prototype.onPluginEvent = function (event) {
        if ((roosterjs_editor_dom_1.Browser.isFirefox && event.eventType == 1 /* KeyPress */) ||
            event.eventType == 10 /* BeforePaste */) {
            var range = this.editor.getSelectionRange();
            if (range && range.collapsed && this.editor.getElementAtCursor('A[href]')) {
                var searcher = this.editor.getContentSearcherOfCursor(event);
                var inlineElementBefore = searcher.getInlineElementBefore();
                var inlineElementAfter = searcher.getInlineElementAfter();
                if (inlineElementBefore instanceof roosterjs_editor_dom_1.LinkInlineElement) {
                    this.editor.select(inlineElementBefore.getContainerNode(), -3 /* After */);
                }
                else if (inlineElementAfter instanceof roosterjs_editor_dom_1.LinkInlineElement) {
                    this.editor.select(inlineElementAfter.getContainerNode(), -2 /* Before */);
                }
            }
        }
    };
    return TypeAfterLinkPlugin;
}());
exports.default = TypeAfterLinkPlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/TypeInContainerPlugin.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/TypeInContainerPlugin.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Typing Component helps to ensure typing is always happening under a DOM container
 */
var TypeInContainerPlugin = /** @class */ (function () {
    function TypeInContainerPlugin() {
    }
    /**
     * Get a friendly name of  this plugin
     */
    TypeInContainerPlugin.prototype.getName = function () {
        return 'TypeInContainer';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    TypeInContainerPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    TypeInContainerPlugin.prototype.dispose = function () {
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    TypeInContainerPlugin.prototype.onPluginEvent = function (event) {
        if (event.eventType == 1 /* KeyPress */) {
            // If normalization was not possible before the keypress,
            // check again after the keyboard event has been processed by browser native behaviour.
            //
            // This handles the case where the keyboard event that first inserts content happens when
            // there is already content under the selection (e.g. Ctrl+a -> type new content).
            //
            // Only scheudle when the range is not collapsed to catch this edge case.
            var range = this.editor.getSelectionRange();
            if (!range || this.editor.contains(roosterjs_editor_dom_1.findClosestElementAncestor(range.startContainer))) {
                return;
            }
            if (range.collapsed) {
                this.editor.ensureTypeInContainer(roosterjs_editor_dom_1.Position.getStart(range), event.rawEvent);
            }
            else {
                this.editor.runAsync(function (editor) {
                    editor.ensureTypeInContainer(editor.getFocusedPosition(), event.rawEvent);
                });
            }
        }
    };
    return TypeInContainerPlugin;
}());
exports.default = TypeInContainerPlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/UndoPlugin.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/UndoPlugin.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
// Max stack size that cannot be exceeded. When exceeded, old undo history will be dropped
// to keep size under limit. This is kept at 10MB
var MAXSIZELIMIT = 1e7;
/**
 * @internal
 * Provides snapshot based undo service for Editor
 */
var UndoPlugin = /** @class */ (function () {
    /**
     * Construct a new instance of UndoPlugin
     * @param options The wrapper of the state object
     */
    function UndoPlugin(options) {
        this.state = {
            snapshotsService: options.undoSnapshotService || createUndoSnapshots(),
            isRestoring: false,
            hasNewContent: false,
            isNested: false,
            autoCompletePosition: null,
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    UndoPlugin.prototype.getName = function () {
        return 'Undo';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    UndoPlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    UndoPlugin.prototype.dispose = function () {
        this.editor = null;
    };
    /**
     * Get plugin state object
     */
    UndoPlugin.prototype.getState = function () {
        return this.state;
    };
    /**
     * Check if the plugin should handle the given event exclusively.
     * @param event The event to check
     */
    UndoPlugin.prototype.willHandleEventExclusively = function (event) {
        return (event.eventType == 0 /* KeyDown */ &&
            event.rawEvent.which == 8 /* BACKSPACE */ &&
            this.canUndoAutoComplete());
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    UndoPlugin.prototype.onPluginEvent = function (event) {
        // if editor is in IME, don't do anything
        if (!this.editor || this.editor.isInIME()) {
            return;
        }
        switch (event.eventType) {
            case 11 /* EditorReady */:
                var undoState = this.editor.getUndoState();
                if (!undoState.canUndo && !undoState.canRedo) {
                    // Only add initial snapshot when there is no existing snapshot
                    // Otherwise preserved undo/redo state may be ruined
                    this.addUndoSnapshot();
                }
                break;
            case 0 /* KeyDown */:
                this.onKeyDown(event.rawEvent);
                break;
            case 1 /* KeyPress */:
                this.onKeyPress(event.rawEvent);
                break;
            case 4 /* CompositionEnd */:
                this.clearRedoForInput();
                this.addUndoSnapshot();
                break;
            case 7 /* ContentChanged */:
                if (!this.state.isRestoring) {
                    this.clearRedoForInput();
                }
                break;
        }
    };
    UndoPlugin.prototype.onKeyDown = function (evt) {
        // Handle backspace/delete when there is a selection to take a snapshot
        // since we want the state prior to deletion restorable
        if (evt.which == 8 /* BACKSPACE */ || evt.which == 46 /* DELETE */) {
            if (evt.which == 8 /* BACKSPACE */ && this.canUndoAutoComplete()) {
                evt.preventDefault();
                this.editor.undo();
                this.state.autoCompletePosition = null;
                this.lastKeyPress = evt.which;
            }
            else {
                var selectionRange = this.editor.getSelectionRange();
                // Add snapshot when
                // 1. Something has been selected (not collapsed), or
                // 2. It has a different key code from the last keyDown event (to prevent adding too many snapshot when keeping press the same key), or
                // 3. Ctrl/Meta key is pressed so that a whole word will be deleted
                if (selectionRange &&
                    (!selectionRange.collapsed ||
                        this.lastKeyPress != evt.which ||
                        roosterjs_editor_dom_1.isCtrlOrMetaPressed(evt))) {
                    this.addUndoSnapshot();
                }
                // Since some content is deleted, always set hasNewContent to true so that we will take undo snapshot next time
                this.state.hasNewContent = true;
                this.lastKeyPress = evt.which;
            }
        }
        else if (evt.which >= 33 /* PAGEUP */ && evt.which <= 40 /* DOWN */) {
            // PageUp, PageDown, Home, End, Left, Right, Up, Down
            if (this.state.hasNewContent) {
                this.addUndoSnapshot();
            }
            this.lastKeyPress = 0;
        }
    };
    UndoPlugin.prototype.onKeyPress = function (evt) {
        if (evt.metaKey) {
            // if metaKey is pressed, simply return since no actual effect will be taken on the editor.
            // this is to prevent changing hasNewContent to true when meta + v to paste on Safari.
            return;
        }
        var range = this.editor.getSelectionRange();
        if ((range && !range.collapsed) ||
            (evt.which == 32 /* SPACE */ && this.lastKeyPress != 32 /* SPACE */) ||
            evt.which == 13 /* ENTER */) {
            this.addUndoSnapshot();
            if (evt.which == 13 /* ENTER */) {
                // Treat ENTER as new content so if there is no input after ENTER and undo,
                // we restore the snapshot before ENTER
                this.state.hasNewContent = true;
            }
        }
        else {
            this.clearRedoForInput();
        }
        this.lastKeyPress = evt.which;
    };
    UndoPlugin.prototype.clearRedoForInput = function () {
        this.state.snapshotsService.clearRedo();
        this.lastKeyPress = 0;
        this.state.hasNewContent = true;
    };
    UndoPlugin.prototype.canUndoAutoComplete = function () {
        var _a;
        return (this.state.snapshotsService.canUndoAutoComplete() && ((_a = this.state.autoCompletePosition) === null || _a === void 0 ? void 0 : _a.equalTo(this.editor.getFocusedPosition())));
    };
    UndoPlugin.prototype.addUndoSnapshot = function () {
        this.editor.addUndoSnapshot();
        this.state.autoCompletePosition = null;
    };
    return UndoPlugin;
}());
exports.default = UndoPlugin;
function createUndoSnapshots() {
    var snapshots = roosterjs_editor_dom_1.createSnapshots(MAXSIZELIMIT);
    return {
        canMove: function (delta) { return roosterjs_editor_dom_1.canMoveCurrentSnapshot(snapshots, delta); },
        move: function (delta) { return roosterjs_editor_dom_1.moveCurrentSnapsnot(snapshots, delta); },
        addSnapshot: function (snapshot, isAutoCompleteSnapshot) {
            return roosterjs_editor_dom_1.addSnapshot(snapshots, snapshot, isAutoCompleteSnapshot);
        },
        clearRedo: function () { return roosterjs_editor_dom_1.clearProceedingSnapshots(snapshots); },
        canUndoAutoComplete: function () { return roosterjs_editor_dom_1.canUndoAutoComplete(snapshots); },
    };
}


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/corePlugins/createCorePlugins.ts":
/*!*****************************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/corePlugins/createCorePlugins.ts ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CopyPastePlugin_1 = __webpack_require__(/*! ./CopyPastePlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/CopyPastePlugin.ts");
var DOMEventPlugin_1 = __webpack_require__(/*! ./DOMEventPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/DOMEventPlugin.ts");
var EditPlugin_1 = __webpack_require__(/*! ./EditPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/EditPlugin.ts");
var EntityPlugin_1 = __webpack_require__(/*! ./EntityPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/EntityPlugin.ts");
var LifecyclePlugin_1 = __webpack_require__(/*! ./LifecyclePlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/LifecyclePlugin.ts");
var MouseUpPlugin_1 = __webpack_require__(/*! ./MouseUpPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/MouseUpPlugin.ts");
var PendingFormatStatePlugin_1 = __webpack_require__(/*! ./PendingFormatStatePlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/PendingFormatStatePlugin.ts");
var TypeAfterLinkPlugin_1 = __webpack_require__(/*! ./TypeAfterLinkPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/TypeAfterLinkPlugin.ts");
var TypeInContainerPlugin_1 = __webpack_require__(/*! ./TypeInContainerPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/TypeInContainerPlugin.ts");
var UndoPlugin_1 = __webpack_require__(/*! ./UndoPlugin */ "./packages/roosterjs-editor-core/lib/corePlugins/UndoPlugin.ts");
/**
 * @internal
 */
exports.PLACEHOLDER_PLUGIN_NAME = '_placeholder';
/**
 * @internal
 * Create Core Plugins
 * @param contentDiv Content DIV of editor
 * @param options Editor options
 */
function createCorePlugins(contentDiv, options) {
    var map = options.corePluginOverride || {};
    // The order matters, some plugin needs to be put before/after others to make sure event
    // can be handled in right order
    return {
        typeInContainer: map.typeInContainer || new TypeInContainerPlugin_1.default(),
        edit: map.edit || new EditPlugin_1.default(),
        _placeholder: null,
        typeAfterLink: map.typeAfterLink || new TypeAfterLinkPlugin_1.default(),
        undo: map.undo || new UndoPlugin_1.default(options),
        domEvent: map.domEvent || new DOMEventPlugin_1.default(options, contentDiv),
        pendingFormatState: map.pendingFormatState || new PendingFormatStatePlugin_1.default(),
        mouseUp: map.mouseUp || new MouseUpPlugin_1.default(),
        copyPaste: map.copyPaste || new CopyPastePlugin_1.default(options),
        entity: map.entity || new EntityPlugin_1.default(),
        lifecycle: map.lifecycle || new LifecyclePlugin_1.default(options, contentDiv),
    };
}
exports.default = createCorePlugins;
/**
 * @internal
 * Get plugin state of core plugins
 * @param corePlugins CorePlugins object
 */
function getPluginState(corePlugins) {
    return {
        domEvent: corePlugins.domEvent.getState(),
        pendingFormatState: corePlugins.pendingFormatState.getState(),
        edit: corePlugins.edit.getState(),
        lifecycle: corePlugins.lifecycle.getState(),
        undo: corePlugins.undo.getState(),
        entity: corePlugins.entity.getState(),
        copyPaste: corePlugins.copyPaste.getState(),
    };
}
exports.getPluginState = getPluginState;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/editor/Editor.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/editor/Editor.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var coreApiMap_1 = __webpack_require__(/*! ../coreApi/coreApiMap */ "./packages/roosterjs-editor-core/lib/coreApi/coreApiMap.ts");
var createCorePlugins_1 = __webpack_require__(/*! ../corePlugins/createCorePlugins */ "./packages/roosterjs-editor-core/lib/corePlugins/createCorePlugins.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * RoosterJs core editor class
 */
var Editor = /** @class */ (function () {
    //#region Lifecycle
    /**
     * Creates an instance of Editor
     * @param contentDiv The DIV HTML element which will be the container element of editor
     * @param options An optional options object to customize the editor
     */
    function Editor(contentDiv, options) {
        var _this = this;
        if (options === void 0) { options = {}; }
        // 1. Make sure all parameters are valid
        if (roosterjs_editor_dom_1.getTagOfNode(contentDiv) != 'DIV') {
            throw new Error('contentDiv must be an HTML DIV element');
        }
        // 2. Store options values to local variables
        var corePlugins = createCorePlugins_1.default(contentDiv, options);
        var plugins = [];
        Object.keys(corePlugins).forEach(function (name) {
            if (name == createCorePlugins_1.PLACEHOLDER_PLUGIN_NAME) {
                roosterjs_editor_dom_1.arrayPush(plugins, options.plugins);
            }
            else {
                plugins.push(corePlugins[name]);
            }
        });
        this.core = __assign({ contentDiv: contentDiv, api: __assign(__assign({}, coreApiMap_1.coreApiMap), (options.coreApiOverride || {})), plugins: plugins.filter(function (x) { return !!x; }) }, createCorePlugins_1.getPluginState(corePlugins));
        // 3. Initialize plugins
        this.core.plugins.forEach(function (plugin) { return plugin.initialize(_this); });
        // 4. Ensure user will type in a container node, not the editor content DIV
        this.ensureTypeInContainer(new roosterjs_editor_dom_1.Position(this.core.contentDiv, 0 /* Begin */).normalize());
    }
    /**
     * Dispose this editor, dispose all plugins and custom data
     */
    Editor.prototype.dispose = function () {
        this.core.plugins.reverse().forEach(function (plugin) { return plugin.dispose(); });
        this.core = null;
    };
    /**
     * Get whether this editor is disposed
     * @returns True if editor is disposed, otherwise false
     */
    Editor.prototype.isDisposed = function () {
        return !this.core;
    };
    //#endregion
    //#region Node API
    /**
     * Insert node into editor
     * @param node The node to insert
     * @param option Insert options. Default value is:
     *  position: ContentPosition.SelectionStart
     *  updateCursor: true
     *  replaceSelection: true
     *  insertOnNewLine: false
     * @returns true if node is inserted. Otherwise false
     */
    Editor.prototype.insertNode = function (node, option) {
        return node ? this.core.api.insertNode(this.core, node, option) : false;
    };
    /**
     * Delete a node from editor content
     * @param node The node to delete
     * @returns true if node is deleted. Otherwise false
     */
    Editor.prototype.deleteNode = function (node) {
        // Only remove the node when it falls within editor
        if (node && this.contains(node)) {
            node.parentNode.removeChild(node);
            return true;
        }
        return false;
    };
    /**
     * Replace a node in editor content with another node
     * @param existingNode The existing node to be replaced
     * @param toNode node to replace to
     * @param transformColorForDarkMode (optional) Whether to transform new node to dark mode. Default is false
     * @returns true if node is replaced. Otherwise false
     */
    Editor.prototype.replaceNode = function (existingNode, toNode, transformColorForDarkMode) {
        // Only replace the node when it falls within editor
        if (this.contains(existingNode) && toNode) {
            this.core.api.transformColor(this.core, transformColorForDarkMode ? toNode : null, true /*includeSelf*/, function () { return existingNode.parentNode.replaceChild(toNode, existingNode); }, 0 /* LightToDark */);
            return true;
        }
        return false;
    };
    /**
     * Get BlockElement at given node
     * @param node The node to create InlineElement
     * @returns The BlockElement result
     */
    Editor.prototype.getBlockElementAtNode = function (node) {
        return roosterjs_editor_dom_1.getBlockElementAtNode(this.core.contentDiv, node);
    };
    Editor.prototype.contains = function (arg) {
        return roosterjs_editor_dom_1.contains(this.core.contentDiv, arg);
    };
    Editor.prototype.queryElements = function (selector, scopeOrCallback, callback) {
        if (scopeOrCallback === void 0) { scopeOrCallback = 0 /* Body */; }
        var scope = scopeOrCallback instanceof Function ? 0 /* Body */ : scopeOrCallback;
        callback = scopeOrCallback instanceof Function ? scopeOrCallback : callback;
        var range = scope == 0 /* Body */ ? null : this.getSelectionRange();
        return roosterjs_editor_dom_1.queryElements(this.core.contentDiv, selector, callback, scope, range);
    };
    /**
     * Collapse nodes within the given start and end nodes to their common ascenstor node,
     * split parent nodes if necessary
     * @param start The start node
     * @param end The end node
     * @param canSplitParent True to allow split parent node there are nodes before start or after end under the same parent
     * and the returned nodes will be all nodes from start trhough end after splitting
     * False to disallow split parent
     * @returns When cansplitParent is true, returns all node from start through end after splitting,
     * otherwise just return start and end
     */
    Editor.prototype.collapseNodes = function (start, end, canSplitParent) {
        return roosterjs_editor_dom_1.collapseNodes(this.core.contentDiv, start, end, canSplitParent);
    };
    //#endregion
    //#region Content API
    /**
     * Check whether the editor contains any visible content
     * @param trim Whether trime the content string before check. Default is false
     * @returns True if there's no visible content, otherwise false
     */
    Editor.prototype.isEmpty = function (trim) {
        return roosterjs_editor_dom_1.isNodeEmpty(this.core.contentDiv, trim);
    };
    /**
     * Get current editor content as HTML string
     * @param mode specify what kind of HTML content to retrieve
     * @returns HTML string representing current editor content
     */
    Editor.prototype.getContent = function (mode) {
        if (mode === void 0) { mode = 0 /* CleanHTML */; }
        return this.core.api.getContent(this.core, mode);
    };
    /**
     * Set HTML content to this editor. All existing content will be replaced. A ContentChanged event will be triggered
     * @param content HTML content to set in
     * @param triggerContentChangedEvent True to trigger a ContentChanged event. Default value is true
     */
    Editor.prototype.setContent = function (content, triggerContentChangedEvent) {
        if (triggerContentChangedEvent === void 0) { triggerContentChangedEvent = true; }
        this.core.api.setContent(this.core, content, triggerContentChangedEvent);
    };
    /**
     * Insert HTML content into editor
     * @param HTML content to insert
     * @param option Insert options. Default value is:
     *  position: ContentPosition.SelectionStart
     *  updateCursor: true
     *  replaceSelection: true
     *  insertOnNewLine: false
     */
    Editor.prototype.insertContent = function (content, option) {
        if (content) {
            var doc = this.getDocument();
            var allNodes = roosterjs_editor_dom_1.fromHtml(content, doc);
            // If it is to insert on new line, and there are more than one node in the collection, wrap all nodes with
            // a parent DIV before calling insertNode on each top level sub node. Otherwise, every sub node may get wrapped
            // separately to show up on its own line
            if (option && option.insertOnNewLine && allNodes.length > 1) {
                allNodes = [roosterjs_editor_dom_1.wrap(allNodes)];
            }
            var fragment_1 = doc.createDocumentFragment();
            allNodes.forEach(function (node) { return fragment_1.appendChild(node); });
            this.insertNode(fragment_1, option);
        }
    };
    /**
     * Delete selected content
     */
    Editor.prototype.deleteSelectedContent = function () {
        var range = this.getSelectionRange();
        return range && !range.collapsed && roosterjs_editor_dom_1.deleteSelectedContent(this.core.contentDiv, range);
    };
    /**
     * Paste into editor using a clipboardData object
     * @param clipboardData Clipboard data retrieved from clipboard
     * @param pasteAsText Force pasting as plain text. Default value is false
     * @param applyCurrentStyle True if apply format of current selection to the pasted content,
     * false to keep original foramt.  Default value is false. When pasteAsText is true, this parameter is ignored
     */
    Editor.prototype.paste = function (clipboardData, pasteAsText, applyCurrentFormat) {
        var _this = this;
        if (!clipboardData) {
            return;
        }
        if (clipboardData.snapshotBeforePaste) {
            // Restore original content before paste a new one
            this.setContent(clipboardData.snapshotBeforePaste);
        }
        else {
            clipboardData.snapshotBeforePaste = this.getContent(2 /* RawHTMLWithSelection */);
        }
        var range = this.getSelectionRange();
        var pos = range && roosterjs_editor_dom_1.Position.getStart(range);
        var fragment = this.core.api.createPasteFragment(this.core, clipboardData, pos, pasteAsText, applyCurrentFormat);
        this.addUndoSnapshot(function () {
            _this.insertNode(fragment);
            return clipboardData;
        }, "Paste" /* Paste */);
    };
    //#endregion
    //#region Focus and Selection
    /**
     * Get current selection range from Editor.
     * It does a live pull on the selection, if nothing retrieved, return whatever we have in cache.
     * @param tryGetFromCache Set to true to retrieve the selection range from cache if editor doesn't own the focus now.
     * Default value is true
     * @returns current selection range, or null if editor never got focus before
     */
    Editor.prototype.getSelectionRange = function (tryGetFromCache) {
        if (tryGetFromCache === void 0) { tryGetFromCache = true; }
        return this.core.api.getSelectionRange(this.core, tryGetFromCache);
    };
    /**
     * Get current selection in a serializable format
     * It does a live pull on the selection, if nothing retrieved, return whatever we have in cache.
     * @returns current selection path, or null if editor never got focus before
     */
    Editor.prototype.getSelectionPath = function () {
        var range = this.getSelectionRange();
        return range && roosterjs_editor_dom_1.getSelectionPath(this.core.contentDiv, range);
    };
    /**
     * Check if focus is in editor now
     * @returns true if focus is in editor, otherwise false
     */
    Editor.prototype.hasFocus = function () {
        return this.core.api.hasFocus(this.core);
    };
    /**
     * Focus to this editor, the selection was restored to where it was before, no unexpected scroll.
     */
    Editor.prototype.focus = function () {
        this.core.api.focus(this.core);
    };
    Editor.prototype.select = function (arg1, arg2, arg3, arg4) {
        var range = !arg1
            ? null
            : roosterjs_editor_dom_1.safeInstanceOf(arg1, 'Range')
                ? arg1
                : Array.isArray(arg1.start) && Array.isArray(arg1.end)
                    ? roosterjs_editor_dom_1.createRange(this.core.contentDiv, arg1.start, arg1.end)
                    : roosterjs_editor_dom_1.createRange(arg1, arg2, arg3, arg4);
        return this.contains(range) && this.core.api.selectRange(this.core, range);
    };
    /**
     * Get current focused position. Return null if editor doesn't have focus at this time.
     */
    Editor.prototype.getFocusedPosition = function () {
        var _a;
        var sel = (_a = this.getDocument().defaultView) === null || _a === void 0 ? void 0 : _a.getSelection();
        if (this.contains(sel && sel.focusNode)) {
            return new roosterjs_editor_dom_1.Position(sel.focusNode, sel.focusOffset);
        }
        var range = this.getSelectionRange();
        if (range) {
            return roosterjs_editor_dom_1.Position.getStart(range);
        }
        return null;
    };
    /**
     * Get an HTML element from current cursor position.
     * When expectedTags is not specified, return value is the current node (if it is HTML element)
     * or its parent node (if current node is a Text node).
     * When expectedTags is specified, return value is the first anscestor of current node which has
     * one of the expected tags.
     * If no element found within editor by the given tag, return null.
     * @param selector Optional, an HTML selector to find HTML element with.
     * @param startFrom Start search from this node. If not specified, start from current focused position
     * @param event Optional, if specified, editor will try to get cached result from the event object first.
     * If it is not cached before, query from DOM and cache the result into the event object
     */
    Editor.prototype.getElementAtCursor = function (selector, startFrom, event) {
        var _this = this;
        event = startFrom ? null : event; // Only use cache when startFrom is not specified, for different start position can have different result
        return roosterjs_editor_dom_1.cacheGetEventData(event, 'GET_ELEMENT_AT_CURSOR_' + selector, function () {
            if (!startFrom) {
                var position = _this.getFocusedPosition();
                startFrom = position && position.node;
            }
            return (startFrom && roosterjs_editor_dom_1.findClosestElementAncestor(startFrom, _this.core.contentDiv, selector));
        });
    };
    /**
     * Check if this position is at beginning of the editor.
     * This will return true if all nodes between the beginning of target node and the position are empty.
     * @param position The position to check
     * @returns True if position is at beginning of the editor, otherwise false
     */
    Editor.prototype.isPositionAtBeginning = function (position) {
        return roosterjs_editor_dom_1.isPositionAtBeginningOf(position, this.core.contentDiv);
    };
    /**
     * Get impacted regions from selection
     */
    Editor.prototype.getSelectedRegions = function (type) {
        if (type === void 0) { type = 0 /* Table */; }
        var range = this.getSelectionRange();
        return range ? roosterjs_editor_dom_1.getRegionsFromRange(this.core.contentDiv, range, type) : [];
    };
    //#endregion
    //#region EVENT API
    Editor.prototype.addDomEventHandler = function (nameOrMap, handler) {
        var _a;
        var eventsToMap = typeof nameOrMap == 'string' ? (_a = {}, _a[nameOrMap] = handler, _a) : nameOrMap;
        return this.core.api.attachDomEvent(this.core, eventsToMap);
    };
    /**
     * Trigger an event to be dispatched to all plugins
     * @param eventType Type of the event
     * @param data data of the event with given type, this is the rest part of PluginEvent with the given type
     * @param broadcast indicates if the event needs to be dispatched to all plugins
     * True means to all, false means to allow exclusive handling from one plugin unless no one wants that
     * @returns the event object which is really passed into plugins. Some plugin may modify the event object so
     * the result of this function provides a chance to read the modified result
     */
    Editor.prototype.triggerPluginEvent = function (eventType, data, broadcast) {
        var event = __assign({ eventType: eventType }, data);
        this.core.api.triggerEvent(this.core, event, broadcast);
        return event;
    };
    /**
     * Trigger a ContentChangedEvent
     * @param source Source of this event, by default is 'SetContent'
     * @param data additional data for this event
     */
    Editor.prototype.triggerContentChangedEvent = function (source, data) {
        if (source === void 0) { source = "SetContent" /* SetContent */; }
        this.triggerPluginEvent(7 /* ContentChanged */, {
            source: source,
            data: data,
        });
    };
    //#endregion
    //#region Undo API
    /**
     * Undo last edit operation
     */
    Editor.prototype.undo = function () {
        this.focus();
        this.core.api.restoreUndoSnapshot(this.core, -1 /*step*/);
    };
    /**
     * Redo next edit operation
     */
    Editor.prototype.redo = function () {
        this.focus();
        this.core.api.restoreUndoSnapshot(this.core, 1 /*step*/);
    };
    /**
     * Add undo snapshot, and execute a format callback function, then add another undo snapshot, then trigger
     * ContentChangedEvent with given change source.
     * If this function is called nested, undo snapshot will only be added in the outside one
     * @param callback The callback function to perform formatting, returns a data object which will be used as
     * the data field in ContentChangedEvent if changeSource is not null.
     * @param changeSource The change source to use when fire ContentChangedEvent. When the value is not null,
     * a ContentChangedEvent will be fired with change source equal to this value
     * @param canUndoByBackspace True if this action can be undone when user press Backspace key (aka Auto Complelte).
     */
    Editor.prototype.addUndoSnapshot = function (callback, changeSource, canUndoByBackspace) {
        this.core.api.addUndoSnapshot(this.core, callback, changeSource, canUndoByBackspace);
    };
    /**
     * Whether there is an available undo/redo snapshot
     */
    Editor.prototype.getUndoState = function () {
        var _a = this.core.undo, hasNewContent = _a.hasNewContent, snapshotsService = _a.snapshotsService;
        return {
            canUndo: hasNewContent || snapshotsService.canMove(-1 /*previousSnapshot*/),
            canRedo: snapshotsService.canMove(1 /*nextSnapshot*/),
        };
    };
    //#endregion
    //#region Misc
    /**
     * Get document which contains this editor
     * @returns The HTML document which contains this editor
     */
    Editor.prototype.getDocument = function () {
        return this.core.contentDiv.ownerDocument;
    };
    /**
     * Get the scroll container of the editor
     */
    Editor.prototype.getScrollContainer = function () {
        return this.core.domEvent.scrollContainer;
    };
    /**
     * Get custom data related to this editor
     * @param key Key of the custom data
     * @param getter Getter function. If custom data for the given key doesn't exist,
     * call this function to get one and store it if it is specified. Otherwise return undefined
     * @param disposer An optional disposer function to dispose this custom data when
     * dispose editor.
     */
    Editor.prototype.getCustomData = function (key, getter, disposer) {
        return (this.core.lifecycle.customData[key] = this.core.lifecycle.customData[key] || {
            value: getter ? getter() : undefined,
            disposer: disposer,
        }).value;
    };
    /**
     * Check if editor is in IME input sequence
     * @returns True if editor is in IME input sequence, otherwise false
     */
    Editor.prototype.isInIME = function () {
        return this.core.domEvent.isInIME;
    };
    /**
     * Get default format of this editor
     * @returns Default format object of this editor
     */
    Editor.prototype.getDefaultFormat = function () {
        return this.core.lifecycle.defaultFormat;
    };
    /**
     * Get a content traverser for the whole editor
     * @param startNode The node to start from. If not passed, it will start from the beginning of the body
     */
    Editor.prototype.getBodyTraverser = function (startNode) {
        return roosterjs_editor_dom_1.ContentTraverser.createBodyTraverser(this.core.contentDiv, startNode);
    };
    /**
     * Get a content traverser for current selection
     */
    Editor.prototype.getSelectionTraverser = function () {
        var range = this.getSelectionRange();
        return (range &&
            roosterjs_editor_dom_1.ContentTraverser.createSelectionTraverser(this.core.contentDiv, this.getSelectionRange()));
    };
    /**
     * Get a content traverser for current block element start from specified position
     * @param startFrom Start position of the traverser. Default value is ContentPosition.SelectionStart
     */
    Editor.prototype.getBlockTraverser = function (startFrom) {
        if (startFrom === void 0) { startFrom = 3 /* SelectionStart */; }
        var range = this.getSelectionRange();
        return (range && roosterjs_editor_dom_1.ContentTraverser.createBlockTraverser(this.core.contentDiv, range, startFrom));
    };
    /**
     * Get a text traverser of current selection
     * @param event Optional, if specified, editor will try to get cached result from the event object first.
     * If it is not cached before, query from DOM and cache the result into the event object
     */
    Editor.prototype.getContentSearcherOfCursor = function (event) {
        var _this = this;
        return roosterjs_editor_dom_1.cacheGetEventData(event, 'CONTENTSEARCHER', function () {
            var range = _this.getSelectionRange();
            return (range && new roosterjs_editor_dom_1.PositionContentSearcher(_this.core.contentDiv, roosterjs_editor_dom_1.Position.getStart(range)));
        });
    };
    /**
     * Run a callback function asynchronously
     * @param callback The callback function to run
     */
    Editor.prototype.runAsync = function (callback) {
        var _this = this;
        var win = this.core.contentDiv.ownerDocument.defaultView || window;
        win.requestAnimationFrame(function () {
            if (!_this.isDisposed() && callback) {
                callback(_this);
            }
        });
    };
    /**
     * Set DOM attribute of editor content DIV
     * @param name Name of the attribute
     * @param value Value of the attribute
     */
    Editor.prototype.setEditorDomAttribute = function (name, value) {
        if (value === null) {
            this.core.contentDiv.removeAttribute(name);
        }
        else {
            this.core.contentDiv.setAttribute(name, value);
        }
    };
    /**
     * get DOM attribute of editor content DIV
     * @param name Name of the attribute
     */
    Editor.prototype.getEditorDomAttribute = function (name) {
        return this.core.contentDiv.getAttribute(name);
    };
    /**
     * Add a Content Edit feature.
     * @param feature The feature to add
     */
    Editor.prototype.addContentEditFeature = function (feature) {
        var _this = this;
        feature === null || feature === void 0 ? void 0 : feature.keys.forEach(function (key) {
            var array = _this.core.edit.features[key] || [];
            array.push(feature);
            _this.core.edit.features[key] = array;
        });
    };
    /**
     * Get style based format state from current selection, including font name/size and colors
     */
    Editor.prototype.getStyleBasedFormatState = function (node) {
        if (!node) {
            var range = this.getSelectionRange();
            node = range && roosterjs_editor_dom_1.Position.getStart(range).normalize().node;
        }
        return this.core.api.getStyleBasedFormatState(this.core, node);
    };
    /**
     * Ensure user will type into a container element rather than into the editor content DIV directly
     * @param position The position that user is about to type to
     * @param keyboardEvent Optional keyboard event object
     */
    Editor.prototype.ensureTypeInContainer = function (position, keyboardEvent) {
        this.core.api.ensureTypeInContainer(this.core, position, keyboardEvent);
    };
    //#endregion
    //#region Dark mode APIs
    /**
     * Set the dark mode state and transforms the content to match the new state.
     * @param nextDarkMode The next status of dark mode. True if the editor should be in dark mode, false if not.
     */
    Editor.prototype.setDarkModeState = function (nextDarkMode) {
        if (this.isDarkMode() == nextDarkMode) {
            return;
        }
        var currentContent = this.getContent(0 /* CleanHTML */);
        this.triggerContentChangedEvent(nextDarkMode ? "SwitchToDarkMode" /* SwitchToDarkMode */ : "SwitchToLightMode" /* SwitchToLightMode */);
        this.setContent(currentContent);
    };
    /**
     * Check if the editor is in dark mode
     * @returns True if the editor is in dark mode, otherwise false
     */
    Editor.prototype.isDarkMode = function () {
        return this.core.lifecycle.isDarkMode;
    };
    /**
     * Make the editor in "Shadow Edit" mode.
     * In Shadow Edit mode, all format change will finally be ignored.
     * This can be used for building a live preview feature for format button, to allow user
     * see format result without really apply it.
     * This function can be called repeatly. If editor is already in shadow edit mode, we can still
     * use this function to do more shadow edit operation.
     */
    Editor.prototype.startShadowEdit = function () {
        this.core.api.switchShadowEdit(this.core, true /*isOn*/);
    };
    /**
     * Leave "Shadow Edit" mode, all changes made during shadow edit will be discarded
     */
    Editor.prototype.stopShadowEdit = function () {
        this.core.api.switchShadowEdit(this.core, false /*isOn*/);
    };
    /**
     * Check if editor is in Shadow Edit mode
     */
    Editor.prototype.isInShadowEdit = function () {
        return !!this.core.lifecycle.shadowEditFragment;
    };
    /**
     * Check if the given experimental feature is enabled
     * @param feature The feature to check
     */
    Editor.prototype.isFeatureEnabled = function (feature) {
        return this.core.lifecycle.experimentalFeatures.indexOf(feature) >= 0;
    };
    return Editor;
}());
exports.default = Editor;


/***/ }),

/***/ "./packages/roosterjs-editor-core/lib/index.ts":
/*!*****************************************************!*\
  !*** ./packages/roosterjs-editor-core/lib/index.ts ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
// Classes
var Editor_1 = __webpack_require__(/*! ./editor/Editor */ "./packages/roosterjs-editor-core/lib/editor/Editor.ts");
exports.Editor = Editor_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/blockElements/NodeBlockElement.ts":
/*!*****************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/blockElements/NodeBlockElement.ts ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var isNodeAfter_1 = __webpack_require__(/*! ../utils/isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
/**
 * @internal
 * This presents a content block that can be reprented by a single html block type element.
 * In most cases, it corresponds to an HTML block level element, i.e. P, DIV, LI, TD etc.
 */
var NodeBlockElement = /** @class */ (function () {
    function NodeBlockElement(element) {
        this.element = element;
    }
    /**
     * Collapse this element to a single DOM element.
     * If the content nodes are separated in different root nodes, wrap them to a single node
     * If the content nodes are included in root node with other nodes, split root node
     */
    NodeBlockElement.prototype.collapseToSingleElement = function () {
        return this.element;
    };
    /**
     * Get the start node of the block
     * For NodeBlockElement, start and end essentially refers to same node
     */
    NodeBlockElement.prototype.getStartNode = function () {
        return this.element;
    };
    /**
     * Get the end node of the block
     * For NodeBlockElement, start and end essentially refers to same node
     */
    NodeBlockElement.prototype.getEndNode = function () {
        return this.element;
    };
    /**
     * Checks if it refers to same block
     */
    NodeBlockElement.prototype.equals = function (blockElement) {
        // Ideally there is only one unique way to generate a block so we only need to compare the startNode
        return this.element == blockElement.getStartNode();
    };
    /**
     * Checks if a block is after the current block
     */
    NodeBlockElement.prototype.isAfter = function (blockElement) {
        // if the block's startNode is after current node endEnd, we say it is after
        return isNodeAfter_1.default(this.element, blockElement.getEndNode());
    };
    /**
     * Checks if a certain html node is within the block
     */
    NodeBlockElement.prototype.contains = function (node) {
        return contains_1.default(this.element, node, true /*treatSameNodeAsContain*/);
    };
    /**
     * Get the text content of this block element
     */
    NodeBlockElement.prototype.getTextContent = function () {
        return this.element ? this.element.textContent : '';
    };
    return NodeBlockElement;
}());
exports.default = NodeBlockElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/blockElements/StartEndBlockElement.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/blockElements/StartEndBlockElement.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var collapseNodes_1 = __webpack_require__(/*! ../utils/collapseNodes */ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts");
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var createRange_1 = __webpack_require__(/*! ../selection/createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var isBlockElement_1 = __webpack_require__(/*! ../utils/isBlockElement */ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts");
var isNodeAfter_1 = __webpack_require__(/*! ../utils/isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
var wrap_1 = __webpack_require__(/*! ../utils/wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
var splitParentNode_1 = __webpack_require__(/*! ../utils/splitParentNode */ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts");
var STRUCTURE_NODE_TAGS = ['TD', 'TH', 'LI', 'BLOCKQUOTE'];
/**
 * @internal
 * This reprents a block that is identified by a start and end node
 * This is for cases like &lt;root&gt;Hello&lt;BR&gt;World&lt;/root&gt;
 * in that case, Hello&lt;BR&gt; is a block, World is another block
 * Such block cannot be represented by a NodeBlockElement since they don't chained up
 * to a single parent node, instead they have a start and end
 * This start and end must be in same sibling level and have same parent in DOM tree
 */
var StartEndBlockElement = /** @class */ (function () {
    function StartEndBlockElement(rootNode, startNode, endNode) {
        this.rootNode = rootNode;
        this.startNode = startNode;
        this.endNode = endNode;
    }
    StartEndBlockElement.getBlockContext = function (node) {
        while (node && !isBlockElement_1.default(node)) {
            node = node.parentNode;
        }
        return node;
    };
    /**
     * Collapse this element to a single DOM element.
     * If the content nodes are separated in different root nodes, wrap them to a single node
     * If the content nodes are included in root node with other nodes, split root node
     */
    StartEndBlockElement.prototype.collapseToSingleElement = function () {
        var nodes = collapseNodes_1.default(StartEndBlockElement.getBlockContext(this.startNode), this.startNode, this.endNode, true /*canSplitParent*/);
        var blockContext = StartEndBlockElement.getBlockContext(this.startNode);
        while (nodes[0] &&
            nodes[0] != blockContext &&
            nodes[0].parentNode != this.rootNode &&
            STRUCTURE_NODE_TAGS.indexOf(getTagOfNode_1.default(nodes[0].parentNode)) < 0) {
            nodes = [splitParentNode_1.splitBalancedNodeRange(nodes)];
        }
        return nodes.length == 1 && isBlockElement_1.default(nodes[0])
            ? nodes[0]
            : wrap_1.default(nodes);
    };
    /**
     * Gets the start node
     */
    StartEndBlockElement.prototype.getStartNode = function () {
        return this.startNode;
    };
    /**
     * Gets the end node
     */
    StartEndBlockElement.prototype.getEndNode = function () {
        return this.endNode;
    };
    /**
     * Checks equals of two blocks
     */
    StartEndBlockElement.prototype.equals = function (blockElement) {
        return (this.startNode == blockElement.getStartNode() &&
            this.endNode == blockElement.getEndNode());
    };
    /**
     * Checks if another block is after this current
     */
    StartEndBlockElement.prototype.isAfter = function (blockElement) {
        return isNodeAfter_1.default(this.getStartNode(), blockElement.getEndNode());
    };
    /**
     * Checks if an Html node is contained within the block
     */
    StartEndBlockElement.prototype.contains = function (node) {
        return (contains_1.default(this.startNode, node, true /*treatSameNodeAsContain*/) ||
            contains_1.default(this.endNode, node, true /*treatSameNodeAsContain*/) ||
            (isNodeAfter_1.default(node, this.startNode) && isNodeAfter_1.default(this.endNode, node)));
    };
    /**
     * Get the text content of this block element
     */
    StartEndBlockElement.prototype.getTextContent = function () {
        var range = createRange_1.default(this.getStartNode(), this.getEndNode());
        return range ? range.toString() : '';
    };
    return StartEndBlockElement;
}());
exports.default = StartEndBlockElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var collapseNodes_1 = __webpack_require__(/*! ../utils/collapseNodes */ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts");
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var isBlockElement_1 = __webpack_require__(/*! ../utils/isBlockElement */ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts");
var NodeBlockElement_1 = __webpack_require__(/*! ./NodeBlockElement */ "./packages/roosterjs-editor-dom/lib/blockElements/NodeBlockElement.ts");
var StartEndBlockElement_1 = __webpack_require__(/*! ./StartEndBlockElement */ "./packages/roosterjs-editor-dom/lib/blockElements/StartEndBlockElement.ts");
/**
 * This produces a block element from a a node
 * It needs to account for various HTML structure. Examples:
 * 1) &lt;root&gt;&lt;div&gt;abc&lt;/div&gt;&lt;/root&gt;
 *   This is most common the case, user passes in a node pointing to abc, and get back a block representing &lt;div&gt;abc&lt;/div&gt;
 * 2) &lt;root&gt;&lt;p&gt;&lt;br&gt;&lt;/p&gt;&lt;/root&gt;
 *   Common content for empty block, user passes node pointing to &lt;br&gt;, and get back a block representing &lt;p&gt;&lt;br&gt;&lt;/p&gt;
 * 3) &lt;root&gt;abc&lt;/root&gt;
 *   Not common, but does happen. It is still a block in user's view. User passes in abc, and get back a start-end block representing abc
 *   NOTE: abc could be just one node. However, since it is not a html block, it is more appropriate to use start-end block although they point to same node
 * 4) &lt;root&gt;&lt;div&gt;abc&lt;br&gt;123&lt;/div&gt;&lt;/root&gt;
 *   A bit tricky, but can happen when user use Ctrl+Enter which simply inserts a &lt;BR&gt; to create a link break. There're two blocks:
 *   block1: 1) abc&lt;br&gt; block2: 123
 * 5) &lt;root&gt;&lt;div&gt;abc&lt;div&gt;123&lt;/div&gt;&lt;/div&gt;&lt;/root&gt;
 *   Nesting div and there is text node in same level as a DIV. Two blocks: 1) abc 2) &lt;div&gt;123&lt;/div&gt;
 * 6) &lt;root&gt;&lt;div&gt;abc&lt;span&gt;123&lt;br&gt;456&lt;/span&gt;&lt;/div&gt;&lt;/root&gt;
 *   This is really tricky. Essentially there is a &lt;BR&gt; in middle of a span breaking the span into two blocks;
 *   block1: abc&lt;span&gt;123&lt;br&gt; block2: 456
 * In summary, given any arbitary node (leaf), to identify the head and tail of the block, following rules need to be followed:
 * 1) to identify the head, it needs to crawl DOM tre left/up till a block node or BR is encountered
 * 2) same for identifying tail
 * 3) should also apply a block ceiling, meaning as it crawls up, it should stop at a block node
 * @param rootNode Root node of the scope, the block element will be inside of this node
 * @param node The node to get BlockElement start from
 */
function getBlockElementAtNode(rootNode, node) {
    if (!contains_1.default(rootNode, node)) {
        return null;
    }
    // Identify the containing block. This serves as ceiling for traversing down below
    // NOTE: this container block could be just the rootNode,
    // which cannot be used to create block element. We will special case handle it later on
    var containerBlockNode = StartEndBlockElement_1.default.getBlockContext(node);
    if (containerBlockNode == node) {
        return new NodeBlockElement_1.default(containerBlockNode);
    }
    // Find the head and leaf node in the block
    var headNode = findHeadTailLeafNode(node, containerBlockNode, false /*isTail*/);
    var tailNode = findHeadTailLeafNode(node, containerBlockNode, true /*isTail*/);
    // At this point, we have the head and tail of a block, here are some examples and where head and tail point to
    // 1) &lt;root&gt;&lt;div&gt;hello&lt;br&gt;&lt;/div&gt;&lt;/root&gt;, head: hello, tail: &lt;br&gt;
    // 2) &lt;root&gt;&lt;div&gt;hello&lt;span style="font-family: Arial"&gt;world&lt;/span&gt;&lt;/div&gt;&lt;/root&gt;, head: hello, tail: world
    // Both are actually completely and exclusively wrapped in a parent div, and can be represented with a Node block
    // So we shall try to collapse as much as we can to the nearest common ancester
    var nodes = collapseNodes_1.default(rootNode, headNode, tailNode, false /*canSplitParent*/);
    headNode = nodes[0];
    tailNode = nodes[nodes.length - 1];
    if (headNode.parentNode != tailNode.parentNode) {
        // Un-Balanced start and end, create a start-end block
        return new StartEndBlockElement_1.default(rootNode, headNode, tailNode);
    }
    else {
        // Balanced start and end (point to same parent), need to see if further collapsing can be done
        while (!headNode.previousSibling && !tailNode.nextSibling) {
            var parentNode = headNode.parentNode;
            if (parentNode == containerBlockNode) {
                // Has reached the container block
                if (containerBlockNode != rootNode) {
                    // If the container block is not the root, use the container block
                    headNode = tailNode = parentNode;
                }
                break;
            }
            else if (parentNode != rootNode) {
                // Continue collapsing to parent
                headNode = tailNode = parentNode;
            }
            else {
                break;
            }
        }
        // If head and tail are same and it is a block element, create NodeBlock, otherwise start-end block
        return headNode == tailNode && isBlockElement_1.default(headNode)
            ? new NodeBlockElement_1.default(headNode)
            : new StartEndBlockElement_1.default(rootNode, headNode, tailNode);
    }
}
exports.default = getBlockElementAtNode;
/**
 * Given a node and container block, identify the first/last leaf node
 * A leaf node is defined as deepest first/last node in a block
 * i.e. &lt;div&gt;&lt;span style="font-family: Arial"&gt;abc&lt;/span&gt;&lt;/div&gt;, abc is the head leaf of the block
 * Often &lt;br&gt; or a child &lt;div&gt; is used to create a block. In that case, the leaf after the sibling div or br should be the head leaf
 * i.e. &lt;div&gt;123&lt;br&gt;abc&lt;/div&gt;, abc is the head of a block because of a previous sibling &lt;br&gt;
 * i.e. &lt;div&gt;&lt;div&gt;123&lt;/div&gt;abc&lt;/div&gt;, abc is also the head of a block because of a previous sibling &lt;div&gt;
 */
function findHeadTailLeafNode(node, containerBlockNode, isTail) {
    var result = node;
    if (getTagOfNode_1.default(result) == 'BR' && isTail) {
        return result;
    }
    while (result) {
        var sibling = node;
        while (!(sibling = isTail ? node.nextSibling : node.previousSibling)) {
            node = node.parentNode;
            if (node == containerBlockNode) {
                return result;
            }
        }
        while (sibling) {
            if (isBlockElement_1.default(sibling)) {
                return result;
            }
            else if (getTagOfNode_1.default(sibling) == 'BR') {
                return isTail ? sibling : result;
            }
            node = sibling;
            sibling = isTail ? node.firstChild : node.lastChild;
        }
        result = node;
    }
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/blockElements/getFirstLastBlockElement.ts":
/*!*************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/blockElements/getFirstLastBlockElement.ts ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getBlockElementAtNode_1 = __webpack_require__(/*! ./getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
/**
 * Get the first/last BlockElement of under the root node.
 * If no suitable BlockElement found, returns null
 * @param rootNode The root node to get BlockElement from
 * @param isFirst True to get first BlockElement, false to get last BlockElement
 */
function getFirstLastBlockElement(rootNode, isFirst) {
    var node = rootNode;
    do {
        node = node && (isFirst ? node.firstChild : node.lastChild);
    } while (node && node.firstChild);
    return node && getBlockElementAtNode_1.default(rootNode, node);
}
exports.default = getFirstLastBlockElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/contentTraverser/BodyScoper.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/contentTraverser/BodyScoper.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getFirstLastBlockElement_1 = __webpack_require__(/*! ../blockElements/getFirstLastBlockElement */ "./packages/roosterjs-editor-dom/lib/blockElements/getFirstLastBlockElement.ts");
var getInlineElementAtNode_1 = __webpack_require__(/*! ../inlineElements/getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
var getFirstLastInlineElement_1 = __webpack_require__(/*! ../inlineElements/getFirstLastInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/getFirstLastInlineElement.ts");
/**
 * @internal
 * provides scoper for traversing the entire editor body starting from the beginning
 */
var BodyScoper = /** @class */ (function () {
    /**
     * Construct a new instance of BodyScoper class
     * @param rootNode Root node of the body
     * @param startNode The node to start from. If not passed, it will start from the beginning of the body
     */
    function BodyScoper(rootNode, startNode) {
        this.rootNode = rootNode;
        this.startNode = contains_1.default(rootNode, startNode) ? startNode : null;
    }
    /**
     * Get the start block element
     */
    BodyScoper.prototype.getStartBlockElement = function () {
        return this.startNode
            ? getBlockElementAtNode_1.default(this.rootNode, this.startNode)
            : getFirstLastBlockElement_1.default(this.rootNode, true /*isFirst*/);
    };
    /**
     * Get the start inline element
     */
    BodyScoper.prototype.getStartInlineElement = function () {
        return this.startNode
            ? getInlineElementAtNode_1.default(this.rootNode, this.startNode)
            : getFirstLastInlineElement_1.getFirstInlineElement(this.rootNode);
    };
    /**
     * Since the scope is global, all blocks under the root node are in scope
     */
    BodyScoper.prototype.isBlockInScope = function (blockElement) {
        return contains_1.default(this.rootNode, blockElement.getStartNode());
    };
    /**
     * Since we're at body scope, inline elements never need to be trimmed
     */
    BodyScoper.prototype.trimInlineElement = function (inlineElement) {
        return inlineElement;
    };
    return BodyScoper;
}());
exports.default = BodyScoper;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts":
/*!********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var BodyScoper_1 = __webpack_require__(/*! ./BodyScoper */ "./packages/roosterjs-editor-dom/lib/contentTraverser/BodyScoper.ts");
var EmptyInlineElement_1 = __webpack_require__(/*! ../inlineElements/EmptyInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/EmptyInlineElement.ts");
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getInlineElementAtNode_1 = __webpack_require__(/*! ../inlineElements/getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
var PartialInlineElement_1 = __webpack_require__(/*! ../inlineElements/PartialInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts");
var SelectionBlockScoper_1 = __webpack_require__(/*! ./SelectionBlockScoper */ "./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionBlockScoper.ts");
var SelectionScoper_1 = __webpack_require__(/*! ./SelectionScoper */ "./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionScoper.ts");
var getInlineElementBeforeAfter_1 = __webpack_require__(/*! ../inlineElements/getInlineElementBeforeAfter */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementBeforeAfter.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * The provides traversing of content inside editor.
 * There are two ways to traverse, block by block, or inline element by inline element
 * Block and inline traversing is independent from each other, meanning if you traverse block by block, it does not change
 * the current inline element position
 */
var ContentTraverser = /** @class */ (function () {
    /**
     * Create a content traverser for the whole body of given root node
     * @param scoper Traversing scoper object to help scope the traversing
     * @param skipTags (Optional) tags that child elements will be skipped
     */
    function ContentTraverser(scoper, skipTags) {
        this.scoper = scoper;
        this.skipTags = skipTags;
    }
    /**
     * Create a content traverser for the whole body of given root node
     * @param rootNode The root node to traverse in
     * @param startNode The node to start from. If not passed, it will start from the beginning of the body
     * @param skipTags (Optional) tags that child elements will be skipped
     */
    ContentTraverser.createBodyTraverser = function (rootNode, startNode, skipTags) {
        return new ContentTraverser(new BodyScoper_1.default(rootNode, startNode));
    };
    /**
     * Create a content traverser for the given selection
     * @param rootNode The root node to traverse in
     * @param range The selection range to scope the traversing
     * @param skipTags (Optional) tags that child elements will be skipped
     */
    ContentTraverser.createSelectionTraverser = function (rootNode, range, skipTags) {
        return new ContentTraverser(new SelectionScoper_1.default(rootNode, range), skipTags);
    };
    /**
     * Create a content traverser for a block element which contains the given position
     * @param rootNode The root node to traverse in
     * @param position A position inside a block, traversing will be scoped within this block.
     * If passing a range, the start position of this range will be used
     * @param startFrom Start position of traversing. The value can be Begin, End, SelectionStart
     * @param skipTags (Optional) tags that child elements will be skipped
     */
    ContentTraverser.createBlockTraverser = function (rootNode, position, start, skipTags) {
        if (start === void 0) { start = 3 /* SelectionStart */; }
        return new ContentTraverser(new SelectionBlockScoper_1.default(rootNode, position, start));
    };
    Object.defineProperty(ContentTraverser.prototype, "currentBlockElement", {
        /**
         * Get current block
         */
        get: function () {
            // Prepare currentBlock from the scoper
            if (!this.currentBlock) {
                this.currentBlock = this.scoper.getStartBlockElement();
            }
            return this.currentBlock;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * Get next block element
     */
    ContentTraverser.prototype.getNextBlockElement = function () {
        return this.getPreviousNextBlockElement(true /*isNext*/);
    };
    /**
     * Get previous block element
     */
    ContentTraverser.prototype.getPreviousBlockElement = function () {
        return this.getPreviousNextBlockElement(false /*isNext*/);
    };
    ContentTraverser.prototype.getPreviousNextBlockElement = function (isNext) {
        var current = this.currentBlockElement;
        if (!current) {
            return null;
        }
        var leaf = getLeafSibling_1.getLeafSibling(this.scoper.rootNode, isNext ? current.getEndNode() : current.getStartNode(), isNext, this.skipTags);
        var newBlock = leaf ? getBlockElementAtNode_1.default(this.scoper.rootNode, leaf) : null;
        // Make sure this is right block:
        // 1) the block is in scope per scoper
        // 2) the block is after (for next) or before (for previous) the current block
        // Then:
        // 1) Re-position current block to newly found block
        if (newBlock &&
            this.scoper.isBlockInScope(newBlock) &&
            ((isNext && newBlock.isAfter(current)) || (!isNext && current.isAfter(newBlock)))) {
            this.currentBlock = newBlock;
            return this.currentBlock;
        }
        return null;
    };
    Object.defineProperty(ContentTraverser.prototype, "currentInlineElement", {
        /**
         * Current inline element getter
         */
        get: function () {
            // Retrieve a start inline from scoper
            if (!this.currentInline) {
                this.currentInline = this.scoper.getStartInlineElement();
            }
            return this.currentInline instanceof EmptyInlineElement_1.default ? null : this.currentInline;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * Get next inline element
     */
    ContentTraverser.prototype.getNextInlineElement = function () {
        return this.getPreviousNextInlineElement(true /*isNext*/);
    };
    /**
     * Get previous inline element
     */
    ContentTraverser.prototype.getPreviousInlineElement = function () {
        return this.getPreviousNextInlineElement(false /*isNext*/);
    };
    ContentTraverser.prototype.getPreviousNextInlineElement = function (isNext) {
        var current = this.currentInlineElement || this.currentInline;
        var newInline;
        if (!current) {
            return null;
        }
        if (current instanceof EmptyInlineElement_1.default) {
            newInline = getInlineElementBeforeAfter_1.getInlineElementBeforeAfter(this.scoper.rootNode, current.getStartPosition(), isNext);
            if (newInline && !current.getParentBlock().contains(newInline.getContainerNode())) {
                newInline = null;
            }
        }
        else {
            newInline = getNextPreviousInlineElement(this.scoper.rootNode, current, isNext);
            newInline =
                newInline &&
                    current &&
                    ((isNext && newInline.isAfter(current)) || (!isNext && current.isAfter(newInline)))
                    ? newInline
                    : null;
        }
        // For inline, we need to make sure:
        // 1) it is really next/previous to current
        // 2) pass on the new inline to this.scoper to do the triming and we still get back an inline
        // Then
        // 1) re-position current inline
        if (newInline && (newInline = this.scoper.trimInlineElement(newInline))) {
            this.currentInline = newInline;
            return this.currentInline;
        }
        return null;
    };
    return ContentTraverser;
}());
exports.default = ContentTraverser;
function getNextPreviousInlineElement(rootNode, current, isNext) {
    if (!current) {
        return null;
    }
    if (current instanceof PartialInlineElement_1.default) {
        // if current is partial, get the the othe half of the inline unless it is no more
        var result = isNext ? current.nextInlineElement : current.previousInlineElement;
        if (result) {
            return result;
        }
    }
    // Get a leaf node after startNode and use that base to find next inline
    var startNode = current.getContainerNode();
    startNode = getLeafSibling_1.getLeafSibling(rootNode, startNode, isNext);
    return getInlineElementAtNode_1.default(rootNode, startNode);
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/contentTraverser/PositionContentSearcher.ts":
/*!***************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/contentTraverser/PositionContentSearcher.ts ***!
  \***************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContentTraverser_1 = __webpack_require__(/*! ./ContentTraverser */ "./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts");
var createRange_1 = __webpack_require__(/*! ../selection/createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
// White space matching regex. It matches following chars:
// \s: white space
// \u00A0: no-breaking white space
// \u200B: zero width space
// \u3000: full width space (which can come from JPN IME)
var WHITESPACE_REGEX = /[\s\u00A0\u200B\u3000]+([^\s\u00A0\u200B\u3000]*)$/i;
/**
 * The class that helps search content around a position
 */
var PositionContentSearcher = /** @class */ (function () {
    /**
     * Create a new CursorData instance
     * @param rootNode Root node of the whole scope
     * @param position Start position
     */
    function PositionContentSearcher(rootNode, position) {
        this.rootNode = rootNode;
        this.position = position;
        // The cached text before position that has been read so far
        this.text = '';
        // All inline elements before position that have been read so far
        this.inlineElements = [];
    }
    /**
     * Get the word before position. The word is determined by scanning backwards till the first white space, the portion
     * between position and the white space is the word before position
     * @returns The word before position
     */
    PositionContentSearcher.prototype.getWordBefore = function () {
        var _this = this;
        if (!this.word) {
            this.traverse(function () { return _this.word; });
        }
        return this.word;
    };
    /**
     * Get the inline element before position
     * @returns The inlineElement before position
     */
    PositionContentSearcher.prototype.getInlineElementBefore = function () {
        if (!this.inlineBefore) {
            this.traverse(null);
        }
        return this.inlineBefore;
    };
    /**
     * Get the inline element after position
     * @returns The inline element after position
     */
    PositionContentSearcher.prototype.getInlineElementAfter = function () {
        if (!this.inlineAfter) {
            this.inlineAfter = ContentTraverser_1.default.createBlockTraverser(this.rootNode, this.position).currentInlineElement;
        }
        return this.inlineAfter;
    };
    /**
     * Get X number of chars before position
     * The actual returned chars may be less than what is requested.
     * @param length The length of string user want to get, the string always ends at the position,
     * so this length determins the start position of the string
     * @returns The actual string we get as a sub string, or the whole string before position when
     * there is not enough chars in the string
     */
    PositionContentSearcher.prototype.getSubStringBefore = function (length) {
        var _this = this;
        if (this.text.length < length) {
            this.traverse(function () { return _this.text.length >= length; });
        }
        return this.text.substr(Math.max(0, this.text.length - length));
    };
    /**
     * Try to get a range matches the given text before the position
     * @param text The text to match against
     * @param exactMatch Whether it is an exact match
     * @returns The range for the matched text, null if unable to find a match
     */
    PositionContentSearcher.prototype.getRangeFromText = function (text, exactMatch) {
        if (!text) {
            return null;
        }
        var startPosition;
        var endPosition;
        var textIndex = text.length - 1;
        this.forEachTextInlineElement(function (textInline) {
            var nodeContent = textInline.getTextContent() || '';
            var nodeIndex = nodeContent.length - 1;
            for (; nodeIndex >= 0 && textIndex >= 0; nodeIndex--) {
                if (text.charCodeAt(textIndex) == nodeContent.charCodeAt(nodeIndex)) {
                    textIndex--;
                    // on first time when end is matched, set the end of range
                    if (!endPosition) {
                        endPosition = textInline.getStartPosition().move(nodeIndex + 1);
                    }
                }
                else if (exactMatch || endPosition) {
                    // Mismatch found when exact match or end already match, so return since matching failed
                    return true;
                }
            }
            // when textIndex == -1, we have a successful complete match
            if (textIndex == -1) {
                startPosition = textInline.getStartPosition().move(nodeIndex + 1);
                return true;
            }
            return false;
        });
        return startPosition && endPosition && createRange_1.default(startPosition, endPosition);
    };
    /**
     * Get text section before position till stop condition is met.
     * This offers consumers to retrieve text section by section
     * The section essentially is just an inline element which has Container element
     * so that the consumer can remember it for anchoring popup or verification purpose
     * when position moves out of context etc.
     * @param stopFunc The callback stop function
     */
    PositionContentSearcher.prototype.forEachTextInlineElement = function (callback) {
        // We cache all text sections read so far
        // Every time when you ask for textSection, we start with the cached first
        // and resort to further reading once we exhausted with the cache
        if (!this.inlineElements.some(callback)) {
            this.traverse(callback);
        }
    };
    /**
     * Get first non textual inline element before position
     * @returns First non textutal inline element before position or null if no such element exists
     */
    PositionContentSearcher.prototype.getNearestNonTextInlineElement = function () {
        var _this = this;
        if (!this.nearestNonTextInlineElement) {
            this.traverse(function () { return _this.nearestNonTextInlineElement; });
        }
        return this.nearestNonTextInlineElement;
    };
    /**
     * Continue traversing backward till stop condition is met or begin of block is reached
     */
    PositionContentSearcher.prototype.traverse = function (callback) {
        this.traverser =
            this.traverser || ContentTraverser_1.default.createBlockTraverser(this.rootNode, this.position);
        if (!this.traverser || this.traversingComplete) {
            return;
        }
        var previousInline = this.traverser.getPreviousInlineElement();
        while (!this.traversingComplete) {
            this.inlineBefore = this.inlineBefore || previousInline;
            if (previousInline && previousInline.isTextualInlineElement()) {
                var textContent = previousInline.getTextContent();
                // build the word before position if it is not built yet
                if (!this.word) {
                    // Match on the white space, the portion after space is on the index of 1 of the matched result
                    // (index at 0 is whole match result, index at 1 is the word)
                    var matches = WHITESPACE_REGEX.exec(textContent);
                    if (matches && matches.length == 2) {
                        this.word = matches[1] + this.text;
                    }
                }
                this.text = textContent + this.text;
                this.inlineElements.push(previousInline);
                // Check if stop condition is met
                if (callback && callback(previousInline)) {
                    break;
                }
            }
            else {
                this.nearestNonTextInlineElement = previousInline;
                this.traversingComplete = true;
                if (!this.word) {
                    // if parsing is done, whatever we get so far in this.cachedText should also be in this.cachedWordBeforeCursor
                    this.word = this.text;
                }
                // When a non-textual inline element, or null is seen, we consider parsing complete
                // TODO: we may need to change this if there is a future need to parse beyond text, i.e.
                // we have aaa @someone bbb<position>, and we want to read the text before @someone
                break;
            }
            previousInline = this.traverser.getPreviousInlineElement();
        }
    };
    return PositionContentSearcher;
}());
exports.default = PositionContentSearcher;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionBlockScoper.ts":
/*!************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionBlockScoper.ts ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var EmptyInlineElement_1 = __webpack_require__(/*! ../inlineElements/EmptyInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/EmptyInlineElement.ts");
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getInlineElementAtNode_1 = __webpack_require__(/*! ../inlineElements/getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
var NodeBlockElement_1 = __webpack_require__(/*! ../blockElements/NodeBlockElement */ "./packages/roosterjs-editor-dom/lib/blockElements/NodeBlockElement.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var getInlineElementBeforeAfter_1 = __webpack_require__(/*! ../inlineElements/getInlineElementBeforeAfter */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementBeforeAfter.ts");
var getFirstLastInlineElement_1 = __webpack_require__(/*! ../inlineElements/getFirstLastInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/getFirstLastInlineElement.ts");
/**
 * @internal
 * This provides traversing content in a selection start block
 * This is commonly used for those cursor context sensitive plugin,
 * they want to know text being typed at cursor
 * This provides a scope for parsing from cursor position up to begin of the selection block
 */
var SelectionBlockScoper = /** @class */ (function () {
    /**
     * Create a new instance of SelectionBlockScoper class
     * @param rootNode The root node of the whole scope
     * @param position Position of the selection start
     * @param startFrom Where to start, can be Begin, End, SelectionStart
     */
    function SelectionBlockScoper(rootNode, position, startFrom) {
        this.rootNode = rootNode;
        this.startFrom = startFrom;
        position = safeInstanceOf_1.default(position, 'Range') ? Position_1.default.getStart(position) : position;
        this.position = position.normalize();
        this.block = getBlockElementAtNode_1.default(this.rootNode, this.position.node);
    }
    /**
     * Get the start block element
     */
    SelectionBlockScoper.prototype.getStartBlockElement = function () {
        return this.block;
    };
    /**
     * Get the start inline element
     * The start inline refers to inline before the selection start
     *  The reason why we choose the one before rather after is, when cursor is at the end of a paragragh,
     * the one after likely will point to inline in next paragragh which may be null if the cursor is at bottom of editor
     */
    SelectionBlockScoper.prototype.getStartInlineElement = function () {
        if (this.block) {
            switch (this.startFrom) {
                case 0 /* Begin */:
                case 1 /* End */:
                case 2 /* DomEnd */:
                    return getFirstLastInlineElementFromBlockElement(this.block, this.startFrom == 0 /* Begin */);
                case 3 /* SelectionStart */:
                    // Get the inline before selection start point, and ensure it falls in the selection block
                    var startInline = getInlineElementBeforeAfter_1.getInlineElementAfter(this.rootNode, this.position);
                    return startInline && this.block.contains(startInline.getContainerNode())
                        ? startInline
                        : new EmptyInlineElement_1.default(this.position, this.block);
            }
        }
        return null;
    };
    /**
     * Check if the given block element is in current scope
     * @param blockElement The block element to check
     */
    SelectionBlockScoper.prototype.isBlockInScope = function (blockElement) {
        return this.block && blockElement ? this.block.equals(blockElement) : false;
    };
    /**
     * Trim the incoming inline element, and return an inline element
     * This just tests and return the inline element if it is in block
     * This is a block scoper, which is not like selection scoper where it may cut an inline element in half
     * A block scoper does not cut an inline in half
     */
    SelectionBlockScoper.prototype.trimInlineElement = function (inlineElement) {
        return this.block && inlineElement && this.block.contains(inlineElement.getContainerNode())
            ? inlineElement
            : null;
    };
    return SelectionBlockScoper;
}());
exports.default = SelectionBlockScoper;
/**
 * Get first/last InlineElement of the given BlockElement
 * @param block The BlockElement to get InlineElement from
 * @param isFirst True to get first InlineElement, false to get last InlineElement
 */
function getFirstLastInlineElementFromBlockElement(block, isFirst) {
    if (block instanceof NodeBlockElement_1.default) {
        var blockNode = block.getStartNode();
        return isFirst ? getFirstLastInlineElement_1.getFirstInlineElement(blockNode) : getFirstLastInlineElement_1.getLastInlineElement(blockNode);
    }
    else {
        return getInlineElementAtNode_1.default(block, isFirst ? block.getStartNode() : block.getEndNode());
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionScoper.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/contentTraverser/SelectionScoper.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var PartialInlineElement_1 = __webpack_require__(/*! ../inlineElements/PartialInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var getInlineElementBeforeAfter_1 = __webpack_require__(/*! ../inlineElements/getInlineElementBeforeAfter */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementBeforeAfter.ts");
/**
 * @internal
 * This is selection scoper that provide a start inline as the start of the selection
 * and checks if a block falls in the selection (isBlockInScope)
 * last trimInlineElement to trim any inline element to return a partial that falls in the selection
 */
var SelectionScoper = /** @class */ (function () {
    /**
     * Create a new instance of SelectionScoper class
     * @param rootNode The root node of the content
     * @param range The selection range to scope to
     */
    function SelectionScoper(rootNode, range) {
        this.rootNode = rootNode;
        this.start = Position_1.default.getStart(range).normalize();
        this.end = Position_1.default.getEnd(range).normalize();
    }
    /**
     * Provide a start block as the first block after the cursor
     */
    SelectionScoper.prototype.getStartBlockElement = function () {
        if (!this.startBlock) {
            this.startBlock = getBlockElementAtNode_1.default(this.rootNode, this.start.node);
        }
        return this.startBlock;
    };
    /**
     * Provide a start inline as the first inline after the cursor
     */
    SelectionScoper.prototype.getStartInlineElement = function () {
        if (!this.startInline) {
            this.startInline = this.trimInlineElement(getInlineElementBeforeAfter_1.getInlineElementAfter(this.rootNode, this.start));
        }
        return this.startInline;
    };
    /**
     * Checks if a block completely falls in the selection
     * @param block The BlockElement to check
     */
    SelectionScoper.prototype.isBlockInScope = function (block) {
        if (!block) {
            return false;
        }
        var inScope = false;
        var selStartBlock = this.getStartBlockElement();
        if (this.start.equalTo(this.end)) {
            inScope = selStartBlock && selStartBlock.equals(block);
        }
        else {
            var selEndBlock = getBlockElementAtNode_1.default(this.rootNode, this.end.node);
            // There are three cases that are considered as "block in scope"
            // 1) The start of selection falls on the block
            // 2) The end of selection falls on the block
            // 3) the block falls in-between selection start and end
            inScope =
                selStartBlock &&
                    selEndBlock &&
                    (block.equals(selStartBlock) ||
                        block.equals(selEndBlock) ||
                        (block.isAfter(selStartBlock) && selEndBlock.isAfter(block)));
        }
        return inScope;
    };
    /**
     * Trim an incoming inline. If it falls completely outside selection, return null
     * otherwise return a partial that represents the portion that falls in the selection
     * @param inline The InlineElement to check
     */
    SelectionScoper.prototype.trimInlineElement = function (inline) {
        if (!inline || this.start.equalTo(this.end)) {
            return null;
        }
        // Temp code. Will be changed to using InlineElement.getStart/EndPosition() soon
        var start = inline.getStartPosition();
        var end = inline.getEndPosition();
        if (start.isAfter(this.end) || this.start.isAfter(end)) {
            return null;
        }
        var startPartial = false;
        var endPartial = false;
        if (this.start.isAfter(start)) {
            start = this.start;
            startPartial = true;
        }
        if (end.isAfter(this.end)) {
            end = this.end;
            endPartial = true;
        }
        return start.isAfter(end) || start.equalTo(end)
            ? null
            : startPartial || endPartial
                ? new PartialInlineElement_1.default(inline, startPartial && start, endPartial && end)
                : inline;
    };
    return SelectionScoper;
}());
exports.default = SelectionScoper;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/entity/commitEntity.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/entity/commitEntity.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CONTENT_EDITABLE = 'contenteditable';
/**
 * Commit information of an entity (type, isReadonly, id) into the wrapper node as CSS Classes
 * @param wrapper The entity wrapper element
 * @param type Entity type
 * @param isReadonly Whether this is a readonly entity
 * @param id Optional Id of the entity
 */
function commitEntity(wrapper, type, isReadonly, id) {
    if (wrapper) {
        wrapper.className = "_Entity" /* ENTITY_INFO_NAME */ + " " + "_EType_" /* ENTITY_TYPE_PREFIX */ + type + " " + (id ? "" + "_EId_" /* ENTITY_ID_PREFIX */ + id + " " : '') + "_EReadonly_" /* ENTITY_READONLY_PREFIX */ + (isReadonly ? '1' : '0');
        if (isReadonly) {
            wrapper.contentEditable = 'false';
        }
        else if (wrapper.getAttribute(CONTENT_EDITABLE)) {
            wrapper.removeAttribute(CONTENT_EDITABLE);
        }
    }
}
exports.default = commitEntity;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/entity/getEntityFromElement.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/entity/getEntityFromElement.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Get Entity object from an entity root element
 * @param element The entity root element. If this element is not an entity root element,
 * it will return null
 */
function getEntityFromElement(element) {
    var _a;
    var isEntity = false;
    var type;
    var id = '';
    var isReadonly = false;
    (_a = element === null || element === void 0 ? void 0 : element.className) === null || _a === void 0 ? void 0 : _a.split(' ').forEach(function (name) {
        if (name == "_Entity" /* ENTITY_INFO_NAME */) {
            isEntity = true;
        }
        else if (name.indexOf("_EType_" /* ENTITY_TYPE_PREFIX */) == 0) {
            type = name.substr("_EType_" /* ENTITY_TYPE_PREFIX */.length);
        }
        else if (name.indexOf("_EId_" /* ENTITY_ID_PREFIX */) == 0) {
            id = name.substr("_EId_" /* ENTITY_ID_PREFIX */.length);
        }
        else if (name.indexOf("_EReadonly_" /* ENTITY_READONLY_PREFIX */) == 0) {
            isReadonly = name.substr("_EReadonly_" /* ENTITY_READONLY_PREFIX */.length) == '1';
        }
    });
    return isEntity
        ? {
            wrapper: element,
            id: id,
            type: type,
            isReadonly: isReadonly,
        }
        : null;
}
exports.default = getEntityFromElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/entity/getEntitySelector.ts":
/*!***********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/entity/getEntitySelector.ts ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal Get a selector string for specified entity type and id
 * @param type (Optional) Type of entity
 * @param id (Optional) Id of entity
 */
function getEntitySelector(type, id) {
    var typeSelector = type ? "." + "_EType_" /* ENTITY_TYPE_PREFIX */ + type : '';
    var idSelector = id ? "." + "_EId_" /* ENTITY_ID_PREFIX */ + id : '';
    return '.' + "_Entity" /* ENTITY_INFO_NAME */ + typeSelector + idSelector;
}
exports.default = getEntitySelector;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/event/cacheGetEventData.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/event/cacheGetEventData.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Gets the cached event data by cache key from event object if there is already one.
 * Otherwise, call getter function to create one, and cache it.
 * @param event The event object
 * @param key Cache key string, need to be unique
 * @param getter Getter function to get the object when it is not in cache yet
 */
function cacheGetEventData(event, key, getter) {
    var result = event && event.eventDataCache && event.eventDataCache.hasOwnProperty(key)
        ? event.eventDataCache[key]
        : getter();
    if (event) {
        event.eventDataCache = event.eventDataCache || {};
        event.eventDataCache[key] = result;
    }
    return result;
}
exports.default = cacheGetEventData;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/event/clearEventDataCache.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/event/clearEventDataCache.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Clear a cached object by its key from an event object
 * @param event The event object
 * @param key The cache key
 */
function clearEventDataCache(event, key) {
    if (event && event.eventDataCache) {
        if (key && event.eventDataCache.hasOwnProperty(key)) {
            delete event.eventDataCache[key];
        }
        else if (!key) {
            event.eventDataCache = {};
        }
    }
}
exports.default = clearEventDataCache;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/event/isCharacterValue.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/event/isCharacterValue.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var isModifierKey_1 = __webpack_require__(/*! ./isModifierKey */ "./packages/roosterjs-editor-dom/lib/event/isModifierKey.ts");
/**
 * Returns true when the event was fired from a key that produces a character value, otherwise false
 * This detection is not 100% accurate. event.key is not fully supported by all browsers, and in some browsers (e.g. IE),
 * event.key is longer than 1 for num pad input. But here we just want to improve performance as much as possible.
 * So if we missed some case here it is still acceptable.
 * @param event The keyboard event object
 */
function isCharacterValue(event) {
    return !isModifierKey_1.default(event) && event.key && event.key.length == 1;
}
exports.default = isCharacterValue;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/event/isCtrlOrMetaPressed.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/event/isCtrlOrMetaPressed.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Browser_1 = __webpack_require__(/*! ../utils/Browser */ "./packages/roosterjs-editor-dom/lib/utils/Browser.ts");
/**
 * Check if Ctrl key (Windows) or Meta key (Mac) is pressed for the given Event
 * @param event A Keyboard event or Mouse event object
 * @returns True if Ctrl key is pressed on Windows or Meta key is pressed on Mac
 */
var isCtrlOrMetaPressed = Browser_1.Browser.isMac
    ? function (event) { return event.metaKey; }
    : function (event) { return event.ctrlKey; };
exports.default = isCtrlOrMetaPressed;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/event/isModifierKey.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/event/isModifierKey.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CTRL_CHARCODE = 'Control';
var ALT_CHARCODE = 'Alt';
var META_CHARCODE = 'Meta';
/**
 * Returns true when the event was fired from a modifier key, otherwise false
 * @param event The keyboard event object
 */
function isModifierKey(event) {
    var isCtrlKey = event.ctrlKey || event.key === CTRL_CHARCODE;
    var isAltKey = event.altKey || event.key === ALT_CHARCODE;
    var isMetaKey = event.metaKey || event.key === META_CHARCODE;
    return isCtrlKey || isAltKey || isMetaKey;
}
exports.default = isModifierKey;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/HtmlSanitizer.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/HtmlSanitizer.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var changeElementTag_1 = __webpack_require__(/*! ../utils/changeElementTag */ "./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts");
var getInheritableStyles_1 = __webpack_require__(/*! ./getInheritableStyles */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getInheritableStyles.ts");
var getPredefinedCssForElement_1 = __webpack_require__(/*! ./getPredefinedCssForElement */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getPredefinedCssForElement.ts");
var getStyles_1 = __webpack_require__(/*! ../style/getStyles */ "./packages/roosterjs-editor-dom/lib/style/getStyles.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var setStyles_1 = __webpack_require__(/*! ../style/setStyles */ "./packages/roosterjs-editor-dom/lib/style/setStyles.ts");
var toArray_1 = __webpack_require__(/*! ../utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
var cloneObject_1 = __webpack_require__(/*! ./cloneObject */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/cloneObject.ts");
var getAllowedValues_1 = __webpack_require__(/*! ./getAllowedValues */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getAllowedValues.ts");
/**
 * HTML sanitizer class provides two featuers:
 * 1. Convert global CSS to inline CSS
 * 2. Sanitize an HTML document, remove unnecessary/dangerous attribute/nodes
 */
var HtmlSanitizer = /** @class */ (function () {
    /**
     * Construct a new instance of HtmlSanitizer
     * @param options Options for HtmlSanitizer
     */
    function HtmlSanitizer(options) {
        options = options || {};
        this.elementCallbacks = cloneObject_1.cloneObject(options.elementCallbacks);
        this.styleCallbacks = getAllowedValues_1.getStyleCallbacks(options.cssStyleCallbacks);
        this.attributeCallbacks = cloneObject_1.cloneObject(options.attributeCallbacks);
        this.tagReplacements = getAllowedValues_1.getTagReplacement(options.additionalTagReplacements);
        this.allowedAttributes = getAllowedValues_1.getAllowedAttributes(options.additionalAllowedAttributes);
        this.allowedCssClassesRegex = getAllowedValues_1.getAllowedCssClassesRegex(options.additionalAllowedCssClasses);
        this.defaultStyleValues = getAllowedValues_1.getDefaultStyleValues(options.additionalDefaultStyleValues);
        this.additionalPredefinedCssForElement = options.additionalPredefinedCssForElement;
        this.additionalGlobalStyleNodes = options.additionalGlobalStyleNodes || [];
        this.unknownTagReplacement = options.unknownTagReplacement;
    }
    /**
     * Convert global CSS to inline CSS if any
     * @param html HTML source
     * @param additionalStyleNodes (Optional) additional HTML STYLE elements used as global CSS
     */
    HtmlSanitizer.convertInlineCss = function (html, additionalStyleNodes) {
        var sanitizer = new HtmlSanitizer({
            additionalGlobalStyleNodes: additionalStyleNodes,
        });
        return sanitizer.exec(html, true /*convertCssOnly*/);
    };
    /**
     * Sanitize HTML string, remove any unuseful HTML node/attribute/CSS.
     * @param html HTML source string
     * @param options Options used for this sanitizing process
     */
    HtmlSanitizer.sanitizeHtml = function (html, options) {
        options = options || {};
        var sanitizer = new HtmlSanitizer(options);
        var currentStyles = safeInstanceOf_1.default(options.currentElementOrStyle, 'HTMLElement')
            ? getInheritableStyles_1.default(options.currentElementOrStyle)
            : options.currentElementOrStyle;
        return sanitizer.exec(html, options.convertCssOnly, currentStyles);
    };
    /**
     * Sanitize HTML string
     * This function will do the following work:
     * 1. Convert global CSS into inline CSS
     * 2. Remove dangerous HTML tags and attributes
     * 3. Remove useless CSS properties
     * @param html The input HTML
     * @param convertInlineCssOnly Whether only convert inline css and skip html content sanitizing
     * @param currentStyles Current inheritable CSS styles
     */
    HtmlSanitizer.prototype.exec = function (html, convertCssOnly, currentStyles) {
        var parser = new DOMParser();
        var doc = parser.parseFromString(html || '', 'text/html');
        if (doc && doc.body && doc.body.firstChild) {
            this.convertGlobalCssToInlineCss(doc);
            if (!convertCssOnly) {
                this.sanitize(doc.body, currentStyles);
            }
        }
        return (doc && doc.body && doc.body.innerHTML) || '';
    };
    /**
     * Sanitize an HTML element, remove unnecessary or dangerous elements/attribute/CSS rules
     * @param rootNode Root node to sanitize
     * @param currentStyles Current CSS styles. Inheritable styles in the given node which has
     * the same value with current styles will be ignored.
     */
    HtmlSanitizer.prototype.sanitize = function (rootNode, currentStyles) {
        if (!rootNode) {
            return '';
        }
        currentStyles = cloneObject_1.cloneObject(currentStyles, getInheritableStyles_1.default(null));
        this.processNode(rootNode, currentStyles, {});
    };
    /**
     * Convert global CSS into inline CSS
     * @param rootNode The HTML Document
     */
    HtmlSanitizer.prototype.convertGlobalCssToInlineCss = function (rootNode) {
        var styleNodes = toArray_1.default(rootNode.querySelectorAll('style'));
        var styleSheets = this.additionalGlobalStyleNodes
            .reverse()
            .map(function (node) { return node.sheet; })
            .concat(styleNodes.map(function (node) { return node.sheet; }).reverse())
            .filter(function (sheet) { return sheet; });
        for (var _i = 0, styleSheets_1 = styleSheets; _i < styleSheets_1.length; _i++) {
            var styleSheet = styleSheets_1[_i];
            var _loop_1 = function (j) {
                // Skip any none-style rule, i.e. @page
                var styleRule = styleSheet.cssRules[j];
                var text = styleRule && styleRule.style ? styleRule.style.cssText : null;
                if (styleRule.type != CSSRule.STYLE_RULE || !text || !styleRule.selectorText) {
                    return "continue";
                }
                // Make sure the selector is not empty
                for (var _i = 0, _a = styleRule.selectorText.split(','); _i < _a.length; _i++) {
                    var selector = _a[_i];
                    if (!selector || !selector.trim() || selector.indexOf(':') >= 0) {
                        continue;
                    }
                    var nodes = toArray_1.default(rootNode.querySelectorAll(selector));
                    // Always put existing styles after so that they have higher priority
                    // Which means if both global style and inline style apply to the same element,
                    // inline style will have higher priority
                    nodes.forEach(function (node) {
                        return node.setAttribute('style', text + (node.getAttribute('style') || ''));
                    });
                }
            };
            for (var j = styleSheet.cssRules.length - 1; j >= 0; j--) {
                _loop_1(j);
            }
        }
        styleNodes.forEach(function (node) {
            if (node.parentNode) {
                node.parentNode.removeChild(node);
            }
        });
    };
    HtmlSanitizer.prototype.processNode = function (node, currentStyle, context) {
        var nodeType = node.nodeType;
        var isElement = nodeType == 1 /* Element */;
        var isText = nodeType == 3 /* Text */;
        var isFragment = nodeType == 11 /* DocumentFragment */;
        var shouldKeep = false;
        if (isElement) {
            var tag = getTagOfNode_1.default(node);
            var callback = this.elementCallbacks[tag];
            var replacement = this.tagReplacements[tag.toLowerCase()];
            if (replacement === undefined) {
                replacement = this.unknownTagReplacement;
            }
            if (callback) {
                shouldKeep = callback(node, context);
            }
            else if (tag.indexOf(':') > 0) {
                shouldKeep = true;
            }
            else if (tag == replacement || replacement == '*') {
                shouldKeep = true;
            }
            else if (replacement && /^[a-zA-Z][\w\-]*$/.test(replacement)) {
                node = changeElementTag_1.default(node, replacement);
                shouldKeep = true;
            }
        }
        else if (isText) {
            var whiteSpace = currentStyle['white-space'];
            shouldKeep =
                whiteSpace == 'pre' ||
                    whiteSpace == 'pre-line' ||
                    whiteSpace == 'pre-wrap' ||
                    !/^[\r\n]*$/g.test(node.nodeValue);
        }
        else if (isFragment) {
            shouldKeep = true;
        }
        else {
            shouldKeep = false;
        }
        if (!shouldKeep) {
            node.parentNode.removeChild(node);
        }
        else if (isText &&
            (currentStyle['white-space'] == 'pre' || currentStyle['white-space'] == 'pre-wrap')) {
            node.nodeValue = node.nodeValue.replace(/^ /gm, '\u00A0').replace(/ {2}/g, ' \u00A0');
        }
        else if (isElement || isFragment) {
            var thisStyle = cloneObject_1.cloneObject(currentStyle);
            var element = node;
            if (isElement) {
                this.processAttributes(element, context);
                this.preprocessCss(element, thisStyle);
                this.processCss(element, thisStyle, context);
            }
            var child = element.firstChild;
            var next = void 0;
            for (; child; child = next) {
                next = child.nextSibling;
                this.processNode(child, thisStyle, context);
            }
        }
    };
    HtmlSanitizer.prototype.preprocessCss = function (element, thisStyle) {
        var predefinedStyles = getPredefinedCssForElement_1.default(element, this.additionalPredefinedCssForElement);
        if (predefinedStyles) {
            Object.keys(predefinedStyles).forEach(function (name) {
                thisStyle[name] = predefinedStyles[name];
            });
        }
    };
    HtmlSanitizer.prototype.processCss = function (element, thisStyle, context) {
        var _this = this;
        var styles = getStyles_1.default(element);
        Object.keys(styles).forEach(function (name) {
            var value = styles[name];
            var callback = _this.styleCallbacks[name];
            var isInheritable = thisStyle[name] != undefined;
            var keep = (!callback || callback(value, element, thisStyle, context)) &&
                value != 'inherit' &&
                value.indexOf('expression') < 0 &&
                name.substr(0, 1) != '-' &&
                _this.defaultStyleValues[name] != value &&
                ((isInheritable && value != thisStyle[name]) ||
                    (!isInheritable && value != 'initial' && value != 'normal'));
            if (keep && isInheritable) {
                thisStyle[name] = value;
            }
            if (!keep) {
                delete styles[name];
            }
        });
        setStyles_1.default(element, styles);
    };
    HtmlSanitizer.prototype.processAttributes = function (element, context) {
        for (var i = element.attributes.length - 1; i >= 0; i--) {
            var attribute = element.attributes[i];
            var name_1 = attribute.name.toLowerCase().trim();
            var value = attribute.value;
            var callback = this.attributeCallbacks[name_1];
            var newValue = callback
                ? callback(value, element, context)
                : this.allowedAttributes.indexOf(name_1) >= 0 || name_1.indexOf('data-') == 0
                    ? value
                    : null;
            if (name_1 == 'class' && this.allowedCssClassesRegex) {
                newValue = this.processCssClass(value, newValue);
            }
            if (newValue === null ||
                newValue === undefined ||
                newValue.match(/s\n*c\n*r\n*i\n*p\n*t\n*:/i) // match script: with any NewLine inside. Browser will ignore those NewLine char and still treat it as script prefix
            ) {
                element.removeAttribute(name_1);
            }
            else {
                attribute.value = newValue;
            }
        }
    };
    HtmlSanitizer.prototype.processCssClass = function (originalValue, calculatedValue) {
        var _this = this;
        var originalClasses = originalValue ? originalValue.split(' ') : [];
        var calculatedClasses = calculatedValue ? calculatedValue.split(' ') : [];
        originalClasses.forEach(function (className) {
            if (_this.allowedCssClassesRegex.test(className) &&
                calculatedClasses.indexOf(className) < 0) {
                calculatedClasses.push(className);
            }
        });
        return calculatedClasses.length > 0 ? calculatedClasses.join(' ') : null;
    };
    return HtmlSanitizer;
}());
exports.default = HtmlSanitizer;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/chainSanitizerCallback.ts":
/*!***********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/chainSanitizerCallback.ts ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Chain all callback for an attribute together
 * @param map The source callback map
 * @param name Name of the property to chain
 * @param newCallback A new callback to process the given name on the given map.
 * If the same property got multiple callbacks, the final return value will be the return
 * value of the latest callback
 */
function chainSanitizerCallback(map, name, newCallback) {
    if (!map[name]) {
        map[name] = newCallback;
    }
    else {
        var originalCallback_1 = map[name];
        map[name] = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            originalCallback_1.apply(void 0, args);
            return newCallback.apply(void 0, args);
        };
    }
}
exports.default = chainSanitizerCallback;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/cloneObject.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/cloneObject.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
function nativeClone(source, existingObj) {
    return Object.assign(existingObj || {}, source);
}
function customClone(source, existingObj) {
    var result = existingObj || {};
    if (source) {
        for (var _i = 0, _a = Object.keys(source); _i < _a.length; _i++) {
            var key = _a[_i];
            result[key] = source[key];
        }
    }
    return result;
}
/**
 * @internal
 */
exports.cloneObject = Object.assign ? nativeClone : customClone;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/createDefaultHtmlSanitizerOptions.ts":
/*!**********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/createDefaultHtmlSanitizerOptions.ts ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Create default value of HtmlSanitizerOptions with every property set
 */
function createDefaultHtmlSanitizerOptions() {
    return {
        elementCallbacks: {},
        attributeCallbacks: {},
        cssStyleCallbacks: {},
        additionalTagReplacements: {},
        additionalAllowedAttributes: [],
        additionalAllowedCssClasses: [],
        additionalDefaultStyleValues: {},
        additionalGlobalStyleNodes: [],
        additionalPredefinedCssForElement: {},
        unknownTagReplacement: null,
    };
}
exports.default = createDefaultHtmlSanitizerOptions;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getAllowedValues.ts":
/*!*****************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/getAllowedValues.ts ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var cloneObject_1 = __webpack_require__(/*! ./cloneObject */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/cloneObject.ts");
var HTML_TAG_REPLACEMENT = {
    // Allowed tags
    a: '*',
    abbr: '*',
    address: '*',
    area: '*',
    article: '*',
    aside: '*',
    b: '*',
    bdi: '*',
    bdo: '*',
    blockquote: '*',
    body: '*',
    br: '*',
    button: '*',
    canvas: '*',
    caption: '*',
    center: '*',
    cite: '*',
    code: '*',
    col: '*',
    colgroup: '*',
    data: '*',
    datalist: '*',
    dd: '*',
    del: '*',
    details: '*',
    dfn: '*',
    dialog: '*',
    dir: '*',
    div: '*',
    dl: '*',
    dt: '*',
    em: '*',
    fieldset: '*',
    figcaption: '*',
    figure: '*',
    font: '*',
    footer: '*',
    h1: '*',
    h2: '*',
    h3: '*',
    h4: '*',
    h5: '*',
    h6: '*',
    head: '*',
    header: '*',
    hgroup: '*',
    hr: '*',
    html: '*',
    i: '*',
    img: '*',
    input: '*',
    ins: '*',
    kbd: '*',
    label: '*',
    legend: '*',
    li: '*',
    main: '*',
    map: '*',
    mark: '*',
    menu: '*',
    menuitem: '*',
    meter: '*',
    nav: '*',
    ol: '*',
    optgroup: '*',
    option: '*',
    output: '*',
    p: '*',
    picture: '*',
    pre: '*',
    progress: '*',
    q: '*',
    rp: '*',
    rt: '*',
    ruby: '*',
    s: '*',
    samp: '*',
    section: '*',
    select: '*',
    small: '*',
    span: '*',
    strike: '*',
    strong: '*',
    sub: '*',
    summary: '*',
    sup: '*',
    table: '*',
    tbody: '*',
    td: '*',
    template: '*',
    textarea: '*',
    tfoot: '*',
    th: '*',
    thead: '*',
    time: '*',
    tr: '*',
    tt: '*',
    u: '*',
    ul: '*',
    var: '*',
    wbr: '*',
    xmp: '*',
    // Replaced tags:
    form: 'SPAN',
    // Disallowed tags
    applet: null,
    audio: null,
    base: null,
    basefont: null,
    embed: null,
    frame: null,
    frameset: null,
    iframe: null,
    link: null,
    meta: null,
    noscript: null,
    object: null,
    param: null,
    script: null,
    slot: null,
    source: null,
    style: null,
    title: null,
    track: null,
    video: null,
};
var ALLOWED_HTML_ATTRIBUTES = ('accept,align,alt,checked,cite,color,cols,colspan,contextmenu,' +
    'coords,datetime,default,dir,dirname,disabled,download,face,headers,height,hidden,high,href,' +
    'hreflang,ismap,kind,label,lang,list,low,max,maxlength,media,min,multiple,open,optimum,pattern,' +
    'placeholder,readonly,rel,required,reversed,rows,rowspan,scope,selected,shape,size,sizes,span,' +
    'spellcheck,src,srclang,srcset,start,step,style,tabindex,target,title,translate,type,usemap,valign,value,' +
    'width,wrap').split(',');
var DEFAULT_STYLE_VALUES = {
    'background-color': 'transparent',
    'border-bottom-color': 'rgb(0, 0, 0)',
    'border-bottom-style': 'none',
    'border-bottom-width': '0px',
    'border-image-outset': '0',
    'border-image-repeat': 'stretch',
    'border-image-slice': '100%',
    'border-image-source': 'none',
    'border-image-width': '1',
    'border-left-color': 'rgb(0, 0, 0)',
    'border-left-style': 'none',
    'border-left-width': '0px',
    'border-right-color': 'rgb(0, 0, 0)',
    'border-right-style': 'none',
    'border-right-width': '0px',
    'border-top-color': 'rgb(0, 0, 0)',
    'border-top-style': 'none',
    'border-top-width': '0px',
    'outline-color': 'transparent',
    'outline-style': 'none',
    'outline-width': '0px',
    overflow: 'visible',
    'text-decoration': 'none',
    '-webkit-text-stroke-width': '0px',
    'word-wrap': 'break-word',
    'margin-left': '0px',
    'margin-right': '0px',
    padding: '0px',
    'padding-top': '0px',
    'padding-left': '0px',
    'padding-right': '0px',
    'padding-bottom': '0px',
    border: '0px',
    'border-top': '0px',
    'border-left': '0px',
    'border-right': '0px',
    'border-bottom': '0px',
    'vertical-align': 'baseline',
    float: 'none',
};
// This is to preserve entity related CSS classes when paste.
var ALLOWED_CSS_CLASSES = [];
/**
 * @internal
 */
function getTagReplacement(additionalReplacements) {
    var result = __assign({}, HTML_TAG_REPLACEMENT);
    var replacements = additionalReplacements || {};
    Object.keys(replacements).forEach(function (key) {
        if (key) {
            result[key.toLowerCase()] = replacements[key];
        }
    });
    return result;
}
exports.getTagReplacement = getTagReplacement;
/**
 * @internal
 */
function getAllowedAttributes(additionalAttributes) {
    return unique(ALLOWED_HTML_ATTRIBUTES.concat(additionalAttributes || [])).map(function (attr) {
        return attr.toLocaleLowerCase();
    });
}
exports.getAllowedAttributes = getAllowedAttributes;
/**
 * @internal
 */
function getAllowedCssClassesRegex(additionalCssClasses) {
    var patterns = ALLOWED_CSS_CLASSES.concat(additionalCssClasses || []);
    return patterns.length > 0 ? new RegExp(patterns.join('|')) : null;
}
exports.getAllowedCssClassesRegex = getAllowedCssClassesRegex;
/**
 * @internal
 */
function getDefaultStyleValues(additionalDefaultStyles) {
    var result = cloneObject_1.cloneObject(DEFAULT_STYLE_VALUES);
    if (additionalDefaultStyles) {
        Object.keys(additionalDefaultStyles).forEach(function (name) {
            var value = additionalDefaultStyles[name];
            if (value !== null && value !== undefined) {
                result[name] = value;
            }
            else {
                delete result[name];
            }
        });
    }
    return result;
}
exports.getDefaultStyleValues = getDefaultStyleValues;
/**
 * @internal
 */
function getStyleCallbacks(callbacks) {
    var result = cloneObject_1.cloneObject(callbacks);
    result.position = result.position || removeValue;
    result.width = result.width || removeWidthForLiAndDiv;
    return result;
}
exports.getStyleCallbacks = getStyleCallbacks;
function removeValue() {
    return null;
}
function removeWidthForLiAndDiv(value, element) {
    var tag = element.tagName;
    return !(tag == 'LI' || tag == 'DIV');
}
function unique(array) {
    return array.filter(function (value, index, self) { return self.indexOf(value) == index; });
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getInheritableStyles.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/getInheritableStyles.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
// Inheritable CSS properties
// Ref: https://www.w3.org/TR/CSS21/propidx.html
var INHERITABLE_PROPERTIES = ('border-spacing,caption-side,color,' +
    'cursor,direction,empty-cells,font-family,font-size,font-style,font-variant,font-weight,' +
    'font,letter-spacing,line-height,list-style-image,list-style-position,list-style-type,' +
    'list-style,orphans,quotes,text-align,text-indent,text-transform,visibility,white-space,' +
    'widows,word-spacing').split(',');
/**
 * Get inheritable CSS style values from the given element
 * @param element The element to get style from
 */
function getInheritableStyles(element) {
    var win = element && element.ownerDocument && element.ownerDocument.defaultView;
    var styles = win && win.getComputedStyle(element);
    var result = {};
    INHERITABLE_PROPERTIES.forEach(function (name) { return (result[name] = (styles && styles.getPropertyValue(name)) || ''); });
    return result;
}
exports.default = getInheritableStyles;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getPredefinedCssForElement.ts":
/*!***************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/htmlSanitizer/getPredefinedCssForElement.ts ***!
  \***************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var PREDEFINED_CSS_FOR_ELEMENT = {
    B: {
        'font-weight': 'bold',
    },
    EM: {
        'font-style': 'italic',
    },
    I: {
        'font-style': 'italic',
    },
    U: {
        'text-decoration': 'underline',
    },
    P: {
        'margin-top': '1em',
        'margin-bottom': '1em',
    },
    PRE: {
        'white-space': 'pre',
    },
    S: {
        'text-decoration': 'line-through',
    },
    STRIKE: {
        'text-decoration': 'line-through',
    },
    SUB: {
        'vertical-align': 'sub',
        'font-size': 'smaller',
    },
    SUP: {
        'vertical-align': 'super',
        'font-size': 'smaller',
    },
};
/**
 * @internal
 * Get a map for browser built-in CSS definations of elements
 */
function getPredefinedCssForElement(element, additionalPredefinedCssForElement) {
    var tag = getTagOfNode_1.default(element);
    return PREDEFINED_CSS_FOR_ELEMENT[tag] || (additionalPredefinedCssForElement || {})[tag];
}
exports.default = getPredefinedCssForElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/index.ts":
/*!****************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/index.ts ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getBlockElementAtNode_1 = __webpack_require__(/*! ./blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
exports.getBlockElementAtNode = getBlockElementAtNode_1.default;
var getFirstLastBlockElement_1 = __webpack_require__(/*! ./blockElements/getFirstLastBlockElement */ "./packages/roosterjs-editor-dom/lib/blockElements/getFirstLastBlockElement.ts");
exports.getFirstLastBlockElement = getFirstLastBlockElement_1.default;
var ContentTraverser_1 = __webpack_require__(/*! ./contentTraverser/ContentTraverser */ "./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts");
exports.ContentTraverser = ContentTraverser_1.default;
var PositionContentSearcher_1 = __webpack_require__(/*! ./contentTraverser/PositionContentSearcher */ "./packages/roosterjs-editor-dom/lib/contentTraverser/PositionContentSearcher.ts");
exports.PositionContentSearcher = PositionContentSearcher_1.default;
var getInlineElementAtNode_1 = __webpack_require__(/*! ./inlineElements/getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
exports.getInlineElementAtNode = getInlineElementAtNode_1.default;
var ImageInlineElement_1 = __webpack_require__(/*! ./inlineElements/ImageInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/ImageInlineElement.ts");
exports.ImageInlineElement = ImageInlineElement_1.default;
var LinkInlineElement_1 = __webpack_require__(/*! ./inlineElements/LinkInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/LinkInlineElement.ts");
exports.LinkInlineElement = LinkInlineElement_1.default;
var NodeInlineElement_1 = __webpack_require__(/*! ./inlineElements/NodeInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts");
exports.NodeInlineElement = NodeInlineElement_1.default;
var PartialInlineElement_1 = __webpack_require__(/*! ./inlineElements/PartialInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts");
exports.PartialInlineElement = PartialInlineElement_1.default;
var arrayPush_1 = __webpack_require__(/*! ./utils/arrayPush */ "./packages/roosterjs-editor-dom/lib/utils/arrayPush.ts");
exports.arrayPush = arrayPush_1.default;
var applyTextStyle_1 = __webpack_require__(/*! ./utils/applyTextStyle */ "./packages/roosterjs-editor-dom/lib/utils/applyTextStyle.ts");
exports.applyTextStyle = applyTextStyle_1.default;
var Browser_1 = __webpack_require__(/*! ./utils/Browser */ "./packages/roosterjs-editor-dom/lib/utils/Browser.ts");
exports.Browser = Browser_1.Browser;
exports.getBrowserInfo = Browser_1.getBrowserInfo;
var applyFormat_1 = __webpack_require__(/*! ./utils/applyFormat */ "./packages/roosterjs-editor-dom/lib/utils/applyFormat.ts");
exports.applyFormat = applyFormat_1.default;
var changeElementTag_1 = __webpack_require__(/*! ./utils/changeElementTag */ "./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts");
exports.changeElementTag = changeElementTag_1.default;
var collapseNodes_1 = __webpack_require__(/*! ./utils/collapseNodes */ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts");
exports.collapseNodes = collapseNodes_1.default;
var contains_1 = __webpack_require__(/*! ./utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
exports.contains = contains_1.default;
var extractClipboardEvent_1 = __webpack_require__(/*! ./utils/extractClipboardEvent */ "./packages/roosterjs-editor-dom/lib/utils/extractClipboardEvent.ts");
exports.extractClipboardEvent = extractClipboardEvent_1.default;
var findClosestElementAncestor_1 = __webpack_require__(/*! ./utils/findClosestElementAncestor */ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts");
exports.findClosestElementAncestor = findClosestElementAncestor_1.default;
var fromHtml_1 = __webpack_require__(/*! ./utils/fromHtml */ "./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts");
exports.fromHtml = fromHtml_1.default;
var getComputedStyles_1 = __webpack_require__(/*! ./utils/getComputedStyles */ "./packages/roosterjs-editor-dom/lib/utils/getComputedStyles.ts");
exports.getComputedStyles = getComputedStyles_1.default;
exports.getComputedStyle = getComputedStyles_1.getComputedStyle;
var getPendableFormatState_1 = __webpack_require__(/*! ./utils/getPendableFormatState */ "./packages/roosterjs-editor-dom/lib/utils/getPendableFormatState.ts");
exports.getPendableFormatState = getPendableFormatState_1.default;
exports.PendableFormatCommandMap = getPendableFormatState_1.PendableFormatCommandMap;
var getTagOfNode_1 = __webpack_require__(/*! ./utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
exports.getTagOfNode = getTagOfNode_1.default;
var isBlockElement_1 = __webpack_require__(/*! ./utils/isBlockElement */ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts");
exports.isBlockElement = isBlockElement_1.default;
var isNodeEmpty_1 = __webpack_require__(/*! ./utils/isNodeEmpty */ "./packages/roosterjs-editor-dom/lib/utils/isNodeEmpty.ts");
exports.isNodeEmpty = isNodeEmpty_1.default;
var isVoidHtmlElement_1 = __webpack_require__(/*! ./utils/isVoidHtmlElement */ "./packages/roosterjs-editor-dom/lib/utils/isVoidHtmlElement.ts");
exports.isVoidHtmlElement = isVoidHtmlElement_1.default;
var matchLink_1 = __webpack_require__(/*! ./utils/matchLink */ "./packages/roosterjs-editor-dom/lib/utils/matchLink.ts");
exports.matchLink = matchLink_1.default;
var queryElements_1 = __webpack_require__(/*! ./utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
exports.queryElements = queryElements_1.default;
var splitParentNode_1 = __webpack_require__(/*! ./utils/splitParentNode */ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts");
exports.splitParentNode = splitParentNode_1.default;
exports.splitBalancedNodeRange = splitParentNode_1.splitBalancedNodeRange;
var unwrap_1 = __webpack_require__(/*! ./utils/unwrap */ "./packages/roosterjs-editor-dom/lib/utils/unwrap.ts");
exports.unwrap = unwrap_1.default;
var wrap_1 = __webpack_require__(/*! ./utils/wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
exports.wrap = wrap_1.default;
var getLeafSibling_1 = __webpack_require__(/*! ./utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
exports.getNextLeafSibling = getLeafSibling_1.getNextLeafSibling;
exports.getPreviousLeafSibling = getLeafSibling_1.getPreviousLeafSibling;
var getLeafNode_1 = __webpack_require__(/*! ./utils/getLeafNode */ "./packages/roosterjs-editor-dom/lib/utils/getLeafNode.ts");
exports.getFirstLeafNode = getLeafNode_1.getFirstLeafNode;
exports.getLastLeafNode = getLeafNode_1.getLastLeafNode;
var getTextContent_1 = __webpack_require__(/*! ./utils/getTextContent */ "./packages/roosterjs-editor-dom/lib/utils/getTextContent.ts");
exports.getTextContent = getTextContent_1.default;
var splitTextNode_1 = __webpack_require__(/*! ./utils/splitTextNode */ "./packages/roosterjs-editor-dom/lib/utils/splitTextNode.ts");
exports.splitTextNode = splitTextNode_1.default;
var normalizeRect_1 = __webpack_require__(/*! ./utils/normalizeRect */ "./packages/roosterjs-editor-dom/lib/utils/normalizeRect.ts");
exports.normalizeRect = normalizeRect_1.default;
var toArray_1 = __webpack_require__(/*! ./utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
exports.toArray = toArray_1.default;
var safeInstanceOf_1 = __webpack_require__(/*! ./utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
exports.safeInstanceOf = safeInstanceOf_1.default;
var readFile_1 = __webpack_require__(/*! ./utils/readFile */ "./packages/roosterjs-editor-dom/lib/utils/readFile.ts");
exports.readFile = readFile_1.default;
var getInnerHTML_1 = __webpack_require__(/*! ./utils/getInnerHTML */ "./packages/roosterjs-editor-dom/lib/utils/getInnerHTML.ts");
exports.getInnerHTML = getInnerHTML_1.default;
var VTable_1 = __webpack_require__(/*! ./table/VTable */ "./packages/roosterjs-editor-dom/lib/table/VTable.ts");
exports.VTable = VTable_1.default;
var VList_1 = __webpack_require__(/*! ./list/VList */ "./packages/roosterjs-editor-dom/lib/list/VList.ts");
exports.VList = VList_1.default;
var VListItem_1 = __webpack_require__(/*! ./list/VListItem */ "./packages/roosterjs-editor-dom/lib/list/VListItem.ts");
exports.VListItem = VListItem_1.default;
var createVListFromRegion_1 = __webpack_require__(/*! ./list/createVListFromRegion */ "./packages/roosterjs-editor-dom/lib/list/createVListFromRegion.ts");
exports.createVListFromRegion = createVListFromRegion_1.default;
var VListChain_1 = __webpack_require__(/*! ./list/VListChain */ "./packages/roosterjs-editor-dom/lib/list/VListChain.ts");
exports.VListChain = VListChain_1.default;
var getRegionsFromRange_1 = __webpack_require__(/*! ./region/getRegionsFromRange */ "./packages/roosterjs-editor-dom/lib/region/getRegionsFromRange.ts");
exports.getRegionsFromRange = getRegionsFromRange_1.default;
var getSelectedBlockElementsInRegion_1 = __webpack_require__(/*! ./region/getSelectedBlockElementsInRegion */ "./packages/roosterjs-editor-dom/lib/region/getSelectedBlockElementsInRegion.ts");
exports.getSelectedBlockElementsInRegion = getSelectedBlockElementsInRegion_1.default;
var collapseNodesInRegion_1 = __webpack_require__(/*! ./region/collapseNodesInRegion */ "./packages/roosterjs-editor-dom/lib/region/collapseNodesInRegion.ts");
exports.collapseNodesInRegion = collapseNodesInRegion_1.default;
var isNodeInRegion_1 = __webpack_require__(/*! ./region/isNodeInRegion */ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts");
exports.isNodeInRegion = isNodeInRegion_1.default;
var getSelectionRangeInRegion_1 = __webpack_require__(/*! ./region/getSelectionRangeInRegion */ "./packages/roosterjs-editor-dom/lib/region/getSelectionRangeInRegion.ts");
exports.getSelectionRangeInRegion = getSelectionRangeInRegion_1.default;
var mergeBlocksInRegion_1 = __webpack_require__(/*! ./region/mergeBlocksInRegion */ "./packages/roosterjs-editor-dom/lib/region/mergeBlocksInRegion.ts");
exports.mergeBlocksInRegion = mergeBlocksInRegion_1.default;
var Position_1 = __webpack_require__(/*! ./selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
exports.Position = Position_1.default;
var createRange_1 = __webpack_require__(/*! ./selection/createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
exports.createRange = createRange_1.default;
var getPositionRect_1 = __webpack_require__(/*! ./selection/getPositionRect */ "./packages/roosterjs-editor-dom/lib/selection/getPositionRect.ts");
exports.getPositionRect = getPositionRect_1.default;
var isPositionAtBeginningOf_1 = __webpack_require__(/*! ./selection/isPositionAtBeginningOf */ "./packages/roosterjs-editor-dom/lib/selection/isPositionAtBeginningOf.ts");
exports.isPositionAtBeginningOf = isPositionAtBeginningOf_1.default;
var getSelectionPath_1 = __webpack_require__(/*! ./selection/getSelectionPath */ "./packages/roosterjs-editor-dom/lib/selection/getSelectionPath.ts");
exports.getSelectionPath = getSelectionPath_1.default;
var getHtmlWithSelectionPath_1 = __webpack_require__(/*! ./selection/getHtmlWithSelectionPath */ "./packages/roosterjs-editor-dom/lib/selection/getHtmlWithSelectionPath.ts");
exports.getHtmlWithSelectionPath = getHtmlWithSelectionPath_1.default;
var setHtmlWithSelectionPath_1 = __webpack_require__(/*! ./selection/setHtmlWithSelectionPath */ "./packages/roosterjs-editor-dom/lib/selection/setHtmlWithSelectionPath.ts");
exports.setHtmlWithSelectionPath = setHtmlWithSelectionPath_1.default;
var addRangeToSelection_1 = __webpack_require__(/*! ./selection/addRangeToSelection */ "./packages/roosterjs-editor-dom/lib/selection/addRangeToSelection.ts");
exports.addRangeToSelection = addRangeToSelection_1.default;
var deleteSelectedContent_1 = __webpack_require__(/*! ./selection/deleteSelectedContent */ "./packages/roosterjs-editor-dom/lib/selection/deleteSelectedContent.ts");
exports.deleteSelectedContent = deleteSelectedContent_1.default;
var addSnapshot_1 = __webpack_require__(/*! ./snapshots/addSnapshot */ "./packages/roosterjs-editor-dom/lib/snapshots/addSnapshot.ts");
exports.addSnapshot = addSnapshot_1.default;
var canMoveCurrentSnapshot_1 = __webpack_require__(/*! ./snapshots/canMoveCurrentSnapshot */ "./packages/roosterjs-editor-dom/lib/snapshots/canMoveCurrentSnapshot.ts");
exports.canMoveCurrentSnapshot = canMoveCurrentSnapshot_1.default;
var clearProceedingSnapshots_1 = __webpack_require__(/*! ./snapshots/clearProceedingSnapshots */ "./packages/roosterjs-editor-dom/lib/snapshots/clearProceedingSnapshots.ts");
exports.clearProceedingSnapshots = clearProceedingSnapshots_1.default;
var moveCurrentSnapsnot_1 = __webpack_require__(/*! ./snapshots/moveCurrentSnapsnot */ "./packages/roosterjs-editor-dom/lib/snapshots/moveCurrentSnapsnot.ts");
exports.moveCurrentSnapsnot = moveCurrentSnapsnot_1.default;
var createSnapshots_1 = __webpack_require__(/*! ./snapshots/createSnapshots */ "./packages/roosterjs-editor-dom/lib/snapshots/createSnapshots.ts");
exports.createSnapshots = createSnapshots_1.default;
var canUndoAutoComplete_1 = __webpack_require__(/*! ./snapshots/canUndoAutoComplete */ "./packages/roosterjs-editor-dom/lib/snapshots/canUndoAutoComplete.ts");
exports.canUndoAutoComplete = canUndoAutoComplete_1.default;
var HtmlSanitizer_1 = __webpack_require__(/*! ./htmlSanitizer/HtmlSanitizer */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/HtmlSanitizer.ts");
exports.HtmlSanitizer = HtmlSanitizer_1.default;
var getInheritableStyles_1 = __webpack_require__(/*! ./htmlSanitizer/getInheritableStyles */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getInheritableStyles.ts");
exports.getInheritableStyles = getInheritableStyles_1.default;
var createDefaultHtmlSanitizerOptions_1 = __webpack_require__(/*! ./htmlSanitizer/createDefaultHtmlSanitizerOptions */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/createDefaultHtmlSanitizerOptions.ts");
exports.createDefaultHtmlSanitizerOptions = createDefaultHtmlSanitizerOptions_1.default;
var chainSanitizerCallback_1 = __webpack_require__(/*! ./htmlSanitizer/chainSanitizerCallback */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/chainSanitizerCallback.ts");
exports.chainSanitizerCallback = chainSanitizerCallback_1.default;
var commitEntity_1 = __webpack_require__(/*! ./entity/commitEntity */ "./packages/roosterjs-editor-dom/lib/entity/commitEntity.ts");
exports.commitEntity = commitEntity_1.default;
var getEntityFromElement_1 = __webpack_require__(/*! ./entity/getEntityFromElement */ "./packages/roosterjs-editor-dom/lib/entity/getEntityFromElement.ts");
exports.getEntityFromElement = getEntityFromElement_1.default;
var getEntitySelector_1 = __webpack_require__(/*! ./entity/getEntitySelector */ "./packages/roosterjs-editor-dom/lib/entity/getEntitySelector.ts");
exports.getEntitySelector = getEntitySelector_1.default;
var cacheGetEventData_1 = __webpack_require__(/*! ./event/cacheGetEventData */ "./packages/roosterjs-editor-dom/lib/event/cacheGetEventData.ts");
exports.cacheGetEventData = cacheGetEventData_1.default;
var clearEventDataCache_1 = __webpack_require__(/*! ./event/clearEventDataCache */ "./packages/roosterjs-editor-dom/lib/event/clearEventDataCache.ts");
exports.clearEventDataCache = clearEventDataCache_1.default;
var isModifierKey_1 = __webpack_require__(/*! ./event/isModifierKey */ "./packages/roosterjs-editor-dom/lib/event/isModifierKey.ts");
exports.isModifierKey = isModifierKey_1.default;
var isCharacterValue_1 = __webpack_require__(/*! ./event/isCharacterValue */ "./packages/roosterjs-editor-dom/lib/event/isCharacterValue.ts");
exports.isCharacterValue = isCharacterValue_1.default;
var isCtrlOrMetaPressed_1 = __webpack_require__(/*! ./event/isCtrlOrMetaPressed */ "./packages/roosterjs-editor-dom/lib/event/isCtrlOrMetaPressed.ts");
exports.isCtrlOrMetaPressed = isCtrlOrMetaPressed_1.default;
var getStyles_1 = __webpack_require__(/*! ./style/getStyles */ "./packages/roosterjs-editor-dom/lib/style/getStyles.ts");
exports.getStyles = getStyles_1.default;
var setStyles_1 = __webpack_require__(/*! ./style/setStyles */ "./packages/roosterjs-editor-dom/lib/style/setStyles.ts");
exports.setStyles = setStyles_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/EmptyInlineElement.ts":
/*!********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/EmptyInlineElement.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Represents an empty InlineElement.
 * This is used for ContentTraverser internally only.
 * An empty InlineElement means current position is at the end of a tag so nothing is included inside this element
 */
var EmptyInlineElement = /** @class */ (function () {
    function EmptyInlineElement(position, parentBlock) {
        this.position = position;
        this.parentBlock = parentBlock;
    }
    /**
     * Get the text content of this inline element
     */
    EmptyInlineElement.prototype.getTextContent = function () {
        return '';
    };
    /**
     * Get the container node of this inline element
     */
    EmptyInlineElement.prototype.getContainerNode = function () {
        return this.position.node;
    };
    /**
     * Get the parent block element of this inline element
     */
    EmptyInlineElement.prototype.getParentBlock = function () {
        return this.parentBlock;
    };
    /**
     * Get the start position of this inline element
     */
    EmptyInlineElement.prototype.getStartPosition = function () {
        return this.position;
    };
    /**
     * Get the end position of this inline element
     */
    EmptyInlineElement.prototype.getEndPosition = function () {
        return this.position;
    };
    /**
     * Checks if the given inline element is after this inline element
     */
    EmptyInlineElement.prototype.isAfter = function (inlineElement) {
        return inlineElement && this.position.isAfter(inlineElement.getEndPosition());
    };
    /**
     * Checks if this inline element is a textual inline element
     */
    EmptyInlineElement.prototype.isTextualInlineElement = function () {
        return false;
    };
    /**
     * Checks if the given editor position is contained in this inline element
     */
    EmptyInlineElement.prototype.contains = function (position) {
        return false;
    };
    /**
     * Apply inline style to a region of an inline element.
     */
    EmptyInlineElement.prototype.applyStyle = function (styler) { };
    return EmptyInlineElement;
}());
exports.default = EmptyInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/ImageInlineElement.ts":
/*!********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/ImageInlineElement.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var NodeInlineElement_1 = __webpack_require__(/*! ./NodeInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts");
/**
 * This is an inline element representing an Html image
 */
var ImageInlineElement = /** @class */ (function (_super) {
    __extends(ImageInlineElement, _super);
    function ImageInlineElement(containerNode, parentBlock) {
        return _super.call(this, containerNode, parentBlock) || this;
    }
    return ImageInlineElement;
}(NodeInlineElement_1.default));
exports.default = ImageInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/LinkInlineElement.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/LinkInlineElement.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var NodeInlineElement_1 = __webpack_require__(/*! ./NodeInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts");
/**
 * This is inline element presenting an html hyperlink
 */
var LinkInlineElement = /** @class */ (function (_super) {
    __extends(LinkInlineElement, _super);
    function LinkInlineElement(containerNode, parentBlock) {
        return _super.call(this, containerNode, parentBlock) || this;
    }
    return LinkInlineElement;
}(NodeInlineElement_1.default));
exports.default = LinkInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyTextStyle_1 = __webpack_require__(/*! ../utils/applyTextStyle */ "./packages/roosterjs-editor-dom/lib/utils/applyTextStyle.ts");
var isNodeAfter_1 = __webpack_require__(/*! ../utils/isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
/**
 * This presents an inline element that can be reprented by a single html node.
 * This serves as base for most inline element as it contains most implentation
 * of all operations that can happen on an inline element. Other sub inline elements mostly
 * just identify themself for a certain type
 */
var NodeInlineElement = /** @class */ (function () {
    function NodeInlineElement(containerNode, parentBlock) {
        this.containerNode = containerNode;
        this.parentBlock = parentBlock;
    }
    /**
     * The text content for this inline element
     */
    NodeInlineElement.prototype.getTextContent = function () {
        // nodeValue is better way to retrieve content for a text. Others, just use textContent
        return this.containerNode.nodeType == 3 /* Text */
            ? this.containerNode.nodeValue
            : this.containerNode.textContent;
    };
    /**
     * Get the container node
     */
    NodeInlineElement.prototype.getContainerNode = function () {
        return this.containerNode;
    };
    // Get the parent block
    NodeInlineElement.prototype.getParentBlock = function () {
        return this.parentBlock;
    };
    /**
     * Get the start position of the inline element
     */
    NodeInlineElement.prototype.getStartPosition = function () {
        // For a position, we always want it to point to a leaf node
        // We should try to go get the lowest first child node from the container
        return new Position_1.default(this.containerNode, 0).normalize();
    };
    /**
     * Get the end position of the inline element
     */
    NodeInlineElement.prototype.getEndPosition = function () {
        // For a position, we always want it to point to a leaf node
        // We should try to go get the lowest last child node from the container
        return new Position_1.default(this.containerNode, -1 /* End */).normalize();
    };
    /**
     * Checks if this inline element is a textual inline element
     */
    NodeInlineElement.prototype.isTextualInlineElement = function () {
        return this.containerNode && this.containerNode.nodeType == 3 /* Text */;
    };
    /**
     * Checks if an inline element is after the current inline element
     */
    NodeInlineElement.prototype.isAfter = function (inlineElement) {
        return inlineElement && isNodeAfter_1.default(this.containerNode, inlineElement.getContainerNode());
    };
    /**
     * Checks if the given position is contained in the inline element
     */
    NodeInlineElement.prototype.contains = function (pos) {
        var start = this.getStartPosition();
        var end = this.getEndPosition();
        return pos && pos.isAfter(start) && end.isAfter(pos);
    };
    /**
     * Apply inline style to an inline element
     */
    NodeInlineElement.prototype.applyStyle = function (styler) {
        applyTextStyle_1.default(this.containerNode, styler);
    };
    return NodeInlineElement;
}());
exports.default = NodeInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var applyTextStyle_1 = __webpack_require__(/*! ../utils/applyTextStyle */ "./packages/roosterjs-editor-dom/lib/utils/applyTextStyle.ts");
var createRange_1 = __webpack_require__(/*! ../selection/createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * This is a special version of inline element that identifies a section of an inline element
 * We often have the need to cut an inline element in half and perform some operation only on half of an inline element
 * i.e. users select only some text of a text node and apply format, in that case, format has to happen on partial of an inline element
 * PartialInlineElement is implemented in a way that decorate another full inline element with its own override on methods like isAfter
 * It also offers some special methods that others don't have, i.e. nextInlineElement etc.
 */
var PartialInlineElement = /** @class */ (function () {
    function PartialInlineElement(inlineElement, start, end) {
        this.inlineElement = inlineElement;
        this.start = start;
        this.end = end;
    }
    /**
     * Get the full inline element that this partial inline decorates
     */
    PartialInlineElement.prototype.getDecoratedInline = function () {
        return this.inlineElement;
    };
    /**
     * Gets the container node
     */
    PartialInlineElement.prototype.getContainerNode = function () {
        return this.inlineElement.getContainerNode();
    };
    /**
     * Gets the parent block
     */
    PartialInlineElement.prototype.getParentBlock = function () {
        return this.inlineElement.getParentBlock();
    };
    /**
     * Gets the text content
     */
    PartialInlineElement.prototype.getTextContent = function () {
        var range = createRange_1.default(this.getStartPosition(), this.getEndPosition());
        return range.toString();
    };
    /**
     * Get start position of this inline element.
     */
    PartialInlineElement.prototype.getStartPosition = function () {
        return this.start || this.inlineElement.getStartPosition();
    };
    /**
     * Get end position of this inline element.
     */
    PartialInlineElement.prototype.getEndPosition = function () {
        return this.end || this.inlineElement.getEndPosition();
    };
    Object.defineProperty(PartialInlineElement.prototype, "nextInlineElement", {
        /**
         * Get next partial inline element if it is not at the end boundary yet
         */
        get: function () {
            return this.end && new PartialInlineElement(this.inlineElement, this.end, null);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartialInlineElement.prototype, "previousInlineElement", {
        /**
         * Get previous partial inline element if it is not at the begin boundary yet
         */
        get: function () {
            return this.start && new PartialInlineElement(this.inlineElement, null, this.start);
        },
        enumerable: true,
        configurable: true
    });
    /**
     * Checks if it contains a position
     */
    PartialInlineElement.prototype.contains = function (pos) {
        return pos && pos.isAfter(this.getStartPosition()) && this.getEndPosition().isAfter(pos);
    };
    /**
     * Checks if this inline element is a textual inline element
     */
    PartialInlineElement.prototype.isTextualInlineElement = function () {
        return this.inlineElement && this.inlineElement.isTextualInlineElement();
    };
    /**
     * Check if this inline element is after the other inline element
     */
    PartialInlineElement.prototype.isAfter = function (inlineElement) {
        var thisStart = this.getStartPosition();
        var otherEnd = inlineElement && inlineElement.getEndPosition();
        return otherEnd && (thisStart.isAfter(otherEnd) || thisStart.equalTo(otherEnd));
    };
    /**
     * apply style
     */
    PartialInlineElement.prototype.applyStyle = function (styler) {
        var from = this.getStartPosition().normalize();
        var to = this.getEndPosition().normalize();
        var container = this.getContainerNode();
        if (from.isAtEnd) {
            var nextNode = getLeafSibling_1.getNextLeafSibling(container, from.node);
            from = nextNode ? new Position_1.default(nextNode, 0 /* Begin */) : null;
        }
        if (to.offset == 0) {
            var previousNode = getLeafSibling_1.getPreviousLeafSibling(container, to.node);
            to = previousNode ? new Position_1.default(previousNode, -1 /* End */) : null;
        }
        applyTextStyle_1.default(container, styler, from, to);
    };
    return PartialInlineElement;
}());
exports.default = PartialInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/getFirstLastInlineElement.ts":
/*!***************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/getFirstLastInlineElement.ts ***!
  \***************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getInlineElementAtNode_1 = __webpack_require__(/*! ./getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
var getLeafNode_1 = __webpack_require__(/*! ../utils/getLeafNode */ "./packages/roosterjs-editor-dom/lib/utils/getLeafNode.ts");
/**
 * @internal
 * Get the first inline element inside the given node
 */
function getFirstInlineElement(rootNode) {
    // getFirstLeafNode can return null for empty container
    // do check null before passing on to get inline from the node
    var node = getLeafNode_1.getFirstLeafNode(rootNode);
    return node ? getInlineElementAtNode_1.default(rootNode, node) : null;
}
exports.getFirstInlineElement = getFirstInlineElement;
/**
 * @internal
 * Get the last inline element inside the given node
 */
function getLastInlineElement(rootNode) {
    // getLastLeafNode can return null for empty container
    // do check null before passing on to get inline from the node
    var node = getLeafNode_1.getLastLeafNode(rootNode);
    return node ? getInlineElementAtNode_1.default(rootNode, node) : null;
}
exports.getLastInlineElement = getLastInlineElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts":
/*!************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var ImageInlineElement_1 = __webpack_require__(/*! ./ImageInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/ImageInlineElement.ts");
var LinkInlineElement_1 = __webpack_require__(/*! ./LinkInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/LinkInlineElement.ts");
var NodeInlineElement_1 = __webpack_require__(/*! ./NodeInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/NodeInlineElement.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
function getInlineElementAtNode(parent, node) {
    // An inline element has to be in a block element, get the block first and then resolve through the factory
    var parentBlock = safeInstanceOf_1.default(parent, 'Node') ? getBlockElementAtNode_1.default(parent, node) : parent;
    return node && parentBlock && resolveInlineElement(node, parentBlock);
}
exports.default = getInlineElementAtNode;
/**
 * Resolve an inline element by a leaf node
 * @param node The node to resolve from
 * @param parentBlock The parent block element
 */
function resolveInlineElement(node, parentBlock) {
    var nodeChain = [node];
    for (var parent_1 = node.parentNode; parent_1 && parentBlock.contains(parent_1); parent_1 = parent_1.parentNode) {
        nodeChain.push(parent_1);
    }
    var inlineElement;
    for (var i = nodeChain.length - 1; i >= 0 && !inlineElement; i--) {
        var currentNode = nodeChain[i];
        var tag = getTagOfNode_1.default(currentNode);
        if (tag == 'A') {
            inlineElement = new LinkInlineElement_1.default(currentNode, parentBlock);
        }
        else if (tag == 'IMG') {
            inlineElement = new ImageInlineElement_1.default(currentNode, parentBlock);
        }
    }
    return inlineElement || new NodeInlineElement_1.default(node, parentBlock);
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementBeforeAfter.ts":
/*!*****************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementBeforeAfter.ts ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getInlineElementAtNode_1 = __webpack_require__(/*! ./getInlineElementAtNode */ "./packages/roosterjs-editor-dom/lib/inlineElements/getInlineElementAtNode.ts");
var PartialInlineElement_1 = __webpack_require__(/*! ./PartialInlineElement */ "./packages/roosterjs-editor-dom/lib/inlineElements/PartialInlineElement.ts");
var shouldSkipNode_1 = __webpack_require__(/*! ../utils/shouldSkipNode */ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * @internal
 * Get inline element before a position
 * This is mostly used when we want to get the inline element before selection/cursor
 * There is a possible that the cursor is in middle of an inline element (i.e. mid of a text node)
 * in this case, we only want to return what is before cursor (a partial of an inline) to indicate
 * that we're in middle.
 * @param root Root node of current scope, use for create InlineElement
 * @param position The position to get InlineElement before
 */
function getInlineElementBefore(root, position) {
    return getInlineElementBeforeAfter(root, position, false /*isAfter*/);
}
exports.getInlineElementBefore = getInlineElementBefore;
/**
 * @internal
 * Get inline element after a position
 * This is mostly used when we want to get the inline element after selection/cursor
 * There is a possible that the cursor is in middle of an inline element (i.e. mid of a text node)
 * in this case, we only want to return what is before cursor (a partial of an inline) to indicate
 * that we're in middle.
 * @param root Root node of current scope, use for create InlineElement
 * @param position The position to get InlineElement after
 */
function getInlineElementAfter(root, position) {
    return getInlineElementBeforeAfter(root, position, true /*isAfter*/);
}
exports.getInlineElementAfter = getInlineElementAfter;
/**
 * @internal
 */
function getInlineElementBeforeAfter(root, position, isAfter) {
    if (!root || !position || !position.node) {
        return null;
    }
    position = position.normalize();
    var node = position.node, offset = position.offset, isAtEnd = position.isAtEnd;
    var isPartial = false;
    if ((!isAfter && offset == 0 && !isAtEnd) || (isAfter && isAtEnd)) {
        node = getLeafSibling_1.getLeafSibling(root, node, isAfter);
    }
    else if (node.nodeType == 3 /* Text */ &&
        ((!isAfter && !isAtEnd) || (isAfter && offset > 0))) {
        isPartial = true;
    }
    if (node && shouldSkipNode_1.default(node)) {
        node = getLeafSibling_1.getLeafSibling(root, node, isAfter);
    }
    var inlineElement = getInlineElementAtNode_1.default(root, node);
    if (inlineElement && (isPartial || inlineElement.contains(position))) {
        inlineElement = isAfter
            ? new PartialInlineElement_1.default(inlineElement, position, null)
            : new PartialInlineElement_1.default(inlineElement, null, position);
    }
    return inlineElement;
}
exports.getInlineElementBeforeAfter = getInlineElementBeforeAfter;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/VList.ts":
/*!*********************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/VList.ts ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var changeElementTag_1 = __webpack_require__(/*! ../utils/changeElementTag */ "./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts");
var getListTypeFromNode_1 = __webpack_require__(/*! ./getListTypeFromNode */ "./packages/roosterjs-editor-dom/lib/list/getListTypeFromNode.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var isBlockElement_1 = __webpack_require__(/*! ../utils/isBlockElement */ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts");
var isNodeEmpty_1 = __webpack_require__(/*! ../utils/isNodeEmpty */ "./packages/roosterjs-editor-dom/lib/utils/isNodeEmpty.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var queryElements_1 = __webpack_require__(/*! ../utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var splitParentNode_1 = __webpack_require__(/*! ../utils/splitParentNode */ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts");
var toArray_1 = __webpack_require__(/*! ../utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
var unwrap_1 = __webpack_require__(/*! ../utils/unwrap */ "./packages/roosterjs-editor-dom/lib/utils/unwrap.ts");
var VListItem_1 = __webpack_require__(/*! ./VListItem */ "./packages/roosterjs-editor-dom/lib/list/VListItem.ts");
var wrap_1 = __webpack_require__(/*! ../utils/wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
/**
 * Represent a bullet or a numbering list
 *
 * @example
 * A VList is a logical representation of list items, it contains an item array with node and list type stack.
 * e.g. We have a list like this
 * ```html
 * <ol>
 *   <li>item 1</li>
 *   <li>item 2</li>
 *   <ul>
 *     <li>item 2.1</li>
 *     <li>item 2.2</li>
 *   <ul>
 * </ol>
 * ```
 *
 * A VList of this list will be like this:
 * ```javascript
 * {
 *   rootList: (OL node),
 *   items: [{
 *       node: (LI node with 'item 1'),
 *       listTypes: [null, OL],
 *     }, {
 *       node: (LI node with 'item 2'),
 *       listTypes: [null, OL],
 *     }, {
 *       node: (LI node with 'item 2.1),
 *       listTypes: [null, OL, UL],
 *     }, {
 *       node: (LI node with 'item 2.2'),
 *       listTypes: [null, OL, UL],
 *     }
 *   ]
 * }
 * ```
 *
 * When we want to outdent item 2.1, we just need to remove the last "UL" from listTypes of item 2.1, then
 * the writeBack() function will handle everything related to DOM change
 */
var VList = /** @class */ (function () {
    /**
     * Create a new instance of VList class
     * @param rootList The root list element, can be either OL or UL tag
     */
    function VList(rootList) {
        this.rootList = rootList;
        this.items = [];
        if (!rootList) {
            throw new Error('rootList must not be null');
        }
        // Before populate items, we need to normalize the list to make sure it is in a correct format
        // otherwise further action may mass thing up.
        //
        // There are two kinds of normalization to perform.
        // 1. Move nodes directly under OL/UL into a LI node, unless it is an orphan node
        // Please see comment for VListItem.isOrphanItem() for more information about orphan node
        // e.g.:
        // ```HTML
        // <ol>
        //   <li>item 1</li>
        //   <div>item 2</div>
        // </ol>
        // ```
        // After this step, it should become:
        // ```html
        // <ol>
        //   <li>item 1
        //     <div>item 2</div>
        //   <li>
        // </ol>
        // ```
        moveChildNodesToLi(this.rootList);
        queryElements_1.default(this.rootList, 'ol,ul', moveChildNodesToLi);
        // 2. Move LI node embeded into another LI node out to directly under OL/UL node
        // Ideally browser we do this for us automatically when out the HTML into DOM. However after
        // step 1, it is possible that we move some LI node into another one. e.g:
        // ```HTML
        // <ol>
        //   <li>item 1</li>
        //   <div>
        //     item 1.1
        //     <li>item 3</li>
        //   </div>
        // </ol>
        // ```
        // See that the second LI tag is not directly under OL, so after step 1, this will become:
        // ```html
        // <ol>
        //   <li>item 1
        //     <div>
        //       item 1.1
        //       <li>item 2</li>
        //     </div>
        //   <li>
        // </ol>
        // ```
        // Now we have a LI tag embeded into another LI tag. So we need step 2 to move the inner LI tag out to be:
        // ```html
        // <ol>
        //   <li>item1
        //     <div>item 1.1</div>
        //   </li>
        //   <li><div>item2</div></li>
        // </ol>
        // ```
        queryElements_1.default(this.rootList, 'li', moveLiToList);
        this.populateItems(this.rootList);
    }
    /**
     * Check if this list contains the given node
     * @param node The node to check
     */
    VList.prototype.contains = function (node) {
        // We don't check if the node is contained by this.rootList here, because after some operation,
        // it is possible a node is logically contained by this list but the container list item hasn't
        // been put under this.rootList in DOM tree yet.
        return this.items.some(function (item) { return item.contains(node); });
    };
    /**
     * Get list number of the last item in this VList.
     * If there is no order list item, result will be undefined
     */
    VList.prototype.getLastItemNumber = function () {
        var start = getStart(this.rootList);
        return start === undefined
            ? start
            : start -
                1 +
                this.items.filter(function (item) { return item.getListType() == 1 /* Ordered */ && item.getLevel() == 1; }).length;
    };
    /**
     * Write the result back into DOM tree
     * After that, this VList becomes unavailable because we set this.rootList to null
     */
    VList.prototype.writeBack = function () {
        var _this = this;
        if (!this.rootList) {
            throw new Error('rootList must not be null');
        }
        var doc = this.rootList.ownerDocument;
        var listStack = [doc.createDocumentFragment()];
        var placeholder = doc.createTextNode('');
        var start = getStart(this.rootList) || 1;
        var lastList;
        // Use a placeholder to hold the position since the root list may be moved into document fragment later
        this.rootList.parentNode.replaceChild(placeholder, this.rootList);
        this.items.forEach(function (item) {
            item.writeBack(listStack, _this.rootList);
            var topList = listStack[1];
            if (safeInstanceOf_1.default(topList, 'HTMLOListElement')) {
                if (lastList != topList) {
                    if (start == 1) {
                        topList.removeAttribute('start');
                    }
                    else {
                        topList.start = start;
                    }
                }
                if (item.getLevel() == 1) {
                    start++;
                }
            }
            lastList = topList;
        });
        // Restore the content to the positioni of placeholder
        placeholder.parentNode.replaceChild(listStack[0], placeholder);
        // Set rootList to null to avoid this to be called again for the same VList, because
        // after change the rootList may not be available any more (e.g. outdent all items).
        this.rootList = null;
    };
    /**
     * Set indentation of the given range of this list
     * @param start Start position to operate from
     * @param end End positon to operate to
     * @param indentation Indent or outdent
     */
    VList.prototype.setIndentation = function (start, end, indentation) {
        this.findListItems(start, end, function (item) {
            return indentation == 1 /* Decrease */ ? item.outdent() : item.indent();
        });
    };
    /**
     * Change list type of the given range of this list.
     * If some of the items are not real list item yet, this will make them to be list item with given type
     * If all items in the given range are already in the type to change to, this becomes an outdent operation
     * @param start Start position to operate from
     * @param end End position to operate to
     * @param targetType Target list type
     */
    VList.prototype.changeListType = function (start, end, targetType) {
        var needChangeType = false;
        this.findListItems(start, end, function (item) {
            needChangeType = needChangeType || item.getListType() != targetType;
        });
        this.findListItems(start, end, function (item) {
            return needChangeType ? item.changeListType(targetType) : item.outdent();
        });
    };
    /**
     * Append a new item to this VList
     * @param node node of the item to append. If it is not wrapped with LI tag, it will be wrapped
     * @param type Type of this list item, can be ListType.None
     */
    VList.prototype.appendItem = function (node, type) {
        var nodeTag = getTagOfNode_1.default(node);
        // Change DIV tag to SPAN. Otherwise we can create new list item by Enter key in Safari
        if (nodeTag == 'DIV') {
            node = changeElementTag_1.default(node, 'LI');
        }
        else if (nodeTag != 'LI') {
            node = wrap_1.default(node, 'LI');
        }
        this.items.push(type == 0 /* None */ ? new VListItem_1.default(node) : new VListItem_1.default(node, type));
    };
    /**
     * Merge the given VList into current VList.
     * - All list items will be removed from the given VList and added into this list.
     * - The root node of the given VList will be removed from DOM tree
     * - If there are orphan items in the given VList, they will be merged into the last item
     *   of this list if any.
     * @param list The vList to merge from
     */
    VList.prototype.mergeVList = function (list) {
        var _this = this;
        var _a;
        if (list && list != this) {
            var originalLength = this.items.length;
            list.items.forEach(function (item) { return _this.items.push(item); });
            list.items.splice(0, list.items.length);
            this.mergeOrphanNodesAfter(originalLength - 1);
            (_a = list.rootList.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(list.rootList);
        }
    };
    VList.prototype.mergeOrphanNodesAfter = function (startIndex) {
        var item = this.items[startIndex];
        if (item && !item.isOrphanItem()) {
            for (var i = startIndex + 1; i <= this.items.length; i++) {
                if (!item || !item.canMerge(this.items[i])) {
                    item.mergeItems(this.items.splice(startIndex + 1, i - startIndex - 1));
                    break;
                }
            }
        }
    };
    VList.prototype.findListItems = function (start, end, callback) {
        if (this.items.length == 0) {
            return [];
        }
        var listStartPos = new Position_1.default(this.items[0].getNode(), 0 /* Begin */);
        var listEndPos = new Position_1.default(this.items[this.items.length - 1].getNode(), -1 /* End */);
        var startIndex = listStartPos.isAfter(start) ? 0 : -1;
        var endIndex = this.items.length - (end.isAfter(listEndPos) ? 1 : 0);
        this.items.forEach(function (item, index) {
            startIndex = item.contains(start.node) ? index : startIndex;
            endIndex = item.contains(end.node) ? index : endIndex;
        });
        startIndex = endIndex < this.items.length ? Math.max(0, startIndex) : startIndex;
        endIndex = startIndex >= 0 ? Math.min(this.items.length - 1, endIndex) : endIndex;
        var result = startIndex <= endIndex ? this.items.slice(startIndex, endIndex + 1) : [];
        if (callback) {
            result.forEach(callback);
            this.mergeOrphanNodesAfter(endIndex);
        }
        return result;
    };
    VList.prototype.populateItems = function (list, listTypes) {
        if (listTypes === void 0) { listTypes = []; }
        var type = getListTypeFromNode_1.default(list);
        for (var item = list.firstChild; !!item; item = item.nextSibling) {
            var newListTypes = __spreadArrays(listTypes, [type]);
            if (getListTypeFromNode_1.isListElement(item)) {
                this.populateItems(item, newListTypes);
            }
            else if (item.nodeType != 3 /* Text */ || item.nodeValue.trim() != '') {
                this.items.push(new (VListItem_1.default.bind.apply(VListItem_1.default, __spreadArrays([void 0, item], newListTypes)))());
            }
        }
    };
    return VList;
}());
exports.default = VList;
//Normalization
// Step 1: Move all non-LI direct children under list into LI
// e.g.
// From: <ul><li>line 1</li>line 2</ul>
// To:   <ul><li>line 1<div>line 2</div></li></ul>
function moveChildNodesToLi(list) {
    var currentItem = null;
    toArray_1.default(list.childNodes).forEach(function (child) {
        if (getTagOfNode_1.default(child) == 'LI') {
            currentItem = child;
        }
        else if (getListTypeFromNode_1.isListElement(child)) {
            currentItem = null;
        }
        else if (currentItem && !isNodeEmpty_1.default(child, true /*trimContent*/)) {
            currentItem.appendChild(isBlockElement_1.default(child) ? child : wrap_1.default(child));
        }
    });
}
// Step 2: Move nested LI up to under list directly
// e.g.
// From: <ul><li>line 1<li>line 2</li>line 3</li></ul>
// To:   <ul><li>line 1</li><li>line 2<div>line 3</div></li></ul>
function moveLiToList(li) {
    while (!getListTypeFromNode_1.isListElement(li.parentNode)) {
        splitParentNode_1.default(li, true /*splitBefore*/);
        var furtherNodes = toArray_1.default(li.parentNode.childNodes).slice(1);
        if (furtherNodes.length > 0) {
            if (!isBlockElement_1.default(furtherNodes[0])) {
                furtherNodes = [wrap_1.default(furtherNodes)];
            }
            furtherNodes.forEach(function (node) { return li.appendChild(node); });
        }
        unwrap_1.default(li.parentNode);
    }
}
function getStart(list) {
    return safeInstanceOf_1.default(list, 'HTMLOListElement') ? list.start : undefined;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/VListChain.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/VListChain.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var arrayPush_1 = __webpack_require__(/*! ../utils/arrayPush */ "./packages/roosterjs-editor-dom/lib/utils/arrayPush.ts");
var getRootListNode_1 = __webpack_require__(/*! ./getRootListNode */ "./packages/roosterjs-editor-dom/lib/list/getRootListNode.ts");
var isNodeAfter_1 = __webpack_require__(/*! ../utils/isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
var isNodeInRegion_1 = __webpack_require__(/*! ../region/isNodeInRegion */ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts");
var queryElements_1 = __webpack_require__(/*! ../utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
var VList_1 = __webpack_require__(/*! ./VList */ "./packages/roosterjs-editor-dom/lib/list/VList.ts");
var CHAIN_NAME_PREFIX = '__List_Chain_';
var CHAIN_DATASET_NAME = 'listchain';
var AFTER_CURSOR_DATASET_NAME = 'listchainafter';
var lastChainIndex = 0;
/**
 * Represent a chain of list nodes.
 * A chain of lists is a virtual link of lists that have continuous numbers, when editor one of them,
 * all others should also be updated in order to main the list number to be continuous.
 */
var VListChain = /** @class */ (function () {
    /**
     * Contruct a new instance of VListChain class
     * @param editor Editor object
     */
    function VListChain(region, name) {
        this.region = region;
        this.name = name;
        this.lastNumber = 0;
        this.lastNumberBeforeCursor = 0;
    }
    /**
     * Create an array of VListChain from current region in editor
     * @param region The region to create VListChain from
     * @param currentNode Optional current node, used for mark lists that are after this node
     * @param nameGenerator Used by test code only
     */
    VListChain.createListChains = function (region, currentNode, nameGenerator) {
        var regions = Array.isArray(region) ? region : region ? [region] : [];
        var result = [];
        regions.forEach(function (region) {
            var chains = [];
            var lastList;
            queryElements_1.default(region.rootNode, 'ol', function (ol) {
                var list = getRootListNode_1.default(region, 'ol', ol);
                if (lastList != list) {
                    var chain = chains.filter(function (c) { return c.canAppendToTail(list); })[0] ||
                        new VListChain(region, (nameGenerator || createListChainName)());
                    var index = chains.indexOf(chain);
                    var afterCurrentNode = currentNode && isNodeAfter_1.default(list, currentNode);
                    if (!afterCurrentNode) {
                        // Make sure current one is at the front if current block has not been met, so that
                        // the first chain is always the nearest one from current node
                        if (index >= 0) {
                            chains.splice(index, 1);
                        }
                        chains.unshift(chain);
                    }
                    else if (index < 0) {
                        chains.push(chain);
                    }
                    chain.append(list, afterCurrentNode);
                    lastList = list;
                }
            });
            arrayPush_1.default(result, chains);
        });
        return result;
    };
    /**
     * Check if a list with the given start number can be appended next to the last list before cursor
     * @param startNumber The start number of the new list
     */
    VListChain.prototype.canAppendAtCursor = function (startNumber) {
        return this.lastNumberBeforeCursor + 1 == startNumber;
    };
    /**
     * Create a VList to wrap the block of the given node, and append to current chain
     * @param container The container node to create list at
     * @param startNumber Start number of the new list
     */
    VListChain.prototype.createVListAtBlock = function (container, startNumber) {
        if (container) {
            var list = container.ownerDocument.createElement('ol');
            list.start = startNumber;
            this.applyChainName(list);
            container.parentNode.insertBefore(list, container);
            var vList = new VList_1.default(list);
            vList.appendItem(container, 0 /* None */);
            return vList;
        }
        else {
            return null;
        }
    };
    /**
     * After change the lists, commit the change to all lists in this chain to update the list number,
     * and clear the temporary dataset values added to list node
     */
    VListChain.prototype.commit = function () {
        var lists = this.getLists();
        var lastNumber = 0;
        for (var i = 0; i < lists.length; i++) {
            var list = lists[i];
            list.start = lastNumber + 1;
            var vlist = new VList_1.default(list);
            lastNumber = vlist.getLastItemNumber();
            delete list.dataset[CHAIN_DATASET_NAME];
            delete list.dataset[AFTER_CURSOR_DATASET_NAME];
            vlist.writeBack();
        }
    };
    /**
     * Check if the given list node is can be appended into current list chain
     * @param list The list node to check
     */
    VListChain.prototype.canAppendToTail = function (list) {
        return this.lastNumber + 1 == list.start;
    };
    /**
     * Append the given list node into this VListChain
     * @param list The list node to append
     * @param isAfterCurrentNode Whether this list is after current node
     */
    VListChain.prototype.append = function (list, isAfterCurrentNode) {
        this.applyChainName(list);
        this.lastNumber = new VList_1.default(list).getLastItemNumber();
        if (isAfterCurrentNode) {
            list.dataset[AFTER_CURSOR_DATASET_NAME] = 'true';
        }
        else {
            this.lastNumberBeforeCursor = this.lastNumber;
        }
    };
    VListChain.prototype.applyChainName = function (list) {
        list.dataset[CHAIN_DATASET_NAME] = this.name;
    };
    VListChain.prototype.getLists = function () {
        var _this = this;
        return queryElements_1.default(this.region.rootNode, "ol[data-" + CHAIN_DATASET_NAME + "=" + this.name + "]").filter(function (node) { return isNodeInRegion_1.default(_this.region, node); });
    };
    return VListChain;
}());
exports.default = VListChain;
function createListChainName() {
    return CHAIN_NAME_PREFIX + lastChainIndex++;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/VListItem.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/VListItem.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getListTypeFromNode_1 = __webpack_require__(/*! ./getListTypeFromNode */ "./packages/roosterjs-editor-dom/lib/list/getListTypeFromNode.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var isBlockElement_1 = __webpack_require__(/*! ../utils/isBlockElement */ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts");
var toArray_1 = __webpack_require__(/*! ../utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
var unwrap_1 = __webpack_require__(/*! ../utils/unwrap */ "./packages/roosterjs-editor-dom/lib/utils/unwrap.ts");
var wrap_1 = __webpack_require__(/*! ../utils/wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
var orderListStyles = [null, 'lower-alpha', 'lower-roman'];
/**
 * @internal
 * !!! Never directly create instance of this class. It should be created within VList class !!!
 *
 * Represent a list item.
 *
 * A list item is normally wrapped using a LI tag. But this class is only a logical item,
 * it can be a LI tag, or another other type of node which means it is actually not a list item.
 * That can happen after we do "outdent" on a 1-level list item, then it becomes not a list item.
 * @internal
 */
var VListItem = /** @class */ (function () {
    /**
     * Construct a new instance of VListItem class
     * @param node The DOM node for this item
     * @param listTypes An array represnets list types of all parent and current level.
     * Skip this parameter for a non-list item.
     */
    function VListItem(node) {
        var listTypes = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            listTypes[_i - 1] = arguments[_i];
        }
        this.node = node;
        if (!node) {
            throw new Error('node must not be null');
        }
        // Always add a None list type in front of all other types to represent non-list scenario.
        this.listTypes = __spreadArrays([0 /* None */], listTypes);
    }
    /**
     * Get type of current list item
     */
    VListItem.prototype.getListType = function () {
        return this.listTypes[this.listTypes.length - 1];
    };
    /**
     * Get the levels of this list item.
     */
    VListItem.prototype.getLevel = function () {
        return this.listTypes.length - 1;
    };
    /**
     * Get DOM node of this list item
     */
    VListItem.prototype.getNode = function () {
        return this.node;
    };
    /**
     * Check if a given node is contained by this list item
     * @param node The node to check
     */
    VListItem.prototype.contains = function (node) {
        return contains_1.default(this.node, node, true /*treateSameNodeAsContain*/);
    };
    /**
     * Check if this item is an orphan item.
     *
     * Orphan item is not a normal case but could happen. It represents the DOM nodes directly under OL/UL tag
     * and are in front of all other LI tags so that they cannot be merged into any existing LI tags.
     *
     * For example:
     * ```html
     * <ol>
     *   <div>Orphan node</div>
     *   <li>first item</li>
     * </ol>
     * ```
     * Here the first DIV tag is an orphan item.
     *
     * There can also be nodes directly under OL/UL but between other LI tags in source HTML which should not be
     * treated as orphan item because they can be merged into their previous LI tag. But when we build VList,
     * those nodes will be merged into LI, so that ideally here they should not exist.
     */
    VListItem.prototype.isOrphanItem = function () {
        return getTagOfNode_1.default(this.node) != 'LI';
    };
    /**
     * Check if the given item can be merged into this item.
     * An item can be merged when it is an orphan item and its list type stack is exactly the same with current one.
     * @param item The item to check
     */
    VListItem.prototype.canMerge = function (item) {
        if (!(item === null || item === void 0 ? void 0 : item.isOrphanItem()) || this.listTypes.length != item.listTypes.length) {
            return false;
        }
        return this.listTypes.every(function (type, index) { return item.listTypes[index] == type; });
    };
    /**
     * Merge items into this item.
     * @example Before merge:
     * ```html
     * <ol>
     *   <li>Current item</li>
     *   <div>line 1</div>
     *   <div>line 2</div>
     * </ol>
     * ```
     * After merge then two DIVs into LI:
     * ```html
     * <ol>
     *   <li>Current item
     *     <div>line 1</div>
     *     <div>line 2</div>
     *   </li>
     * </ol>
     * ```
     * @param items The items to merge
     */
    VListItem.prototype.mergeItems = function (items) {
        var _this = this;
        var nodesToWrap = (items === null || items === void 0 ? void 0 : items.map(function (item) { return item.node; })) || [];
        var targetNodes = wrapIfNotBlockNode(nodesToWrap, true /*checkFirst*/, false /*checkLast*/);
        targetNodes.forEach(function (node) { return _this.node.appendChild(node); });
    };
    /**
     * Indent this item
     * If this is not an list item, it will be no op
     */
    VListItem.prototype.indent = function () {
        var listType = this.getListType();
        if (listType != 0 /* None */) {
            this.listTypes.push(listType);
        }
    };
    /**
     * Outdent this item
     * If this item is already not an list item, it will be no op
     */
    VListItem.prototype.outdent = function () {
        if (this.listTypes.length > 1) {
            this.listTypes.pop();
        }
    };
    /**
     * Change list type of this item
     * @param targetType The target list type to change to
     */
    VListItem.prototype.changeListType = function (targetType) {
        if (targetType == 0 /* None */) {
            this.listTypes = [targetType];
        }
        else {
            this.outdent();
            this.listTypes.push(targetType);
        }
    };
    /**
     * Write the change result back into DOM
     * @param listStack current stack of list elements
     * @param originalRoot Original list root element. It will be reused when write back if possible
     */
    VListItem.prototype.writeBack = function (listStack, originalRoot) {
        var nextLevel = 1;
        // 1. Determine list elements that we can reuse
        // e.g.:
        //    passed in listStack: Fragment > OL > UL > OL
        //    local listTypes:     null     > OL > UL > UL > OL
        //    then Fragment > OL > UL can be reused
        for (; nextLevel < listStack.length; nextLevel++) {
            if (getListTypeFromNode_1.default(listStack[nextLevel]) !== this.listTypes[nextLevel]) {
                listStack.splice(nextLevel);
                break;
            }
        }
        // 2. Add new list elements
        // e.g.:
        //    passed in listStack: Fragment > OL > UL
        //    local listTypes:     null     > OL > UL > UL > OL
        //    then we need to create a UL and a OL tag
        for (; nextLevel < this.listTypes.length; nextLevel++) {
            var newList = createListElement(listStack[0], this.listTypes[nextLevel], nextLevel, originalRoot);
            listStack[listStack.length - 1].appendChild(newList);
            listStack.push(newList);
        }
        // 3. Add current node into deepest list element
        listStack[listStack.length - 1].appendChild(this.node);
        // 4. If this is not a list item now, need to unwrap the LI node and do proper handling
        if (this.listTypes.length <= 1) {
            wrapIfNotBlockNode(getTagOfNode_1.default(this.node) == 'LI' ? getChildrenAndUnwrap(this.node) : [this.node], true /*checkFirst*/, true /*checkLast*/);
        }
    };
    return VListItem;
}());
exports.default = VListItem;
function createListElement(newRoot, listType, nextLevel, originalRoot) {
    var doc = newRoot.ownerDocument;
    var result;
    // Try to reuse the existing root element
    // It can be reused when
    // 1. Current list item is level 1 (top level), AND
    // 2. Original root exists, AND
    // 3. They have the same list type AND
    // 4. The original root is not used yet
    if (nextLevel == 1 && originalRoot && listType == getListTypeFromNode_1.default(originalRoot)) {
        if (contains_1.default(newRoot, originalRoot)) {
            // If it is already used, let's clone one and remove ID to avoid duplicating ID
            result = originalRoot.cloneNode(false /*deep*/);
            result.removeAttribute('id');
        }
        else {
            // Remove all child nodes, they will be added back later when write back other items
            while (originalRoot.firstChild) {
                originalRoot.removeChild(originalRoot.firstChild);
            }
            result = originalRoot;
        }
    }
    else {
        // Can't be reused, can't clone, let's create a new one
        result = doc.createElement(listType == 1 /* Ordered */ ? 'ol' : 'ul');
    }
    if (listType == 1 /* Ordered */ && nextLevel > 1) {
        result.style.listStyleType = orderListStyles[(nextLevel - 1) % orderListStyles.length];
    }
    return result;
}
function wrapIfNotBlockNode(nodes, checkFirst, checkLast) {
    if (nodes.length > 0 &&
        (!checkFirst || !isBlockElement_1.default(nodes[0])) &&
        (!checkLast || !isBlockElement_1.default(nodes[nodes.length]))) {
        nodes = [wrap_1.default(nodes)];
    }
    return nodes;
}
function getChildrenAndUnwrap(node) {
    var result = toArray_1.default(node.childNodes);
    unwrap_1.default(node);
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/createVListFromRegion.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/createVListFromRegion.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var fromHtml_1 = __webpack_require__(/*! ../utils/fromHtml */ "./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts");
var getRootListNode_1 = __webpack_require__(/*! ./getRootListNode */ "./packages/roosterjs-editor-dom/lib/list/getRootListNode.ts");
var getSelectedBlockElementsInRegion_1 = __webpack_require__(/*! ../region/getSelectedBlockElementsInRegion */ "./packages/roosterjs-editor-dom/lib/region/getSelectedBlockElementsInRegion.ts");
var isNodeInRegion_1 = __webpack_require__(/*! ../region/isNodeInRegion */ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var shouldSkipNode_1 = __webpack_require__(/*! ../utils/shouldSkipNode */ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts");
var toArray_1 = __webpack_require__(/*! ../utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
var VList_1 = __webpack_require__(/*! ./VList */ "./packages/roosterjs-editor-dom/lib/list/VList.ts");
var wrap_1 = __webpack_require__(/*! ../utils/wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
var getListTypeFromNode_1 = __webpack_require__(/*! ./getListTypeFromNode */ "./packages/roosterjs-editor-dom/lib/list/getListTypeFromNode.ts");
var ListSelector = 'ol,ul';
/**
 * @internal
 * @param region The region to get VList from
 * @param includeSiblingLists True to also try get lists before and after the selection and merge them together,
 * false to only include the list for the selected blocks
 * @param startNode (Optional) When specified, try get VList which will contain this node.
 * If not specified, get VList from selection of this region
 */
function createVListFromRegion(region, includeSiblingLists, startNode) {
    if (!region) {
        return null;
    }
    var nodes = [];
    if (startNode) {
        var list = getRootListNode_1.default(region, ListSelector, startNode);
        if (list) {
            nodes.push(list);
        }
    }
    else {
        var blocks = getSelectedBlockElementsInRegion_1.default(region);
        blocks.forEach(function (block) {
            var list = getRootListNode_1.default(region, ListSelector, block.getStartNode());
            if (list) {
                if (nodes[nodes.length - 1] != list) {
                    nodes.push(list);
                }
                if (nodes.length == 1 &&
                    safeInstanceOf_1.default(list, 'HTMLOListElement') &&
                    list.start > 1) {
                    // Do not include sibling lists if this list is not start from 1
                    includeSiblingLists = false;
                }
            }
            else {
                nodes.push(block.collapseToSingleElement());
            }
        });
        if (nodes.length == 0 && !region.rootNode.firstChild) {
            var newNode = fromHtml_1.default('<div><br></div>', region.rootNode.ownerDocument)[0];
            region.rootNode.appendChild(newNode);
            nodes.push(newNode);
            region.fullSelectionStart = new Position_1.default(newNode, 0 /* Begin */);
            region.fullSelectionEnd = new Position_1.default(newNode, -1 /* End */);
        }
        if (includeSiblingLists) {
            tryIncludeSiblingNode(region, nodes, false /*isNext*/);
            tryIncludeSiblingNode(region, nodes, true /*isNext*/);
        }
        nodes = nodes.filter(function (node) { return !shouldSkipNode_1.default(node, true /*ignoreSpace*/); });
    }
    var vList = null;
    if (nodes.length > 0) {
        var firstNode = nodes.shift();
        vList = getListTypeFromNode_1.isListElement(firstNode)
            ? new VList_1.default(firstNode)
            : createVListFromItemNode(firstNode);
        nodes.forEach(function (node) {
            if (getListTypeFromNode_1.isListElement(node)) {
                vList.mergeVList(new VList_1.default(node));
            }
            else {
                vList.appendItem(node, 0 /* None */);
            }
        });
    }
    return vList;
}
exports.default = createVListFromRegion;
function tryIncludeSiblingNode(region, nodes, isNext) {
    var node = nodes[isNext ? nodes.length - 1 : 0];
    node = getLeafSibling_1.getLeafSibling(region.rootNode, node, isNext, region.skipTags, true /*ignoreSpace*/);
    node = getRootListNode_1.default(region, ListSelector, node);
    if (isNodeInRegion_1.default(region, node) && getListTypeFromNode_1.isListElement(node)) {
        if (isNext) {
            if (!safeInstanceOf_1.default(node, 'HTMLOListElement') || node.start == 1) {
                // Only include sibling list when
                // 1. This is a unordered list, OR
                // 2. This list starts from 1
                nodes.push(node);
            }
        }
        else {
            nodes.unshift(node);
        }
    }
}
function createVListFromItemNode(node) {
    // Wrap all child nodes under a single one, and put the new list under original root node
    // so that the list can carry over styles under the root node.
    var childNodes = toArray_1.default(node.childNodes);
    var nodeForItem = childNodes.length == 1 ? childNodes[0] : wrap_1.default(childNodes, 'SPAN');
    // Create a temporary OL root element for this list.
    var listNode = node.ownerDocument.createElement('ol'); // Either OL or UL is ok here
    node.appendChild(listNode);
    // Create the VList and append items
    var vList = new VList_1.default(listNode);
    vList.appendItem(nodeForItem, 0 /* None */);
    return vList;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/getListTypeFromNode.ts":
/*!***********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/getListTypeFromNode.ts ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
function getListTypeFromNode(node) {
    switch (getTagOfNode_1.default(node)) {
        case 'OL':
            return 1 /* Ordered */;
        case 'UL':
            return 2 /* Unordered */;
        default:
            return 0 /* None */;
    }
}
exports.default = getListTypeFromNode;
/**
 * @internal
 * Check if the given DOM node is a list element (OL or UL)
 * @param node The node to check
 */
function isListElement(node) {
    return getListTypeFromNode(node) != 0 /* None */;
}
exports.isListElement = isListElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/list/getRootListNode.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/list/getRootListNode.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var findClosestElementAncestor_1 = __webpack_require__(/*! ../utils/findClosestElementAncestor */ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts");
/**
 * @internal
 * Get Root list node from the given node within the given region
 * @param region Region to scope the search inot
 * @param selector The selector to search
 * @param node The start node
 */
function getRootListNode(region, selector, node) {
    var list = region &&
        findClosestElementAncestor_1.default(node, region.rootNode, selector);
    if (list) {
        var ancestor = void 0;
        while ((ancestor = findClosestElementAncestor_1.default(list.parentNode, region.rootNode, selector))) {
            list = ancestor;
        }
    }
    return list;
}
exports.default = getRootListNode;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/collapseNodesInRegion.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/collapseNodesInRegion.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var collapseNodes_1 = __webpack_require__(/*! ../utils/collapseNodes */ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts");
var isNodeInRegion_1 = __webpack_require__(/*! ./isNodeInRegion */ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
/**
 * Collapse nodes within this region to their common ascenstor node under this region
 * @param region The region to collapse nodes in.
 * @param nodesOrBlockElements Nodes or block elements to collapse. When take BlockElement[] as input,
 * start node of the first BlockElement and end node of the last BlockElement will be used as the nodes.
 * All nodes not contained by the given region will be ignored.
 */
function collapseNodesInRegion(region, nodesOrBlockElements) {
    if (!nodesOrBlockElements || nodesOrBlockElements.length == 0) {
        return [];
    }
    var nodes = safeInstanceOf_1.default(nodesOrBlockElements[0], 'Node')
        ? nodesOrBlockElements
        : [
            nodesOrBlockElements[0].getStartNode(),
            nodesOrBlockElements[nodesOrBlockElements.length - 1].getEndNode(),
        ];
    nodes = nodes && nodes.filter(function (node) { return isNodeInRegion_1.default(region, node); });
    var firstNode = nodes[0];
    var lastNode = nodes[nodes.length - 1];
    if (isNodeInRegion_1.default(region, firstNode) && isNodeInRegion_1.default(region, lastNode)) {
        return collapseNodes_1.default(region.rootNode, firstNode, lastNode, true /*canSplitParent*/);
    }
    else {
        return [];
    }
}
exports.default = collapseNodesInRegion;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/getRegionsFromRange.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/getRegionsFromRange.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var findClosestElementAncestor_1 = __webpack_require__(/*! ../utils/findClosestElementAncestor */ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var queryElements_1 = __webpack_require__(/*! ../utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
var regionTypeData_1 = __webpack_require__(/*! ./regionTypeData */ "./packages/roosterjs-editor-dom/lib/region/regionTypeData.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * Get regions impacted by the given range under the root node
 * @param root Root node to get regions from
 * @param range A selection range. Regions will be created acording to this range. Each region will be
 * fully or partially covered by this range.
 * @param type Type of region. Currently we only support TABLE region.
 */
function getRegionsFromRange(root, range, type) {
    var regions = [];
    if (root && range) {
        var _a = regionTypeData_1.default[type], innerSelector = _a.innerSelector, skipTags = _a.skipTags;
        var boundaryTree = buildBoundaryTree(root, range, type);
        var start = findClosestElementAncestor_1.default(range.startContainer, root, innerSelector) || root;
        var end = findClosestElementAncestor_1.default(range.endContainer, root, innerSelector) || root;
        var creator = getRegionCreator(range, skipTags);
        regions = iterateNodes(creator, boundaryTree, start, end)[0];
    }
    return regions.filter(function (r) { return !!r; });
}
exports.default = getRegionsFromRange;
/**
 * @internal export for test only
 */
function getRegionCreator(fullRange, skipTags) {
    var fullSelectionStart = Position_1.default.getStart(fullRange).normalize();
    var fullSelectionEnd = Position_1.default.getEnd(fullRange).normalize();
    return function (rootNode, nodeBefore, nodeAfter) {
        return areNodesValid(rootNode, nodeBefore, nodeAfter, skipTags)
            ? {
                rootNode: rootNode,
                nodeBefore: nodeBefore,
                nodeAfter: nodeAfter,
                skipTags: skipTags,
                fullSelectionStart: fullSelectionStart,
                fullSelectionEnd: fullSelectionEnd,
            }
            : null;
    };
}
exports.getRegionCreator = getRegionCreator;
/**
 * Step 1: Build boundary tree
 * @param root Root node of the whole scope, normally this will be the root of editable scope
 * @param range Existing selected full range
 * @param type Type of region to create
 */
function buildBoundaryTree(root, range, type) {
    var allBoundaries = [{ innerNode: root, children: [] }];
    var _a = regionTypeData_1.default[type], outerSelector = _a.outerSelector, innerSelector = _a.innerSelector;
    var inSelectionOuterNode = queryElements_1.default(root, outerSelector, null /*callback*/, 2 /* InSelection */, range);
    // According to https://www.w3.org/TR/selectors-api/#queryselectorall, the result of querySelectorAll
    // is in document order, which is what we expect. So we don't need to sort the result here.
    queryElements_1.default(root, innerSelector, function (thisInnerNode) {
        var thisOuterNode = findClosestElementAncestor_1.default(thisInnerNode, root, outerSelector);
        if (thisOuterNode && inSelectionOuterNode.indexOf(thisOuterNode) < 0) {
            var boundary = { innerNode: thisInnerNode, children: [] };
            for (var i = allBoundaries.length - 1; i >= 0; i--) {
                var _a = allBoundaries[i], innerNode = _a.innerNode, children = _a.children;
                if (contains_1.default(innerNode, thisOuterNode)) {
                    var child = children.filter(function (c) { return c.outerNode == thisOuterNode; })[0];
                    if (!child) {
                        child = { outerNode: thisOuterNode, boundaries: [] };
                        children.push(child);
                    }
                    child.boundaries.push(boundary);
                    break;
                }
            }
            allBoundaries.push(boundary);
        }
    }, 1 /* OnSelection */, range);
    return allBoundaries[0];
}
/**
 * Step 2: Recursively iterate all boundaries and create regions
 * @param creator A region creator function to help create region
 * @param boundary Current root boundary
 * @param start A node where full range start from. This may not be the direct node container of range.startContenter.
 * It is the nearest ancestor which satisfies the InnerSelector of the given region type
 * @param end A node where full range end from. This may not be the direct node container of range.endContenter.
 * It is the nearest ancestor which satisfies the InnerSelector of the given region type
 * @param started Whether we have already hit the start node
 */
function iterateNodes(creator, boundary, start, end, started) {
    var _a;
    started = started || boundary.innerNode == start;
    var ended = false;
    var children = boundary.children, innerNode = boundary.innerNode;
    var regions = [];
    if (children.length == 0) {
        regions.push(creator(innerNode));
    }
    else {
        // Need to run one more time to add region after all children
        for (var i = 0; i <= children.length && !ended; i++) {
            var _b = children[i] || {}, outerNode = _b.outerNode, boundaries = _b.boundaries;
            var previousOuterNode = (_a = children[i - 1]) === null || _a === void 0 ? void 0 : _a.outerNode;
            if (started) {
                regions.push(creator(innerNode, previousOuterNode, outerNode));
            }
            boundaries === null || boundaries === void 0 ? void 0 : boundaries.forEach(function (child) {
                var _a;
                var newRegions;
                _a = iterateNodes(creator, child, start, end, started), newRegions = _a[0], started = _a[1], ended = _a[2];
                regions = regions.concat(newRegions);
            });
        }
    }
    return [regions, started, ended || innerNode == end];
}
/**
 * Check if the given nodes combination is valid to create a region.
 * A combination is valid when:
 * 1. Root node is not null and is not empty. And
 * 2. For nodeBefore and nodeAfter, each of them should be either null or contained by root node. And
 * 3. If none of nodeBefore and nodeAfter is null, the should not contain each other, and there should be
 * node between them.
 * @param root Root node of region
 * @param nodeBefore The boundary node before the region under root
 * @param nodeAfter The boundary node after the region under root
 * @param skipTags Tags to skip
 */
function areNodesValid(root, nodeBefore, nodeAfter, skipTags) {
    if (!root) {
        return false;
    }
    else {
        var firstNodeOfRegion = nodeBefore && getLeafSibling_1.getNextLeafSibling(root, nodeBefore, skipTags);
        var lastNodeOfRegion = nodeAfter && getLeafSibling_1.getPreviousLeafSibling(root, nodeAfter, skipTags);
        var firstNodeValid = !nodeBefore || (contains_1.default(root, nodeBefore) && contains_1.default(root, firstNodeOfRegion));
        var lastNodeValid = !nodeAfter || (contains_1.default(root, nodeAfter) && contains_1.default(root, lastNodeOfRegion));
        var bothValid = !nodeBefore ||
            !nodeAfter ||
            (!contains_1.default(nodeBefore, nodeAfter, true /*treatSameAsContain*/) &&
                !contains_1.default(nodeBefore, lastNodeOfRegion, true /*treatSameAsContain*/) &&
                !contains_1.default(nodeAfter, nodeBefore, true /*treatSameAsContain*/) &&
                !contains_1.default(nodeAfter, firstNodeOfRegion, true /*treatSameAsContain*/));
        return firstNodeValid && lastNodeValid && bothValid;
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/getSelectedBlockElementsInRegion.ts":
/*!**************************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/getSelectedBlockElementsInRegion.ts ***!
  \**************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContentTraverser_1 = __webpack_require__(/*! ../contentTraverser/ContentTraverser */ "./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts");
var fromHtml_1 = __webpack_require__(/*! ../utils/fromHtml */ "./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts");
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getSelectionRangeInRegion_1 = __webpack_require__(/*! ./getSelectionRangeInRegion */ "./packages/roosterjs-editor-dom/lib/region/getSelectionRangeInRegion.ts");
var shouldSkipNode_1 = __webpack_require__(/*! ../utils/shouldSkipNode */ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts");
/**
 * Get all block elements covered by the selection under this region
 * @param regionBase The region to get block elements from
 * @param createBlockIfEmpty When set to true, a new empty block element will be created if there is not
 * any blocks in the region. Default value is false
 */
function getSelectedBlockElementsInRegion(regionBase, createBlockIfEmpty) {
    var range = getSelectionRangeInRegion_1.default(regionBase);
    var blocks = [];
    if (range) {
        var rootNode = regionBase.rootNode, skipTags = regionBase.skipTags;
        var traverser = ContentTraverser_1.default.createSelectionTraverser(rootNode, range, skipTags);
        for (var block = traverser === null || traverser === void 0 ? void 0 : traverser.currentBlockElement; !!block; block = traverser.getNextBlockElement()) {
            blocks.push(block);
        }
        // Remove unmeaningful nodes
        blocks = blocks.filter(function (block) {
            var _a;
            var startNode = block.getStartNode();
            var endNode = block.getEndNode();
            if (startNode == endNode && shouldSkipNode_1.default(startNode, true /*ignoreSpace*/)) {
                (_a = startNode.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(startNode);
                return false;
            }
            else {
                return true;
            }
        });
    }
    if (blocks.length == 0 && regionBase && !regionBase.rootNode.firstChild && createBlockIfEmpty) {
        var newNode = fromHtml_1.default('<div><br></div>', regionBase.rootNode.ownerDocument)[0];
        regionBase.rootNode.appendChild(newNode);
        blocks.push(getBlockElementAtNode_1.default(regionBase.rootNode, newNode));
    }
    return blocks;
}
exports.default = getSelectedBlockElementsInRegion;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/getSelectionRangeInRegion.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/getSelectionRangeInRegion.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var createRange_1 = __webpack_require__(/*! ../selection/createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var getLeafSibling_1 = __webpack_require__(/*! ../utils/getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * Get the selection range in the given region.
 * The original range can cover multiple regions, this function will narrow the origianl selection
 * of a region into current region
 * @param regionBase The region to get range from
 */
function getSelectionRangeInRegion(regionBase) {
    if (!regionBase) {
        return null;
    }
    var nodeBefore = regionBase.nodeBefore, nodeAfter = regionBase.nodeAfter, rootNode = regionBase.rootNode, skipTags = regionBase.skipTags;
    var startNode = nodeBefore
        ? getLeafSibling_1.getNextLeafSibling(regionBase.rootNode, nodeBefore, regionBase.skipTags)
        : rootNode.firstChild;
    var endNode = nodeAfter
        ? getLeafSibling_1.getPreviousLeafSibling(rootNode, nodeAfter, skipTags)
        : rootNode.lastChild;
    var regionRange = startNode && endNode && createRange_1.default(startNode, endNode);
    if (!isRegion(regionBase)) {
        return regionRange;
    }
    else if (regionRange) {
        var regionStart = Position_1.default.getStart(regionRange).normalize();
        var regionEnd = Position_1.default.getEnd(regionRange).normalize();
        var fullSelectionEnd = regionBase.fullSelectionEnd, fullSelectionStart = regionBase.fullSelectionStart;
        if (!fullSelectionStart.isAfter(regionEnd) && !regionStart.isAfter(fullSelectionEnd)) {
            var start = fullSelectionStart.isAfter(regionStart)
                ? fullSelectionStart
                : regionStart;
            var end = fullSelectionEnd.isAfter(regionEnd) ? regionEnd : fullSelectionEnd;
            return createRange_1.default(start, end);
        }
        else {
            return null;
        }
    }
}
exports.default = getSelectionRangeInRegion;
function isRegion(regionBase) {
    var region = regionBase;
    return !!region.fullSelectionEnd && !!region.fullSelectionStart;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
/**
 * Check if a given node is contained by the given region
 * @param region The region to check from
 * @param node The node or block element to check
 */
function isNodeInRegion(region, node) {
    return !!(region &&
        contains_1.default(region.rootNode, node) &&
        (!region.nodeBefore ||
            region.nodeBefore.compareDocumentPosition(node) == 4 /* Following */) &&
        (!region.nodeAfter ||
            region.nodeAfter.compareDocumentPosition(node) == 2 /* Preceding */));
}
exports.default = isNodeInRegion;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/mergeBlocksInRegion.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/mergeBlocksInRegion.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var changeElementTag_1 = __webpack_require__(/*! ../utils/changeElementTag */ "./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts");
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getBlockElementAtNode_1 = __webpack_require__(/*! ../blockElements/getBlockElementAtNode */ "./packages/roosterjs-editor-dom/lib/blockElements/getBlockElementAtNode.ts");
var getPredefinedCssForElement_1 = __webpack_require__(/*! ../htmlSanitizer/getPredefinedCssForElement */ "./packages/roosterjs-editor-dom/lib/htmlSanitizer/getPredefinedCssForElement.ts");
var getStyles_1 = __webpack_require__(/*! ../style/getStyles */ "./packages/roosterjs-editor-dom/lib/style/getStyles.ts");
var isNodeInRegion_1 = __webpack_require__(/*! ../region/isNodeInRegion */ "./packages/roosterjs-editor-dom/lib/region/isNodeInRegion.ts");
var setStyles_1 = __webpack_require__(/*! ../style/setStyles */ "./packages/roosterjs-editor-dom/lib/style/setStyles.ts");
var collapseNodes_1 = __webpack_require__(/*! ../utils/collapseNodes */ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts");
var __1 = __webpack_require__(/*! .. */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Merge a BlockElement of given node after another node
 * @param region Region to operate in
 * @param refNode The node to merge after
 * @param targetNode The node of target block element
 */
function mergeBlocksInRegion(region, refNode, targetNode) {
    var _a, _b;
    var block;
    if (!isNodeInRegion_1.default(region, refNode) ||
        !isNodeInRegion_1.default(region, targetNode) ||
        !(block = getBlockElementAtNode_1.default(region.rootNode, targetNode)) ||
        block.contains(refNode)) {
        return;
    }
    var blockRoot = block.collapseToSingleElement();
    var commonContainer = collapseNodes_1.collapse(region.rootNode, blockRoot, refNode, false /*isStart*/, true /*canSplitParent*/);
    // Copy styles of parent nodes into blockRoot
    for (var node = blockRoot; contains_1.default(commonContainer, node);) {
        var parent_1 = node.parentNode;
        if (__1.safeInstanceOf(parent_1, 'HTMLElement')) {
            var styles = __assign(__assign(__assign({}, (getPredefinedCssForElement_1.default(parent_1) || {})), getStyles_1.default(parent_1)), getStyles_1.default(blockRoot));
            setStyles_1.default(blockRoot, styles);
        }
        node = parent_1;
    }
    var nodeToRemove = null;
    var nodeToMerge = blockRoot.childNodes.length == 1 && blockRoot.attributes.length == 0
        ? blockRoot.firstChild
        : changeElementTag_1.default(blockRoot, 'SPAN');
    // Remove empty node
    for (var node = nodeToMerge; contains_1.default(commonContainer, node) && node.parentNode.childNodes.length == 1; node = node.parentNode) {
        // If the only child is the one which is about to be removed, this node should also be removed
        nodeToRemove = node.parentNode;
    }
    // Finally, merge blocks, and remove empty nodes
    (_a = refNode.parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(nodeToMerge, refNode.nextSibling);
    (_b = nodeToRemove === null || nodeToRemove === void 0 ? void 0 : nodeToRemove.parentNode) === null || _b === void 0 ? void 0 : _b.removeChild(nodeToRemove);
}
exports.default = mergeBlocksInRegion;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/region/regionTypeData.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/region/regionTypeData.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var _a;
Object.defineProperty(exports, "__esModule", { value: true });
var regionTypeData = (_a = {},
    _a[0 /* Table */] = {
        skipTags: ['TABLE'],
        outerSelector: 'table',
        innerSelector: 'td,th',
    },
    _a);
/**
 * @internal
 */
exports.default = regionTypeData;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/Position.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/Position.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var findClosestElementAncestor_1 = __webpack_require__(/*! ../utils/findClosestElementAncestor */ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts");
var isNodeAfter_1 = __webpack_require__(/*! ../utils/isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
/**
 * Represent a position in DOM tree by the node and its offset index
 */
var Position = /** @class */ (function () {
    function Position(nodeOrPosition, offsetOrPosType, isFromEndOfRange) {
        this.isFromEndOfRange = isFromEndOfRange;
        if (nodeOrPosition.node) {
            this.node = nodeOrPosition.node;
            offsetOrPosType = nodeOrPosition.offset;
        }
        else {
            this.node = nodeOrPosition;
        }
        switch (offsetOrPosType) {
            case -2 /* Before */:
                this.offset = getIndexOfNode(this.node);
                this.node = this.node.parentNode;
                this.isAtEnd = false;
                break;
            case -3 /* After */:
                this.offset = getIndexOfNode(this.node) + 1;
                this.isAtEnd = !this.node.nextSibling;
                this.node = this.node.parentNode;
                break;
            case -1 /* End */:
                this.offset = getEndOffset(this.node);
                this.isAtEnd = true;
                break;
            default:
                var endOffset = getEndOffset(this.node);
                this.offset = Math.max(0, Math.min(offsetOrPosType, endOffset));
                this.isAtEnd = offsetOrPosType > 0 && offsetOrPosType >= endOffset;
                break;
        }
        this.element = findClosestElementAncestor_1.default(this.node);
    }
    /**
     * Normalize this position to the leaf node, return the normalize result.
     * If current position is already using leaf node, return this position object itself
     */
    Position.prototype.normalize = function () {
        if (this.node.nodeType == 3 /* Text */ || !this.node.firstChild) {
            return this;
        }
        var node = this.node;
        var newOffset = this.isAtEnd
            ? -1 /* End */
            : this.offset;
        while (node.nodeType == 1 /* Element */ || node.nodeType == 11 /* DocumentFragment */) {
            var nextNode = this.isFromEndOfRange
                ? newOffset == -1 /* End */
                    ? node.lastChild
                    : node.childNodes[newOffset - 1]
                : newOffset == 0 /* Begin */
                    ? node.firstChild
                    : newOffset == -1 /* End */
                        ? node.lastChild
                        : node.childNodes[newOffset];
            if (nextNode) {
                node = nextNode;
                newOffset =
                    this.isAtEnd || this.isFromEndOfRange ? -1 /* End */ : 0 /* Begin */;
            }
            else {
                break;
            }
        }
        return new Position(node, newOffset, this.isFromEndOfRange);
    };
    /**
     * Check if this position is equal to the given position
     * @param position The position to check
     */
    Position.prototype.equalTo = function (position) {
        return (position &&
            (this == position ||
                (this.node == position.node &&
                    this.offset == position.offset &&
                    this.isAtEnd == position.isAtEnd)));
    };
    /**
     * Checks if this position is after the given position
     */
    Position.prototype.isAfter = function (position) {
        return this.node == position.node
            ? (this.isAtEnd && !position.isAtEnd) || this.offset > position.offset
            : isNodeAfter_1.default(this.node, position.node);
    };
    /**
     * Move this position with offset, returns a new position with a valid offset in the same node
     * @param offset Offset to move with
     */
    Position.prototype.move = function (offset) {
        return new Position(this.node, Math.max(this.offset + offset, 0));
    };
    /**
     * Get start position of the given Range
     * @param range The range to get position from
     */
    Position.getStart = function (range) {
        return new Position(range.startContainer, range.startOffset);
    };
    /**
     * Get end position of the given Range
     * @param range The range to get position from
     */
    Position.getEnd = function (range) {
        // For collapsed range, always return the same value of start container to make sure
        // end position is not before start position
        return range.collapsed
            ? Position.getStart(range)
            : new Position(range.endContainer, range.endOffset, true /*isFromEndOfRange*/);
    };
    return Position;
}());
exports.default = Position;
function getIndexOfNode(node) {
    var i = 0;
    while ((node = node.previousSibling)) {
        i++;
    }
    return i;
}
function getEndOffset(node) {
    if (node.nodeType == 3 /* Text */) {
        return node.nodeValue.length;
    }
    else if (node.nodeType == 1 /* Element */) {
        return node.childNodes.length;
    }
    else {
        return 1;
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/addRangeToSelection.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/addRangeToSelection.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Browser_1 = __webpack_require__(/*! ../utils/Browser */ "./packages/roosterjs-editor-dom/lib/utils/Browser.ts");
/**
 * Add the given range into selection of the given document
 * @param range The range to select
 * @param skipSameRange When set to true, do nothing if the given range is the same with current selection,
 * otherwise it will always remove current selection ranage and set to the given one.
 * This parameter is always treat as true in Edge to avoid some weird runtime exception.
 */
function addRangeToSelection(range, skipSameRange) {
    var _a, _b, _c;
    var selection = (_c = (_b = (_a = range === null || range === void 0 ? void 0 : range.commonAncestorContainer) === null || _a === void 0 ? void 0 : _a.ownerDocument) === null || _b === void 0 ? void 0 : _b.defaultView) === null || _c === void 0 ? void 0 : _c.getSelection();
    if (selection) {
        var needAddRange = true;
        if (selection.rangeCount > 0) {
            // Workaround IE exception 800a025e
            try {
                var currentRange = void 0;
                // Do not remove/add range if current selection is the same with target range
                // Without this check, execCommand() may fail in Edge since we changed the selection
                if ((skipSameRange || Browser_1.Browser.isEdge) &&
                    (currentRange = selection.rangeCount == 1 ? selection.getRangeAt(0) : null) &&
                    currentRange.startContainer == range.startContainer &&
                    currentRange.startOffset == range.startOffset &&
                    currentRange.endContainer == range.endContainer &&
                    currentRange.endOffset == range.endOffset) {
                    needAddRange = false;
                }
                else {
                    selection.removeAllRanges();
                }
            }
            catch (e) { }
        }
        if (needAddRange) {
            selection.addRange(range);
        }
    }
}
exports.default = addRangeToSelection;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/createRange.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var isVoidHtmlElement_1 = __webpack_require__(/*! ../utils/isVoidHtmlElement */ "./packages/roosterjs-editor-dom/lib/utils/isVoidHtmlElement.ts");
var Position_1 = __webpack_require__(/*! ./Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
function createRange(arg1, arg2, arg3, arg4) {
    var start;
    var end;
    if (isNodePosition(arg1)) {
        // function createRange(startPosition: NodePosition, endPosition?: NodePosition): Range;
        start = arg1;
        end = isNodePosition(arg2) ? arg2 : null;
    }
    else if (safeInstanceOf_1.default(arg1, 'Node')) {
        if (Array.isArray(arg2)) {
            // function createRange(rootNode: Node, startPath: number[], endPath?: number[]): Range;
            start = getPositionFromPath(arg1, arg2);
            end = Array.isArray(arg3) ? getPositionFromPath(arg1, arg3) : null;
        }
        else if (typeof arg2 == 'number') {
            // function createRange(node: Node, offset: number | PositionType): Range;
            // function createRange(startNode: Node, startOffset: number | PositionType, endNode: Node, endOffset: number | PositionType): Range;
            start = new Position_1.default(arg1, arg2);
            end = safeInstanceOf_1.default(arg3, 'Node') ? new Position_1.default(arg3, arg4) : null;
        }
        else if (safeInstanceOf_1.default(arg2, 'Node') || !arg2) {
            // function createRange(startNode: Node, endNode?: Node): Range;
            start = new Position_1.default(arg1, -2 /* Before */);
            end = new Position_1.default(arg2 || arg1, -3 /* After */);
        }
    }
    if (start && start.node) {
        var range = start.node.ownerDocument.createRange();
        start = getFocusablePosition(start);
        end = getFocusablePosition(end || start);
        range.setStart(start.node, start.offset);
        range.setEnd(end.node, end.offset);
        return range;
    }
    else {
        return null;
    }
}
exports.default = createRange;
/**
 * Convert to focusable position
 * If current node is a void element, we need to move up one level to put cursor outside void element
 */
function getFocusablePosition(position) {
    return position.node.nodeType == 1 /* Element */ && isVoidHtmlElement_1.default(position.node)
        ? new Position_1.default(position.node, position.isAtEnd ? -3 /* After */ : -2 /* Before */)
        : position;
}
function isNodePosition(arg) {
    return arg && arg.node;
}
function getPositionFromPath(node, path) {
    if (!node || !path) {
        return null;
    }
    // Iterate with a for loop to avoid mutating the passed in element path stack
    // or needing to copy it.
    var offset;
    for (var i = 0; i < path.length; i++) {
        offset = path[i];
        if (i < path.length - 1 &&
            node &&
            node.nodeType == 1 /* Element */ &&
            node.childNodes.length > offset) {
            node = node.childNodes[offset];
        }
        else {
            break;
        }
    }
    return new Position_1.default(node, offset);
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/deleteSelectedContent.ts":
/*!******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/deleteSelectedContent.ts ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var arrayPush_1 = __webpack_require__(/*! ../utils/arrayPush */ "./packages/roosterjs-editor-dom/lib/utils/arrayPush.ts");
var collapseNodesInRegion_1 = __webpack_require__(/*! ../region/collapseNodesInRegion */ "./packages/roosterjs-editor-dom/lib/region/collapseNodesInRegion.ts");
var getRegionsFromRange_1 = __webpack_require__(/*! ../region/getRegionsFromRange */ "./packages/roosterjs-editor-dom/lib/region/getRegionsFromRange.ts");
var getSelectionRangeInRegion_1 = __webpack_require__(/*! ../region/getSelectionRangeInRegion */ "./packages/roosterjs-editor-dom/lib/region/getSelectionRangeInRegion.ts");
var mergeBlocksInRegion_1 = __webpack_require__(/*! ../region/mergeBlocksInRegion */ "./packages/roosterjs-editor-dom/lib/region/mergeBlocksInRegion.ts");
var Position_1 = __webpack_require__(/*! ./Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var queryElements_1 = __webpack_require__(/*! ../utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var splitTextNode_1 = __webpack_require__(/*! ../utils/splitTextNode */ "./packages/roosterjs-editor-dom/lib/utils/splitTextNode.ts");
/**
 * Delete selected content, and return the new positon to select
 * @param core The EditorCore object.
 * @param range The range to delete
 */
function deleteSelectedContent(root, range) {
    var nodeBefore = null;
    // 1. TABLE and TR node in selected should be deleted. It is possible we don't detecte them from step 2
    // since table cells will fall in to different regions
    var nodesToDelete = queryElements_1.default(root, 'table,tr', null /*callback*/, 2 /* InSelection */, range);
    // 2. Loop all selected regions, find out those nodes need to be deleted and merged.
    // We don't delete them directly here because delete node from one region may cause selection range
    // another region becomes invalid. So we delay the process of deletion.
    var regions = getRegionsFromRange_1.default(root, range, 0 /* Table */);
    var nodesPairToMerge = regions
        .map(function (region) {
        var regionRange = getSelectionRangeInRegion_1.default(region);
        if (!regionRange) {
            return null;
        }
        var startContainer = regionRange.startContainer, endContainer = regionRange.endContainer, startOffset = regionRange.startOffset, endOffset = regionRange.endOffset;
        // Make sure there are node before and after the merging point.
        // This is required by mergeBlocksInRegion API.
        // This may create some empty text node as anchor
        var _a = ensureBeforeAndAfter(endContainer, endOffset, false /*isStart*/), beforeEnd = _a[0], afterEnd = _a[1];
        var _b = ensureBeforeAndAfter(startContainer, startOffset, true /*isStart*/), beforeStart = _b[0], afterStart = _b[1];
        nodeBefore = nodeBefore || beforeStart;
        // Find out all nodes to be deleted
        var nodes = collapseNodesInRegion_1.default(region, [afterStart, beforeEnd]);
        arrayPush_1.default(nodesToDelete, nodes);
        return { region: region, beforeStart: beforeStart, afterEnd: afterEnd };
    })
        .filter(function (x) { return !!x; });
    // 3. Delete all nodes that we found
    nodesToDelete.forEach(function (node) { var _a; return (_a = node.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(node); });
    // 4. Merge lines for each region, so that after we don't see extra line breaks
    nodesPairToMerge.forEach(function (nodes) {
        return mergeBlocksInRegion_1.default(nodes.region, nodes.beforeStart, nodes.afterEnd);
    });
    return nodeBefore && new Position_1.default(nodeBefore, -1 /* End */);
}
exports.default = deleteSelectedContent;
function ensureBeforeAndAfter(node, offset, isStart) {
    var _a;
    if (safeInstanceOf_1.default(node, 'Text')) {
        var newNode = splitTextNode_1.default(node, offset, isStart);
        return isStart ? [newNode, node] : [node, newNode];
    }
    else {
        var nodeBefore = node.childNodes[offset - 1];
        var nodeAfter = node.childNodes[offset];
        // Condition 1: node child nodes
        // ("I" means cursor; "o" means a DOM node, "[ ]" means a parent node)
        // [ I ]
        // need to use parent node instead to convert to condition 2
        if (!nodeBefore && !nodeAfter) {
            if (isStart) {
                nodeAfter = node;
                nodeBefore = nodeAfter.previousSibling;
            }
            else {
                nodeBefore = node;
                nodeAfter = nodeBefore.nextSibling;
            }
        }
        // Condition 2: Either nodeBefore or nodeAfter is null (XOR case)
        // [ o I ]  or [ I o]
        // need to add empty text node to convert to condition 3
        if ((nodeBefore || nodeAfter) && (!nodeBefore || !nodeAfter)) {
            var emptyNode = node.ownerDocument.createTextNode('');
            (_a = (nodeBefore || nodeAfter).parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(emptyNode, nodeAfter);
            if (nodeBefore) {
                nodeAfter = emptyNode;
            }
            else {
                nodeBefore = emptyNode;
            }
        }
        // Condition 3: Both nodeBefore and nodeAfter are not null
        // [o I o]
        // return the nodes
        return [nodeBefore, nodeAfter];
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/getHtmlWithSelectionPath.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/getHtmlWithSelectionPath.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getInnerHTML_1 = __webpack_require__(/*! ../utils/getInnerHTML */ "./packages/roosterjs-editor-dom/lib/utils/getInnerHTML.ts");
var getSelectionPath_1 = __webpack_require__(/*! ./getSelectionPath */ "./packages/roosterjs-editor-dom/lib/selection/getSelectionPath.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var queryElements_1 = __webpack_require__(/*! ../utils/queryElements */ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts");
/**
 * Get inner Html of a root node with a selection path which can be used for restore selection.
 * The result string can be used by setHtmlWithSelectionPath() to restore the HTML and selection.
 * @param rootNode Root node to get inner Html from
 * @param range The range of selection. If pass null, no selection path will be added
 * @returns Inner HTML of the root node, followed by HTML comment contains selection path if the given range is valid
 */
function getHtmlWithSelectionPath(rootNode, range) {
    if (!rootNode) {
        return '';
    }
    var _a = range || {}, startContainer = _a.startContainer, endContainer = _a.endContainer, startOffset = _a.startOffset, endOffset = _a.endOffset;
    var isDOMChanged = false;
    queryElements_1.default(rootNode, 'table', function (table) {
        var tbody = null;
        for (var child = table.firstChild; child; child = child.nextSibling) {
            if (getTagOfNode_1.default(child) == 'TR') {
                if (!tbody) {
                    tbody = table.ownerDocument.createElement('tbody');
                    table.insertBefore(tbody, child);
                }
                tbody.appendChild(child);
                child = tbody;
                isDOMChanged = true;
            }
            else {
                tbody = null;
            }
        }
    });
    if (range && isDOMChanged) {
        try {
            range.setStart(startContainer, startOffset);
            range.setEnd(endContainer, endOffset);
        }
        catch (_b) { }
    }
    var content = getInnerHTML_1.default(rootNode);
    var selectionPath = range && getSelectionPath_1.default(rootNode, range);
    return selectionPath ? content + "<!--" + JSON.stringify(selectionPath) + "-->" : content;
}
exports.default = getHtmlWithSelectionPath;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/getPositionRect.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/getPositionRect.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var createRange_1 = __webpack_require__(/*! ./createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
var normalizeRect_1 = __webpack_require__(/*! ../utils/normalizeRect */ "./packages/roosterjs-editor-dom/lib/utils/normalizeRect.ts");
/**
 * Get bounding rect of this position
 * @param position The positioin to get rect from
 */
function getPositionRect(position) {
    if (!position) {
        return null;
    }
    var range = createRange_1.default(position);
    // 1) try to get rect using range.getBoundingClientRect()
    var rect = range.getBoundingClientRect && normalizeRect_1.default(range.getBoundingClientRect());
    if (rect) {
        return rect;
    }
    // 2) try to get rect using range.getClientRects
    position = position.normalize();
    var rects = range.getClientRects && range.getClientRects();
    rect = rects && rects.length == 1 && normalizeRect_1.default(rects[0]);
    if (rect) {
        return rect;
    }
    // 3) if node is text node, try inserting a SPAN and get the rect of SPAN for others
    if (position.node.nodeType == 3 /* Text */) {
        var document_1 = position.node.ownerDocument;
        var span = document_1.createElement('SPAN');
        span.innerHTML = '\u200b';
        range = createRange_1.default(position);
        range.insertNode(span);
        rect = span.getBoundingClientRect && normalizeRect_1.default(span.getBoundingClientRect());
        span.parentNode.removeChild(span);
        if (rect) {
            return rect;
        }
    }
    // 4) try getBoundingClientRect on element
    var element = position.element;
    if (element && element.getBoundingClientRect) {
        rect = normalizeRect_1.default(element.getBoundingClientRect());
        if (rect) {
            return rect;
        }
    }
    return null;
}
exports.default = getPositionRect;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/getSelectionPath.ts":
/*!*************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/getSelectionPath.ts ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var Position_1 = __webpack_require__(/*! ./Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
/**
 * Get path of the given selection range related to the given rootNode
 * @param rootNode The root node where the path start from
 * @param range The range of selection
 */
function getSelectionPath(rootNode, range) {
    if (!range) {
        return null;
    }
    var selectionPath = {
        start: getPositionPath(Position_1.default.getStart(range), rootNode),
        end: getPositionPath(Position_1.default.getEnd(range), rootNode),
    };
    return selectionPath;
}
exports.default = getSelectionPath;
/**
 * Get the path of the node relative to rootNode.
 * The path of the node is an array of integer indecies into the childNodes of the given node.
 *
 * The node path will be what the node path will be on a _normalized_ dom
 * (e.g. empty text nodes will be ignored and adjacent text nodes will be concatenated)
 *
 * @param rootNode the node the path will be relative to
 * @param position the position to get indexes from. Follows the same semantics
 * as selectionRange (if node is of type Text, it is an offset into the text of that node.
 * If node is of type Element, it is the index of a child in that Element node.)
 */
function getPositionPath(position, rootNode) {
    if (!position || !rootNode) {
        return [];
    }
    var node = position.node, offset = position.offset;
    var result = [];
    var parent;
    if (!contains_1.default(rootNode, node, true)) {
        return [];
    }
    if (node.nodeType == 3 /* Text */) {
        parent = node.parentNode;
        while (node.previousSibling && node.previousSibling.nodeType == 3 /* Text */) {
            offset += node.previousSibling.nodeValue.length;
            node = node.previousSibling;
        }
        result.unshift(offset);
    }
    else {
        parent = node;
        node = node.childNodes[offset];
    }
    do {
        offset = 0;
        var isPreviousText = false;
        for (var c = parent.firstChild; c && c != node; c = c.nextSibling) {
            if (c.nodeType == 3 /* Text */) {
                if (c.nodeValue.length == 0 || isPreviousText) {
                    continue;
                }
                isPreviousText = true;
            }
            else {
                isPreviousText = false;
            }
            offset++;
        }
        result.unshift(offset);
        node = parent;
        parent = parent.parentNode;
    } while (node && node != rootNode);
    return result;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/isPositionAtBeginningOf.ts":
/*!********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/isPositionAtBeginningOf.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ../utils/contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getTagOfNode_1 = __webpack_require__(/*! ../utils/getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var isNodeEmpty_1 = __webpack_require__(/*! ../utils/isNodeEmpty */ "./packages/roosterjs-editor-dom/lib/utils/isNodeEmpty.ts");
/**
 * Check if this position is at beginning of the given node.
 * This will return true if all nodes between the beginning of target node and the position are empty.
 * @param position The position to check
 * @param targetNode The node to check
 * @returns True if position is at beginning of the node, otherwise false
 */
function isPositionAtBeginningOf(position, targetNode) {
    if (position) {
        var _a = position.normalize(), node = _a.node, offset = _a.offset;
        if (offset == 0) {
            while (contains_1.default(targetNode, node) && areAllPrevousNodesEmpty(node)) {
                node = node.parentNode;
            }
            return node == targetNode;
        }
    }
    return false;
}
exports.default = isPositionAtBeginningOf;
function areAllPrevousNodesEmpty(node) {
    while (node.previousSibling) {
        node = node.previousSibling;
        if (getTagOfNode_1.default(node) == 'BR' || !isNodeEmpty_1.default(node)) {
            return false;
        }
    }
    return true;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/selection/setHtmlWithSelectionPath.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/selection/setHtmlWithSelectionPath.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var createRange_1 = __webpack_require__(/*! ./createRange */ "./packages/roosterjs-editor-dom/lib/selection/createRange.ts");
/**
 * Restore inner Html of a root element from given html string. If the string contains selection path,
 * remove the selection path and return a range represented by the path
 * @param root The root element
 * @param html The html to restore
 * @returns A selection range if the html contains a valid selection path, otherwise null
 */
function setHtmlWithSelectionPath(rootNode, html) {
    rootNode.innerHTML = html || '';
    var path = null;
    var pathComment = rootNode.lastChild;
    try {
        path =
            pathComment &&
                pathComment.nodeType == 8 /* Comment */ &&
                JSON.parse(pathComment.nodeValue);
        if (path && path.end && path.end.length > 0 && path.start && path.start.length > 0) {
            rootNode.removeChild(pathComment);
        }
        else {
            path = null;
        }
    }
    catch (_a) { }
    return path && createRange_1.default(rootNode, path.start, path.end);
}
exports.default = setHtmlWithSelectionPath;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/addSnapshot.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/addSnapshot.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var clearProceedingSnapshots_1 = __webpack_require__(/*! ./clearProceedingSnapshots */ "./packages/roosterjs-editor-dom/lib/snapshots/clearProceedingSnapshots.ts");
/**
 * Add a new snapshot to the given snapshots data structure
 * @param snapshots The snapshots data structure to add new snapshot into
 * @param snapshot The snapshot to add
 * @param isAutoCompleteSnapshot Whether this is a snapshot before auto complete action
 */
function addSnapshot(snapshots, snapshot, isAutoCompleteSnapshot) {
    if (snapshots.currentIndex < 0 || snapshot != snapshots.snapshots[snapshots.currentIndex]) {
        clearProceedingSnapshots_1.default(snapshots);
        snapshots.snapshots.push(snapshot);
        snapshots.currentIndex++;
        snapshots.totalSize += snapshot.length;
        var removeCount = 0;
        while (removeCount < snapshots.snapshots.length &&
            snapshots.totalSize > snapshots.maxSize) {
            snapshots.totalSize -= snapshots.snapshots[removeCount].length;
            removeCount++;
        }
        if (removeCount > 0) {
            snapshots.snapshots.splice(0, removeCount);
            snapshots.currentIndex -= removeCount;
            snapshots.autoCompleteIndex -= removeCount;
        }
        if (isAutoCompleteSnapshot) {
            snapshots.autoCompleteIndex = snapshots.currentIndex;
        }
    }
}
exports.default = addSnapshot;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/canMoveCurrentSnapshot.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/canMoveCurrentSnapshot.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Check whether can move current snapshot with the given step
 * @param snapshots The snapshots data structure to check
 * @param step The step to check, can be positive, negative or 0
 * @returns True if can move current snapshot with the given step, otherwise false
 */
function canMoveCurrentSnapshot(snapshots, step) {
    var newIndex = snapshots.currentIndex + step;
    return newIndex >= 0 && newIndex < snapshots.snapshots.length;
}
exports.default = canMoveCurrentSnapshot;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/canUndoAutoComplete.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/canUndoAutoComplete.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Whether there is a snapshot added before auto complete and it can be undone now
 */
function canUndoAutoComplete(snapshots) {
    return (snapshots.autoCompleteIndex >= 0 &&
        snapshots.currentIndex - snapshots.autoCompleteIndex == 1);
}
exports.default = canUndoAutoComplete;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/clearProceedingSnapshots.ts":
/*!*********************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/clearProceedingSnapshots.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var canMoveCurrentSnapshot_1 = __webpack_require__(/*! ./canMoveCurrentSnapshot */ "./packages/roosterjs-editor-dom/lib/snapshots/canMoveCurrentSnapshot.ts");
/**
 * Clear all snapshots after the current one
 * @param snapshots The snapshots data structure to clear
 */
function clearProceedingSnapshots(snapshots) {
    if (canMoveCurrentSnapshot_1.default(snapshots, 1)) {
        var removedSize = 0;
        for (var i = snapshots.currentIndex + 1; i < snapshots.snapshots.length; i++) {
            removedSize += snapshots.snapshots[i].length;
        }
        snapshots.snapshots.splice(snapshots.currentIndex + 1);
        snapshots.totalSize -= removedSize;
        snapshots.autoCompleteIndex = -1;
    }
}
exports.default = clearProceedingSnapshots;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/createSnapshots.ts":
/*!************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/createSnapshots.ts ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Create initial snapshots
 * @param maxSize max size of all snapshots
 */
function createSnapshots(maxSize) {
    return {
        snapshots: [],
        totalSize: 0,
        currentIndex: -1,
        autoCompleteIndex: -1,
        maxSize: maxSize,
    };
}
exports.default = createSnapshots;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/snapshots/moveCurrentSnapsnot.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/snapshots/moveCurrentSnapsnot.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var canMoveCurrentSnapshot_1 = __webpack_require__(/*! ./canMoveCurrentSnapshot */ "./packages/roosterjs-editor-dom/lib/snapshots/canMoveCurrentSnapshot.ts");
/**
 * Move current snapshot with the given step if can move this step. Otherwise no action and return null
 * @param snapshots The snapshots data structure to move
 * @param step The step to move
 * @returns If can move with the given step, returns the snapshot after move, otherwise null
 */
function moveCurrentSnapsnot(snapshots, step) {
    if (canMoveCurrentSnapshot_1.default(snapshots, step)) {
        snapshots.currentIndex += step;
        snapshots.autoCompleteIndex = -1;
        return snapshots.snapshots[snapshots.currentIndex];
    }
    else {
        return null;
    }
}
exports.default = moveCurrentSnapsnot;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/style/getStyles.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/style/getStyles.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Get CSS styles of a given element in name-value pair format
 * @param element The element to get styles from
 */
function getStyles(element) {
    var result = {};
    var style = (element === null || element === void 0 ? void 0 : element.getAttribute('style')) || '';
    style.split(';').forEach(function (pair) {
        var valueIndex = pair.indexOf(':');
        var name = pair.slice(0, valueIndex);
        var value = pair.slice(valueIndex + 1);
        if (name && value) {
            result[name.trim()] = value.trim();
        }
    });
    return result;
}
exports.default = getStyles;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/style/setStyles.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/style/setStyles.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Set styles to an HTML element. If styles are empty, remove 'style' attribute
 * @param element The element to set styles
 * @param styles The styles to set, in name-value pair format
 */
function setStyles(element, styles) {
    if (element) {
        var style = Object.keys(styles || {})
            .map(function (name) {
            var value = styles[name];
            name = name ? name.trim() : null;
            value = value ? value.trim() : null;
            return name && value ? name + ":" + value : null;
        })
            .filter(function (x) { return x; })
            .join(';');
        if (style) {
            element.setAttribute('style', style);
        }
        else {
            element.removeAttribute('style');
        }
    }
}
exports.default = setStyles;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/table/VTable.ts":
/*!***********************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/table/VTable.ts ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var normalizeRect_1 = __webpack_require__(/*! ../utils/normalizeRect */ "./packages/roosterjs-editor-dom/lib/utils/normalizeRect.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
var toArray_1 = __webpack_require__(/*! ../utils/toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
/**
 * A virtual table class, represent an HTML table, by expand all merged cells to each separated cells
 */
var VTable = /** @class */ (function () {
    /**
     * Create a new instance of VTable object using HTML TABLE or TD node
     * @param node The HTML Table or TD node
     */
    function VTable(node) {
        var _this = this;
        this.trs = [];
        this.table = safeInstanceOf_1.default(node, 'HTMLTableElement') ? node : getTableFromTd(node);
        if (this.table) {
            var currentTd_1 = safeInstanceOf_1.default(node, 'HTMLTableElement') ? null : node;
            var trs = toArray_1.default(this.table.rows);
            this.cells = trs.map(function (row) { return []; });
            trs.forEach(function (tr, rowIndex) {
                _this.trs[rowIndex % 2] = tr;
                for (var sourceCol = 0, targetCol = 0; sourceCol < tr.cells.length; sourceCol++) {
                    // Skip the cells which already initialized
                    for (; _this.cells[rowIndex][targetCol]; targetCol++) { }
                    var td = tr.cells[sourceCol];
                    if (td == currentTd_1) {
                        _this.col = targetCol;
                        _this.row = rowIndex;
                    }
                    for (var colSpan = 0; colSpan < td.colSpan; colSpan++, targetCol++) {
                        for (var rowSpan = 0; rowSpan < td.rowSpan; rowSpan++) {
                            _this.cells[rowIndex + rowSpan][targetCol] = {
                                td: colSpan + rowSpan == 0 ? td : null,
                                spanLeft: colSpan > 0,
                                spanAbove: rowSpan > 0,
                            };
                        }
                    }
                }
            });
        }
    }
    /**
     * Write the virtual table back to DOM tree to represent the change of VTable
     */
    VTable.prototype.writeBack = function () {
        var _this = this;
        if (this.cells) {
            moveChildren(this.table);
            this.cells.forEach(function (row, r) {
                var tr = cloneNode(_this.trs[r % 2] || _this.trs[0]);
                _this.table.appendChild(tr);
                row.forEach(function (cell, c) {
                    if (cell.td) {
                        _this.recalcSpans(r, c);
                        tr.appendChild(cell.td);
                    }
                });
            });
        }
        else if (this.table) {
            this.table.parentNode.removeChild(this.table);
        }
    };
    /**
     * Apply the given table format to this virtual table
     * @param format Table format to apply
     */
    VTable.prototype.applyFormat = function (format) {
        if (!format || !this.table) {
            return;
        }
        this.table.style.borderCollapse = 'collapse';
        this.trs[0].style.backgroundColor = format.bgColorOdd || 'transparent';
        if (this.trs[1]) {
            this.trs[1].style.backgroundColor = format.bgColorEven || 'transparent';
        }
        this.cells.forEach(function (row) {
            return row
                .filter(function (cell) { return cell.td; })
                .forEach(function (cell) {
                cell.td.style.borderTop = getBorderStyle(format.topBorderColor);
                cell.td.style.borderBottom = getBorderStyle(format.bottomBorderColor);
                cell.td.style.borderLeft = getBorderStyle(format.verticalBorderColor);
                cell.td.style.borderRight = getBorderStyle(format.verticalBorderColor);
            });
        });
    };
    /**
     * Edit table with given operation.
     * @param operation Table operation
     */
    VTable.prototype.edit = function (operation) {
        var _this = this;
        if (!this.table) {
            return;
        }
        var currentRow = this.cells[this.row];
        var currentCell = currentRow[this.col];
        switch (operation) {
            case 0 /* InsertAbove */:
                this.cells.splice(this.row, 0, currentRow.map(cloneCell));
                break;
            case 1 /* InsertBelow */:
                var newRow_1 = this.row + this.countSpanAbove(this.row, this.col);
                this.cells.splice(newRow_1, 0, this.cells[newRow_1 - 1].map(function (cell, colIndex) {
                    var nextCell = _this.getCell(newRow_1, colIndex);
                    if (nextCell.spanAbove) {
                        return cloneCell(nextCell);
                    }
                    else if (cell.spanLeft) {
                        var newCell = cloneCell(cell);
                        newCell.spanAbove = false;
                        return newCell;
                    }
                    else {
                        return {
                            td: cloneNode(_this.getTd(_this.row, colIndex)),
                        };
                    }
                }));
                break;
            case 2 /* InsertLeft */:
                this.forEachCellOfCurrentColumn(function (cell, row) {
                    row.splice(_this.col, 0, cloneCell(cell));
                });
                break;
            case 3 /* InsertRight */:
                var newCol_1 = this.col + this.countSpanLeft(this.row, this.col);
                this.forEachCellOfColumn(newCol_1 - 1, function (cell, row, i) {
                    var nextCell = _this.getCell(i, newCol_1);
                    var newCell;
                    if (nextCell.spanLeft) {
                        newCell = cloneCell(nextCell);
                    }
                    else if (cell.spanAbove) {
                        newCell = cloneCell(cell);
                        newCell.spanLeft = false;
                    }
                    else {
                        newCell = {
                            td: cloneNode(_this.getTd(i, _this.col)),
                        };
                    }
                    row.splice(newCol_1, 0, newCell);
                });
                break;
            case 6 /* DeleteRow */:
                this.forEachCellOfCurrentRow(function (cell, i) {
                    var nextCell = _this.getCell(_this.row + 1, i);
                    if (cell.td && cell.td.rowSpan > 1 && nextCell.spanAbove) {
                        nextCell.td = cell.td;
                    }
                });
                this.cells.splice(this.row, 1);
                break;
            case 5 /* DeleteColumn */:
                this.forEachCellOfCurrentColumn(function (cell, row, i) {
                    var nextCell = _this.getCell(i, _this.col + 1);
                    if (cell.td && cell.td.colSpan > 1 && nextCell.spanLeft) {
                        nextCell.td = cell.td;
                    }
                    row.splice(_this.col, 1);
                });
                break;
            case 7 /* MergeAbove */:
            case 8 /* MergeBelow */:
                var rowStep = operation == 7 /* MergeAbove */ ? -1 : 1;
                for (var rowIndex = this.row + rowStep; rowIndex >= 0 && rowIndex < this.cells.length; rowIndex += rowStep) {
                    var cell = this.getCell(rowIndex, this.col);
                    if (cell.td && !cell.spanAbove) {
                        var aboveCell = rowIndex < this.row ? cell : currentCell;
                        var belowCell = rowIndex < this.row ? currentCell : cell;
                        if (aboveCell.td.colSpan == belowCell.td.colSpan) {
                            moveChildren(belowCell.td, aboveCell.td);
                            belowCell.td = null;
                            belowCell.spanAbove = true;
                        }
                        break;
                    }
                }
                break;
            case 9 /* MergeLeft */:
            case 10 /* MergeRight */:
                var colStep = operation == 9 /* MergeLeft */ ? -1 : 1;
                for (var colIndex = this.col + colStep; colIndex >= 0 && colIndex < this.cells[this.row].length; colIndex += colStep) {
                    var cell = this.getCell(this.row, colIndex);
                    if (cell.td && !cell.spanLeft) {
                        var leftCell = colIndex < this.col ? cell : currentCell;
                        var rightCell = colIndex < this.col ? currentCell : cell;
                        if (leftCell.td.rowSpan == rightCell.td.rowSpan) {
                            moveChildren(rightCell.td, leftCell.td);
                            rightCell.td = null;
                            rightCell.spanLeft = true;
                        }
                        break;
                    }
                }
                break;
            case 4 /* DeleteTable */:
                this.cells = null;
                break;
            case 12 /* SplitVertically */:
                if (currentCell.td.rowSpan > 1) {
                    this.getCell(this.row + 1, this.col).td = cloneNode(currentCell.td);
                }
                else {
                    var splitRow = currentRow.map(function (cell) {
                        return {
                            td: cell == currentCell ? cloneNode(cell.td) : null,
                            spanAbove: cell != currentCell,
                            spanLeft: cell.spanLeft,
                        };
                    });
                    this.cells.splice(this.row + 1, 0, splitRow);
                }
                break;
            case 11 /* SplitHorizontally */:
                if (currentCell.td.colSpan > 1) {
                    this.getCell(this.row, this.col + 1).td = cloneNode(currentCell.td);
                }
                else {
                    this.forEachCellOfCurrentColumn(function (cell, row) {
                        row.splice(_this.col + 1, 0, {
                            td: row == currentRow ? cloneNode(cell.td) : null,
                            spanAbove: cell.spanAbove,
                            spanLeft: row != currentRow,
                        });
                    });
                }
                break;
        }
    };
    /**
     * Loop each cell of current column and invoke a callback function
     * @param callback The callback function to invoke
     */
    VTable.prototype.forEachCellOfCurrentColumn = function (callback) {
        this.forEachCellOfColumn(this.col, callback);
    };
    /**
     * Loop each table cell and get all the cells that share the same border from one side
     * The result is an array of table cell elements where the first element is the narrowest td
     * @param borderPos The position of the border
     * @param getLeftCells Get left-hand-side or right-hand-side cells of the border
     *
     * Example, consider having a 3 by 4 table as below with merged and split cells
     *
     *     | 1 | 4 | 7 | 8 |
     *     |   5   |   9   |
     *     |   3   |   10  |
     *
     *  input => borderPos: the 3rd border, getLeftCells: true
     *  output => [4, 5, 3], where the first element (4) is the narrowest cell
     *
     *  input => borderPos: the 3rd border, getLeftCells: false
     *  output => [7, 9, 10], where the first element (7) is the narrowest cell
     *
     *  input => borderPos: the 2nd border, getLeftCells: true
     *  output => [1], where the first element (1) is the narrowest (and only) cell
     *
     *  input => borderPos: the 2nd border, getLeftCells: false
     *  output => [4], where the first element (4) is the narrowest (and only) cell
     */
    VTable.prototype.getCellsWithBorder = function (borderPos, getLeftCells) {
        var cells = [];
        var closestIndex = 0;
        var closestValue = getLeftCells ? -1 : Number.MAX_SAFE_INTEGER;
        for (var i = 0; i < this.cells.length; i++) {
            for (var j = 0; j < this.cells[i].length; j++) {
                var cell = this.getCell(i, j);
                if (cell.td) {
                    var cellRect = normalizeRect_1.default(cell.td.getBoundingClientRect());
                    var found = false;
                    if (getLeftCells) {
                        if (cellRect.right == borderPos) {
                            found = true;
                            if (cellRect.left > closestValue) {
                                closestValue = cellRect.left;
                                closestIndex = cells.length;
                            }
                            cell.td.setAttribute('originalLeftBorder', cellRect.left.toString());
                            cells.push(cell.td);
                        }
                        else if (found) {
                            break;
                        }
                    }
                    else {
                        if (cellRect.left == borderPos) {
                            found = true;
                            if (cellRect.right < closestValue) {
                                closestValue = cellRect.right;
                                closestIndex = cells.length;
                            }
                            cell.td.setAttribute('originalRightBorder', cellRect.right.toString());
                            cells.push(cell.td);
                        }
                        else if (found) {
                            break;
                        }
                    }
                }
            }
        }
        if (cells.length > 0) {
            var temp = cells[0];
            cells[0] = cells[closestIndex];
            cells[closestIndex] = temp;
        }
        return cells;
    };
    /**
     * Loop each cell of current row and invoke a callback function
     * @param callback The callback function to invoke
     */
    VTable.prototype.forEachCellOfCurrentRow = function (callback) {
        this.forEachCellOfRow(this.row, callback);
    };
    /**
     * Get a table cell using its row and column index. This function will always return an object
     * even if the given indexes don't exist in table.
     * @param row The row index
     * @param col The column index
     */
    VTable.prototype.getCell = function (row, col) {
        return (this.cells && this.cells[row] && this.cells[row][col]) || {};
    };
    /**
     * Get current HTML table cell object. If the current table cell is a virtual expanded cell, return its root cell
     */
    VTable.prototype.getCurrentTd = function () {
        return this.getTd(this.row, this.col);
    };
    VTable.prototype.getTd = function (row, col) {
        if (this.cells) {
            row = Math.min(this.cells.length - 1, row);
            col = this.cells[row] ? Math.min(this.cells[row].length - 1, col) : col;
            if (!isNaN(row) && !isNaN(col)) {
                while (row >= 0 && col >= 0) {
                    var cell = this.getCell(row, col);
                    if (cell.td) {
                        return cell.td;
                    }
                    else if (cell.spanLeft) {
                        col--;
                    }
                    else if (cell.spanAbove) {
                        row--;
                    }
                    else {
                        break;
                    }
                }
            }
        }
        return null;
    };
    VTable.prototype.forEachCellOfColumn = function (col, callback) {
        for (var i = 0; i < this.cells.length; i++) {
            callback(this.getCell(i, col), this.cells[i], i);
        }
    };
    VTable.prototype.forEachCellOfRow = function (row, callback) {
        for (var i = 0; i < this.cells[row].length; i++) {
            callback(this.getCell(row, i), i);
        }
    };
    VTable.prototype.recalcSpans = function (row, col) {
        var td = this.getCell(row, col).td;
        if (td) {
            td.colSpan = this.countSpanLeft(row, col);
            td.rowSpan = this.countSpanAbove(row, col);
            if (td.colSpan == 1) {
                td.removeAttribute('colSpan');
            }
            if (td.rowSpan == 1) {
                td.removeAttribute('rowSpan');
            }
        }
    };
    VTable.prototype.countSpanLeft = function (row, col) {
        var result = 1;
        for (var i = col + 1; i < this.cells[row].length; i++) {
            var cell = this.getCell(row, i);
            if (cell.td || !cell.spanLeft) {
                break;
            }
            result++;
        }
        return result;
    };
    VTable.prototype.countSpanAbove = function (row, col) {
        var result = 1;
        for (var i = row + 1; i < this.cells.length; i++) {
            var cell = this.getCell(i, col);
            if (cell.td || !cell.spanAbove) {
                break;
            }
            result++;
        }
        return result;
    };
    return VTable;
}());
exports.default = VTable;
function getTableFromTd(td) {
    var result = td;
    for (; result && result.tagName != 'TABLE'; result = result.parentElement) { }
    return result;
}
function getBorderStyle(style) {
    return 'solid 1px ' + (style || 'transparent');
}
/**
 * Clone a table cell
 * @param cell The cell to clone
 */
function cloneCell(cell) {
    return {
        td: cloneNode(cell.td),
        spanAbove: cell.spanAbove,
        spanLeft: cell.spanLeft,
    };
}
/**
 * Clone a node without its children.
 * @param node The node to clone
 */
function cloneNode(node) {
    var newNode = node ? node.cloneNode(false /*deep*/) : null;
    if (safeInstanceOf_1.default(newNode, 'HTMLTableCellElement')) {
        newNode.removeAttribute('id');
        if (!newNode.firstChild) {
            newNode.appendChild(node.ownerDocument.createElement('br'));
        }
    }
    return newNode;
}
/**
 * Move all children from one node to another
 * @param fromNode The source node to move children from
 * @param toNode Target node. If not passed, children nodes of source node will be removed
 */
function moveChildren(fromNode, toNode) {
    while (fromNode.firstChild) {
        if (toNode) {
            toNode.appendChild(fromNode.firstChild);
        }
        else {
            fromNode.removeChild(fromNode.firstChild);
        }
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/Browser.ts":
/*!************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/Browser.ts ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Get current browser information from user agent string
 * @param userAgent The userAgent string of a browser
 * @param appVersion The appVersion string of a browser
 * @returns The BrowserInfo object calculated from the given userAgent and appVersion
 */
function getBrowserInfo(userAgent, appVersion) {
    // checks whether the browser is running in IE
    // IE11 will use rv in UA instead of MSIE. Unfortunately Firefox also uses this. We should also look for "Trident" to confirm this.
    // There have been cases where companies using older version of IE and custom UserAgents have broken this logic (e.g. IE 10 and KellyServices)
    // therefore we should check that the Trident/rv combo is not just from an older IE browser
    var isIE11OrGreater = userAgent.indexOf('rv:') != -1 && userAgent.indexOf('Trident') != -1;
    var isIE = userAgent.indexOf('MSIE') != -1 || isIE11OrGreater;
    // IE11+ may also have 'Chrome', 'Firefox' and 'Safari' in user agent. But it will have 'trident' as well
    var isChrome = false;
    var isFirefox = false;
    var isSafari = false;
    var isEdge = false;
    var isWebKit = userAgent.indexOf('WebKit') != -1;
    if (!isIE) {
        isChrome = userAgent.indexOf('Chrome') != -1;
        isFirefox = userAgent.indexOf('Firefox') != -1;
        if (userAgent.indexOf('Safari') != -1) {
            // Android and Chrome have Safari in the user string
            isSafari = userAgent.indexOf('Chrome') == -1 && userAgent.indexOf('Android') == -1;
        }
        // Sample Edge UA: Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10121
        isEdge = userAgent.indexOf('Edge') != -1;
        // When it is edge, it should not be chrome or firefox. and it is also not webkit
        if (isEdge) {
            isWebKit = isChrome = isFirefox = false;
        }
    }
    var isMac = appVersion.indexOf('Mac') != -1;
    var isWin = appVersion.indexOf('Win') != -1 || appVersion.indexOf('NT') != -1;
    return {
        isMac: isMac,
        isWin: isWin,
        isWebKit: isWebKit,
        isIE: isIE,
        isIE11OrGreater: isIE11OrGreater,
        isSafari: isSafari,
        isChrome: isChrome,
        isFirefox: isFirefox,
        isEdge: isEdge,
        isIEOrEdge: isIE || isEdge,
    };
}
exports.getBrowserInfo = getBrowserInfo;
/**
 * Browser object contains browser and operating system informations of current environment
 */
exports.Browser = window
    ? getBrowserInfo(window.navigator.userAgent, window.navigator.appVersion)
    : {};


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/applyFormat.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/applyFormat.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Apply format to an HTML element
 * @param element The HTML element to apply format to
 * @param format The format to apply
 */
function applyFormat(element, format, isDarkMode) {
    if (format) {
        var elementStyle = element.style;
        var fontFamily = format.fontFamily, fontSize = format.fontSize, textColor = format.textColor, textColors = format.textColors, backgroundColor = format.backgroundColor, backgroundColors = format.backgroundColors, bold = format.bold, italic = format.italic, underline = format.underline;
        if (fontFamily) {
            elementStyle.fontFamily = fontFamily;
        }
        if (fontSize) {
            elementStyle.fontSize = fontSize;
        }
        if (textColor || textColors) {
            elementStyle.color =
                (isDarkMode ? textColors === null || textColors === void 0 ? void 0 : textColors.darkModeColor : textColors === null || textColors === void 0 ? void 0 : textColors.lightModeColor) || textColor;
            if (textColors && isDarkMode) {
                element.dataset["ogsc" /* OriginalStyleColor */] =
                    textColors.lightModeColor;
            }
        }
        if (backgroundColor || backgroundColors) {
            elementStyle.backgroundColor =
                (isDarkMode ? backgroundColors === null || backgroundColors === void 0 ? void 0 : backgroundColors.darkModeColor : backgroundColors === null || backgroundColors === void 0 ? void 0 : backgroundColors.lightModeColor) ||
                    backgroundColor;
            if (backgroundColors && isDarkMode) {
                element.dataset["ogsb" /* OriginalStyleBackgroundColor */] =
                    backgroundColors.lightModeColor;
            }
        }
        if (bold) {
            elementStyle.fontWeight = 'bold';
        }
        if (italic) {
            elementStyle.fontStyle = 'italic';
        }
        if (underline) {
            elementStyle.textDecoration = 'underline';
        }
    }
}
exports.default = applyFormat;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/applyTextStyle.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/applyTextStyle.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var Position_1 = __webpack_require__(/*! ../selection/Position */ "./packages/roosterjs-editor-dom/lib/selection/Position.ts");
var splitTextNode_1 = __webpack_require__(/*! ./splitTextNode */ "./packages/roosterjs-editor-dom/lib/utils/splitTextNode.ts");
var wrap_1 = __webpack_require__(/*! ./wrap */ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts");
var getLeafSibling_1 = __webpack_require__(/*! ./getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
var splitParentNode_1 = __webpack_require__(/*! ./splitParentNode */ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts");
var STYLETAGS = 'SPAN,B,I,U,EM,STRONG,STRIKE,S,SMALL'.split(',');
/**
 * Apply style using a styler function to the given container node in the given range
 * @param container The container node to apply style to
 * @param styler The styler function
 * @param from From position
 * @param to To position
 */
function applyTextStyle(container, styler, from, to) {
    if (from === void 0) { from = new Position_1.default(container, 0 /* Begin */).normalize(); }
    if (to === void 0) { to = new Position_1.default(container, -1 /* End */).normalize(); }
    var formatNodes = [];
    while (from && to && to.isAfter(from)) {
        var formatNode = from.node;
        var parentTag = getTagOfNode_1.default(formatNode.parentNode);
        // The code below modifies DOM. Need to get the next sibling first otherwise you won't be able to reliably get a good next sibling node
        var nextNode = getLeafSibling_1.getNextLeafSibling(container, formatNode);
        if (formatNode.nodeType == 3 /* Text */ && ['TR', 'TABLE'].indexOf(parentTag) < 0) {
            if (formatNode == to.node && !to.isAtEnd) {
                formatNode = splitTextNode_1.default(formatNode, to.offset, true /*returnFirstPart*/);
            }
            if (from.offset > 0) {
                formatNode = splitTextNode_1.default(formatNode, from.offset, false /*returnFirstPart*/);
            }
            formatNodes.push(formatNode);
        }
        from = nextNode && new Position_1.default(nextNode, 0 /* Begin */);
    }
    if (formatNodes.length > 0) {
        if (formatNodes.every(function (node) { return node.parentNode == formatNodes[0].parentNode; })) {
            var newNode_1 = formatNodes.shift();
            formatNodes.forEach(function (node) {
                newNode_1.nodeValue += node.nodeValue;
                node.parentNode.removeChild(node);
            });
            formatNodes = [newNode_1];
        }
        formatNodes.forEach(function (node) {
            // When apply style within style tags like B/I/U/..., we split the tag and apply outside them
            // So that the inner style tag such as U, STRIKE can inherit the style we added
            while (getTagOfNode_1.default(node) != 'SPAN' &&
                STYLETAGS.indexOf(getTagOfNode_1.default(node.parentNode)) >= 0) {
                callStylerWithInnerNode(node, styler);
                node = splitParentNode_1.splitBalancedNodeRange(node);
            }
            if (getTagOfNode_1.default(node) != 'SPAN') {
                callStylerWithInnerNode(node, styler);
                node = wrap_1.default(node, 'SPAN');
            }
            styler(node);
        });
    }
}
exports.default = applyTextStyle;
function callStylerWithInnerNode(node, styler) {
    if (node && node.nodeType == 1 /* Element */) {
        styler(node, true /*isInnerNode*/);
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/arrayPush.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/arrayPush.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * A type-safe wrapper for Array.prototype.push.apply()
 * @param mainArray The main array to push items into
 * @param itemsArray The items to push to main array
 */
function arrayPush(mainArray, itemsArray) {
    Array.prototype.push.apply(mainArray, itemsArray);
}
exports.default = arrayPush;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts":
/*!*********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/changeElementTag.ts ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getComputedStyles_1 = __webpack_require__(/*! ./getComputedStyles */ "./packages/roosterjs-editor-dom/lib/utils/getComputedStyles.ts");
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
function changeElementTag(element, newTag) {
    var _a;
    if (!element || !newTag) {
        return null;
    }
    var newElement = element.ownerDocument.createElement(newTag);
    for (var i = 0; i < element.attributes.length; i++) {
        var attr = element.attributes[i];
        newElement.setAttribute(attr.name, attr.value);
    }
    while (element.firstChild) {
        newElement.appendChild(element.firstChild);
    }
    if (getTagOfNode_1.default(element) == 'P' || getTagOfNode_1.default(newElement) == 'P') {
        _a = getComputedStyles_1.default(element, [
            'margin-top',
            'margin-bottom',
        ]), newElement.style.marginTop = _a[0], newElement.style.marginBottom = _a[1];
    }
    if (element.parentNode) {
        element.parentNode.replaceChild(newElement, element);
    }
    return newElement;
}
exports.default = changeElementTag;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/collapseNodes.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ./contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var splitParentNode_1 = __webpack_require__(/*! ./splitParentNode */ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts");
var toArray_1 = __webpack_require__(/*! ./toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
/**
 * Collapse nodes within the given start and end nodes to their common ascenstor node,
 * split parent nodes if necessary
 * @param root The root node of the scope
 * @param start The start node
 * @param end The end node
 * @param canSplitParent True to allow split parent node there are nodes before start or after end under the same parent
 * and the returned nodes will be all nodes from start trhough end after splitting
 * False to disallow split parent
 * @returns When cansplitParent is true, returns all node from start through end after splitting,
 * otherwise just return start and end
 */
function collapseNodes(root, start, end, canSplitParent) {
    if (!contains_1.default(root, start) || !contains_1.default(root, end)) {
        return [];
    }
    start = collapse(root, start, end, true /*isStart*/, canSplitParent);
    end = collapse(root, end, start, false /*isStart*/, canSplitParent);
    if (contains_1.default(start, end, true /*treateSameNodeAsContain*/)) {
        return [start];
    }
    else if (contains_1.default(end, start)) {
        return [end];
    }
    else if (start.parentNode == end.parentNode) {
        var nodes = toArray_1.default(start.parentNode.childNodes);
        var startIndex = nodes.indexOf(start);
        var endIndex = nodes.indexOf(end);
        return nodes.slice(startIndex, endIndex + 1);
    }
    else {
        return [start, end];
    }
}
exports.default = collapseNodes;
/**
 * Collapse a node by traversing its parent nodes until we get the common ancestor node of node and ref node
 * @param root Root node, traversing will be limited under this scope
 * @param node The node to collapse
 * @param ref Ref node. The result will be the nearest common ancestor node of the given node and this ref node
 * @param isStart Whether the given node is start of the sequence of nodes to collapse
 * @param canSplitParent Whether splitting parent node is allowed
 * @returns The common ancestor node of the given node ref node
 */
function collapse(root, node, ref, isStart, canSplitParent) {
    while (node.parentNode != root && !contains_1.default(node.parentNode, ref)) {
        if ((isStart && node.previousSibling) || (!isStart && node.nextSibling)) {
            if (!canSplitParent) {
                break;
            }
            splitParentNode_1.default(node, isStart);
        }
        node = node.parentNode;
    }
    return node;
}
exports.collapse = collapse;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/contains.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/contains.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
function contains(container, contained, treatSameNodeAsContain) {
    if (!container || !contained) {
        return false;
    }
    if (treatSameNodeAsContain && container == contained) {
        return true;
    }
    if (safeInstanceOf_1.default(contained, 'Range')) {
        contained = contained && contained.commonAncestorContainer;
        treatSameNodeAsContain = true;
    }
    if (contained && contained.nodeType == 3 /* Text */) {
        contained = contained.parentNode;
        treatSameNodeAsContain = true;
    }
    if (container.nodeType != 1 /* Element */ && container.nodeType != 11 /* DocumentFragment */) {
        return !!treatSameNodeAsContain && container == contained;
    }
    return (!!(treatSameNodeAsContain || container != contained) &&
        internalContains(container, contained));
}
exports.default = contains;
function internalContains(container, contained) {
    if (container.contains) {
        return container.contains(contained);
    }
    else {
        while (contained) {
            if (contained == container) {
                return true;
            }
            contained = contained.parentNode;
        }
        return false;
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/extractClipboardEvent.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/extractClipboardEvent.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var toArray_1 = __webpack_require__(/*! ./toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
var Browser_1 = __webpack_require__(/*! ./Browser */ "./packages/roosterjs-editor-dom/lib/utils/Browser.ts");
// HTML header to indicate where is the HTML content started from.
// Sample header:
// Version:0.9
// StartHTML:71
// EndHTML:170
// StartFragment:140
// EndFragment:160
// StartSelection:140
// EndSelection:160
var CLIPBOARD_HTML_HEADER_REGEX = /^Version:[0-9\.]+\s+StartHTML:\s*([0-9]+)\s+EndHTML:\s*([0-9]+)\s+/i;
var TEXT_TYPE_PREFIX = 'text/';
var IMAGE_TYPE_PREFIX = 'image/';
var HTML_TYPE = TEXT_TYPE_PREFIX + 'html';
var LINKPREVIEW_TYPE = TEXT_TYPE_PREFIX + 'link-preview';
/**
 * Extract a Clipboard event
 * @param event The paste event
 * @param callback Callback function when data is ready
 * @param fallbackHtmlRetriever If direct HTML retriving is not support (e.g. Internet Explorer), as a fallback,
 * using this helper function to retrieve HTML content
 * @returns An object with the following properties:
 *  types: Available types from the clipboard event
 *  text: Plain text from the clipboard event
 *  image: Image file from the clipboard event
 *  html: Html string from the clipboard event. When set to null, it means there's no HTML found from the event.
 *   When set to undefined, it means can't retrieve HTML string, there may be HTML string but direct retrieving is
 *   not supported by browser.
 */
function extractClipboardEvent(event, callback, options) {
    var _a;
    var dataTransfer = event.clipboardData ||
        event.target.ownerDocument.defaultView.clipboardData;
    var result = {
        types: dataTransfer.types ? toArray_1.default(dataTransfer.types) : [],
        text: dataTransfer.getData('text'),
        image: getImage(dataTransfer),
        rawHtml: undefined,
        customValues: {},
    };
    var handlers = [];
    if (event.clipboardData && event.clipboardData.items) {
        event.preventDefault();
        // Set rawHtml to null so that caller knows that we have tried
        result.rawHtml = null;
        var items = event.clipboardData.items;
        var _loop_1 = function (i) {
            var item = items[i];
            switch (item.type) {
                case HTML_TYPE:
                    handlers.push({
                        promise: getAsString(item),
                        callback: function (value) {
                            result.rawHtml = Browser_1.Browser.isEdge ? workaroundForEdge(value) : value;
                        },
                    });
                    break;
                case LINKPREVIEW_TYPE:
                    if (options === null || options === void 0 ? void 0 : options.allowLinkPreview) {
                        handlers.push({
                            promise: getAsString(item),
                            callback: function (value) {
                                try {
                                    result.linkPreview = JSON.parse(value);
                                }
                                catch (_a) { }
                            },
                        });
                    }
                    break;
                default:
                    if (item.type.indexOf(TEXT_TYPE_PREFIX) == 0) {
                        var textType_1 = item.type.substr(TEXT_TYPE_PREFIX.length);
                        if (((_a = options === null || options === void 0 ? void 0 : options.allowedCustomPasteType) === null || _a === void 0 ? void 0 : _a.indexOf(textType_1)) >= 0) {
                            handlers.push({
                                promise: getAsString(item),
                                callback: function (value) { return (result.customValues[textType_1] = value); },
                            });
                        }
                    }
                    break;
            }
        };
        for (var i = 0; i < items.length; i++) {
            _loop_1(i);
        }
    }
    Promise.all(handlers.map(function (handler) { return handler.promise; })).then(function (values) {
        for (var i = 0; i < handlers.length; i++) {
            handlers[i].callback(values[i]);
        }
        callback(result);
    });
}
exports.default = extractClipboardEvent;
function getImage(dataTransfer) {
    // Chrome, Firefox, Edge support dataTransfer.items
    var fileCount = dataTransfer.items ? dataTransfer.items.length : 0;
    for (var i = 0; i < fileCount; i++) {
        var item = dataTransfer.items[i];
        if (item.type && item.type.indexOf(IMAGE_TYPE_PREFIX) == 0) {
            return item.getAsFile();
        }
    }
    // IE, Safari support dataTransfer.files
    fileCount = dataTransfer.files ? dataTransfer.files.length : 0;
    for (var i = 0; i < fileCount; i++) {
        var file = dataTransfer.files.item(i);
        if (file.type && file.type.indexOf(IMAGE_TYPE_PREFIX) == 0) {
            return file;
        }
    }
    return null;
}
/**
 * Edge sometimes doesn't remove the headers, which cause we paste more things then expected.
 * So we need to remove it in our code
 * @param html The HTML string got from clipboard
 */
function workaroundForEdge(html) {
    var headerValues = CLIPBOARD_HTML_HEADER_REGEX.exec(html);
    if (headerValues && headerValues.length == 3) {
        var start = parseInt(headerValues[1]);
        var end = parseInt(headerValues[2]);
        if (start > 0 && end > start) {
            html = html.substring(start, end);
        }
    }
    return html;
}
function getAsString(item) {
    return new Promise(function (resolve) {
        item.getAsString(function (value) {
            resolve(value);
        });
    });
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts":
/*!*******************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ./contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
/**
 * Find closest element ancestor start from the given node which matches the given selector
 * @param node Find ancestor start from this node
 * @param root Root node where the search should stop at. The return value can never be this node
 * @param selector The expected selector. If null, return the first HTML Element found from start node
 * @returns An HTML element which matches the given selector. If the given start node matches the selector,
 * returns the given node
 */
function findClosestElementAncestor(node, root, selector) {
    node = !node ? null : node.nodeType == 1 /* Element */ ? node : node.parentNode;
    var element = node && node.nodeType == 1 /* Element */ ? node : null;
    if (element && selector) {
        if (element.closest) {
            element = element.closest(selector);
        }
        else {
            while (element &&
                element != root &&
                !(element.matches || element.msMatchesSelector).call(element, selector)) {
                element = element.parentElement;
            }
        }
    }
    return !root || contains_1.default(root, element) ? element : null;
}
exports.default = findClosestElementAncestor;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var toArray_1 = __webpack_require__(/*! ./toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
/**
 * Creates an HTML node array from html
 * @param html the html string to create HTML elements from
 * @param ownerDocument Owner document of the result HTML elements
 * @returns An HTML node array to represent the given html string
 */
function fromHtml(html, ownerDocument) {
    var element = ownerDocument.createElement('DIV');
    element.innerHTML = html;
    return toArray_1.default(element.childNodes);
}
exports.default = fromHtml;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getComputedStyles.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getComputedStyles.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var findClosestElementAncestor_1 = __webpack_require__(/*! ./findClosestElementAncestor */ "./packages/roosterjs-editor-dom/lib/utils/findClosestElementAncestor.ts");
/**
 * Get computed styles of a node
 * @param node The node to get computed styles from
 * @param styleNames Names of style to get, can be a single name or an array.
 * Default value is font-family, font-size, color, background-color
 * @returns An array of the computed styles
 */
function getComputedStyles(node, styleNames) {
    if (styleNames === void 0) { styleNames = ['font-family', 'font-size', 'color', 'background-color']; }
    var element = findClosestElementAncestor_1.default(node);
    var result = [];
    styleNames = Array.isArray(styleNames) ? styleNames : [styleNames];
    if (element) {
        var win = element.ownerDocument.defaultView || window;
        var styles = win.getComputedStyle(element);
        if (styles) {
            for (var _i = 0, styleNames_1 = styleNames; _i < styleNames_1.length; _i++) {
                var style = styleNames_1[_i];
                var value = (styles.getPropertyValue(style) || '').toLowerCase();
                value = style == 'font-size' ? px2Pt(value) : value;
                result.push(value);
            }
        }
    }
    return result;
}
exports.default = getComputedStyles;
/**
 * A shortcut for getComputedStyles() when only one style is to be retrieved
 * @param node The node to get style from
 * @param styleName The style name
 * @returns The style value
 */
function getComputedStyle(node, styleName) {
    return getComputedStyles(node, styleName)[0] || '';
}
exports.getComputedStyle = getComputedStyle;
function px2Pt(px) {
    if (px && px.indexOf('px') == px.length - 2) {
        // Edge may not handle the floating computing well which causes the calculated value is a little less than actual value
        // So add 0.05 to fix it
        return Math.round(parseFloat(px) * 75 + 0.05) / 100 + 'pt';
    }
    return px;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getInnerHTML.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getInnerHTML.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
/**
 * Get innerHTML of the given node
 * @param node The DOM node to get innerHTML from
 */
function getInnerHTML(node) {
    if (safeInstanceOf_1.default(node, 'HTMLElement')) {
        return node.innerHTML;
    }
    else if (node) {
        var tempNode = node.ownerDocument.createElement('span');
        tempNode.appendChild(node.cloneNode(true /*deep*/));
        return tempNode.innerHTML;
    }
    else {
        return '';
    }
}
exports.default = getInnerHTML;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getLeafNode.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getLeafNode.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var shouldSkipNode_1 = __webpack_require__(/*! ./shouldSkipNode */ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts");
var getLeafSibling_1 = __webpack_require__(/*! ./getLeafSibling */ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts");
/**
 * Get first/last leaf node of the given root node.
 * @param rootNode Root node to get leaf node from
 * @param isFirst True to get first leaf node, false to get last leaf node
 */
function getLeafNode(rootNode, isFirst) {
    var getChild = function (node) { return (isFirst ? node.firstChild : node.lastChild); };
    var result = getChild(rootNode);
    while (result && getChild(result)) {
        result = getChild(result);
    }
    if (result && shouldSkipNode_1.default(result)) {
        result = getLeafSibling_1.getLeafSibling(rootNode, result, isFirst);
    }
    return result;
}
/**
 * Get the first meaningful leaf node
 * @param rootNode Root node to get leaf node from
 */
function getFirstLeafNode(rootNode) {
    return getLeafNode(rootNode, true /*isFirst*/);
}
exports.getFirstLeafNode = getFirstLeafNode;
/**
 * Get the last meaningful leaf node
 * @param rootNode Root node to get leaf node from
 */
function getLastLeafNode(rootNode) {
    return getLeafNode(rootNode, false /*isFirst*/);
}
exports.getLastLeafNode = getLastLeafNode;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getLeafSibling.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var contains_1 = __webpack_require__(/*! ./contains */ "./packages/roosterjs-editor-dom/lib/utils/contains.ts");
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var shouldSkipNode_1 = __webpack_require__(/*! ./shouldSkipNode */ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts");
/**
 * This walks forwards/backwards DOM tree to get next meaningful node
 * @param rootNode Root node to scope the leaf sibling node
 * @param startNode current node to get sibling node from
 * @param isNext True to get next leaf sibling node, false to get previous leaf sibling node
 * @param skipTags (Optional) tags that child elements will be skipped
 * @param ignoreSpace (Optional) Ignore pure space text node when check if the node should be skipped
 */
function getLeafSibling(rootNode, startNode, isNext, skipTags, ignoreSpace) {
    var result = null;
    var getSibling = isNext
        ? function (node) { return node.nextSibling; }
        : function (node) { return node.previousSibling; };
    var getChild = isNext ? function (node) { return node.firstChild; } : function (node) { return node.lastChild; };
    if (contains_1.default(rootNode, startNode)) {
        var curNode = startNode;
        var shouldContinue = true;
        while (shouldContinue) {
            // Find next/previous node, starting from next/previous sibling, then one level up to find next/previous sibling from parent
            // till a non-null nextSibling/previousSibling is found or the ceiling is encountered (rootNode)
            var parentNode = curNode.parentNode;
            curNode = getSibling(curNode);
            while (!curNode && parentNode != rootNode) {
                curNode = getSibling(parentNode);
                parentNode = parentNode.parentNode;
            }
            // Now traverse down to get first/last child
            while (curNode &&
                (!skipTags || skipTags.indexOf(getTagOfNode_1.default(curNode)) < 0) &&
                getChild(curNode)) {
                curNode = getChild(curNode);
            }
            // Check special nodes (i.e. node that has a display:none etc.) and continue looping if so
            shouldContinue = curNode && shouldSkipNode_1.default(curNode, ignoreSpace);
            if (!shouldContinue) {
                // Found a good leaf node, assign and exit
                result = curNode;
                break;
            }
        }
    }
    return result;
}
exports.getLeafSibling = getLeafSibling;
/**
 * This walks forwards DOM tree to get next meaningful node
 * @param rootNode Root node to scope the leaf sibling node
 * @param startNode current node to get sibling node from
 * @param skipTags (Optional) tags that child elements will be skipped
 */
function getNextLeafSibling(rootNode, startNode, skipTags) {
    return getLeafSibling(rootNode, startNode, true /*isNext*/, skipTags);
}
exports.getNextLeafSibling = getNextLeafSibling;
/**
 * This walks backwards DOM tree to get next meaningful node
 * @param rootNode Root node to scope the leaf sibling node
 * @param startNode current node to get sibling node from
 * @param skipTags (Optional) tags that child elements will be skipped
 */
function getPreviousLeafSibling(rootNode, startNode, skipTags) {
    return getLeafSibling(rootNode, startNode, false /*isNext*/, skipTags);
}
exports.getPreviousLeafSibling = getPreviousLeafSibling;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getPendableFormatState.ts":
/*!***************************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getPendableFormatState.ts ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * A map from pendable format name to document command
 */
exports.PendableFormatCommandMap = {
    /**
     * Bold
     */
    isBold: "bold" /* Bold */,
    /**
     * Italic
     */
    isItalic: "italic" /* Italic */,
    /**
     * Underline
     */
    isUnderline: "underline" /* Underline */,
    /**
     * StrikeThrough
     */
    isStrikeThrough: "strikeThrough" /* StrikeThrough */,
    /**
     * Subscript
     */
    isSubscript: "subscript" /* Subscript */,
    /**
     * Superscript
     */
    isSuperscript: "superscript" /* Superscript */,
};
/**
 * Get Pendable Format State at cursor.
 * @param document The HTML Document to get format state from
 * @returns A PendableFormatState object which contains the values of pendable format states
 */
function getPendableFormatState(document) {
    var keys = Object.keys(exports.PendableFormatCommandMap);
    return keys.reduce(function (state, key) {
        state[key] = document.queryCommandState(exports.PendableFormatCommandMap[key]);
        return state;
    }, {});
}
exports.default = getPendableFormatState;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts":
/*!*****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Get the html tag of a node, or empty if it is not an element
 * @param node The node to get tag of
 * @returns Tag name in upper case if the given node is an Element, or empty string otherwise
 */
function getTagOfNode(node) {
    return node && node.nodeType == 1 /* Element */ ? node.tagName.toUpperCase() : '';
}
exports.default = getTagOfNode;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/getTextContent.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/getTextContent.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContentTraverser_1 = __webpack_require__(/*! ../contentTraverser/ContentTraverser */ "./packages/roosterjs-editor-dom/lib/contentTraverser/ContentTraverser.ts");
/**
 * get block element's text content.
 * @param rootNode Root node that the get the textContent of.
 * @returns text content of given text content.
 */
function getTextContent(rootNode) {
    var traverser = ContentTraverser_1.default.createBodyTraverser(rootNode);
    var block = traverser && traverser.currentBlockElement;
    var textContent = [];
    while (block) {
        textContent.push(block.getTextContent());
        block = traverser.getNextBlockElement();
    }
    return textContent.join('\n');
}
exports.default = getTextContent;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/isBlockElement.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var BLOCK_ELEMENT_TAGS = 'ADDRESS,ARTICLE,ASIDE,BLOCKQUOTE,CANVAS,DD,DIV,DL,DT,FIELDSET,FIGCAPTION,FIGURE,FOOTER,FORM,H1,H2,H3,H4,H5,H6,HEADER,HR,LI,MAIN,NAV,NOSCRIPT,OL,OUTPUT,P,PRE,SECTION,TABLE,TD,TH,TFOOT,UL,VIDEO'.split(',');
var BLOCK_DISPLAY_STYLES = ['block', 'list-item', 'table-cell'];
/**
 * Checks if the node is a block like element. Block like element are usually those P, DIV, LI, TD etc.
 * @param node The node to check
 * @returns True if the node is a block element, otherwise false
 */
function isBlockElement(node) {
    var tag = getTagOfNode_1.default(node);
    return !!(tag &&
        (BLOCK_DISPLAY_STYLES.indexOf(node.style.display) >= 0 ||
            BLOCK_ELEMENT_TAGS.indexOf(tag) >= 0));
}
exports.default = isBlockElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Checks if node1 is after node2
 * @param node1 The node to check if it is after another node
 * @param node2 The node to check if another node is after this one
 * @returns True if node1 is after node2, otherwise false
 */
function isNodeAfter(node1, node2) {
    return !!(node1 &&
        node2 &&
        (node2.compareDocumentPosition(node1) & 4 /* Following */) ==
            4 /* Following */);
}
exports.default = isNodeAfter;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/isNodeEmpty.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/isNodeEmpty.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var VISIBLE_ELEMENT_TAGS = ['IMG'];
var VISIBLE_CHILD_ELEMENT_SELECTOR = ['TABLE', 'IMG', 'LI'].join(',');
var ZERO_WIDTH_SPACE = /\u200b/g;
/**
 * Check if a given node has no visible content
 * @param node The node to check
 * @param trimContent Whether trim the text content so that spaces will be treated as empty.
 * Default value is false
 * @returns True if there isn't any visible element inside node, otherwise false
 */
function isNodeEmpty(node, trimContent) {
    if (!node) {
        return false;
    }
    else if (node.nodeType == 3 /* Text */) {
        return trim(node.nodeValue, trimContent) == '';
    }
    else if (node.nodeType == 1 /* Element */) {
        var element = node;
        var textContent = trim(element.textContent, trimContent);
        if (textContent != '' ||
            VISIBLE_ELEMENT_TAGS.indexOf(getTagOfNode_1.default(element)) >= 0 ||
            element.querySelectorAll(VISIBLE_CHILD_ELEMENT_SELECTOR)[0]) {
            return false;
        }
    }
    return true;
}
exports.default = isNodeEmpty;
function trim(s, trim) {
    s = s.replace(ZERO_WIDTH_SPACE, '');
    return trim ? s.trim() : s;
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/isVoidHtmlElement.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/isVoidHtmlElement.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
/**
 * HTML void elements
 * Per https://www.w3.org/TR/html/syntax.html#syntax-elements, cannot have child nodes
 * This regex is used when we move focus to very begin of editor. We should avoid putting focus inside
 * void elements so users don't accidently create child nodes in them
 */
var HTML_VOID_ELEMENTS = 'AREA,BASE,BR,COL,COMMAND,EMBED,HR,IMG,INPUT,KEYGEN,LINK,META,PARAM,SOURCE,TRACK,WBR'.split(',');
/**
 * Check if the given node is html void element. Void element cannot have childen
 * @param node The node to check
 */
function isVoidHtmlElement(node) {
    return !!node && HTML_VOID_ELEMENTS.indexOf(getTagOfNode_1.default(node)) >= 0;
}
exports.default = isVoidHtmlElement;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/matchLink.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/matchLink.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
// http exclude matching regex
// invalid URL example (in paricular on IE and Edge):
// - http://www.bing.com%00, %00 before ? (question mark) is considered invalid. IE/Edge throws invalid argument exception
// - http://www.bing.com%1, %1 is invalid
// - http://www.bing.com%g, %g is invalid (IE and Edge expects a two hex value after a %)
// - http://www.bing.com%, % as ending is invalid (IE and Edge expects a two hex value after a %)
// All above % cases if they're after ? (question mark) is then considered valid again
// Similar for @, it needs to be after / (forward slash), or ? (question mark). Otherwise IE/Edge will throw security exception
// - http://www.bing.com@name, @name before ? (question mark) is considered invalid
// - http://www.bing.com/@name, is valid sine it is after / (forward slash)
// - http://www.bing.com?@name, is also valid sinve it is after ? (question mark)
// The regex below is essentially a break down of:
// ^[^?]+%[^0-9a-f]+ => to exclude URL like www.bing.com%%
// ^[^?]+%[0-9a-f][^0-9a-f]+ => to exclude URL like www.bing.com%1
// ^[^?]+%00 => to exclude URL like www.bing.com%00
// ^[^?]+%$ => to exclude URL like www.bing.com%
// ^https?:\/\/[^?\/]+@ => to exclude URL like http://www.bing.com@name
// ^www\.[^?\/]+@ => to exclude URL like www.bing.com@name
// , => to exclude url like www.bing,,com
var httpExcludeRegEx = /^[^?]+%[^0-9a-f]+|^[^?]+%[0-9a-f][^0-9a-f]+|^[^?]+%00|^[^?]+%$|^https?:\/\/[^?\/]+@|^www\.[^?\/]+@/i;
// via https://tools.ietf.org/html/rfc1035 Page 7
var labelRegEx = '[a-z0-9](?:[a-z0-9-]*[a-z0-9])?'; // We're using case insensitive regexes below so don't bother including A-Z
var domainNameRegEx = "(?:" + labelRegEx + "\\.)*" + labelRegEx;
var domainPortRegEx = domainNameRegEx + "(?:\\:[0-9]+)?";
var domainPortWithUrlRegEx = domainPortRegEx + "(?:[\\/\\?]\\S*)?";
var linkMatchRules = {
    http: {
        match: new RegExp("^(?:microsoft-edge:)?http:\\/\\/" + domainPortWithUrlRegEx + "|www\\." + domainPortWithUrlRegEx, 'i'),
        except: httpExcludeRegEx,
        normalizeUrl: function (url) {
            return new RegExp('^(?:microsoft-edge:)?http:\\/\\/', 'i').test(url) ? url : 'http://' + url;
        },
    },
    https: {
        match: new RegExp("^(?:microsoft-edge:)?https:\\/\\/" + domainPortWithUrlRegEx, 'i'),
        except: httpExcludeRegEx,
    },
    mailto: { match: new RegExp('^mailto:\\S+@\\S+\\.\\S+', 'i') },
    notes: { match: new RegExp('^notes:\\/\\/\\S+', 'i') },
    file: { match: new RegExp('^file:\\/\\/\\/?\\S+', 'i') },
    unc: { match: new RegExp('^\\\\\\\\\\S+', 'i') },
    ftp: {
        match: new RegExp("^ftp:\\/\\/" + domainPortWithUrlRegEx + "|ftp\\." + domainPortWithUrlRegEx, 'i'),
        normalizeUrl: function (url) { return (new RegExp('^ftp:\\/\\/', 'i').test(url) ? url : 'ftp://' + url); },
    },
    news: { match: new RegExp("^news:(\\/\\/)?" + domainPortWithUrlRegEx, 'i') },
    telnet: { match: new RegExp("^telnet:(\\/\\/)?" + domainPortWithUrlRegEx, 'i') },
    gopher: { match: new RegExp("^gopher:\\/\\/" + domainPortWithUrlRegEx, 'i') },
    wais: { match: new RegExp("^wais:(\\/\\/)?" + domainPortWithUrlRegEx, 'i') },
};
/**
 * Try to match a given string with link match rules, return matched link
 * @param url Input url to match
 * @param option Link match option, exact or partial. If it is exact match, we need
 * to check the length of matched link and url
 * @param rules Optional link match rules, if not passed, only the default link match
 * rules will be applied
 * @returns The matched link data, or null if no match found.
 * The link data includes an original url and a normalized url
 */
function matchLink(url) {
    if (url) {
        for (var _i = 0, _a = Object.keys(linkMatchRules); _i < _a.length; _i++) {
            var schema = _a[_i];
            var rule = linkMatchRules[schema];
            var matches = url.match(rule.match);
            if (matches && matches[0] == url && (!rule.except || !rule.except.test(url))) {
                return {
                    scheme: schema,
                    originalUrl: url,
                    normalizedUrl: rule.normalizeUrl ? rule.normalizeUrl(url) : url,
                };
            }
        }
    }
    return null;
}
exports.default = matchLink;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/normalizeRect.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/normalizeRect.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * A ClientRect of all 0 is possible. i.e. chrome returns a ClientRect of 0 when the cursor is on an empty p
 * We validate that and only return a rect when the passed in ClientRect is valid
 */
function normalizeRect(clientRect) {
    var _a = clientRect || { left: 0, right: 0, top: 0, bottom: 0 }, left = _a.left, right = _a.right, top = _a.top, bottom = _a.bottom;
    return left + right + top + bottom > 0
        ? {
            left: Math.round(left),
            right: Math.round(right),
            top: Math.round(top),
            bottom: Math.round(bottom),
        }
        : null;
}
exports.default = normalizeRect;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/queryElements.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/queryElements.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var toArray_1 = __webpack_require__(/*! ./toArray */ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts");
/**
 * Query HTML elements in the container by a selector string
 * @param container Container element to query from
 * @param selector Selector string to query
 * @param forEachCallback An optional callback to be invoked on each node in query result
 * @param scope The scope of the query, default value is QueryScope.Body
 * @param range The selection range to query with. This is required when scope is not Body
 * @returns HTML Element array of the query result
 */
function queryElements(container, selector, forEachCallback, scope, range) {
    if (scope === void 0) { scope = 0 /* Body */; }
    if (!container || !selector) {
        return [];
    }
    var elements = toArray_1.default(container.querySelectorAll(selector));
    if (scope != 0 /* Body */ && range) {
        var startContainer_1 = range.startContainer, startOffset = range.startOffset, endContainer_1 = range.endContainer, endOffset = range.endOffset;
        if (startContainer_1.nodeType == 1 /* Element */ && startContainer_1.firstChild) {
            var child = startContainer_1.childNodes[startOffset];
            // range.startOffset can give a value of child.length+1 when selection is after the last child
            // In that case we will use the last child instead
            startContainer_1 = child || startContainer_1.lastChild;
        }
        endContainer_1 =
            endContainer_1.nodeType == 1 /* Element */ && endContainer_1.firstChild && endOffset > 0
                ? endContainer_1.childNodes[endOffset - 1]
                : endContainer_1;
        elements = elements.filter(function (element) {
            return isIntersectWithNodeRange(element, startContainer_1, endContainer_1, scope == 2 /* InSelection */);
        });
    }
    if (forEachCallback) {
        elements.forEach(forEachCallback);
    }
    return elements;
}
exports.default = queryElements;
function isIntersectWithNodeRange(node, startNode, endNode, nodeContainedByRangeOnly) {
    var startPosition = node.compareDocumentPosition(startNode);
    var endPosition = node.compareDocumentPosition(endNode);
    var targetPositions = [0 /* Same */, 8 /* Contains */];
    if (!nodeContainedByRangeOnly) {
        targetPositions.push(16 /* ContainedBy */);
    }
    return (checkPosition(startPosition, targetPositions) || // intersectStart
        checkPosition(endPosition, targetPositions) || // intersectEnd
        (checkPosition(startPosition, [2 /* Preceding */]) && // Contains
            checkPosition(endPosition, [4 /* Following */]) &&
            !checkPosition(endPosition, [16 /* ContainedBy */])));
}
function checkPosition(position, targets) {
    return targets.some(function (target) {
        return target == 0 /* Same */
            ? position == 0 /* Same */
            : (position & target) == target;
    });
}


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/readFile.ts":
/*!*************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/readFile.ts ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Read a file object and invoke a callback function with the data url of this file
 * @param file The file to read
 * @param callback the callback to invoke with data url of the file.
 * If fail to read, dataUrl will be null
 */
function readFile(file, callback) {
    if (file && callback) {
        var reader_1 = new FileReader();
        reader_1.onload = function () {
            callback(reader_1.result);
        };
        reader_1.onerror = function () {
            callback(null);
        };
        reader_1.readAsDataURL(file);
    }
}
exports.default = readFile;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
// NOTE: Type TargetWindow is an auto-generated type.
// Run node ./tools/generateTargetWindow.js to generate it.
/**
 * @internal Export for test only
 * Try get window from the given node or range
 * @param source Source node or range
 */
function getTargetWindow(source) {
    var node = source && (source.commonAncestorContainer || source);
    var document = node &&
        (node.ownerDocument ||
            (Object.prototype.toString.apply(node) == '[object HTMLDocument]'
                ? node
                : null));
    // If document exists but document.defaultView doesn't exist, it is a detached object, just use current window instead
    var targetWindow = document && (document.defaultView || window);
    return targetWindow;
}
exports.getTargetWindow = getTargetWindow;
/**
 * Check if the given object is instance of the target type
 * @param obj Object to check
 * @param typeName Target type name
 */
function safeInstanceOf(obj, typeName) {
    var targetWindow = getTargetWindow(obj);
    var targetType = targetWindow && targetWindow[typeName];
    var mainWindow = window;
    var mainWindowType = mainWindow && mainWindow[typeName];
    return ((mainWindowType && obj instanceof mainWindowType) ||
        (targetType && obj instanceof targetType));
}
exports.default = safeInstanceOf;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts":
/*!*******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/shouldSkipNode.ts ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getTagOfNode_1 = __webpack_require__(/*! ./getTagOfNode */ "./packages/roosterjs-editor-dom/lib/utils/getTagOfNode.ts");
var getComputedStyles_1 = __webpack_require__(/*! ./getComputedStyles */ "./packages/roosterjs-editor-dom/lib/utils/getComputedStyles.ts");
var CRLF = /^[\r\n]+$/gm;
var CRLFSPACE = /[\t\r\n\u0020\u200B]/gm; // We should only find new line, real space or ZeroWidthSpace (TAB, %20, but not &nbsp;)
/**
 * Skip a node when any of following conditions are true
 * - it is neither Element nor Text
 * - it is a text node but is empty
 * - it is a text node but contains just CRLF (noisy text node that often comes in-between elements)
 * - has a display:none
 * - it is just <div></div>
 * @param node The node to check
 * @param ignoreSpace (Optional) True to ignore pure space text node of the node when check.
 * If the value of a node value is only space, set this to true will treat this node as skippable.
 * Default value is false
 */
function shouldSkipNode(node, ignoreSpace) {
    if (node.nodeType == 3 /* Text */) {
        if (!node.nodeValue || node.textContent == '' || CRLF.test(node.nodeValue)) {
            return true;
        }
        else if (ignoreSpace && node.nodeValue.replace(CRLFSPACE, '') == '') {
            return true;
        }
        else {
            return false;
        }
    }
    else if (node.nodeType == 1 /* Element */) {
        if (getComputedStyles_1.getComputedStyle(node, 'display') == 'none') {
            return true;
        }
        var tag = getTagOfNode_1.default(node);
        if (tag == 'DIV' || tag == 'SPAN') {
            // Empty SPAN/DIV or SPAN/DIV with only unmeaningful children is unmeaningful,
            // because it can render nothing. If we keep them here, there may be unexpected
            // LI elements added for those unmeaningful nodes.
            for (var child = node.firstChild; !!child; child = child.nextSibling) {
                if (!shouldSkipNode(child, ignoreSpace)) {
                    return false;
                }
            }
            return true;
        }
        else {
            // There may still be other cases that the node is not meaningful.
            // We can add those cases here once we hit them.
            return false;
        }
    }
    else {
        return true;
    }
}
exports.default = shouldSkipNode;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/splitParentNode.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var isNodeAfter_1 = __webpack_require__(/*! ./isNodeAfter */ "./packages/roosterjs-editor-dom/lib/utils/isNodeAfter.ts");
/**
 * Split parent node of the given node before/after the given node.
 * When a parent node contains [A,B,C] and pass B as the given node,
 * If split before, the new nodes will be [A][B,C] and returns [A];
 * otherwise, it will be [A,B][C] and returns [C].
 * @param node The node to split before/after
 * @param splitBefore Whether split before or after
 * @param removeEmptyNewNode If the new node is empty (even then only child is space or ZER_WIDTH_SPACE),
 * we remove it. @default false
 * @returns The new parent node
 */
function splitParentNode(node, splitBefore) {
    if (!node || !node.parentNode) {
        return null;
    }
    var parentNode = node.parentNode;
    var newParent = parentNode.cloneNode(false /*deep*/);
    newParent.removeAttribute('id');
    if (splitBefore) {
        while (parentNode.firstChild && parentNode.firstChild != node) {
            newParent.appendChild(parentNode.firstChild);
        }
    }
    else {
        while (node.nextSibling) {
            newParent.appendChild(node.nextSibling);
        }
    }
    // When the only child of new parent is ZERO_WIDTH_SPACE, we can still prevent keeping it by set removeEmptyNewNode to true
    if (newParent.firstChild && newParent.innerHTML != '') {
        parentNode.parentNode.insertBefore(newParent, splitBefore ? parentNode : parentNode.nextSibling);
    }
    else {
        newParent = null;
    }
    return newParent;
}
exports.default = splitParentNode;
/**
 * Split parent node by a balanced node range
 * @param nodes The nodes to split from. If only one node is passed, split it from all its siblings.
 * If two or nodes are passed, will split before the first one and after the last one, all other nodes will be ignored
 * @returns The parent node of the given node range if the given nodes are balanced, otherwise null
 */
function splitBalancedNodeRange(nodes) {
    var start = Array.isArray(nodes) ? nodes[0] : nodes;
    var end = Array.isArray(nodes) ? nodes[nodes.length - 1] : nodes;
    var parentNode = start && end && start.parentNode == end.parentNode ? start.parentNode : null;
    if (parentNode) {
        if (isNodeAfter_1.default(start, end)) {
            var temp = end;
            end = start;
            start = temp;
        }
        splitParentNode(start, true /*splitBefore*/);
        splitParentNode(end, false /*splitBefore*/);
    }
    return parentNode;
}
exports.splitBalancedNodeRange = splitBalancedNodeRange;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/splitTextNode.ts":
/*!******************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/splitTextNode.ts ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Split a text node into two parts by an offset number, and return one of them
 * @param textNode The text node to split
 * @param offset The offset number to split at
 * @param returnFirstPart True to return the first part, then the passed in textNode will become the second part.
 * Otherwise return the second part, and the passed in textNode will become the first part
 */
function splitTextNode(textNode, offset, returnFirstPart) {
    var firstPart = textNode.nodeValue.substr(0, offset);
    var secondPart = textNode.nodeValue.substr(offset);
    var newNode = textNode.ownerDocument.createTextNode(returnFirstPart ? firstPart : secondPart);
    textNode.nodeValue = returnFirstPart ? secondPart : firstPart;
    textNode.parentNode.insertBefore(newNode, returnFirstPart ? textNode : textNode.nextSibling);
    return newNode;
}
exports.default = splitTextNode;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/toArray.ts":
/*!************************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/toArray.ts ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
function toArray(collection) {
    return [].slice.call(collection);
}
exports.default = toArray;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/unwrap.ts":
/*!***********************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/unwrap.ts ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * Removes the node and keep all children in place, return the parentNode where the children are attached
 * @param node the node to remove
 */
function unwrap(node) {
    // Unwrap requires a parentNode
    var parentNode = node ? node.parentNode : null;
    if (!parentNode) {
        return null;
    }
    while (node.firstChild) {
        parentNode.insertBefore(node.firstChild, node);
    }
    parentNode.removeChild(node);
    return parentNode;
}
exports.default = unwrap;


/***/ }),

/***/ "./packages/roosterjs-editor-dom/lib/utils/wrap.ts":
/*!*********************************************************!*\
  !*** ./packages/roosterjs-editor-dom/lib/utils/wrap.ts ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var fromHtml_1 = __webpack_require__(/*! ./fromHtml */ "./packages/roosterjs-editor-dom/lib/utils/fromHtml.ts");
var safeInstanceOf_1 = __webpack_require__(/*! ../utils/safeInstanceOf */ "./packages/roosterjs-editor-dom/lib/utils/safeInstanceOf.ts");
function wrap(nodes, wrapper) {
    nodes = !nodes ? [] : safeInstanceOf_1.default(nodes, 'Node') ? [nodes] : nodes;
    if (nodes.length == 0 || !nodes[0]) {
        return null;
    }
    if (!safeInstanceOf_1.default(wrapper, 'HTMLElement')) {
        var document_1 = nodes[0].ownerDocument;
        wrapper = wrapper || 'div';
        wrapper = /^\w+$/.test(wrapper)
            ? document_1.createElement(wrapper)
            : fromHtml_1.default(wrapper, document_1)[0];
    }
    var parentNode = nodes[0].parentNode;
    if (parentNode) {
        parentNode.insertBefore(wrapper, nodes[0]);
    }
    for (var _i = 0, nodes_1 = nodes; _i < nodes_1.length; _i++) {
        var node = nodes_1[_i];
        wrapper.appendChild(node);
    }
    return wrapper;
}
exports.default = wrap;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/ContentEdit.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/ContentEdit.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/ContentEdit/index */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/ContextMenu.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/ContextMenu.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/ContextMenu/index */ "./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/CustomReplace.ts":
/*!****************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/CustomReplace.ts ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/CustomReplace/index */ "./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/CutPasteListChain.ts":
/*!********************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/CutPasteListChain.ts ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/CutPasteListChain/index */ "./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/HyperLink.ts":
/*!************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/HyperLink.ts ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/HyperLink/index */ "./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/ImageResize.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/ImageResize.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/ImageResize/index */ "./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/Paste.ts":
/*!********************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/Paste.ts ***!
  \********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/Paste/index */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/Picker.ts":
/*!*********************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/Picker.ts ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/Picker/index */ "./packages/roosterjs-editor-plugins/lib/plugins/Picker/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/TableResize.ts":
/*!**************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/TableResize.ts ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/TableResize/index */ "./packages/roosterjs-editor-plugins/lib/plugins/TableResize/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/Watermark.ts":
/*!************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/Watermark.ts ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./plugins/Watermark/index */ "./packages/roosterjs-editor-plugins/lib/plugins/Watermark/index.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/index.ts":
/*!********************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/index.ts ***!
  \********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(/*! ./ContentEdit */ "./packages/roosterjs-editor-plugins/lib/ContentEdit.ts"));
__export(__webpack_require__(/*! ./ContextMenu */ "./packages/roosterjs-editor-plugins/lib/ContextMenu.ts"));
__export(__webpack_require__(/*! ./CustomReplace */ "./packages/roosterjs-editor-plugins/lib/CustomReplace.ts"));
__export(__webpack_require__(/*! ./CutPasteListChain */ "./packages/roosterjs-editor-plugins/lib/CutPasteListChain.ts"));
__export(__webpack_require__(/*! ./HyperLink */ "./packages/roosterjs-editor-plugins/lib/HyperLink.ts"));
__export(__webpack_require__(/*! ./ImageResize */ "./packages/roosterjs-editor-plugins/lib/ImageResize.ts"));
__export(__webpack_require__(/*! ./Paste */ "./packages/roosterjs-editor-plugins/lib/Paste.ts"));
__export(__webpack_require__(/*! ./Picker */ "./packages/roosterjs-editor-plugins/lib/Picker.ts"));
__export(__webpack_require__(/*! ./TableResize */ "./packages/roosterjs-editor-plugins/lib/TableResize.ts"));
__export(__webpack_require__(/*! ./Watermark */ "./packages/roosterjs-editor-plugins/lib/Watermark.ts"));


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/ContentEdit.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/ContentEdit.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var getAllFeatures_1 = __webpack_require__(/*! ./getAllFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/getAllFeatures.ts");
/**
 * An editor plugin to handle content edit event.
 * The following cases are included:
 * 1. Auto increase/decrease indentation on Tab, Shift+tab
 * 2. Enter, Backspace on empty list item
 * 3. Enter, Backspace on empty blockquote line
 * 4. Auto bullet/numbering
 * 5. Auto link
 * 6. Tab in table
 * 7. Up/Down in table
 * 8. Manage list style
 */
var ContentEdit = /** @class */ (function () {
    /**
     * Create instance of ContentEdit plugin
     * @param settingsOverride An optional feature set to override default feature settings
     * @param additionalFeatures Optional. More features to add
     */
    function ContentEdit(settingsOverride, additionalFeatures) {
        this.settingsOverride = settingsOverride;
        this.additionalFeatures = additionalFeatures;
    }
    /**
     * Get a friendly name of  this plugin
     */
    ContentEdit.prototype.getName = function () {
        return 'ContentEdit';
    };
    /**
     * Initialize this plugin
     * @param editor The editor instance
     */
    ContentEdit.prototype.initialize = function (editor) {
        var _this = this;
        var features = [];
        var allFeatures = getAllFeatures_1.default();
        Object.keys(allFeatures).forEach(function (key) {
            var feature = allFeatures[key];
            var hasSettingForKey = _this.settingsOverride && _this.settingsOverride[key] !== undefined;
            if ((hasSettingForKey && _this.settingsOverride[key]) ||
                (!hasSettingForKey && !feature.defaultDisabled)) {
                features.push(feature);
            }
        });
        features
            .concat(this.additionalFeatures || [])
            .forEach(function (feature) { return editor.addContentEditFeature(feature); });
    };
    /**
     * Dispose this plugin
     */
    ContentEdit.prototype.dispose = function () { };
    return ContentEdit;
}());
exports.default = ContentEdit;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/autoLinkFeatures.ts":
/*!************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/autoLinkFeatures.ts ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * When user type, they may end a link with a puncatuation, i.e. www.bing.com;
 * we need to trim off the trailing puncatuation before turning it to link match
 */
var TRAILING_PUNCTUATION_REGEX = /[.+=\s:;"',>]+$/i;
var MINIMUM_LENGTH = 5;
/**
 * AutoLink edit feature, provides the ability to automatically convert text user typed or pasted
 * in hyperlink format into a real hyperlink
 */
var AutoLink = {
    keys: [13 /* ENTER */, 32 /* SPACE */, 257 /* CONTENTCHANGED */],
    shouldHandleEvent: cacheGetLinkData,
    handleEvent: autoLink,
};
/**
 * UnlinkWhenBackspaceAfterLink edit feature, provides the ability to convert a hyperlink back into text
 * if user presses BACKSPACE right after a hyperlink
 */
var UnlinkWhenBackspaceAfterLink = {
    keys: [8 /* BACKSPACE */],
    shouldHandleEvent: hasLinkBeforeCursor,
    handleEvent: function (event, editor) {
        event.rawEvent.preventDefault();
        roosterjs_editor_api_1.removeLink(editor);
    },
    defaultDisabled: true,
};
function cacheGetLinkData(event, editor) {
    return event.eventType == 0 /* KeyDown */ ||
        (event.eventType == 7 /* ContentChanged */ && event.source == "Paste" /* Paste */)
        ? roosterjs_editor_dom_1.cacheGetEventData(event, 'LINK_DATA', function () {
            // First try to match link from the whole paste string from the plain text in clipboard.
            // This helps when we paste a link next to some existing character, and the text we got
            // from clipboard will only contain what we pasted, any existing characters will not
            // be included.
            var clipboardData = event.eventType == 7 /* ContentChanged */ &&
                event.source == "Paste" /* Paste */ &&
                event.data;
            var link = roosterjs_editor_dom_1.matchLink((clipboardData.text || '').trim());
            var searcher = editor.getContentSearcherOfCursor(event);
            // In case the matched link is already inside a <A> tag, we do a range search.
            // getRangeFromText will return null if the given text is already in a LinkInlineElement
            if (link && searcher.getRangeFromText(link.originalUrl, false /*exactMatch*/)) {
                return link;
            }
            var word = searcher && searcher.getWordBefore();
            if (word && word.length > MINIMUM_LENGTH) {
                // Check for trailing punctuation
                var trailingPunctuations = word.match(TRAILING_PUNCTUATION_REGEX);
                var trailingPunctuation = (trailingPunctuations || [])[0] || '';
                var candidate_1 = word.substring(0, word.length - trailingPunctuation.length);
                // Do special handling for ')', '}', ']'
                ['()', '{}', '[]'].forEach(function (str) {
                    if (candidate_1[candidate_1.length - 1] == str[1] &&
                        candidate_1.indexOf(str[0]) < 0) {
                        candidate_1 = candidate_1.substr(0, candidate_1.length - 1);
                    }
                });
                // Match and replace in editor
                return roosterjs_editor_dom_1.matchLink(candidate_1);
            }
            return null;
        })
        : null;
}
function hasLinkBeforeCursor(event, editor) {
    var contentSearcher = editor.getContentSearcherOfCursor(event);
    var inline = contentSearcher.getInlineElementBefore();
    return inline instanceof roosterjs_editor_dom_1.LinkInlineElement;
}
function autoLink(event, editor) {
    var anchor = editor.getDocument().createElement('a');
    var linkData = cacheGetLinkData(event, editor);
    // Need to get searcher before we enter the async callback since the callback can happen when cursor is moved to next line
    // and at that time a new searcher won't be able to find the link text to replace
    var searcher = editor.getContentSearcherOfCursor();
    anchor.textContent = linkData.originalUrl;
    anchor.href = linkData.normalizedUrl;
    editor.runAsync(function (editor) {
        editor.addUndoSnapshot(function () {
            roosterjs_editor_api_1.replaceWithNode(editor, linkData.originalUrl, anchor, false /* exactMatch */, searcher);
            // The content at cursor has changed. Should also clear the cursor data cache
            roosterjs_editor_dom_1.clearEventDataCache(event);
            return anchor;
        }, "AutoLink" /* AutoLink */, true /*canUndoByBackspace*/);
    });
}
/**
 * @internal
 */
exports.AutoLinkFeatures = {
    autoLink: AutoLink,
    unlinkWhenBackspaceAfterLink: UnlinkWhenBackspaceAfterLink,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/cursorFeatures.ts":
/*!**********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/cursorFeatures.ts ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var NoCycleCursorMove = {
    keys: [37 /* LEFT */, 39 /* RIGHT */],
    allowFunctionKeys: true,
    shouldHandleEvent: function (event, editor, ctrlOrMeta) {
        var range;
        var position;
        if (!ctrlOrMeta ||
            !(range = editor.getSelectionRange()) ||
            !range.collapsed ||
            !(position = roosterjs_editor_dom_1.Position.getStart(range)) ||
            !editor.isPositionAtBeginning(position)) {
            return false;
        }
        var rtl = roosterjs_editor_dom_1.getComputedStyle(position.element, 'direction') == 'rtl';
        var rawEvent = event.rawEvent;
        return (!rtl && rawEvent.which == 37 /* LEFT */) || (rtl && rawEvent.which == 39 /* RIGHT */);
    },
    handleEvent: function (event) {
        event.rawEvent.preventDefault();
    },
    defaultDisabled: !roosterjs_editor_dom_1.Browser.isChrome,
};
/**
 * @internal
 */
exports.CursorFeatures = {
    noCycleCursorMove: NoCycleCursorMove,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/entityFeatures.ts":
/*!**********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/entityFeatures.ts ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * A content edit feature to trigger EntityOperation event with operation "Click" when user
 * clicks on a readonly entity.
 */
var ClickOnEntityFeature = {
    keys: [13 /* ENTER */],
    shouldHandleEvent: function (event, editor) { return cacheGetReadonlyEntityElement(event, editor); },
    handleEvent: function (event, editor) {
        cacheGetReadonlyEntityElement(event, editor, 1 /* Click */);
    },
};
/**
 * A content edit feature to trigger EntityOperation event with operation "Escape" when user
 * presses ESC on a readonly entity.
 */
var EscapeFromEntityFeature = {
    keys: [27 /* ESCAPE */],
    shouldHandleEvent: function (event, editor) { return cacheGetReadonlyEntityElement(event, editor); },
    handleEvent: function (event, editor) {
        cacheGetReadonlyEntityElement(event, editor, 3 /* Escape */);
    },
};
function cacheGetReadonlyEntityElement(event, editor, operation) {
    var element = roosterjs_editor_dom_1.cacheGetEventData(event, 'READONLY_ENTITY_ELEMENT', function () {
        var node = event.rawEvent.target;
        var entityElement = node && editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node);
        return entityElement && !entityElement.isContentEditable ? entityElement : null;
    });
    if (element && operation !== undefined) {
        editor.triggerPluginEvent(15 /* EntityOperation */, {
            operation: operation,
            rawEvent: event.rawEvent,
            entity: roosterjs_editor_dom_1.getEntityFromElement(element),
        });
    }
    return element;
}
/**
 * A content edit feature to split current line into two lines at the cursor when user presses
 * ENTER right before a readonly entity.
 * Browser's default behavior will insert an extra BR tag before the entity which causes an extra
 * empty line. So we override the default behavior here.
 */
var EnterBeforeReadonlyEntityFeature = {
    keys: [13 /* ENTER */],
    shouldHandleEvent: function (event, editor) {
        return cacheGetNeighborEntityElement(event, editor, true /*isNext*/, false /*collapseOnly*/);
    },
    handleEvent: function (event, editor) {
        var _a;
        event.rawEvent.preventDefault();
        var range = editor.getSelectionRange();
        var node = roosterjs_editor_dom_1.Position.getEnd(range).normalize().node;
        var br = editor.getDocument().createElement('BR');
        node.parentNode.insertBefore(br, node.nextSibling);
        var block = editor.getBlockElementAtNode(node);
        var newContainer;
        if (block) {
            newContainer = block.collapseToSingleElement();
            (_a = br.parentNode) === null || _a === void 0 ? void 0 : _a.removeChild(br);
        }
        editor.getSelectionRange().deleteContents();
        if (newContainer.nextSibling) {
            editor.select(newContainer.nextSibling, 0 /* Begin */);
        }
    },
};
/**
 * A content edit feature to trigger EntityOperation event with operation "RemoveFromEnd" when user
 * press BACKSPACE right after an entity
 */
var BackspaceAfterEntityFeature = {
    keys: [8 /* BACKSPACE */],
    shouldHandleEvent: function (event, editor) {
        return cacheGetNeighborEntityElement(event, editor, false /*isNext*/, true /*collapseOnly*/);
    },
    handleEvent: function (event, editor) {
        cacheGetNeighborEntityElement(event, editor, false /*isNext*/, true /*collapseOnly*/, 5 /* RemoveFromEnd */);
    },
};
/**
 * A content edit feature to trigger EntityOperation event with operation "RemoveFromStart" when user
 * press DELETE right after an entity
 */
var DeleteBeforeEntityFeature = {
    keys: [46 /* DELETE */],
    shouldHandleEvent: function (event, editor) {
        return cacheGetNeighborEntityElement(event, editor, true /*isNext*/, true /*collapseOnly*/);
    },
    handleEvent: function (event, editor) {
        cacheGetNeighborEntityElement(event, editor, true /*isNext*/, true /*collapseOnly*/, 4 /* RemoveFromStart */);
    },
};
function cacheGetNeighborEntityElement(event, editor, isNext, collapseOnly, operation) {
    var element = roosterjs_editor_dom_1.cacheGetEventData(event, 'NEIGHBOR_ENTITY_ELEMENT_' + isNext + '_' + collapseOnly, function () {
        var range = editor.getSelectionRange();
        if (collapseOnly && !range.collapsed) {
            return null;
        }
        var pos = roosterjs_editor_dom_1.Position.getEnd(range).normalize();
        var isAtBeginOrEnd = pos.offset == 0 || pos.isAtEnd;
        var entityNode = null;
        if (isAtBeginOrEnd) {
            var traverser = editor.getBodyTraverser(pos.node);
            var sibling = isNext
                ? pos.offset == 0
                    ? traverser.currentInlineElement
                    : traverser.getNextInlineElement()
                : pos.isAtEnd
                    ? traverser.currentInlineElement
                    : traverser.getPreviousInlineElement();
            var node = sibling && sibling.getContainerNode();
            if (!collapseOnly) {
                var block = editor.getBlockElementAtNode(pos.node);
                if (!block || !block.contains(node)) {
                    node = null;
                }
            }
            entityNode = node && editor.getElementAtCursor(roosterjs_editor_dom_1.getEntitySelector(), node);
        }
        return entityNode;
    });
    if (element && operation !== undefined) {
        editor.triggerPluginEvent(15 /* EntityOperation */, {
            operation: operation,
            rawEvent: event.rawEvent,
            entity: roosterjs_editor_dom_1.getEntityFromElement(element),
        });
    }
    return element;
}
/**
 * @internal
 */
exports.EntityFeatures = {
    clickOnEntity: ClickOnEntityFeature,
    escapeFromEntity: EscapeFromEntityFeature,
    enterBeforeReadonlyEntity: EnterBeforeReadonlyEntityFeature,
    backspaceAfterEntity: BackspaceAfterEntityFeature,
    deleteBeforeEntity: DeleteBeforeEntityFeature,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/listFeatures.ts":
/*!********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/listFeatures.ts ***!
  \********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * IndentWhenTab edit feature, provides the ability to indent current list when user press TAB
 */
var IndentWhenTab = {
    keys: [9 /* TAB */],
    shouldHandleEvent: function (event, editor) {
        return !event.rawEvent.shiftKey && cacheGetListElement(event, editor);
    },
    handleEvent: function (event, editor) {
        roosterjs_editor_api_1.setIndentation(editor, 0 /* Increase */);
        event.rawEvent.preventDefault();
    },
};
/**
 * OutdentWhenShiftTab edit feature, provides the ability to outdent current list when user press Shift+TAB
 */
var OutdentWhenShiftTab = {
    keys: [9 /* TAB */],
    shouldHandleEvent: function (event, editor) {
        return event.rawEvent.shiftKey && cacheGetListElement(event, editor);
    },
    handleEvent: function (event, editor) {
        roosterjs_editor_api_1.setIndentation(editor, 1 /* Decrease */);
        event.rawEvent.preventDefault();
    },
};
/**
 * MergeInNewLine edit feature, provides the ability to merge current line into a new line when user press
 * BACKSPACE at beginning of a list item
 */
var MergeInNewLine = {
    keys: [8 /* BACKSPACE */],
    shouldHandleEvent: function (event, editor) {
        var li = editor.getElementAtCursor('LI', null /*startFrom*/, event);
        var range = editor.getSelectionRange();
        return li && (range === null || range === void 0 ? void 0 : range.collapsed) && roosterjs_editor_dom_1.isPositionAtBeginningOf(roosterjs_editor_dom_1.Position.getStart(range), li);
    },
    handleEvent: function (event, editor) {
        var li = editor.getElementAtCursor('LI', null /*startFrom*/, event);
        if (li.previousSibling) {
            var chains_1 = getListChains(editor);
            editor.runAsync(function (editor) {
                var br = editor.getDocument().createElement('BR');
                editor.insertNode(br);
                editor.select(br, -3 /* After */);
                roosterjs_editor_api_1.experimentCommitListChains(editor, chains_1);
            });
        }
        else {
            toggleListAndPreventDefault(event, editor);
        }
    },
    defaultDisabled: true,
};
/**
 * OutdentWhenBackOn1stEmptyLine edit feature, provides the ability to outdent current item if user press
 * BACKSPACE at the first and empty line of a list
 */
var OutdentWhenBackOn1stEmptyLine = {
    keys: [8 /* BACKSPACE */],
    shouldHandleEvent: function (event, editor) {
        var li = editor.getElementAtCursor('LI', null /*startFrom*/, event);
        return li && roosterjs_editor_dom_1.isNodeEmpty(li) && !li.previousSibling;
    },
    handleEvent: toggleListAndPreventDefault,
};
/**
 * OutdentWhenEnterOnEmptyLine edit feature, provides the ability to outdent current item if user press
 * ENTER at the beginning of an empty line of a list
 */
var OutdentWhenEnterOnEmptyLine = {
    keys: [13 /* ENTER */],
    shouldHandleEvent: function (event, editor) {
        var li = editor.getElementAtCursor('LI', null /*startFrom*/, event);
        return !event.rawEvent.shiftKey && li && roosterjs_editor_dom_1.isNodeEmpty(li);
    },
    handleEvent: function (event, editor) {
        editor.addUndoSnapshot(function () { return toggleListAndPreventDefault(event, editor); }, null /*changeSource*/, true /*canUndoByBackspace*/);
    },
    defaultDisabled: !roosterjs_editor_dom_1.Browser.isIE && !roosterjs_editor_dom_1.Browser.isChrome,
};
/**
 * AutoBullet edit feature, provides the ablility to automatically convert current line into a list.
 * When user input "1. ", convert into a numbering list
 * When user input "- " or "* ", convert into a bullet list
 */
var AutoBullet = {
    keys: [32 /* SPACE */],
    shouldHandleEvent: function (event, editor) {
        if (!cacheGetListElement(event, editor)) {
            var searcher = editor.getContentSearcherOfCursor(event);
            var textBeforeCursor = searcher.getSubStringBefore(4);
            // Auto list is triggered if:
            // 1. Text before cursor exactly mathces '*', '-' or '1.'
            // 2. There's no non-text inline entities before cursor
            return (/^(\*|-|[0-9]{1,2}\.)$/.test(textBeforeCursor) &&
                !searcher.getNearestNonTextInlineElement());
        }
        return false;
    },
    handleEvent: function (event, editor) {
        editor.runAsync(function (editor) {
            editor.addUndoSnapshot(function () {
                var regions;
                var searcher = editor.getContentSearcherOfCursor();
                var textBeforeCursor = searcher.getSubStringBefore(4);
                var rangeToDelete = searcher.getRangeFromText(textBeforeCursor, true /*exactMatch*/);
                if (!rangeToDelete) {
                    // no op if the range can't be found
                }
                else if (textBeforeCursor.indexOf('*') == 0 ||
                    textBeforeCursor.indexOf('-') == 0) {
                    prepareAutoBullet(editor, rangeToDelete);
                    roosterjs_editor_api_1.toggleBullet(editor);
                }
                else if (textBeforeCursor.indexOf('1.') == 0) {
                    prepareAutoBullet(editor, rangeToDelete);
                    roosterjs_editor_api_1.toggleNumbering(editor);
                }
                else if ((regions = editor.getSelectedRegions()) && regions.length == 1) {
                    var num = parseInt(textBeforeCursor);
                    prepareAutoBullet(editor, rangeToDelete);
                    roosterjs_editor_api_1.toggleNumbering(editor, num);
                }
            }, null /*changeSource*/, true /*canUndoByBackspace*/);
        });
    },
};
/**
 * Maintain the list numbers in list chain
 * e.g. we have two lists:
 * 1, 2, 3 and 4, 5, 6
 * Now we delete list item 2, so the first one becomes "1, 2".
 * This edit feature can maintain the list number of the second list to become "3, 4, 5"
 */
var MaintainListChain = {
    keys: [13 /* ENTER */, 9 /* TAB */, 46 /* DELETE */, 8 /* BACKSPACE */, 258 /* RANGE */],
    shouldHandleEvent: function (event, editor) {
        return editor.queryElements('li', 1 /* OnSelection */).length > 0;
    },
    handleEvent: function (event, editor) {
        var chains = getListChains(editor);
        editor.runAsync(function (editor) { return roosterjs_editor_api_1.experimentCommitListChains(editor, chains); });
    },
};
function getListChains(editor) {
    return roosterjs_editor_dom_1.VListChain.createListChains(editor.getSelectedRegions());
}
function prepareAutoBullet(editor, range) {
    range.deleteContents();
    var node = range.startContainer;
    if ((node === null || node === void 0 ? void 0 : node.nodeType) == 3 /* Text */ && node.nodeValue == '' && !node.nextSibling) {
        var br = editor.getDocument().createElement('BR');
        editor.insertNode(br);
        editor.select(br, -2 /* Before */);
    }
}
function toggleListAndPreventDefault(event, editor) {
    var listInfo = cacheGetListElement(event, editor);
    if (listInfo) {
        var listElement = listInfo[0];
        var tag = roosterjs_editor_dom_1.getTagOfNode(listElement);
        if (tag == 'UL') {
            roosterjs_editor_api_1.toggleBullet(editor);
        }
        else if (tag == 'OL') {
            roosterjs_editor_api_1.toggleNumbering(editor);
        }
        editor.focus();
        event.rawEvent.preventDefault();
    }
}
function cacheGetListElement(event, editor) {
    var li = editor.getElementAtCursor('LI,TABLE', null /*startFrom*/, event);
    var listElement = li && roosterjs_editor_dom_1.getTagOfNode(li) == 'LI' && editor.getElementAtCursor('UL,OL', li);
    return listElement ? [listElement, li] : null;
}
/**
 * @internal
 */
exports.ListFeatures = {
    autoBullet: AutoBullet,
    indentWhenTab: IndentWhenTab,
    outdentWhenShiftTab: OutdentWhenShiftTab,
    outdentWhenBackspaceOnEmptyFirstLine: OutdentWhenBackOn1stEmptyLine,
    outdentWhenEnterOnEmptyLine: OutdentWhenEnterOnEmptyLine,
    mergeInNewLineWhenBackspaceOnFirstChar: MergeInNewLine,
    maintainListChain: MaintainListChain,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/markdownFeatures.ts":
/*!************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/markdownFeatures.ts ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var ZERO_WIDTH_SPACE = '\u200B';
function generateBasicMarkdownFeature(key, triggerCharacter, elementTag, useShiftKey) {
    return {
        keys: [key],
        shouldHandleEvent: function (event, editor) {
            return event.rawEvent.shiftKey === useShiftKey &&
                !!cacheGetRangeForMarkdownOperation(event, editor, triggerCharacter);
        },
        handleEvent: function (event, editor) {
            // runAsync is here to allow the event to complete so autocomplete will present the trigger character.
            editor.runAsync(function (editor) {
                handleMarkdownEvent(event, editor, triggerCharacter, elementTag);
            });
        },
    };
}
function cacheGetRangeForMarkdownOperation(event, editor, triggerCharacter) {
    return roosterjs_editor_dom_1.cacheGetEventData(event, 'MARKDOWN_RANGE', function () {
        var searcher = editor.getContentSearcherOfCursor(event);
        var startPosition;
        var endPosition;
        searcher.forEachTextInlineElement(function (textInlineElement) {
            if (endPosition && startPosition) {
                return true;
            }
            var inlineTextContent = textInlineElement.getTextContent();
            // special case for immediately preceeding character being whitespace
            if (inlineTextContent[inlineTextContent.length - 1].trim().length == 0) {
                return false;
            }
            // special case for consecutive trigger characters
            if (inlineTextContent[inlineTextContent.length - 1] === triggerCharacter) {
                return false;
            }
            if (!endPosition) {
                endPosition = textInlineElement.getStartPosition().move(inlineTextContent.length);
            }
            if (inlineTextContent[0] == triggerCharacter) {
                startPosition = textInlineElement.getStartPosition();
            }
            else {
                var contentIndex = inlineTextContent.length - 1;
                for (; contentIndex > 0; contentIndex--) {
                    if (startPosition) {
                        return true;
                    }
                    if (inlineTextContent[contentIndex] == triggerCharacter &&
                        inlineTextContent[contentIndex - 1].trim().length == 0) {
                        startPosition = textInlineElement.getStartPosition().move(contentIndex);
                        return true;
                    }
                }
            }
        });
        return !!startPosition && !!endPosition && roosterjs_editor_dom_1.createRange(startPosition, endPosition);
    });
}
function handleMarkdownEvent(event, editor, triggerCharacter, elementTag) {
    editor.addUndoSnapshot(function () {
        var range = cacheGetRangeForMarkdownOperation(event, editor, triggerCharacter);
        if (!!range) {
            // get the text content range
            var textContentRange = range.cloneRange();
            textContentRange.setStart(textContentRange.startContainer, textContentRange.startOffset + 1);
            // set the removal range to include the typed last character.
            range.setEnd(range.endContainer, range.endOffset + 1);
            // extract content and put it into a new element.
            var elementToWrap = editor.getDocument().createElement(elementTag);
            elementToWrap.appendChild(textContentRange.extractContents());
            range.deleteContents();
            // ZWS here ensures we don't end up inside the newly created node.
            var nonPrintedSpaceTextNode = editor
                .getDocument()
                .createTextNode(ZERO_WIDTH_SPACE);
            range.insertNode(nonPrintedSpaceTextNode);
            range.insertNode(elementToWrap);
            editor.select(nonPrintedSpaceTextNode, -1 /* End */);
        }
    }, "Format" /* Format */, true /*canUndoByBackspace*/);
}
/**
 * Markdown bold feature. Bolds text with markdown shortcuts.
 */
var MarkdownBold = generateBasicMarkdownFeature(56 /* EIGHT_ASTIRISK */, '*', 'b', true);
/**
 * Markdown italics feature. Italicises text with markdown shortcuts.
 */
var MarkdownItalic = generateBasicMarkdownFeature(189 /* DASH_UNDERSCORE */, '_', 'i', true);
/**
 * Markdown strikethru feature. Strikethrus text with markdown shortcuts.
 */
var MarkdownStrikethru = generateBasicMarkdownFeature(192 /* GRAVE_TILDE */, '~', 's', true);
/**
 * Markdown inline code feature. Marks specific text as inline code with markdown shortcuts.
 */
var MarkdownInlineCode = generateBasicMarkdownFeature(192 /* GRAVE_TILDE */, '`', 'code', false);
/**
 * @internal
 */
exports.MarkdownFeatures = {
    markdownBold: MarkdownBold,
    markdownItalic: MarkdownItalic,
    markdownStrikethru: MarkdownStrikethru,
    markdownInlineCode: MarkdownInlineCode,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/quoteFeatures.ts":
/*!*********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/quoteFeatures.ts ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var QUOTE_TAG = 'BLOCKQUOTE';
var STRUCTURED_TAGS = [QUOTE_TAG, 'LI', 'TD', 'TH'].join(',');
/**
 * UnquoteWhenBackOnEmpty1stLine edit feature, provides the ability to Unquote current line when
 * user press BACKSPACE on first and empty line of a BLOCKQUOTE
 */
var UnquoteWhenBackOnEmpty1stLine = {
    keys: [8 /* BACKSPACE */],
    shouldHandleEvent: function (event, editor) {
        var childOfQuote = cacheGetQuoteChild(event, editor);
        return childOfQuote && roosterjs_editor_dom_1.isNodeEmpty(childOfQuote) && !childOfQuote.previousSibling;
    },
    handleEvent: splitQuote,
};
/**
 * UnquoteWhenEnterOnEmptyLine edit feature, provides the ability to Unquote current line when
 * user press ENTER on an empty line of a BLOCKQUOTE
 */
var UnquoteWhenEnterOnEmptyLine = {
    keys: [13 /* ENTER */],
    shouldHandleEvent: function (event, editor) {
        var childOfQuote = cacheGetQuoteChild(event, editor);
        var shift = event.rawEvent.shiftKey;
        return !shift && childOfQuote && roosterjs_editor_dom_1.isNodeEmpty(childOfQuote);
    },
    handleEvent: function (event, editor) {
        return editor.addUndoSnapshot(function () { return splitQuote(event, editor); }, null /*changeSource*/, true /*canUndoByBackspace*/);
    },
};
function cacheGetQuoteChild(event, editor) {
    return roosterjs_editor_dom_1.cacheGetEventData(event, 'QUOTE_CHILD', function () {
        var quote = editor.getElementAtCursor(STRUCTURED_TAGS);
        if (quote && roosterjs_editor_dom_1.getTagOfNode(quote) == QUOTE_TAG) {
            var pos = editor.getFocusedPosition();
            var block = pos && editor.getBlockElementAtNode(pos.normalize().node);
            if (block) {
                var node = block.getStartNode() == quote
                    ? block.getStartNode()
                    : block.collapseToSingleElement();
                return roosterjs_editor_dom_1.isNodeEmpty(node) ? node : null;
            }
        }
        return null;
    });
}
function splitQuote(event, editor) {
    editor.addUndoSnapshot(function () {
        var childOfQuote = cacheGetQuoteChild(event, editor);
        var parent;
        if (roosterjs_editor_dom_1.getTagOfNode(childOfQuote) == QUOTE_TAG) {
            childOfQuote = roosterjs_editor_dom_1.wrap(roosterjs_editor_dom_1.toArray(childOfQuote.childNodes));
        }
        parent = roosterjs_editor_dom_1.splitBalancedNodeRange(childOfQuote);
        roosterjs_editor_dom_1.unwrap(parent);
        editor.select(childOfQuote, 0 /* Begin */);
    });
    event.rawEvent.preventDefault();
}
/**
 * @internal
 */
exports.QuoteFeatures = {
    unquoteWhenBackspaceOnEmptyFirstLine: UnquoteWhenBackOnEmpty1stLine,
    unquoteWhenEnterOnEmptyLine: UnquoteWhenEnterOnEmptyLine,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/shortcutFeatures.ts":
/*!************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/shortcutFeatures.ts ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
function createCommand(winKey, macKey, action) {
    return {
        winKey: winKey,
        macKey: macKey,
        action: action,
    };
}
var commands = [
    createCommand(4096 /* Ctrl */ | 66 /* B */, 8192 /* Meta */ | 66 /* B */, roosterjs_editor_api_1.toggleBold),
    createCommand(4096 /* Ctrl */ | 73 /* I */, 8192 /* Meta */ | 73 /* I */, roosterjs_editor_api_1.toggleItalic),
    createCommand(4096 /* Ctrl */ | 85 /* U */, 8192 /* Meta */ | 85 /* U */, roosterjs_editor_api_1.toggleUnderline),
    createCommand(4096 /* Ctrl */ | 90 /* Z */, 8192 /* Meta */ | 90 /* Z */, function (editor) { return editor.undo(); }),
    createCommand(4096 /* Ctrl */ | 89 /* Y */, 8192 /* Meta */ | 16384 /* Shift */ | 90 /* Z */, function (editor) { return editor.redo(); }),
    createCommand(4096 /* Ctrl */ | 190 /* PERIOD */, 8192 /* Meta */ | 190 /* PERIOD */, roosterjs_editor_api_1.toggleBullet),
    createCommand(4096 /* Ctrl */ | 191 /* FORWARDSLASH */, 8192 /* Meta */ | 191 /* FORWARDSLASH */, roosterjs_editor_api_1.toggleNumbering),
    createCommand(4096 /* Ctrl */ | 16384 /* Shift */ | 190 /* PERIOD */, 8192 /* Meta */ | 16384 /* Shift */ | 190 /* PERIOD */, function (editor) { return roosterjs_editor_api_1.changeFontSize(editor, 0 /* Increase */); }),
    createCommand(4096 /* Ctrl */ | 16384 /* Shift */ | 188 /* COMMA */, 8192 /* Meta */ | 16384 /* Shift */ | 188 /* COMMA */, function (editor) { return roosterjs_editor_api_1.changeFontSize(editor, 1 /* Decrease */); }),
];
/**
 * DefaultShortcut edit feature, provides shortcuts for the following features:
 * Ctrl/Meta+B: toggle bold style
 * Ctrl/Meta+I: toggle italic style
 * Ctrl/Meta+U: toggle underline style
 * Ctrl/Meta+Z: undo
 * Ctrl+Y/Meta+Shift+Z: redo
 * Ctrl/Meta+PERIOD: toggle bullet list
 * Ctrl/Meta+/: toggle numbering list
 * Ctrl/Meta+Shift+>: increase font size
 * Ctrl/Meta+Shift+<: decrease font size
 */
var DefaultShortcut = {
    allowFunctionKeys: true,
    keys: [66 /* B */, 73 /* I */, 85 /* U */, 89 /* Y */, 90 /* Z */, 188 /* COMMA */, 190 /* PERIOD */, 191 /* FORWARDSLASH */],
    shouldHandleEvent: cacheGetCommand,
    handleEvent: function (event, editor) {
        var command = cacheGetCommand(event);
        if (command) {
            command.action(editor);
            event.rawEvent.preventDefault();
            event.rawEvent.stopPropagation();
        }
    },
};
function cacheGetCommand(event) {
    return roosterjs_editor_dom_1.cacheGetEventData(event, 'DEFAULT_SHORT_COMMAND', function () {
        var e = event.rawEvent;
        var key = 
        // Need to check ALT key to be false since in some language (e.g. Polski) uses AltGr to input some special charactors
        // In that case, ctrlKey and altKey are both true in Edge, but we should not trigger any shortcut function here
        event.eventType == 0 /* KeyDown */ && !e.altKey
            ? e.which |
                (e.metaKey && 8192 /* Meta */) |
                (e.shiftKey && 16384 /* Shift */) |
                (e.ctrlKey && 4096 /* Ctrl */)
            : 0;
        return key && commands.filter(function (cmd) { return (roosterjs_editor_dom_1.Browser.isMac ? cmd.macKey : cmd.winKey) == key; })[0];
    });
}
/**
 * @internal
 */
exports.ShortcutFeatures = {
    defaultShortcut: DefaultShortcut,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/structuredNodeFeatures.ts":
/*!******************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/structuredNodeFeatures.ts ***!
  \******************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
// Edge can sometimes lose current format when Enter to new line.
// So here we add an extra SPAN for Edge to workaround this bug
var NEWLINE_HTML = roosterjs_editor_dom_1.Browser.isEdge ? '<div><span><br></span></div>' : '<div><br></div>';
var CHILD_PARENT_TAG_MAP = {
    TD: 'TABLE',
    TH: 'TABLE',
    LI: 'OL,UL',
};
var CHILD_SELECTOR = Object.keys(CHILD_PARENT_TAG_MAP).join(',');
/**
 * InsertLineBeforeStructuredNode edit feature, provides the ability to insert an empty line before
 * a structured element (bullet/numbering list, blockquote, table) if the element is at beginning of
 * document
 */
var InsertLineBeforeStructuredNodeFeature = {
    keys: [13 /* ENTER */],
    shouldHandleEvent: cacheGetStructuredElement,
    handleEvent: function (event, editor) {
        var element = cacheGetStructuredElement(event, editor);
        var div = roosterjs_editor_dom_1.fromHtml(NEWLINE_HTML, editor.getDocument())[0];
        editor.addUndoSnapshot(function () {
            element.parentNode.insertBefore(div, element);
            // Select the new line when we are in table. This is the same behavior with Word
            if (roosterjs_editor_dom_1.getTagOfNode(element) == 'TABLE') {
                editor.select(new roosterjs_editor_dom_1.Position(div, 0 /* Begin */).normalize());
            }
        });
        event.rawEvent.preventDefault();
    },
    defaultDisabled: true,
};
function cacheGetStructuredElement(event, editor) {
    return roosterjs_editor_dom_1.cacheGetEventData(event, 'FIRST_STRUCTURE', function () {
        // Provide a chance to keep browser default behavior by pressing SHIFT
        var element = event.rawEvent.shiftKey ? null : editor.getElementAtCursor(CHILD_SELECTOR);
        if (element) {
            var range = editor.getSelectionRange();
            if (range &&
                range.collapsed &&
                roosterjs_editor_dom_1.isPositionAtBeginningOf(roosterjs_editor_dom_1.Position.getStart(range), element) &&
                !editor.getBodyTraverser(element).getPreviousBlockElement()) {
                return editor.getElementAtCursor(CHILD_PARENT_TAG_MAP[roosterjs_editor_dom_1.getTagOfNode(element)]);
            }
        }
        return null;
    });
}
/**
 * @internal
 */
exports.StructuredNodeFeatures = {
    insertLineBeforeStructuredNodeFeature: InsertLineBeforeStructuredNodeFeature,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/tableFeatures.ts":
/*!*********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/tableFeatures.ts ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * TabInTable edit feature, provides the ability to jump between cells when user press TAB in table
 */
var TabInTable = {
    keys: [9 /* TAB */],
    shouldHandleEvent: cacheGetTableCell,
    handleEvent: function (event, editor) {
        var shift = event.rawEvent.shiftKey;
        var td = cacheGetTableCell(event, editor);
        for (var vtable = new roosterjs_editor_dom_1.VTable(td), step = shift ? -1 : 1, row = vtable.row, col = vtable.col + step;; col += step) {
            if (col < 0 || col >= vtable.cells[row].length) {
                row += step;
                if (row < 0) {
                    editor.select(vtable.table, -2 /* Before */);
                    break;
                }
                else if (row >= vtable.cells.length) {
                    roosterjs_editor_api_1.editTable(editor, 1 /* InsertBelow */);
                    break;
                }
                col = shift ? vtable.cells[row].length - 1 : 0;
            }
            var cell = vtable.getCell(row, col);
            if (cell.td) {
                editor.select(cell.td, 0 /* Begin */);
                break;
            }
        }
        event.rawEvent.preventDefault();
    },
};
/**
 * UpDownInTable edit feature, provides the ability to jump to cell above/below when user press UP/DOWN
 * in table
 */
var UpDownInTable = {
    keys: [38 /* UP */, 40 /* DOWN */],
    shouldHandleEvent: cacheGetTableCell,
    handleEvent: function (event, editor) {
        var _a;
        var td = cacheGetTableCell(event, editor);
        var vtable = new roosterjs_editor_dom_1.VTable(td);
        var isUp = event.rawEvent.which == 38 /* UP */;
        var step = isUp ? -1 : 1;
        var hasShiftKey = event.rawEvent.shiftKey;
        var selection = (_a = editor.getDocument().defaultView) === null || _a === void 0 ? void 0 : _a.getSelection();
        var targetTd = null;
        if (selection) {
            var anchorNode_1 = selection.anchorNode, anchorOffset_1 = selection.anchorOffset;
            for (var row = vtable.row; row >= 0 && row < vtable.cells.length; row += step) {
                var cell = vtable.getCell(row, vtable.col);
                if (cell.td && cell.td != td) {
                    targetTd = cell.td;
                    break;
                }
            }
            editor.runAsync(function (editor) {
                var _a;
                var newContainer = editor.getElementAtCursor();
                if (roosterjs_editor_dom_1.contains(vtable.table, newContainer) &&
                    !roosterjs_editor_dom_1.contains(td, newContainer, true /*treatSameNodeAsContain*/)) {
                    var newPos = targetTd
                        ? new roosterjs_editor_dom_1.Position(targetTd, 0 /* Begin */)
                        : new roosterjs_editor_dom_1.Position(vtable.table, isUp ? -2 /* Before */ : -3 /* After */);
                    if (hasShiftKey) {
                        newPos =
                            newPos.node.nodeType == 1 /* Element */ &&
                                roosterjs_editor_dom_1.isVoidHtmlElement(newPos.node)
                                ? new roosterjs_editor_dom_1.Position(newPos.node, newPos.isAtEnd ? -3 /* After */ : -2 /* Before */)
                                : newPos;
                        var selection_1 = (_a = editor.getDocument().defaultView) === null || _a === void 0 ? void 0 : _a.getSelection();
                        selection_1 === null || selection_1 === void 0 ? void 0 : selection_1.setBaseAndExtent(anchorNode_1, anchorOffset_1, newPos.node, newPos.offset);
                    }
                    else {
                        editor.select(newPos);
                    }
                }
            });
        }
    },
    defaultDisabled: !roosterjs_editor_dom_1.Browser.isChrome && !roosterjs_editor_dom_1.Browser.isSafari,
};
function cacheGetTableCell(event, editor) {
    return roosterjs_editor_dom_1.cacheGetEventData(event, 'TABLECELL_FOR_TABLE_FEATURES', function () {
        var pos = editor.getFocusedPosition();
        var firstTd = pos && editor.getElementAtCursor('TD,TH,LI', pos.node);
        return (firstTd && (roosterjs_editor_dom_1.getTagOfNode(firstTd) == 'LI' ? null : firstTd));
    });
}
/**
 * @internal
 */
exports.TableFeatures = {
    tabInTable: TabInTable,
    upDownInTable: UpDownInTable,
};


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/getAllFeatures.ts":
/*!*************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/getAllFeatures.ts ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var autoLinkFeatures_1 = __webpack_require__(/*! ./features/autoLinkFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/autoLinkFeatures.ts");
var cursorFeatures_1 = __webpack_require__(/*! ./features/cursorFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/cursorFeatures.ts");
var entityFeatures_1 = __webpack_require__(/*! ./features/entityFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/entityFeatures.ts");
var listFeatures_1 = __webpack_require__(/*! ./features/listFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/listFeatures.ts");
var markdownFeatures_1 = __webpack_require__(/*! ./features/markdownFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/markdownFeatures.ts");
var quoteFeatures_1 = __webpack_require__(/*! ./features/quoteFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/quoteFeatures.ts");
var shortcutFeatures_1 = __webpack_require__(/*! ./features/shortcutFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/shortcutFeatures.ts");
var structuredNodeFeatures_1 = __webpack_require__(/*! ./features/structuredNodeFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/structuredNodeFeatures.ts");
var tableFeatures_1 = __webpack_require__(/*! ./features/tableFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/features/tableFeatures.ts");
var allFeatures = __assign(__assign(__assign(__assign(__assign(__assign(__assign(__assign(__assign({}, listFeatures_1.ListFeatures), quoteFeatures_1.QuoteFeatures), tableFeatures_1.TableFeatures), structuredNodeFeatures_1.StructuredNodeFeatures), autoLinkFeatures_1.AutoLinkFeatures), shortcutFeatures_1.ShortcutFeatures), cursorFeatures_1.CursorFeatures), markdownFeatures_1.MarkdownFeatures), entityFeatures_1.EntityFeatures);
/**
 * Get all content edit features provided by roosterjs
 */
function getAllFeatures() {
    return allFeatures;
}
exports.default = getAllFeatures;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/index.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/index.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContentEdit_1 = __webpack_require__(/*! ./ContentEdit */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/ContentEdit.ts");
exports.ContentEdit = ContentEdit_1.default;
var getAllFeatures_1 = __webpack_require__(/*! ./getAllFeatures */ "./packages/roosterjs-editor-plugins/lib/plugins/ContentEdit/getAllFeatures.ts");
exports.getAllFeatures = getAllFeatures_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/ContextMenu.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/ContextMenu.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var CONTAINER_HTML = '<div style="position: fixed; width: 0; height: 0"></div>';
/**
 * An editor plugin that support showing a context menu using render() function from options parameter
 */
var ContextMenu = /** @class */ (function () {
    /**
     * Create a new instance of ContextMenu class
     * @param options An options object to determine how to show/hide the context menu
     */
    function ContextMenu(options) {
        var _this = this;
        this.options = options;
        this.onDismiss = function () {
            var _a, _b;
            if (_this.container && _this.isMenuShowing) {
                (_b = (_a = _this.options).dismiss) === null || _b === void 0 ? void 0 : _b.call(_a, _this.container);
                _this.isMenuShowing = false;
            }
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    ContextMenu.prototype.getName = function () {
        return 'ContextMenu';
    };
    /**
     * Initialize this plugin
     * @param editor The editor instance
     */
    ContextMenu.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    ContextMenu.prototype.dispose = function () {
        this.onDismiss();
        if (this.container) {
            this.container.parentNode.removeChild(this.container);
            this.container = null;
        }
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    ContextMenu.prototype.onPluginEvent = function (event) {
        if (event.eventType == 16 /* ContextMenu */ && event.items.length > 0) {
            var rawEvent = event.rawEvent, items = event.items;
            this.onDismiss();
            if (!this.options.allowDefaultMenu) {
                rawEvent.preventDefault();
            }
            this.initContainer(rawEvent.pageX, rawEvent.pageY);
            this.options.render(this.container, items, this.onDismiss);
            this.isMenuShowing = true;
        }
    };
    ContextMenu.prototype.initContainer = function (x, y) {
        if (!this.container) {
            this.container = roosterjs_editor_dom_1.fromHtml(CONTAINER_HTML, this.editor.getDocument())[0];
            this.editor.insertNode(this.container, {
                position: 4 /* Outside */,
            });
        }
        this.container.style.left = x + 'px';
        this.container.style.top = y + 'px';
    };
    return ContextMenu;
}());
exports.default = ContextMenu;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/index.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/index.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContextMenu_1 = __webpack_require__(/*! ./ContextMenu */ "./packages/roosterjs-editor-plugins/lib/plugins/ContextMenu/ContextMenu.ts");
exports.ContextMenu = ContextMenu_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/CustomReplace.ts":
/*!**************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/CustomReplace.ts ***!
  \**************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var makeReplacement = function (sourceString, replacementHTML, matchSourceCaseSensitive) { return ({ sourceString: sourceString, replacementHTML: replacementHTML, matchSourceCaseSensitive: matchSourceCaseSensitive }); };
var defaultReplacements = [
    makeReplacement(':)', '🙂', true),
    makeReplacement(';)', '😉', true),
    makeReplacement(':O', '😲', true),
    makeReplacement(':o', '😯', true),
    makeReplacement('<3', '❤️', true),
];
/**
 * Wrapper for CustomReplaceContentEditFeature that provides an API for updating the
 * content edit feature
 */
var CustomReplacePlugin = /** @class */ (function () {
    /**
     * Create instance of CustomReplace plugin
     * @param replacements Replacement rules. If not passed, a default replacement rule set will be applied
     */
    function CustomReplacePlugin(replacements) {
        if (replacements === void 0) { replacements = defaultReplacements; }
        this.updateReplacements(replacements);
    }
    /**
     * Set the replacements that this plugin is looking for.
     * @param newReplacements new set of replacements for this plugin
     */
    CustomReplacePlugin.prototype.updateReplacements = function (newReplacements) {
        this.replacements = newReplacements;
        this.longestReplacementLength = getLongestReplacementSourceLength(this.replacements);
        this.replacementEndCharacters = getReplacementEndCharacters(this.replacements);
    };
    /**
     * Get a friendly name of this plugin
     */
    CustomReplacePlugin.prototype.getName = function () {
        return 'CustomReplace';
    };
    /**
     * Initialize this plugin
     * @param editor The editor instance
     */
    CustomReplacePlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };
    /**
     * Dispose this plugin
     */
    CustomReplacePlugin.prototype.dispose = function () {
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    CustomReplacePlugin.prototype.onPluginEvent = function (event) {
        var _this = this;
        if (this.editor.isInIME() || event.eventType != 3 /* Input */) {
            return;
        }
        // Exit early on input events that do not insert a replacement's final character.
        if (!event.rawEvent.data || !this.replacementEndCharacters.has(event.rawEvent.data)) {
            return;
        }
        // Get the matching replacement
        var range = this.editor.getSelectionRange();
        if (range == null) {
            return;
        }
        var searcher = this.editor.getContentSearcherOfCursor(event);
        var stringToSearch = searcher.getSubStringBefore(this.longestReplacementLength);
        var replacement = this.getMatchingReplacement(stringToSearch);
        if (replacement == null) {
            return;
        }
        // Reconstruct a selection of the text on the document that matches the
        // replacement we selected.
        var matchingText = searcher.getSubStringBefore(replacement.sourceString.length);
        var matchingRange = searcher.getRangeFromText(matchingText, true /* exactMatch */);
        // parse the html string off the dom and inline the resulting element.
        var document = this.editor.getDocument();
        var parsingSpan = document.createElement('span');
        parsingSpan.innerHTML = replacement.replacementHTML;
        var nodeToInsert = parsingSpan.childNodes.length == 1 ? parsingSpan.childNodes[0] : parsingSpan;
        // Switch the node for the selection range
        this.editor.addUndoSnapshot(function () {
            matchingRange.deleteContents();
            matchingRange.insertNode(nodeToInsert);
            _this.editor.select(nodeToInsert, -1 /* End */);
        }, null /*changeSource*/, true /*canUndoByBackspace*/);
    };
    CustomReplacePlugin.prototype.getMatchingReplacement = function (stringToSearch) {
        if (stringToSearch.length == 0) {
            return null;
        }
        var lowerCaseStringToSearch = stringToSearch.toLocaleLowerCase();
        for (var _i = 0, _a = this.replacements; _i < _a.length; _i++) {
            var replacement = _a[_i];
            var _b = replacement.matchSourceCaseSensitive
                ? [stringToSearch, replacement.sourceString]
                : [lowerCaseStringToSearch, replacement.sourceString.toLocaleLowerCase()], sourceMatch = _b[0], replacementMatch = _b[1];
            if (sourceMatch.substring(sourceMatch.length - replacementMatch.length) ==
                replacementMatch) {
                return replacement;
            }
        }
        return null;
    };
    return CustomReplacePlugin;
}());
exports.default = CustomReplacePlugin;
function getLongestReplacementSourceLength(replacements) {
    return Math.max.apply(null, replacements.map(function (replacement) { return replacement.sourceString.length; }));
}
function getReplacementEndCharacters(replacements) {
    var endChars = new Set();
    for (var _i = 0, replacements_1 = replacements; _i < replacements_1.length; _i++) {
        var replacement = replacements_1[_i];
        var sourceString = replacement.sourceString;
        if (sourceString.length == 0) {
            continue;
        }
        var lastChar = sourceString[sourceString.length - 1];
        if (!replacement.matchSourceCaseSensitive) {
            endChars.add(lastChar.toLocaleLowerCase());
            endChars.add(lastChar.toLocaleUpperCase());
        }
        else {
            endChars.add(lastChar);
        }
    }
    return endChars;
}


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/index.ts":
/*!******************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/index.ts ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CustomReplace_1 = __webpack_require__(/*! ./CustomReplace */ "./packages/roosterjs-editor-plugins/lib/plugins/CustomReplace/CustomReplace.ts");
exports.CustomReplace = CustomReplace_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/CutPasteListChain.ts":
/*!**********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/CutPasteListChain.ts ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Maintain list numbers of list chain when content is modified by cut/paste/drag&drop
 */
var CutPasteListChain = /** @class */ (function () {
    function CutPasteListChain() {
        var _this = this;
        this.onDrop = function () {
            _this.cacheListChains("Drop" /* Drop */);
        };
    }
    /**
     * Get a friendly name of this plugin
     */
    CutPasteListChain.prototype.getName = function () {
        return 'CutPasteListChain';
    };
    /**
     * Initialize this plugin
     * @param editor The editor instance
     */
    CutPasteListChain.prototype.initialize = function (editor) {
        this.editor = editor;
        this.disposer = this.editor.addDomEventHandler('drop', this.onDrop);
    };
    /**
     * Dispose this plugin
     */
    CutPasteListChain.prototype.dispose = function () {
        var _a;
        (_a = this.disposer) === null || _a === void 0 ? void 0 : _a.call(this);
        this.disposer = null;
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    CutPasteListChain.prototype.onPluginEvent = function (event) {
        var _a;
        switch (event.eventType) {
            case 9 /* BeforeCutCopy */:
                if (event.isCut) {
                    this.cacheListChains("Cut" /* Cut */);
                }
                break;
            case 10 /* BeforePaste */:
                this.cacheListChains("Paste" /* Paste */);
                break;
            case 7 /* ContentChanged */:
                if (((_a = this.chains) === null || _a === void 0 ? void 0 : _a.length) > 0 && this.expectedChangeSource == event.source) {
                    roosterjs_editor_api_1.experimentCommitListChains(this.editor, this.chains);
                    this.chains = null;
                    this.expectedChangeSource = null;
                }
                break;
        }
    };
    CutPasteListChain.prototype.cacheListChains = function (source) {
        this.chains = roosterjs_editor_dom_1.VListChain.createListChains(this.editor.getSelectedRegions());
        this.expectedChangeSource = source;
    };
    return CutPasteListChain;
}());
exports.default = CutPasteListChain;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/index.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/index.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var CutPasteListChain_1 = __webpack_require__(/*! ./CutPasteListChain */ "./packages/roosterjs-editor-plugins/lib/plugins/CutPasteListChain/CutPasteListChain.ts");
exports.CutPasteListChain = CutPasteListChain_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/HyperLink.ts":
/*!******************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/HyperLink.ts ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * An editor plugin that show a tooltip for existing link
 */
var HyperLink = /** @class */ (function () {
    /**
     * Create a new instance of HyperLink class
     * @param getTooltipCallback A callback function to get tooltip text for an existing hyperlink.
     * Default value is to return the href itself. If null, there will be no tooltip text.
     * @param target (Optional) Target window name for hyperlink. If null, will use "_blank"
     * @param onLinkClick (Optional) Open link callback (return false to use default behavior)
     */
    function HyperLink(getTooltipCallback, target, onLinkClick) {
        var _this = this;
        if (getTooltipCallback === void 0) { getTooltipCallback = function (href) { return href; }; }
        this.getTooltipCallback = getTooltipCallback;
        this.target = target;
        this.onLinkClick = onLinkClick;
        this.trackedLink = null;
        this.onMouse = function (e) {
            var a = _this.editor.getElementAtCursor('a[href]', e.target);
            var href = _this.tryGetHref(a);
            if (href) {
                _this.editor.setEditorDomAttribute('title', e.type == 'mouseover' ? _this.getTooltipCallback(href, a) : null);
            }
        };
        this.onBlur = function (e) {
            if (_this.trackedLink) {
                _this.updateLinkHrefIfShouldUpdate();
            }
            _this.resetLinkTracking();
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    HyperLink.prototype.getName = function () {
        return 'Hyperlink';
    };
    /**
     * Initialize this plugin
     * @param editor The editor instance
     */
    HyperLink.prototype.initialize = function (editor) {
        this.editor = editor;
        this.disposer =
            this.getTooltipCallback &&
                editor.addDomEventHandler({
                    mouseover: this.onMouse,
                    mouseout: this.onMouse,
                    blur: this.onBlur,
                });
    };
    /**
     * Dispose this plugin
     */
    HyperLink.prototype.dispose = function () {
        if (this.disposer) {
            this.disposer();
            this.disposer = null;
        }
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    HyperLink.prototype.onPluginEvent = function (event) {
        if (event.eventType == 6 /* MouseUp */ ||
            (event.eventType == 2 /* KeyUp */ &&
                (!this.isContentEditValue(event.rawEvent) || event.rawEvent.which == 32 /* SPACE */)) ||
            event.eventType == 7 /* ContentChanged */) {
            var anchor = this.editor.getElementAtCursor('A[href]', null /*startFrom*/, event);
            var shouldCheckUpdateLink = anchor !== this.trackedLink ||
                event.eventType == 2 /* KeyUp */ ||
                event.eventType == 7 /* ContentChanged */;
            if (this.trackedLink &&
                (shouldCheckUpdateLink || this.tryGetHref(this.trackedLink) !== this.originalHref)) {
                // If cursor has moved out of previously tracked link
                // update link href if display text doesn't match href anymore.
                if (shouldCheckUpdateLink) {
                    this.updateLinkHrefIfShouldUpdate();
                }
                // If the link's href value was edited, or the cursor has moved out of the
                // previously tracked link, stop tracking the link.
                this.resetLinkTracking();
            }
            // Cache link and href value if its href attribute currently matches its display text
            if (!this.trackedLink && this.doesLinkDisplayMatchHref(anchor)) {
                this.trackedLink = anchor;
                this.originalHref = this.tryGetHref(anchor);
            }
        }
        if (event.eventType == 6 /* MouseUp */) {
            var anchor = this.editor.getElementAtCursor('A', event.rawEvent.srcElement);
            if (anchor) {
                if (this.onLinkClick && this.onLinkClick(anchor, event.rawEvent) !== false) {
                    return;
                }
                var href = void 0;
                if (!roosterjs_editor_dom_1.Browser.isFirefox &&
                    (href = this.tryGetHref(anchor)) &&
                    roosterjs_editor_dom_1.isCtrlOrMetaPressed(event.rawEvent) &&
                    event.rawEvent.button === 0) {
                    try {
                        var target = this.target || '_blank';
                        var window_1 = this.editor.getDocument().defaultView;
                        window_1.open(href, target);
                    }
                    catch (_a) { }
                }
            }
        }
    };
    /**
     * Try get href from an anchor element
     * The reason this is put in a try-catch is that
     * it has been seen that accessing href may throw an exception, in particular on IE/Edge
     */
    HyperLink.prototype.tryGetHref = function (anchor) {
        try {
            return anchor ? anchor.href : null;
        }
        catch (_a) { }
    };
    /**
     * Determines if KeyboardEvent is meant to edit content
     */
    HyperLink.prototype.isContentEditValue = function (event) {
        return (roosterjs_editor_dom_1.isCharacterValue(event) || event.which == 8 /* BACKSPACE */ || event.which == 46 /* DELETE */);
    };
    /**
     * Updates the href of the tracked link if the display text doesn't match href anymore
     */
    HyperLink.prototype.updateLinkHrefIfShouldUpdate = function () {
        if (!this.doesLinkDisplayMatchHref(this.trackedLink)) {
            this.updateLinkHref();
        }
    };
    /**
     * Clears the tracked link and its original href value so that it's back to default state
     */
    HyperLink.prototype.resetLinkTracking = function () {
        this.trackedLink = null;
        this.originalHref = '';
    };
    /**
     * Compares the normalized URL of inner text of element to its href to see if they match.
     */
    HyperLink.prototype.doesLinkDisplayMatchHref = function (element) {
        if (element) {
            var display = element.innerText.trim();
            // We first escape the display text so that any text passed into the regex is not
            // treated as a special character.
            var escapedDisplay = display.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
            var rule = new RegExp("^(?:https?:\\/\\/)?" + escapedDisplay + "\\/?", 'i');
            var href = this.tryGetHref(element);
            if (href !== null) {
                return rule.test(href);
            }
        }
        return false;
    };
    /**
     * Update href of an element in place to new display text if it's a valid URL
     */
    HyperLink.prototype.updateLinkHref = function () {
        var _this = this;
        if (this.trackedLink) {
            var linkData_1 = roosterjs_editor_dom_1.matchLink(this.trackedLink.innerText.trim());
            if (linkData_1 !== null) {
                this.editor.addUndoSnapshot(function () {
                    _this.trackedLink.href = linkData_1.normalizedUrl;
                });
            }
        }
    };
    return HyperLink;
}());
exports.default = HyperLink;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/index.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/index.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HyperLink_1 = __webpack_require__(/*! ./HyperLink */ "./packages/roosterjs-editor-plugins/lib/plugins/HyperLink/HyperLink.ts");
exports.HyperLink = HyperLink_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/ImageResize.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/ImageResize.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var DELETE_KEYCODE = 46;
var BACKSPACE_KEYCODE = 8;
var SHIFT_KEYCODE = 16;
var CTRL_KEYCODE = 17;
var ALT_KEYCODE = 18;
var ENTITY_TYPE = 'IMAGE_RESIZE_WRAPPER';
var HANDLE_SIZE = 7;
var HANDLE_MARGIN = 3;
var CORNER_HANDLE_POSITIONS = ['nw', 'ne', 'se', 'sw'];
var SIDE_HANDLE_POSITIONS = ['n', 'e', 's', 'w'];
var ALL_HANDLE_POSITIONS = CORNER_HANDLE_POSITIONS.concat(SIDE_HANDLE_POSITIONS);
/**
 * ImageResize plugin provides the ability to resize an inline image in editor
 */
var ImageResize = /** @class */ (function () {
    /**
     * Create a new instance of ImageResize
     * @param minWidth Minimum width of image when resize in pixel, default value is 10
     * @param minHeight Minimum height of image when resize in pixel, default value is 10
     * @param selectionBorderColor Color of resize border and handles, default value is #DB626C
     * @param forcePreserveRatio Whether always preserve width/height ratio when resize, default value is false
     * @param resizableImageSelector Selector for picking which image is resizable (e.g. for all images not placeholders), note
     * that the tag must be IMG regardless what the selector is
     */
    function ImageResize(minWidth, minHeight, selectionBorderColor, forcePreserveRatio, resizableImageSelector) {
        var _this = this;
        if (minWidth === void 0) { minWidth = 10; }
        if (minHeight === void 0) { minHeight = 10; }
        if (selectionBorderColor === void 0) { selectionBorderColor = '#DB626C'; }
        if (forcePreserveRatio === void 0) { forcePreserveRatio = false; }
        if (resizableImageSelector === void 0) { resizableImageSelector = 'img'; }
        this.minWidth = minWidth;
        this.minHeight = minHeight;
        this.selectionBorderColor = selectionBorderColor;
        this.forcePreserveRatio = forcePreserveRatio;
        this.resizableImageSelector = resizableImageSelector;
        this.startResize = function (e) {
            var img = _this.getSelectedImage();
            if (_this.editor && img) {
                _this.startPageX = e.pageX;
                _this.startPageY = e.pageY;
                _this.startWidth = img.clientWidth;
                _this.startHeight = img.clientHeight;
                _this.editor.addUndoSnapshot();
                var document_1 = _this.editor.getDocument();
                document_1.addEventListener('mousemove', _this.doResize, true /*useCapture*/);
                document_1.addEventListener('mouseup', _this.finishResize, true /*useCapture*/);
                _this.direction = (e.srcElement || e.target).dataset.direction;
            }
            _this.stopEvent(e);
        };
        this.doResize = function (e) {
            var img = _this.getSelectedImage();
            if (_this.editor && img) {
                var widthChange = e.pageX - _this.startPageX;
                var heightChange = e.pageY - _this.startPageY;
                var newWidth = _this.calculateNewWidth(widthChange);
                var newHeight = _this.calculateNewHeight(heightChange);
                var isSingleDirection = _this.isSingleDirectionNS(_this.direction) ||
                    _this.isSingleDirectionWE(_this.direction);
                var shouldPreserveRatio = !isSingleDirection && (_this.forcePreserveRatio || e.shiftKey);
                if (shouldPreserveRatio) {
                    newHeight = Math.min(newHeight, (newWidth * _this.startHeight) / _this.startWidth);
                    newWidth = Math.min(newWidth, (newHeight * _this.startWidth) / _this.startHeight);
                    var ratio = _this.startWidth > 0 && _this.startHeight > 0
                        ? (_this.startWidth * 1.0) / _this.startHeight
                        : 0;
                    if (ratio > 0) {
                        if (newWidth < newHeight * ratio) {
                            newWidth = newHeight * ratio;
                        }
                        else {
                            newHeight = newWidth / ratio;
                        }
                    }
                }
                img.style.width = newWidth + 'px';
                img.style.height = newHeight + 'px';
                // double check
                if (shouldPreserveRatio) {
                    var ratio = _this.startWidth > 0 && _this.startHeight > 0
                        ? (_this.startWidth * 1.0) / _this.startHeight
                        : 0;
                    var clientWidth = Math.floor(img.clientWidth);
                    var clientHeight = Math.floor(img.clientHeight);
                    newWidth = Math.floor(newWidth);
                    newHeight = Math.floor(newHeight);
                    if (clientHeight !== newHeight || clientWidth !== newWidth) {
                        if (clientHeight < newHeight) {
                            newWidth = clientHeight * ratio;
                        }
                        else {
                            newHeight = clientWidth / ratio;
                        }
                        img.style.width = newWidth + 'px';
                        img.style.height = newHeight + 'px';
                    }
                }
            }
            _this.stopEvent(e);
        };
        this.finishResize = function (e) {
            var img = _this.getSelectedImage();
            if (_this.editor && img) {
                var document_2 = _this.editor.getDocument();
                document_2.removeEventListener('mousemove', _this.doResize, true /*useCapture*/);
                document_2.removeEventListener('mouseup', _this.finishResize, true /*useCapture*/);
                var width = img.clientWidth;
                var height = img.clientHeight;
                img.style.width = width + 'px';
                img.style.height = height + 'px';
                img.width = width;
                img.height = height;
                _this.resizeDiv.style.width = '';
                _this.resizeDiv.style.height = '';
            }
            _this.direction = null;
            _this.editor.addUndoSnapshot();
            _this.editor.triggerContentChangedEvent("ImageResize" /* ImageResize */, img);
            _this.stopEvent(e);
        };
        this.stopEvent = function (e) {
            e.stopPropagation();
            e.preventDefault();
        };
        this.removeResizeDiv = function (resizeDiv) {
            if (resizeDiv === null || resizeDiv === void 0 ? void 0 : resizeDiv.parentNode) {
                var img = resizeDiv.querySelector('img');
                if (img) {
                    resizeDiv.parentNode.insertBefore(img, resizeDiv);
                }
                resizeDiv.parentNode.removeChild(resizeDiv);
                return img;
            }
            else {
                return null;
            }
        };
        this.onBlur = function (e) {
            _this.hideResizeHandle();
        };
        this.onDragStart = function (e) {
            if ((e.srcElement || e.target) == _this.getSelectedImage()) {
                _this.hideResizeHandle(true);
            }
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    ImageResize.prototype.getName = function () {
        return 'ImageResize';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    ImageResize.prototype.initialize = function (editor) {
        this.editor = editor;
        this.disposer = editor.addDomEventHandler({
            dragstart: this.onDragStart,
            blur: this.onBlur,
        });
    };
    /**
     * Dispose this plugin
     */
    ImageResize.prototype.dispose = function () {
        this.hideResizeHandle();
        this.disposer();
        this.disposer = null;
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    ImageResize.prototype.onPluginEvent = function (e) {
        var _this = this;
        var _a;
        if (e.eventType == 5 /* MouseDown */) {
            if (this.resizeDiv) {
                this.hideResizeHandle();
            }
        }
        else if (e.eventType == 6 /* MouseUp */) {
            var event_1 = e.rawEvent;
            var target = (event_1.srcElement || event_1.target);
            if (roosterjs_editor_dom_1.getTagOfNode(target) == 'IMG' && target.isContentEditable) {
                var parent_1 = target.parentNode;
                var elements = parent_1
                    ? roosterjs_editor_dom_1.toArray(parent_1.querySelectorAll(this.resizableImageSelector))
                    : [];
                if (elements.indexOf(target) < 0) {
                    return;
                }
                var currentImg = this.getSelectedImage();
                if (currentImg && currentImg != target) {
                    this.hideResizeHandle();
                }
                if (!this.resizeDiv) {
                    this.showResizeHandle(target);
                }
            }
        }
        else if (e.eventType == 0 /* KeyDown */ && this.resizeDiv) {
            var event_2 = e.rawEvent;
            if (event_2.which == DELETE_KEYCODE || event_2.which == BACKSPACE_KEYCODE) {
                this.editor.addUndoSnapshot(function () {
                    _this.editor.deleteNode(_this.resizeDiv);
                });
                this.resizeDiv = null;
                event_2.preventDefault();
            }
            else if (event_2.which != SHIFT_KEYCODE &&
                event_2.which != CTRL_KEYCODE &&
                event_2.which != ALT_KEYCODE) {
                this.hideResizeHandle(true /*selectImage*/);
            }
        }
        else if (e.eventType == 7 /* ContentChanged */ &&
            e.source != "ImageResize" /* ImageResize */ &&
            (e.source != "InsertEntity" /* InsertEntity */ || ((_a = e.data) === null || _a === void 0 ? void 0 : _a.type) != ENTITY_TYPE)) {
            this.editor.queryElements(roosterjs_editor_dom_1.getEntitySelector(ENTITY_TYPE), this.removeResizeDiv);
            this.resizeDiv = null;
        }
        else if (e.eventType == 15 /* EntityOperation */ && e.entity.type == ENTITY_TYPE) {
            if (e.operation == 8 /* ReplaceTemporaryContent */) {
                this.removeResizeDiv(e.entity.wrapper);
            }
            else if (e.operation == 1 /* Click */) {
                this.stopEvent(e.rawEvent);
            }
        }
    };
    /**
     * Select a given IMG element, show the resize handle
     * @param img The IMG element to select
     */
    ImageResize.prototype.showResizeHandle = function (img) {
        this.resizeDiv = this.createResizeDiv(img);
        this.editor.select(this.resizeDiv, -3 /* After */);
    };
    /**
     * Hide resize handle of current selected image
     * @param selectImageAfterUnSelect Optional, when set to true, select the image element after hide the resize handle
     */
    ImageResize.prototype.hideResizeHandle = function (selectImageAfterUnSelect) {
        if (this.resizeDiv) {
            var transform = this.resizeDiv.style.transform;
            var img = this.removeResizeDiv(this.resizeDiv);
            if (img) {
                img.style.transform = transform;
                if (selectImageAfterUnSelect) {
                    this.editor.select(img);
                }
            }
            this.resizeDiv = null;
        }
    };
    ImageResize.prototype.calculateNewWidth = function (widthChange) {
        var newWidth = this.startWidth;
        if (!this.isSingleDirectionNS(this.direction)) {
            newWidth = Math.max(this.startWidth + (this.isWest(this.direction) ? -widthChange : widthChange), this.minWidth);
        }
        return newWidth;
    };
    ImageResize.prototype.calculateNewHeight = function (heightChange) {
        var newHeight = this.startHeight;
        if (!this.isSingleDirectionWE(this.direction)) {
            newHeight = Math.max(this.startHeight + (this.isNorth(this.direction) ? -heightChange : heightChange), this.minHeight);
        }
        return newHeight;
    };
    ImageResize.prototype.createResizeDiv = function (target) {
        var _this = this;
        var wrapper = roosterjs_editor_api_1.insertEntity(this.editor, ENTITY_TYPE, target, false /*isBlock*/, true /*isReadonly*/).wrapper;
        wrapper.style.position = 'relative';
        wrapper.style.display = roosterjs_editor_dom_1.Browser.isSafari ? 'inline-block' : 'inline-flex';
        var html = (this.editor.isFeatureEnabled("SingleDirectionResize" /* SingleDirectionResize */)
            ? ALL_HANDLE_POSITIONS
            : CORNER_HANDLE_POSITIONS)
            .map(function (pos) {
            return "<div style=\"position:absolute;" + (_this.isWest(pos) ? 'left' : 'right') + ":" + (_this.isSingleDirectionNS(pos) ? '50%' : '0px') + ";" + (_this.isNorth(pos) ? 'top' : 'bottom') + ":" + (_this.isSingleDirectionWE(pos) ? '50%' : '0px') + "\">\n                            <div id=" + pos + "-handle data-direction=\"" + pos + "\" style=\"position:relative;width:" + HANDLE_SIZE + "px;height:" + HANDLE_SIZE + "px;background-color: " + _this.selectionBorderColor + ";cursor: " + pos + "-resize;" + (_this.isNorth(pos) ? 'top' : 'bottom') + ":-" + HANDLE_MARGIN + "px;" + (_this.isWest(pos) ? 'left' : 'right') + ":-" + HANDLE_MARGIN + "px\"></div></div>";
        })
            .join('') +
            ("<div style=\"position:absolute;left:0;right:0;top:0;bottom:0;border:solid 1px " + this.selectionBorderColor + ";pointer-events:none;\">");
        roosterjs_editor_dom_1.fromHtml(html, this.editor.getDocument()).forEach(function (div) {
            wrapper.appendChild(div);
            div.addEventListener('mousedown', _this.startResize);
        });
        // If the resizeDiv's image has a transform, apply it to the container
        var selectedImage = this.getSelectedImage(wrapper);
        if (selectedImage && selectedImage.style && selectedImage.style.transform) {
            wrapper.style.transform = selectedImage.style.transform;
            selectedImage.style.transform = '';
        }
        return wrapper;
    };
    ImageResize.prototype.getSelectedImage = function (div) {
        var divWithImage = div || this.resizeDiv;
        return divWithImage ? divWithImage.getElementsByTagName('IMG')[0] : null;
    };
    ImageResize.prototype.isNorth = function (direction) {
        return direction && direction.substr(0, 1) == 'n';
    };
    ImageResize.prototype.isWest = function (direction) {
        return direction && (direction.substr(1, 1) == 'w' || direction == 'w');
    };
    ImageResize.prototype.isSingleDirectionNS = function (direction) {
        return direction && (direction == 'n' || direction == 's');
    };
    ImageResize.prototype.isSingleDirectionWE = function (direction) {
        return direction && (direction == 'w' || direction == 'e');
    };
    return ImageResize;
}());
exports.default = ImageResize;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/index.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/index.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ImageResize_1 = __webpack_require__(/*! ./ImageResize */ "./packages/roosterjs-editor-plugins/lib/plugins/ImageResize/ImageResize.ts");
exports.ImageResize = ImageResize_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/Paste.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/Paste.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var convertPastedContentFromExcel_1 = __webpack_require__(/*! ./excelConverter/convertPastedContentFromExcel */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/excelConverter/convertPastedContentFromExcel.ts");
var convertPastedContentFromPowerPoint_1 = __webpack_require__(/*! ./pptConverter/convertPastedContentFromPowerPoint */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/pptConverter/convertPastedContentFromPowerPoint.ts");
var convertPastedContentFromTeams_1 = __webpack_require__(/*! ./teamsConverter/convertPastedContentFromTeams */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/teamsConverter/convertPastedContentFromTeams.ts");
var convertPastedContentFromWord_1 = __webpack_require__(/*! ./wordConverter/convertPastedContentFromWord */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/convertPastedContentFromWord.ts");
var handleLineMerge_1 = __webpack_require__(/*! ./lineMerge/handleLineMerge */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/lineMerge/handleLineMerge.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var constants_1 = __webpack_require__(/*! ./officeOnlineConverter/constants */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/constants.ts");
var convertPastedContentFromWordOnline_1 = __webpack_require__(/*! ./officeOnlineConverter/convertPastedContentFromWordOnline */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/convertPastedContentFromWordOnline.ts");
var WORD_ATTRIBUTE_NAME = 'xmlns:w';
var WORD_ATTRIBUTE_VALUE = 'urn:schemas-microsoft-com:office:word';
var EXCEL_ATTRIBUTE_NAME = 'xmlns:x';
var EXCEL_ATTRIBUTE_VALUE = 'urn:schemas-microsoft-com:office:excel';
var PROG_ID_NAME = 'ProgId';
var EXCEL_ONLINE_ATTRIBUTE_VALUE = 'Excel.Sheet';
var POWERPOINT_ATTRIBUTE_VALUE = 'PowerPoint.Slide';
var GOOGLE_SHEET_NODE_NAME = 'google-sheets-html-origin';
/**
 * Paste plugin, handles BeforePaste event and reformat some special content, including:
 * 1. Content copied from Word
 * 2. Content copied from Excel
 * 3. Content copied from Word Online or Onenote Online
 */
var Paste = /** @class */ (function () {
    /**
     * Construct a new instance of Paste class
     * @param unknownTagReplacement Replace solution of unknown tags, default behavior is to replace with SPAN
     */
    function Paste(unknownTagReplacement) {
        if (unknownTagReplacement === void 0) { unknownTagReplacement = 'SPAN'; }
        this.unknownTagReplacement = unknownTagReplacement;
    }
    /**
     * Get a friendly name of  this plugin
     */
    Paste.prototype.getName = function () {
        return 'Paste';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    Paste.prototype.initialize = function () { };
    /**
     * Dispose this plugin
     */
    Paste.prototype.dispose = function () { };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    Paste.prototype.onPluginEvent = function (event) {
        if (event.eventType == 10 /* BeforePaste */) {
            var htmlAttributes = event.htmlAttributes, fragment = event.fragment, sanitizingOption = event.sanitizingOption;
            var wacListElements = void 0;
            if (htmlAttributes[WORD_ATTRIBUTE_NAME] == WORD_ATTRIBUTE_VALUE) {
                // Handle HTML copied from Word
                convertPastedContentFromWord_1.default(event);
            }
            else if (htmlAttributes[EXCEL_ATTRIBUTE_NAME] == EXCEL_ATTRIBUTE_VALUE ||
                htmlAttributes[PROG_ID_NAME] == EXCEL_ONLINE_ATTRIBUTE_VALUE) {
                // Handle HTML copied from Excel
                convertPastedContentFromExcel_1.default(event);
            }
            else if (htmlAttributes[PROG_ID_NAME] == POWERPOINT_ATTRIBUTE_VALUE) {
                convertPastedContentFromPowerPoint_1.default(event);
            }
            else if ((wacListElements = roosterjs_editor_dom_1.toArray(fragment.querySelectorAll(constants_1.WAC_IDENTIFING_SELECTOR))) &&
                wacListElements.length > 0) {
                // Once it is known that the document is from WAC
                // We need to remove the display property and margin from all the list item
                wacListElements.forEach(function (el) {
                    el.style.display = null;
                    el.style.margin = null;
                });
                // call conversion function if the pasted content is from word online and
                // has list element in the pasted content.
                if (convertPastedContentFromWordOnline_1.isWordOnlineWithList(fragment)) {
                    convertPastedContentFromWordOnline_1.default(fragment);
                }
            }
            else if (fragment.querySelector(GOOGLE_SHEET_NODE_NAME)) {
                sanitizingOption.additionalTagReplacements[GOOGLE_SHEET_NODE_NAME] = '*';
            }
            else {
                convertPastedContentFromTeams_1.default(fragment);
                handleLineMerge_1.default(fragment);
            }
            // Replace unknown tags with SPAN
            sanitizingOption.unknownTagReplacement = this.unknownTagReplacement;
        }
    };
    return Paste;
}());
exports.default = Paste;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/excelConverter/convertPastedContentFromExcel.ts":
/*!*************************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/excelConverter/convertPastedContentFromExcel.ts ***!
  \*************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var LAST_TD_END_REGEX = /<\/\s*td\s*>((?!<\/\s*tr\s*>)[\s\S])*$/i;
var LAST_TR_END_REGEX = /<\/\s*tr\s*>((?!<\/\s*table\s*>)[\s\S])*$/i;
var LAST_TR_REGEX = /<tr[^>]*>[^<]*/i;
var LAST_TABLE_REGEX = /<table[^>]*>[^<]*/i;
var DEFAULT_BORDER_STYLE = 'solid 1px #d4d4d4';
/**
 * @internal
 * Convert pasted content from Excel, add borders when source doc doesn't have a border
 * @param event The BeforePaste event
 */
function convertPastedContentFromExcel(event) {
    var _a;
    var fragment = event.fragment, sanitizingOption = event.sanitizingOption, htmlBefore = event.htmlBefore, clipboardData = event.clipboardData;
    var html = excelHandler(clipboardData.html, htmlBefore);
    if (clipboardData.html != html) {
        var doc = new DOMParser().parseFromString(html, 'text/html');
        while (fragment.firstChild) {
            fragment.removeChild(fragment.firstChild);
        }
        while ((_a = doc === null || doc === void 0 ? void 0 : doc.body) === null || _a === void 0 ? void 0 : _a.firstChild) {
            fragment.appendChild(doc.body.firstChild);
        }
    }
    roosterjs_editor_dom_1.chainSanitizerCallback(sanitizingOption.elementCallbacks, 'TD', function (element) {
        if (element.style.borderStyle == 'none') {
            element.style.border = DEFAULT_BORDER_STYLE;
        }
        return true;
    });
}
exports.default = convertPastedContentFromExcel;
/**
 * @internal Export for test only
 * @param html Source html
 */
function excelHandler(html, htmlBefore) {
    if (html.match(LAST_TD_END_REGEX)) {
        var trMatch = htmlBefore.match(LAST_TR_REGEX);
        var tr = trMatch ? trMatch[0] : '<TR>';
        html = tr + html + '</TR>';
    }
    if (html.match(LAST_TR_END_REGEX)) {
        var tableMatch = htmlBefore.match(LAST_TABLE_REGEX);
        var table = tableMatch ? tableMatch[0] : '<TABLE>';
        html = table + html + '</TABLE>';
    }
    return html;
}
exports.excelHandler = excelHandler;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/index.ts":
/*!**********************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/index.ts ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Paste_1 = __webpack_require__(/*! ./Paste */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/Paste.ts");
exports.Paste = Paste_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/lineMerge/handleLineMerge.ts":
/*!******************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/lineMerge/handleLineMerge.ts ***!
  \******************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * Process pasted content, if there are multiple blocks that are not wrapped by a shared ancestor node,
 * change the tag of first and last node to be SPAN so that it will be merged into current block
 * @param root Root node of content to process
 */
function handleLineMerge(root) {
    var traverser = roosterjs_editor_dom_1.ContentTraverser.createBodyTraverser(root);
    var blocks = [];
    for (var block = traverser === null || traverser === void 0 ? void 0 : traverser.currentBlockElement; block; block = traverser.getNextBlockElement()) {
        blocks.push({
            start: block.getStartNode(),
            end: block.getEndNode(),
        });
    }
    if (blocks.length > 0) {
        processBlock(blocks[0]);
        processBlock(blocks[blocks.length - 1]);
        checkAndAddBr(root, blocks[0], true /*isFirst*/);
        checkAndAddBr(root, blocks[blocks.length - 1], false /*isFirst*/);
    }
}
exports.default = handleLineMerge;
function processBlock(block) {
    var _a, _b;
    var start = block.start, end = block.end;
    if (start == end && roosterjs_editor_dom_1.getTagOfNode(start) == 'DIV') {
        var node = roosterjs_editor_dom_1.changeElementTag(start, 'SPAN');
        block.start = node;
        block.end = node;
        if (roosterjs_editor_dom_1.getTagOfNode(node.lastChild) == 'BR') {
            node.removeChild(node.lastChild);
        }
    }
    else if (roosterjs_editor_dom_1.getTagOfNode(end) == 'BR') {
        var node = end.ownerDocument.createTextNode('');
        (_a = end.parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(node, end);
        block.end = node;
        (_b = end.parentNode) === null || _b === void 0 ? void 0 : _b.removeChild(end);
    }
}
function checkAndAddBr(root, block, isFirst) {
    var _a;
    var blockElement = roosterjs_editor_dom_1.getBlockElementAtNode(root, block.start);
    var sibling = isFirst
        ? roosterjs_editor_dom_1.getNextLeafSibling(root, block.end)
        : roosterjs_editor_dom_1.getPreviousLeafSibling(root, block.start);
    if (blockElement === null || blockElement === void 0 ? void 0 : blockElement.contains(sibling)) {
        (_a = (isFirst ? block.end : block.start).parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(block.start.ownerDocument.createElement('br'), isFirst ? block.end.nextSibling : block.start);
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/ListItemBlock.ts":
/*!****************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/ListItemBlock.ts ***!
  \****************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Initialize an empty ListItemBlock
 */
function createListItemBlock(listItem) {
    if (listItem === void 0) { listItem = null; }
    return {
        startElement: listItem,
        endElement: listItem,
        insertPositionNode: null,
        listItemContainers: listItem ? [listItem] : [],
    };
}
exports.createListItemBlock = createListItemBlock;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/constants.ts":
/*!************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/constants.ts ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 */
exports.WORD_ORDERED_LIST_SELECTOR = 'div.ListContainerWrapper > ul[class^="BulletListStyle"]';
/**
 * @internal
 */
exports.WORD_UNORDERED_LIST_SELECTOR = 'div.ListContainerWrapper > ol[class^="NumberListStyle"]';
/**
 * @internal
 */
exports.WORD_ONLINE_IDENTIFYING_SELECTOR = exports.WORD_ORDERED_LIST_SELECTOR + "," + exports.WORD_UNORDERED_LIST_SELECTOR;
/**
 * @internal
 */
exports.LIST_CONTAINER_ELEMENT_CLASS_NAME = 'ListContainerWrapper';
/**
 * @internal
 */
exports.UNORDERED_LIST_TAG_NAME = 'UL';
/**
 * @internal
 */
exports.ORDERED_LIST_TAG_NAME = 'OL';
var TEXT_CONTAINER_ELEMENT_CLASS_NAME = 'OutlineElement';
/**
 * @internal
 */
exports.WAC_IDENTIFING_SELECTOR = "ul[class^=\"BulletListStyle\"]>." + TEXT_CONTAINER_ELEMENT_CLASS_NAME + ",ol[class^=\"NumberListStyle\"]>." + TEXT_CONTAINER_ELEMENT_CLASS_NAME;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/convertPastedContentFromWordOnline.ts":
/*!*************************************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/convertPastedContentFromWordOnline.ts ***!
  \*************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ListItemBlock_1 = __webpack_require__(/*! ./ListItemBlock */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/ListItemBlock.ts");
var constants_1 = __webpack_require__(/*! ./constants */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/officeOnlineConverter/constants.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 */
function isWordOnlineWithList(fragment) {
    return !!(fragment && fragment.querySelector(constants_1.WORD_ONLINE_IDENTIFYING_SELECTOR));
}
exports.isWordOnlineWithList = isWordOnlineWithList;
// Word Online pasted content DOM structure as of July 12th 2019
//<html>
//  <body>
//      <div class='OutlineGroup'>  ----------> this layer may exist depend on the content user paste
//          <div class="OutlineElement">  ----------> text content
//              <p></p>
//          </div>
//          <div class="ListItemWrapper">  ----------> list items: for unordered list, all the items on the same level is under the same wrapper
//              <ul>                                       list items in the same list can be divided into different ListItemWrapper
//                  <li></li>                              list items in the same list can also be divided into different Outline Group;
//                  <li></li>
//              </ul>
//          </div>
//      </div>
//      <div class='OutlineGroup'>
//          <div class="ListItemWrapper">  ----------> list items: for ordered list, each items has it's own wrapper
//              <ol>
//                  <li></li>
//              </ol>
//          </div>
//          <div class="ListItemWrapper">
//              <ol>
//                  <li></li>
//              </ol>
//          </div>
//      </div>
//  </body>
//</html>
//
/**
 * @internal
 * Convert text copied from word online into text that's workable with rooster editor
 * @param fragment Document fragment that is being pasted into editor.
 */
function convertPastedContentFromWordOnline(fragment) {
    sanitizeListItemContainer(fragment);
    var listItemBlocks = getListItemBlocks(fragment);
    listItemBlocks.forEach(function (itemBlock) {
        // There are cases where consecutive List Elements are seperated into different divs:
        // <div>
        //   <div>
        //      <ol></ol>
        //   </div>
        //   <div>
        //      <ol></ol>
        //   </div>
        // </div>
        // <div>
        //   <div>
        //      <ol></ol>
        //   </div>
        // </div>
        // in the above case we want to collapse the two root level div into one and unwrap the list item divs.
        // after the following flattening the list will become following:
        //
        // <div>
        //    <ol></ol>
        // </div>
        // <div>
        //    <ol></ol>
        // </div>
        // <div>
        //    <ol></ol>
        // </div>
        // Then we are start processing.
        flattenListBlock(fragment, itemBlock);
        // Find the node to insertBefore, which is next sibling node of the end of a listItemBlock.
        itemBlock.insertPositionNode = itemBlock.endElement.nextSibling;
        var convertedListElement;
        var doc = fragment.ownerDocument;
        itemBlock.listItemContainers.forEach(function (listItemContainer) {
            var listType = getContainerListType(listItemContainer); // list type that is contained by iterator.
            // Initialize processed element with propery listType if this is the first element
            if (!convertedListElement) {
                convertedListElement = doc.createElement(listType);
            }
            // Get all list items(<li>) in the current iterator element.
            var currentListItems = roosterjs_editor_dom_1.toArray(listItemContainer.querySelectorAll('li'));
            currentListItems.forEach(function (item) {
                // If item is in root level and the type of list changes then
                // insert the current list into body and then reinitialize the convertedListElement
                // Word Online is using data-aria-level to determine the the depth of the list item.
                var itemLevel = parseInt(item.getAttribute('data-aria-level'));
                // In first level list, there are cases where a consecutive list item divs may have different list type
                // When that happens we need to insert the processed elements into the document, then change the list type
                // and keep the processing going.
                if (roosterjs_editor_dom_1.getTagOfNode(convertedListElement) != listType && itemLevel == 1) {
                    insertConvertedListToDoc(convertedListElement, fragment, itemBlock);
                    convertedListElement = doc.createElement(listType);
                }
                insertListItem(convertedListElement, item, listType, doc);
            });
        });
        insertConvertedListToDoc(convertedListElement, fragment, itemBlock);
        // Once we finish the process the list items and put them into a list.
        // After inserting the processed element,
        // we need to remove all the non processed node from the parent node.
        var parentContainer = itemBlock.startElement.parentNode;
        if (parentContainer) {
            itemBlock.listItemContainers.forEach(function (listItemContainer) {
                parentContainer.removeChild(listItemContainer);
            });
        }
    });
}
exports.default = convertPastedContentFromWordOnline;
/**
 * The node processing is based on the premise of only ol/ul is in ListContainerWrapper class
 * However the html might be melformed, this function is to split all the other elements out of ListContainerWrapper
 * @param fragment pasted document that contains all the list element.
 */
function sanitizeListItemContainer(fragment) {
    var listItemContainerListEl = roosterjs_editor_dom_1.toArray(fragment.querySelectorAll(constants_1.WORD_ORDERED_LIST_SELECTOR + ", " + constants_1.WORD_UNORDERED_LIST_SELECTOR));
    listItemContainerListEl.forEach(function (el) {
        var replaceRegex = new RegExp("\\b" + constants_1.LIST_CONTAINER_ELEMENT_CLASS_NAME + "\\b", 'g');
        if (el.previousSibling) {
            var prevParent = roosterjs_editor_dom_1.splitParentNode(el, true);
            prevParent.className = prevParent.className.replace(replaceRegex, '');
        }
        if (el.nextSibling) {
            var nextParent = roosterjs_editor_dom_1.splitParentNode(el, false);
            nextParent.className = nextParent.className.replace(replaceRegex, '');
        }
    });
}
/**
 * Take all the list items in the document, and group the consecutive list times in a list block;
 * @param fragment pasted document that contains all the list element.
 */
function getListItemBlocks(fragment) {
    var listElements = fragment.querySelectorAll('.' + constants_1.LIST_CONTAINER_ELEMENT_CLASS_NAME);
    var result = [];
    var curListItemBlock;
    for (var i = 0; i < listElements.length; i++) {
        var curItem = listElements[i];
        if (!curListItemBlock) {
            curListItemBlock = ListItemBlock_1.createListItemBlock(curItem);
        }
        else {
            var listItemContainers = curListItemBlock.listItemContainers;
            var lastItemInCurBlock = listItemContainers[listItemContainers.length - 1];
            if (curItem == lastItemInCurBlock.nextSibling ||
                roosterjs_editor_dom_1.getFirstLeafNode(curItem) ==
                    roosterjs_editor_dom_1.getNextLeafSibling(lastItemInCurBlock.parentNode, lastItemInCurBlock)) {
                listItemContainers.push(curItem);
                curListItemBlock.endElement = curItem;
            }
            else {
                curListItemBlock.endElement = lastItemInCurBlock;
                result.push(curListItemBlock);
                curListItemBlock = ListItemBlock_1.createListItemBlock(curItem);
            }
        }
    }
    if ((curListItemBlock === null || curListItemBlock === void 0 ? void 0 : curListItemBlock.listItemContainers.length) > 0) {
        result.push(curListItemBlock);
    }
    return result;
}
/**
 * Flatten the list items, so that all the consecutive list items are under the same parent.
 * @param fragment Root element of that contains the element.
 * @param listItemBlock The list item block needed to be flattened.
 */
function flattenListBlock(fragment, listItemBlock) {
    var collapsedListItemSections = roosterjs_editor_dom_1.collapseNodes(fragment, listItemBlock.startElement, listItemBlock.endElement, true);
    collapsedListItemSections.forEach(function (section) {
        if (roosterjs_editor_dom_1.getTagOfNode(section.firstChild) == 'DIV') {
            roosterjs_editor_dom_1.unwrap(section);
        }
    });
}
/**
 * Get the list type that the container contains. If there is no list in the container
 * return null;
 * @param listItemContainer Container that contains a list
 */
function getContainerListType(listItemContainer) {
    var tag = roosterjs_editor_dom_1.getTagOfNode(listItemContainer.firstChild);
    return tag == constants_1.UNORDERED_LIST_TAG_NAME || tag == constants_1.ORDERED_LIST_TAG_NAME ? tag : null;
}
/**
 * Insert list item into the correct position of a list
 * @param listRootElement Root element of the list that is accepting a coming element.
 * @param itemToInsert List item that needed to be inserted.
 * @param listType Type of list(ul/ol)
 */
function insertListItem(listRootElement, itemToInsert, listType, doc) {
    if (!listType) {
        return;
    }
    // Get item level from 'data-aria-level' attribute
    var itemLevel = parseInt(itemToInsert.getAttribute('data-aria-level'));
    var curListLevel = listRootElement; // Level iterator to find the correct place for the current element.
    // if the itemLevel is 1 it means the level iterator is at the correct place.
    while (itemLevel > 1) {
        if (!curListLevel.firstChild) {
            // If the current level is empty, create empty list within the current level
            // then move the level iterator into the next level.
            curListLevel.appendChild(doc.createElement(listType));
            curListLevel = curListLevel.firstElementChild;
        }
        else {
            // If the current level is not empty, the last item in the needs to be a UL or OL
            // and the level iterator should move to the UL/OL at the last position.
            var lastChild = curListLevel.lastElementChild;
            var lastChildTag = roosterjs_editor_dom_1.getTagOfNode(lastChild);
            if (lastChildTag == constants_1.UNORDERED_LIST_TAG_NAME || lastChildTag == constants_1.ORDERED_LIST_TAG_NAME) {
                // If the last child is a list(UL/OL), then move the level iterator to last child.
                curListLevel = lastChild;
            }
            else {
                // If the last child is not a list, then append a new list to the level
                // and move the level iterator to the new level.
                curListLevel.appendChild(doc.createElement(listType));
                curListLevel = curListLevel.lastElementChild;
            }
        }
        itemLevel--;
    }
    // Once the level iterator is at the right place, then append the list item in the level.
    curListLevel.appendChild(itemToInsert);
}
/**
 * Insert the converted list item into the correct place.
 * @param convertedListElement List element that is converted from list item block
 * @param fragment Root element of that contains the converted listItemBlock
 * @param listItemBlock List item block that was converted.
 */
function insertConvertedListToDoc(convertedListElement, fragment, listItemBlock) {
    if (!convertedListElement) {
        return;
    }
    var insertPositionNode = listItemBlock.insertPositionNode;
    if (insertPositionNode) {
        var parentNode = insertPositionNode.parentNode;
        if (parentNode) {
            parentNode.insertBefore(convertedListElement, insertPositionNode);
        }
    }
    else {
        var parentNode = listItemBlock.startElement.parentNode;
        if (parentNode) {
            parentNode.appendChild(convertedListElement);
        }
        else {
            fragment.appendChild(convertedListElement);
        }
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/pptConverter/convertPastedContentFromPowerPoint.ts":
/*!****************************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/pptConverter/convertPastedContentFromPowerPoint.ts ***!
  \****************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * Convert pasted content from PowerPoint
 * @param event The BeforePaste event
 */
function convertPastedContentFromExcel(event) {
    var _a;
    var fragment = event.fragment, clipboardData = event.clipboardData;
    if (clipboardData.html && !clipboardData.text && clipboardData.image) {
        // It is possible that PowerPoint copied both image and HTML but not plain text.
        // We always prefer HTML if any.
        var doc = new DOMParser().parseFromString(clipboardData.html, 'text/html');
        while (fragment.firstChild) {
            fragment.removeChild(fragment.firstChild);
        }
        while ((_a = doc === null || doc === void 0 ? void 0 : doc.body) === null || _a === void 0 ? void 0 : _a.firstChild) {
            fragment.appendChild(doc.body.firstChild);
        }
    }
}
exports.default = convertPastedContentFromExcel;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/teamsConverter/convertPastedContentFromTeams.ts":
/*!*************************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/teamsConverter/convertPastedContentFromTeams.ts ***!
  \*************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/**
 * @internal
 * Convert content copied from Teams to be well-formed
 */
function convertPastedContentFromTeams(fragment) {
    var firstChild = fragment.firstChild;
    // When copy from Teams, it is possible that we get LI nodes directly under DIV.
    // In that case we need to convert DIV to UL. It is also possible to be OL, but we don't know it.
    // So always assume it is UL here, and later user can change it.
    if (firstChild &&
        !firstChild.nextSibling &&
        roosterjs_editor_dom_1.getTagOfNode(firstChild) == 'DIV' &&
        !roosterjs_editor_dom_1.toArray(firstChild.childNodes).some(function (node) { return roosterjs_editor_dom_1.getTagOfNode(node) != 'LI'; })) {
        roosterjs_editor_dom_1.changeElementTag(firstChild, 'UL');
    }
}
exports.default = convertPastedContentFromTeams;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/LevelLists.ts":
/*!*****************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/LevelLists.ts ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
 * @internal
 * create an empty LevelLists
 */
function createLevelLists() {
    return {
        listsMetadata: {},
        currentUniqueListId: -1,
    };
}
exports.createLevelLists = createLevelLists;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordConverterArguments.ts":
/*!*****************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordConverterArguments.ts ***!
  \*****************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var LevelLists_1 = __webpack_require__(/*! ./LevelLists */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/LevelLists.ts");
/**
 * @internal
 * create an empty WordConverterArguments
 */
function createWordConverterArguments(nodes) {
    return {
        nodes: nodes,
        currentIndex: 0,
        lists: {},
        listItems: [],
        currentListIdsByLevels: [LevelLists_1.createLevelLists()],
        lastProcessedItem: null,
    };
}
exports.createWordConverterArguments = createWordConverterArguments;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordCustomData.ts":
/*!*********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordCustomData.ts ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/** NodeId attribute */
var NODE_ID_ATTRIBUTE_NAME = 'NodeId';
/**
 * @internal
 * Create an empty WordCustomData
 */
function createCustomData() {
    return {
        dict: {},
        nextNodeId: 1,
    };
}
exports.createCustomData = createCustomData;
/**
 * @internal
 * Sets the specified object data
 */
function setObject(wordCustomData, element, key, value) {
    // Get the id for the element
    if (element.nodeType == 1 /* Element */) {
        var id = getAndSetNodeId(wordCustomData, element);
        if (id != '') {
            // Get the values for the element
            if (!wordCustomData.dict[id]) {
                // First time dictionary creation
                wordCustomData.dict[id] = {};
            }
            wordCustomData.dict[id][key] = value;
        }
    }
}
exports.setObject = setObject;
/**
 * @internal
 * Reads the specified object data
 */
function getObject(wordCustomData, element, key) {
    if (element.nodeType == 1 /* Element */) {
        var id = getAndSetNodeId(wordCustomData, element);
        if (id != '') {
            return wordCustomData.dict[id] && wordCustomData.dict[id][key];
        }
    }
    return null;
}
exports.getObject = getObject;
/**
 * Get the unique id for the specified node...
 */
function getAndSetNodeId(wordCustomData, element) {
    var id = element.getAttribute(NODE_ID_ATTRIBUTE_NAME);
    if (!id) {
        id = wordCustomData.nextNodeId.toString();
        wordCustomData.nextNodeId++;
        element.setAttribute(NODE_ID_ATTRIBUTE_NAME, id);
    }
    return id;
}


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/convertPastedContentFromWord.ts":
/*!***********************************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/convertPastedContentFromWord.ts ***!
  \***********************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var wordConverter_1 = __webpack_require__(/*! ./wordConverter */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/wordConverter.ts");
var WordConverterArguments_1 = __webpack_require__(/*! ./WordConverterArguments */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordConverterArguments.ts");
var converterUtils_1 = __webpack_require__(/*! ./converterUtils */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/converterUtils.ts");
/**
 * @internal
 * Converts all the Word generated list items in the specified node into standard HTML UL and OL tags
 */
function convertPastedContentFromWord(event) {
    var sanitizingOption = event.sanitizingOption, fragment = event.fragment;
    // Preserve <o:p> when its innerHTML is "&nbsp;" to avoid dropping an empty line
    roosterjs_editor_dom_1.chainSanitizerCallback(sanitizingOption.elementCallbacks, 'O:P', function (element) {
        element.innerHTML = '&nbsp;';
        return true;
    });
    var wordConverter = wordConverter_1.createWordConverter();
    // First find all the nodes that we need to check for list item information
    // This call will return all the p and header elements under the root node.. These are the elements that
    // Word uses a list items, so we'll only process them and avoid walking the whole tree.
    var elements = fragment.querySelectorAll('p');
    if (elements.length > 0) {
        wordConverter.wordConverterArgs = WordConverterArguments_1.createWordConverterArguments(elements);
        if (converterUtils_1.processNodesDiscovery(wordConverter)) {
            converterUtils_1.processNodeConvert(wordConverter);
        }
    }
}
exports.default = convertPastedContentFromWord;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/converterUtils.ts":
/*!*********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/converterUtils.ts ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var LevelLists_1 = __webpack_require__(/*! ./LevelLists */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/LevelLists.ts");
var WordCustomData_1 = __webpack_require__(/*! ./WordCustomData */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordCustomData.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
/** Word list metadata style name */
var LOOKUP_DEPTH = 5;
/** Name for the word list id property in the custom data */
var UNIQUE_LIST_ID_CUSTOM_DATA = 'UniqueListId';
/** Word list metadata style name */
var MSO_LIST_STYLE_NAME = 'mso-list';
/** Regular expression to match line breaks */
var LINE_BREAKS = /[\n|\r]/gi;
/**
 * @internal
 * Handles the pass 1: Discovery
 * During discovery, we'll parse the metadata out of the elements and store it in the list items dictionary.
 * We'll detect cases where the list items for a particular ordered list are not next to each other. Word does these
 * for numbered headers, and we don't want to convert those, because the numbering would be completely wrong.
 */
function processNodesDiscovery(wordConverter) {
    var args = wordConverter.wordConverterArgs;
    while (args.currentIndex < args.nodes.length) {
        var node = args.nodes.item(args.currentIndex);
        // Try to get the list metadata for the specified node
        var itemMetadata = getListItemMetadata(node);
        if (itemMetadata) {
            var levelInfo = args.currentListIdsByLevels[itemMetadata.level - 1] || LevelLists_1.createLevelLists();
            args.currentListIdsByLevels[itemMetadata.level - 1] = levelInfo;
            // We need to drop some list information if this is not an item next to another
            if (args.lastProcessedItem && getRealPreviousSibling(node) != args.lastProcessedItem) {
                // This list item is not next to the previous one. This means that there is some content in between them
                // so we need to reset our list of list ids per level
                resetCurrentLists(args);
            }
            // Get the list metadata for the list that will hold this item
            var listMetadata = levelInfo.listsMetadata[itemMetadata.wordListId];
            if (!listMetadata) {
                // Get the first item fake bullet.. This will be used later to check what is the right type of list
                var firstFakeBullet = getFakeBulletText(node, LOOKUP_DEPTH);
                // This is a the first item of a list.. We'll create the list metadata using the information
                // we already have from this first item
                listMetadata = {
                    numberOfItems: 0,
                    uniqueListId: wordConverter.nextUniqueId++,
                    firstFakeBullet: firstFakeBullet,
                    // If the bullet we got is emtpy or not found, we ignore the list out.. this means
                    // that this is not an item we need to convert of that the format doesn't match what
                    // we are expecting
                    ignore: !firstFakeBullet || firstFakeBullet.length == 0,
                    // We'll use the first fake bullet to try to figure out which type of list we create. If this list has a second
                    // item, we'll perform a better comparasion, but for one item lists, this will be check that will determine the list type
                    tagName: getFakeBulletTagName(firstFakeBullet),
                };
                levelInfo.listsMetadata[itemMetadata.wordListId] = listMetadata;
                args.lists[listMetadata.uniqueListId.toString()] = listMetadata;
            }
            else if (!listMetadata.ignore && listMetadata.numberOfItems == 1) {
                // This is the second item we've seen for this list.. we'll compare the 2 fake bullet
                // items we have an decide if we create ordered or unordered lists based on this.
                // This is the best way we can do this since we cannot read the metadata that Word
                // puts in the head of the HTML...
                var secondFakeBullet = getFakeBulletText(node, LOOKUP_DEPTH);
                listMetadata.tagName =
                    listMetadata.firstFakeBullet == secondFakeBullet ? 'UL' : 'OL';
            }
            // Set the unique id to the list
            itemMetadata.uniqueListId = listMetadata.uniqueListId;
            // Check if we need to ignore this list... we'll either know already that we need to ignore
            // it, or we'll know it because the previous list items are not next to this one
            if (listMetadata.ignore ||
                (listMetadata.tagName == 'OL' &&
                    listMetadata.numberOfItems > 0 &&
                    levelInfo.currentUniqueListId != itemMetadata.uniqueListId)) {
                // We need to ignore this item... and we also need to forget about the lists that
                // are not at the root level
                listMetadata.ignore = true;
                args.currentListIdsByLevels[0].currentUniqueListId = -1;
                args.currentListIdsByLevels = args.currentListIdsByLevels.slice(0, 1);
            }
            else {
                // This is an item we don't need to ignore... If added lists deep under this one before
                // we'll drop their ids from the list of ids per level.. this is because this list item
                // breaks the deeper lists.
                if (args.currentListIdsByLevels.length > itemMetadata.level) {
                    args.currentListIdsByLevels = args.currentListIdsByLevels.slice(0, itemMetadata.level);
                }
                levelInfo.currentUniqueListId = itemMetadata.uniqueListId;
                // Add the list item into the list of items to be processed
                args.listItems.push(itemMetadata);
                listMetadata.numberOfItems++;
            }
            args.lastProcessedItem = node;
        }
        else {
            // Here, we know that this is not a list item, but we'll want to check if it is one "no bullet" list items...
            // these can be created by creating a bullet and hitting delete on it it... The content will continue to be indented, but there will
            // be no bullet and the list will continue correctly after that. Visually, it looks like the previous item has multiple lines, but
            // the HTML generated has multiple paragraphs with the same class. We'll merge these when we find them, so the logic doesn't skips
            // the list conversion thinking that the list items are not together...
            var last = args.lastProcessedItem;
            if (last &&
                getRealPreviousSibling(node) == last &&
                node.tagName == last.tagName &&
                node.className == last.className) {
                // Add 2 line breaks and move all the nodes to the last item
                last.appendChild(last.ownerDocument.createElement('br'));
                last.appendChild(last.ownerDocument.createElement('br'));
                while (node.firstChild != null) {
                    last.appendChild(node.firstChild);
                }
                // Remove the item that we don't need anymore
                node.parentNode.removeChild(node);
            }
        }
        // Move to the next element are return true if more elements need to be processed
        args.currentIndex++;
    }
    return args.listItems.length > 0;
}
exports.processNodesDiscovery = processNodesDiscovery;
/**
 * @internal
 * Handles the pass 2: Conversion
 * During conversion, we'll go over the elements that belong to a list that we've marked as a list to convert, and we'll perform the
 * conversion needed
 */
function processNodeConvert(wordConverter) {
    var args = wordConverter.wordConverterArgs;
    args.currentIndex = 0;
    while (args.currentIndex < args.listItems.length) {
        var metadata = args.listItems[args.currentIndex];
        var node = metadata.originalNode;
        var listMetadata = args.lists[metadata.uniqueListId.toString()];
        if (!listMetadata.ignore) {
            // We have a list item that we need to convert, get or create the list
            // that hold this item out
            var list = getOrCreateListForNode(wordConverter, node, metadata, listMetadata);
            if (list) {
                // Clean the element out.. this call gets rid of the fake bullet and unneeded nodes
                cleanupListIgnore(node, LOOKUP_DEPTH);
                // Create a new list item and transfer the children
                var li = node.ownerDocument.createElement('LI');
                while (node.firstChild) {
                    li.appendChild(node.firstChild);
                }
                // Append the list item into the list
                list.appendChild(li);
                // Remove the node we just converted
                node.parentNode.removeChild(node);
                if (listMetadata.tagName == 'UL') {
                    wordConverter.numBulletsConverted++;
                }
                else {
                    wordConverter.numNumberedConverted++;
                }
            }
        }
        args.currentIndex++;
    }
    return wordConverter.numBulletsConverted > 0 || wordConverter.numNumberedConverted > 0;
}
exports.processNodeConvert = processNodeConvert;
/**
 * Gets or creates the list (UL or OL) that holds this item out based on the
 * items content and the specified metadata
 */
function getOrCreateListForNode(wordConverter, node, metadata, listMetadata) {
    // First get the last list next to this node under the specified level. This code
    // path will return the list or will create lists if needed
    var list = recurringGetOrCreateListAtNode(node, metadata.level, listMetadata);
    // Here use the unique list ID to detect if we have the right list...
    // it is possible to have 2 different lists next to each other with different formats, so
    // we want to detect this an create separate lists for those cases
    var listId = WordCustomData_1.getObject(wordConverter.wordCustomData, list, UNIQUE_LIST_ID_CUSTOM_DATA);
    // If we have a list with and ID, but the ID is different than the ID for this list item, this
    // is a completely new list, so we'll append a new list for that
    if ((listId && listId != metadata.uniqueListId) || (!listId && list.firstChild)) {
        var newList = node.ownerDocument.createElement(listMetadata.tagName);
        list.parentNode.insertBefore(newList, list.nextSibling);
        list = newList;
    }
    // Set the list id into the custom data
    WordCustomData_1.setObject(wordConverter.wordCustomData, list, UNIQUE_LIST_ID_CUSTOM_DATA, metadata.uniqueListId);
    // This call will convert the list if needed to the right type of list required. This can happen
    // on the cases where the first list item for this list is located after a deeper list. for that
    // case, we will have created a UL for it, and we may need to convert it
    return convertListIfNeeded(wordConverter, list, listMetadata);
}
/**
 * Converts the list between UL and OL if needed, by using the fake bullet and
 * information already stored in the list itself
 */
function convertListIfNeeded(wordConverter, list, listMetadata) {
    // Check if we need to convert the list out
    if (listMetadata.tagName != roosterjs_editor_dom_1.getTagOfNode(list)) {
        // We have the wrong list type.. convert it, set the id again and tranfer all the childs
        var newList = list.ownerDocument.createElement(listMetadata.tagName);
        WordCustomData_1.setObject(wordConverter.wordCustomData, newList, UNIQUE_LIST_ID_CUSTOM_DATA, WordCustomData_1.getObject(wordConverter.wordCustomData, list, UNIQUE_LIST_ID_CUSTOM_DATA));
        while (list.firstChild) {
            newList.appendChild(list.firstChild);
        }
        list.parentNode.insertBefore(newList, list);
        list.parentNode.removeChild(list);
        list = newList;
    }
    return list;
}
/**
 * Gets or creates the specified list
 */
function recurringGetOrCreateListAtNode(node, level, listMetadata) {
    var parent = null;
    var possibleList;
    if (level == 1) {
        // Root case, we'll check if the list is the previous sibling of the node
        possibleList = getRealPreviousSibling(node);
    }
    else {
        // If we get here, we are looking for level 2 or deeper... get the upper list
        // and check if the last element is a list
        parent = recurringGetOrCreateListAtNode(node, level - 1, null);
        possibleList = parent.lastChild;
    }
    // Check the element that we got and verify that it is a list
    if (possibleList && possibleList.nodeType == 1 /* Element */) {
        var tag = roosterjs_editor_dom_1.getTagOfNode(possibleList);
        if (tag == 'UL' || tag == 'OL') {
            // We have a list.. use it
            return possibleList;
        }
    }
    // If we get here, it means we don't have a list and we need to create one
    // this code path will always create new lists as UL lists
    var newList = node.ownerDocument.createElement(listMetadata ? listMetadata.tagName : 'UL');
    if (level == 1) {
        // For level 1, we'll insert the list beofre the node
        node.parentNode.insertBefore(newList, node);
    }
    else {
        // Any level 2 or above, we insert the list as the last
        // child of the upper level list
        parent.appendChild(newList);
    }
    return newList;
}
/**
 * Cleans up the node children by removing the childs marked as mso-list: Ignore.
 * This nodes hold the fake bullet information that Word puts in and when
 * conversion is happening, we want to get rid of these elements
 */
function cleanupListIgnore(node, levels) {
    var nodesToRemove = [];
    for (var child = node.firstChild; child; child = child.nextSibling) {
        // Clean up the item internally first if we need to based on the number of levels
        if (child.nodeType == 1 /* Element */ && levels > 1) {
            cleanupListIgnore(child, levels - 1);
        }
        // Try to convert word comments into ignore elements if we haven't done so for this element
        child = fixWordListComments(child, true /*removeComments*/);
        // Check if we can remove this item out
        if (isEmptySpan(child) || isIgnoreNode(child)) {
            nodesToRemove.push(child);
        }
    }
    nodesToRemove.forEach(function (child) { return node.removeChild(child); });
}
/**
 * Reads the word list metadada out of the specified node. If the node
 * is not a Word list item, it returns null.
 */
function getListItemMetadata(node) {
    if (node.nodeType == 1 /* Element */) {
        var listatt = getStyleValue(node, MSO_LIST_STYLE_NAME);
        if (listatt && listatt.length > 0) {
            try {
                // Word mso-list property holds 3 space separated values in the following format: lst1 level1 lfo0
                // Where:
                // (0) List identified for the metadata in the &lt;head&gt; of the document. We cannot read the &lt;head&gt; metada
                // (1) Level of the list. This also maps to the &lt;head&gt; metadata that we cannot read, but
                // for almost all cases, it maps to the list identation (or level). We'll use it as the
                // list indentation value
                // (2) Contains a specific list identifier.
                // Example value: "l0 level1 lfo1"
                var listprops = listatt.split(' ');
                if (listprops.length == 3) {
                    return {
                        level: parseInt(listprops[1].substr('level'.length)),
                        wordListId: listatt,
                        originalNode: node,
                        uniqueListId: 0,
                    };
                }
            }
            catch (e) { }
        }
    }
    return null;
}
function isFakeBullet(fakeBullet) {
    return ['o', '·', '§', '-'].indexOf(fakeBullet) >= 0;
}
/** Given a fake bullet text, returns the type of list that should be used for it */
function getFakeBulletTagName(fakeBullet) {
    return isFakeBullet(fakeBullet) ? 'UL' : 'OL';
}
/**
 * Finds the fake bullet text out of the specified node and returns it. For images, it will return
 * a bullet string. If not found, it returns null...
 */
function getFakeBulletText(node, levels) {
    // Word uses the following format for their bullets:
    // &lt;p style="mso-list:l1 level1 lfo2"&gt;
    // &lt;span style="..."&gt;
    // &lt;span style="mso-list:Ignore"&gt;1.&lt;span style="..."&gt;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;/span&gt;&lt;/span&gt;
    // &lt;/span&gt;
    // Content here...
    // &lt;/p&gt;
    //
    // Basically, we need to locate the mso-list:Ignore SPAN, which holds either one text or image node. That
    // text or image node will be the fake bullet we are looking for
    var result = null;
    var child = node.firstChild;
    while (!result && child) {
        // First, check if we need to convert the Word list comments into real elements
        child = fixWordListComments(child, true /*removeComments*/);
        // Check if this is the node that holds the fake bullets (mso-list: Ignore)
        if (isIgnoreNode(child)) {
            // Yes... this is the node that holds either the text or image data
            result = child.textContent.trim();
            // This is the case for image case
            if (result.length == 0) {
                result = 'o';
            }
        }
        else if (child.nodeType == 1 /* Element */ && levels > 1) {
            // If this is an element and we are not in the last level, try to get the fake bullet
            // out of the child
            result = getFakeBulletText(child, levels - 1);
        }
        child = child.nextSibling;
    }
    return result;
}
/**
 * If the specified element is a Word List comments, this code verifies and fixes
 * the markup when needed to ensure that Chrome bullet conversions work as expected
 * -----
 * We'll convert &lt;!--[if !supportLists]--&gt; and &lt;!--[endif]--&gt; comments into
 * &lt;span style="mso-list:Ignore"&gt;&lt;/span&gt;... Chrome has a bug where it drops the
 * styles of the span, but we'll use these comments to recreate them out
 */
function fixWordListComments(child, removeComments) {
    if (child.nodeType == 8 /* Comment */) {
        var value = child.data;
        if (value && value.trim().toLowerCase() == '[if !supportlists]') {
            // We have a list ignore start, find the end.. We know is not more than
            // 3 nodes away, so we'll optimize our checks
            var nextElement = child;
            var endComment = null;
            for (var j = 0; j < 4; j++) {
                nextElement = getRealNextSibling(nextElement);
                if (!nextElement) {
                    break;
                }
                if (nextElement.nodeType == 8 /* Comment */) {
                    value = nextElement.data;
                    if (value && value.trim().toLowerCase() == '[endif]') {
                        endComment = nextElement;
                        break;
                    }
                }
            }
            // if we found the end node, wrap everything out
            if (endComment) {
                var newSpan = child.ownerDocument.createElement('span');
                newSpan.setAttribute('style', 'mso-list: ignore');
                nextElement = getRealNextSibling(child);
                while (nextElement != endComment) {
                    nextElement = nextElement.nextSibling;
                    newSpan.appendChild(nextElement.previousSibling);
                }
                // Insert the element out and use that one as the current child
                endComment.parentNode.insertBefore(newSpan, endComment);
                // Remove the comments out if the call specified it out
                if (removeComments) {
                    child.parentNode.removeChild(child);
                    endComment.parentNode.removeChild(endComment);
                }
                // Last, make sure we return the new element out instead of the comment
                child = newSpan;
            }
        }
    }
    return child;
}
/** Finds the real previous sibling, ignoring emtpy text nodes */
function getRealPreviousSibling(node) {
    var prevSibling = node;
    do {
        prevSibling = prevSibling.previousSibling;
    } while (prevSibling && isEmptyTextNode(prevSibling));
    return prevSibling;
}
/** Finds the real next sibling, ignoring empty text nodes */
function getRealNextSibling(node) {
    var nextSibling = node;
    do {
        nextSibling = nextSibling.nextSibling;
    } while (nextSibling && isEmptyTextNode(nextSibling));
    return nextSibling;
}
/**
 * Checks if the specified node is marked as a mso-list: Ignore. These
 * nodes need to be ignored when a list item is converted into standard
 * HTML lists
 */
function isIgnoreNode(node) {
    if (node.nodeType == 1 /* Element */) {
        var listatt = getStyleValue(node, MSO_LIST_STYLE_NAME);
        if (listatt && listatt.length > 0 && listatt.trim().toLowerCase() == 'ignore') {
            return true;
        }
    }
    return false;
}
/** Checks if the specified node is an empty span. */
function isEmptySpan(node) {
    return roosterjs_editor_dom_1.getTagOfNode(node) == 'SPAN' && !node.firstChild;
}
/** Reads the specified style value from the node */
function getStyleValue(node, styleName) {
    // Word uses non-standard names for the metadata that puts in the style of the element...
    // Most browsers will not provide the information for those unstandard values throug the node.style
    // property, so the only reliable way to read them is to get the attribute directly and do
    // the required parsing..
    return roosterjs_editor_dom_1.getStyles(node)[styleName] || null;
}
/** Checks if the node is an empty text node that can be ignored */
function isEmptyTextNode(node) {
    // No node is empty
    if (!node) {
        return true;
    }
    // Empty text node is empty
    if (node.nodeType == 3 /* Text */) {
        var value = node.nodeValue;
        value = value.replace(LINE_BREAKS, '');
        return value.trim().length == 0;
    }
    // Span or Font with an empty child node is empty
    var tagName = roosterjs_editor_dom_1.getTagOfNode(node);
    if (node.firstChild == node.lastChild && (tagName == 'SPAN' || tagName == 'FONT')) {
        return isEmptyTextNode(node.firstChild);
    }
    // If not found, then this is not empty
    return false;
}
/** Resets the list */
function resetCurrentLists(args) {
    for (var i = 0; i < args.currentListIdsByLevels.length; i++) {
        var ll = args.currentListIdsByLevels[i];
        if (ll) {
            ll.currentUniqueListId = -1;
        }
    }
}


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/wordConverter.ts":
/*!********************************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/wordConverter.ts ***!
  \********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var WordCustomData_1 = __webpack_require__(/*! ./WordCustomData */ "./packages/roosterjs-editor-plugins/lib/plugins/Paste/wordConverter/WordCustomData.ts");
/**
 * @internal
 * create an empty WordConverter
 */
function createWordConverter() {
    return {
        nextUniqueId: 1,
        numBulletsConverted: 0,
        numNumberedConverted: 0,
        wordConverterArgs: null,
        wordCustomData: WordCustomData_1.createCustomData(),
    };
}
exports.createWordConverter = createWordConverter;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Picker/PickerPlugin.ts":
/*!******************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Picker/PickerPlugin.ts ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
// Character codes.
// IE11 uses different character codes. which are noted below.
// If adding a new key, test in IE to figure out what the code is.
var BACKSPACE_CHARCODE = 'Backspace';
var TAB_CHARCODE = 'Tab';
var ENTER_CHARCODE = 'Enter';
var ESC_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'Escape' : 'Esc';
var LEFT_ARROW_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'ArrowLeft' : 'Left';
var UP_ARROW_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'ArrowUp' : 'Up';
var RIGHT_ARROW_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'ArrowRight' : 'Right';
var DOWN_ARROW_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'ArrowDown' : 'Down';
var DELETE_CHARCODE = !roosterjs_editor_dom_1.Browser.isIE ? 'Delete' : 'Del';
// Input event input types.
var DELETE_CONTENT_BACKWARDS_INPUT_TYPE = 'deleteContentBackwards';
// Unidentified key, the code for Android keyboard events.
var UNIDENTIFIED_KEY = 'Unidentified';
// the char code for Android keyboard events on Webview below 51.
var UNIDENTIFIED_CODE = [0, 229];
/**
 * PickerPlugin represents a plugin of editor which can handle picker related behaviors, including
 * - Show picker when special trigger key is pressed
 * - Hide picker
 * - Change selection in picker by Up/Down/Left/Right
 * - Apply selected item in picker
 *
 * PickerPlugin doesn't provide any UI, it just wraps related DOM events and invoke callback functions.
 * To show a picker UI, you need to build your own UI component. Please reference to
 * https://github.com/microsoft/roosterjs/tree/master/demo/scripts/controls/samplepicker
 */
var PickerPlugin = /** @class */ (function () {
    function PickerPlugin(dataProvider, pickerOptions) {
        this.dataProvider = dataProvider;
        this.pickerOptions = pickerOptions;
        // For detecting backspace in Android
        this.isPendingInputEventHandling = false;
    }
    /**
     * Get a friendly name
     */
    PickerPlugin.prototype.getName = function () {
        return 'Picker';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    PickerPlugin.prototype.initialize = function (editor) {
        var _this = this;
        this.editor = editor;
        this.dataProvider.onInitalize(function (htmlNode) {
            _this.editor.focus();
            var wordToReplace = _this.getWord(null);
            // Safari drops our focus out so we get an empty word to replace when we call getWord.
            // We fall back to using the lastKnownRange to try to get around this.
            if ((!wordToReplace || wordToReplace.length == 0) && _this.lastKnownRange) {
                _this.editor.select(_this.lastKnownRange);
                wordToReplace = _this.getWord(null);
            }
            var insertNode = function () {
                if (wordToReplace) {
                    roosterjs_editor_api_1.replaceWithNode(_this.editor, wordToReplace, htmlNode, true /* exactMatch */);
                }
                else {
                    _this.editor.insertNode(htmlNode);
                }
                _this.setIsSuggesting(false);
            };
            _this.editor.addUndoSnapshot(insertNode, _this.pickerOptions.changeSource, _this.pickerOptions.handleAutoComplete);
        }, function (isSuggesting) {
            _this.setIsSuggesting(isSuggesting);
        }, editor);
    };
    /**
     * Dispose this plugin
     */
    PickerPlugin.prototype.dispose = function () {
        this.editor = null;
        this.dataProvider.onDispose();
    };
    /**
     * Check if the plugin should handle the given event exclusively.
     * Handle an event exclusively means other plugin will not receive this event in
     * onPluginEvent method.
     * If two plugins will return true in willHandleEventExclusively() for the same event,
     * the final result depends on the order of the plugins are added into editor
     * @param event The event to check
     */
    PickerPlugin.prototype.willHandleEventExclusively = function (event) {
        return (this.isSuggesting &&
            (event.eventType == 0 /* KeyDown */ ||
                event.eventType == 2 /* KeyUp */ ||
                event.eventType == 3 /* Input */));
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    PickerPlugin.prototype.onPluginEvent = function (event) {
        switch (event.eventType) {
            case 7 /* ContentChanged */:
                if (event.source == "SetContent" /* SetContent */ && this.dataProvider.onContentChanged) {
                    // Stop suggesting since content is fully changed
                    if (this.isSuggesting) {
                        this.setIsSuggesting(false);
                    }
                    // Undos and other major changes to document content fire this type of event.
                    // Inform the data provider of the current picker placed elements in the body.
                    var elementIds_1 = [];
                    this.editor.queryElements("[id^='" + this.pickerOptions.elementIdPrefix + "']", function (element) {
                        if (element.id) {
                            elementIds_1.push(element.id);
                        }
                    });
                    this.dataProvider.onContentChanged(elementIds_1);
                }
                break;
            case 0 /* KeyDown */:
                this.eventHandledOnKeyDown = false;
                if (this.isAndroidKeyboardEvent(event)) {
                    // On Android, the key for KeyboardEvent is "Unidentified" or undefined,
                    // so handling should be done using the input rather than key down event
                    // Since the key down event happens right before the input event, calculate the input
                    // length here in preparation for onAndroidInputEvent
                    this.currentInputLength = this.calcInputLength(event);
                    this.isPendingInputEventHandling = true;
                }
                else {
                    this.onKeyDownEvent(event);
                    this.isPendingInputEventHandling = false;
                }
                break;
            case 3 /* Input */:
                if (this.isPendingInputEventHandling) {
                    this.onAndroidInputEvent(event);
                }
                break;
            case 2 /* KeyUp */:
                if (!this.eventHandledOnKeyDown && this.shouldHandleKeyUpEvent(event)) {
                    this.onKeyUpDomEvent(event);
                    this.isPendingInputEventHandling = false;
                }
                break;
            case 6 /* MouseUp */:
                if (this.isSuggesting) {
                    this.setIsSuggesting(false);
                }
                break;
            case 14 /* Scroll */:
                if (this.dataProvider.onScroll) {
                    // Dispatch scroll event to data provider
                    this.dataProvider.onScroll(event.scrollContainer);
                }
                break;
        }
    };
    PickerPlugin.prototype.setLastKnownRange = function (range) {
        this.lastKnownRange = range;
    };
    PickerPlugin.prototype.setIsSuggesting = function (isSuggesting) {
        this.isSuggesting = isSuggesting;
        if (!isSuggesting) {
            this.setLastKnownRange(null);
        }
        this.dataProvider.onIsSuggestingChanged(isSuggesting);
        this.setAriaOwns(isSuggesting);
        this.setAriaActiveDescendant(isSuggesting ? 0 : null);
    };
    PickerPlugin.prototype.cancelDefaultKeyDownEvent = function (event) {
        this.eventHandledOnKeyDown = true;
        event.rawEvent.preventDefault();
        event.rawEvent.stopImmediatePropagation();
    };
    PickerPlugin.prototype.getIdValue = function (node) {
        var element = node;
        return element.attributes && element.attributes.getNamedItem('id')
            ? element.attributes.getNamedItem('id').value
            : null;
    };
    PickerPlugin.prototype.getWordBeforeCursor = function (event) {
        var searcher = this.editor.getContentSearcherOfCursor(event);
        return searcher ? searcher.getWordBefore() : null;
    };
    PickerPlugin.prototype.replaceNode = function (currentNode, replacementNode) {
        if (currentNode) {
            this.editor.deleteNode(currentNode);
        }
        if (replacementNode) {
            this.editor.insertNode(replacementNode);
        }
    };
    PickerPlugin.prototype.getRangeUntilAt = function (event) {
        var _this = this;
        var positionContentSearcher = this.editor.getContentSearcherOfCursor(event);
        var startPos;
        var endPos;
        positionContentSearcher.forEachTextInlineElement(function (textInline) {
            var hasMatched = false;
            var nodeContent = textInline.getTextContent();
            var nodeIndex = nodeContent ? nodeContent.length : -1;
            while (nodeIndex >= 0) {
                if (nodeContent[nodeIndex] == _this.pickerOptions.triggerCharacter) {
                    startPos = textInline.getStartPosition().move(nodeIndex);
                    hasMatched = true;
                    break;
                }
                nodeIndex--;
            }
            if (hasMatched) {
                endPos = textInline.getEndPosition();
            }
            return hasMatched;
        });
        return roosterjs_editor_dom_1.createRange(startPos, endPos) || this.editor.getDocument().createRange();
    };
    PickerPlugin.prototype.shouldHandleKeyUpEvent = function (event) {
        // onKeyUpDomEvent should only be called when a key that produces a character value is pressed
        // This check will always fail on Android since the KeyboardEvent's key is "Unidentified" or undefined
        // However, we don't need to check for modifier events on mobile, so can ignore this check
        return (this.isAndroidKeyboardEvent(event) ||
            roosterjs_editor_dom_1.isCharacterValue(event.rawEvent) ||
            (this.isSuggesting && !roosterjs_editor_dom_1.isModifierKey(event.rawEvent)));
    };
    PickerPlugin.prototype.onKeyUpDomEvent = function (event) {
        if (this.isSuggesting) {
            // Word before cursor represents the text prior to the cursor, up to and including the trigger symbol.
            var wordBeforeCursor = this.getWord(event);
            var wordBeforeCursorWithoutTriggerChar = wordBeforeCursor.substring(1);
            var trimmedWordBeforeCursor = wordBeforeCursorWithoutTriggerChar.trim();
            // If we hit a case where wordBeforeCursor is just the trigger character,
            // that means we've gotten a onKeyUp event right after it's been typed.
            // Otherwise, update the query string when:
            // 1. There's an actual value
            // 2. That actual value isn't just pure whitespace
            // 3. That actual value isn't more than 4 words long (at which point we assume the person kept typing)
            // Otherwise, we want to dismiss the picker plugin's UX.
            if (wordBeforeCursor == this.pickerOptions.triggerCharacter ||
                (trimmedWordBeforeCursor &&
                    trimmedWordBeforeCursor.length > 0 &&
                    trimmedWordBeforeCursor.split(' ').length <= 4)) {
                this.dataProvider.queryStringUpdated(trimmedWordBeforeCursor, wordBeforeCursorWithoutTriggerChar == trimmedWordBeforeCursor);
                this.setLastKnownRange(this.editor.getSelectionRange());
            }
            else {
                this.setIsSuggesting(false);
            }
        }
        else {
            var wordBeforeCursor = this.getWordBeforeCursor(event);
            if (!this.blockSuggestions) {
                if (wordBeforeCursor != null &&
                    wordBeforeCursor.split(' ').length <= 4 &&
                    wordBeforeCursor[0] == this.pickerOptions.triggerCharacter) {
                    this.setIsSuggesting(true);
                    var wordBeforeCursorWithoutTriggerChar = wordBeforeCursor.substring(1);
                    var trimmedWordBeforeCursor = wordBeforeCursorWithoutTriggerChar.trim();
                    this.dataProvider.queryStringUpdated(trimmedWordBeforeCursor, wordBeforeCursorWithoutTriggerChar == trimmedWordBeforeCursor);
                    this.setLastKnownRange(this.editor.getSelectionRange());
                    if (this.dataProvider.setCursorPoint) {
                        // Determine the bounding rectangle for the @mention
                        var searcher = this.editor.getContentSearcherOfCursor(event);
                        var rangeNode = this.editor.getDocument().createRange();
                        var nodeBeforeCursor = searcher.getInlineElementBefore().getContainerNode();
                        var rangeStartSuccessfullySet = this.setRangeStart(rangeNode, nodeBeforeCursor, wordBeforeCursor);
                        if (!rangeStartSuccessfullySet) {
                            // VSO 24891: Out of range error is occurring because nodeBeforeCursor
                            // is not including the trigger character. In this case, the node before
                            // the node before cursor is the trigger character, and this is where the range should start.
                            var nodeBeforeNodeBeforeCursor = nodeBeforeCursor.previousSibling;
                            this.setRangeStart(rangeNode, nodeBeforeNodeBeforeCursor, this.pickerOptions.triggerCharacter);
                        }
                        var rect = rangeNode.getBoundingClientRect();
                        // Safari's support for range.getBoundingClientRect is incomplete.
                        // We perform this check to fall back to getClientRects in case it's at the page origin.
                        if (rect.left == 0 && rect.bottom == 0 && rect.top == 0) {
                            rect = rangeNode.getClientRects()[0];
                        }
                        if (rect) {
                            rangeNode.detach();
                            // Display the @mention popup in the correct place
                            var targetPoint = { x: rect.left, y: (rect.bottom + rect.top) / 2 };
                            var bufferZone = (rect.bottom - rect.top) / 2;
                            this.dataProvider.setCursorPoint(targetPoint, bufferZone);
                        }
                    }
                }
            }
            else {
                if (wordBeforeCursor != null &&
                    wordBeforeCursor[0] != this.pickerOptions.triggerCharacter) {
                    this.blockSuggestions = false;
                }
            }
        }
    };
    PickerPlugin.prototype.onKeyDownEvent = function (event) {
        var keyboardEvent = event.rawEvent;
        if (this.isSuggesting) {
            if (keyboardEvent.key == ESC_CHARCODE) {
                this.setIsSuggesting(false);
                this.blockSuggestions = true;
                this.cancelDefaultKeyDownEvent(event);
            }
            else if (keyboardEvent.key == BACKSPACE_CHARCODE) {
                // #483: If we are backspacing over the trigger character that triggered this Picker
                // then we need to hide the Picker
                var wordBeforeCursor = this.getWord(event);
                if (wordBeforeCursor == this.pickerOptions.triggerCharacter) {
                    this.setIsSuggesting(false);
                }
            }
            else if (this.dataProvider.shiftHighlight &&
                (this.pickerOptions.isHorizontal
                    ? keyboardEvent.key == LEFT_ARROW_CHARCODE ||
                        keyboardEvent.key == RIGHT_ARROW_CHARCODE
                    : keyboardEvent.key == UP_ARROW_CHARCODE ||
                        keyboardEvent.key == DOWN_ARROW_CHARCODE)) {
                this.dataProvider.shiftHighlight(this.pickerOptions.isHorizontal
                    ? keyboardEvent.key == RIGHT_ARROW_CHARCODE
                    : keyboardEvent.key == DOWN_ARROW_CHARCODE);
                if (this.dataProvider.getSelectedIndex) {
                    this.setAriaActiveDescendant(this.dataProvider.getSelectedIndex());
                }
                this.cancelDefaultKeyDownEvent(event);
            }
            else if (this.dataProvider.selectOption &&
                (keyboardEvent.key == ENTER_CHARCODE || keyboardEvent.key == TAB_CHARCODE)) {
                this.dataProvider.selectOption();
                this.cancelDefaultKeyDownEvent(event);
            }
            else {
                // Currently no op.
            }
        }
        else {
            if (keyboardEvent.key == BACKSPACE_CHARCODE) {
                var nodeRemoved = this.tryRemoveNode(event);
                if (nodeRemoved) {
                    this.cancelDefaultKeyDownEvent(event);
                }
            }
            else if (keyboardEvent.key == DELETE_CHARCODE) {
                var searcher = this.editor.getContentSearcherOfCursor(event);
                var nodeAfterCursor = searcher.getInlineElementAfter()
                    ? searcher.getInlineElementAfter().getContainerNode()
                    : null;
                var nodeId = nodeAfterCursor ? this.getIdValue(nodeAfterCursor) : null;
                if (nodeId && nodeId.indexOf(this.pickerOptions.elementIdPrefix) == 0) {
                    var replacementNode = this.dataProvider.onRemove(nodeAfterCursor, false);
                    this.replaceNode(nodeAfterCursor, replacementNode);
                    this.cancelDefaultKeyDownEvent(event);
                }
            }
        }
    };
    PickerPlugin.prototype.onAndroidInputEvent = function (event) {
        this.newInputLength = this.calcInputLength(event);
        if (this.newInputLength < this.currentInputLength ||
            event.rawEvent.inputType === DELETE_CONTENT_BACKWARDS_INPUT_TYPE) {
            var nodeRemoved = this.tryRemoveNode(event);
            if (nodeRemoved) {
                this.eventHandledOnKeyDown = true;
            }
        }
    };
    PickerPlugin.prototype.calcInputLength = function (event) {
        var wordBeforCursor = this.getInlineElementBeforeCursor(event);
        return wordBeforCursor ? wordBeforCursor.length : 0;
    };
    PickerPlugin.prototype.tryRemoveNode = function (event) {
        var searcher = this.editor.getContentSearcherOfCursor(event);
        var inlineElementBefore = searcher.getInlineElementBefore();
        var nodeBeforeCursor = inlineElementBefore
            ? inlineElementBefore.getContainerNode()
            : null;
        var nodeId = nodeBeforeCursor ? this.getIdValue(nodeBeforeCursor) : null;
        var inlineElementAfter = searcher.getInlineElementAfter();
        if (nodeId &&
            nodeId.indexOf(this.pickerOptions.elementIdPrefix) == 0 &&
            (inlineElementAfter == null || !(inlineElementAfter instanceof roosterjs_editor_dom_1.PartialInlineElement))) {
            var replacementNode_1 = this.dataProvider.onRemove(nodeBeforeCursor, true);
            if (replacementNode_1) {
                this.replaceNode(nodeBeforeCursor, replacementNode_1);
                if (this.isPendingInputEventHandling) {
                    this.editor.runAsync(function (editor) {
                        editor.select(replacementNode_1, -3 /* After */);
                    });
                }
                else {
                    this.editor.select(replacementNode_1, -3 /* After */);
                }
            }
            else {
                this.editor.deleteNode(nodeBeforeCursor);
            }
            return true;
        }
        return false;
    };
    PickerPlugin.prototype.getWord = function (event) {
        var wordFromRange = this.getRangeUntilAt(event).toString();
        var wordFromCache = this.getWordBeforeCursor(event);
        // VSO 24891: In picker, trigger and mention are separated into two nodes.
        // In this case, wordFromRange is the trigger character while wordFromCache is the whole string,
        // so wordFromCache is what we want to return.
        if (wordFromRange == this.pickerOptions.triggerCharacter &&
            wordFromRange != wordFromCache) {
            return wordFromCache;
        }
        return wordFromRange;
    };
    PickerPlugin.prototype.setRangeStart = function (rangeNode, node, target) {
        var nodeOffset = node ? node.textContent.lastIndexOf(target) : -1;
        if (nodeOffset > -1) {
            rangeNode.setStart(node, nodeOffset);
            return true;
        }
        return false;
    };
    PickerPlugin.prototype.setAriaOwns = function (isSuggesting) {
        this.editor.setEditorDomAttribute('aria-owns', isSuggesting && this.pickerOptions.suggestionsLabel
            ? this.pickerOptions.suggestionsLabel
            : null);
    };
    PickerPlugin.prototype.setAriaActiveDescendant = function (selectedIndex) {
        this.editor.setEditorDomAttribute('aria-activedescendant', selectedIndex != null && this.pickerOptions.suggestionLabelPrefix
            ? this.pickerOptions.suggestionLabelPrefix + selectedIndex.toString()
            : null);
    };
    PickerPlugin.prototype.getInlineElementBeforeCursor = function (event) {
        var searcher = this.editor.getContentSearcherOfCursor(event);
        var element = searcher ? searcher.getInlineElementBefore() : null;
        return element ? element.getTextContent() : null;
    };
    PickerPlugin.prototype.isAndroidKeyboardEvent = function (event) {
        // Check keyboard events on Android for further handling.
        // On Android Webview later 51, the KeyboardEvent's key is "Unidentified".
        // On Android Webview below 51, the KeyboardEvent's key is not supported and always returns undefined,
        // so using the charCode property, which is 0 or 229.
        return (event.rawEvent.key == UNIDENTIFIED_KEY ||
            (event.rawEvent.key == undefined &&
                UNIDENTIFIED_CODE.indexOf(event.rawEvent.charCode) > -1));
    };
    return PickerPlugin;
}());
exports.default = PickerPlugin;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Picker/index.ts":
/*!***********************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Picker/index.ts ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var PickerPlugin_1 = __webpack_require__(/*! ./PickerPlugin */ "./packages/roosterjs-editor-plugins/lib/plugins/Picker/PickerPlugin.ts");
exports.PickerPlugin = PickerPlugin_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/TableResize/TableResize.ts":
/*!**********************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/TableResize/TableResize.ts ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var INSERTER_COLOR = '#4A4A4A';
var INSERTER_COLOR_DARK_MODE = 'white';
var INSERTER_SIDE_LENGTH = 12;
var INSERTER_BORDER_SIZE = 1;
var INSERTER_HOVER_OFFSET = 5;
var MIN_CELL_WIDTH = 30;
var MIN_CELL_HEIGHT = 20;
var CELL_RESIZER_WIDTH = 4;
var TABLE_RESIZER_LENGTH = 12;
var HORIZONTAL_RESIZER_HTML = '<div style="position: fixed; cursor: row-resize; user-select: none"></div>';
var VERTICAL_RESIZER_HTML = '<div style="position: fixed; cursor: col-resize; user-select: none"></div>';
var TABLE_RESIZER_HTML_LTR = '<div style="position: fixed; cursor: nw-resize; user-select: none; border: 1px solid #808080"></div>';
var TABLE_RESIZER_HTML_RTL = '<div style="position: fixed; cursor: ne-resize; user-select: none; border: 1px solid #808080""></div>';
/**
 * TableResize plugin, provides the ability to resize a table by drag-and-drop
 */
var TableResize = /** @class */ (function () {
    function TableResize() {
        var _this = this;
        this.tableRectMap = null;
        this.currentCellsToResize = [];
        this.nextCellsToResize = [];
        this.resizingState = 0 /* None */;
        this.insertingState = 0 /* None */;
        this.onMouseMove = function (e) {
            var _a;
            if (_this.resizingState != 0 /* None */) {
                return;
            }
            if (!_this.tableRectMap) {
                _this.cacheRects();
            }
            if (_this.tableRectMap) {
                _this.setCurrentTable(null);
                var i = _this.tableRectMap.length - 1;
                while (i >= 0) {
                    var _b = _this.tableRectMap[i], table = _b.table, rect = _b.rect;
                    if (e.pageX <=
                        rect.right + (_this.isRTL ? INSERTER_SIDE_LENGTH : TABLE_RESIZER_LENGTH) &&
                        e.pageX >=
                            rect.left - (_this.isRTL ? TABLE_RESIZER_LENGTH : INSERTER_SIDE_LENGTH) &&
                        e.pageY >= rect.top - INSERTER_SIDE_LENGTH &&
                        e.pageY <= rect.bottom + TABLE_RESIZER_LENGTH) {
                        _this.setCurrentTable(table);
                        break;
                    }
                    i--;
                }
                if (_this.currentTable) {
                    var map = _this.tableRectMap.filter(function (map) { return map.table == _this.currentTable; })[0];
                    _this.setTableResizer(map.rect);
                    for (var i_1 = 0; i_1 < _this.currentTable.rows.length; i_1++) {
                        var tr = _this.currentTable.rows[i_1];
                        var j = 0;
                        for (; j < tr.cells.length; j++) {
                            var td = tr.cells[j];
                            var tdRect = roosterjs_editor_dom_1.normalizeRect(td.getBoundingClientRect());
                            if (tdRect &&
                                (_this.isRTL ? e.pageX >= tdRect.left : e.pageX <= tdRect.right) &&
                                e.pageY <= tdRect.bottom) {
                                // check vertical inserter
                                if (i_1 == 0 && e.pageY <= tdRect.top + INSERTER_HOVER_OFFSET) {
                                    var verticalInserterTd = null;
                                    // set inserter at current td
                                    if (_this.isRTL
                                        ? e.pageX <=
                                            tdRect.left + (tdRect.right - tdRect.left) / 2.0
                                        : e.pageX >=
                                            tdRect.left + (tdRect.right - tdRect.left) / 2.0) {
                                        verticalInserterTd = td;
                                    }
                                    else if (_this.isRTL ? e.pageX <= tdRect.right : e.pageX >= tdRect.left) {
                                        // set inserter at previous td if it exists
                                        var preTd = td.previousElementSibling;
                                        if (preTd) {
                                            verticalInserterTd = preTd;
                                        }
                                    }
                                    if (verticalInserterTd) {
                                        _this.setCurrentTd(null);
                                        // we hide the inserter if left mouse button is pressed
                                        if (e.buttons == 0) {
                                            _this.setCurrentInsertTd(2 /* Vertical */, verticalInserterTd, map.rect);
                                        }
                                        break;
                                    }
                                    // check horizontal inserter
                                }
                                else if (j == 0 &&
                                    (_this.isRTL
                                        ? e.pageX >= tdRect.right - INSERTER_HOVER_OFFSET
                                        : e.pageX <= tdRect.left + INSERTER_HOVER_OFFSET)) {
                                    var horizontalInserterTd = null;
                                    // set inserter at current td
                                    if (e.pageY >= tdRect.top + (tdRect.bottom - tdRect.top) / 2.0) {
                                        horizontalInserterTd = td;
                                    }
                                    else if (e.pageY >= tdRect.top) {
                                        // set insert at previous td if it exists
                                        var preTd = (_a = _this.currentTable.rows[i_1 - 1]) === null || _a === void 0 ? void 0 : _a.cells[0];
                                        if (preTd) {
                                            horizontalInserterTd = preTd;
                                        }
                                    }
                                    if (horizontalInserterTd) {
                                        _this.setCurrentTd(null);
                                        // we hide the inserter if left mouse button is pressed
                                        if (e.buttons == 0) {
                                            _this.setCurrentInsertTd(1 /* Horizontal */, horizontalInserterTd, map.rect);
                                        }
                                        break;
                                    }
                                }
                                else {
                                    _this.setCurrentTd(td, map.rect, _this.isRTL ? tdRect.left : tdRect.right, tdRect.bottom);
                                    _this.setCurrentInsertTd(0 /* None */);
                                    break;
                                }
                            }
                        }
                        if (j < tr.cells.length) {
                            break;
                        }
                    }
                }
                else {
                    _this.setTableResizer(null);
                }
            }
        };
        this.insertTd = function () {
            _this.editor.addUndoSnapshot(function (start, end) {
                var vtable = new roosterjs_editor_dom_1.VTable(_this.currentInsertTd);
                vtable.edit(_this.insertingState == 1 /* Horizontal */
                    ? 1 /* InsertBelow */
                    : 3 /* InsertRight */);
                vtable.writeBack();
                _this.editor.select(start, end);
                _this.setCurrentInsertTd(0 /* None */);
            }, "Format" /* Format */);
        };
        this.startResizingTable = function (e) {
            if (_this.currentTable == null) {
                return;
            }
            _this.resizingState = 3 /* Both */;
            var rect = roosterjs_editor_dom_1.normalizeRect(_this.currentTable.getBoundingClientRect());
            if (_this.isRTL) {
                _this.currentTable.setAttribute('currentLeftBorder', rect.left.toString());
            }
            else {
                _this.currentTable.setAttribute('currentRightBorder', rect.right.toString());
            }
            _this.currentTable.setAttribute('currentBottomBorder', rect.bottom.toString());
            _this.startResizeCells(e);
        };
        this.startHorizontalResizeCells = function (e) {
            _this.resizingState = 1 /* Horizontal */;
            _this.startResizeCells(e);
        };
        this.startVerticalResizeCells = function (e) {
            _this.resizingState = 2 /* Vertical */;
            var vtable = new roosterjs_editor_dom_1.VTable(_this.currentTd);
            if (vtable) {
                var rect = roosterjs_editor_dom_1.normalizeRect(_this.currentTd.getBoundingClientRect());
                // calculate and retrieve the cells of the two columns shared by the current vertical resizer
                _this.currentCellsToResize = vtable.getCellsWithBorder(_this.isRTL ? rect.left : rect.right, !_this.isRTL);
                _this.nextCellsToResize = vtable.getCellsWithBorder(_this.isRTL ? rect.left : rect.right, _this.isRTL);
            }
            _this.startResizeCells(e);
        };
        this.frameAnimateResizeCells = function (e) {
            _this.editor.runAsync(function () { return _this.resizeCells(e); });
        };
        this.resizeCells = function (e) {
            _this.setTableResizer(null);
            if (_this.resizingState === 0 /* None */) {
                return;
            }
            else if (_this.resizingState === 3 /* Both */) {
                var rect = roosterjs_editor_dom_1.normalizeRect(_this.currentTable.getBoundingClientRect());
                var vtable = new roosterjs_editor_dom_1.VTable(_this.currentTable);
                var currentBorder = parseFloat(_this.currentTable.getAttribute(_this.isRTL ? 'currentLeftBorder' : 'currentRightBorder'));
                var tableBottomBorder = parseFloat(_this.currentTable.getAttribute('currentBottomBorder'));
                var ratioX = 1.0 +
                    (_this.isRTL
                        ? (currentBorder - e.pageX) / (rect.right - currentBorder)
                        : (e.pageX - currentBorder) / (currentBorder - rect.left));
                var ratioY = 1.0 + (e.pageY - tableBottomBorder) / (tableBottomBorder - rect.top);
                var shouldResizeX = Math.abs(ratioX - 1.0) > 1e-3;
                var shouldResizeY = Math.abs(ratioY - 1.0) > 1e-3;
                if (shouldResizeX || shouldResizeY) {
                    for (var i = 0; i < vtable.cells.length; i++) {
                        for (var j = 0; j < vtable.cells[i].length; j++) {
                            var cell = vtable.cells[i][j];
                            if (cell.td) {
                                if (shouldResizeX) {
                                    var originalWidth = cell.td.style.width
                                        ? parseFloat(cell.td.style.width.substr(0, cell.td.style.width.length - 2))
                                        : cell.td.getBoundingClientRect().right -
                                            cell.td.getBoundingClientRect().left;
                                    var newWidth = originalWidth * ratioX;
                                    cell.td.style.boxSizing = 'border-box';
                                    if (newWidth >= MIN_CELL_WIDTH) {
                                        cell.td.style.wordBreak = 'break-word';
                                        cell.td.style.width = newWidth + "px";
                                    }
                                }
                                if (shouldResizeY) {
                                    if (j == 0) {
                                        var originalHeight = cell.td.getBoundingClientRect().bottom -
                                            cell.td.getBoundingClientRect().top;
                                        var newHeight = originalHeight * ratioY;
                                        if (newHeight >= MIN_CELL_HEIGHT) {
                                            cell.td.style.height = newHeight + "px";
                                        }
                                    }
                                    else {
                                        cell.td.style.height = '';
                                    }
                                }
                            }
                        }
                    }
                }
                rect = roosterjs_editor_dom_1.normalizeRect(_this.currentTable.getBoundingClientRect());
                currentBorder = _this.isRTL ? rect.left : rect.right;
                _this.currentTable.setAttribute(_this.isRTL ? 'currentLeftBorder' : 'currentRightBorder', currentBorder.toString());
                var currentBottomBorder = _this.currentTable.getBoundingClientRect().bottom;
                _this.currentTable.setAttribute('currentBottomBorder', currentBottomBorder.toString());
                vtable.writeBack();
                return;
            }
            else if (_this.currentTd) {
                var rect_1 = roosterjs_editor_dom_1.normalizeRect(_this.currentTd.getBoundingClientRect());
                if (rect_1) {
                    var newPos_1 = _this.resizingState == 1 /* Horizontal */ ? e.pageY : e.pageX;
                    var vtable = new roosterjs_editor_dom_1.VTable(_this.currentTd);
                    if (_this.resizingState == 1 /* Horizontal */) {
                        vtable.table.style.height = null;
                        vtable.forEachCellOfCurrentRow(function (cell) {
                            if (cell.td) {
                                cell.td.style.height =
                                    cell.td == _this.currentTd ? newPos_1 - rect_1.top + "px" : null;
                            }
                        });
                    }
                    else {
                        var leftBoundary = void 0;
                        var rightBoundary = void 0;
                        if (_this.isRTL) {
                            leftBoundary =
                                _this.nextCellsToResize.length > 0
                                    ? parseInt(_this.nextCellsToResize[0].getAttribute('originalLeftBorder'))
                                    : 0;
                            rightBoundary = parseInt(_this.currentCellsToResize[0].getAttribute('originalRightBorder'));
                        }
                        else {
                            leftBoundary = parseInt(_this.currentCellsToResize[0].getAttribute('originalLeftBorder'));
                            rightBoundary =
                                _this.nextCellsToResize.length > 0
                                    ? parseInt(_this.nextCellsToResize[0].getAttribute('originalRightBorder'))
                                    : Number.MAX_SAFE_INTEGER;
                        }
                        if (e.shiftKey) {
                            if ((!_this.isRTL && newPos_1 <= leftBoundary + MIN_CELL_WIDTH) ||
                                (_this.isRTL && newPos_1 >= rightBoundary - MIN_CELL_WIDTH)) {
                                return;
                            }
                        }
                        else if (newPos_1 <= leftBoundary + MIN_CELL_WIDTH ||
                            newPos_1 >= rightBoundary - MIN_CELL_WIDTH) {
                            return;
                        }
                        _this.currentCellsToResize.forEach(function (td) {
                            var rect = roosterjs_editor_dom_1.normalizeRect(td.getBoundingClientRect());
                            td.style.wordBreak = 'break-word';
                            td.style.boxSizing = 'border-box';
                            td.style.width = _this.isRTL
                                ? rect.right - newPos_1 + "px"
                                : newPos_1 - rect.left + "px";
                        });
                        if (!e.shiftKey) {
                            _this.nextCellsToResize.forEach(function (td) {
                                td.style.wordBreak = 'break-word';
                                var tdWidth = _this.isRTL
                                    ? newPos_1 - parseInt(td.getAttribute('originalLeftBorder'))
                                    : parseInt(td.getAttribute('originalRightBorder')) - newPos_1;
                                td.style.boxSizing = 'border-box';
                                td.style.width = tdWidth + "px";
                            });
                        }
                    }
                    vtable.writeBack();
                }
            }
        };
        this.endResizeCells = function (e) {
            var doc = _this.editor.getDocument();
            doc.removeEventListener('mousemove', _this.frameAnimateResizeCells, true);
            doc.removeEventListener('mouseup', _this.endResizeCells, true);
            _this.currentCellsToResize = [];
            _this.nextCellsToResize = [];
            _this.editor.addUndoSnapshot(function (start, end) {
                _this.frameAnimateResizeCells(e);
                _this.editor.select(start, end);
            }, "Format" /* Format */);
            _this.setCurrentTd(null);
            _this.setTableResizer(null);
            _this.resizingState = 0 /* None */;
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    TableResize.prototype.getName = function () {
        return 'TableResize';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    TableResize.prototype.initialize = function (editor) {
        this.editor = editor;
        this.setupResizerContainer();
        this.onMouseMoveDisposer = this.editor.addDomEventHandler('mousemove', this.onMouseMove);
    };
    /**
     * Dispose this plugin
     */
    TableResize.prototype.dispose = function () {
        this.onMouseMoveDisposer();
        this.tableRectMap = null;
        this.removeResizerContainer();
        this.setCurrentTable(null);
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    TableResize.prototype.onPluginEvent = function (e) {
        switch (e.eventType) {
            case 3 /* Input */:
            case 7 /* ContentChanged */:
            case 14 /* Scroll */:
                this.tableRectMap = null;
                break;
        }
    };
    TableResize.prototype.setupResizerContainer = function () {
        var document = this.editor.getDocument();
        this.resizerContainer = document.createElement('div');
        this.editor.insertNode(this.resizerContainer, {
            updateCursor: false,
            insertOnNewLine: false,
            replaceSelection: false,
            position: 4 /* Outside */,
        });
        this.tableResizerContainer = document.createElement('div');
        this.editor.insertNode(this.tableResizerContainer, {
            updateCursor: false,
            insertOnNewLine: false,
            replaceSelection: false,
            position: 4 /* Outside */,
        });
    };
    TableResize.prototype.removeResizerContainer = function () {
        var _a, _b, _c, _d;
        (_b = (_a = this.resizerContainer) === null || _a === void 0 ? void 0 : _a.parentNode) === null || _b === void 0 ? void 0 : _b.removeChild(this.resizerContainer);
        this.resizerContainer = null;
        (_d = (_c = this.tableResizerContainer) === null || _c === void 0 ? void 0 : _c.parentNode) === null || _d === void 0 ? void 0 : _d.removeChild(this.tableResizerContainer);
        this.tableResizerContainer = null;
    };
    TableResize.prototype.setCurrentInsertTd = function (insertingState, td, tableRect) {
        var _a, _b;
        if (td != this.currentInsertTd || insertingState != this.insertingState) {
            if (this.currentInsertTd) {
                (_b = (_a = this.inserter) === null || _a === void 0 ? void 0 : _a.parentNode) === null || _b === void 0 ? void 0 : _b.removeChild(this.inserter);
                this.inserter = null;
            }
            this.insertingState = insertingState;
            this.currentInsertTd = td;
            if (this.currentInsertTd) {
                this.inserter = this.createInserter(tableRect);
                this.resizerContainer.appendChild(this.inserter);
            }
        }
    };
    TableResize.prototype.createInserter = function (tableRect) {
        if (this.insertingState == 0 /* None */) {
            return;
        }
        var rect = roosterjs_editor_dom_1.normalizeRect(this.currentInsertTd.getBoundingClientRect());
        var editorBackgroundColor = this.editor.getDefaultFormat().backgroundColor;
        var inserterBackgroundColor = editorBackgroundColor || 'white';
        var inserterColor = this.editor.isDarkMode() ? INSERTER_COLOR_DARK_MODE : INSERTER_COLOR;
        var leftOrRight = this.isRTL ? 'right' : 'left';
        var HORIZONTAL_INSERTER_HTML = "<div style=\"position: fixed; width: " + INSERTER_SIDE_LENGTH + "px; height: " + INSERTER_SIDE_LENGTH + "px; font-size: 16px; color: " + inserterColor + "; line-height: 10px; vertical-align: middle; text-align: center; cursor: pointer; border: solid " + INSERTER_BORDER_SIZE + "px " + inserterColor + "; border-radius: 50%; background-color: " + inserterBackgroundColor + "\"><div style=\"position: absolute; " + leftOrRight + ": 12px; top: 5px; height: 3px; border-top: 1px solid " + inserterColor + "; border-bottom: 1px solid " + inserterColor + "; border-right: 1px solid " + inserterColor + "; border-left: 0px; box-sizing: border-box; background-color: " + inserterBackgroundColor + ";\"></div>+</div>";
        var VERTICAL_INSERTER_HTML = "<div style=\"position: fixed; width: " + INSERTER_SIDE_LENGTH + "px; height: " + INSERTER_SIDE_LENGTH + "px; font-size: 16px; color: " + inserterColor + "; line-height: 10px; vertical-align: middle; text-align: center; cursor: pointer; border: solid " + INSERTER_BORDER_SIZE + "px " + inserterColor + "; border-radius: 50%; background-color: " + inserterBackgroundColor + "\"><div style=\"position: absolute; left: 5px; top: 12px; width: 3px; border-left: 1px solid " + inserterColor + "; border-right: 1px solid " + inserterColor + "; border-bottom: 1px solid " + inserterColor + "; border-top: 0px; box-sizing: border-box; background-color: " + inserterBackgroundColor + ";\"></div>+</div>";
        var inserter = roosterjs_editor_dom_1.fromHtml(this.insertingState == 1 /* Horizontal */
            ? HORIZONTAL_INSERTER_HTML
            : VERTICAL_INSERTER_HTML, this.editor.getDocument())[0];
        if (rect) {
            if (this.insertingState == 1 /* Horizontal */) {
                if (this.isRTL) {
                    inserter.style.left = rect.right + "px";
                }
                else {
                    inserter.style.left = rect.left - (INSERTER_SIDE_LENGTH - 1 + 2 * INSERTER_BORDER_SIZE) + "px";
                }
                inserter.style.top = rect.bottom - 8 + "px";
                inserter.firstChild.style.width = tableRect.right - tableRect.left + "px";
            }
            else {
                if (this.isRTL) {
                    inserter.style.left = rect.left - 8 + "px";
                }
                else {
                    inserter.style.left = rect.right - 8 + "px";
                }
                inserter.style.top = rect.top - (INSERTER_SIDE_LENGTH - 1 + 2 * INSERTER_BORDER_SIZE) + "px";
                inserter.firstChild.style.height = tableRect.bottom - tableRect.top + "px";
            }
        }
        inserter.addEventListener('click', this.insertTd);
        return inserter;
    };
    TableResize.prototype.setCurrentTable = function (table) {
        if (this.currentTable != table) {
            this.setCurrentTd(null);
            this.setCurrentInsertTd(0 /* None */);
            this.currentTable = table;
        }
    };
    TableResize.prototype.setCurrentTd = function (td, tableRect, resizerPosX, bottom) {
        var _a, _b, _c, _d;
        if (this.currentTd != td) {
            if (this.currentTd) {
                (_b = (_a = this.horizontalResizer) === null || _a === void 0 ? void 0 : _a.parentNode) === null || _b === void 0 ? void 0 : _b.removeChild(this.horizontalResizer);
                (_d = (_c = this.verticalResizer) === null || _c === void 0 ? void 0 : _c.parentNode) === null || _d === void 0 ? void 0 : _d.removeChild(this.verticalResizer);
                this.horizontalResizer = null;
                this.verticalResizer = null;
            }
            this.currentTd = td;
            if (this.currentTd) {
                this.horizontalResizer = this.createCellsResizer(true /*horizontal*/, tableRect.left, bottom - CELL_RESIZER_WIDTH + 1, tableRect.right - tableRect.left, CELL_RESIZER_WIDTH);
                this.verticalResizer = this.createCellsResizer(false /*horizontal*/, resizerPosX - CELL_RESIZER_WIDTH + 1, tableRect.top, CELL_RESIZER_WIDTH, tableRect.bottom - tableRect.top);
                this.resizerContainer.appendChild(this.horizontalResizer);
                this.resizerContainer.appendChild(this.verticalResizer);
            }
        }
    };
    TableResize.prototype.setTableResizer = function (rect) {
        var _a;
        // remove old one if exists
        while ((_a = this.tableResizerContainer) === null || _a === void 0 ? void 0 : _a.hasChildNodes()) {
            this.tableResizerContainer.removeChild(this.tableResizerContainer.lastChild);
        }
        this.tableResizer = null;
        // add new one if exists
        if (rect) {
            this.tableResizer = this.createTableResizer(rect);
            this.tableResizerContainer.appendChild(this.tableResizer);
        }
    };
    TableResize.prototype.createTableResizer = function (rect) {
        var div = roosterjs_editor_dom_1.fromHtml(this.isRTL ? TABLE_RESIZER_HTML_RTL : TABLE_RESIZER_HTML_LTR, this.editor.getDocument())[0];
        div.style.top = rect.bottom + "px";
        div.style.left = this.isRTL
            ? rect.left - TABLE_RESIZER_LENGTH - 2 + "px"
            : rect.right + "px";
        div.style.width = TABLE_RESIZER_LENGTH + "px";
        div.style.height = TABLE_RESIZER_LENGTH + "px";
        div.addEventListener('mousedown', this.startResizingTable);
        return div;
    };
    TableResize.prototype.createCellsResizer = function (horizontal, left, top, width, height) {
        var div = roosterjs_editor_dom_1.fromHtml(horizontal ? HORIZONTAL_RESIZER_HTML : VERTICAL_RESIZER_HTML, this.editor.getDocument())[0];
        div.style.top = top + "px";
        div.style.left = left + "px";
        div.style.width = width + "px";
        div.style.height = height + "px";
        div.addEventListener('mousedown', horizontal ? this.startHorizontalResizeCells : this.startVerticalResizeCells);
        return div;
    };
    TableResize.prototype.startResizeCells = function (e) {
        var doc = this.editor.getDocument();
        doc.addEventListener('mousemove', this.frameAnimateResizeCells, true);
        doc.addEventListener('mouseup', this.endResizeCells, true);
    };
    TableResize.prototype.cacheRects = function () {
        var _this = this;
        this.tableRectMap = [];
        this.editor.queryElements('table', function (table) {
            if (table.isContentEditable) {
                var rect = roosterjs_editor_dom_1.normalizeRect(table.getBoundingClientRect());
                if (rect) {
                    _this.tableRectMap.push({
                        table: table,
                        rect: rect,
                    });
                }
            }
        });
        this.isRTL = roosterjs_editor_dom_1.getComputedStyle(this.editor.getDocument().body, 'direction') == 'rtl';
    };
    return TableResize;
}());
exports.default = TableResize;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/TableResize/index.ts":
/*!****************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/TableResize/index.ts ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var TableResize_1 = __webpack_require__(/*! ./TableResize */ "./packages/roosterjs-editor-plugins/lib/plugins/TableResize/TableResize.ts");
exports.TableResize = TableResize_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Watermark/Watermark.ts":
/*!******************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Watermark/Watermark.ts ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var roosterjs_editor_dom_1 = __webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts");
var roosterjs_editor_api_1 = __webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts");
var ENTITY_TYPE = 'WATERMARK_WRAPPER';
/**
 * A watermark plugin to manage watermark string for roosterjs
 */
var Watermark = /** @class */ (function () {
    /**
     * Create an instance of Watermark plugin
     * @param watermark The watermark string
     */
    function Watermark(watermark, format) {
        var _this = this;
        this.watermark = watermark;
        this.format = format;
        this.showHideWatermark = function () {
            var hasFocus = _this.editor.hasFocus();
            var watermarks = _this.editor.queryElements(roosterjs_editor_dom_1.getEntitySelector(ENTITY_TYPE));
            var isShowing = watermarks.length > 0;
            if (hasFocus && isShowing) {
                watermarks.forEach(_this.removeWatermark);
                _this.editor.focus();
            }
            else if (!hasFocus && !isShowing && _this.editor.isEmpty()) {
                roosterjs_editor_api_1.insertEntity(_this.editor, ENTITY_TYPE, _this.editor.getDocument().createTextNode(_this.watermark), false /*isBlock*/, false /*isReadonly*/, 0 /* Begin */);
            }
        };
        this.removeWatermark = function (wrapper) {
            var parentNode = wrapper.parentNode;
            parentNode === null || parentNode === void 0 ? void 0 : parentNode.removeChild(wrapper);
            // After remove watermark node, if it leaves an empty DIV, append a BR node into it to make it a regular empty line
            if (_this.editor.contains(parentNode) &&
                roosterjs_editor_dom_1.getTagOfNode(parentNode) == 'DIV' &&
                !parentNode.firstChild) {
                parentNode.appendChild(_this.editor.getDocument().createElement('BR'));
            }
        };
        this.format = this.format || {
            fontSize: '14px',
            textColor: '#aaa',
        };
    }
    /**
     * Get a friendly name of  this plugin
     */
    Watermark.prototype.getName = function () {
        return 'Watermark';
    };
    /**
     * Initialize this plugin. This should only be called from Editor
     * @param editor Editor instance
     */
    Watermark.prototype.initialize = function (editor) {
        this.editor = editor;
        this.disposer = this.editor.addDomEventHandler({
            focus: this.showHideWatermark,
            blur: this.showHideWatermark,
        });
    };
    /**
     * Dispose this plugin
     */
    Watermark.prototype.dispose = function () {
        this.disposer();
        this.disposer = null;
        this.editor = null;
    };
    /**
     * Handle events triggered from editor
     * @param event PluginEvent object
     */
    Watermark.prototype.onPluginEvent = function (event) {
        var _a;
        if (event.eventType == 11 /* EditorReady */ ||
            (event.eventType == 7 /* ContentChanged */ &&
                ((_a = event.data) === null || _a === void 0 ? void 0 : _a.type) != ENTITY_TYPE)) {
            this.showHideWatermark();
        }
        else if (event.eventType == 15 /* EntityOperation */ &&
            event.entity.type == ENTITY_TYPE) {
            var operation = event.operation, wrapper = event.entity.wrapper;
            if (operation == 8 /* ReplaceTemporaryContent */) {
                this.removeWatermark(wrapper);
            }
            else if (event.operation == 0 /* NewEntity */) {
                roosterjs_editor_dom_1.applyFormat(wrapper, this.format, this.editor.isDarkMode());
                wrapper.spellcheck = false;
            }
        }
    };
    return Watermark;
}());
exports.default = Watermark;


/***/ }),

/***/ "./packages/roosterjs-editor-plugins/lib/plugins/Watermark/index.ts":
/*!**************************************************************************!*\
  !*** ./packages/roosterjs-editor-plugins/lib/plugins/Watermark/index.ts ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var Watermark_1 = __webpack_require__(/*! ./Watermark */ "./packages/roosterjs-editor-plugins/lib/plugins/Watermark/Watermark.ts");
exports.Watermark = Watermark_1.default;


/***/ }),

/***/ "./packages/roosterjs-editor-types/lib/index.ts":
/*!******************************************************!*\
  !*** ./packages/roosterjs-editor-types/lib/index.ts ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });


/***/ }),

/***/ "./packages/roosterjs/lib/createEditor.ts":
/*!************************************************!*\
  !*** ./packages/roosterjs/lib/createEditor.ts ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ContentEdit_1 = __webpack_require__(/*! roosterjs-editor-plugins/lib/ContentEdit */ "./packages/roosterjs-editor-plugins/lib/ContentEdit.ts");
var roosterjs_editor_core_1 = __webpack_require__(/*! roosterjs-editor-core */ "./packages/roosterjs-editor-core/lib/index.ts");
var HyperLink_1 = __webpack_require__(/*! roosterjs-editor-plugins/lib/HyperLink */ "./packages/roosterjs-editor-plugins/lib/HyperLink.ts");
var Paste_1 = __webpack_require__(/*! roosterjs-editor-plugins/lib/Paste */ "./packages/roosterjs-editor-plugins/lib/Paste.ts");
/**
 * Create an editor instance with most common options
 * @param contentDiv The html div element needed for creating the editor
 * @param additionalPlugins The additional user defined plugins. Currently the default plugins that are already included are
 * ContentEdit, HyperLink and Paste, user don't need to add those.
 * @param initialContent The initial content to show in editor. It can't be removed by undo, user need to manually remove it if needed.
 * @returns The editor instance
 */
function createEditor(contentDiv, additionalPlugins, initialContent) {
    var plugins = [new HyperLink_1.HyperLink(), new Paste_1.Paste(), new ContentEdit_1.ContentEdit()];
    if (additionalPlugins) {
        plugins = plugins.concat(additionalPlugins);
    }
    var options = {
        plugins: plugins,
        initialContent: initialContent,
        defaultFormat: {
            fontFamily: 'Calibri,Arial,Helvetica,sans-serif',
            fontSize: '11pt',
            textColor: '#000000',
        },
    };
    return new roosterjs_editor_core_1.Editor(contentDiv, options);
}
exports.default = createEditor;


/***/ }),

/***/ "./packages/roosterjs/lib/index.ts":
/*!*****************************************!*\
  !*** ./packages/roosterjs/lib/index.ts ***!
  \*****************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
var createEditor_1 = __webpack_require__(/*! ./createEditor */ "./packages/roosterjs/lib/createEditor.ts");
exports.createEditor = createEditor_1.default;
__export(__webpack_require__(/*! roosterjs-editor-types */ "./packages/roosterjs-editor-types/lib/index.ts"));
__export(__webpack_require__(/*! roosterjs-editor-dom */ "./packages/roosterjs-editor-dom/lib/index.ts"));
__export(__webpack_require__(/*! roosterjs-editor-core */ "./packages/roosterjs-editor-core/lib/index.ts"));
__export(__webpack_require__(/*! roosterjs-editor-api */ "./packages/roosterjs-editor-api/lib/index.ts"));
__export(__webpack_require__(/*! roosterjs-editor-plugins */ "./packages/roosterjs-editor-plugins/lib/index.ts"));


/***/ })

/******/ });
//# sourceMappingURL=rooster.js.map
var roosterEditors = [];
var roosterActiveDiv = null;

var SamplePlugin = (function () {

    function SamplePlugin() {
    }

    //function
    SamplePlugin.prototype.getName = function () {
        return 'SamplePlugin';
    };

    SamplePlugin.prototype.initialize = function (editor) {
        this.editor = editor;
    };

    SamplePlugin.prototype.dispose = function () {
        this.editor = null;
    };

    SamplePlugin.prototype.onPluginEvent = function (event) {
        // Check if the event is BeforePasteEvent
        if (event.eventType == 10 /*PluginEventType.BeforePaste*/) {
            let beforePasteEvent = /*BeforePasteEvent*/ event;
            // Check if pasting image
            if (beforePasteEvent.clipboardData.image != null) {
                let image = beforePasteEvent.clipboardData.image;
                let placeholder = this.createPlaceholder(image);

                // Modify the pasting content and option
                let originalImage = beforePasteEvent.fragment.children[0];
                let container = document.createElement("div");
                container.appendChild(originalImage);
                beforePasteEvent.fragment.appendChild(container);
                container.appendChild(placeholder);
                beforePasteEvent.clipboardData.html = placeholder.outerHTML;
                //beforePasteEvent.clipboardData.image = null;
                beforePasteEvent.pasteOption = 0 /*PasteOption.PasteHtml*/;

                // Start upload image and handle async result
                DotNet.invokeMethodAsync("ExtraDry.Blazor", "UploadImage", beforePasteEvent.clipboardData.imageDataUri).then((blob) => {
                    // Check editor availability in async callback
                    if(this.editor) {
                        originalImage.src = blob.url;
                        placeholder.remove();
                    }
                });
            }
        }
    }

    SamplePlugin.prototype.createPlaceholder = function(img) {
        var paragraph = document.createElement("P");
        paragraph.style = "color: white;";
        paragraph.innerHTML = "Uploading...";
        return paragraph;
    }

    return SamplePlugin;
}());

function startEditing(name) {
    //var roosterjs = require('roosterjs');
    var editorDiv = document.getElementById(name);
    var editor = roosterjs.createEditor(editorDiv, new SamplePlugin());

    editor.dryId = name;

    editorDiv.roosterEditor = editor;
    editorDiv.addEventListener("focus", roosterEditorFocus);

    roosterEditors.push(editor);
}

function roosterEditorFocus(focusArgs) {
    var editorDiv = focusArgs.target;
    if(roosterActiveDiv) {
        roosterActiveDiv.classList.remove("rooster-selected");
        roosterActiveDiv.parentNode.classList.remove("rooster-selected");
        roosterActiveDiv.parentNode.parentNode.classList.remove("rooster-selected");
    }
    roosterActiveDiv = editorDiv;
    if(roosterActiveDiv) {
        roosterActiveDiv.classList.add("rooster-selected");
        roosterActiveDiv.parentNode.classList.add("rooster-selected");
        roosterActiveDiv.parentNode.parentNode.classList.add("rooster-selected");
    }
}

function roosterToggleBold() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleBold(editor);
    }
}

function roosterToggleItalic() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleItalic(editor);
    }
}

function roosterToggleHeader(level) {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.toggleHeader(editor, level);
    }
}

function roosterClearFormat() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        roosterjs.clearFormat(editor);
    }
}

function roosterSetContent(id, html) {
    var div = document.getElementById(id);
    if(div) {
        div.innerHTML = html;
    }
}

function roosterHorizontalRule() {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        editor.insertContent("<hr />");
    }
}

function roosterInsertHyperlink(className, title, hyperlink) {
    var editor = roosterActiveDiv.roosterEditor;
    if(editor) {
        editor.insertContent(`<a class=''${className}'' href=''${hyperlink}''>${title}</a>`);
    }
}

function roosterGetContent(id) {
    var div = document.getElementById(id);
    return div.innerHTML;
}

//// Adapted from https://github.com/microsoft/roosterjs/blob/cfe4f3515833480b66f4c0214f93bc337410bddb/packages/roosterjs-editor-api/lib/format/toggleHeader.ts
//function roosterTestSanitize(editor) {
//    var editor = roosterActiveDiv.roosterEditor;

//    // Don't allow undo to de-sanitized state...
//    editor.focus();

//    //let wrapped = false;
//    //editor.queryElements('*', 0 /* QueryScope.Body */, header => {
//    //    if (!wrapped) {
//    //        editor.getDocument().execCommand("formatBlock" /* DocumentCommand.FormatBlock */, false, '<DIV>');
//    //        wrapped = true;
//    //    }

//    //    let div = editor.getDocument().createElement('div');
//    //    while (header.firstChild) {
//    //        div.appendChild(header.firstChild);
//    //    }
//    //    editor.replaceNode(header, div);
//    //});

//    let traverser = editor.getBodyTraverser();
//    let blockElement = traverser ? traverser.currentBlockElement : null;
//    let sanitizer = new roosterjs.HtmlSanitizer({
//        cssStyleCallbacks: {
//            'font-size': () => { console.log("font-size Callback"); return false; },
//            'display': () => { console.log("display Callback"); return false; },
//            'color': () => { console.log("color Callback"); return false; },
//        },
//    });
//    while (blockElement) {
//        console.log(blockElement);
//        let element = blockElement.collapseToSingleElement();

//        blockElement.innerHTML = blockElement.getTextContent();
//        sanitizer.sanitize(element);
//        blockElement = traverser.getNextBlockElement();
//    }
//}

console.log(`Blazor Extra Dry by @aakison - https://github.com/fmi-works/blazor-extra-dry License - https://github.com/fmi-works/blazor-extra-dry/blob/main/LICENSE (MIT License)`);
window.addEventListener('resize', DryHorizontalScrollNav);

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
    var li = document.querySelector("nav li.active");
    if (li == null) {
        // Nothing is active, typically when first loaded and script is called before navigation rendered.
        return;
    }
    var ul = li.parentElement;
    var nav = ul.parentElement;

    if (nav.offsetWidth >= ul.offsetWidth) {
        ul.style.transform = "translateX(0px)";
    }
    else {
        var firstLi = ul.firstElementChild;
        var lastLi = ul.lastElementChild;

        var firstScreenPosition = firstLi.offsetLeft;
        var lastScreenPosition = nav.clientWidth - lastLi.clientWidth;

        var percent = ilerp(li.offsetLeft, firstLi.offsetLeft, lastLi.offsetLeft);
        var position = lerp(percent, firstScreenPosition, lastScreenPosition);

        var offset = position - li.offsetLeft;

        ul.style.transform = `translateX(${offset}px)`;
    }
}

function extraDry_setIndeterminate(id, value) {
    console.log("setIndeterminate", id, value);
    var checkbox = document.getElementById(id);
    if (checkbox != null) {
        checkbox.indeterminate = value;
    }
}
