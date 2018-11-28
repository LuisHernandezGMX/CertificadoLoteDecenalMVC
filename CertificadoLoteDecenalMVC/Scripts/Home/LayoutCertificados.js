(function () {
    $("#formPrevisualizar").submit(function () {
        // Se agrega el valor del FileInput antes de enviar al servidor
        $("#layoutCertificados")
            .css("display", "none")
            .appendTo($(this));

        // Deshabilita los botones, cambia el color de la barra de progreso y muestra un mensaje de progreso.
        $('button').prop("disabled", true);
        $("#progressBar").addClass("progress-bar-success");
        $("#progressBar").children("span").text("Previsualizando. Un momento por favor...");
        $(".alert").remove();
        $(".progress").css("display", "");
    });

    $("#formSubir").submit(function () {
        // Deshabilita los botones y muestra un mensaje de progreso.
        $('button').prop("disabled", true);
        $("#progressBar").children("span").text("Subiendo archivo. Un momento por favor...");
        $(".alert").remove();
        $(".progress").css("display", "");
    });
})();