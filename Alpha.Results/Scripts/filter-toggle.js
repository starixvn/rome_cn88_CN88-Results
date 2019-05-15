"use strict";
var ToggleHandlerOption = /** @class */ (function () {
    function ToggleHandlerOption(toggleIconId, sourceId, targetId, hideSourceElement) {
        if (toggleIconId === void 0) { toggleIconId = "toggleIconId"; }
        if (sourceId === void 0) { sourceId = "sourceId"; }
        if (targetId === void 0) { targetId = "targetId"; }
        if (hideSourceElement === void 0) { hideSourceElement = true; }
        this.toggleIconId = toggleIconId;
        this.sourceId = sourceId;
        this.targetId = targetId;
        this.hideSourceElement = hideSourceElement;
    }
    return ToggleHandlerOption;
}());
var ToggleHandler = /** @class */ (function () {
    function ToggleHandler($) {
        var _this = this;
        this.options = new ToggleHandlerOption();
        this.render = function (options) {
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$filterToggleIcon = _this.$("#" + _this.options.toggleIconId);
            _this.$sourceElement = _this.$("#" + _this.options.sourceId);
            _this.$targetElement = _this.$("#" + _this.options.targetId);
            if (_this.options.hideSourceElement) {
                _this.$sourceElement.hide();
            }
            _this.$filterToggleIcon.on("click touchend", function (e) {
                e.preventDefault();
                var isCollapsed = _this.$filterToggleIcon.hasClass("icon-filter-down");
                if (isCollapsed) {
                    _this.$sourceElement
                        .slideDown(200);
                    _this.$targetElement
                        .slideUp(210);
                    _this.$filterToggleIcon
                        .removeClass("icon-filter-down")
                        .addClass("icon-filter-up");
                }
                else {
                    _this.$sourceElement.slideUp(200);
                    _this.$targetElement
                        .slideDown(210);
                    _this.$filterToggleIcon
                        .removeClass("icon-filter-up")
                        .addClass("icon-filter-down");
                }
            });
        };
        this.$ = $;
    }
    return ToggleHandler;
}());
//# sourceMappingURL=filter-toggle.js.map