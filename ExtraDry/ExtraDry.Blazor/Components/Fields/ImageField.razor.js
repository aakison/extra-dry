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
