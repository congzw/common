require(['jquery', 'toastr'], function ($, toastr) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-full-width",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "-1",
        "extendedTimeOut": "-1",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        closeMethod: false
    };

    var toastMessage = function (msg, group) {
        toastr.info(msg, group);
    };

    $(function () {

        var asyncScripts = "";
        var syncScripts = "";

        $("script").each(function (index, element) {
            if ($(element).attr('async')) {
                asyncScripts += "<p>" + this.src + "</p>";
            } else {
                if (this.src) {
                    syncScripts += "<p>" + this.src + "</p>";
                } else {
                    syncScripts += "<p> inline script: " + this.id + "</p>";
                }
            }
        });
        toastMessage(syncScripts, 'sync load scripts:');
        toastMessage(asyncScripts, 'async load scripts:');
    });
});