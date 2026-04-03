window.onload = function() {
    // 默认折叠所有模型
    setTimeout(function() {
        // 折叠所有模型
        const schemaWrapperElements = document.querySelectorAll('.opblock-tag');
        schemaWrapperElements.forEach(function(element) {
            if (!element.classList.contains('is-open')) {
                return;
            }
            const titleElement = element.querySelector('.opblock-tag-section > h3 > button');
            if (titleElement) {
                titleElement.click();
            }
        });
    }, 100);
}; 