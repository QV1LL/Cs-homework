window.scrollToBottom = function (element) {
    if (!element || !(element instanceof HTMLElement)) return;
    element.scrollTop = element.scrollHeight;
};

window.attachScrollListener = function (element, dotNetHelper, threshold) {
    if (!element || !(element instanceof HTMLElement)) return;
    const handleScroll = function () {
        if (element.scrollTop <= threshold)
            dotNetHelper.invokeMethodAsync('OnScrolledToTop');
    };
    element.addEventListener('scroll', handleScroll);
    element._scrollHandler = handleScroll;
};

window.detachScrollListener = function (element) {
    if (!element || !element._scrollHandler) return;
    element.removeEventListener('scroll', element._scrollHandler);
    delete element._scrollHandler;
};

window.isNearBottom = function (element, threshold) {
    if (!element || !(element instanceof HTMLElement)) return false;
    return (element.scrollHeight - element.scrollTop - element.clientHeight) <= threshold;
};

window.saveScrollState = function (element) {
    if (!element || !(element instanceof HTMLElement)) return;
    element._oldScrollTop = element.scrollTop;
    element._oldScrollHeight = element.scrollHeight;
};

window.restoreScrollAfterChange = function (element) {
    if (!element || !(element instanceof HTMLElement) || element._oldScrollTop === undefined) return;
    const delta = element.scrollHeight - element._oldScrollHeight;
    element.scrollTop = element._oldScrollTop + delta;
    delete element._oldScrollTop;
    delete element._oldScrollHeight;
};
