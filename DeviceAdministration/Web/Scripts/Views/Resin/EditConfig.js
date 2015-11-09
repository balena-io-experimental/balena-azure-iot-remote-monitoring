IoTApp.createModule('IoTApp.EditResinConfig', (function () {
    "use strict";

    var self = this;

    var init = function () {
        self.backButton = $(".header_main__button_back");
        self.backButton.off("click").click(onBackButtonClicked);
    }

    var onBackButtonClicked = function () {
        location.href = resources.redirectToIndexUrl;
    }

    var onSuccess = function () {
        location.href = resources.redirectToIndexUrl;
    }

    var onFailure = function (data) {
        $('content').html(data);
        IoTApp.Helpers.Dialog.displayError(resources.errorSendingConfig);
    }
    return {
        init: init,
        onSuccess: onSuccess,
        onFailure: onFailure
    }
}), [jQuery, resources]);

$(function () {
    "use strict";

    IoTApp.EditResinConfig.init();
});