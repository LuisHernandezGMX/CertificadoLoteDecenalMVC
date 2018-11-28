// [SpanishDTUrl] => URL hacia el archivo de lenguaje español para DataTable.
(function (SpanishDTUrl) {
    $("table").DataTable({
        scrollX: true,
        order: [[1, "desc"]],
        language: {
            url: SpanishDTUrl
        }
    });


})(SPANISH_DT_URL);

var DetailsUtils = (function () {
    function mostrarResultado(html, alertStyle) {
        var styleOptions;

        if (alertStyle === "Success") {
            styleOptions = {
                type: BootstrapDialog.TYPE_SUCCESS,
                size: BootstrapDialog.SIZE_NORMAL,
                cssClass: "btn-success"
            };
        } else if (alertStyle === "Info") {
            styleOptions = {
                type: BootstrapDialog.TYPE_INFO,
                size: BootstrapDialog.SIZE_NORMAL,
                cssClass: "btn-info"
            };
        } else if (alertStyle === "Warning") {
            styleOptions = {
                type: BootstrapDialog.TYPE_WARNING,
                size: BootstrapDialog.SIZE_NORMAL,
                cssClass: "btn-warning"
            };
        } else {
            styleOptions = {
                type: BootstrapDialog.TYPE_DANGER,
                size: BootstrapDialog.SIZE_WIDE,
                cssClass: "btn-danger"
            };
        }

        BootstrapDialog.show({
            title: "Resultado del Evento",
            type: styleOptions.type,
            size: styleOptions.size,
            message: html,
            buttons: [{
                label: "Cerrar",
                cssClass: styleOptions.cssClass,
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    }

    function mostrarUbicacion(html) {
        BootstrapDialog.show({
            title: '<span class="fas fa-code"></span> Ubicación del Evento',
            type: BootstrapDialog.TYPE_PRIMARY,
            message: html,
            buttons: [{
                label: "Cerrar",
                cssClass: "btn-primary",
                action: function (dialog) {
                    dialog.close();
                }
            }]
        });
    }

    return {
        mostrarResultado: mostrarResultado,
        mostrarUbicacion: mostrarUbicacion
    };
})();